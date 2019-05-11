using System;
using System.IO;
using System.Security;
using System.Xml.Serialization;

namespace BR.AN.Logging
{
	internal static class LoggerSettingsProvider
	{
		private static string _loggerSettingsFileName = "BR.AN.Logging.Settings";

		public static void LoadLoggingSettings(ref LoggerSettings loggerSettings, string uniqueLoggerName)
		{
			_loggerSettingsFileName = uniqueLoggerName + ".config";
			FileInfo fileInfo = new FileInfo(GetSettingsFileFullName());
			if (fileInfo.Exists)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoggerSettings));
					StreamReader streamReader = new StreamReader(fileInfo.FullName);
					loggerSettings = (LoggerSettings)xmlSerializer.Deserialize(streamReader);
					streamReader.Close();
					loggerSettings.SetSettingsFileName(fileInfo.FullName);
				}
				catch (Exception)
				{
					throw;
				}
			}
			else
			{
				try
				{
					SaveLoggingSettings(ref loggerSettings, uniqueLoggerName);
				}
				catch (Exception)
				{
				}
			}
		}

		public static void SaveLoggingSettings(ref LoggerSettings loggerSettings, string uniqueLoggerName)
		{
			_loggerSettingsFileName = uniqueLoggerName + ".config";
			FileInfo fileInfo = new FileInfo(GetSettingsFileFullName());
			DirectoryInfo directory = fileInfo.Directory;
			try
			{
				if (!directory.Exists)
				{
					directory.Create();
				}
			}
			catch (SecurityException inner)
			{
				throw new SecurityException("Can't save settings", inner);
			}
			catch (Exception)
			{
				throw;
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoggerSettings));
				StreamWriter streamWriter = new StreamWriter(fileInfo.FullName);
				xmlSerializer.Serialize(streamWriter, loggerSettings);
				streamWriter.Close();
				loggerSettings.SetSettingsFileName(fileInfo.FullName);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static DirectoryInfo GetSettingsFilePath()
		{
			return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BR\\Logging\\Settings\\");
		}

		public static string GetSettingsFileFullName()
		{
			return GetSettingsFilePath().FullName + _loggerSettingsFileName;
		}
	}
}
