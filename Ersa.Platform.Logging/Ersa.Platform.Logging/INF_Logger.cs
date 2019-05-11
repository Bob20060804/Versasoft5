using System;

namespace Ersa.Platform.Logging
{
	public interface INF_Logger
	{
		bool PRO_blnIstLoggerAktiv
		{
			get;
		}

		bool PRO_blnIstInitialisiert
		{
			get;
		}

		ENUM_LogLevel PRO_enmLoggingLevel
		{
			get;
		}

		void SUB_LoggerInitialisieren(string i_strLogDateiPfad, bool i_blnStartetApplikation, string i_strFirmenName, string i_strAssemblyName, string i_strVersion, bool i_blnErweitertesLoggingVerwenden);

		void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strNamespace = null, string i_strKlassenName = null, string i_strMethodenName = null, Exception i_excException = null);

		void SUB_LogLevelSetzen(ENUM_LogLevel i_enmLogLevel);

		void SUB_DateiAktualisieren();

		void SUB_PfadNeuSetzen(string i_strLogDateiVerzeichnis);
	}
}
