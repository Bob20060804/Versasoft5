using System.Configuration;

namespace Prism.Modularity
{
	public class ModuleConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("assemblyFile", IsRequired = true)]
		public string AssemblyFile
		{
			get
			{
				return (string)base["assemblyFile"];
			}
			set
			{
				base["assemblyFile"] = value;
			}
		}

		[ConfigurationProperty("moduleType", IsRequired = true)]
		public string ModuleType
		{
			get
			{
				return (string)base["moduleType"];
			}
			set
			{
				base["moduleType"] = value;
			}
		}

		[ConfigurationProperty("moduleName", IsRequired = true)]
		public string ModuleName
		{
			get
			{
				return (string)base["moduleName"];
			}
			set
			{
				base["moduleName"] = value;
			}
		}

		[ConfigurationProperty("startupLoaded", IsRequired = false, DefaultValue = true)]
		public bool StartupLoaded
		{
			get
			{
				return (bool)base["startupLoaded"];
			}
			set
			{
				base["startupLoaded"] = value;
			}
		}

		[ConfigurationProperty("dependencies", IsDefaultCollection = true, IsKey = false)]
		public ModuleDependencyCollection Dependencies
		{
			get
			{
				return (ModuleDependencyCollection)base["dependencies"];
			}
			set
			{
				base["dependencies"] = value;
			}
		}

		public ModuleConfigurationElement()
		{
		}

		public ModuleConfigurationElement(string assemblyFile, string moduleType, string moduleName, bool startupLoaded)
		{
			base["assemblyFile"] = assemblyFile;
			base["moduleType"] = moduleType;
			base["moduleName"] = moduleName;
			base["startupLoaded"] = startupLoaded;
		}
	}
}
