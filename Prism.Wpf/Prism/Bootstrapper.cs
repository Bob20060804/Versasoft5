using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Regions.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Prism
{
	public abstract class Bootstrapper
	{
		protected ILoggerFacade Logger
		{
			get;
			set;
		}

		protected IModuleCatalog ModuleCatalog
		{
			get;
			set;
		}

		protected DependencyObject Shell
		{
			get;
			set;
		}

		protected virtual ILoggerFacade CreateLogger()
		{
			return new TextLogger();
		}

		public void Run()
		{
			Run(runWithDefaultConfiguration: true);
		}

		protected virtual IModuleCatalog CreateModuleCatalog()
		{
			return new ModuleCatalog();
		}

		protected virtual void ConfigureModuleCatalog()
		{
		}

		protected virtual void ConfigureViewModelLocator()
		{
			ViewModelLocationProvider.SetDefaultViewModelFactory((Type type) => ServiceLocator.Current.GetInstance(type));
		}

		protected virtual void RegisterFrameworkExceptionTypes()
		{
			ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ActivationException));
		}

		protected virtual void InitializeModules()
		{
			ServiceLocator.Current.GetInstance<IModuleManager>().Run();
		}

		protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			RegionAdapterMappings instance = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
			if (instance != null)
			{
				instance.RegisterMapping(typeof(Selector), ServiceLocator.Current.GetInstance<SelectorRegionAdapter>());
				instance.RegisterMapping(typeof(ItemsControl), ServiceLocator.Current.GetInstance<ItemsControlRegionAdapter>());
				instance.RegisterMapping(typeof(ContentControl), ServiceLocator.Current.GetInstance<ContentControlRegionAdapter>());
			}
			return instance;
		}

		protected virtual IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
		{
			IRegionBehaviorFactory instance = ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>();
			if (instance != null)
			{
				instance.AddIfMissing("ContextToDependencyObject", typeof(BindRegionContextToDependencyObjectBehavior));
				instance.AddIfMissing("ActiveAware", typeof(RegionActiveAwareBehavior));
				instance.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey, typeof(SyncRegionContextWithHostBehavior));
				instance.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey, typeof(RegionManagerRegistrationBehavior));
				instance.AddIfMissing("RegionMemberLifetimeBehavior", typeof(RegionMemberLifetimeBehavior));
				instance.AddIfMissing("ClearChildViews", typeof(ClearChildViewsRegionBehavior));
				instance.AddIfMissing("AutoPopulate", typeof(AutoPopulateRegionBehavior));
			}
			return instance;
		}

		protected virtual DependencyObject CreateShell()
		{
			return null;
		}

		protected virtual void InitializeShell()
		{
		}

		public abstract void Run(bool runWithDefaultConfiguration);

		protected abstract void ConfigureServiceLocator();
	}
}
