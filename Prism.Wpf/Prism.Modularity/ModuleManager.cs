using Prism.Logging;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Prism.Modularity
{
	public class ModuleManager : IModuleManager, IDisposable
	{
		private readonly IModuleInitializer moduleInitializer;

		private readonly IModuleCatalog moduleCatalog;

		private readonly ILoggerFacade loggerFacade;

		private IEnumerable<IModuleTypeLoader> typeLoaders;

		private HashSet<IModuleTypeLoader> subscribedToModuleTypeLoaders = new HashSet<IModuleTypeLoader>();

		protected IModuleCatalog ModuleCatalog => moduleCatalog;

		public virtual IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
		{
			get
			{
				if (typeLoaders == null)
				{
					typeLoaders = new List<IModuleTypeLoader>
					{
						new FileModuleTypeLoader()
					};
				}
				return typeLoaders;
			}
			set
			{
				typeLoaders = value;
			}
		}

		public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		public ModuleManager(IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog, ILoggerFacade loggerFacade)
		{
			if (moduleInitializer == null)
			{
				throw new ArgumentNullException("moduleInitializer");
			}
			if (moduleCatalog == null)
			{
				throw new ArgumentNullException("moduleCatalog");
			}
			if (loggerFacade == null)
			{
				throw new ArgumentNullException("loggerFacade");
			}
			this.moduleInitializer = moduleInitializer;
			this.moduleCatalog = moduleCatalog;
			this.loggerFacade = loggerFacade;
		}

		private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
		{
			if (this.ModuleDownloadProgressChanged != null)
			{
				this.ModuleDownloadProgressChanged(this, e);
			}
		}

		private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
		{
			RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
		}

		private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
		{
			if (this.LoadModuleCompleted != null)
			{
				this.LoadModuleCompleted(this, e);
			}
		}

		public void Run()
		{
			moduleCatalog.Initialize();
			LoadModulesWhenAvailable();
		}

		public void LoadModule(string moduleName)
		{
			IEnumerable<ModuleInfo> enumerable = from m in moduleCatalog.Modules
			where m.ModuleName == moduleName
			select m;
			if (enumerable == null || enumerable.Count() != 1)
			{
				throw new ModuleNotFoundException(moduleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.ModuleNotFound, new object[1]
				{
					moduleName
				}));
			}
			IEnumerable<ModuleInfo> moduleInfos = moduleCatalog.CompleteListWithDependencies(enumerable);
			LoadModuleTypes(moduleInfos);
		}

		protected virtual bool ModuleNeedsRetrieval(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			if (moduleInfo.State == ModuleState.NotStarted)
			{
				bool num = Type.GetType(moduleInfo.ModuleType) != null;
				if (num)
				{
					moduleInfo.State = ModuleState.ReadyForInitialization;
				}
				return !num;
			}
			return false;
		}

		private void LoadModulesWhenAvailable()
		{
			IEnumerable<ModuleInfo> modules = from m in moduleCatalog.Modules
			where m.InitializationMode == InitializationMode.WhenAvailable
			select m;
			IEnumerable<ModuleInfo> enumerable = moduleCatalog.CompleteListWithDependencies(modules);
			if (enumerable != null)
			{
				LoadModuleTypes(enumerable);
			}
		}

		private void LoadModuleTypes(IEnumerable<ModuleInfo> moduleInfos)
		{
			if (moduleInfos != null)
			{
				foreach (ModuleInfo moduleInfo in moduleInfos)
				{
					if (moduleInfo.State == ModuleState.NotStarted)
					{
						if (ModuleNeedsRetrieval(moduleInfo))
						{
							BeginRetrievingModule(moduleInfo);
						}
						else
						{
							moduleInfo.State = ModuleState.ReadyForInitialization;
						}
					}
				}
				LoadModulesThatAreReadyForLoad();
			}
		}

		protected virtual void LoadModulesThatAreReadyForLoad()
		{
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (ModuleInfo item in from m in moduleCatalog.Modules
				where m.State == ModuleState.ReadyForInitialization
				select m)
				{
					if (item.State != ModuleState.Initialized && AreDependenciesLoaded(item))
					{
						item.State = ModuleState.Initializing;
						InitializeModule(item);
						flag = true;
						break;
					}
				}
			}
		}

		private void BeginRetrievingModule(ModuleInfo moduleInfo)
		{
			IModuleTypeLoader typeLoaderForModule = GetTypeLoaderForModule(moduleInfo);
			moduleInfo.State = ModuleState.LoadingTypes;
			if (!subscribedToModuleTypeLoaders.Contains(typeLoaderForModule))
			{
				typeLoaderForModule.ModuleDownloadProgressChanged += IModuleTypeLoader_ModuleDownloadProgressChanged;
				typeLoaderForModule.LoadModuleCompleted += IModuleTypeLoader_LoadModuleCompleted;
				subscribedToModuleTypeLoaders.Add(typeLoaderForModule);
			}
			typeLoaderForModule.LoadModuleType(moduleInfo);
		}

		private void IModuleTypeLoader_ModuleDownloadProgressChanged(object sender, ModuleDownloadProgressChangedEventArgs e)
		{
			RaiseModuleDownloadProgressChanged(e);
		}

		private void IModuleTypeLoader_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				if (e.ModuleInfo.State != ModuleState.Initializing && e.ModuleInfo.State != ModuleState.Initialized)
				{
					e.ModuleInfo.State = ModuleState.ReadyForInitialization;
				}
				LoadModulesThatAreReadyForLoad();
			}
			else
			{
				RaiseLoadModuleCompleted(e);
				if (!e.IsErrorHandled)
				{
					HandleModuleTypeLoadingError(e.ModuleInfo, e.Error);
				}
			}
		}

		protected virtual void HandleModuleTypeLoadingError(ModuleInfo moduleInfo, Exception exception)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			ModuleTypeLoadingException ex = exception as ModuleTypeLoadingException;
			if (ex == null)
			{
				ex = new ModuleTypeLoadingException(moduleInfo.ModuleName, exception.Message, exception);
			}
			loggerFacade.Log(ex.Message, Category.Exception, Priority.High);
			throw ex;
		}

		private bool AreDependenciesLoaded(ModuleInfo moduleInfo)
		{
			IEnumerable<ModuleInfo> dependentModules = moduleCatalog.GetDependentModules(moduleInfo);
			if (dependentModules == null)
			{
				return true;
			}
			return dependentModules.Count((ModuleInfo requiredModule) => requiredModule.State != ModuleState.Initialized) == 0;
		}

		private IModuleTypeLoader GetTypeLoaderForModule(ModuleInfo moduleInfo)
		{
			foreach (IModuleTypeLoader moduleTypeLoader in ModuleTypeLoaders)
			{
				if (moduleTypeLoader.CanLoadModuleType(moduleInfo))
				{
					return moduleTypeLoader;
				}
			}
			throw new ModuleTypeLoaderNotFoundException(moduleInfo.ModuleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.NoRetrieverCanRetrieveModule, new object[1]
			{
				moduleInfo.ModuleName
			}), null);
		}

		private void InitializeModule(ModuleInfo moduleInfo)
		{
			if (moduleInfo.State == ModuleState.Initializing)
			{
				moduleInitializer.Initialize(moduleInfo);
				moduleInfo.State = ModuleState.Initialized;
				RaiseLoadModuleCompleted(moduleInfo, null);
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			foreach (IModuleTypeLoader moduleTypeLoader in ModuleTypeLoaders)
			{
				(moduleTypeLoader as IDisposable)?.Dispose();
			}
		}
	}
}
