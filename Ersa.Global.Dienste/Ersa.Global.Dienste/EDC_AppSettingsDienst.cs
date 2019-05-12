using Ersa.Global.Dienste.Interfaces;
using System.ComponentModel.Composition;
using System.Configuration;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_AppSettingsDienst))]
	public class EDC_AppSettingsDienst : INF_AppSettingsDienst
	{
		private readonly object m_objLock = new object();

		public string FUN_strAppSettingErmitteln(string i_strKey)
		{
			return ConfigurationManager.AppSettings[i_strKey];
		}

		public void SUB_AppSettingSpeichern(string i_strKey, string i_strWert)
		{
			lock (m_objLock)
			{
				Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				if (configuration.AppSettings.Settings[i_strKey] != null && !(configuration.AppSettings.Settings[i_strKey].Value == i_strWert))
				{
					configuration.AppSettings.Settings[i_strKey].Value = i_strWert;
					configuration.Save(ConfigurationSaveMode.Modified);
					ConfigurationManager.RefreshSection("appSettings");
				}
			}
		}
	}
}
