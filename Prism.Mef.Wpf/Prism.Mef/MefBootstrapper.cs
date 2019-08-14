using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Mef.Properties;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Prism.Mef
{
	public abstract class MefBootstrapper : Bootstrapper
	{
		protected AggregateCatalog AggregateCatalog
		{
			get;
			set;
		}

		protected CompositionContainer Container
		{
			get;
			set;
		}

		public override void Run(bool runWithDefaultConfiguration)
		{
			base.Logger = CreateLogger();
			if (base.Logger == null)
			{
				throw new InvalidOperationException(Resources.NullLoggerFacadeException);
			}
			base.Logger.Log(Resources.LoggerWasCreatedSuccessfully, Category.Debug, Priority.Low);
			base.Logger.Log(Resources.CreatingModuleCatalog, Category.Debug, Priority.Low);
			base.ModuleCatalog = CreateModuleCatalog();
			if (base.ModuleCatalog == null)
			{
				throw new InvalidOperationException(Resources.NullModuleCatalogException);
			}
			base.Logger.Log(Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
			ConfigureModuleCatalog();
			base.Logger.Log(Resources.CreatingCatalogForMEF, Category.Debug, Priority.Low);
			AggregateCatalog = CreateAggregateCatalog();
			base.Logger.Log(Resources.ConfiguringCatalogForMEF, Category.Debug, Priority.Low);
			ConfigureAggregateCatalog();
			RegisterDefaultTypesIfMissing();
			base.Logger.Log(Resources.CreatingMefContainer, Category.Debug, Priority.Low);
			Container = CreateContainer();
			if (Container == null)
			{
				throw new InvalidOperationException(Resources.NullCompositionContainerException);
			}
			base.Logger.Log(Resources.ConfiguringMefContainer, Category.Debug, Priority.Low);
			ConfigureContainer();
			base.Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
			ConfigureServiceLocator();
			base.Logger.Log(Resources.ConfiguringViewModelLocator, Category.Debug, Priority.Low);
			ConfigureViewModelLocator();
			base.Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
			ConfigureRegionAdapterMappings();
			base.Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
			ConfigureDefaultRegionBehaviors();
			base.Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
			RegisterFrameworkExceptionTypes();
			base.Logger.Log(Resources.CreatingShell, Category.Debug, Priority.Low);
			base.Shell = CreateShell();
			if (base.Shell != null)
			{
				base.Logger.Log(Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
				RegionManager.SetRegionManager(base.Shell, Container.GetExportedValue<IRegionManager>());
				base.Logger.Log(Resources.UpdatingRegions, Category.Debug, Priority.Low);
				RegionManager.UpdateRegions();
				base.Logger.Log(Resources.InitializingShell, Category.Debug, Priority.Low);
				InitializeShell();
			}
			IEnumerable<Lazy<object, object>> exports = Container.GetExports(typeof(IModuleManager), null, null);
			if (exports != null && exports.Count() > 0)
			{
				base.Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
				InitializeModules();
			}
			base.Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
		}

		protected virtual AggregateCatalog CreateAggregateCatalog()
		{
			return new AggregateCatalog();
		}

		protected virtual void ConfigureAggregateCatalog()
		{
		}

		protected virtual CompositionContainer CreateContainer()
		{
			return new CompositionContainer(AggregateCatalog);
		}

		protected virtual void ConfigureContainer()
		{
			RegisterBootstrapperProvidedTypes();
		}

		public virtual void RegisterDefaultTypesIfMissing()
		{
			AggregateCatalog = DefaultPrismServiceRegistrar.RegisterRequiredPrismServicesIfMissing(AggregateCatalog);
		}

		protected virtual void RegisterBootstrapperProvidedTypes()
		{
			Container.ComposeExportedValue(base.Logger);
			Container.ComposeExportedValue(base.ModuleCatalog);
			Container.ComposeExportedValue((IServiceLocator)new MefServiceLocatorAdapter(Container));
			Container.ComposeExportedValue(AggregateCatalog);
		}

		protected override void ConfigureServiceLocator()
		{
			IServiceLocator serviceLocator = Container.GetExportedValue<IServiceLocator>();
			ServiceLocator.SetLocatorProvider(() => serviceLocator);
		}

		protected override void InitializeShell()
		{
			Container.ComposeParts(base.Shell);
		}

		protected override void InitializeModules()
		{
			Container.GetExportedValue<IModuleManager>().Run();
		}
	}
}
