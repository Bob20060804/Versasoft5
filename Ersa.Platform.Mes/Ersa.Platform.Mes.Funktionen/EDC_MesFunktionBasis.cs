using Ersa.Global.Common.Extensions;
using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using System.Runtime.CompilerServices;

namespace Ersa.Platform.Mes.Funktionen
{
    /// <summary>
    /// MES base function 
    /// </summary>
	public class EDC_MesFunktionBasis
	{
        /// <summary>
        /// Namespace name
        /// </summary>
		private readonly string m_strNamespaceName;
        /// <summary>
        /// class name
        /// </summary>
		private readonly string m_strKlassenName;
        /// <summary>
        /// 日志
        /// </summary>
		private readonly INF_Logger m_edcLogger;

        /// <summary>
        /// MES容器
        /// </summary>
		protected INF_MesContainer PRO_edcContainer
		{
			get;
			private set;
		}

        /// <summary>
        /// Localization Service
        /// </summary>
		protected INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			get;
			private set;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_edcLogger">log</param>
        /// <param name="i_edcContainer">MES容器</param>
        /// <param name="i_edcLokalisierungsDienst">localization service</param>
		protected EDC_MesFunktionBasis(INF_Logger i_edcLogger, INF_MesContainer i_edcContainer, INF_LokalisierungsDienst i_edcLokalisierungsDienst)
		{
			m_strNamespaceName = this.FUN_strGibNameSpace();
			m_strKlassenName = this.FUN_strGibKlassenName();
			m_edcLogger = i_edcLogger;
			PRO_edcContainer = i_edcContainer;
			PRO_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
		}

        /// <summary>
        /// 写日志 条目?
        /// </summary>
        /// <param name="i_strEintrag"></param>
        /// <param name="i_strMethodenName"></param>
		protected void SUB_LogEintragSchreiben(string i_strEintrag, [CallerMemberName] string i_strMethodenName = "")
		{
			if ((m_edcLogger.PRO_enmLoggingLevel & ENUM_LogLevel.Traceability) != ENUM_LogLevel.Kein)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Traceability, i_strEintrag, m_strNamespaceName, m_strKlassenName, i_strMethodenName);
			}
		}
	}
}
