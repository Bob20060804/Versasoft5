using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Resources;

namespace Prism.Modularity
{
	[ContentProperty("Items")]
	public class ModuleCatalog : IModuleCatalog
	{
		private class ModuleCatalogItemCollection : Collection<IModuleCatalogItem>, INotifyCollectionChanged
		{
			public event NotifyCollectionChangedEventHandler CollectionChanged;

			protected override void InsertItem(int index, IModuleCatalogItem item)
			{
				base.InsertItem(index, item);
				OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}

			protected void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
			{
				if (this.CollectionChanged != null)
				{
					this.CollectionChanged(this, eventArgs);
				}
			}
		}

		private readonly ModuleCatalogItemCollection items;

		private bool isLoaded;

		public Collection<IModuleCatalogItem> Items => items;

		public virtual IEnumerable<ModuleInfo> Modules => GrouplessModules.Union(Groups.SelectMany((ModuleInfoGroup g) => g));

		public IEnumerable<ModuleInfoGroup> Groups => Items.OfType<ModuleInfoGroup>();

		protected bool Validated
		{
			get;
			set;
		}

		protected IEnumerable<ModuleInfo> GrouplessModules => Items.OfType<ModuleInfo>();

		public ModuleCatalog()
		{
			items = new ModuleCatalogItemCollection();
			items.CollectionChanged += ItemsCollectionChanged;
		}

		public ModuleCatalog(IEnumerable<ModuleInfo> modules)
			: this()
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			foreach (ModuleInfo module in modules)
			{
				Items.Add(module);
			}
		}

		public static ModuleCatalog CreateFromXaml(Stream xamlStream)
		{
			if (xamlStream == null)
			{
				throw new ArgumentNullException("xamlStream");
			}
			return XamlReader.Load(xamlStream) as ModuleCatalog;
		}

		public static ModuleCatalog CreateFromXaml(Uri builderResourceUri)
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(builderResourceUri);
			if (resourceStream != null && resourceStream.Stream != null)
			{
				return CreateFromXaml(resourceStream.Stream);
			}
			return null;
		}

		public void Load()
		{
			isLoaded = true;
			InnerLoad();
		}

		public virtual IEnumerable<ModuleInfo> GetDependentModules(ModuleInfo moduleInfo)
		{
			EnsureCatalogValidated();
			return GetDependentModulesInner(moduleInfo);
		}

		public virtual IEnumerable<ModuleInfo> CompleteListWithDependencies(IEnumerable<ModuleInfo> modules)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			EnsureCatalogValidated();
			List<ModuleInfo> list = new List<ModuleInfo>();
			List<ModuleInfo> list2 = modules.ToList();
			while (list2.Count > 0)
			{
				ModuleInfo moduleInfo = list2[0];
				foreach (ModuleInfo dependentModule in GetDependentModules(moduleInfo))
				{
					if (!list.Contains(dependentModule) && !list2.Contains(dependentModule))
					{
						list2.Add(dependentModule);
					}
				}
				list2.RemoveAt(0);
				list.Add(moduleInfo);
			}
			return Sort(list);
		}

		public virtual void Validate()
		{
			ValidateUniqueModules();
			ValidateDependencyGraph();
			ValidateCrossGroupDependencies();
			ValidateDependenciesInitializationMode();
			Validated = true;
		}

		public virtual void AddModule(ModuleInfo moduleInfo)
		{
			Items.Add(moduleInfo);
		}

		public ModuleCatalog AddModule(Type moduleType, params string[] dependsOn)
		{
			return AddModule(moduleType, InitializationMode.WhenAvailable, dependsOn);
		}

		public ModuleCatalog AddModule(Type moduleType, InitializationMode initializationMode, params string[] dependsOn)
		{
			if (moduleType == null)
			{
				throw new ArgumentNullException("moduleType");
			}
			return AddModule(moduleType.Name, moduleType.AssemblyQualifiedName, initializationMode, dependsOn);
		}

		public ModuleCatalog AddModule(string moduleName, string moduleType, params string[] dependsOn)
		{
			return AddModule(moduleName, moduleType, InitializationMode.WhenAvailable, dependsOn);
		}

		public ModuleCatalog AddModule(string moduleName, string moduleType, InitializationMode initializationMode, params string[] dependsOn)
		{
			return AddModule(moduleName, moduleType, null, initializationMode, dependsOn);
		}

		public ModuleCatalog AddModule(string moduleName, string moduleType, string refValue, InitializationMode initializationMode, params string[] dependsOn)
		{
			if (moduleName == null)
			{
				throw new ArgumentNullException("moduleName");
			}
			if (moduleType == null)
			{
				throw new ArgumentNullException("moduleType");
			}
			ModuleInfo moduleInfo = new ModuleInfo(moduleName, moduleType);
			moduleInfo.DependsOn.AddRange(dependsOn);
			moduleInfo.InitializationMode = initializationMode;
			moduleInfo.Ref = refValue;
			Items.Add(moduleInfo);
			return this;
		}

		public virtual void Initialize()
		{
			if (!isLoaded)
			{
				Load();
			}
			Validate();
		}

		public virtual ModuleCatalog AddGroup(InitializationMode initializationMode, string refValue, params ModuleInfo[] moduleInfos)
		{
			if (moduleInfos == null)
			{
				throw new ArgumentNullException("moduleInfos");
			}
			ModuleInfoGroup moduleInfoGroup = new ModuleInfoGroup();
			moduleInfoGroup.InitializationMode = initializationMode;
			moduleInfoGroup.Ref = refValue;
			foreach (ModuleInfo item in moduleInfos)
			{
				moduleInfoGroup.Add(item);
			}
			items.Add(moduleInfoGroup);
			return this;
		}

		protected static string[] SolveDependencies(IEnumerable<ModuleInfo> modules)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			ModuleDependencySolver moduleDependencySolver = new ModuleDependencySolver();
			foreach (ModuleInfo module in modules)
			{
				moduleDependencySolver.AddModule(module.ModuleName);
				if (module.DependsOn != null)
				{
					foreach (string item in module.DependsOn)
					{
						moduleDependencySolver.AddDependency(module.ModuleName, item);
					}
				}
			}
			if (moduleDependencySolver.ModuleCount > 0)
			{
				return moduleDependencySolver.Solve();
			}
			return new string[0];
		}

		protected static void ValidateDependencies(IEnumerable<ModuleInfo> modules)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			List<string> second = (from m in modules
			select m.ModuleName).ToList();
			foreach (ModuleInfo module in modules)
			{
				if (module.DependsOn != null && module.DependsOn.Except(second).Any())
				{
					throw new ModularityException(module.ModuleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.ModuleDependenciesNotMetInGroup, new object[1]
					{
						module.ModuleName
					}));
				}
			}
		}

		protected virtual void InnerLoad()
		{
		}

		protected virtual IEnumerable<ModuleInfo> Sort(IEnumerable<ModuleInfo> modules)
		{
			string[] array = SolveDependencies(modules);
			foreach (string moduleName in array)
			{
				yield return modules.First((ModuleInfo m) => m.ModuleName == moduleName);
			}
		}

		protected virtual void ValidateUniqueModules()
		{
			List<string> moduleNames = (from m in Modules
			select m.ModuleName).ToList();
			string text = moduleNames.FirstOrDefault((string m) => moduleNames.Count((string m2) => m2 == m) > 1);
			if (text != null)
			{
				throw new DuplicateModuleException(text, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.DuplicatedModule, new object[1]
				{
					text
				}));
			}
		}

		protected virtual void ValidateDependencyGraph()
		{
			SolveDependencies(Modules);
		}

		protected virtual void ValidateCrossGroupDependencies()
		{
			ValidateDependencies(GrouplessModules);
			foreach (ModuleInfoGroup group in Groups)
			{
				ValidateDependencies(GrouplessModules.Union(group));
			}
		}

		protected virtual void ValidateDependenciesInitializationMode()
		{
			ModuleInfo moduleInfo = Modules.FirstOrDefault(delegate(ModuleInfo m)
			{
				if (m.InitializationMode == InitializationMode.WhenAvailable)
				{
					return GetDependentModulesInner(m).Any((ModuleInfo dependency) => dependency.InitializationMode == InitializationMode.OnDemand);
				}
				return false;
			});
			if (moduleInfo != null)
			{
				throw new ModularityException(moduleInfo.ModuleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StartupModuleDependsOnAnOnDemandModule, new object[1]
				{
					moduleInfo.ModuleName
				}));
			}
		}

		protected virtual IEnumerable<ModuleInfo> GetDependentModulesInner(ModuleInfo moduleInfo)
		{
			return from dependantModule in Modules
			where moduleInfo.DependsOn.Contains(dependantModule.ModuleName)
			select dependantModule;
		}

		protected virtual void EnsureCatalogValidated()
		{
			if (!Validated)
			{
				Validate();
			}
		}

		private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (Validated)
			{
				EnsureCatalogValidated();
			}
		}
	}
}
