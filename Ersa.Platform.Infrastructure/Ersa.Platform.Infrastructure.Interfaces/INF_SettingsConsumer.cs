using System.Configuration;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_SettingsConsumer
	{
		string PRO_strConfigSectionName
		{
			get;
		}

		void SUB_SettingsAktualisieren(ConfigurationSection i_fdcConfigSection);

		void SUB_SettingsUebernehmen(ConfigurationSection i_fdcConfigSection);
	}
}
