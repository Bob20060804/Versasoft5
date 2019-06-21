using System;

namespace Ersa.Platform.Logging
{
	public interface INF_Logger
	{
        /// <summary>
        /// Active
        /// </summary>
		bool PRO_blnIstLoggerAktiv
		{
			get;
		}

        /// <summary>
        /// Initialize
        /// </summary>
		bool PRO_blnIstInitialisiert
		{
			get;
		}

        /// <summary>
        /// log level
        /// </summary>
		ENUM_LogLevel PRO_enmLoggingLevel
		{
			get;
		}

        /// <summary>
        /// Initrilize
        /// </summary>
        /// <param name="i_strLogDateiPfad"></param>
        /// <param name="i_blnStartetApplikation"></param>
        /// <param name="i_strFirmenName"></param>
        /// <param name="i_strAssemblyName"></param>
        /// <param name="i_strVersion"></param>
        /// <param name="i_blnErweitertesLoggingVerwenden"></param>
		void SUB_LoggerInitialisieren(string i_strLogDateiPfad, bool i_blnStartetApplikation, string i_strFirmenName, string i_strAssemblyName, string i_strVersion, bool i_blnErweitertesLoggingVerwenden);

        /// <summary>
        /// –¥»’÷æ
        /// </summary>
        /// <param name="i_enmLogLevel"></param>
        /// <param name="i_strEintrag"></param>
        /// <param name="i_strNamespace"></param>
        /// <param name="i_strKlassenName"></param>
        /// <param name="i_strMethodenName"></param>
        /// <param name="i_excException"></param>
		void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strNamespace = null, string i_strKlassenName = null, string i_strMethodenName = null, Exception i_excException = null);

        /// <summary>
        /// Set log level
        /// </summary>
        /// <param name="i_enmLogLevel"></param>
		void SUB_LogLevelSetzen(ENUM_LogLevel i_enmLogLevel);

        /// <summary>
        /// Update file
        /// </summary>
		void SUB_DateiAktualisieren();

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="i_strLogDateiVerzeichnis"></param>
		void SUB_PfadNeuSetzen(string i_strLogDateiVerzeichnis);
	}
}
