using Ersa.Logging.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ersa.Logging
{
	internal class EDC_StringFeldInDatei
	{
		public enum ENUM_StringFelder : byte
		{
			Kein,
			StringFeld1,
			StringFeld2
		}

		private const ushort mC_u16MaxPufferGroesse = 600;

		private const ushort mC_u16MaxMinutenBisDatenUebernahme = 10;

		private string[] ma_strFeld1;

		private string[] ma_strFeld2;

		private TimeSpan m_tspZeitBisDatenUebernahme;

		private DateTime m_dtmStartZeit;

		private ushort m_u16NaechsterFreierFeldIndexPufferAktiv;

		private ushort m_u16NaechsterFreierFeldIndexPufferSchreiben;

		private object m_objLock;

		private ENUM_StringFelder m_enmStringFeldAktiv;

		private ENUM_StringFelder m_enmStringFeldSchreiben;

		private string m_strNameDateiMitPfad;

		internal EDC_ERSALogging m_objLogging;

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
			}
		}

		public ushort PRO_u16AnzahlEintraegeImStringFeld => m_u16NaechsterFreierFeldIndexPufferAktiv;

		public EDC_StringFeldInDatei()
		{
			m_tspZeitBisDatenUebernahme = TimeSpan.Zero;
			m_dtmStartZeit = DateTime.Now;
			m_u16NaechsterFreierFeldIndexPufferAktiv = 0;
			m_u16NaechsterFreierFeldIndexPufferSchreiben = 0;
			m_objLock = RuntimeHelpers.GetObjectValue(new object());
			m_enmStringFeldAktiv = ENUM_StringFelder.Kein;
			m_enmStringFeldSchreiben = ENUM_StringFelder.Kein;
			m_strNameDateiMitPfad = string.Empty;
		}

		public bool FUN_blnInit(bool i_blnProgrammStart)
		{
			bool result = default(bool);
			try
			{
				result = false;
				string empty = string.Empty;
				ushort num = 0;
				if (!i_blnProgrammStart)
				{
					if (m_u16NaechsterFreierFeldIndexPufferAktiv <= 0)
					{
						result = true;
						return result;
					}
					result = FUN_blnDateiAktualisieren();
					return result;
				}
				ma_strFeld1 = new string[601];
				ma_strFeld2 = new string[601];
				num = 0;
				do
				{
					ma_strFeld1[num] = string.Empty;
					ma_strFeld2[num] = string.Empty;
					checked
					{
						num = (ushort)unchecked((uint)(num + 1));
					}
				}
				while ((uint)num <= 600u);
				m_enmStringFeldAktiv = ENUM_StringFelder.StringFeld1;
				m_enmStringFeldSchreiben = ENUM_StringFelder.StringFeld1;
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

        /// <summary>
        /// update file
        /// </summary>
        /// <returns></returns>
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public bool FUN_blnDateiAktualisieren()
		{
			try
			{
				object objLock = m_objLock;
				ObjectFlowControl.CheckForSyncLockOnValueType(objLock);
				Monitor.Enter(objLock);
				try
				{
					bool flag = false;
					string empty = string.Empty;
					ushort num = 0;
					m_dtmStartZeit = DateAndTime.Now;
					if (string.IsNullOrEmpty(PRO_strNameDateiMitPfad))
					{
						PRO_strNameDateiMitPfad = MyProject.Application.Info.DirectoryPath + "\\Logging\\ERSAsoftStandard.log";
					}
					if (m_enmStringFeldAktiv == m_enmStringFeldSchreiben)
					{
						bool result = default(bool);
						if (m_u16NaechsterFreierFeldIndexPufferAktiv < 1)
						{
							return result;
						}
						m_u16NaechsterFreierFeldIndexPufferSchreiben = m_u16NaechsterFreierFeldIndexPufferAktiv;
						m_u16NaechsterFreierFeldIndexPufferAktiv = 0;
						m_objLogging.PRO_blnLoggingEintraegeVorhanden = false;
						switch (m_enmStringFeldAktiv)
						{
						case ENUM_StringFelder.StringFeld1:
							m_enmStringFeldAktiv = ENUM_StringFelder.StringFeld2;
							break;
						case ENUM_StringFelder.StringFeld2:
							m_enmStringFeldAktiv = ENUM_StringFelder.StringFeld1;
							break;
						default:
							empty = "'Select Case'-Anweisung: " + MethodBase.GetCurrentMethod().Name + ": " + m_enmStringFeldAktiv.ToString() + " --> Nicht definiert";
							Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + empty, MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
							break;
						}
					}
					else if ((uint)m_u16NaechsterFreierFeldIndexPufferAktiv >= 600u)
					{
						empty = "String-Array zum Schreiben ist nicht leer (Schreiben schlug fehl)\r\nund String-Array zum Befuellen ist bereits voll.";
						Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + empty, MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
					}
					StreamWriter streamWriter = new StreamWriter(PRO_strNameDateiMitPfad, append: true);
					empty = "{" + EDC_ERSALogging.FUN_strDatumZeitFormatiert() + "}" + Strings.LSet("DLL", m_objLogging.m_i32LaengeLoggingLevelBeschreibung) + ": " + MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + ": " + MethodBase.GetCurrentMethod().Name + " - Aufruf starten: Schreibe aktives 'String-Array zum Schreiben' in Datei --> aktives 'String-Array zum Befuellen' = " + m_enmStringFeldAktiv.ToString() + " / aktives 'String-Array zum Schreiben' = " + m_enmStringFeldSchreiben.ToString();
					streamWriter.WriteLine(empty);
					checked
					{
						switch (m_enmStringFeldSchreiben)
						{
						case ENUM_StringFelder.StringFeld1:
						{
							ushort num3 = (ushort)unchecked((uint)(m_u16NaechsterFreierFeldIndexPufferSchreiben - 1));
							num = 0;
							while (unchecked((uint)num <= (uint)num3))
							{
								streamWriter.WriteLine(ma_strFeld1[num]);
								ma_strFeld1[num] = string.Empty;
								num = (ushort)unchecked((uint)(num + 1));
							}
							m_enmStringFeldSchreiben = m_enmStringFeldAktiv;
							break;
						}
						case ENUM_StringFelder.StringFeld2:
						{
							ushort num2 = (ushort)unchecked((uint)(m_u16NaechsterFreierFeldIndexPufferSchreiben - 1));
							num = 0;
							while (unchecked((uint)num <= (uint)num2))
							{
								streamWriter.WriteLine(ma_strFeld2[num]);
								ma_strFeld2[num] = string.Empty;
								num = (ushort)unchecked((uint)(num + 1));
							}
							m_enmStringFeldSchreiben = m_enmStringFeldAktiv;
							break;
						}
						default:
							empty = "'Select Case'-Anweisung: " + MethodBase.GetCurrentMethod().Name + ": " + m_enmStringFeldSchreiben.ToString() + " --> Nicht definiert";
							Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + empty, MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
							break;
						}
						empty = "{" + EDC_ERSALogging.FUN_strDatumZeitFormatiert() + "}" + Strings.LSet("DLL", m_objLogging.m_i32LaengeLoggingLevelBeschreibung) + ": " + MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + ": " + MethodBase.GetCurrentMethod().Name + " - Aufruf erfolgreich beendet: Schreibe aktives 'String-Array zum Schreiben' in Datei --> aktives 'String-Array zum Befuellen' = " + m_enmStringFeldAktiv.ToString() + " / aktives 'String-Array zum Schreiben' = " + m_enmStringFeldSchreiben.ToString();
						streamWriter.WriteLine(empty);
						streamWriter.Close();
						streamWriter.Dispose();
						streamWriter = null;
					}
				}
				finally
				{
					Monitor.Exit(objLock);
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				bool flag = false;
				ProjectData.ClearProjectError();
				return flag;
			}
		}

		public bool FUN_blnZeilenEintragInFeldAufnehmen(string i_strZeilenEintrag)
		{
			bool flag = default(bool);
			try
			{
				flag = false;
				string empty = string.Empty;
				ushort num = 0;
				m_tspZeitBisDatenUebernahme = TimeSpan.Zero;
				m_tspZeitBisDatenUebernahme = DateAndTime.Now.Subtract(m_dtmStartZeit);
				flag = (((uint)m_u16NaechsterFreierFeldIndexPufferAktiv < 600u && !(m_tspZeitBisDatenUebernahme.TotalMinutes >= 10.0) && m_enmStringFeldAktiv == m_enmStringFeldSchreiben) || (flag & FUN_blnDateiAktualisieren()));
				checked
				{
					switch (m_enmStringFeldAktiv)
					{
					case ENUM_StringFelder.StringFeld1:
					{
						object syncRoot2 = ma_strFeld1.SyncRoot;
						ObjectFlowControl.CheckForSyncLockOnValueType(syncRoot2);
						Monitor.Enter(syncRoot2);
						try
						{
							ma_strFeld1[m_u16NaechsterFreierFeldIndexPufferAktiv] = i_strZeilenEintrag;
							m_u16NaechsterFreierFeldIndexPufferAktiv = (ushort)unchecked((uint)(m_u16NaechsterFreierFeldIndexPufferAktiv + 1));
							return flag;
						}
						finally
						{
							Monitor.Exit(syncRoot2);
						}
					}
					case ENUM_StringFelder.StringFeld2:
					{
						object syncRoot = ma_strFeld2.SyncRoot;
						ObjectFlowControl.CheckForSyncLockOnValueType(syncRoot);
						Monitor.Enter(syncRoot);
						try
						{
							ma_strFeld2[m_u16NaechsterFreierFeldIndexPufferAktiv] = i_strZeilenEintrag;
							m_u16NaechsterFreierFeldIndexPufferAktiv = (ushort)unchecked((uint)(m_u16NaechsterFreierFeldIndexPufferAktiv + 1));
							return flag;
						}
						finally
						{
							Monitor.Exit(syncRoot);
						}
					}
					default:
						return flag;
					}
				}
				IL_00ce:
				return flag;
				IL_0118:
				return flag;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
				return flag;
			}
		}

		public bool FUN_blnNameNeueDateiUebernehmen(string i_strNameNeueDateiMitPfad)
		{
			bool result = default(bool);
			try
			{
				result = false;
				result = FUN_blnDateiAktualisieren();
				PRO_strNameDateiMitPfad = i_strNameNeueDateiMitPfad;
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

		public bool FUN_blnStringFeldExportieren(ref string[] ra_strFeld)
		{
			checked
			{
				bool result = default(bool);
				try
				{
					result = false;
					if (m_u16NaechsterFreierFeldIndexPufferAktiv > 0)
					{
						int num = Information.UBound(ra_strFeld);
						ra_strFeld = (string[])Utils.CopyArray(ra_strFeld, new string[num + unchecked((int)m_u16NaechsterFreierFeldIndexPufferAktiv) + 1]);
						ushort num2 = (ushort)unchecked((uint)(m_u16NaechsterFreierFeldIndexPufferAktiv - 1));
						ushort num3 = 0;
						while (unchecked((uint)num3 <= (uint)num2))
						{
							switch (m_enmStringFeldAktiv)
							{
							case ENUM_StringFelder.StringFeld1:
								ra_strFeld[num + 1 + unchecked((int)num3)] = ma_strFeld1[num3];
								break;
							case ENUM_StringFelder.StringFeld2:
								ra_strFeld[num + 1 + unchecked((int)num3)] = ma_strFeld2[num3];
								break;
							}
							num3 = (ushort)unchecked((uint)(num3 + 1));
						}
						result = true;
						return result;
					}
					bool result2 = default(bool);
					return result2;
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
		}
	}
}
