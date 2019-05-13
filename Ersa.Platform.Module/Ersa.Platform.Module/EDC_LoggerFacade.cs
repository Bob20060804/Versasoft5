#define TRACE
using Ersa.Platform.Infrastructure.Prism;
using Ersa.Platform.Logging;
using Prism.Logging;
using System;
using System.Diagnostics;

namespace Ersa.Platform.Module
{
	public class EDC_LoggerFacade : ILoggerFacade
	{
		private INF_Logger m_edcLogger;

		public void Log(string message, Category category, Priority priority)
		{
			if (m_edcLogger == null)
			{
				try
				{
					m_edcLogger = EDC_ServiceLocator.PRO_edcInstanz.FUN_edcLoggerHolen();
				}
				catch (Exception)
				{
					Trace.WriteLine("Logger konnte nicht am ServiceLocator ermittelt werden. Verwende Trace-Ausgabe.");
					Trace.WriteLine(message);
					return;
				}
			}
			if (m_edcLogger.PRO_blnIstInitialisiert)
			{
				m_edcLogger.SUB_LogEintragSchreiben(FUN_LogLevelKonvertieren(category), message);
			}
		}

		private ENUM_LogLevel FUN_LogLevelKonvertieren(Category i_fdcCategory)
		{
			switch (i_fdcCategory)
			{
			case Category.Debug:
			case Category.Info:
				return ENUM_LogLevel.Info;
			case Category.Exception:
				return ENUM_LogLevel.Fehler;
			case Category.Warn:
				return ENUM_LogLevel.Warnung;
			default:
				throw new ArgumentOutOfRangeException("i_fdcCategory");
			}
		}
	}
}
