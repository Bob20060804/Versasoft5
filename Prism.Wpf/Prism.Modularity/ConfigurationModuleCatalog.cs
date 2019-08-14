using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Prism.Modularity
{
	public class ConfigurationModuleCatalog : ModuleCatalog
	{
		public IConfigurationStore Store
		{
			get;
			set;
		}

		public ConfigurationModuleCatalog()
		{
			Store = new ConfigurationStore();
		}

		protected override void InnerLoad()
		{
			if (Store == null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.ConfigurationStoreCannotBeNull);
			}
			EnsureModulesDiscovered();
		}

		private static string GetFileAbsoluteUri(string filePath)
		{
			return new UriBuilder
			{
				Host = string.Empty,
				Scheme = Uri.UriSchemeFile,
				Path = Path.GetFullPath(filePath)
			}.Uri.ToString();
		}

		private void EnsureModulesDiscovered()
		{
			ModulesConfigurationSection modulesConfigurationSection = Store.RetrieveModuleConfigurationSection();
			if (modulesConfigurationSection != null)
			{
				foreach (ModuleConfigurationElement module in modulesConfigurationSection.Modules)
				{
					IList<string> list = new List<string>();
					if (module.Dependencies.Count > 0)
					{
						foreach (ModuleDependencyConfigurationElement dependency in module.Dependencies)
						{
							list.Add(dependency.ModuleName);
						}
					}
					ModuleInfo moduleInfo = new ModuleInfo(module.ModuleName, module.ModuleType)
					{
						Ref = GetFileAbsoluteUri(module.AssemblyFile),
						InitializationMode = ((!module.StartupLoaded) ? InitializationMode.OnDemand : InitializationMode.WhenAvailable)
					};
					moduleInfo.DependsOn.AddRange(list.ToArray());
					AddModule(moduleInfo);
				}
			}
		}
	}
}
