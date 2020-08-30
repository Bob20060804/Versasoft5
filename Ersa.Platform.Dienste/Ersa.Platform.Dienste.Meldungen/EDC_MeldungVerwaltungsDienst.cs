using Ersa.Global.Common;
using Ersa.Global.Common.Extensions;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.CapabilityContracts.Meldungen;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using Ersa.Platform.DataContracts.Meldungen;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Meldungen.Interfaces;
using Ersa.Platform.Dienste.ZyklischeMeldungen;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ersa.Platform.Dienste.Meldungen
{
	/// <summary>
	/// 消息服务管理者
	///  message administration service
	/// </summary>
	[Export(typeof(INF_MeldungVerwaltungsDienst))]
	public class EDC_MeldungVerwaltungsDienst : INF_MeldungVerwaltungsDienst
	{
		private static readonly SemaphoreSlim ms_fdcSemaphore = new SemaphoreSlim(1);

		private readonly INF_BenutzerInfoProvider m_edcBenutzerInfoProvider;

		private readonly EDC_ZyklischeMeldungVorlageCache m_edcVorlageCache;

		private readonly Lazy<INF_MeldungenDataAccess> m_edcMeldungenDataAccess;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		private readonly Lazy<INF_ProzessAktionCapability> m_edcProzessMeldungenCapability;

		private readonly Lazy<IEnumerable<INF_MeldungProduzentCapability>> m_edcModulMeldungen;

		private readonly Dictionary<object, Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task>> m_dicWeitergabeActionen = new Dictionary<object, Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task>>();

		private DispatcherTimer m_fdcOfflineTimer;

		public EDC_SmartObservableCollection<INF_Meldung> PRO_lstNichtQuittierteMeldungen
		{
			get;
			set;
		}

		[ImportMany]
		public IEnumerable<INF_MeldungProduzentCapability> PRO_lstMeldungProduzenten
		{
			get;
			private set;
		}

		[ImportingConstructor]
		public EDC_MeldungVerwaltungsDienst(INF_BenutzerInfoProvider i_edcBenutzerInfoProvider, EDC_ZyklischeMeldungVorlageCache i_edcVorlageCache, INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
		{
			PRO_lstNichtQuittierteMeldungen = new EDC_SmartObservableCollection<INF_Meldung>();
			m_edcBenutzerInfoProvider = i_edcBenutzerInfoProvider;
			m_edcVorlageCache = i_edcVorlageCache;
			m_edcMeldungenDataAccess = new Lazy<INF_MeldungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MeldungenDataAccess>);
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			m_edcProzessMeldungenCapability = new Lazy<INF_ProzessAktionCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_ProzessAktionCapability>);
			m_edcModulMeldungen = new Lazy<IEnumerable<INF_MeldungProduzentCapability>>(i_edcCapabilityProvider.FUN_edcMehrfachCapabilityListeHolen<INF_MeldungProduzentCapability>);
		}

		public IDisposable FUN_fdcWeitergabeAktionRegistrieren(object i_objSender, Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task> i_delAction)
		{
			if (!m_dicWeitergabeActionen.ContainsKey(i_objSender))
			{
				m_dicWeitergabeActionen.Add(i_objSender, i_delAction);
			}
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				m_dicWeitergabeActionen.Remove(i_objSender);
			});
		}

		/// <summary>
		/// 处理消息异步
		/// </summary>
		/// <param name="i_enuMeldungen"></param>
		/// <param name="i_enmAktion"></param>
		/// <returns></returns>
		public async Task FUN_fdcMeldungenBehandelnAsync(IEnumerable<INF_Meldung> i_enuMeldungen, ENUM_MeldungAktionen i_enmAktion)
		{
			try
			{
				await ms_fdcSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<INF_Meldung> lstMeldungen = i_enuMeldungen.ToList();
				switch (i_enmAktion)
				{
				case ENUM_MeldungAktionen.Erstellen:
					await FUN_fdcMeldungenErstellenAsync(lstMeldungen).ConfigureAwait( false);
					break;
				case ENUM_MeldungAktionen.Quittieren:
					await FUN_fdcMeldungenQuittierenAsync(lstMeldungen).ConfigureAwait( false);
					break;
				case ENUM_MeldungAktionen.Zurueckstellen:
					await FUN_fdcMeldungenZurueckstellenAsync(lstMeldungen).ConfigureAwait( false);
					break;
				case ENUM_MeldungAktionen.Loggen:
					await FUN_fdcMeldungenLoggenAsync(lstMeldungen).ConfigureAwait( false);
					break;
				}
				await FUN_fdcMeldungenWeitergebenAsync(lstMeldungen, i_enmAktion, i_blnRefresh: false).ConfigureAwait( false);
			}
			finally
			{
				ms_fdcSemaphore.Release();
			}
		}

		public async Task FUN_fdcMeldungenInitialisierenAsync()
		{
			if (m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstOffline())
			{
				SUB_StarteOfflineAktualisierungTimer();
				await FUN_edcAktualisiereOfflineModusAsync(null, null).ConfigureAwait(continueOnCapturedContext: true);
				return;
			}
			await FUN_fdcNichtQuittierteMeldungenValidierenAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<INF_Meldung> list = (await FUN_fdcNichtQuittierteMeldungenLadenAsync(ENUM_MeldungProduzent.Alle).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			IEnumerable<ENUM_MeldungProduzent> lstProduzenten = FUN_enuVorhandeneMeldungsProduzentenErmitteln();
			list.RemoveAll((INF_Meldung i_item) => !lstProduzenten.Contains(i_item.PRO_enmMeldungProduzent));
			await FUN_fdcMeldungenWeitergebenAsync(list, ENUM_MeldungAktionen.Erstellen, i_blnRefresh: true).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcMeldungBehandelnAnfordernAsync(IEnumerable<INF_Meldung> i_enuMeldungen, ENUM_MeldungAktionen i_enmAktion)
		{
			List<INF_Meldung> list = i_enuMeldungen.ToList();
			if (list.Any())
			{
				foreach (INF_Meldung item in list)
				{
					await FUN_edcHoleMeldungsCapability(item.PRO_enmMeldungProduzent).FUN_fdcMeldungBehandelnAnfordernAsync(item, i_enmAktion).ConfigureAwait(continueOnCapturedContext: true);
				}
			}
		}

		public async Task<IEnumerable<INF_Meldung>> FUN_fdcAlleMeldungenImZeitraumLadenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, DateTime i_sttVon, DateTime i_sttBis)
		{
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return FUN_enuMeldungenMappen(await m_edcMeldungenDataAccess.Value.FUN_fdcAlleMeldungenImIntervallAsync(i_enmMeldungProduzent, i_enmMeldungsTyp, i_i64MaschinenId, i_sttVon, i_sttBis).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<INF_Meldung>> FUN_fdcQuittierteMeldungenImZeitraumLadenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, DateTime i_sttVon, DateTime i_sttBis)
		{
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return FUN_enuMeldungenMappen(await m_edcMeldungenDataAccess.Value.FUN_fdcQuittierteMeldungenImIntervallAsync(i_enmMeldungProduzent, i_enmMeldungsTyp, i_i64MaschinenId, i_sttVon, i_sttBis).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<INF_Meldung>> FUN_fdcNichtQuittierteMeldungenLadenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent)
		{
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return FUN_enuMeldungenMappen(await m_edcMeldungenDataAccess.Value.FUN_fdcAlleNichtQuittiertenMeldungenAsync(i_enmMeldungProduzent, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task FUN_fdcNichtQuittierteMeldungenRefreshenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent)
		{
			List<INF_Meldung> lstNichtQuittierteMeldungen = FUN_lstAktuelleNichtQuittierteMeldungenListe(i_enmMeldungProduzent).ToList();
			PRO_lstNichtQuittierteMeldungen.RemoveRange(lstNichtQuittierteMeldungen);
			List<INF_Meldung> list = (await FUN_fdcNichtQuittierteMeldungenLadenAsync(i_enmMeldungProduzent).ConfigureAwait(continueOnCapturedContext: true)).ToList();
			if (list.Any())
			{
				PRO_lstNichtQuittierteMeldungen.AddRange(list);
			}
			await FUN_fdcProzessAktionenBeendenAsync(lstNichtQuittierteMeldungen.Except(list)).ConfigureAwait(continueOnCapturedContext: false);
		}

		public IEnumerable<INF_Meldung> FUN_lstAktuelleNichtQuittierteMeldungenListe(ENUM_MeldungProduzent i_enmMeldungProduzent)
		{
			return from i_edcMeldung in PRO_lstNichtQuittierteMeldungen
			where i_edcMeldung.PRO_enmMeldungProduzent == i_enmMeldungProduzent
			select i_edcMeldung;
		}

		public IEnumerable<ENUM_MeldungProduzent> FUN_enuVorhandeneMeldungsProduzentenErmitteln()
		{
			return from i_edcProduzent in FUN_enmErstelleMeldungProduzentenListe()
			select i_edcProduzent.PRO_enmMeldungProduzent;
		}

		public bool FUN_blnIstMeldungBereitsVorhanden(ENUM_MeldungProduzent i_enmMeldungProduzent, int i_i32ProduzentenCode, int i_i32MeldungsNummer, int i_i32MeldungsOrt1, int i_i32MeldungsOrt2, int i_i32MeldungsOrt3)
		{
			return PRO_lstNichtQuittierteMeldungen.Where(delegate(INF_Meldung i_edcItem)
			{
				if (i_edcItem.PRO_enmMeldungProduzent == i_enmMeldungProduzent && i_edcItem.PRO_i32ProduzentenCode == i_i32ProduzentenCode && i_edcItem.PRO_i32MeldungsNummer == i_i32MeldungsNummer && i_edcItem.PRO_i32MeldungsOrt1 == i_i32MeldungsOrt1 && i_edcItem.PRO_i32MeldungsOrt2 == i_i32MeldungsOrt2)
				{
					return i_edcItem.PRO_i32MeldungsOrt3 == i_i32MeldungsOrt3;
				}
				return false;
			}).Any();
		}

		private async Task FUN_fdcMeldungenWeitergebenAsync(List<INF_Meldung> i_lstMeldungen, ENUM_MeldungAktionen i_enmAktion, bool i_blnRefresh)
		{
			List<Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task>> list = m_dicWeitergabeActionen.Values.ToList();
			foreach (Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task> item in list)
			{
				await item(i_lstMeldungen, i_enmAktion, i_blnRefresh).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private async Task FUN_fdcNichtQuittierteMeldungenValidierenAsync()
		{
			List<INF_MeldungProduzentCapability> list = FUN_enmErstelleMeldungProduzentenListe().ToList();
			foreach (INF_MeldungProduzentCapability edcProduzent in list)
			{
				List<INF_Meldung> list2 = (await edcProduzent.FUN_fdcErmittleZuQuittierendeMeldungenAsync(await FUN_fdcNichtQuittierteMeldungenLadenAsync(edcProduzent.PRO_enmMeldungProduzent).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				if (list2.Any())
				{
					await FUN_fdcMeldungenBehandelnAsync(list2, ENUM_MeldungAktionen.Quittieren).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		private async Task FUN_fdcMeldungenErstellenAsync(IEnumerable<INF_Meldung> i_enuMeldungen)
		{
			List<INF_Meldung> lstAlleMeldungen = i_enuMeldungen.ToList();
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> lstZyklischeMeldungen = FUN_enuZyklischeMeldungTuples(lstAlleMeldungen, i_i64MaschinenId).ToList();
			List<EDC_MeldungData> lstMeldungen = FUN_lstMeldungenZuDataOhneZyklischeMappen(lstAlleMeldungen, i_i64MaschinenId).ToList();
			IDbTransaction fdcTransaktion = await m_edcMeldungenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (lstMeldungen.Any())
				{
					await m_edcMeldungenDataAccess.Value.FUN_fdcMeldungenHinzufuegenAsync(lstMeldungen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					List<EDC_MeldungContextData> list = FUN_lstMeldungenContextMappen(lstAlleMeldungen).ToList();
					if (list.Any())
					{
						await m_edcMeldungenDataAccess.Value.FUN_fdcMeldungenContextHinzufuegenAsync(list, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				if (lstZyklischeMeldungen.Any())
				{
					await m_edcMeldungenDataAccess.Value.FUN_fdcZyklischeMeldungenHinzufuegenAsync(lstZyklischeMeldungen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_edcMeldungenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMeldungenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
			foreach (INF_Meldung item in lstAlleMeldungen)
			{
				if (item.PRO_enuProzessAktionen.Any() && m_edcProzessMeldungenCapability.Value != null)
				{
					await m_edcProzessMeldungenCapability.Value.FUN_fdcProzessAktionAnfordernAsync(item).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		private async Task FUN_fdcMeldungenQuittierenAsync(IEnumerable<INF_Meldung> i_enuMeldungen)
		{
			List<INF_Meldung> lstMeldungen = i_enuMeldungen.ToList();
			if (lstMeldungen.Any())
			{
				long i64BenutzerId = m_edcBenutzerInfoProvider.PRO_i64BenutzerId;
				long i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				IDbTransaction fdcTransaktion = await m_edcMeldungenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					foreach (INF_Meldung item in lstMeldungen)
					{
						if (!item.PRO_sttQuittiert.HasValue)
						{
							item.PRO_sttQuittiert = DateTime.Now;
						}
					}
					await m_edcMeldungenDataAccess.Value.FUN_fdcMeldungenAktualisierenAsync(FUN_lstMeldungenZuDataMappen(lstMeldungen, i64BenutzerId, i64MaschinenId), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await FUN_fdcProzessAktionenBeendenAsync(lstMeldungen).ConfigureAwait(continueOnCapturedContext: false);
					m_edcMeldungenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
				}
				catch (Exception)
				{
					m_edcMeldungenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
					throw;
				}
			}
		}

		private async Task FUN_fdcMeldungenZurueckstellenAsync(IEnumerable<INF_Meldung> i_enuMeldungen)
		{
			List<INF_Meldung> lstMeldungen = i_enuMeldungen.ToList();
			long i64BenutzerId = m_edcBenutzerInfoProvider.PRO_i64BenutzerId;
			long i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcMeldungenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (INF_Meldung item in lstMeldungen)
				{
					if (!item.PRO_sttZurueckgestellt.HasValue)
					{
						item.PRO_sttZurueckgestellt = DateTime.Now;
					}
					if (!item.PRO_sttQuittiert.HasValue)
					{
						item.PRO_sttQuittiert = item.PRO_sttZurueckgestellt;
					}
				}
				await m_edcMeldungenDataAccess.Value.FUN_fdcMeldungenAktualisierenAsync(FUN_lstMeldungenZuDataMappen(lstMeldungen, i64BenutzerId, i64MaschinenId), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcProzessAktionenBeendenAsync(lstMeldungen).ConfigureAwait(continueOnCapturedContext: false);
				m_edcMeldungenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMeldungenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcMeldungenLoggenAsync(IEnumerable<INF_Meldung> i_enuMeldungen)
		{
			List<INF_Meldung> lstAlleMeldungen = i_enuMeldungen.ToList();
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> lstZyklischeMeldungen = FUN_enuZyklischeMeldungTuples(lstAlleMeldungen, i_i64MaschinenId).ToList();
			List<EDC_MeldungData> lstMeldungen = FUN_lstMeldungenZuDataOhneZyklischeMappen(lstAlleMeldungen, i_i64MaschinenId).ToList();
			IDbTransaction fdcTransaktion = await m_edcMeldungenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (lstMeldungen.Any())
				{
					await m_edcMeldungenDataAccess.Value.FUN_fdcMeldungenHinzufuegenAsync(lstMeldungen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (lstZyklischeMeldungen.Any())
				{
					await m_edcMeldungenDataAccess.Value.FUN_fdcZyklischeMeldungenHinzufuegenAsync(lstZyklischeMeldungen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_edcMeldungenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMeldungenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcProzessAktionenBeendenAsync(IEnumerable<INF_Meldung> i_enuMeldungen)
		{
			List<INF_Meldung> list = i_enuMeldungen.ToList();
			foreach (INF_Meldung edcMeldung in list)
			{
				if (edcMeldung.PRO_enuProzessAktionen.Any())
				{
					List<INF_Meldung> list2 = PRO_lstNichtQuittierteMeldungen.Where(delegate(INF_Meldung i_edcItem)
					{
						if (i_edcItem.PRO_enuProzessAktionen.Any())
						{
							return !i_edcItem.PRO_strMeldungGuid.Equals(edcMeldung.PRO_strMeldungGuid);
						}
						return false;
					}).ToList();
					List<ENUM_ProzessAktionen> list3 = new List<ENUM_ProzessAktionen>();
					foreach (INF_Meldung item in list2)
					{
						list3 = list3.Union(item.PRO_enuProzessAktionen).ToList();
					}
					List<ENUM_ProzessAktionen> list4 = edcMeldung.PRO_enuProzessAktionen.Except(list3).ToList();
					if (list4.Any())
					{
						await m_edcProzessMeldungenCapability.Value.FUN_fdcProzessAktionBeendenAsync(list4).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}
		}

		private IEnumerable<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> FUN_enuZyklischeMeldungTuples(IEnumerable<INF_Meldung> i_enuMeldungen, long i_i64MaschinenId)
		{
			List<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> list = new List<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>>();
			long pRO_i64BenutzerId = m_edcBenutzerInfoProvider.PRO_i64BenutzerId;
			foreach (INF_Meldung item4 in i_enuMeldungen)
			{
				if (item4.PRO_enmMeldungsTyp == ENUM_MeldungsTypen.enmZyklisch)
				{
					EDC_ZyklischeMeldungData item = FUN_edcZylischeMeldungMappen(item4);
					EDC_MeldungData item2 = FUN_edcMeldungZuDataMappen(item4, pRO_i64BenutzerId, i_i64MaschinenId);
					Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData> item3 = new Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>(item, item2);
					list.Add(item3);
				}
			}
			return list;
		}

		private EDC_ZyklischeMeldungData FUN_edcZylischeMeldungMappen(INF_Meldung i_edcMeldungen)
		{
			EDC_ZyklischeMeldungData eDC_ZyklischeMeldungData = new EDC_ZyklischeMeldungData();
			EDC_ZyklischeMeldungVorlage eDC_ZyklischeMeldungVorlage = m_edcVorlageCache.FUN_edcVorlageErmitteln(i_edcMeldungen.PRO_i32MeldungsNummer);
			string text2 = eDC_ZyklischeMeldungData.PRO_strMeldungsOrt1 = (i_edcMeldungen.PRO_strMeldeort1 = eDC_ZyklischeMeldungVorlage.PRO_strMeldeort1);
			text2 = (eDC_ZyklischeMeldungData.PRO_strMeldungsOrt2 = (i_edcMeldungen.PRO_strMeldeort2 = eDC_ZyklischeMeldungVorlage.PRO_strMeldeort2));
			text2 = (eDC_ZyklischeMeldungData.PRO_strMeldungsOrt3 = (i_edcMeldungen.PRO_strMeldeort3 = eDC_ZyklischeMeldungVorlage.PRO_strMeldeort3));
			text2 = (eDC_ZyklischeMeldungData.PRO_strMeldung = (i_edcMeldungen.PRO_strMeldetext = eDC_ZyklischeMeldungVorlage.PRO_strMeldetext));
			eDC_ZyklischeMeldungData.PRO_blnEinlaufSperreAktiv = eDC_ZyklischeMeldungVorlage.PRO_blnEinlaufSperreAktiv;
			return eDC_ZyklischeMeldungData;
		}

		private IEnumerable<EDC_MeldungData> FUN_lstMeldungenZuDataOhneZyklischeMappen(IEnumerable<INF_Meldung> i_lstMeldungen, long i_i64MaschinenId)
		{
			List<EDC_MeldungData> list = new List<EDC_MeldungData>();
			long pRO_i64BenutzerId = m_edcBenutzerInfoProvider.PRO_i64BenutzerId;
			foreach (INF_Meldung item in i_lstMeldungen)
			{
				if (!ENUM_MeldungsTypen.enmZyklisch.Equals(item.PRO_enmMeldungsTyp))
				{
					list.Add(FUN_edcMeldungZuDataMappen(item, pRO_i64BenutzerId, i_i64MaschinenId));
				}
			}
			return list;
		}

		private IEnumerable<EDC_MeldungData> FUN_lstMeldungenZuDataMappen(IEnumerable<INF_Meldung> i_lstMeldungen, long i_i64BenutzerId, long i_i64MaschinenId)
		{
			List<EDC_MeldungData> list = new List<EDC_MeldungData>();
			string pRO_strAktiverBenutzer = m_edcBenutzerInfoProvider.PRO_strAktiverBenutzer;
			foreach (INF_Meldung item in i_lstMeldungen)
			{
				item.PRO_strBenutzerName = pRO_strAktiverBenutzer;
				list.Add(FUN_edcMeldungZuDataMappen(item, i_i64BenutzerId, i_i64MaschinenId));
			}
			return list;
		}

		private IEnumerable<EDC_MeldungContextData> FUN_lstMeldungenContextMappen(IEnumerable<INF_Meldung> i_lstMeldungen)
		{
			return (from edcMeldung in i_lstMeldungen.Where(delegate(INF_Meldung i_edcItem)
			{
				if (!string.IsNullOrEmpty(i_edcItem.PRO_strDetails))
				{
					return !string.IsNullOrEmpty(i_edcItem.PRO_strContext);
				}
				return false;
			})
			select new EDC_MeldungContextData
			{
				PRO_strMeldungGuid = edcMeldung.PRO_strMeldungGuid,
				PRO_strDetails = edcMeldung.PRO_strDetails,
				PRO_strContext = edcMeldung.PRO_strContext
			}).ToList();
		}

		private EDC_MeldungData FUN_edcMeldungZuDataMappen(INF_Meldung i_edcMeldung, long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return new EDC_MeldungData
			{
				PRO_strMeldungsId = i_edcMeldung.PRO_strMeldungGuid,
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_fdcAufgetreten = i_edcMeldung.PRO_sttAufgetreten,
				PRO_fdcQuittiert = i_edcMeldung.PRO_sttQuittiert,
				PRO_fdcZurueckgestellt = i_edcMeldung.PRO_sttZurueckgestellt,
				PRO_i64BenutzerId = i_i64BenutzerId,
				PRO_i32Betriebsart = i_edcMeldung.PRO_i32Betriebsart,
				PRO_enmMeldungsTyp = i_edcMeldung.PRO_enmMeldungsTyp,
				PRO_i32MeldungsNummer = i_edcMeldung.PRO_i32MeldungsNummer,
				PRO_i32MeldungsOrt1 = i_edcMeldung.PRO_i32MeldungsOrt1,
				PRO_i32MeldungsOrt2 = i_edcMeldung.PRO_i32MeldungsOrt2,
				PRO_i32MeldungsOrt3 = i_edcMeldung.PRO_i32MeldungsOrt3,
				PRO_enmMeldungProduzent = i_edcMeldung.PRO_enmMeldungProduzent,
				PRO_i32ProduzentenCode = i_edcMeldung.PRO_i32ProduzentenCode,
				PRO_enuProzessAktionen = i_edcMeldung.PRO_enuProzessAktionen,
				PRO_enuMoeglicheAktionen = i_edcMeldung.PRO_enuMoeglicheAktionen
			};
		}

		private INF_Meldung FUN_edcMeldungDataZuMeldungMappen(EDC_MeldungenAbfrageData i_edcMeldung)
		{
			return new EDC_Meldung
			{
				PRO_strMeldungGuid = i_edcMeldung.PRO_strMeldungsId,
				PRO_sttAufgetreten = i_edcMeldung.PRO_fdcAufgetreten,
				PRO_sttQuittiert = i_edcMeldung.PRO_fdcQuittiert,
				PRO_sttZurueckgestellt = i_edcMeldung.PRO_fdcZurueckgestellt,
				PRO_strBenutzerName = i_edcMeldung.PRO_strBenutzername,
				PRO_i32Betriebsart = i_edcMeldung.PRO_i32Betriebsart,
				PRO_enmMeldungsTyp = i_edcMeldung.PRO_enmMeldungsTyp,
				PRO_i32MeldungsNummer = i_edcMeldung.PRO_i32MeldungsNummer,
				PRO_i32MeldungsOrt1 = i_edcMeldung.PRO_i32MeldungsOrt1,
				PRO_i32MeldungsOrt2 = i_edcMeldung.PRO_i32MeldungsOrt2,
				PRO_i32MeldungsOrt3 = i_edcMeldung.PRO_i32MeldungsOrt3,
				PRO_enmMeldungProduzent = i_edcMeldung.PRO_enmMeldungProduzent,
				PRO_i32ProduzentenCode = i_edcMeldung.PRO_i32ProduzentenCode,
				PRO_enuProzessAktionen = i_edcMeldung.PRO_enuProzessAktionen,
				PRO_enuMoeglicheAktionen = i_edcMeldung.PRO_enuMoeglicheAktionen,
				PRO_strMeldetext = i_edcMeldung.PRO_strMeldung,
				PRO_strMeldeort1 = i_edcMeldung.PRO_strMeldungsOrt1,
				PRO_strMeldeort2 = i_edcMeldung.PRO_strMeldungsOrt2,
				PRO_strMeldeort3 = i_edcMeldung.PRO_strMeldungsOrt3,
				PRO_strDetails = i_edcMeldung.PRO_strDetails,
				PRO_strContext = i_edcMeldung.PRO_strContext
			};
		}

		private IEnumerable<INF_Meldung> FUN_enuMeldungenMappen(IEnumerable<EDC_MeldungenAbfrageData> i_enuMeldungen)
		{
			List<INF_Meldung> list = new List<INF_Meldung>();
			foreach (EDC_MeldungenAbfrageData item in i_enuMeldungen)
			{
				list.Add(FUN_edcMeldungDataZuMeldungMappen(item));
			}
			return list;
		}

		private INF_MeldungProduzentCapability FUN_edcHoleMeldungsCapability(ENUM_MeldungProduzent i_enmMeldungProduzent)
		{
			return FUN_enmErstelleMeldungProduzentenListe().FirstOrDefault((INF_MeldungProduzentCapability i_edcItem) => i_enmMeldungProduzent.Equals(i_edcItem.PRO_enmMeldungProduzent));
		}

		private IEnumerable<INF_MeldungProduzentCapability> FUN_enmErstelleMeldungProduzentenListe()
		{
			List<INF_MeldungProduzentCapability> list = (from i_item in m_edcModulMeldungen.Value
			where i_item.FUN_blnIstKonfiguriert()
			select i_item).ToList();
			if (PRO_lstMeldungProduzenten == null || !PRO_lstMeldungProduzenten.Any())
			{
				return list;
			}
			foreach (INF_MeldungProduzentCapability edcProduzent in PRO_lstMeldungProduzenten)
			{
				if (edcProduzent.FUN_blnIstKonfiguriert() && list.FirstOrDefault((INF_MeldungProduzentCapability i_edcProduzent) => i_edcProduzent.PRO_enmMeldungProduzent.Equals(edcProduzent.PRO_enmMeldungProduzent)) == null)
				{
					list.Add(edcProduzent);
				}
			}
			return list;
		}

		private void SUB_StarteOfflineAktualisierungTimer()
		{
			m_fdcOfflineTimer?.Stop();
			m_fdcOfflineTimer = new DispatcherTimer();
			m_fdcOfflineTimer.Tick += async delegate(object s, EventArgs e)
			{
				await FUN_edcAktualisiereOfflineModusAsync(s, e).ConfigureAwait(continueOnCapturedContext: true);
			};
			m_fdcOfflineTimer.Interval = TimeSpan.FromSeconds(5.0);
			m_fdcOfflineTimer.Start();
		}

		private void SUB_StoppeOfflineAktualisierungTimer()
		{
			m_fdcOfflineTimer?.Stop();
			m_fdcOfflineTimer = null;
		}

		private async Task FUN_edcAktualisiereOfflineModusAsync(object i_objSender, EventArgs i_objArgs)
		{
			if (!m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstOffline())
			{
				SUB_StoppeOfflineAktualisierungTimer();
				return;
			}
			List<INF_Meldung> lstAlleMeldungen = (await FUN_fdcNichtQuittierteMeldungenLadenAsync(ENUM_MeldungProduzent.Alle).ConfigureAwait(continueOnCapturedContext: true)).ToList();
			IEnumerable<ENUM_MeldungProduzent> lstProduzenten = FUN_enuVorhandeneMeldungsProduzentenErmitteln();
			lstAlleMeldungen.RemoveAll((INF_Meldung i_item) => !lstProduzenten.Contains(i_item.PRO_enmMeldungProduzent));
			foreach (INF_Meldung item in lstAlleMeldungen)
			{
				item.PRO_enuMoeglicheAktionen = new List<ENUM_MeldungAktionen>();
			}
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				PRO_lstNichtQuittierteMeldungen.SUB_Reset(lstAlleMeldungen);
			});
		}
	}
}
