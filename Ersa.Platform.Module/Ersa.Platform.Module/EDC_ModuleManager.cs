using Prism.Logging;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Module
{
	[Export]
	[Export(typeof(IModuleManager))]
	public sealed class EDC_ModuleManager : ModuleManager, IPartImportsSatisfiedNotification
	{
		[Import(AllowRecomposition = false)]
		private EDC_FileModuleTypeLoader m_fdcMefFileModuleTypeLoader;

		private IEnumerable<IModuleTypeLoader> m_fdcIModuleTypeLoaders;

		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<Lazy<IModule, IModuleExport>> PRO_fdcImportedModules
		{
			get;
			set;
		}

		public override IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
		{
			get
			{
				object enumerable = m_fdcIModuleTypeLoaders;
				if (enumerable == null)
				{
					List<IModuleTypeLoader> obj = new List<IModuleTypeLoader>
					{
						m_fdcMefFileModuleTypeLoader
					};
					IEnumerable<IModuleTypeLoader> enumerable2 = obj;
					m_fdcIModuleTypeLoaders = obj;
					enumerable = enumerable2;
				}
				return (IEnumerable<IModuleTypeLoader>)enumerable;
			}
			set
			{
				m_fdcIModuleTypeLoaders = value;
			}
		}

		public event EventHandler m_evtAlleModuleGeladen;

		[ImportingConstructor]
		public EDC_ModuleManager(IModuleInitializer i_fdcIModuleInitializer, IModuleCatalog i_fdcIModuleCatalog, ILoggerFacade i_fdcILoggerFacade)
			: base(i_fdcIModuleInitializer, i_fdcIModuleCatalog, i_fdcILoggerFacade)
		{
			base.LoadModuleCompleted += async delegate
			{
				await FUN_fdcModuleGeladenBehandelnAsync();
			};
		}

		public void OnImportsSatisfied()
		{
			IDictionary<string, ModuleInfo> dictionary = base.ModuleCatalog.Modules.ToDictionary((ModuleInfo l_fdcModule) => l_fdcModule.ModuleName);
			foreach (Lazy<IModule, IModuleExport> pRO_fdcImportedModule in PRO_fdcImportedModules)
			{
				Type moduleType = pRO_fdcImportedModule.Metadata.ModuleType;
				if (!dictionary.TryGetValue(pRO_fdcImportedModule.Metadata.ModuleName, out ModuleInfo value))
				{
					ModuleInfo moduleInfo = new ModuleInfo
					{
						ModuleName = pRO_fdcImportedModule.Metadata.ModuleName,
						ModuleType = moduleType.AssemblyQualifiedName,
						InitializationMode = pRO_fdcImportedModule.Metadata.InitializationMode,
						State = ((pRO_fdcImportedModule.Metadata.InitializationMode != InitializationMode.OnDemand) ? ModuleState.ReadyForInitialization : ModuleState.NotStarted)
					};
					if (pRO_fdcImportedModule.Metadata.DependsOnModuleNames != null)
					{
						moduleInfo.DependsOn.AddRange(pRO_fdcImportedModule.Metadata.DependsOnModuleNames);
					}
					base.ModuleCatalog.AddModule(moduleInfo);
				}
				else
				{
					value.ModuleType = moduleType.AssemblyQualifiedName;
				}
			}
		}

		protected override bool ModuleNeedsRetrieval(ModuleInfo i_fdcModuleInfo)
		{
			if (PRO_fdcImportedModules != null)
			{
				return PRO_fdcImportedModules.All((Lazy<IModule, IModuleExport> l_fdcLazyModule) => l_fdcLazyModule.Metadata.ModuleName != i_fdcModuleInfo.ModuleName);
			}
			return true;
		}

		private async Task FUN_fdcModuleGeladenBehandelnAsync()
		{
			if (!base.ModuleCatalog.Modules.Any((ModuleInfo i_edcmoduleInfo) => i_edcmoduleInfo.State != ModuleState.Initialized))
			{
				List<Task> list = (from i_edcAsyncModule in (from i_fdcLazy in PRO_fdcImportedModules
				select i_fdcLazy.Value).OfType<INF_AsyncModule>()
				select i_edcAsyncModule.PRO_fdcInitialisierung).ToList();
				if (list.Any())
				{
					await Task.WhenAll(list);
				}
				SUB_AlleModuleGeladenEventFeuern();
			}
		}

		private void SUB_AlleModuleGeladenEventFeuern()
		{
			this.m_evtAlleModuleGeladen?.Invoke(this, null);
		}
	}
}
