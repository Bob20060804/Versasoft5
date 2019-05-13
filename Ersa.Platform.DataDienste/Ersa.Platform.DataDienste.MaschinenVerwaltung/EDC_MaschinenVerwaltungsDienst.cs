using Ersa.Global.Common.FortsetzungsPolicy;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.CapabilityContracts.BenutzerVerwaltung;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common;
using Ersa.Platform.Common.Data.Maschinenkonfiguration;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Exceptions;
using Ersa.Platform.DataContracts.Maschinenkonfiguration;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.MaschinenVerwaltung
{
	[Export(typeof(INF_MaschinenVerwaltungsDienst))]
	public class EDC_MaschinenVerwaltungsDienst : INF_MaschinenVerwaltungsDienst
	{
		private const int mC_i32MaxVersucheWiederholbareOperation = 5;

		private readonly INF_IODienst m_edcIoDienst;

		private readonly Lazy<INF_MaschinenDataAccess> m_edcMaschinenDataAccess;

		private readonly Lazy<INF_MaschinenkonfigurationDataAccess> m_edcMaschinenKonfigurationsDataAccess;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcBasisDatenCapability;

		private readonly Lazy<INF_MaschinenDatenSichernCapability> m_edcMaschinenDatenExportCapability;

		private readonly Lazy<INF_MaschinenDatenLadenCapability> m_edcMaschinenDatenImportCapability;

		private readonly Lazy<INF_BenutzerVerwaltungCapability> m_edcBenutzerVerwaltungCapability;

		private EDC_ParameterDatenKonfig m_edcDefaultDaten;

		private EDC_ParameterDatenKonfig m_edcAktuelleDaten;

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_MaschinenVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, INF_IODienst i_edcIoDienst)
		{
			m_edcIoDienst = i_edcIoDienst;
			m_edcMaschinenDataAccess = new Lazy<INF_MaschinenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenDataAccess>);
			m_edcMaschinenKonfigurationsDataAccess = new Lazy<INF_MaschinenkonfigurationDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenkonfigurationDataAccess>);
			m_edcBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			m_edcMaschinenDatenExportCapability = new Lazy<INF_MaschinenDatenSichernCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenDatenSichernCapability>);
			m_edcMaschinenDatenImportCapability = new Lazy<INF_MaschinenDatenLadenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenDatenLadenCapability>);
			m_edcBenutzerVerwaltungCapability = new Lazy<INF_BenutzerVerwaltungCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_BenutzerVerwaltungCapability>);
		}

		public Task<string> FUN_strMaschinenNummerErmittelnAsync()
		{
			return Task.FromResult(m_edcBasisDatenCapability.Value.FUN_strMaschinenNummerErmitteln());
		}

		public Task<string> FUN_strHoleDefaultGruppenNameAsync()
		{
			return Task.FromResult(EDC_MaschinenGruppeData.FUN_strHoleDefaultGruppenName(m_edcBasisDatenCapability.Value.FUN_strHoleMaschinenTyp()));
		}

		public async Task<long[]> FUNa_i64AktiveGruppenIdsErmittelnAsync()
		{
			long i_i64Maschinenid = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (await m_edcMaschinenDataAccess.Value.FUN_fdcHoleZugewieseneGruppenIdsAsync(i_i64Maschinenid).ConfigureAwait(continueOnCapturedContext: false)).ToArray();
		}

		public async Task<EDC_MaschineData> FUN_edcMaschinenDatenErmittelnAsync()
		{
			long i_i64MaschinenId = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenDataAccess.Value.FUN_fdcHoleMaschinenDataAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcMaschinenDatenSpeichernAsync(EDC_MaschineData i_edcMaschinenDaten)
		{
			long num = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (i_edcMaschinenDaten.PRO_i64MaschinenId != num)
			{
				throw new InvalidOperationException("Invalid machine id. Cannot store machine data.");
			}
			await m_edcMaschinenDataAccess.Value.FUN_fdcUpdateMaschineAsync(i_edcMaschinenDaten).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<bool> FUN_fdcSindMehrereGleicheMaschinentypenRegistriertAsync()
		{
			long i_i64MaschinenId = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenDataAccess.Value.FUN_fdcSindMehrereGleicheMaschinentypenRegistriertAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_enuGruppenFuerMaschinenTypErmittelnAsync()
		{
			string i_strMaschinenTyp = m_edcBasisDatenCapability.Value.FUN_strHoleMaschinenTyp();
			return m_edcMaschinenDataAccess.Value.FUN_enuHoleMaschinenGruppeDataAsync(i_strMaschinenTyp);
		}

		public Task<long> FUN_i64GruppeErstellenAsync(string i_strGruppenName)
		{
			if (string.IsNullOrEmpty(i_strGruppenName))
			{
				throw new ArgumentNullException("i_strGruppenName");
			}
			string i_strMaschinenTyp = m_edcBasisDatenCapability.Value.FUN_strHoleMaschinenTyp();
			return m_edcMaschinenDataAccess.Value.FUN_i64LegeMaschinenGruppeAnAsync(i_strGruppenName, i_strMaschinenTyp);
		}

		public async Task FUN_fdcGruppeUmbenennenAsync(long i_i64GruppenId, string i_strNeuerName)
		{
			IDbTransaction fdcTransaktion = await m_edcMaschinenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				EDC_MaschinenGruppeData eDC_MaschinenGruppeData = await m_edcMaschinenDataAccess.Value.FUN_fdcHoleMaschinenGruppeDataAsync(i_i64GruppenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (eDC_MaschinenGruppeData == null)
				{
					throw new ArgumentOutOfRangeException("i_i64GruppenId", $"There does not exist a machine group with the id {i_i64GruppenId}.");
				}
				eDC_MaschinenGruppeData.PRO_strGruppenName = i_strNeuerName;
				await m_edcMaschinenDataAccess.Value.FUN_fdcUpdateMaschinenGruppeAsync(eDC_MaschinenGruppeData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcMaschinenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMaschinenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcAktiveGruppenIdsSetzenAsync(long[] ia_i64GruppenIds)
		{
			IDbTransaction fdcTransaktion = await m_edcMaschinenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long i64MaschinenId = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<long> lstGespeicherteIds = (await m_edcMaschinenDataAccess.Value.FUN_fdcHoleZugewieseneGruppenIdsAsync(i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList();
				foreach (long item in lstGespeicherteIds.Except(ia_i64GruppenIds))
				{
					await m_edcMaschinenDataAccess.Value.FUN_fdcEntferneMaschineAusGruppeAsync(i64MaschinenId, item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				foreach (long item2 in ia_i64GruppenIds.Except(lstGespeicherteIds))
				{
					await m_edcMaschinenDataAccess.Value.FUN_fdcFuegeMaschineZuGruppeHinzuAsync(i64MaschinenId, item2, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_edcMaschinenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMaschinenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public IEnumerable<Func<Task>> FUN_enuMaschinenDatenSichernOperationenErstellen(string i_strPfad, long i_i64BenutzerId)
		{
			Func<Task>[] first = new Func<Task>[1]
			{
				async delegate
				{
					string i_strSerialisierterInhalt = await Task.Run(delegate
					{
						long i_i64KonfigurationsVersion = m_edcMaschinenDatenExportCapability.Value.FUN_i64MaschinenKonfigurationsVersionErmitteln();
						IEnumerable<EDC_ParameterDaten> first2 = m_edcMaschinenDatenExportCapability.Value.FUN_enuZuSicherndeKonfigParameterErmitteln();
						IEnumerable<EDC_ParameterDaten> second2 = m_edcMaschinenDatenExportCapability.Value.FUN_enuZuSicherndeMaschinenParameterErmitteln();
						return SUB_DatenExportieren(first2.Union(second2), i_strPfad, i_i64BenutzerId, i_i64KonfigurationsVersion);
					}).ConfigureAwait(continueOnCapturedContext: false);
					await FUN_fdcKonfigurationInDatenbankSpeichernAsync(Path.GetFileNameWithoutExtension(i_strPfad), "Export", i_strSerialisierterInhalt, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
				}
			};
			IEnumerable<Func<Task>> second = m_edcMaschinenDatenExportCapability.Value.FUN_enuMaschinenDateienSichernOperationenErstellen(Path.GetDirectoryName(i_strPfad));
			return first.Union(second);
		}

		public IEnumerable<Func<Task>> FUN_enuDefaultMaschinenDatenSichernOperationenErstellen(string i_strPfad, long i_i64BenutzerId)
		{
			yield return () => Task.Run(delegate
			{
				long i_i64KonfigurationsVersion = m_edcMaschinenDatenExportCapability.Value.FUN_i64MaschinenKonfigurationsVersionErmitteln();
				IEnumerable<EDC_ParameterDaten> i_enuDaten = m_edcMaschinenDatenExportCapability.Value.FUN_enuZuSicherndeMaschinenParameterErmitteln();
				SUB_DatenExportieren(i_enuDaten, i_strPfad, i_i64BenutzerId, i_i64KonfigurationsVersion);
			});
		}

		public IEnumerable<Func<Task>> FUN_enuMaschinenDatenLadenOperationenErstellen(string i_strPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcProgress = null)
		{
			yield return () => Task.Run(delegate
			{
				string i_strPfad2 = i_strPfad;
				string i_strPfad3 = Path.Combine(EDC_VerzeichnisHelfer.FUN_strDefaultKonfigVerzeichnisErmitteln(), m_edcBasisDatenCapability.Value.FUN_strDefaultMaschinenKonfigDateiNamenErmitteln());
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmLesenDefault,
					PRO_strStatusKey = "13_255"
				});
				string i_strInhalt = FUN_strDateiLesen(i_strPfad3);
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmLesen,
					PRO_strStatusKey = "11_1556"
				});
				string i_strInhalt2 = FUN_strDateiLesen(i_strPfad2);
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmInterpretierenDefault,
					PRO_strStatusKey = "13_256"
				});
				m_edcDefaultDaten = FUN_edcDeserialisieren(i_strInhalt);
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmInterpretieren,
					PRO_strStatusKey = "4_8012"
				});
				m_edcAktuelleDaten = FUN_edcDeserialisieren(i_strInhalt2);
			});
			yield return async delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmUebertragungAufSteuerungVorbereitenDefault,
					PRO_strStatusKey = "13_257"
				});
				SUB_KonfigurationAnAktuelleVersionAnpassen(m_edcDefaultDaten);
				SUB_AlteVariablenAufNeueMappen(m_edcDefaultDaten);
				await FUN_fdcDatenUebertragungVorbereitenAsync(m_edcDefaultDaten).ConfigureAwait(continueOnCapturedContext: false);
			};
			yield return async delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmUebertragungAufSteuerungVorbereiten,
					PRO_strStatusKey = "13_253"
				});
				SUB_KonfigurationAnAktuelleVersionAnpassen(m_edcAktuelleDaten);
				SUB_AlteVariablenAufNeueMappen(m_edcAktuelleDaten);
				await FUN_fdcDatenUebertragungVorbereitenAsync(m_edcAktuelleDaten).ConfigureAwait(continueOnCapturedContext: false);
			};
			yield return async delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmAufSteuerungUebertragenDefault,
					PRO_strStatusKey = "13_258"
				});
				await FUN_fdcDatenAufSteuerungUebertragenDefaultAsync().ConfigureAwait(continueOnCapturedContext: false);
			};
			yield return async delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmAufSteuerungUebertragen,
					PRO_strStatusKey = "13_254"
				});
				await FUN_fdcDatenAufSteuerungUebertragenAsync().ConfigureAwait(continueOnCapturedContext: false);
			};
			yield return async delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmMaschinenDatenImportAbschliessen,
					PRO_strStatusKey = "10_251"
				});
				await FUN_fdcMaschinenDatenImportAbschliessenAsync().ConfigureAwait(continueOnCapturedContext: false);
			};
			yield return delegate
			{
				i_fdcProgress?.Report(new STRUCT_MaschinenDatenLadenStatus
				{
					PRO_enmStatus = ENUM_MaschinenDatenLadenStatus.enmInDatenbankSpeichern,
					PRO_strStatusKey = "13_996"
				});
				string i_strSerialisierterInhalt = PRO_edcSerialisierer.FUN_strSerialisieren(m_edcAktuelleDaten);
				return FUN_fdcKonfigurationInDatenbankSpeichernAsync(Path.GetFileNameWithoutExtension(i_strPfad), "Import", i_strSerialisierterInhalt, m_edcBenutzerVerwaltungCapability.Value.FUN_i64AktuelleBenutzerIdHolen());
			};
			yield return FUN_fdcAufraeumenAsync;
		}

		public Task FUN_fdcMaschinenDatenLadenAsync(string i_strPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcProgress = null)
		{
			return FUN_fdcOperationenMitWiederholungAusfuehrenAsync(FUN_enuMaschinenDatenLadenOperationenErstellen(i_strPfad, i_fdcProgress));
		}

		private static async Task FUN_fdcOperationenMitWiederholungAusfuehrenAsync(IEnumerable<Func<Task>> i_enuOperationen)
		{
			EDC_FortsetzungsPolicy edcFortsetzungsPolicy = new EDC_FortsetzungsPolicy();
			if ((await edcFortsetzungsPolicy.FUN_fdcOperationenAusfuehrenAsync(i_enuOperationen).ConfigureAwait(continueOnCapturedContext: false)).PRO_blnErfolgreich)
			{
				return;
			}
			int i32Versuche = 0;
			STRUCT_OperationsErgebnis sTRUCT_OperationsErgebnis;
			do
			{
				sTRUCT_OperationsErgebnis = await edcFortsetzungsPolicy.FUN_fdcOperationWiederholenUndFortsetzenAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (sTRUCT_OperationsErgebnis.PRO_blnErfolgreich)
				{
					return;
				}
			}
			while (i32Versuche++ <= 5);
			throw sTRUCT_OperationsErgebnis.PRO_fdcException;
		}

		private string FUN_strDateiLesen(string i_strPfad)
		{
			return m_edcIoDienst.FUN_strDateiInhaltLesen(i_strPfad);
		}

		private EDC_ParameterDatenKonfig FUN_edcDeserialisieren(string i_strInhalt)
		{
			return PRO_edcSerialisierer.FUN_objDeserialisieren<EDC_ParameterDatenKonfig>(i_strInhalt);
		}

		private void SUB_KonfigurationAnAktuelleVersionAnpassen(EDC_ParameterDatenKonfig i_edcKonfiguration)
		{
			m_edcMaschinenDatenImportCapability.Value.SUB_KonfigurationAktualisieren(i_edcKonfiguration);
		}

		private void SUB_AlteVariablenAufNeueMappen(EDC_ParameterDatenKonfig i_edcDaten)
		{
			m_edcMaschinenDatenImportCapability.Value.SUB_AlteVariablenAufNeueMappen(i_edcDaten.PRO_enuParameter);
		}

		private async Task FUN_fdcDatenUebertragungVorbereitenAsync(EDC_ParameterDatenKonfig i_edcDaten)
		{
			try
			{
				await m_edcMaschinenDatenImportCapability.Value.FUN_fdcUebertragungAufSteuerungVorbereitenAsync(i_edcDaten.PRO_enuParameter).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (EDC_UnbekannterParameterException ex)
			{
				SUB_ParameterDatenBereinigen(i_edcDaten, ex.PRO_enuUnbekannteParameter);
				throw;
			}
		}

		private Task FUN_fdcDatenAufSteuerungUebertragenDefaultAsync()
		{
			return m_edcMaschinenDatenImportCapability.Value.FUN_fdcDatenAufSteuerungUebertragenAsync(m_edcDefaultDaten.PRO_enuParameter);
		}

		private Task FUN_fdcDatenAufSteuerungUebertragenAsync()
		{
			return m_edcMaschinenDatenImportCapability.Value.FUN_fdcDatenAufSteuerungUebertragenAsync(m_edcAktuelleDaten.PRO_enuParameter);
		}

		private Task FUN_fdcMaschinenDatenImportAbschliessenAsync()
		{
			return m_edcMaschinenDatenImportCapability.Value.FUN_fdcMaschinenDatenLadenAbschliessenAsync();
		}

		private Task FUN_fdcAufraeumenAsync()
		{
			m_edcDefaultDaten = null;
			m_edcAktuelleDaten = null;
			return Task.FromResult(0);
		}

		private string SUB_DatenExportieren(IEnumerable<EDC_ParameterDaten> i_enuDaten, string i_strPfad, long i_i64BenutzerId, long i_i64KonfigurationsVersion)
		{
			EDC_ParameterDatenKonfig i_objObjekt = new EDC_ParameterDatenKonfig
			{
				PRO_strVersion = i_i64KonfigurationsVersion.ToString(),
				PRO_strDatum = DateTime.Now.ToString(CultureInfo.InvariantCulture),
				PRO_i64BenutzerId = i_i64BenutzerId,
				PRO_enuParameter = i_enuDaten
			};
			string text = PRO_edcSerialisierer.FUN_strSerialisieren(i_objObjekt);
			m_edcIoDienst.SUB_DateiInhaltSchreiben(i_strPfad, text);
			return text;
		}

		private void SUB_ParameterDatenBereinigen(EDC_ParameterDatenKonfig i_edcKonfig, IEnumerable<string> i_enuZuIgnorierendeAdressen)
		{
			List<EDC_ParameterDaten> list = i_edcKonfig.PRO_enuParameter.ToList();
			if (list.Any())
			{
				foreach (EDC_ParameterDaten item in from i_edcParameter in i_edcKonfig.PRO_enuParameter
				where i_enuZuIgnorierendeAdressen.Contains(i_edcParameter.PRO_strPhysischeAdresse)
				select i_edcParameter)
				{
					list.Remove(item);
				}
				i_edcKonfig.PRO_enuParameter = list;
			}
		}

		private async Task FUN_fdcKonfigurationInDatenbankSpeichernAsync(string i_strDateiName, string i_strBeschreibung, string i_strSerialisierterInhalt, long i_i64BenutzerId)
		{
			EDC_MaschinenkonfigurationData eDC_MaschinenkonfigurationData = new EDC_MaschinenkonfigurationData();
			EDC_MaschinenkonfigurationData eDC_MaschinenkonfigurationData2 = eDC_MaschinenkonfigurationData;
			long num2 = eDC_MaschinenkonfigurationData2.PRO_i64MaschinenId = await m_edcBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			eDC_MaschinenkonfigurationData.PRO_strDateiname = i_strDateiName;
			eDC_MaschinenkonfigurationData.PRO_strBeschreibung = i_strBeschreibung;
			eDC_MaschinenkonfigurationData.PRO_strKonfigurationsDatei = i_strSerialisierterInhalt;
			eDC_MaschinenkonfigurationData.PRO_dtmAngelegtVon = i_i64BenutzerId;
			await m_edcMaschinenKonfigurationsDataAccess.Value.FUN_fdcSpeichereKonfigurationAsync(eDC_MaschinenkonfigurationData).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
