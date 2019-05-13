using Ersa.Platform.Logging;
using System;
using System.Diagnostics;

namespace Ersa.Platform.Infrastructure.Prism
{
	public class EDC_ServiceLocator : EDC_ServiceLocatorBase
	{
		private static readonly Lazy<EDC_ServiceLocator> ms_edcInstanz = new Lazy<EDC_ServiceLocator>(() => new EDC_ServiceLocator());

		private INF_Logger m_edcLogger;

		private INF_Logger m_edcFallbackLogger;

		public static EDC_ServiceLocator PRO_edcInstanz
		{
			[DebuggerStepThrough]
			get
			{
				return ms_edcInstanz.Value;
			}
		}

		public event EventHandler EVT_AktiverLoggerGeaendert;

		[DebuggerStepThrough]
		public INF_Logger FUN_edcLoggerHolen()
		{
			if (m_edcLogger == null)
			{
				m_edcLogger = FUN_objObjektSicherAusContainerHolen<INF_Logger>();
				EventHandler eVT_AktiverLoggerGeaendert = this.EVT_AktiverLoggerGeaendert;
				if (m_edcLogger != null && eVT_AktiverLoggerGeaendert != null)
				{
					eVT_AktiverLoggerGeaendert(this, null);
					return m_edcLogger;
				}
				return FUN_edcFallbackLoggerErstellen();
			}
			m_edcFallbackLogger = null;
			return m_edcLogger;
		}

		[DebuggerStepThrough]
		private INF_Logger FUN_edcFallbackLoggerErstellen()
		{
			if (m_edcFallbackLogger == null)
			{
				m_edcFallbackLogger = new EDC_KonsolenLogger();
				m_edcFallbackLogger.SUB_LoggerInitialisieren(null, i_blnStartetApplikation: true, null, null, null, i_blnErweitertesLoggingVerwenden: true);
				m_edcFallbackLogger.SUB_LogLevelSetzen(ENUM_LogLevel.Alle);
			}
			return m_edcFallbackLogger;
		}
	}
}
