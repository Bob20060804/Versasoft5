using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Prism.Properties
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
					resourceMan = new ResourceManager("Prism.Properties.Resources", typeof(Prism.Properties.Resources).Assembly);
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

		internal static string AdapterInvalidTypeException => ResourceManager.GetString("AdapterInvalidTypeException", resourceCulture);

		internal static string CannotChangeRegionNameException => ResourceManager.GetString("CannotChangeRegionNameException", resourceCulture);

		internal static string CannotCreateNavigationTarget => ResourceManager.GetString("CannotCreateNavigationTarget", resourceCulture);

		internal static string CanOnlyAddTypesThatInheritIFromRegionBehavior => ResourceManager.GetString("CanOnlyAddTypesThatInheritIFromRegionBehavior", resourceCulture);

		internal static string ConfigurationStoreCannotBeNull => ResourceManager.GetString("ConfigurationStoreCannotBeNull", resourceCulture);

		internal static string ContentControlHasContentException => ResourceManager.GetString("ContentControlHasContentException", resourceCulture);

		internal static string CyclicDependencyFound => ResourceManager.GetString("CyclicDependencyFound", resourceCulture);

		internal static string DeactiveNotPossibleException => ResourceManager.GetString("DeactiveNotPossibleException", resourceCulture);

		internal static string DefaultTextLoggerPattern => ResourceManager.GetString("DefaultTextLoggerPattern", resourceCulture);

		internal static string DelegateCommandDelegatesCannotBeNull => ResourceManager.GetString("DelegateCommandDelegatesCannotBeNull", resourceCulture);

		internal static string DelegateCommandInvalidGenericPayloadType => ResourceManager.GetString("DelegateCommandInvalidGenericPayloadType", resourceCulture);

		internal static string DependencyForUnknownModule => ResourceManager.GetString("DependencyForUnknownModule", resourceCulture);

		internal static string DependencyOnMissingModule => ResourceManager.GetString("DependencyOnMissingModule", resourceCulture);

		internal static string DirectoryNotFound => ResourceManager.GetString("DirectoryNotFound", resourceCulture);

		internal static string DuplicatedModule => ResourceManager.GetString("DuplicatedModule", resourceCulture);

		internal static string DuplicatedModuleGroup => ResourceManager.GetString("DuplicatedModuleGroup", resourceCulture);

		internal static string FailedToGetType => ResourceManager.GetString("FailedToGetType", resourceCulture);

		internal static string FailedToLoadModule => ResourceManager.GetString("FailedToLoadModule", resourceCulture);

		internal static string FailedToLoadModuleNoAssemblyInfo => ResourceManager.GetString("FailedToLoadModuleNoAssemblyInfo", resourceCulture);

		internal static string FailedToRetrieveModule => ResourceManager.GetString("FailedToRetrieveModule", resourceCulture);

		internal static string HostControlCannotBeNull => ResourceManager.GetString("HostControlCannotBeNull", resourceCulture);

		internal static string HostControlCannotBeSetAfterAttach => ResourceManager.GetString("HostControlCannotBeSetAfterAttach", resourceCulture);

		internal static string HostControlMustBeATabControl => ResourceManager.GetString("HostControlMustBeATabControl", resourceCulture);

		internal static string IEnumeratorObsolete => ResourceManager.GetString("IEnumeratorObsolete", resourceCulture);

		internal static string InvalidArgumentAssemblyUri => ResourceManager.GetString("InvalidArgumentAssemblyUri", resourceCulture);

		internal static string InvalidDelegateRerefenceTypeException => ResourceManager.GetString("InvalidDelegateRerefenceTypeException", resourceCulture);

		internal static string ItemsControlHasItemsSourceException => ResourceManager.GetString("ItemsControlHasItemsSourceException", resourceCulture);

		internal static string MappingExistsException => ResourceManager.GetString("MappingExistsException", resourceCulture);

		internal static string ModuleDependenciesNotMetInGroup => ResourceManager.GetString("ModuleDependenciesNotMetInGroup", resourceCulture);

		internal static string ModuleNotFound => ResourceManager.GetString("ModuleNotFound", resourceCulture);

		internal static string ModulePathCannotBeNullOrEmpty => ResourceManager.GetString("ModulePathCannotBeNullOrEmpty", resourceCulture);

		internal static string ModuleTypeNotFound => ResourceManager.GetString("ModuleTypeNotFound", resourceCulture);

		internal static string NavigationInProgress => ResourceManager.GetString("NavigationInProgress", resourceCulture);

		internal static string NavigationServiceHasNoRegion => ResourceManager.GetString("NavigationServiceHasNoRegion", resourceCulture);

		internal static string NoRegionAdapterException => ResourceManager.GetString("NoRegionAdapterException", resourceCulture);

		internal static string NoRetrieverCanRetrieveModule => ResourceManager.GetString("NoRetrieverCanRetrieveModule", resourceCulture);

		internal static string OnViewRegisteredException => ResourceManager.GetString("OnViewRegisteredException", resourceCulture);

		internal static string PropertySupport_ExpressionNotProperty_Exception => ResourceManager.GetString("PropertySupport_ExpressionNotProperty_Exception", resourceCulture);

		internal static string PropertySupport_NotMemberAccessExpression_Exception => ResourceManager.GetString("PropertySupport_NotMemberAccessExpression_Exception", resourceCulture);

		internal static string PropertySupport_StaticExpression_Exception => ResourceManager.GetString("PropertySupport_StaticExpression_Exception", resourceCulture);

		internal static string RegionBehaviorAttachCannotBeCallWithNullRegion => ResourceManager.GetString("RegionBehaviorAttachCannotBeCallWithNullRegion", resourceCulture);

		internal static string RegionBehaviorRegionCannotBeSetAfterAttach => ResourceManager.GetString("RegionBehaviorRegionCannotBeSetAfterAttach", resourceCulture);

		internal static string RegionCreationException => ResourceManager.GetString("RegionCreationException", resourceCulture);

		internal static string RegionManagerWithDifferentNameException => ResourceManager.GetString("RegionManagerWithDifferentNameException", resourceCulture);

		internal static string RegionNameCannotBeEmptyException => ResourceManager.GetString("RegionNameCannotBeEmptyException", resourceCulture);

		internal static string RegionNameExistsException => ResourceManager.GetString("RegionNameExistsException", resourceCulture);

		internal static string RegionNotFound => ResourceManager.GetString("RegionNotFound", resourceCulture);

		internal static string RegionNotInRegionManagerException => ResourceManager.GetString("RegionNotInRegionManagerException", resourceCulture);

		internal static string RegionViewExistsException => ResourceManager.GetString("RegionViewExistsException", resourceCulture);

		internal static string RegionViewNameExistsException => ResourceManager.GetString("RegionViewNameExistsException", resourceCulture);

		internal static string StartupModuleDependsOnAnOnDemandModule => ResourceManager.GetString("StartupModuleDependsOnAnOnDemandModule", resourceCulture);

		internal static string StringCannotBeNullOrEmpty => ResourceManager.GetString("StringCannotBeNullOrEmpty", resourceCulture);

		internal static string StringCannotBeNullOrEmpty1 => ResourceManager.GetString("StringCannotBeNullOrEmpty1", resourceCulture);

		internal static string TypeWithKeyNotRegistered => ResourceManager.GetString("TypeWithKeyNotRegistered", resourceCulture);

		internal static string UpdateRegionException => ResourceManager.GetString("UpdateRegionException", resourceCulture);

		internal static string ValueMustBeOfTypeModuleInfo => ResourceManager.GetString("ValueMustBeOfTypeModuleInfo", resourceCulture);

		internal static string ValueNotFound => ResourceManager.GetString("ValueNotFound", resourceCulture);

		internal static string ViewNotInRegionException => ResourceManager.GetString("ViewNotInRegionException", resourceCulture);

		internal Resources()
		{
		}
	}
}
