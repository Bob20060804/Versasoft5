using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.CapabilityContracts.Meldungen;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using Ersa.Platform.Dienste.Meldungen.Interfaces;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// MES 功能
/// </summary>
namespace Ersa.Platform.Mes.Capabilities
{
	/// <summary>
	/// MES消息生成器功能
	/// </summary>
	[Export(typeof(INF_MeldungProduzentCapability))]
	[Export(typeof(INF_MeldungHinzufuegen))]
	public class EDC_MesMeldungProduzentCapability : INF_MeldungProduzentCapability, INF_MeldungHinzufuegen
	{
		private const int mC_i32MeldungsOrt1 = 12113;

		private readonly INF_JsonSerialisierungsDienst m_edcJsonSerialisierungsDienst;

		private readonly INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		/// <summary>
		/// MES Service
		/// </summary>
		[Import(typeof(INF_MesDienst))]
		public INF_MesDienst PRO_edcMesDienst
		{
			get;
			set;
		}

		/// <summary>
		/// message administration service
		/// </summary>
		[Import(typeof(INF_MeldungVerwaltungsDienst))]
		public INF_MeldungVerwaltungsDienst PRO_edcMeldungVerwaltungsDienst
		{
			get;
			set;
		}

		/// <summary>
		/// 消息产生者
		/// </summary>
		public ENUM_MeldungProduzent PRO_enmMeldungProduzent => ENUM_MeldungProduzent.Mes;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="i_edcJsonSerialisierungsDienst"></param>
		/// <param name="i_edcLokalisierungsDienst"></param>
		[ImportingConstructor]
		public EDC_MesMeldungProduzentCapability(INF_JsonSerialisierungsDienst i_edcJsonSerialisierungsDienst, INF_LokalisierungsDienst i_edcLokalisierungsDienst)
		{
			m_edcJsonSerialisierungsDienst = i_edcJsonSerialisierungsDienst;
			m_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
		}

		/// <summary>
		/// 创建消息
		/// </summary>
		/// <param name="i_i32Meldungsnummer"></param>
		/// <param name="i_strLokalisierungsKeyMesTyp"></param>
		/// <param name="i_i32MeldungOrt3"></param>
		/// <param name="i_edcPossibleactions"></param>
		/// <param name="i_edcProcessactionsns"></param>
		/// <param name="i_strDetails"></param>
		/// <param name="i_strContext"></param>
		/// <param name="i_blnDuplicateAllowed">是否允许重复</param>
		/// <param name="i_enuAktion"></param>
		/// <returns></returns>
		public async Task SUB_CreateMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3, IEnumerable<ENUM_MeldungAktionen> i_edcPossibleactions, IEnumerable<ENUM_ProzessAktionen> i_edcProcessactionsns, string i_strDetails, string i_strContext, bool i_blnDuplicateAllowed, ENUM_MeldungAktionen i_enuAktion)
		{
			EDC_Meldung eDC_Meldung = new EDC_Meldung
			{
				PRO_strMeldungGuid = Guid.NewGuid().ToString(),
				PRO_enmMeldungProduzent = ENUM_MeldungProduzent.Mes,
				PRO_sttAufgetreten = DateTime.Now,
				PRO_enmMeldungsTyp = ((i_enuAktion == ENUM_MeldungAktionen.Erstellen) ? ENUM_MeldungsTypen.enmFehler : ENUM_MeldungsTypen.enmHinweis),
				PRO_i32ProduzentenCode = 0,
				PRO_i32MeldungsNummer = i_i32Meldungsnummer,
				PRO_i32MeldungsOrt1 = 12113,
				PRO_i32MeldungsOrt2 = Convert.ToInt32(i_strLokalisierungsKeyMesTyp),
				PRO_i32MeldungsOrt3 = i_i32MeldungOrt3,
				PRO_enuMoeglicheAktionen = i_edcPossibleactions,
				PRO_enuProzessAktionen = i_edcProcessactionsns,
				PRO_strDetails = i_strDetails,
				PRO_strContext = i_strContext
			};
			if (i_enuAktion == ENUM_MeldungAktionen.Loggen)
			{
				eDC_Meldung.PRO_sttQuittiert = DateTime.Now;
			}
			if (i_blnDuplicateAllowed || !(from i_edcMeldung in PRO_edcMeldungVerwaltungsDienst.FUN_lstAktuelleNichtQuittierteMeldungenListe(ENUM_MeldungProduzent.Mes)
			where i_edcMeldung.PRO_i32MeldungsNummer == i_i32Meldungsnummer
			select i_edcMeldung).Any())
			{
				await PRO_edcMeldungVerwaltungsDienst.FUN_fdcMeldungenBehandelnAsync(new List<INF_Meldung>
				{
					eDC_Meldung
				}, i_enuAktion).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		/// <summary>
		/// 是否配置
		/// </summary>
		/// <returns></returns>
		public bool FUN_blnIstKonfiguriert()
		{
			return true;
		}

		/// <summary>
		/// 请求处理消息异步
		/// </summary>
		/// <param name="i_edcMeldung">Report</param>
		/// <param name="i_enmAktion">可采取的行动(创造|确认...)</param>
		/// <returns></returns>
		public async Task FUN_fdcMeldungBehandelnAnfordernAsync(INF_Meldung i_edcMeldung, ENUM_MeldungAktionen i_enmAktion)
		{
			switch (i_enmAktion)
			{
				case ENUM_MeldungAktionen.Quittieren:
					// 需要确认
					if (await PRO_edcMesDienst.FUN_fdcAcknowledgeMessageAsync(i_edcMeldung).ConfigureAwait(true))
					{
						await PRO_edcMeldungVerwaltungsDienst.FUN_fdcMeldungenBehandelnAsync(new List<INF_Meldung>
					{
						i_edcMeldung
					}, ENUM_MeldungAktionen.Quittieren).ConfigureAwait(true);
					}
					break;
				case ENUM_MeldungAktionen.Zurueckstellen:
					// 需要返回
					if (await PRO_edcMesDienst.FUN_fdcResetMessageAsync(i_edcMeldung).ConfigureAwait(true))
					{
						await PRO_edcMeldungVerwaltungsDienst.FUN_fdcMeldungenBehandelnAsync(new List<INF_Meldung>
					{
						i_edcMeldung
					}, ENUM_MeldungAktionen.Zurueckstellen).ConfigureAwait(true);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("i_enmAktion", i_enmAktion, null);
			}
		}

		public Task<IEnumerable<INF_Meldung>> FUN_fdcErmittleZuQuittierendeMeldungenAsync(IEnumerable<INF_Meldung> i_enuNichQuittierteMeldungen)
		{
			return Task.FromResult(i_enuNichQuittierteMeldungen);
		}

		public async Task SUB_QuittiereMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3)
		{
			IEnumerable<INF_Meldung> source = PRO_edcMeldungVerwaltungsDienst.FUN_lstAktuelleNichtQuittierteMeldungenListe(ENUM_MeldungProduzent.Mes);
			source = from i_edcMeldung in source
			where i_edcMeldung.PRO_i32MeldungsNummer == i_i32Meldungsnummer
			select i_edcMeldung;
			if (source.Any())
			{
				await PRO_edcMeldungVerwaltungsDienst.FUN_fdcMeldungenBehandelnAsync(source, ENUM_MeldungAktionen.Quittieren).ConfigureAwait(continueOnCapturedContext: true);
			}
		}
	}
}
