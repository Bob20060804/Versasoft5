using Ersa.Global.Common.Extensions;
using Ersa.Global.Common.Helper;
using Ersa.Global.Controls.Dialoge;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.Common.Meldungen;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Konfiguration;
using Ersa.Platform.Mes.Modell;
using Ersa.Platform.UI.Common.Helfer;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ersa.Platform.Mes.Dienste
{
    /// <summary>
	/// MES 服务
    /// MES Service
    /// </summary>
	[Export(typeof(INF_MesDienst))]
	internal class EDC_MesDienst : INF_MesDienst, INF_MeldungProzessor
	{
		private const int mC_i32MeldungsOrt3Initialisierung = 12118;

		private const int mC_i32MeldungsOrt3Deinitialisierung = 12125;

		private const int mC_i32MeldungsOrt3IstVerbunden = 12126;

		private const int mC_i32MeldungsOrt3IstFunktionAktiv = 12127;

		private const int mC_i32MeldungsOrt3IstFunktionUndMesAktiv = 12128;

		private const int mC_i32MeldungsOrt3LoetprotokollEinzelnSenden = 12166;

		private const int mC_i32LokalisierungBestaetigungJa = 12173;

		private const int mC_i32LokalisierungBestaetigungNein = 12174;

		private const int mC_i32FehlerKeyInitMesDeaktiviert = 12122;

		private const int mC_i32FehlerKeyInitMesNichtKonfiguriert = 12123;

		private const int mC_i32FehlerKeyKommDienstUngueltig = 12117;

		private const int mC_i32FehlerKeyUnerwarteterFehler = 12124;

		private const int mC_i32MaschinendatenUngueltig = 12161;

		private const int mC_i32FehlerKeyKommMesDeaktiviert = 12155;

		private const int mC_i32FehlerKeyKommMesNichtKonfiguriert = 12156;

		private const int mC_i32FehlerKeyKommFunktionNichtAktiv = 12157;

		private static readonly SemaphoreSlim ms_fdcDienstzugriffSemaphore = new SemaphoreSlim(1);

		private readonly INF_Logger m_edcLogger;

		private readonly INF_MesKonfigurationsManager m_edcKonfigurationsManager;

		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		private readonly INF_MesArgumenteValidierungsHelfer m_edcMesArgumenteValidierungsHelfer;

		private readonly INF_JsonSerialisierungsDienst m_edcJsonSerialisierungsDienst;

		private readonly string m_strNamespaceName;

		private readonly string m_strKlassenName;

		private readonly DispatcherTimer m_fdcPingTimer = new DispatcherTimer();

		private string m_strLokalisierungsKeyMesTyp;

		private int m_i32PingFehlerNr;

		[Import(typeof(INF_MeldungHinzufuegen))]
		public INF_MeldungHinzufuegen PRO_edcMeldungHinzufuegen
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="i_edcLogger">日期</param>
		/// <param name="i_edcKonfigurationsManager">Configuration manager</param>
		/// <param name="i_fdcEventAggregator">prism 事件聚合器</param>
		/// <param name="i_edcLokalisierungsDienst">本地化服务</param>
		/// <param name="i_edcMesArgumenteValidierungsHelfer">MES参数验证帮助</param>
		/// <param name="i_edcJsonSerialisierungsDienst">Json序列化服务</param>
		[ImportingConstructor]
		public EDC_MesDienst(INF_Logger i_edcLogger, INF_MesKonfigurationsManager i_edcKonfigurationsManager, IEventAggregator i_fdcEventAggregator, INF_LokalisierungsDienst i_edcLokalisierungsDienst, INF_MesArgumenteValidierungsHelfer i_edcMesArgumenteValidierungsHelfer, INF_JsonSerialisierungsDienst i_edcJsonSerialisierungsDienst)
		{
			m_edcLogger = i_edcLogger;
			m_edcKonfigurationsManager = i_edcKonfigurationsManager;
			m_strNamespaceName = this.FUN_strGibNameSpace();
			m_strKlassenName = this.FUN_strGibKlassenName();
			m_fdcEventAggregator = i_fdcEventAggregator;
			m_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
			m_edcMesArgumenteValidierungsHelfer = i_edcMesArgumenteValidierungsHelfer;
			m_edcJsonSerialisierungsDienst = i_edcJsonSerialisierungsDienst;
			Action<List<EDC_SoftwareFeatureGeaendertPayload>> action = async delegate(List<EDC_SoftwareFeatureGeaendertPayload> i_edcFeature)
			{
				await FUN_fdcSoftwareFeatureGeaendertAsync(i_edcFeature).ConfigureAwait(continueOnCapturedContext: false);
			};
			m_fdcEventAggregator.GetEvent<EDC_SoftwareFeatureGeaendertEvent>().Subscribe(action);
			m_fdcEventAggregator.GetEvent<EDC_ProduktionssteuerungGeaendertEvent>().Subscribe(async delegate
			{
				await FUN_fdcProduktionssteuerungGeaendertAsync().ConfigureAwait(continueOnCapturedContext: false);
			});
		}

		/// <summary>
		/// 确认消息 异步
		/// </summary>
		/// <param name="i_edcMessage">Message</param>
		/// <returns></returns>
		public async Task<bool> FUN_fdcAcknowledgeMessageAsync(INF_Meldung i_edcMessage)
		{
			EDC_EnumMember eDC_EnumMember = EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(typeof(ENUM_MesFunktionen)).Single((EDC_EnumMember i_edcEnumMember) => i_edcEnumMember.PRO_i32Value == 14);
			string strPcbProzessParameterSenden = eDC_EnumMember.PRO_strDescription;
			if (eDC_EnumMember.PRO_strDescription.Contains("_"))
			{
				strPcbProzessParameterSenden = eDC_EnumMember.PRO_strDescription.Split('_')[1];
			}
			if (i_edcMessage.PRO_i32MeldungsOrt3 == 12118)
			{
				await FUN_fdcInitialisiereAsync();
			}
			if (i_edcMessage.PRO_i32MeldungsOrt3 == Convert.ToInt32(strPcbProzessParameterSenden))
			{
				EDU_BenutzerAbfrageDialog eduDialog = null;
				bool? blnDialogResult = null;
				await EDC_Dispatch.FUN_fdcAwaitableAktion(delegate
				{
					eduDialog = new EDU_BenutzerAbfrageDialog(Application.Current.MainWindow)
					{
						PRO_strText = m_edcLokalisierungsDienst.FUN_strText("18_295"),
						PRO_strBestaetigenPositivText = m_edcLokalisierungsDienst.FUN_strText("13_80"),
						PRO_strBestaetigenNegativText = m_edcLokalisierungsDienst.FUN_strText("13_79"),
						PRO_blnIstNegativeAuswahlSichtbar = true,
						PRO_blnIstAbbrechenSichtbar = false
					};
					blnDialogResult = eduDialog.ShowDialog();
				});
				EDC_MesMeldungContext edcMesMeldungContext = m_edcJsonSerialisierungsDienst.FUN_objDeserialisieren<EDC_MesMeldungContext>(i_edcMessage.PRO_strContext);
				if (blnDialogResult.HasValue && !eduDialog.PRO_blnWurdeNegativBestatigt)
				{
					await FUN_fdcFunktionAufrufenAsync(edcMesMeldungContext.PRO_enmMesFunktion, edcMesMeldungContext.PRO_edcMesMaschinenDaten).ConfigureAwait(continueOnCapturedContext: false);
				}
				string i_strMeldungsDetails = string.Format(m_edcLokalisierungsDienst.FUN_strText("18_296"), i_edcMessage.PRO_strMeldungGuid, eduDialog.PRO_blnWurdeNegativBestatigt);
				await FUN_fdcExterneMeldungHinzufuegenAsync(i_edcMessage.PRO_i32MeldungsOrt3, eduDialog.PRO_blnWurdeNegativBestatigt ? 12174 : 12173, i_strMeldungsDetails, edcMesMeldungContext, i_blnEinlaufSperreAktiv: false, i_blnDuplicateAllowed: true, ENUM_MeldungAktionen.Loggen);
				return true;
			}
			return true;
		}

		public Task<bool> FUN_fdcResetMessageAsync(INF_Meldung i_edcMessage)
		{
			return Task.FromResult(result: true);
		}

		/// <summary>
		/// Initialize
		/// </summary>
		/// <returns></returns>
		public async Task<EDC_MesInitialisierungsRueckgabe> FUN_fdcInitialisiereAsync()
		{
			try
			{
				await FUN_fdcDeinitialisiereAsync().ConfigureAwait(false);
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(false);
				await m_edcKonfigurationsManager.FUN_fdcInitialisierenAsync().ConfigureAwait(false);
				if (!(await m_edcKonfigurationsManager.FUN_fdcIstMesAktivAsync().ConfigureAwait(false)))
				{
					EDC_MesInitialisierungsRueckgabe eDC_MesInitialisierungsRueckgabe = new EDC_MesInitialisierungsRueckgabe().FUN_edcRueckgabe(ENUM_MesInitialisierungsStatus.MesDeaktiviert, 12122);
					SUB_LogEintragSchreiben("Initialization returned: ", eDC_MesInitialisierungsRueckgabe, null, "FUN_fdcInitialisiereAsync");
					return eDC_MesInitialisierungsRueckgabe;
				}
				if (!(await m_edcKonfigurationsManager.FUN_fdcIstMesKonfiguriertAsync().ConfigureAwait(continueOnCapturedContext: false)))
				{
					EDC_MesInitialisierungsRueckgabe eDC_MesInitialisierungsRueckgabe2 = new EDC_MesInitialisierungsRueckgabe().FUN_edcRueckgabe(ENUM_MesInitialisierungsStatus.MesNichtKonfiguriert, 12123);
					SUB_LogEintragSchreiben("Initialization returned: ", eDC_MesInitialisierungsRueckgabe2, null, "FUN_fdcInitialisiereAsync");
					return eDC_MesInitialisierungsRueckgabe2;
				}
				_003C_003Ec__DisplayClass35_0 _003C_003Ec__DisplayClass35_;
				EDC_MesKonfiguration edcKonfiguration2 = _003C_003Ec__DisplayClass35_.edcKonfiguration;
				EDC_MesKonfiguration edcKonfiguration = await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
				SUB_LogEintragSchreiben("Communication type: ", edcKonfiguration.PRO_enuMesTyp.ToString(), "FUN_fdcInitialisiereAsync");
				EDC_EnumMember eDC_EnumMember = EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(typeof(ENUM_MesTyp)).Single((EDC_EnumMember i_edcEnumMember) => i_edcEnumMember.PRO_i32Value == (int)edcKonfiguration.PRO_enuMesTyp);
				m_strLokalisierungsKeyMesTyp = eDC_EnumMember.PRO_strDescription;
				if (eDC_EnumMember.PRO_strDescription.Contains("_"))
				{
					m_strLokalisierungsKeyMesTyp = eDC_EnumMember.PRO_strDescription.Split('_')[1];
				}
				INF_MesKommunikationsDienst edcDienst = await m_edcKonfigurationsManager.FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (edcDienst == null)
				{
					SUB_LogEintragSchreiben("Communication service:", "null", "FUN_fdcInitialisiereAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(12118, 12117);
					return new EDC_MesInitialisierungsRueckgabe().FUN_edcRueckgabeFehlerhaft(12117);
				}
				SUB_LogEintragSchreiben("Communication service:", edcDienst.ToString(), "FUN_fdcInitialisiereAsync");
				SUB_LogEintragSchreiben("Function exit enabled: ", edcKonfiguration.PRO_blnIstFunctionExitAktiv.ToString(), "FUN_fdcInitialisiereAsync");
				EDC_MesInitialisierungsRueckgabe edcInitRueckgabe2 = await edcDienst.FUN_fdcInitialisiereAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (edcInitRueckgabe2.PRO_enuInitialisierungsStatus != 0)
				{
					SUB_LogEintragSchreiben("Initialization failed: ", edcInitRueckgabe2, null, "FUN_fdcInitialisiereAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(12118, edcInitRueckgabe2.PRO_i32FehlerKey, edcInitRueckgabe2.PRO_strFehlerErweiterung);
					return edcInitRueckgabe2;
				}
				EDC_MesTypEinstellung eDC_MesTypEinstellung = await edcDienst.FUN_fdcHoleDienstEinstellungenAsync<EDC_MesTypEinstellung>(edcKonfiguration.PRO_enuMesTyp, null).ConfigureAwait(continueOnCapturedContext: false);
				if (edcKonfiguration[ENUM_MesFunktionen.PingSenden] != null && edcKonfiguration[ENUM_MesFunktionen.PingSenden].PRO_blnIstAktiv)
				{
					m_fdcPingTimer.Interval = new TimeSpan(0, 0, Convert.ToInt32(eDC_MesTypEinstellung.PRO_dblPingIntervall));
					m_fdcPingTimer.Tick += async delegate(object i_objSources, EventArgs i_fdcEventArgs)
					{
						await FUN_blnPingSenden(i_objSources, i_fdcEventArgs).ConfigureAwait(continueOnCapturedContext: false);
					};
					m_fdcPingTimer.Start();
				}
				edcInitRueckgabe2 = new EDC_MesInitialisierungsRueckgabe().FUN_edcRueckgabeErfolgreich();
				SUB_LogEintragSchreiben("Initialization returned: ", edcInitRueckgabe2, null, "FUN_fdcInitialisiereAsync");
				SUB_StatusGeaendertVeroeffentlichen(ENUM_VerbindungsStatus.Verbunden);
				return edcInitRueckgabe2;
			}
			catch (Exception fdcException)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred during initialization: ", fdcException.ToString(), "FUN_fdcInitialisiereAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12118, 12124, fdcException.ToString(), null, i_blnEinlaufSperreAktiv: true);
				return new EDC_MesInitialisierungsRueckgabe().FUN_edcRueckgabeFehlerhaft(12124, fdcException.Message, fdcException.ToString());
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
				SUB_LogdateiAktualisieren();
			}
		}

        /// <summary>
        /// 取消初始化 异步
        /// deinitialize
        /// </summary>
        /// <returns></returns>
		public async Task FUN_fdcDeinitialisiereAsync()
		{
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				INF_MesKommunikationsDienst iNF_MesKommunikationsDienst = await m_edcKonfigurationsManager.FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync().ConfigureAwait(continueOnCapturedContext: false);
				m_fdcPingTimer.Stop();
				if (iNF_MesKommunikationsDienst != null && iNF_MesKommunikationsDienst.PRO_blnIstVerbunden)
				{
					EDC_MesInitialisierungsRueckgabe eDC_MesInitialisierungsRueckgabe = await iNF_MesKommunikationsDienst.FUN_fdcDeinitialisiereAsync().ConfigureAwait(continueOnCapturedContext: false);
					if (eDC_MesInitialisierungsRueckgabe.PRO_enuInitialisierungsStatus != 0)
					{
						SUB_LogEintragSchreiben("Deinitialization failed: ", eDC_MesInitialisierungsRueckgabe, null, "FUN_fdcDeinitialisiereAsync");
						await FUN_fdcExterneMeldungHinzufuegenAsync(12125, eDC_MesInitialisierungsRueckgabe.PRO_i32FehlerKey, eDC_MesInitialisierungsRueckgabe.PRO_strFehlerErweiterung);
					}
					SUB_StatusGeaendertVeroeffentlichen(ENUM_VerbindungsStatus.NichtVerbunden);
				}
			}
			catch (Exception ex)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred during deinitialization: ", ex.ToString(), "FUN_fdcDeinitialisiereAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12125, 12124, ex.ToString(), null, i_blnEinlaufSperreAktiv: true);
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
				SUB_LogdateiAktualisieren();
			}
		}

		public async Task<bool> FUN_fdcIstMesSystemVerbundenAsync()
		{
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				return (await m_edcKonfigurationsManager.FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync().ConfigureAwait(continueOnCapturedContext: false)).PRO_blnIstVerbunden;
			}
			catch (Exception ex)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred: ", ex.ToString(), "FUN_fdcIstMesSystemVerbundenAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12126, 12124, ex.ToString(), null, i_blnEinlaufSperreAktiv: true);
				return false;
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
			}
		}

		public async Task<bool> FUN_fdcIstFunktionAktivAsync(ENUM_MesFunktionen i_enuFunktion)
		{
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				EDC_MesKonfiguration eDC_MesKonfiguration = await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
				return eDC_MesKonfiguration[i_enuFunktion] != null && eDC_MesKonfiguration[i_enuFunktion].PRO_blnIstAktiv;
			}
			catch (Exception ex)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred: ", ex.ToString(), "FUN_fdcIstFunktionAktivAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12127, 12124, ex.ToString(), null, i_blnEinlaufSperreAktiv: true);
				return false;
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
			}
		}

		public async Task<bool> FUN_fdcIstFunktionUndMesAktivAsync(ENUM_MesFunktionen i_enuFunktion)
		{
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				bool blnIstMesAktiv = await m_edcKonfigurationsManager.FUN_fdcIstMesAktivAsync().ConfigureAwait(continueOnCapturedContext: false);
				EDC_MesKonfiguration eDC_MesKonfiguration = await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false);
				return blnIstMesAktiv && eDC_MesKonfiguration[i_enuFunktion] != null && eDC_MesKonfiguration[i_enuFunktion].PRO_blnIstAktiv;
			}
			catch (Exception ex)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred: ", ex.ToString(), "FUN_fdcIstFunktionUndMesAktivAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12128, 12124, ex.ToString(), null, i_blnEinlaufSperreAktiv: true);
				return false;
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
			}
		}

		public async Task<bool> FUN_fdcLoetprotokollEinzelnSendenAsync()
		{
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				if ((await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false)).PRO_enuMesTyp == ENUM_MesTyp.Zvei)
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred: ", ex.ToString(), "FUN_fdcLoetprotokollEinzelnSendenAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(12166, 12124, ex.ToString(), null, i_blnEinlaufSperreAktiv: true);
				return false;
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
			}
		}

        /// <summary>
        /// 调用方法
        /// Call function
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <param name="i_edcMaschinenDaten"></param>
        /// <returns></returns>
		public async Task<EDC_MesKommunikationsRueckgabe> FUN_fdcFunktionAufrufenAsync(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten)
		{
			string strFunktionKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_enuFunktion).Remove(0, 2);
			try
			{
				await ms_fdcDienstzugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				EDC_MesKommunikationsRueckgabe edcRueckgabe = new EDC_MesKommunikationsRueckgabe();
				try
				{
					m_edcMesArgumenteValidierungsHelfer.SUB_PruefeArgumente(i_enuFunktion, i_edcMaschinenDaten);
				}
				catch (ArgumentException fdcArgumentException)
				{
					SUB_LogEintragSchreiben("Function NOT called: ", edcRueckgabe, i_edcMaschinenDaten, "FUN_fdcFunktionAufrufenAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), 12161, fdcArgumentException.ToString(), null, i_blnEinlaufSperreAktiv: true);
					return new EDC_MesKommunikationsRueckgabe().FUN_edcRueckgabeFehlerhaft(12161, fdcArgumentException.Message, fdcArgumentException.StackTrace);
				}
				if (!(await m_edcKonfigurationsManager.FUN_fdcIstMesAktivAsync().ConfigureAwait(false)))
				{
					SUB_LogEintragSchreiben("Function NOT called: ", edcRueckgabe, i_edcMaschinenDaten, "FUN_fdcFunktionAufrufenAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), 12155, string.Empty, null, i_blnEinlaufSperreAktiv: true);
					return new EDC_MesKommunikationsRueckgabe().FUN_edcRueckgabe(ENUM_MesKommunikationsStatus.MesDeaktiviert, 12155);
				}
				if (!(await m_edcKonfigurationsManager.FUN_fdcIstMesKonfiguriertAsync().ConfigureAwait(false)))
				{
					SUB_LogEintragSchreiben("Function NOT called: ", edcRueckgabe, i_edcMaschinenDaten, "FUN_fdcFunktionAufrufenAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), 12156, string.Empty, null, i_blnEinlaufSperreAktiv: true);
					return new EDC_MesKommunikationsRueckgabe().FUN_edcRueckgabe(ENUM_MesKommunikationsStatus.MesNichtKonfiguriert, 12156);
				}
				if (!(await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(false))[i_enuFunktion].PRO_blnIstAktiv)
				{
					SUB_LogEintragSchreiben("Function NOT called: ", edcRueckgabe, i_edcMaschinenDaten, "FUN_fdcFunktionAufrufenAsync");
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), 12157, string.Empty, null, i_blnEinlaufSperreAktiv: true);
					return new EDC_MesKommunikationsRueckgabe().FUN_edcRueckgabe(ENUM_MesKommunikationsStatus.FunktionDeaktiviert, 12157);
				}
				edcRueckgabe = await(await m_edcKonfigurationsManager.FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync().ConfigureAwait(continueOnCapturedContext: false)).FUN_fdcNachrichtSendenAsync(i_enuFunktion, i_edcMaschinenDaten).ConfigureAwait(continueOnCapturedContext: false);
				SUB_LogEintragSchreiben("Function called: ", edcRueckgabe, i_edcMaschinenDaten, "FUN_fdcFunktionAufrufenAsync");
				if (edcRueckgabe.PRO_enuKommunikationsStatus == ENUM_MesKommunikationsStatus.Fehlerhaft && i_enuFunktion == ENUM_MesFunktionen.PingSenden)
				{
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), edcRueckgabe.PRO_i32FehlerKey, edcRueckgabe.PRO_strFehlerErweiterung, new EDC_MesMeldungContext(i_enuFunktion, i_edcMaschinenDaten), i_blnEinlaufSperreAktiv: true, i_blnDuplicateAllowed: false);
				}
				else if (edcRueckgabe.PRO_enuKommunikationsStatus == ENUM_MesKommunikationsStatus.Fehlerhaft)
				{
					await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), edcRueckgabe.PRO_i32FehlerKey, edcRueckgabe.PRO_strFehlerErweiterung, new EDC_MesMeldungContext(i_enuFunktion, i_edcMaschinenDaten), i_blnEinlaufSperreAktiv: true);
				}
				return edcRueckgabe;
			}
			catch (Exception fdcException)
			{
				SUB_LogEintragSchreiben("An unexpected error has accourred: ", fdcException.ToString(), "FUN_fdcFunktionAufrufenAsync");
				await FUN_fdcExterneMeldungHinzufuegenAsync(Convert.ToInt32(strFunktionKey), 12124, fdcException.ToString(), new EDC_MesMeldungContext(i_enuFunktion, i_edcMaschinenDaten), i_blnEinlaufSperreAktiv: true);
				return new EDC_MesKommunikationsRueckgabe().FUN_edcRueckgabeFehlerhaft(12124, fdcException.Message, fdcException.ToString());
			}
			finally
			{
				ms_fdcDienstzugriffSemaphore.Release();
				SUB_LogdateiAktualisieren();
			}
			IL_0a1a:
			EDC_MesKommunikationsRueckgabe result;
			return result;
		}

		private async Task FUN_fdcSoftwareFeatureGeaendertAsync(IEnumerable<EDC_SoftwareFeatureGeaendertPayload> i_enuSoftwareFeatureGeaendertPayload)
		{
			if (i_enuSoftwareFeatureGeaendertPayload.Any((EDC_SoftwareFeatureGeaendertPayload i_edcSoftwareFeatureGeaendertPayload) => i_edcSoftwareFeatureGeaendertPayload.PRO_enmSoftwareFeature == ENUM_SoftwareFeatures.MesKonfiguriertFeature))
			{
				await FUN_fdcInitialisiereAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private Task FUN_fdcProduktionssteuerungGeaendertAsync()
		{
			return FUN_fdcInitialisiereAsync();
		}

		private void SUB_LogEintragSchreiben(string i_strNachricht, EDC_MesBasisRueckgabe i_edcBasisRueckgabe, EDC_MesMaschinenDaten i_edcMaschinenDaten = null, [CallerMemberName] string i_strMethodenName = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			string str = string.Empty;
			string i_strKey = string.Empty;
			EDC_MesInitialisierungsRueckgabe eDC_MesInitialisierungsRueckgabe = i_edcBasisRueckgabe as EDC_MesInitialisierungsRueckgabe;
			EDC_MesKommunikationsRueckgabe eDC_MesKommunikationsRueckgabe = i_edcBasisRueckgabe as EDC_MesKommunikationsRueckgabe;
			if (eDC_MesInitialisierungsRueckgabe != null)
			{
				str = "Initializing";
				i_strKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(eDC_MesInitialisierungsRueckgabe.PRO_enuInitialisierungsStatus);
			}
			if (eDC_MesKommunikationsRueckgabe != null)
			{
				str = "Communication";
				i_strKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(eDC_MesKommunikationsRueckgabe.PRO_enuKommunikationsStatus);
			}
			if (i_edcMaschinenDaten != null)
			{
				stringBuilder.AppendLine(i_edcMaschinenDaten.ToString());
			}
			string i_strKey2 = "4_" + i_edcBasisRueckgabe.PRO_i32FehlerKey;
			string pRO_strFehlerErweiterung = i_edcBasisRueckgabe.PRO_strFehlerErweiterung;
			string pRO_strStacktrace = i_edcBasisRueckgabe.PRO_strStacktrace;
			stringBuilder.AppendLine("Caller: " + str);
			CultureInfo i_fdcCulture = new CultureInfo("en");
			stringBuilder.AppendLine("State: " + m_edcLokalisierungsDienst.FUN_strText(i_strKey, i_fdcCulture));
			if (eDC_MesKommunikationsRueckgabe != null)
			{
				stringBuilder.AppendLine("Output arguments:");
				if (eDC_MesKommunikationsRueckgabe.PRO_dicArgumente != null && eDC_MesKommunikationsRueckgabe.PRO_dicArgumente.Count > 0)
				{
					foreach (ENUM_MesRueckgabeArgumente key in eDC_MesKommunikationsRueckgabe.PRO_dicArgumente.Keys)
					{
						if (key.GetType() == typeof(IEnumerable))
						{
							string arg = string.Join("; ", (IEnumerable)eDC_MesKommunikationsRueckgabe.PRO_dicArgumente[key]);
							stringBuilder.AppendLine($"{key}|{arg}");
						}
						else
						{
							stringBuilder.AppendLine($"{key}|{eDC_MesKommunikationsRueckgabe.PRO_dicArgumente[key]}");
						}
					}
				}
				stringBuilder.AppendLine(eDC_MesKommunikationsRueckgabe.ToString());
			}
			stringBuilder.AppendLine("Error: " + m_edcLokalisierungsDienst.FUN_strText(i_strKey2, i_fdcCulture) + pRO_strFehlerErweiterung);
			stringBuilder.AppendLine("Stacktrace: " + pRO_strStacktrace);
			SUB_LogEintragSchreiben(i_strNachricht, stringBuilder.ToString(), i_strMethodenName);
		}

		private void SUB_LogEintragSchreiben(string i_strNachricht, string i_strArgument, [CallerMemberName] string i_strMethodenName = "")
		{
			if ((m_edcLogger.PRO_enmLoggingLevel & ENUM_LogLevel.Traceability) != ENUM_LogLevel.Kein)
			{
				string i_strEintrag = (!string.IsNullOrEmpty(i_strArgument)) ? $"{i_strNachricht}{Environment.NewLine}{i_strArgument}" : i_strNachricht;
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Traceability, i_strEintrag, m_strNamespaceName, m_strKlassenName, i_strMethodenName);
			}
		}

		private void SUB_LogdateiAktualisieren()
		{
			if ((m_edcLogger.PRO_enmLoggingLevel & ENUM_LogLevel.Traceability) != ENUM_LogLevel.Kein)
			{
				m_edcLogger.SUB_DateiAktualisieren();
			}
		}

		/// <summary>
		/// 添加外部消息 异步
		/// Add external message Async
		/// </summary>
		/// <param name="i_i32MeldungOrt3"></param>
		/// <param name="i_i32Meldungsnummer">Message number</param>
		/// <param name="i_strMeldungsDetails"></param>
		/// <param name="i_edcMesMeldungContext"></param>
		/// <param name="i_blnEinlaufSperreAktiv">入口锁</param>
		/// <param name="i_blnDuplicateAllowed">是否允许重复</param>
		/// <param name="i_enuAktion"></param>
		/// <returns></returns>
		private async Task FUN_fdcExterneMeldungHinzufuegenAsync(int i_i32MeldungOrt3, int i_i32Meldungsnummer, string i_strMeldungsDetails = "", EDC_MesMeldungContext i_edcMesMeldungContext = null, bool i_blnEinlaufSperreAktiv = false, bool i_blnDuplicateAllowed = true, ENUM_MeldungAktionen i_enuAktion = ENUM_MeldungAktionen.Erstellen)
		{
			List<ENUM_ProzessAktionen> list = new List<ENUM_ProzessAktionen>();
			List<ENUM_MeldungAktionen> possibleactions = new List<ENUM_MeldungAktionen>
			{
				ENUM_MeldungAktionen.Quittieren
			};
			if (i_blnEinlaufSperreAktiv)
			{
				list.Add(ENUM_ProzessAktionen.Einlausperre);
			}
			await PRO_edcMeldungHinzufuegen.SUB_CreateMessageAsync(i_i32Meldungsnummer, m_strLokalisierungsKeyMesTyp, i_i32MeldungOrt3, possibleactions, list, i_strMeldungsDetails, m_edcJsonSerialisierungsDienst.FUN_strSerialisieren(i_edcMesMeldungContext), i_blnDuplicateAllowed, i_enuAktion).ConfigureAwait(false);
		}

		private void SUB_StatusGeaendertVeroeffentlichen(ENUM_VerbindungsStatus i_enuStatus)
		{
			m_fdcEventAggregator.GetEvent<EDC_MesStatusGeaendertEvent>().Publish(new EDC_MesStatusGeaendertPayload(i_enuStatus));
		}

		private async Task<bool> FUN_blnPingSenden(object i_objSource, EventArgs i_fdcArgs)
		{
			EDC_MesKommunikationsRueckgabe eDC_MesKommunikationsRueckgabe = await FUN_fdcFunktionAufrufenAsync(ENUM_MesFunktionen.PingSenden, new EDC_MesMaschinenDaten()).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_MesKommunikationsRueckgabe.PRO_enuKommunikationsStatus == ENUM_MesKommunikationsStatus.Fehlerhaft)
			{
				m_i32PingFehlerNr = eDC_MesKommunikationsRueckgabe.PRO_i32FehlerKey;
				return false;
			}
			if (m_i32PingFehlerNr != 0)
			{
				await PRO_edcMeldungHinzufuegen.SUB_QuittiereMessageAsync(m_i32PingFehlerNr, m_strLokalisierungsKeyMesTyp, Convert.ToInt32(ENUM_MesFunktionen.PingSenden));
				m_i32PingFehlerNr = 0;
			}
			return true;
		}
	}
}
