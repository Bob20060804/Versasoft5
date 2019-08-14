using System.Configuration;

namespace Prism.Modularity
{
	public class ModuleDependencyConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("moduleName", IsRequired = true, IsKey = true)]
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

		public ModuleDependencyConfigurationElement()
		{
		}

		public ModuleDependencyConfigurationElement(string moduleName)
		{
			base["moduleName"] = moduleName;
		}
	}
}
