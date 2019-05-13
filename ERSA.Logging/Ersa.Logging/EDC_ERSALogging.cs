using Ersa.Logging.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Ersa.Logging
{
	public class EDC_ERSALogging
	{
		private const string mC_strNameSoftwareProdukt = "ERSA GmbH - Logging";

		private const string mC_strNameDateiDLL = "Ersa.Logging.dll";

		private ENUM_LoggingLevels m_enmLoggingLevelAktuell;

		private string m_strNameDateiMitPfad;

		private string m_strNameFirmaExtern;

		private string m_strNameSoftwareProduktExtern;

		private string m_strVersionExtern;

		private bool m_blnLoggingGestartet;

		private string m_strVersion;

		private bool m_blnLoggingErweitert;

		internal int m_i32LaengeLoggingLevelBeschreibung;

		private bool m_blnLoggingEintraegeVorhanden;

		private dlgLoggingLevelEinstellung m_dlgLoggingLevelEinstellung;

		internal EDC_StringFeldInDatei m_edcStringFeldInDatei;

		public string PRO_strNameFirmaExtern
		{
			get
			{
				return m_strNameFirmaExtern;
			}
			set
			{
				m_strNameFirmaExtern = value;
			}
		}

		public string PRO_strNameSoftwareProduktExtern
		{
			get
			{
				return m_strNameSoftwareProduktExtern;
			}
			set
			{
				m_strNameSoftwareProduktExtern = value;
			}
		}

		public string PRO_strVersionSoftwareProduktExtern
		{
			get
			{
				return m_strVersionExtern;
			}
			set
			{
				m_strVersionExtern = value;
			}
		}

		public string PRO_strVersionLogging => m_strVersion;

		public string PRO_strNameDateiMitPfad
		{
			get
			{
				return m_strNameDateiMitPfad;
			}
			set
			{
				if (LikeOperator.LikeString(value, "*.???", CompareMethod.Binary))
				{
					m_strNameDateiMitPfad = value;
				}
				else
				{
					m_strNameDateiMitPfad = value + ".log";
				}
				if (m_edcStringFeldInDatei != null)
				{
					m_edcStringFeldInDatei.PRO_strNameDateiMitPfad = m_strNameDateiMitPfad;
				}
			}
		}

		public ENUM_LoggingLevels PRO_enmLoggingLevel
		{
			get
			{
				return m_enmLoggingLevelAktuell;
			}
			set
			{
				if (m_blnLoggingErweitert)
				{
					m_enmLoggingLevelAktuell = (value & ENUM_LoggingLevels.Alle);
				}
				else
				{
					m_enmLoggingLevelAktuell = (value & ENUM_LoggingLevels.AlleBasis);
				}
				SUB_LoggingEintragInStringFeldUebernehmen(Strings.LSet("DLL", m_i32LaengeLoggingLevelBeschreibung) + ": " + MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + "(" + Conversions.ToString((ulong)m_enmLoggingLevelAktuell) + ")");
				if (value == ENUM_LoggingLevels.Kein)
				{
					SUB_LoggingBeenden();
				}
			}
		}

		public bool PRO_blnLoggingErweitert
		{
			get
			{
				return m_blnLoggingErweitert;
			}
			set
			{
				m_blnLoggingErweitert = value;
			}
		}

		public bool PRO_blnLoggingEintraegeVorhanden
		{
			get
			{
				return m_blnLoggingEintraegeVorhanden;
			}
			set
			{
				m_blnLoggingEintraegeVorhanden = value;
			}
		}

		public EDC_ERSALogging()
		{
			m_enmLoggingLevelAktuell = ENUM_LoggingLevels.Kein;
			m_strNameDateiMitPfad = string.Empty;
			m_strNameFirmaExtern = "FIRMA";
			m_strNameSoftwareProduktExtern = "SOFTWAREPRODUKT";
			m_strVersionExtern = "VERSION";
			m_blnLoggingGestartet = false;
			m_strVersion = string.Empty;
			m_blnLoggingErweitert = false;
			m_i32LaengeLoggingLevelBeschreibung = int.MinValue;
			m_blnLoggingEintraegeVorhanden = false;
		}

		public bool FUN_blnDateiAktualisieren()
		{
			bool result = default(bool);
			try
			{
				result = false;
				string empty = string.Empty;
				result = m_edcStringFeldInDatei.FUN_blnDateiAktualisieren();
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public bool FUN_blnInit(bool i_blnProgrammStart)
		{
			bool result = default(bool);
			try
			{
				result = false;
				string empty = string.Empty;
				AssemblyName assemblyName = null;
				if (i_blnProgrammStart)
				{
					empty = MyProject.Application.Info.DirectoryPath + "\\Ersa.Logging.dll";
					assemblyName = AssemblyName.GetAssemblyName(empty);
					m_strVersion = Conversions.ToString(assemblyName.Version.Major) + "." + $"{assemblyName.Version.Minor:d3}" + "." + $"{assemblyName.Version.Revision:d3}";
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = Enum.GetValues(typeof(ENUM_LoggingLevels)).GetEnumerator();
						while (enumerator.MoveNext())
						{
							ENUM_LoggingLevels eNUM_LoggingLevels = (ENUM_LoggingLevels)Conversions.ToULong(enumerator.Current);
							if (eNUM_LoggingLevels.ToString().Length > m_i32LaengeLoggingLevelBeschreibung)
							{
								m_i32LaengeLoggingLevelBeschreibung = eNUM_LoggingLevels.ToString().Length;
							}
						}
					}
					finally
					{
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
					m_edcStringFeldInDatei = new EDC_StringFeldInDatei();
					m_edcStringFeldInDatei.m_objLogging = this;
					m_edcStringFeldInDatei.FUN_blnInit(i_blnProgrammStart);
				}
				else
				{
					m_edcStringFeldInDatei.FUN_blnInit(i_blnProgrammStart);
					m_edcStringFeldInDatei = null;
				}
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private void SUB_LoggingEintragBeiLoggingLevel(string i_strLoggingEintrag, ENUM_LoggingLevels i_enmLoggingLevel)
		{
			try
			{
				CheckState r_intCheckStateErgebnis = CheckState.Unchecked;
				FUN_blnLoggingLevelUeberpruefen(i_enmLoggingLevel, m_enmLoggingLevelAktuell, ref r_intCheckStateErgebnis);
				if (r_intCheckStateErgebnis == CheckState.Checked && m_enmLoggingLevelAktuell != ENUM_LoggingLevels.Kein)
				{
					SUB_LoggingEintragInStringFeldUebernehmen(i_strLoggingEintrag);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n--> Level = " + Conversions.ToString(checked((int)i_enmLoggingLevel)) + "\r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
				ProjectData.ClearProjectError();
			}
		}

		private void SUB_LoggingEintragInStringFeldUebernehmen(string i_strLoggingEintrag)
		{
			try
			{
				string empty = string.Empty;
				if (!m_blnLoggingGestartet)
				{
					string empty2 = string.Empty;
					empty2 = "ERSA GmbH - Logging - V" + m_strVersion + " Start " + FUN_strDatumZeitFormatiert() + Strings.Space(1) + "\r\n" + m_strNameFirmaExtern + " - " + m_strNameSoftwareProduktExtern + " - V" + m_strVersionExtern;
					m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("");
					m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("########################################################################");
					m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen(empty2);
					m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("########################################################################");
					m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("");
					m_blnLoggingGestartet = true;
				}
				empty = "{" + FUN_strDatumZeitFormatiert() + "}" + i_strLoggingEintrag;
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen(empty);
				if (m_edcStringFeldInDatei.PRO_u16AnzahlEintraegeImStringFeld > 0)
				{
					PRO_blnLoggingEintraegeVorhanden = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n--> Eintrag = " + i_strLoggingEintrag + "\r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
				ProjectData.ClearProjectError();
			}
		}

		public void SUB_Logging(ENUM_LoggingLevels i_enmLoggingLevel, string i_strLoggingNachricht = null, string i_strNameNamespaceKlasse = null, string i_strNameMethode = null, Exception i_ex = null)
		{
			checked
			{
				try
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					string empty3 = string.Empty;
					string empty4 = string.Empty;
					if (m_enmLoggingLevelAktuell != ENUM_LoggingLevels.Kein)
					{
						empty3 = i_strNameNamespaceKlasse + ": " + i_strNameMethode + Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(i_strLoggingNachricht), string.Empty, " - " + i_strLoggingNachricht));
						if (i_ex != null)
						{
							empty2 = Strings.LSet("#####              Exception", 25 + m_i32LaengeLoggingLevelBeschreibung - 5) + Strings.StrDup(5, "#") + ": ";
							empty4 = Strings.StrDup(25 + m_i32LaengeLoggingLevelBeschreibung, "#");
							i_enmLoggingLevel = ENUM_LoggingLevels.Fehler;
						}
						else
						{
							empty2 = Strings.LSet("*****              Error", 25 + m_i32LaengeLoggingLevelBeschreibung - 5) + Strings.StrDup(5, "*") + ": ";
							empty4 = Strings.StrDup(25 + m_i32LaengeLoggingLevelBeschreibung, "*");
						}
						switch (i_enmLoggingLevel)
						{
						case ENUM_LoggingLevels.Fehler:
							empty = "\r\n" + empty4 + "\r\n" + empty2 + empty3 + "\r\n" + empty4;
							if (i_ex != null)
							{
								empty = empty + empty + "\r\n" + i_ex.ToString();
							}
							break;
						case ENUM_LoggingLevels.Warnung:
							empty = "Warning";
							break;
						case ENUM_LoggingLevels.Info:
							empty = "Info";
							break;
						case ENUM_LoggingLevels.AlleBasis:
							empty = "Basic All";
							break;
						case ENUM_LoggingLevels.Traceability:
							empty = "Traceability";
							break;
						case ENUM_LoggingLevels.Einlauf:
							empty = "Einlauf";
							break;
						case ENUM_LoggingLevels.Einlaufmodul:
							empty = "Einlaufmodul";
							break;
						case ENUM_LoggingLevels.Fluxermodul:
							empty = "Fluxermodul";
							break;
						case ENUM_LoggingLevels.Vorheizmodul:
							empty = "Vorheizmodul";
							break;
						case ENUM_LoggingLevels.Loetmodul:
							empty = "Loetmodul";
							break;
						case ENUM_LoggingLevels.Auslaufmodul:
							empty = "Auslaufmodul";
							break;
						case ENUM_LoggingLevels.Auslauf:
							empty = "Auslauf";
							break;
						case ENUM_LoggingLevels.Protokoll:
							empty = "Protokoll";
							break;
						case ENUM_LoggingLevels.Codebetrieb:
							empty = "Codebetrieb";
							break;
						case ENUM_LoggingLevels.Konfiguration:
							empty = "Konfiguration";
							break;
						case ENUM_LoggingLevels.LoetProgrammEditor:
							empty = "LoetProgrammEditor";
							break;
						case ENUM_LoggingLevels.Datensicherung:
							empty = "Datensicherung";
							break;
						case ENUM_LoggingLevels.ProzessSchreiber:
							empty = "Prozessschreiber";
							break;
						case ENUM_LoggingLevels.PCBDurchlauf:
							empty = "PCBDurchlauf";
							break;
						case ENUM_LoggingLevels.LoetProgramm:
							empty = "LoetProgramm";
							break;
						case ENUM_LoggingLevels.Uebersicht:
							empty = "Uebersicht";
							break;
						case ENUM_LoggingLevels.Basisklasse:
							empty = "Grundmodul";
							break;
						case ENUM_LoggingLevels.Leiterkarte:
							empty = "Leiterkarte";
							break;
						case ENUM_LoggingLevels.Ruecktransportmodul:
							empty = "Ruecktransportmodul";
							break;
						case ENUM_LoggingLevels.Heizung:
							empty = "Heizung";
							break;
						case ENUM_LoggingLevels.Fluxer:
							empty = "Fluxer";
							break;
						case ENUM_LoggingLevels.Loeteinheit:
							empty = "Loeteinheit";
							break;
						case ENUM_LoggingLevels.CNC:
							empty = "CNC";
							break;
						case ENUM_LoggingLevels.AlleErweitert:
							empty = "Alle Erweiterten";
							break;
						case ENUM_LoggingLevels.SpsKommunkation:
							empty = "SpsKommunkation";
							break;
						case ENUM_LoggingLevels.Shell:
							empty = "Shell";
							break;
						case ENUM_LoggingLevels.MaschinenModel:
							empty = "MaschinenModel";
							break;
						case ENUM_LoggingLevels.Benutzerverwaltung:
							empty = "Benutzerverwaltung";
							break;
						case ENUM_LoggingLevels.UserControls:
							empty = "UserControls";
							break;
						case ENUM_LoggingLevels.Hilfsklassen:
							empty = "Hilfsklassen";
							break;
						case ENUM_LoggingLevels.MefKontext:
							empty = "MefKontext";
							break;
						case ENUM_LoggingLevels.Cad:
							empty = "Cad";
							break;
						case ENUM_LoggingLevels.Prism:
							empty = "Prism";
							break;
						case ENUM_LoggingLevels.ViewModel:
							empty = "ViewModel";
							break;
						case ENUM_LoggingLevels.Meldungen:
							empty = "Meldungen";
							break;
						case ENUM_LoggingLevels.EventSourcing:
							empty = "EventSourcing";
							break;
						case ENUM_LoggingLevels.Setup:
							empty = "Setup";
							break;
						default:
							empty = "Mix";
							break;
						}
						if (i_enmLoggingLevel != ENUM_LoggingLevels.Fehler)
						{
							empty = Strings.LSet(empty, m_i32LaengeLoggingLevelBeschreibung) + ": " + empty3;
						}
						SUB_LoggingEintragBeiLoggingLevel(empty, i_enmLoggingLevel);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
					ProjectData.ClearProjectError();
				}
			}
		}

		public void SUB_LoggingLevelSetzenMitFormular(bool i_blnBerechtigung)
		{
			m_dlgLoggingLevelEinstellung = new dlgLoggingLevelEinstellung();
			m_dlgLoggingLevelEinstellung.m_edcLogging = this;
			m_dlgLoggingLevelEinstellung.m_blnLoggingErweitert = m_blnLoggingErweitert;
			checked
			{
				try
				{
					if (m_blnLoggingErweitert)
					{
						m_dlgLoggingLevelEinstellung.Height = 750;
						m_dlgLoggingLevelEinstellung.Width = 800;
						m_dlgLoggingLevelEinstellung.pnlForm.Top = 576;
						m_dlgLoggingLevelEinstellung.grpModule.Visible = true;
						m_dlgLoggingLevelEinstellung.tcbAlleErweitert.Visible = true;
						m_dlgLoggingLevelEinstellung.tcbAlle.Visible = true;
					}
					else
					{
						m_dlgLoggingLevelEinstellung.Height = 750 - (m_dlgLoggingLevelEinstellung.grpModule.Height + 8);
						m_dlgLoggingLevelEinstellung.Width = 736;
						m_dlgLoggingLevelEinstellung.pnlForm.Top = 576 - (m_dlgLoggingLevelEinstellung.grpModule.Height + 8);
						m_dlgLoggingLevelEinstellung.grpModule.Visible = false;
						m_dlgLoggingLevelEinstellung.tcbAlleErweitert.Visible = false;
						m_dlgLoggingLevelEinstellung.tcbAlle.Visible = false;
					}
					m_dlgLoggingLevelEinstellung.m_enmLoggingLevelAktuell = m_enmLoggingLevelAktuell;
					m_dlgLoggingLevelEinstellung.lblNameDateiMitPfadLogging.Text = PRO_strNameDateiMitPfad;
					if (!i_blnBerechtigung)
					{
						m_dlgLoggingLevelEinstellung.btnUebernehmen.Enabled = false;
					}
					if (m_dlgLoggingLevelEinstellung.ShowDialog() == DialogResult.OK)
					{
						SUB_LoggingEintragInStringFeldUebernehmen(Strings.LSet("DLL", m_i32LaengeLoggingLevelBeschreibung) + ": " + MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name);
						PRO_enmLoggingLevel = m_dlgLoggingLevelEinstellung.m_enmLoggingLevelAktuell;
					}
					m_dlgLoggingLevelEinstellung.Close();
					m_dlgLoggingLevelEinstellung.Dispose();
					m_dlgLoggingLevelEinstellung = null;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
					ProjectData.ClearProjectError();
				}
			}
		}

		public void SUB_LoggingBeenden()
		{
			try
			{
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("");
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen(PRO_strNameFirmaExtern + " End   " + FUN_strDatumZeitFormatiert());
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("");
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("========================================================================");
				m_edcStringFeldInDatei.FUN_blnZeilenEintragInFeldAufnehmen("");
				m_edcStringFeldInDatei.FUN_blnDateiAktualisieren();
				m_blnLoggingGestartet = false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + PRO_strNameDateiMitPfad);
				ProjectData.ClearProjectError();
			}
		}

		public static bool FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels i_enmLoggingLevelAngefragt, ENUM_LoggingLevels i_enmLoggingLevelAktuell, ref CheckState r_intCheckStateErgebnis)
		{
			bool result = default(bool);
			try
			{
				result = false;
				string empty = string.Empty;
				ENUM_LoggingLevels eNUM_LoggingLevels = ENUM_LoggingLevels.Kein;
				r_intCheckStateErgebnis = CheckState.Unchecked;
				eNUM_LoggingLevels = (i_enmLoggingLevelAktuell & i_enmLoggingLevelAngefragt);
				if (i_enmLoggingLevelAngefragt == ENUM_LoggingLevels.Kein)
				{
					if (i_enmLoggingLevelAngefragt == i_enmLoggingLevelAktuell)
					{
						r_intCheckStateErgebnis = CheckState.Checked;
					}
				}
				else if (eNUM_LoggingLevels == i_enmLoggingLevelAngefragt)
				{
					r_intCheckStateErgebnis = CheckState.Checked;
				}
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string FUN_strDatumZeitFormatiert()
		{
			try
			{
				return $"{DateAndTime.Now.Year:d4}" + "-" + $"{DateAndTime.Now.Month:d2}" + "-" + $"{DateAndTime.Now.Day:d2}" + " " + $"{DateAndTime.Now.Hour:d2}" + ":" + $"{DateAndTime.Now.Minute:d2}" + ":" + $"{DateAndTime.Now.Second:d2}" + ":" + $"{DateAndTime.Now.Millisecond:d3}";
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = $"{DateTime.MinValue.Year:d4}" + "-" + $"{DateTime.MinValue.Month:d2}" + "-" + $"{DateTime.MinValue.Day:d2}" + " " + $"{DateTime.MinValue.Hour:d2}" + ":" + $"{DateTime.MinValue.Minute:d2}" + ":" + $"{DateTime.MinValue.Second:d2}" + ":" + $"{DateTime.MinValue.Millisecond:d3}";
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void SUB_LoggingMitErrorProvider(ref ErrorProvider r_objErrorProvider, ref Control r_ctlContolBeliebig, bool i_blnMessageBoxAnzeigen = false, string i_strLoggingNachricht = null, ref EDC_ERSALogging r_objLogging = default(ref EDC_ERSALogging), ref ENUM_LoggingLevels r_enmLoggingLevel = 0uL, string i_strNameNamespaceKlasse = null, string i_strNameMethode = null)
		{
			try
			{
				if (Operators.CompareString(i_strNameNamespaceKlasse, string.Empty, TextCompare: false) == 0)
				{
					i_strNameNamespaceKlasse = MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name;
				}
				r_objErrorProvider.SetError(r_ctlContolBeliebig, i_strLoggingNachricht);
				if (i_blnMessageBoxAnzeigen)
				{
					Interaction.MsgBox(i_strLoggingNachricht, MsgBoxStyle.OkOnly, "Error: ");
				}
				if (r_enmLoggingLevel != ENUM_LoggingLevels.Kein && r_objLogging != null)
				{
					r_objLogging.SUB_Logging(r_enmLoggingLevel, i_strLoggingNachricht, i_strNameNamespaceKlasse, i_strNameMethode);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
			}
		}
	}
}
