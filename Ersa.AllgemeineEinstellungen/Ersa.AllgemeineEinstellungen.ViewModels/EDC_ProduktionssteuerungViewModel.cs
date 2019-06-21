using Ersa.Global.Common;
using Ersa.Global.Common.Helper;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Produktionssteuerung;
using Ersa.Platform.CapabilityContracts.Mes;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Common.Produktionssteuerung;
using Ersa.Platform.DataDienste.CodeBetrieb.Interfaces;
using Ersa.Platform.DataDienste.Codetabelle.Interfaces;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Mes.Konfiguration;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_ProduktionssteuerungViewModel : EDC_NavigationsViewModel
	{
		private const string mC_strJsonFilter = "JSON Files|*.json|All Files (*.*)|*.*";

		private const string mC_strJsonErweiterung = "json";

		private const string mC_strExportDateiname = "settings";

		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly Lazy<INF_ProduktionssteuerungCapability> m_edcProduktionssteuerungCapability;

		private readonly Lazy<INF_MesMaschinenDatenCapability> m_edcMesMaschinenDatenCapability;

		private readonly INF_MesKonfigurationsManager m_edcMesKonfigurationsManeger;

		private readonly INF_CodeBetriebEinstellungenDienst m_edcCodeBetriebEinstellungenDienst;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private readonly INF_ProduktionssteuerungsDienst m_edcProduktionssteuerungsDienst;

		private readonly INF_ProduktionsEinstellungenImportExportDienst m_edcProduktionsEinstellungenImportExportDienst;

		private readonly INF_CodetabellenVerwaltungsDienst m_edcCodetabellenVerwaltungsDienst;

		private readonly INF_LoetprogrammVerwaltungsDienst m_edcLoetprogrammVerwaltungsDienst;

		private EDC_Produktionssteuerungsdaten m_edcGespeicherteAktiveProduktionsdaten;

		private long m_i64GespeicherteProgrammId;

		private bool m_blnIstLoetprotokollAktiv;

		private bool m_blnIstMesAktiv;

		private bool m_blnIstTestBoardErlaubt;

		private bool m_blnVierAugenFreigabeAktiv;

		private bool m_blnIstMesVorhanden;

		private bool m_blnIstCodelesenProPcbVorhanden;

		private bool m_blnIstCodelesenBeiStartVorhanden;

		private bool m_blnCodetabellenHinweisAnzeigen;

		private bool m_blnProgrammGueltigHinweisAnzeigen;

		private bool m_blnProgrammFreigabeHinweisAnzeigen;

		[Import]
		public INF_IoDialogHelfer PRO_edcIoDialogHelfer
		{
			get;
			set;
		}

		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		[Import]
		public INF_VisuSettingsDienst PRO_edcVisuSettingsDienst
		{
			get;
			set;
		}

		[Import]
		public EDC_AllgemeineEinstellungenController PRO_edcController
		{
			get;
			set;
		}

		public EDC_SmartObservableCollection<EDC_EinstellungsGruppe> PRO_lstEinstellungsGruppen
		{
			get;
		}

		public EDC_SmartObservableCollection<EDC_ProduktionsArtElement> PRO_lstStartAssistentOptionen
		{
			get;
		}

		public IEnumerable<EDC_ProduktionsArten> PRO_enuProduktionsArten => PRO_lstStartAssistentOptionen.OfType<EDC_ProduktionsArten>().Union(PRO_lstStartAssistentOptionen.OfType<EDC_ProduktionsArtGruppe>().SelectMany((EDC_ProduktionsArtGruppe i_edcGruppe) => i_edcGruppe.PRO_enuProduktionsArten));

		public EDC_SmartObservableCollection<EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten> PRO_lstCodeLeseFehlerBestaetigungen
		{
			get;
		}

		public EDC_SmartObservableCollection<EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten> PRO_lstCodeNichtGefundenBestaetigungen
		{
			get;
		}

		public DelegateCommand PRO_cmdEinstellungenExportieren
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdEinstellungenImportieren
		{
			get;
			private set;
		}

		public AsyncCommand<EDC_ProduktionsArtMitPrgId> PRO_cmdDefaultLoetprogrammAuswahlOeffnen
		{
			get;
			private set;
		}

		public AsyncCommand<EDC_ProduktionsArtMitPrgId> PRO_cmdDefaultLoetprogrammAuswahlVerwerfen
		{
			get;
			private set;
		}

		public bool PRO_blnIstLoetprotokollAktiv
		{
			get
			{
				return m_blnIstLoetprotokollAktiv;
			}
			set
			{
				SetProperty(ref m_blnIstLoetprotokollAktiv, value, "PRO_blnIstLoetprotokollAktiv");
			}
		}

		public bool PRO_blnIstMesAktiv
		{
			get
			{
				return m_blnIstMesAktiv;
			}
			set
			{
				SetProperty(ref m_blnIstMesAktiv, value, "PRO_blnIstMesAktiv");
			}
		}

		public bool PRO_blnIstMesVorhanden
		{
			get
			{
				return m_blnIstMesVorhanden;
			}
			set
			{
				SetProperty(ref m_blnIstMesVorhanden, value, "PRO_blnIstMesVorhanden");
			}
		}

		public bool PRO_blnIstTestBoardErlaubt
		{
			get
			{
				return m_blnIstTestBoardErlaubt;
			}
			set
			{
				SetProperty(ref m_blnIstTestBoardErlaubt, value, "PRO_blnIstTestBoardErlaubt");
			}
		}

		public bool PRO_blnVierAugenFreigabeAktiv
		{
			get
			{
				return m_blnVierAugenFreigabeAktiv;
			}
			set
			{
				SetProperty(ref m_blnVierAugenFreigabeAktiv, value, "PRO_blnVierAugenFreigabeAktiv");
			}
		}

		public bool PRO_blnCodetabellenHinweisAnzeigen
		{
			get
			{
				return m_blnCodetabellenHinweisAnzeigen;
			}
			set
			{
				SetProperty(ref m_blnCodetabellenHinweisAnzeigen, value, "PRO_blnCodetabellenHinweisAnzeigen");
			}
		}

		public bool PRO_blnProgrammGueltigHinweisAnzeigen
		{
			get
			{
				return m_blnProgrammGueltigHinweisAnzeigen;
			}
			set
			{
				SetProperty(ref m_blnProgrammGueltigHinweisAnzeigen, value, "PRO_blnProgrammGueltigHinweisAnzeigen");
			}
		}

		public bool PRO_blnProgrammFreigabeHinweisAnzeigen
		{
			get
			{
				return m_blnProgrammFreigabeHinweisAnzeigen;
			}
			set
			{
				SetProperty(ref m_blnProgrammFreigabeHinweisAnzeigen, value, "PRO_blnProgrammFreigabeHinweisAnzeigen");
			}
		}

		public bool PRO_blnFehlerbehebungsartenUngueltig
		{
			get
			{
				if (!PRO_lstCodeLeseFehlerBestaetigungen.All((EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten i_edcMoeglichkeit) => !i_edcMoeglichkeit.PRO_blnAktiv))
				{
					return PRO_lstCodeNichtGefundenBestaetigungen.All((EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten i_edcMoeglichkeit) => !i_edcMoeglichkeit.PRO_blnAktiv);
				}
				return true;
			}
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				if (PRO_blnFehlerbehebungsartenUngueltig || (!FUN_blnHatParameterAenderungen() && (m_edcGespeicherteAktiveProduktionsdaten == null || !FUN_blnHatEinstellungsAenderungen(m_edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen))))
				{
					return PRO_blnDefaultProgrammIdGeaendert;
				}
				return true;
			}
		}

		public bool PRO_blnDarfEditieren
		{
			get
			{
				if (!base.PRO_edcShellViewModel.PRO_blnIstMaschineInProduktion)
				{
					return m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungProduktionSteuern");
				}
				return false;
			}
		}

		public bool PRO_blnIstCodelesenVorhanden
		{
			get
			{
				if (!m_blnIstCodelesenProPcbVorhanden)
				{
					return m_blnIstCodelesenBeiStartVorhanden;
				}
				return true;
			}
		}

		public string PRO_strDefaultLoetprogrammString
		{
			get;
			set;
		}

		private EDC_ProduktionsArtMitPrgId PRO_edcCodeProduktionsArt => PRO_enuProduktionsArten.OfType<EDC_ProduktionsArtMitPrgId>().SingleOrDefault();

		private bool PRO_blnDefaultProgrammIdGeaendert => (PRO_edcCodeProduktionsArt?.PRO_blnPrgIdHatAenderung).GetValueOrDefault();

		[ImportingConstructor]
		public EDC_ProduktionssteuerungViewModel(IEventAggregator i_fdcEventAggregator, INF_CapabilityProvider i_edcCapabilityProvider, INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_CodeBetriebEinstellungenDienst i_edcCodeBetriebEinstellungenDienst, INF_ProduktionssteuerungsDienst i_edcProduktionssteuerungsDienst, INF_ProduktionsEinstellungenImportExportDienst i_edcProduktionsEinstellungenImportExportDienst, INF_CodetabellenVerwaltungsDienst i_edcCodetabellenVerwaltungsDienst, INF_MesKonfigurationsManager i_edcMesKonfigurationsManeger, INF_ShellViewModel i_edcShellViewModel, INF_LoetprogrammVerwaltungsDienst i_edcLoetprogrammVerwaltungsDienst)
		{
			m_fdcEventAggregator = i_fdcEventAggregator;
			m_edcProduktionssteuerungCapability = new Lazy<INF_ProduktionssteuerungCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_ProduktionssteuerungCapability>);
			m_edcMesMaschinenDatenCapability = new Lazy<INF_MesMaschinenDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MesMaschinenDatenCapability>);
			m_edcCodeBetriebEinstellungenDienst = i_edcCodeBetriebEinstellungenDienst;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			m_edcProduktionssteuerungsDienst = i_edcProduktionssteuerungsDienst;
			m_edcProduktionsEinstellungenImportExportDienst = i_edcProduktionsEinstellungenImportExportDienst;
			m_edcCodetabellenVerwaltungsDienst = i_edcCodetabellenVerwaltungsDienst;
			m_edcMesKonfigurationsManeger = i_edcMesKonfigurationsManeger;
			m_edcLoetprogrammVerwaltungsDienst = i_edcLoetprogrammVerwaltungsDienst;
			PRO_lstEinstellungsGruppen = new EDC_SmartObservableCollection<EDC_EinstellungsGruppe>();
			PRO_lstStartAssistentOptionen = new EDC_SmartObservableCollection<EDC_ProduktionsArtElement>();
			PRO_lstCodeLeseFehlerBestaetigungen = new EDC_SmartObservableCollection<EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten>();
			PRO_lstCodeNichtGefundenBestaetigungen = new EDC_SmartObservableCollection<EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten>();
			PRO_cmdEinstellungenExportieren = new DelegateCommand(SUB_EinstellungenExportieren);
			PRO_cmdEinstellungenImportieren = new DelegateCommand(SUB_EinstellungenImportieren);
			PRO_cmdDefaultLoetprogrammAuswahlOeffnen = new AsyncCommand<EDC_ProduktionsArtMitPrgId>(FUN_fdcDefaultLoetprogrammAuswahlOeffnenAsync);
			PRO_cmdDefaultLoetprogrammAuswahlVerwerfen = new AsyncCommand<EDC_ProduktionsArtMitPrgId>(FUN_fdcDefaultLoetprogrammAuswahlVerwerfenAsync, (EDC_ProduktionsArtMitPrgId i) => i.PRO_i64AusgewaehlteId != 0);
			PropertyChangedEventManager.AddHandler(i_edcShellViewModel, delegate
			{
				SUB_BerechtigungenAuswerten();
			}, "PRO_blnIstMaschineInProduktion");
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			SUB_EinstellungsGruppenLaden();
			PRO_blnIstMesVorhanden = await m_edcMesKonfigurationsManeger.FUN_fdcIstMesKonfiguriertAsync().ConfigureAwait(continueOnCapturedContext: true);
			m_blnIstCodelesenProPcbVorhanden = await FUN_blnExistiertGeraetMitFunktionAsync(ENUM_LsgVerwendung.LesenProLoetgut).ConfigureAwait(continueOnCapturedContext: true);
			m_blnIstCodelesenBeiStartVorhanden = await FUN_blnExistiertGeraetMitFunktionAsync(ENUM_LsgVerwendung.LesenBeiProduktionsstart).ConfigureAwait(continueOnCapturedContext: true);
			RaisePropertyChanged("PRO_blnIstCodelesenVorhanden");
			m_edcGespeicherteAktiveProduktionsdaten = await FUN_edcProduktionsdatenLadenUndNormalisierenAsync().ConfigureAwait(continueOnCapturedContext: true);
			await FUN_fdcCodetabellenHinweisAktualisierenAsync();
			await FUN_fdcGespeicherteDefaultLoetprogrammIdLadenAsync();
			await FUN_fdcLoetprogrammHinweiseAktualisierenAsync();
			SUB_AktualisiereZustand();
		}

		public override async Task FUN_fdcAenderungenVerwerfenAsync()
		{
			m_edcProduktionssteuerungCapability.Value?.SUB_Verwerfen();
			SUB_InitialisiereProduktionssteuerungsWerte(m_edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen);
			await FUN_fdcDefaultProgrammAenderungVerwerfenAsync();
			await base.FUN_fdcAenderungenVerwerfenAsync();
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			await FUN_fdcDatenSpeichernAsync(m_edcGespeicherteAktiveProduktionsdaten).ConfigureAwait(continueOnCapturedContext: true);
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override async void SUB_AktualisiereZustand()
		{
			base.SUB_AktualisiereZustand();
			RaisePropertyChanged("PRO_blnFehlerbehebungsartenUngueltig");
			PRO_cmdDefaultLoetprogrammAuswahlVerwerfen.RaiseCanExecuteChanged();
			await FUN_fdcLoetprogrammHinweiseAktualisierenAsync();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfEditieren");
		}

		private void SUB_EinstellungsGruppenLaden()
		{
			if (m_edcProduktionssteuerungCapability.Value == null)
			{
				PRO_lstEinstellungsGruppen.Clear();
			}
			else
			{
				PRO_lstEinstellungsGruppen.SUB_Reset(m_edcProduktionssteuerungCapability.Value.FUN_edcEinstellungsGruppenLaden());
			}
		}

		private async Task<EDC_Produktionssteuerungsdaten> FUN_edcProduktionsdatenLadenUndNormalisierenAsync()
		{
			EDC_Produktionssteuerungsdaten eDC_Produktionssteuerungsdaten = await m_edcProduktionssteuerungsDienst.FUN_edcAktiveProduktionssteuerungsDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (eDC_Produktionssteuerungsdaten == null)
			{
				eDC_Produktionssteuerungsdaten = await FUN_edcDefaultWerteErzeugenAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
			EDC_Produktionssteuerungsdaten edcGespeicherteAktiveProduktionsdaten = eDC_Produktionssteuerungsdaten;
			if (edcGespeicherteAktiveProduktionsdaten == null)
			{
				return null;
			}
			PRO_blnIstLoetprotokollAktiv = edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen.PRO_blnLoetprotokollAktiv;
			PRO_blnIstMesAktiv = edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen.PRO_blnMesAktiv;
			PRO_blnIstTestBoardErlaubt = edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen.PRO_blnTestBoardAktiv;
			PRO_blnVierAugenFreigabeAktiv = edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen.PRO_blnVierAugenFreigabeAktiv;
			SUB_MesDatenNormalisieren();
			SUB_InitialisiereProduktionssteuerungsWerte(edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen);
			if (FUN_blnHatEinstellungsAenderungen(edcGespeicherteAktiveProduktionsdaten.PRO_edcProduktionsEinstellungen))
			{
				await FUN_fdcDatenSpeichernAsync(edcGespeicherteAktiveProduktionsdaten);
			}
			return edcGespeicherteAktiveProduktionsdaten;
		}

		private void SUB_MesDatenNormalisieren()
		{
			if (!PRO_blnIstMesVorhanden && PRO_blnIstMesAktiv)
			{
				PRO_blnIstMesAktiv = false;
			}
		}

		private async Task FUN_fdcDatenSpeichernAsync(EDC_Produktionssteuerungsdaten i_edcDaten)
		{
			m_edcProduktionssteuerungCapability.Value?.SUB_Speichern();
			bool blnProtokollAktivAenderung = PRO_blnIstLoetprotokollAktiv != i_edcDaten.PRO_edcProduktionsEinstellungen.PRO_blnLoetprotokollAktiv;
			bool blnMesAktivAenderung = PRO_blnIstMesAktiv != i_edcDaten.PRO_edcProduktionsEinstellungen.PRO_blnMesAktiv;
			EDC_ProduktionsEinstellungen pRO_edcProduktionsEinstellungen = FUN_edcErmittleProduktionssteuerungsWerte();
			i_edcDaten.PRO_blnIstAktiv = true;
			i_edcDaten.PRO_edcProduktionsEinstellungen = pRO_edcProduktionsEinstellungen;
			await m_edcProduktionssteuerungsDienst.FUN_fdcProduktionssteuerungsDatenAendernAsync(i_edcDaten);
			if (blnProtokollAktivAenderung | blnMesAktivAenderung)
			{
				m_fdcEventAggregator.GetEvent<EDC_ProduktionssteuerungGeaendertEvent>().Publish(new EDC_ProduktionssteuerungGeaendertPayload
				{
					PRO_blnLoetprotokollAktiv = PRO_blnIstLoetprotokollAktiv
				});
			}
			m_edcMesMaschinenDatenCapability.Value.SUB_MesAktivSetzen(PRO_blnIstMesAktiv);
			await FUN_fdcDefaultLoetprogrammIdSpeichernAsync();
		}

		private void SUB_EinstellungenExportieren()
		{
			string text = Path.Combine(PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadExport"), "ProductionSettings");
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(text);
			}
			string text2 = PRO_edcIoDialogHelfer.FUN_strSaveFiledialog("JSON Files|*.json|All Files (*.*)|*.*", text, "json", "settings");
			if (!string.IsNullOrEmpty(text2))
			{
				EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen = FUN_edcErmittleProduktionssteuerungsWerte();
				m_edcProduktionsEinstellungenImportExportDienst.SUB_Export(i_edcProduktionsEinstellungen, text2);
			}
		}

		private void SUB_EinstellungenImportieren()
		{
			string text = Path.Combine(PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadExport"), "ProductionSettings");
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(text);
			}
			string text2 = PRO_edcIoDialogHelfer.FUN_strOpenFileDialog(text, "JSON Files|*.json|All Files (*.*)|*.*");
			if (!string.IsNullOrEmpty(text2))
			{
				EDC_ProduktionsEinstellungen i_edcEinstellungen = m_edcProduktionsEinstellungenImportExportDienst.FUN_edcImport(text2);
				SUB_InitialisiereProduktionssteuerungsWerte(i_edcEinstellungen);
				RaisePropertyChanged("PRO_blnHatAenderung");
			}
		}

		private async Task FUN_fdcDefaultLoetprogrammAuswahlOeffnenAsync(EDC_ProduktionsArtMitPrgId i_edcProdArt)
		{
			long? num = await PRO_edcController.FUN_edcDefaultProgrammAuswahlDialogAnzeigen(i_edcProdArt.PRO_i64AusgewaehlteId);
			if (num.HasValue)
			{
				i_edcProdArt.PRO_i64AusgewaehlteId = num.Value;
				await FUN_fdcDefaultLoetprogrammStringAktualisierenAsync(num.Value);
				SUB_AktualisiereZustand();
			}
		}

		private async Task FUN_fdcDefaultLoetprogrammAuswahlVerwerfenAsync(EDC_ProduktionsArtMitPrgId i_edcProdArt)
		{
			i_edcProdArt.PRO_i64AusgewaehlteId = 0L;
			await FUN_fdcDefaultLoetprogrammStringAktualisierenAsync(i_edcProdArt.PRO_i64AusgewaehlteId);
			SUB_AktualisiereZustand();
		}

		private bool FUN_blnHatParameterAenderungen()
		{
			return PRO_lstEinstellungsGruppen.SelectMany((EDC_EinstellungsGruppe i_edcGruppe) => i_edcGruppe.PRO_edcEinstellungen).Any((EDC_BooleanParameter i_edcParameter) => i_edcParameter.PRO_blnHatAenderung);
		}

		private bool FUN_blnHatEinstellungsAenderungen(EDC_ProduktionsEinstellungen i_edcOriginalEinstellungen)
		{
			if (i_edcOriginalEinstellungen == null)
			{
				return false;
			}
			if (PRO_blnIstLoetprotokollAktiv != i_edcOriginalEinstellungen.PRO_blnLoetprotokollAktiv)
			{
				return true;
			}
			if (PRO_blnIstMesAktiv != i_edcOriginalEinstellungen.PRO_blnMesAktiv)
			{
				return true;
			}
			if (PRO_blnIstTestBoardErlaubt != i_edcOriginalEinstellungen.PRO_blnTestBoardAktiv)
			{
				return true;
			}
			if (PRO_blnVierAugenFreigabeAktiv != i_edcOriginalEinstellungen.PRO_blnVierAugenFreigabeAktiv)
			{
				return true;
			}
			IEnumerable<ENUM_Produktionsart> first = FUN_enuGewaehlteArtenErmitteln();
			IOrderedEnumerable<ENUM_Produktionsart> second = from i_enmArt in i_edcOriginalEinstellungen.PRO_lstProduktionsart
			orderby i_enmArt
			select i_enmArt;
			if (!first.SequenceEqual(second))
			{
				return true;
			}
			IOrderedEnumerable<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit> first2 = from i_edcOption in PRO_lstCodeNichtGefundenBestaetigungen
			where i_edcOption.PRO_blnAktiv
			select i_edcOption.PRO_enmCodeNichtGefundenBestaetingungsMoeglichkeit into i_edcMoeglichkeit
			orderby i_edcMoeglichkeit
			select i_edcMoeglichkeit;
			IOrderedEnumerable<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit> second2 = from i_edcMoeglichkeit in i_edcOriginalEinstellungen.PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten
			orderby i_edcMoeglichkeit
			select i_edcMoeglichkeit;
			if (!first2.SequenceEqual(second2))
			{
				return true;
			}
			IOrderedEnumerable<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit> first3 = from i_edcOption in PRO_lstCodeLeseFehlerBestaetigungen
			where i_edcOption.PRO_blnAktiv
			select i_edcOption.PRO_enmCodeLeseFehlerBestaetingungsMoeglichkeit into i_edcMoeglichkeit
			orderby i_edcMoeglichkeit
			select i_edcMoeglichkeit;
			IOrderedEnumerable<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit> second3 = from i_edcMoeglichkeit in i_edcOriginalEinstellungen.PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten
			orderby i_edcMoeglichkeit
			select i_edcMoeglichkeit;
			if (!first3.SequenceEqual(second3))
			{
				return true;
			}
			return false;
		}

		private async Task<EDC_Produktionssteuerungsdaten> FUN_edcDefaultWerteErzeugenAsync()
		{
			EDC_Produktionssteuerungsdaten edcGespeicherteAktiveProduktionsdaten = new EDC_Produktionssteuerungsdaten
			{
				PRO_blnIstAktiv = true,
				PRO_dtmAngelegtAm = DateTime.Now,
				PRO_dtmBearbeitetAm = DateTime.Now,
				PRO_edcProduktionsEinstellungen = new EDC_ProduktionsEinstellungen
				{
					PRO_blnLoetprotokollAktiv = false,
					PRO_blnMesAktiv = false,
					PRO_lstProduktionsart = new List<ENUM_Produktionsart>(Enum.GetValues(typeof(ENUM_Produktionsart)).OfType<ENUM_Produktionsart>()),
					PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten = new List<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit>(Enum.GetValues(typeof(ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit)).OfType<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit>()),
					PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten = new List<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit>(Enum.GetValues(typeof(ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit)).OfType<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit>())
				}
			};
			EDC_Produktionssteuerungsdaten eDC_Produktionssteuerungsdaten = edcGespeicherteAktiveProduktionsdaten;
			eDC_Produktionssteuerungsdaten.PRO_i64ProduktionssteuerungId = await m_edcProduktionssteuerungsDienst.FUN_fdcProduktionssteuerungsDatenErstellenAsync(edcGespeicherteAktiveProduktionsdaten);
			return edcGespeicherteAktiveProduktionsdaten;
		}

		private void SUB_InitialisiereProduktionssteuerungsWerte(EDC_ProduktionsEinstellungen i_edcEinstellungen)
		{
			IEnumerable<EDC_ProduktionsArtElement> i_enuElemente = FUN_enuErstelleMoeglicheProduktionsElemente(i_edcEinstellungen);
			PRO_lstStartAssistentOptionen.SUB_Reset(i_enuElemente);
			List<EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten> list = new List<EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten>();
			foreach (ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit item3 in Enum.GetValues(typeof(ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit)).OfType<ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit>())
			{
				if (item3 != ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit.CodeLesenWiederholen)
				{
					EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten item = new EDC_CodeLeseFehlerBestaetigungsMoeglichkeiten
					{
						PRO_enmCodeLeseFehlerBestaetingungsMoeglichkeit = item3,
						PRO_strLocKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(item3),
						PRO_blnAktiv = (i_edcEinstellungen.PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten != null && i_edcEinstellungen.PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten.Contains(item3))
					};
					list.Add(item);
				}
			}
			PRO_lstCodeLeseFehlerBestaetigungen.SUB_Reset(list);
			List<EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten> list2 = new List<EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten>();
			foreach (ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit item4 in Enum.GetValues(typeof(ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit)).OfType<ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit>())
			{
				EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten item2 = new EDC_CodeNichtGefundenBestaetingungsMoeglichkeiten
				{
					PRO_enmCodeNichtGefundenBestaetingungsMoeglichkeit = item4,
					PRO_strLocKey = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(item4),
					PRO_blnAktiv = (i_edcEinstellungen.PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten != null && i_edcEinstellungen.PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten.Contains(item4))
				};
				list2.Add(item2);
			}
			PRO_lstCodeNichtGefundenBestaetigungen.SUB_Reset(list2);
		}

		private IEnumerable<EDC_ProduktionsArtElement> FUN_enuErstelleMoeglicheProduktionsElemente(EDC_ProduktionsEinstellungen i_edcEinstellungen)
		{
			List<ENUM_Produktionsart> lstGespeicherteArten = i_edcEinstellungen.PRO_lstProduktionsart ?? new List<ENUM_Produktionsart>();
			yield return new EDC_ProduktionsArten("13_542", ENUM_Produktionsart.LetztesProgrammVerwenden)
			{
				PRO_blnAktiv = lstGespeicherteArten.Contains(ENUM_Produktionsart.LetztesProgrammVerwenden)
			};
			yield return new EDC_ProduktionsArten("13_543", ENUM_Produktionsart.ProgrammauswahlManuell)
			{
				PRO_blnAktiv = lstGespeicherteArten.Contains(ENUM_Produktionsart.ProgrammauswahlManuell)
			};
			if (m_blnIstCodelesenProPcbVorhanden || m_blnIstCodelesenBeiStartVorhanden)
			{
				yield return FUN_edcCodeProduktionsArtErstellen(lstGespeicherteArten);
			}
		}

		private EDC_ProduktionsArtElement FUN_edcCodeProduktionsArtErstellen(IList<ENUM_Produktionsart> i_lstGespeicherteArten)
		{
			EDC_ProduktionsArten eDC_ProduktionsArten = m_blnIstCodelesenBeiStartVorhanden ? new EDC_ProduktionsArten("13_934", ENUM_Produktionsart.ProgrammauswahlUeberCodeleserBeiProduktionsstart)
			{
				PRO_blnAktiv = i_lstGespeicherteArten.Contains(ENUM_Produktionsart.ProgrammauswahlUeberCodeleserBeiProduktionsstart)
			} : null;
			EDC_ProduktionsArtMitPrgId eDC_ProduktionsArtMitPrgId = null;
			if (m_blnIstCodelesenProPcbVorhanden)
			{
				EDC_ProduktionsUnterart eDC_ProduktionsUnterart = new EDC_ProduktionsUnterart("13_936", ENUM_Produktionsart.CodeleserProLoetgutNurFuerProtokoll);
				EDC_ProduktionsUnterart eDC_ProduktionsUnterart2 = new EDC_ProduktionsUnterart("13_937", ENUM_Produktionsart.ProgrammauswahlUeberCodeleserProLoetgut);
				EDC_ProduktionsUnterart pRO_edcAktiveUnterart = (i_lstGespeicherteArten.Contains(ENUM_Produktionsart.CodeleserProLoetgutNurFuerProtokoll) && !i_lstGespeicherteArten.Contains(ENUM_Produktionsart.ProgrammauswahlUeberCodeleserProLoetgut)) ? eDC_ProduktionsUnterart : eDC_ProduktionsUnterart2;
				eDC_ProduktionsArtMitPrgId = new EDC_ProduktionsArtMitPrgId("13_935", eDC_ProduktionsUnterart, eDC_ProduktionsUnterart2)
				{
					PRO_blnAktiv = (i_lstGespeicherteArten.Contains(ENUM_Produktionsart.CodeleserProLoetgutNurFuerProtokoll) || i_lstGespeicherteArten.Contains(ENUM_Produktionsart.ProgrammauswahlUeberCodeleserProLoetgut)),
					PRO_edcAktiveUnterart = pRO_edcAktiveUnterart,
					PRO_i64GespeicherteId = m_i64GespeicherteProgrammId
				};
			}
			EDC_ProduktionsArten[] ia_edcArten = (from i_edcUnterart in new EDC_ProduktionsArten[2]
			{
				eDC_ProduktionsArten,
				eDC_ProduktionsArtMitPrgId
			}
			where i_edcUnterart != null
			select i_edcUnterart).ToArray();
			return new EDC_ProduktionsArtGruppe("10_1526", ia_edcArten);
		}

		private EDC_ProduktionsEinstellungen FUN_edcErmittleProduktionssteuerungsWerte()
		{
			return new EDC_ProduktionsEinstellungen
			{
				PRO_blnLoetprotokollAktiv = PRO_blnIstLoetprotokollAktiv,
				PRO_blnMesAktiv = PRO_blnIstMesAktiv,
				PRO_blnTestBoardAktiv = PRO_blnIstTestBoardErlaubt,
				PRO_blnVierAugenFreigabeAktiv = PRO_blnVierAugenFreigabeAktiv,
				PRO_lstProduktionsart = FUN_enuGewaehlteArtenErmitteln().ToList(),
				PRO_lstCodeLeseFehlerBestaetingungsMoeglichkeiten = (from i_edcOption in PRO_lstCodeLeseFehlerBestaetigungen
				where i_edcOption.PRO_blnAktiv
				select i_edcOption.PRO_enmCodeLeseFehlerBestaetingungsMoeglichkeit).ToList(),
				PRO_lstCodeNichtGefundenBestaetingungsMoeglichkeiten = (from i_edcOption in PRO_lstCodeNichtGefundenBestaetigungen
				where i_edcOption.PRO_blnAktiv
				select i_edcOption.PRO_enmCodeNichtGefundenBestaetingungsMoeglichkeit).ToList()
			};
		}

		private async Task FUN_fdcCodetabellenHinweisAktualisierenAsync()
		{
			bool blnIstCodeOptionVerfuegbar = FUN_enuWaehlbareArtenErmitteln().Any(delegate(ENUM_Produktionsart i_enmArt)
			{
				if (i_enmArt != ENUM_Produktionsart.ProgrammauswahlUeberCodeleserBeiProduktionsstart)
				{
					return i_enmArt == ENUM_Produktionsart.ProgrammauswahlUeberCodeleserProLoetgut;
				}
				return true;
			});
			bool flag = await m_edcCodetabellenVerwaltungsDienst.FUN_edcAktiveCodetabelleErmittelnAsync() != null;
			PRO_blnCodetabellenHinweisAnzeigen = (blnIstCodeOptionVerfuegbar && !flag);
		}

		private IEnumerable<ENUM_Produktionsart> FUN_enuWaehlbareArtenErmitteln()
		{
			IEnumerable<ENUM_Produktionsart> first = from i_edcOption in PRO_enuProduktionsArten
			where i_edcOption.PRO_enmProduktionsart.HasValue
			select i_edcOption.PRO_enmProduktionsart.Value;
			IEnumerable<ENUM_Produktionsart> second = from i_edcOption in PRO_enuProduktionsArten.Where(delegate(EDC_ProduktionsArten i_edcOption)
			{
				if (i_edcOption.PRO_enuUnterarten.Any())
				{
					return i_edcOption.PRO_edcAktiveUnterart != null;
				}
				return false;
			})
			select i_edcOption.PRO_edcAktiveUnterart.PRO_enmProduktionsart;
			return from i_enmArt in first.Union(second).Distinct()
			orderby i_enmArt
			select i_enmArt;
		}

		private IEnumerable<ENUM_Produktionsart> FUN_enuGewaehlteArtenErmitteln()
		{
			IEnumerable<ENUM_Produktionsart> first = from i_edcOption in PRO_enuProduktionsArten.Where(delegate(EDC_ProduktionsArten i_edcOption)
			{
				if (i_edcOption.PRO_blnAktiv)
				{
					return i_edcOption.PRO_enmProduktionsart.HasValue;
				}
				return false;
			})
			select i_edcOption.PRO_enmProduktionsart.Value;
			IEnumerable<ENUM_Produktionsart> second = from i_edcOption in PRO_enuProduktionsArten.Where(delegate(EDC_ProduktionsArten i_edcOption)
			{
				if (i_edcOption.PRO_blnAktiv && i_edcOption.PRO_enuUnterarten.Any())
				{
					return i_edcOption.PRO_edcAktiveUnterart != null;
				}
				return false;
			})
			select i_edcOption.PRO_edcAktiveUnterart.PRO_enmProduktionsart;
			return from i_enmArt in first.Union(second).Distinct()
			orderby i_enmArt
			select i_enmArt;
		}

		private async Task<bool> FUN_blnExistiertGeraetMitFunktionAsync(ENUM_LsgVerwendung i_enmVerwendung)
		{
			return (await m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(i_enmVerwendung).ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		private async Task FUN_fdcGespeicherteDefaultLoetprogrammIdLadenAsync()
		{
			EDC_ProduktionsArtMitPrgId edcCodeProdArt = PRO_edcCodeProduktionsArt;
			if (edcCodeProdArt != null)
			{
				long num = await m_edcProduktionssteuerungsDienst.FUN_i64AktiveDefaultLoetprogrammIdLadenAsync();
				edcCodeProdArt.PRO_i64GespeicherteId = (m_i64GespeicherteProgrammId = num);
				await FUN_fdcDefaultLoetprogrammStringAktualisierenAsync(num);
			}
		}

		private async Task FUN_fdcDefaultLoetprogrammIdSpeichernAsync()
		{
			EDC_ProduktionsArtMitPrgId pRO_edcCodeProduktionsArt = PRO_edcCodeProduktionsArt;
			if (pRO_edcCodeProduktionsArt != null)
			{
				pRO_edcCodeProduktionsArt.SUB_PrgIdAenderungUebernehmen();
				m_i64GespeicherteProgrammId = pRO_edcCodeProduktionsArt.PRO_i64GespeicherteId;
				await m_edcProduktionssteuerungsDienst.FUN_fdcAktiveDefaultLoetprogrammIdSpeichernAsync(pRO_edcCodeProduktionsArt.PRO_i64GespeicherteId);
			}
		}

		private async Task FUN_fdcDefaultProgrammAenderungVerwerfenAsync()
		{
			EDC_ProduktionsArtMitPrgId pRO_edcCodeProduktionsArt = PRO_edcCodeProduktionsArt;
			if (pRO_edcCodeProduktionsArt != null)
			{
				pRO_edcCodeProduktionsArt.SUB_PrgIdAenderungVerwerfen();
				await FUN_fdcDefaultLoetprogrammStringAktualisierenAsync(pRO_edcCodeProduktionsArt.PRO_i64GespeicherteId);
				await FUN_fdcLoetprogrammHinweiseAktualisierenAsync();
			}
		}

		private async Task FUN_fdcDefaultLoetprogrammStringAktualisierenAsync(long i_i64ProgrammId)
		{
			PRO_strDefaultLoetprogrammString = await FUN_fdcDefaultLoetprogrammStringErmittelnAsync(i_i64ProgrammId);
			RaisePropertyChanged("PRO_strDefaultLoetprogrammString");
		}

		private async Task<string> FUN_fdcDefaultLoetprogrammStringErmittelnAsync(long i_i64ProgrammId)
		{
			if (i_i64ProgrammId == 0L)
			{
				return string.Empty;
			}
			EDC_ProgrammInfo eDC_ProgrammInfo = await m_edcLoetprogrammVerwaltungsDienst.FUN_fdcProgrammInfoLesenAsync(i_i64ProgrammId);
			if (eDC_ProgrammInfo == null)
			{
				return string.Empty;
			}
			return eDC_ProgrammInfo.PRO_strBibliotheksName + " / " + eDC_ProgrammInfo.PRO_strProgrammName;
		}

		private async Task FUN_fdcLoetprogrammHinweiseAktualisierenAsync()
		{
			PRO_blnProgrammGueltigHinweisAnzeigen = false;
			PRO_blnProgrammFreigabeHinweisAnzeigen = false;
			if (PRO_edcCodeProduktionsArt != null)
			{
				EDC_ProgrammInfo eDC_ProgrammInfo = await m_edcLoetprogrammVerwaltungsDienst.FUN_fdcProgrammInfoLesenAsync(PRO_edcCodeProduktionsArt.PRO_i64AusgewaehlteId);
				if (eDC_ProgrammInfo != null)
				{
					PRO_blnProgrammGueltigHinweisAnzeigen = eDC_ProgrammInfo.PRO_blnIstFehlerhaft;
					PRO_blnProgrammFreigabeHinweisAnzeigen = !eDC_ProgrammInfo.PROa_enmStatus.Contains(ENUM_LoetprogrammStatus.Freigegeben);
				}
			}
		}
	}
}
