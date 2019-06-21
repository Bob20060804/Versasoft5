using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Sprache;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_PfadeViewModel : EDC_NavigationsViewModel
	{
		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<Lazy<IList<EDC_GlobalSetting>>> m_enuGlobaleSettings;

		private readonly INF_IoDialogHelfer m_edcIoDialogHelfer;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private EDC_GlobalSetting m_edcExportSpracheSetting;

		private EDC_Sprache m_edcAktuelleExportSprache;

		[Import]
		public EDC_AllgemeineEinstellungenController PRO_edcController
		{
			get;
			set;
		}

		[Import]
		public INF_VisuSettingsDienst PRO_edcSettingDienst
		{
			get;
			set;
		}

		public ObservableCollection<EDC_Sprache> PRO_lstExportSprachen
		{
			get;
			private set;
		}

		[Export]
		public EDC_GlobalSetting PRO_edcExportSpracheSetting
		{
			get
			{
				object eDC_GlobalSetting = m_edcExportSpracheSetting;
				if (eDC_GlobalSetting == null)
				{
					EDC_GlobalSetting obj = new EDC_GlobalSetting("ExportSprache")
					{
						PRO_strDefaultWert = string.Empty,
						PRO_blnIstAusgeblendet = true,
						PRO_strLokalisierungsKey = "13_291",
						PRO_enmBereich = ENUM_GlobalSettingBereich.enmAnwender,
						PRO_delWertGeaendertAktion = delegate
						{
							SUB_OnExportSpracheSettingGeaendert();
						}
					};
					EDC_GlobalSetting eDC_GlobalSetting2 = obj;
					m_edcExportSpracheSetting = obj;
					eDC_GlobalSetting = eDC_GlobalSetting2;
				}
				return (EDC_GlobalSetting)eDC_GlobalSetting;
			}
		}

		public EDC_Sprache PRO_edcAktuelleExportSprache
		{
			get
			{
				return m_edcAktuelleExportSprache;
			}
			set
			{
				if (object.Equals(m_edcAktuelleExportSprache, value))
				{
					return;
				}
				m_edcAktuelleExportSprache = value;
				if (PRO_edcExportSpracheSetting != null && PRO_edcAktuelleExportSprache != null)
				{
					string text = (PRO_edcAktuelleExportSprache.PRO_fdcCultureInfo == null) ? string.Empty : PRO_edcAktuelleExportSprache.PRO_fdcCultureInfo.Name;
					if (PRO_edcExportSpracheSetting.PRO_strWert != text)
					{
						PRO_edcExportSpracheSetting.PRO_strWert = text;
					}
				}
				RaisePropertyChanged("PRO_edcAktuelleExportSprache");
			}
		}

		public ICommand PRO_cmdPfadAuswahl
		{
			get;
			private set;
		}

		public IEnumerable<EDC_GlobalSetting> PRO_enuGlobaleSettingsExport => FUN_enuGlobalFuerSettingsErmitteln(ENUM_GlobalSettingBereich.enmExport);

		public IEnumerable<EDC_GlobalSetting> PRO_enuGlobaleSettingsAnwender => FUN_enuGlobalFuerSettingsErmitteln(ENUM_GlobalSettingBereich.enmAnwender);

		public override bool PRO_blnHatAenderung => FUN_enuGeaenderteSettingsErmitteln().Any();

		public bool PRO_blnDarfPfadeEditieren
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

		private IEnumerable<EDC_GlobalSetting> PRO_enuGlobaleSettings => PRO_enuGlobaleSettingsExport.Union(PRO_enuGlobaleSettingsAnwender);

		[ImportingConstructor]
		public EDC_PfadeViewModel(INF_IoDialogHelfer i_edcIoDialogHelfer, INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_ShellViewModel i_edcShellViewModel)
		{
			m_edcIoDialogHelfer = i_edcIoDialogHelfer;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			PRO_cmdPfadAuswahl = new DelegateCommand<string>(SUB_PfadAuswahl);
			PropertyChangedEventManager.AddHandler(i_edcShellViewModel, delegate
			{
				SUB_BerechtigungenAuswerten();
			}, "PRO_blnIstMaschineInProduktion");
			PRO_lstExportSprachen = new ObservableCollection<EDC_Sprache>();
			SUB_InitialisiereSprachListen();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			RaisePropertyChanged("PRO_enuGlobaleSettingsAnwender");
			RaisePropertyChanged("PRO_enuGlobaleSettingsExport");
		}

		public override Task FUN_fdcAenderungenSpeichernAsync()
		{
			List<EDC_GlobalSetting> i_enuGeaenderteSettings = FUN_enuGeaenderteSettingsErmitteln().ToList();
			PRO_edcSettingDienst.SUB_Speichern();
			SUB_AenderungenAuswerten(i_enuGeaenderteSettings);
			return base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			foreach (EDC_GlobalSetting pRO_enuGlobaleSetting in PRO_enuGlobaleSettings)
			{
				string strGlobalSettingWert = PRO_edcSettingDienst.FUN_strGlobalSettingWertErmitteln(pRO_enuGlobaleSetting.PRO_strKey);
				if (pRO_enuGlobaleSetting.PRO_strWert != strGlobalSettingWert)
				{
					pRO_enuGlobaleSetting.PRO_strWert = strGlobalSettingWert;
					if (pRO_enuGlobaleSetting.PRO_strKey == "ExportSprache")
					{
						PRO_edcAktuelleExportSprache = EDC_SpracheHelfer.FUN_enuVerfuegbareSprachenErstellen().FirstOrDefault((EDC_Sprache i_edcSprache) => i_edcSprache.PRO_fdcCultureInfo.Name == strGlobalSettingWert);
					}
				}
			}
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfPfadeEditieren");
		}

		private void SUB_AenderungenAuswerten(IEnumerable<EDC_GlobalSetting> i_enuGeaenderteSettings)
		{
			foreach (EDC_GlobalSetting i_enuGeaenderteSetting in i_enuGeaenderteSettings)
			{
				i_enuGeaenderteSetting.PRO_delWertGeaendertAktion(i_enuGeaenderteSetting);
			}
		}

		private void SUB_InitialisiereSprachListen()
		{
			PRO_lstExportSprachen.Add(new EDC_Sprache
			{
				PRO_strText = "13_292",
				PRO_fdcCultureInfo = null
			});
			foreach (EDC_Sprache item in EDC_SpracheHelfer.FUN_enuVerfuegbareSprachenErstellen())
			{
				PRO_lstExportSprachen.Add(item);
			}
		}

		private void SUB_OnExportSpracheSettingGeaendert()
		{
			PRO_edcAktuelleExportSprache = (string.IsNullOrEmpty(PRO_edcExportSpracheSetting.PRO_strWert) ? PRO_lstExportSprachen.First((EDC_Sprache i_edcSprache) => i_edcSprache.PRO_fdcCultureInfo == null) : PRO_lstExportSprachen.First(delegate(EDC_Sprache i_edcSprache)
			{
				if (i_edcSprache.PRO_fdcCultureInfo != null)
				{
					return i_edcSprache.PRO_fdcCultureInfo.Name == PRO_edcExportSpracheSetting.PRO_strWert;
				}
				return false;
			}));
		}

		private IEnumerable<EDC_GlobalSetting> FUN_enuGeaenderteSettingsErmitteln()
		{
			return from edcSetting in PRO_enuGlobaleSettings
			let strWert = PRO_edcSettingDienst.FUN_strGlobalSettingWertErmitteln(edcSetting.PRO_strKey)
			where edcSetting.PRO_strWert != strWert
			select edcSetting;
		}

		private void SUB_PfadAuswahl(string i_strSettingsKey)
		{
			if (string.IsNullOrEmpty(i_strSettingsKey))
			{
				return;
			}
			EDC_GlobalSetting eDC_GlobalSetting = PRO_enuGlobaleSettings.SingleOrDefault((EDC_GlobalSetting i_edcSetting) => i_edcSetting.PRO_strKey == i_strSettingsKey);
			if (eDC_GlobalSetting != null)
			{
				string text = m_edcIoDialogHelfer.FUN_strBrowseFolderDialog(string.IsNullOrEmpty(eDC_GlobalSetting.PRO_strWert) ? Path.GetFullPath(eDC_GlobalSetting.PRO_strDefaultWert) : eDC_GlobalSetting.PRO_strWert);
				if (!string.IsNullOrEmpty(text))
				{
					eDC_GlobalSetting.PRO_strWert = text;
					SUB_AktualisiereZustand();
				}
			}
		}

		private IEnumerable<EDC_GlobalSetting> FUN_enuGlobalFuerSettingsErmitteln(ENUM_GlobalSettingBereich i_enmBereich)
		{
			if (m_enuGlobaleSettings != null)
			{
				return from i_edcSetting in m_enuGlobaleSettings.SelectMany((Lazy<IList<EDC_GlobalSetting>> i_lstGlobaleSettings) => i_lstGlobaleSettings.Value)
				where i_edcSetting.PRO_enmBereich == i_enmBereich
				select i_edcSetting;
			}
			return new List<EDC_GlobalSetting>();
		}
	}
}
