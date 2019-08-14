using System.Configuration;

namespace Prism.Modularity
{
	public class ConfigurationStore : IConfigurationStore
	{
		public ModulesConfigurationSection RetrieveModuleConfigurationSection()
		{
			return ConfigurationManager.GetSection("modules") as ModulesConfigurationSection;
		}
	}
}
