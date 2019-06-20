using Ersa.Global.Common.Extensions;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Logging;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Modell;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Dienste
{
    /// <summary>
    /// Abstract base communication service 
    /// </summary>
	public abstract class EDC_MesKommunikationsDienstBasis
	{
        #region parameter
        /// <summary>
        /// 小信号量
        /// </summary>
        protected static readonly SemaphoreSlim ms_fdcDienstzugriffSemaphore = new SemaphoreSlim(1);
        /// <summary>
        /// namespace name
        /// </summary>
		private readonly string m_strNamespaceName;
        /// <summary>
        /// class name
        /// </summary>
		private readonly string m_strKlassenName;
        /// <summary>
        /// log
        /// </summary>
		private readonly INF_Logger m_edcLogger;
        /// <summary>
        /// machine settings service
        /// </summary>
		private readonly INF_MaschinenEinstellungenDienst m_edcMaschinenEinstellungenDienst;
        /// <summary>
        /// json serialalization servcie
        /// </summary>
		private readonly INF_JsonSerialisierungsDienst m_edcJsonSerialisierungsDienst;
        /// <summary>
        /// 
        /// </summary>
		public IEnumerable<INF_MesFunktion> PRO_enuFunktionen
		{
			get;
		}

		public ENUM_MesTyp PRO_enmMesTyp
		{
			get;
		}

		public bool PRO_blnIstVerbunden
		{
			get;
			protected set;
		}

		protected INF_MesContainer PRO_edcMesContainer
		{
			get;
		}

		protected IEventAggregator PRO_fdcEventAggregator
		{
			get;
		}

		protected EDC_MesTypEinstellung PRO_edcEinstellungen
		{
			get;
			private set;
		}
        #endregion 

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="i_edcMaschinenEinstellungenDienst"></param>
        /// <param name="i_edcLogger"></param>
        /// <param name="i_fdcEventAggregator"></param>
        /// <param name="i_edcMesContainer"></param>
        /// <param name="i_enuFunktionen"></param>
        /// <param name="i_edcJsonSerialisierungsDienst"></param>
        /// <param name="i_enmMesTyp"></param>
        protected EDC_MesKommunikationsDienstBasis(INF_MaschinenEinstellungenDienst i_edcMaschinenEinstellungenDienst, INF_Logger i_edcLogger, IEventAggregator i_fdcEventAggregator, INF_MesContainer i_edcMesContainer, IEnumerable<INF_MesFunktion> i_enuFunktionen, INF_JsonSerialisierungsDienst i_edcJsonSerialisierungsDienst, ENUM_MesTyp i_enmMesTyp)
		{
			m_strNamespaceName = this.FUN_strGibNameSpace();
			m_strKlassenName = this.FUN_strGibKlassenName();
			m_edcLogger = i_edcLogger;
			m_edcMaschinenEinstellungenDienst = i_edcMaschinenEinstellungenDienst;
			m_edcJsonSerialisierungsDienst = i_edcJsonSerialisierungsDienst;
			PRO_blnIstVerbunden = false;
			PRO_fdcEventAggregator = i_fdcEventAggregator;
			PRO_edcMesContainer = i_edcMesContainer;
			PRO_enuFunktionen = i_enuFunktionen;
			PRO_enmMesTyp = i_enmMesTyp;
		}

		public async Task<EDC_MesTypEinstellung> FUN_fdcHoleDienstEinstellungenAsync<T>(ENUM_MesTyp i_enmMesTyp, EDC_MesTypEinstellung i_edcDefaultEinstellungen)
		{
			if (PRO_edcEinstellungen != null)
			{
				return PRO_edcEinstellungen;
			}
			string i_strFormatierterString = await m_edcMaschinenEinstellungenDienst.FUN_fdcHoleMesEinstellungenAsync(i_enmMesTyp).ConfigureAwait(continueOnCapturedContext: false);
			PRO_edcEinstellungen = (m_edcJsonSerialisierungsDienst.FUN_objDeserialisieren<T>(i_strFormatierterString) as EDC_MesTypEinstellung);
			if (PRO_edcEinstellungen == null)
			{
				PRO_edcEinstellungen = i_edcDefaultEinstellungen;
			}
			PRO_edcEinstellungen.SUB_VerionskompatiblitaetHerstellen();
			return PRO_edcEinstellungen;
		}

		public async Task FUN_fdcSetzeDienstEinstellungenAsync<T>(ENUM_MesTyp i_enmMesTyp, EDC_MesTypEinstellung i_edcEinstellungen)
		{
			if (i_edcEinstellungen is T)
			{
				PRO_edcEinstellungen = i_edcEinstellungen;
				string i_strEinstellungen = m_edcJsonSerialisierungsDienst.FUN_strSerialisieren(PRO_edcEinstellungen);
				await m_edcMaschinenEinstellungenDienst.FUN_fdcSpeichereMesEinstellungenAsync(i_strEinstellungen, i_enmMesTyp).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		protected virtual async Task<EDC_MesKommunikationsRueckgabe> FUN_fdcNachrichtSendenAsync<T>(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten, EDC_MesTypEinstellung i_edcDefaultEinstellungen)
		{
			EDC_MesTypEinstellung i_edcEinstellung = await FUN_fdcHoleDienstEinstellungenAsync<T>(ENUM_MesTyp.Zvei, i_edcDefaultEinstellungen).ConfigureAwait(continueOnCapturedContext: false);
			return await PRO_enuFunktionen.FirstOrDefault((INF_MesFunktion i_edcFunktion) => i_edcFunktion.PRO_enuMesFunktion.Equals(i_enuFunktion)).FUN_fdcAusfuehrenAsync(i_edcEinstellung, i_edcMaschinenDaten).ConfigureAwait(continueOnCapturedContext: false);
		}

        // 写入日志
		protected void SUB_LogEintragSchreiben(string i_strEintrag, [CallerMemberName] string i_strMethodenName = "")
		{
			if ((m_edcLogger.PRO_enmLoggingLevel & ENUM_LogLevel.Traceability) != ENUM_LogLevel.Kein)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Traceability, i_strEintrag, m_strNamespaceName, m_strKlassenName, i_strMethodenName);
			}
		}
	}
}
