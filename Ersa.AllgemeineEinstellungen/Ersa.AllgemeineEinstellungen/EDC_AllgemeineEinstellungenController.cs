using Ersa.AllgemeineEinstellungen.BetriebsmittelVerwaltung;
using Ersa.AllgemeineEinstellungen.Dialoge;
using Ersa.AllgemeineEinstellungen.ViewModels;
using Ersa.AllgemeineEinstellungen.Views;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Module;
using Ersa.Platform.UI.Common;
using Ersa.Platform.UI.Common.TabItem;
using Ersa.Platform.UI.Dialoge;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.AllgemeineEinstellungen
{
	[Export]
	public class EDC_AllgemeineEinstellungenController : EDC_NavigationsControllerBase
	{
		private readonly CompositionContainer m_fdcContainer;

		private EDC_TabItemSpezifikation[] ma_edcEinstellungenTabs;

		[Export("AllgemeineEinstellungenTabContainer", typeof(IEnumerable<EDC_TabItemSpezifikation>))]
		public IEnumerable<EDC_TabItemSpezifikation> PRO_edcEinstellungenTabItemsSpezifikationen
		{
			get
			{
				return ma_edcEinstellungenTabs ?? (ma_edcEinstellungenTabs = FUN_enuEinstellungenTabsErstellen().ToArray());
			}
		}

		[ImportMany(Source = ImportSource.Local)]
		public IEnumerable<EDC_GlobalSetting> PRO_enuGlobalSettings
		{
			get;
			set;
		}

		[Import]
		public INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			get;
			set;
		}

		public EDC_HauptmenuEintragSpezifikation[] PRO_edcHauptmenuEintraege
		{
			get;
			private set;
		}

		[ImportingConstructor]
		public EDC_AllgemeineEinstellungenController(CompositionContainer i_fdcContainer)
		{
			m_fdcContainer = i_fdcContainer;
		}

		public void SUB_InitialisiereNavigationsElemente()
		{
			PRO_edcHauptmenuEintraege = new EDC_HauptmenuEintragSpezifikation[1]
			{
				new EDC_HauptmenuEintragSpezifikation
				{
					PRO_blnIstStandardmaessigAktiviert = true,
					PRO_objView = m_fdcContainer.GetExportedValue<EDV_AllgemeineEinstellungenView>(),
					PRO_i32Reihenfolge = 6,
					PRO_strNameKey = "7_102",
					PRO_uriIcon = new Uri("pack://application:,,,/Ersa.Global.Controls;component/Bilder/Icons/Hauptmenue_Einstellungen_48x48.png", UriKind.Absolute)
				}
			};
		}

		public virtual async Task<long?> FUN_edcDefaultProgrammAuswahlDialogAnzeigen(long i_i64AktuellesDefaultProgramm)
		{
			EDU_LoetprogrammAuswahlDialog edcDialog = m_fdcContainer.GetExportedValueOrDefault<EDU_LoetprogrammAuswahlDialog>();
			await edcDialog.PRO_edcViewModel.FUN_fdcInitialisierenAsync();
			await edcDialog.PRO_edcViewModel.FUN_fdcProgrammSetzenAsync(i_i64AktuellesDefaultProgramm);
			if (edcDialog.ShowDialog() == true)
			{
				return edcDialog.PRO_i64PrgId;
			}
			return null;
		}

		public virtual bool FUN_edcFlussmittelBearbeitenDialogAnzeigen(EDC_Flussmittel i_edcFlussmittel, Func<string, string, string> i_delValidierung)
		{
			EDU_FlussmittelBearbeitenDialog exportedValueOrDefault = m_fdcContainer.GetExportedValueOrDefault<EDU_FlussmittelBearbeitenDialog>();
			exportedValueOrDefault.PRO_strTitel = PRO_edcLokalisierungsDienst.FUN_strText("10_1490");
			exportedValueOrDefault.PRO_strName = i_edcFlussmittel.PRO_strName;
			exportedValueOrDefault.PRO_strSpezifikation = i_edcFlussmittel.PRO_strSpezifikation;
			exportedValueOrDefault.PRO_delValidierung = i_delValidierung;
			if (exportedValueOrDefault.ShowDialog() == true)
			{
				i_edcFlussmittel.PRO_strName = exportedValueOrDefault.PRO_strName;
				i_edcFlussmittel.PRO_strSpezifikation = exportedValueOrDefault.PRO_strSpezifikation;
				return true;
			}
			return false;
		}

		private IEnumerable<EDC_TabItemSpezifikation> FUN_enuEinstellungenTabsErstellen()
		{
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "1_158",
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_ZeitschaltuhrView>(),
				PRO_i32Reihenfolge = 2,
				PRO_strRecht = "BerechtigungZeitschaltuhr",
				PRO_strRechtNameKey = "7_103",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_ZeitschaltuhrViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_368",
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_MeldeAmpelView>(),
				PRO_i32Reihenfolge = 3,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_MeldeAmpelViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "1_552",
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_PfadeView>(),
				PRO_i32Reihenfolge = 4,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_PfadeViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_715",
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_ProduktionssteuerungView>(),
				PRO_i32Reihenfolge = 6,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_ProduktionssteuerungViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_202",
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_MaschinenIdentifikationView>(),
				PRO_i32Reihenfolge = 7,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_MaschinenIdentifikationViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_845",
				PRO_lstSoftwareFeatures = new List<ENUM_SoftwareFeatures>
				{
					ENUM_SoftwareFeatures.GemeinsameProgrammeFeature
				},
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_GruppenVerwaltungView>(),
				PRO_i32Reihenfolge = 8,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_GruppenVerwaltungViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_1068",
				PRO_lstSoftwareFeatures = new List<ENUM_SoftwareFeatures>
				{
					ENUM_SoftwareFeatures.BetriebsmittelFeature
				},
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_BetriebsmittelVerwaltungView>(),
				PRO_i32Reihenfolge = 9,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_BetriebsmittelVerwaltungViewModel>()
			};
			yield return new EDC_TabItemSpezifikation
			{
				PRO_strNameKey = "13_181",
				PRO_lstSoftwareFeatures = new List<ENUM_SoftwareFeatures>
				{
					ENUM_SoftwareFeatures.BetriebsmittelFeature
				},
				PRO_objTabView = m_fdcContainer.GetExportedValue<EDV_RuestkomponentenVerwaltungView>(),
				PRO_i32Reihenfolge = 9,
				PRO_strRecht = "BerechtigungProduktionSteuern",
				PRO_strRechtNameKey = "13_702",
				PRO_edcNavigationsViewModel = m_fdcContainer.GetExportedValue<EDC_RuestkomponentenVerwaltungViewModel>()
			};
		}
	}
}
