using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.Common.Produktionssteuerung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Modell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Konfiguration
{
	/// <summary>
	/// MES配置管理者
	/// </summary>
	[Export(typeof(INF_MesKonfigurationsManager))]
	public class EDC_MesKonfigurationsManager : INF_MesKonfigurationsManager
	{
		/// <summary>
		/// 机器配置服务
		/// </summary>
		private readonly INF_MaschinenEinstellungenDienst m_edcMaschinenEinstellungenDienst;

		/// <summary>
		/// 生产控制服务
		/// </summary>
		private readonly INF_ProduktionssteuerungsDienst m_edcProduktionssteuerungsDienst;

		/// <summary>
		/// Json序列化服务
		/// </summary>
		private readonly INF_JsonSerialisierungsDienst m_edcJsonSerialisierungsDienst;

		/// <summary>
		/// 
		/// </summary>
		private EDC_Produktionssteuerungsdaten m_edcProduktionssteuerungsdaten;

		private EDC_MesKonfiguration m_edcMesKonfiguration;

		[ImportMany]
		public IEnumerable<INF_MesKommunikationsDienst> PRO_enuKommunikationsDienste
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_MesKonfigurationsManager(INF_MaschinenEinstellungenDienst i_edcMaschinenEinstellungenDienst, INF_ProduktionssteuerungsDienst i_edcProduktionssteuerungsDienst, INF_JsonSerialisierungsDienst i_edcJsonSerialisierungsDienst)
		{
			m_edcProduktionssteuerungsDienst = i_edcProduktionssteuerungsDienst;
			m_edcMaschinenEinstellungenDienst = i_edcMaschinenEinstellungenDienst;
			m_edcJsonSerialisierungsDienst = i_edcJsonSerialisierungsDienst;
		}

		public async Task FUN_fdcInitialisierenAsync()
		{
			await FUN_fdcLadeProduktionssteuerungsdatenAsync().ConfigureAwait(continueOnCapturedContext: false);
			await FUN_fdcLadeMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<bool> FUN_fdcIstMesKonfiguriertAsync()
		{
			await FUN_fdcLadeMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
			return m_edcMesKonfiguration != null && !m_edcMesKonfiguration.PRO_enuMesTyp.Equals(ENUM_MesTyp.KeinMes);
		}

		public async Task<bool> FUN_fdcIstMesAktivAsync()
		{
			if (!(await FUN_fdcIstMesKonfiguriertAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return false;
			}
			await FUN_fdcLadeProduktionssteuerungsdatenAsync().ConfigureAwait(continueOnCapturedContext: false);
			return m_edcProduktionssteuerungsdaten != null && m_edcProduktionssteuerungsdaten.PRO_edcProduktionsEinstellungen != null && m_edcProduktionssteuerungsdaten.PRO_edcProduktionsEinstellungen.PRO_blnMesAktiv;
		}

		public Task FUN_fdcMesAktivSetzenAsync()
		{
			return FUN_fdcSetzeMesStatusInProduktionssteuerungAsync(i_blnZustand: true);
		}

		public Task FUN_fdcMesInaktivSetzenAsync()
		{
			return FUN_fdcSetzeMesStatusInProduktionssteuerungAsync(i_blnZustand: false);
		}

		public async Task<EDC_MesKonfiguration> FUN_fdcHoleMesKonfigurationAsync()
		{
			if (m_edcMesKonfiguration == null)
			{
				await FUN_fdcLadeMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			return m_edcMesKonfiguration;
		}

		public async Task<ENUM_MesTyp> FUN_fdcHoleMesTypAsync()
		{
			return (await FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false))?.PRO_enuMesTyp ?? ENUM_MesTyp.KeinMes;
		}

		public async Task FUN_fdcLadeProduktionssteuerungsdatenAsync()
		{
			EDC_Produktionssteuerungsdaten edcProduktionssteuerungsdaten = m_edcProduktionssteuerungsdaten;
			EDC_Produktionssteuerungsdaten eDC_Produktionssteuerungsdaten = m_edcProduktionssteuerungsdaten = await m_edcProduktionssteuerungsDienst.FUN_edcAktiveProduktionssteuerungsDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcLadeMesKonfigurationAsync()
		{
			string text = await m_edcMaschinenEinstellungenDienst.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
			m_edcMesKonfiguration = (string.IsNullOrEmpty(text) ? EDC_MesKonstanten.gs_edcMesDefaultKonfiguration : m_edcJsonSerialisierungsDienst.FUN_objDeserialisieren<EDC_MesKonfiguration>(text));
			m_edcMesKonfiguration.SUB_VerionskompatiblitaetHerstellen();
			foreach (ENUM_MesTyp enmMesTyp in Enum.GetValues(typeof(ENUM_MesTyp)))
			{
				if (!m_edcMesKonfiguration.PRO_lstMesFunktionen.Exists((EDC_MesTypFunktionenListe i_edcMesTyp) => i_edcMesTyp.PRO_enuMesTyp == enmMesTyp))
				{
					m_edcMesKonfiguration.PRO_lstMesFunktionen.Add(new EDC_MesTypFunktionenListe
					{
						PRO_enuMesTyp = enmMesTyp,
						PRO_lstMesFunktionen = new List<EDC_MesFunktionenKonfiguration>()
					});
				}
			}
			foreach (INF_MesKommunikationsDienst item in PRO_enuKommunikationsDienste)
			{
				List<EDC_MesFunktionenKonfiguration> list = m_edcMesKonfiguration.FUN_lstHoleMesFunktionenListeFuerMesTyp(item.PRO_enmMesTyp);
				foreach (INF_MesFunktion edcFunktion in item.PRO_enuFunktionen)
				{
					if (!list.Exists((EDC_MesFunktionenKonfiguration i_edcFunktion) => i_edcFunktion.PRO_enmFunktion.Equals(edcFunktion.PRO_enuMesFunktion)))
					{
						list.Add(new EDC_MesFunktionenKonfiguration
						{
							PRO_enmFunktion = edcFunktion.PRO_enuMesFunktion,
							PRO_blnIstAktiv = false
						});
					}
				}
				List<EDC_MesFunktionenKonfiguration> list2 = new List<EDC_MesFunktionenKonfiguration>();
				foreach (EDC_MesFunktionenKonfiguration edcMesFunktionenKonfiguration in list)
				{
					if (item.PRO_enuFunktionen.FirstOrDefault((INF_MesFunktion i_edcFunktion) => i_edcFunktion.PRO_enuMesFunktion.Equals(edcMesFunktionenKonfiguration.PRO_enmFunktion)) == null)
					{
						list2.Add(edcMesFunktionenKonfiguration);
					}
				}
				foreach (EDC_MesFunktionenKonfiguration item2 in list2)
				{
					list.Remove(item2);
				}
			}
			string i_strKonfiguration = m_edcJsonSerialisierungsDienst.FUN_strSerialisieren(m_edcMesKonfiguration);
			await m_edcMaschinenEinstellungenDienst.FUN_fdcSpeichereMesKonfigurationAsync(i_strKonfiguration).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereMesKonfigurationAsync(EDC_MesKonfiguration i_edcKonfiguration)
		{
			string i_strKonfiguration = m_edcJsonSerialisierungsDienst.FUN_strSerialisieren(i_edcKonfiguration);
			await m_edcMaschinenEinstellungenDienst.FUN_fdcSpeichereMesKonfigurationAsync(i_strKonfiguration).ConfigureAwait(continueOnCapturedContext: false);
			m_edcMesKonfiguration = i_edcKonfiguration;
		}

		public async Task<INF_MesKommunikationsDienst> FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync()
		{
			_003C_003Ec__DisplayClass20_0 _003C_003Ec__DisplayClass20_;
			ENUM_MesTyp enmTyp2 = _003C_003Ec__DisplayClass20_.enmTyp;
			ENUM_MesTyp enmTyp;
			ENUM_MesTyp eNUM_MesTyp = enmTyp = await FUN_fdcHoleMesTypAsync().ConfigureAwait(continueOnCapturedContext: false);
			return PRO_enuKommunikationsDienste.FirstOrDefault((INF_MesKommunikationsDienst i_edcDienst) => i_edcDienst.PRO_enmMesTyp.Equals(enmTyp));
		}

		public Task<INF_MesKommunikationsDienst> FUN_fdcHoleKommunikationsDienstAsync(ENUM_MesTyp i_enmMesTyp)
		{
			return Task.Factory.StartNew(() => PRO_enuKommunikationsDienste.FirstOrDefault((INF_MesKommunikationsDienst i_edcDienst) => i_edcDienst.PRO_enmMesTyp.Equals(i_enmMesTyp)));
		}

		private async Task FUN_fdcSetzeMesStatusInProduktionssteuerungAsync(bool i_blnZustand)
		{
			bool blnUpdate = true;
			EDC_Produktionssteuerungsdaten edcProduktionssteuerungsdaten = await m_edcProduktionssteuerungsDienst.FUN_edcAktiveProduktionssteuerungsDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (edcProduktionssteuerungsdaten == null)
			{
				blnUpdate = false;
				edcProduktionssteuerungsdaten = new EDC_Produktionssteuerungsdaten();
			}
			if (edcProduktionssteuerungsdaten.PRO_edcProduktionsEinstellungen == null)
			{
				edcProduktionssteuerungsdaten.PRO_edcProduktionsEinstellungen = new EDC_ProduktionsEinstellungen();
			}
			edcProduktionssteuerungsdaten.PRO_edcProduktionsEinstellungen.PRO_blnMesAktiv = i_blnZustand;
			if (blnUpdate)
			{
				await m_edcProduktionssteuerungsDienst.FUN_fdcProduktionssteuerungsDatenAendernAsync(edcProduktionssteuerungsdaten).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcProduktionssteuerungsDienst.FUN_fdcProduktionssteuerungsDatenErstellenAsync(edcProduktionssteuerungsdaten).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
