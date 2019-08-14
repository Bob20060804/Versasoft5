using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Prism.Mef.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("Prism.Mef.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string BootstrapperSequenceCompleted => ResourceManager.GetString("BootstrapperSequenceCompleted", resourceCulture);

		internal static string ConfiguringCatalogForMEF => ResourceManager.GetString("ConfiguringCatalogForMEF", resourceCulture);

		internal static string ConfiguringDefaultRegionBehaviors => ResourceManager.GetString("ConfiguringDefaultRegionBehaviors", resourceCulture);

		internal static string ConfiguringMefContainer => ResourceManager.GetString("ConfiguringMefContainer", resourceCulture);

		internal static string ConfiguringModuleCatalog => ResourceManager.GetString("ConfiguringModuleCatalog", resourceCulture);

		internal static string ConfiguringRegionAdapters => ResourceManager.GetString("ConfiguringRegionAdapters", resourceCulture);

		internal static string ConfiguringServiceLocatorSingleton => ResourceManager.GetString("ConfiguringServiceLocatorSingleton", resourceCulture);

		internal static string ConfiguringViewModelLocator => ResourceManager.GetString("ConfiguringViewModelLocator", resourceCulture);

		internal static string CreatingCatalogForMEF => ResourceManager.GetString("CreatingCatalogForMEF", resourceCulture);

		internal static string CreatingMefContainer => ResourceManager.GetString("CreatingMefContainer", resourceCulture);

		internal static string CreatingModuleCatalog => ResourceManager.GetString("CreatingModuleCatalog", resourceCulture);

		internal static string CreatingShell => ResourceManager.GetString("CreatingShell", resourceCulture);

		internal static string FailedToGetType => ResourceManager.GetString("FailedToGetType", resourceCulture);

		internal static string InitializingModules => ResourceManager.GetString("InitializingModules", resourceCulture);

		internal static string InitializingShell => ResourceManager.GetString("InitializingShell", resourceCulture);

		internal static string LoggerWasCreatedSuccessfully => ResourceManager.GetString("LoggerWasCreatedSuccessfully", resourceCulture);

		internal static string NullCompositionContainerException => ResourceManager.GetString("NullCompositionContainerException", resourceCulture);

		internal static string NullLoggerFacadeException => ResourceManager.GetString("NullLoggerFacadeException", resourceCulture);

		internal static string NullModuleCatalogException => ResourceManager.GetString("NullModuleCatalogException", resourceCulture);

		internal static string RegisteringFrameworkExceptionTypes => ResourceManager.GetString("RegisteringFrameworkExceptionTypes", resourceCulture);

		internal static string SettingTheRegionManager => ResourceManager.GetString("SettingTheRegionManager", resourceCulture);

		internal static string UpdatingRegions => ResourceManager.GetString("UpdatingRegions", resourceCulture);

		internal Resources()
		{
		}
	}
}
