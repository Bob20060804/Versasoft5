using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ersa.Platform.Dienste
{
	[Export(typeof(INF_VisuSettingsDienst))]
	public class EDC_VisuSettingsDienst : INF_VisuSettingsDienst
	{
		private readonly object m_objLock = new object();

		private Configuration m_fdcConfiguration;

		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<IDictionary<string, Func<ConfigurationSection>>> PRO_enuSectionErsteller
		{
			get;
			set;
		}

		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<IEnumerable<INF_SettingsConsumer>> PRO_enuSettingsConsumer
		{
			get;
			set;
		}

		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<IList<EDC_GlobalSetting>> PRO_enuGlobalSettings
		{
			get;
			set;
		}

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		public void SUB_Initialisieren(string i_strSettingsFilePath)
		{
			try
			{
				SUB_InitializeSettingsFile(i_strSettingsFilePath);
			}
			catch (Exception ex)
			{
				Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, ex.Message, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, ex);
				File.Delete(i_strSettingsFilePath);
				throw new Exception("The settings file (" + i_strSettingsFilePath + ") is corrupted.\nThe file has been recreated.\nPlease restart the application and reenter your settings for the language, protocol and folders.");
			}
			finally
			{
				if (m_fdcConfiguration == null)
				{
					SUB_InitializeSettingsFile(i_strSettingsFilePath);
				}
				if (!m_fdcConfiguration.HasFile)
				{
					SUB_GlobaleSettingsSynchronisieren();
					lock (m_objLock)
					{
						m_fdcConfiguration.Save();
					}
				}
				SUB_GlobaleSettingsUebernehmen();
			}
		}

		public void SUB_SettingsAnwenden()
		{
			SUB_GlobaleSettingsSynchronisieren();
			SUB_SectionSettingsSynchronisieren();
			SUB_GlobaleSettingsUebernehmen();
			SUB_ConfigSectionSettingsUebernehmen();
			lock (m_objLock)
			{
				m_fdcConfiguration.Save();
			}
		}

		public void SUB_Speichern()
		{
			if (m_fdcConfiguration != null)
			{
				SUB_ConfigSectionSettingsAktualisieren();
				SUB_GlobaleSettingsAktualisieren();
				lock (m_objLock)
				{
					m_fdcConfiguration.Save();
				}
			}
		}

		public string FUN_strGlobalSettingWertErmitteln(string i_strKey)
		{
			if (m_fdcConfiguration == null)
			{
				return null;
			}
			KeyValueConfigurationCollection fdcAppsettings = FUN_fdcHoleSettings();
			return (from strKey in fdcAppsettings.AllKeys
			where strKey == i_strKey
			select fdcAppsettings[strKey].Value).FirstOrDefault();
		}

		public ConfigurationSection FUN_fdcSektionErmitteln(string i_strName)
		{
			lock (m_objLock)
			{
				return m_fdcConfiguration.Sections.Get(i_strName);
			}
		}

		private void SUB_SectionSettingsSynchronisieren()
		{
			SUB_HinzugekommeneConfigSectionsAufnehmen();
			SUB_NichtMehrVorhandeneConfigSectionsEntfernen();
		}

		private void SUB_NichtMehrVorhandeneConfigSectionsEntfernen()
		{
			IEnumerable<KeyValuePair<string, Func<ConfigurationSection>>> enuConfigSectionErsteller = PRO_enuSectionErsteller.SelectMany((IDictionary<string, Func<ConfigurationSection>> i_lstErsteller) => i_lstErsteller);
			lock (m_objLock)
			{
				foreach (string item in m_fdcConfiguration.Sections.Keys.OfType<string>().Where(delegate(string i_strKey)
				{
					if (i_strKey.StartsWith("ersa.", ignoreCase: true, CultureInfo.InvariantCulture))
					{
						return enuConfigSectionErsteller.All((KeyValuePair<string, Func<ConfigurationSection>> i_edcErsteller) => i_edcErsteller.Key != i_strKey);
					}
					return false;
				}).ToList())
				{
					m_fdcConfiguration.Sections.Remove(item);
				}
			}
		}

		private void SUB_HinzugekommeneConfigSectionsAufnehmen()
		{
			IEnumerable<KeyValuePair<string, Func<ConfigurationSection>>> enumerable = from i_edcSection in PRO_enuSectionErsteller.SelectMany((IDictionary<string, Func<ConfigurationSection>> i_lstErsteller) => i_lstErsteller)
			where FUN_fdcSektionErmitteln(i_edcSection.Key) == null
			select i_edcSection;
			lock (m_objLock)
			{
				foreach (KeyValuePair<string, Func<ConfigurationSection>> item in enumerable)
				{
					m_fdcConfiguration.Sections.Add(item.Key, item.Value());
				}
			}
		}

		private void SUB_GlobaleSettingsSynchronisieren()
		{
			SUB_HinzugekommeneGlobaleSettingsAufnehmen();
			SUB_NichtMehrVorhandeneGlobaleSettingsEntfernen();
		}

		private void SUB_NichtMehrVorhandeneGlobaleSettingsEntfernen()
		{
			KeyValueConfigurationCollection keyValueConfigurationCollection = FUN_fdcHoleSettings();
			IEnumerable<EDC_GlobalSetting> enuGlobaleSettings = PRO_enuGlobalSettings.SelectMany((IList<EDC_GlobalSetting> i_lstSettings) => i_lstSettings);
			foreach (string item in from i_strKey in keyValueConfigurationCollection.AllKeys
			where enuGlobaleSettings.All((EDC_GlobalSetting i_edcSetting) => i_edcSetting.PRO_strKey != i_strKey)
			select i_strKey)
			{
				keyValueConfigurationCollection.Remove(item);
			}
		}

		private void SUB_HinzugekommeneGlobaleSettingsAufnehmen()
		{
			KeyValueConfigurationCollection fdcAppsettings = FUN_fdcHoleSettings();
			foreach (EDC_GlobalSetting item in from i_edcSetting in PRO_enuGlobalSettings.SelectMany((IList<EDC_GlobalSetting> i_lstSettings) => i_lstSettings)
			where !fdcAppsettings.AllKeys.Contains(i_edcSetting.PRO_strKey)
			select i_edcSetting)
			{
				fdcAppsettings.Add(item.PRO_strKey, item.PRO_strDefaultWert);
			}
		}

		private void SUB_ConfigSectionSettingsAktualisieren()
		{
			if (PRO_enuSettingsConsumer != null)
			{
				foreach (INF_SettingsConsumer item in PRO_enuSettingsConsumer.SelectMany((IEnumerable<INF_SettingsConsumer> i_edcSettingsconsumer) => (i_edcSettingsconsumer as IList<INF_SettingsConsumer>) ?? i_edcSettingsconsumer.ToList()))
				{
					ConfigurationSection i_fdcConfigSection = FUN_fdcSektionErmitteln(item.PRO_strConfigSectionName);
					item.SUB_SettingsAktualisieren(i_fdcConfigSection);
				}
			}
		}

		private void SUB_GlobaleSettingsAktualisieren()
		{
			if (PRO_enuGlobalSettings != null)
			{
				KeyValueConfigurationCollection fdcAppsettings = FUN_fdcHoleSettings();
				foreach (EDC_GlobalSetting item in from i_edcGlobalSetting in PRO_enuGlobalSettings.SelectMany((IList<EDC_GlobalSetting> i_lstSettings) => i_lstSettings)
				where fdcAppsettings.AllKeys.Contains(i_edcGlobalSetting.PRO_strKey)
				select i_edcGlobalSetting)
				{
					fdcAppsettings[item.PRO_strKey].Value = item.PRO_strWert;
				}
			}
		}

		private void SUB_GlobaleSettingsUebernehmen()
		{
			KeyValueConfigurationCollection keyValueConfigurationCollection = FUN_fdcHoleSettings();
			List<EDC_GlobalSetting> source = PRO_enuGlobalSettings.SelectMany((IList<EDC_GlobalSetting> i_lstSettings) => i_lstSettings).ToList();
			string[] allKeys = keyValueConfigurationCollection.AllKeys;
			foreach (string strKey in allKeys)
			{
				EDC_GlobalSetting eDC_GlobalSetting = source.SingleOrDefault((EDC_GlobalSetting i_edcSetting) => i_edcSetting.PRO_strKey == strKey);
				if (eDC_GlobalSetting != null)
				{
					eDC_GlobalSetting.PRO_strWert = keyValueConfigurationCollection[strKey].Value;
					eDC_GlobalSetting.PRO_delWertGeaendertAktion(eDC_GlobalSetting);
				}
			}
		}

		private void SUB_ConfigSectionSettingsUebernehmen()
		{
			foreach (INF_SettingsConsumer item in PRO_enuSettingsConsumer.SelectMany((IEnumerable<INF_SettingsConsumer> i_edcSettingsconsumer) => (i_edcSettingsconsumer as IList<INF_SettingsConsumer>) ?? i_edcSettingsconsumer.ToList()))
			{
				ConfigurationSection i_fdcConfigSection = FUN_fdcSektionErmitteln(item.PRO_strConfigSectionName);
				item.SUB_SettingsUebernehmen(i_fdcConfigSection);
			}
		}

		private void SUB_InitializeSettingsFile(string i_strSettingsFilePath)
		{
			lock (m_objLock)
			{
				ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
				{
					ExeConfigFilename = i_strSettingsFilePath
				};
				m_fdcConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			}
		}

		private KeyValueConfigurationCollection FUN_fdcHoleSettings()
		{
			lock (m_objLock)
			{
				return m_fdcConfiguration.AppSettings.Settings;
			}
		}
	}
}
