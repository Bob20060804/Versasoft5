#define TRACE
using Ersa.Platform.Logging;
using System;
using System.Diagnostics;

namespace Ersa.Platform.Infrastructure
{
	internal class EDC_KonsolenLogger : INF_Logger
	{
		public bool PRO_blnIstInitialisiert
		{
			get;
			private set;
		}

		public bool PRO_blnIstLoggerAktiv
		{
			[DebuggerStepThrough]
			get
			{
				return PRO_enmLoggingLevel != ENUM_LogLevel.Kein;
			}
		}

		public ENUM_LogLevel PRO_enmLoggingLevel
		{
			get;
			private set;
		}

		public void SUB_LoggerInitialisieren(string i_strLogDateiPfad, bool i_blnStartetApplikation, string i_strFirmenName, string i_strAssemblyName, string i_strVersion, bool i_blnErweitertesLoggingVerwenden)
		{
			PRO_blnIstInitialisiert = true;
		}

		[DebuggerStepThrough]
		public void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strNamespace = null, string i_strKlassenName = null, string i_strMethodenName = null, Exception i_excException = null)
		{
			if (PRO_blnIstInitialisiert && PRO_enmLoggingLevel >= i_enmLogLevel)
			{
				if (i_excException == null)
				{
					Trace.WriteLine($"{DateTime.Now} {i_enmLogLevel}:\t{i_strNamespace}:{i_strKlassenName}:{i_strMethodenName} - {i_strEintrag}");
					return;
				}
				Trace.WriteLine("---!!! Fehler !!!---");
				Trace.WriteLine($"{DateTime.Now} {i_enmLogLevel}:\t{i_strNamespace}:{i_strKlassenName}:{i_strMethodenName} - {i_strEintrag}");
				Trace.WriteLine(i_excException);
			}
		}

		public void SUB_LogLevelSetzen(ENUM_LogLevel i_enmLogLevel)
		{
			PRO_enmLoggingLevel = i_enmLogLevel;
		}

		public void SUB_DateiAktualisieren()
		{
		}

		public void SUB_PfadNeuSetzen(string i_strLogDateiVerzeichnis)
		{
		}
	}
}
