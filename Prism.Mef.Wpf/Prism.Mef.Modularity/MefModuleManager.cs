using Prism.Logging;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Prism.Mef.Modularity
{
	[Export(typeof(IModuleManager))]
	public class MefModuleManager : ModuleManager, IPartImportsSatisfiedNotification
	{
		[Import(AllowRecomposition = false)]
		private MefFileModuleTypeLoader mefFileModuleTypeLoader;

		private IEnumerable<IModuleTypeLoader> mefTypeLoaders;

		[ImportMany(AllowRecomposition = true)]
		protected IEnumerable<Lazy<IModule, IModuleExport>> ImportedModules
		{
			get;
			set;
		}

		public override IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
		{
			get
			{
				if (mefTypeLoaders == null)
				{
					mefTypeLoaders = new List<IModuleTypeLoader>
					{
						mefFileModuleTypeLoader
					};
				}
				return mefTypeLoaders;
			}
			set
			{
				mefTypeLoaders = value;
			}
		}

		[ImportingConstructor]
		public MefModuleManager(IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog, ILoggerFacade loggerFacade)
			: base(moduleInitializer, moduleCatalog, loggerFacade)
		{
		}

		public virtual void OnImportsSatisfied()
		{
			IDictionary<string, ModuleInfo> dictionary = base.ModuleCatalog.Modules.ToDictionary((ModuleInfo m) => m.ModuleName);
			foreach (Lazy<IModule, IModuleExport> importedModule in ImportedModules)
			{
				Type moduleType = importedModule.Metadata.ModuleType;
				ModuleInfo value = null;
				if (!dictionary.TryGetValue(importedModule.Metadata.ModuleName, out value))
				{
					ModuleInfo moduleInfo = new ModuleInfo
					{
						ModuleName = importedModule.Metadata.ModuleName,
						ModuleType = moduleType.AssemblyQualifiedName,
						InitializationMode = importedModule.Metadata.InitializationMode,
						State = ((importedModule.Metadata.InitializationMode != InitializationMode.OnDemand) ? ModuleState.ReadyForInitialization : ModuleState.NotStarted)
					};
					if (importedModule.Metadata.DependsOnModuleNames != null)
					{
						moduleInfo.DependsOn.AddRange(importedModule.Metadata.DependsOnModuleNames);
					}
					base.ModuleCatalog.AddModule(moduleInfo);
				}
				else
				{
					value.ModuleType = moduleType.AssemblyQualifiedName;
				}
			}
			LoadModulesThatAreReadyForLoad();
		}

		protected override bool ModuleNeedsRetrieval(ModuleInfo moduleInfo)
		{
			if (ImportedModules != null)
			{
				return !ImportedModules.Any((Lazy<IModule, IModuleExport> lazyModule) => lazyModule.Metadata.ModuleName == moduleInfo.ModuleName);
			}
			return true;
		}
	}
}
