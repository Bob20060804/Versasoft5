using System.Configuration;

namespace Prism.Modularity
{
	public class ModulesConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsDefaultCollection = true, IsKey = false)]
		public ModuleConfigurationElementCollection Modules
		{
			get
			{
				return (ModuleConfigurationElementCollection)base[""];
			}
			set
			{
				base[""] = value;
			}
		}
	}
}
