using Ersa.Global.Common;
using Ersa.Platform.Common;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Module.Extensions;
using Ersa.Platform.UI.Interfaces;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Ersa.AllgemeineEinstellungen
{
	[ModuleExport("AllgemeineEinstellungen", typeof(EDC_AllgemeineEinstellungenModule))]
	public class EDC_AllgemeineEinstellungenModule : EDC_DisposableObject, IModule
	{
		private const string mC_strModulKey = "13_460";

		private readonly INF_ShellViewModel m_edcShellViewModel;

		private readonly EDC_AllgemeineEinstellungenController m_edcModuleController;

		private readonly CompositionContainer m_fdcChildContainer;

		private readonly INF_SplashScreenViewModel m_edcSplashViewModel;

		[Export]
		public EDC_ElementVersion PRO_strModulVersion
		{
			get
			{
				return GetType().FUN_edcVersionErmitteln();
			}
		}

		[Export]
		public EDC_RechteGruppe PRO_edcRechteGruppe
		{
			get
			{
				return new EDC_RechteGruppe
				{
					PRO_strNameKey = "AllgemeineEinstellungen",
					PRO_edcRechte = 
					{
						new EDC_RechtBeschreibung
						{
							PRO_strRecht = "BerechtigungProduktionSteuern",
							PRO_strNameKey = "13_702",
							PRO_strGruppierungsId = "GruppierungProduktionsleiter"
						},
						new EDC_RechtBeschreibung
						{
							PRO_strRecht = "BerechtigungZeitschaltuhr",
							PRO_strNameKey = "7_103",
							PRO_strGruppierungsId = "GruppierungProduktionsleiter"
						}
					}
				};
			}
		}

		[Export]
		public IList<EDC_GlobalSetting> PRO_lstGlobaleSettings
		{
			get
			{
				return m_edcModuleController.PRO_enuGlobalSettings.ToList();
			}
		}

		[ImportingConstructor]
		public EDC_AllgemeineEinstellungenModule(CompositionContainer i_fdcContainer, INF_ShellViewModel i_edcShellViewModel, INF_SplashScreenViewModel i_edcSplashViewModel)
		{
			TypeCatalog catalog = new TypeCatalog(this.FUN_enuAlleModulTypen());
			m_fdcChildContainer = new CompositionContainer(catalog, i_fdcContainer);
			m_fdcChildContainer.ComposeExportedValue(m_fdcChildContainer);
			m_edcSplashViewModel = i_edcSplashViewModel;
			m_edcModuleController = m_fdcChildContainer.GetExportedValue<EDC_AllgemeineEinstellungenController>();
			m_edcShellViewModel = i_edcShellViewModel;
		}

		public void Initialize()
		{
			SUB_AktualisiereSplash(ENUM_ModulStatus.enmInitializing, "13_453");
			m_edcModuleController.SUB_InitialisiereNavigationsElemente();
			SUB_AktualisiereSplash(ENUM_ModulStatus.enmInitializing, "13_433");
			m_edcShellViewModel.SUB_RegistriereHauptMenuEintraege(m_edcModuleController.PRO_edcHauptmenuEintraege);
			SUB_AktualisiereSplash(ENUM_ModulStatus.enmInitializing, "13_452");
			m_edcModuleController.SUB_ViewsZuRegionsHinzufuegen();
			SUB_AktualisiereSplash(ENUM_ModulStatus.enmCompleted, "13_443");
		}

		protected override void SUB_InternalDispose()
		{
			m_fdcChildContainer.Dispose();
		}

		private void SUB_AktualisiereSplash(ENUM_ModulStatus i_enmModulStatus, string i_strMeldungKey)
		{
			m_edcSplashViewModel.SUB_AktualisiereDasModul("13_460", i_enmModulStatus, i_strMeldungKey);
		}
	}
}
