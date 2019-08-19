using Ersa.Global.Common;
using Ersa.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Ersa.Platform.Logging
{
	[Export(typeof(INF_Logger))]
	public class EDC_Logger : EDC_DisposableObject, INF_Logger
	{
		private readonly IDictionary<ENUM_LogLevel, ENUM_LoggingLevels> m_dicLogLevelDllEnums;

		private readonly IDictionary<ENUM_LoggingLevels, ENUM_LogLevel> m_dicLogLevelLokaleEnums;

		private EDC_ERSALogging m_edcErsaLogger;

		private bool m_blnIstLoggerInitialisiert;

        /// <summary>
        /// Active
        /// </summary>
		public bool PRO_blnIstLoggerAktiv
		{
			[DebuggerStepThrough]
			get
			{
				if (PRO_blnIstInitialisiert && m_edcErsaLogger != null)
				{
					return m_edcErsaLogger.PRO_enmLoggingLevel != ENUM_LoggingLevels.Kein;
				}
				return false;
			}
		}

        /// <summary>
        /// Initialize
        /// </summary>
		public bool PRO_blnIstInitialisiert
		{
			[DebuggerStepThrough]
			get
			{
				return m_blnIstLoggerInitialisiert;
			}
		}

		public ENUM_LogLevel PRO_enmLoggingLevel
		{
			[DebuggerStepThrough]
			get
			{
				if (!PRO_blnIstInitialisiert || m_edcErsaLogger == null)
				{
					return ENUM_LogLevel.Kein;
				}
				return FUN_enmLogLevelKonvertieren(m_edcErsaLogger.PRO_enmLoggingLevel);
			}
		}

		public EDC_Logger()
		{
			m_dicLogLevelDllEnums = new Dictionary<ENUM_LogLevel, ENUM_LoggingLevels>();
			m_dicLogLevelLokaleEnums = new Dictionary<ENUM_LoggingLevels, ENUM_LogLevel>();
			SUB_LogLevelEntsprechungenInitialisieren();
		}

		public void SUB_LoggerInitialisieren(string i_strLogDateiPfad, bool i_blnStartetApplikation, string i_strFirmenName, string i_strAssemblyName, string i_strVersion, bool i_blnErweitertesLoggingVerwenden)
		{
			m_blnIstLoggerInitialisiert = false;
			m_edcErsaLogger = new EDC_ERSALogging();
			if (m_edcErsaLogger.FUN_blnInit(i_blnStartetApplikation))
			{
				m_edcErsaLogger.PRO_strNameDateiMitPfad = i_strLogDateiPfad;
				m_edcErsaLogger.PRO_strNameFirmaExtern = i_strFirmenName;
				m_edcErsaLogger.PRO_strNameSoftwareProduktExtern = i_strAssemblyName;
				m_edcErsaLogger.PRO_strVersionSoftwareProduktExtern = i_strVersion;
				m_edcErsaLogger.PRO_blnLoggingErweitert = i_blnErweitertesLoggingVerwenden;
				m_blnIstLoggerInitialisiert = true;
			}
		}

		public void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strNamespace = null, string i_strKlassenName = null, string i_strMethodenName = null, Exception i_excException = null)
		{
			if (m_blnIstLoggerInitialisiert)
			{
				m_edcErsaLogger.SUB_Logging(FUN_enmLogLevelKonvertieren(i_enmLogLevel), i_strEintrag, $"{i_strNamespace}: {i_strKlassenName}", i_strMethodenName, i_excException);
				if (i_enmLogLevel == ENUM_LogLevel.Fehler || i_excException != null)
				{
					SUB_DateiAktualisieren();
				}
			}
		}

		public void SUB_LogLevelSetzen(ENUM_LogLevel i_enmLogLevel)
		{
			if (m_blnIstLoggerInitialisiert)
			{
				m_edcErsaLogger.PRO_enmLoggingLevel = FUN_enmLogLevelKonvertieren(i_enmLogLevel);
			}
		}

        /// <summary>
        /// Update file
        /// </summary>
		public void SUB_DateiAktualisieren()
		{
			if (m_edcErsaLogger != null && PRO_blnIstInitialisiert)
			{
				m_edcErsaLogger.FUN_blnDateiAktualisieren();
			}
		}

		public void SUB_PfadNeuSetzen(string i_strLogDateiVerzeichnis)
		{
			if (m_edcErsaLogger != null && PRO_blnIstInitialisiert)
			{
				string fileName = Path.GetFileName(m_edcErsaLogger.PRO_strNameDateiMitPfad);
				if (!string.IsNullOrWhiteSpace(fileName))
				{
					m_edcErsaLogger.PRO_strNameDateiMitPfad = Path.Combine(i_strLogDateiVerzeichnis, fileName);
					m_edcErsaLogger.FUN_blnDateiAktualisieren();
				}
			}
		}

		protected override void SUB_InternalDispose()
		{
			m_edcErsaLogger.SUB_LoggingBeenden();
		}

        /// <summary>
        /// Convert loglevel
        /// </summary>
        /// <param name="i_enmLogLevel"></param>
        /// <returns></returns>
		private ENUM_LoggingLevels FUN_enmLogLevelKonvertieren(ENUM_LogLevel i_enmLogLevel)
		{
			if (!m_dicLogLevelDllEnums.TryGetValue(i_enmLogLevel, out ENUM_LoggingLevels value))
			{
				throw new ArgumentOutOfRangeException("i_enmLogLevel");
			}
			return value;
		}

        /// <summary>
        /// Convert Log level
        /// </summary>
        /// <param name="i_enmLogLevel"></param>
        /// <returns></returns>
		[DebuggerStepThrough]
		private ENUM_LogLevel FUN_enmLogLevelKonvertieren(ENUM_LoggingLevels i_enmLogLevel)
		{
			if (!m_dicLogLevelLokaleEnums.TryGetValue(i_enmLogLevel, out ENUM_LogLevel value))
			{
				throw new ArgumentOutOfRangeException("i_enmLogLevel");
			}
			return value;
		}

		private void SUB_LogLevelEntsprechungenInitialisieren()
		{
			m_dicLogLevelDllEnums.Clear();
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Alle, ENUM_LoggingLevels.Alle);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.AlleBasis, ENUM_LoggingLevels.AlleBasis);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.AlleErweitert, ENUM_LoggingLevels.AlleErweitert);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Auslauf, ENUM_LoggingLevels.Auslauf);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Auslaufmodul, ENUM_LoggingLevels.Auslaufmodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Basisklasse, ENUM_LoggingLevels.Basisklasse);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Cnc, ENUM_LoggingLevels.CNC);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Codebetrieb, ENUM_LoggingLevels.Codebetrieb);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Datensicherung, ENUM_LoggingLevels.Datensicherung);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Einlauf, ENUM_LoggingLevels.Einlauf);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Einlaufmodul, ENUM_LoggingLevels.Einlaufmodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Fehler, ENUM_LoggingLevels.Fehler);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Fluxer, ENUM_LoggingLevels.Fluxer);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Fluxermodul, ENUM_LoggingLevels.Fluxermodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Heizung, ENUM_LoggingLevels.Heizung);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Info, ENUM_LoggingLevels.Info);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Kein, ENUM_LoggingLevels.Kein);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Konfiguration, ENUM_LoggingLevels.Konfiguration);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Leiterkarte, ENUM_LoggingLevels.Leiterkarte);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.LoetProgramm, ENUM_LoggingLevels.LoetProgramm);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.LoetProgrammEditor, ENUM_LoggingLevels.LoetProgrammEditor);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Loeteinheit, ENUM_LoggingLevels.Loeteinheit);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Loetmodul, ENUM_LoggingLevels.Loetmodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.PcbDurchlauf, ENUM_LoggingLevels.PCBDurchlauf);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Protokoll, ENUM_LoggingLevels.Protokoll);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.ProzessSchreiber, ENUM_LoggingLevels.ProzessSchreiber);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Ruecktransportmodul, ENUM_LoggingLevels.Ruecktransportmodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Traceability, ENUM_LoggingLevels.Traceability);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Uebersicht, ENUM_LoggingLevels.Uebersicht);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Vorheizmodul, ENUM_LoggingLevels.Vorheizmodul);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.Warnung, ENUM_LoggingLevels.Warnung);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmShell, ENUM_LoggingLevels.Shell);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmSpsKommunkation, ENUM_LoggingLevels.SpsKommunkation);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmMaschinenModel, ENUM_LoggingLevels.MaschinenModel);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmBenutzerverwaltung, ENUM_LoggingLevels.Benutzerverwaltung);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmUserControls, ENUM_LoggingLevels.UserControls);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmHilfsklassen, ENUM_LoggingLevels.Hilfsklassen);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmMefKontext, ENUM_LoggingLevels.MefKontext);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmPrism, ENUM_LoggingLevels.Prism);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmViewModel, ENUM_LoggingLevels.ViewModel);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmEventSourcing, ENUM_LoggingLevels.EventSourcing);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmMeldungen, ENUM_LoggingLevels.Meldungen);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmSetup, ENUM_LoggingLevels.Setup);
			m_dicLogLevelDllEnums.Add(ENUM_LogLevel.enmCad, ENUM_LoggingLevels.Cad);
			foreach (KeyValuePair<ENUM_LogLevel, ENUM_LoggingLevels> item in from emLokalEnum in m_dicLogLevelDllEnums
			where !m_dicLogLevelLokaleEnums.ContainsKey(emLokalEnum.Value)
			select emLokalEnum)
			{
				m_dicLogLevelLokaleEnums.Add(item.Value, item.Key);
			}
		}
	}
}
