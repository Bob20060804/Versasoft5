using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BR.AN.Logging
{
	public class Logger : IDisposable
	{
		private static Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();

		private readonly LoggerSettings _settings;

		private readonly DateTime _currentLogfileDate = DateTime.MinValue;

		private readonly string _uniqueLoggerName;

		private EventLog _loggerEventLog;

		private string _currentLogfileName;

		private readonly List<string> _lastCallers = new List<string>();

		public LoggerSettings Settings => _settings;

		public FileInfo SettingsFile => new FileInfo(_settings.LoggingDirectory + "\\" + _currentLogfileName);

		private Logger(string uniqueLoggerName)
		{
			_uniqueLoggerName = uniqueLoggerName;
			if (_settings == null)
			{
				if (_settings == null)
				{
					_settings = new LoggerSettings();
				}
				try
				{
					LoggerSettingsProvider.LoadLoggingSettings(ref _settings, uniqueLoggerName);
				}
				catch (Exception)
				{
				}
			}
			if (_settings.IsLevelOn(LoggingOutputs.FlatFile))
			{
				if (!DateTime.Today.Equals(_currentLogfileDate))
				{
					_currentLogfileDate = DateTime.Today;
					_currentLogfileName = "";
				}
				if (string.IsNullOrEmpty(_currentLogfileName))
				{
					_currentLogfileName = _uniqueLoggerName + DateTime.Today.ToString("_yyyyMMdd") + ".log";
					DirectoryInfo directoryInfo = new DirectoryInfo(_settings.LoggingDirectory);
					if (directoryInfo.Exists)
					{
						FileInfo[] files = directoryInfo.GetFiles(_uniqueLoggerName + "_*.log");
						FileInfo[] array = files;
						foreach (FileInfo fileInfo in array)
						{
							if (fileInfo.LastWriteTime < DateTime.Now.AddDays(_settings.MaxLogFileAge * -1))
							{
								try
								{
									fileInfo.Delete();
								}
								catch
								{
								}
							}
						}
					}
				}
			}
			_settings.IsLevelOn(LoggingOutputs.FlatFile);
		}

		public static Logger GetLogger()
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileInfo fileInfo = new FileInfo(callingAssembly.Location);
			return GetLogger(fileInfo.Name.Replace(fileInfo.Extension, ""));
		}

		public static Logger GetLogger(string uniqueLoggerName)
		{
			if (!_loggers.ContainsKey(uniqueLoggerName))
			{
				Logger value = new Logger(uniqueLoggerName);
				_loggers.Add(uniqueLoggerName, value);
			}
			return _loggers[uniqueLoggerName];
		}

		public void ResetSettings()
		{
			_currentLogfileName = "";
		}

		public void Write(string text)
		{
			Write(text, LoggingType.Information);
		}

		public void Write(string text, LoggingType currentType)
		{
			string caller = "";
			if (_settings.WriteCallerInfo)
			{
				StackTrace stackTrace = new StackTrace();
				string name = stackTrace.GetFrame(1).GetMethod().Name;
				string name2 = stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
				caller = name2 + "." + name;
			}
			Write(caller, text, currentType);
		}

		public void Write(string caller, string text, LoggingType currentType)
		{
			if (!CheckLoggingType(currentType))
			{
				return;
			}
			string str = DateTime.Now.ToString("HH:mm:ss.ffff ") + "\t";
			switch (currentType)
			{
			case LoggingType.Information:
				str += "INF\t";
				break;
			case LoggingType.Warning:
				str += "WRN\t";
				break;
			case LoggingType.Error:
				str += "ERR\t";
				break;
			case LoggingType.Debug:
				str += "DBG\t";
				break;
			}
			if (_settings.WriteCallerInfo)
			{
				str = str + caller + "\t";
			}
			str += text;
			if (_settings.IsLevelOn(LoggingOutputs.FlatFile))
			{
				FileInfo fileInfo = new FileInfo(_settings.LoggingDirectory + "\\" + _currentLogfileName);
				try
				{
					if (!fileInfo.Directory.Exists)
					{
						fileInfo.Directory.Create();
					}
				}
				catch (Exception)
				{
				}
				if (!fileInfo.Exists)
				{
					_currentLogfileName = _uniqueLoggerName + DateTime.Today.ToString("_yyyyMMdd") + ".log";
					fileInfo = new FileInfo(_settings.LoggingDirectory + "\\" + _currentLogfileName);
				}
				if (fileInfo.Exists && fileInfo.Length > _settings.MaxLogFileSize)
				{
					_currentLogfileName = fileInfo.Name.Replace(fileInfo.Extension, "") + "_" + DateTime.Now.Ticks + fileInfo.Extension;
				}
				try
				{
					StreamWriter streamWriter = new StreamWriter(_settings.LoggingDirectory + "\\" + _currentLogfileName, append: true);
					streamWriter.WriteLine(str);
					streamWriter.Close();
				}
				catch (Exception)
				{
				}
			}
			if (_settings.IsLevelOn(LoggingOutputs.EventLog))
			{
				if (_loggerEventLog == null)
				{
					try
					{
						if (!EventLog.SourceExists("BrAutomation"))
						{
							EventLog.CreateEventSource("BrAutomation", "BrAutomation");
						}
						_loggerEventLog = new EventLog();
						_loggerEventLog.Source = "BrAutomation";
					}
					catch (Exception)
					{
					}
				}
				try
				{
					EventLogEntryType type = EventLogEntryType.Information;
					switch (currentType)
					{
					case LoggingType.Information:
					case LoggingType.Debug:
						type = EventLogEntryType.Information;
						break;
					case LoggingType.Warning:
						type = EventLogEntryType.Warning;
						break;
					case LoggingType.Error:
						type = EventLogEntryType.Error;
						break;
					}
					if (_loggerEventLog != null)
					{
						_loggerEventLog.WriteEntry(str, type);
					}
				}
				catch (Exception)
				{
				}
			}
			if (!_settings.IsLevelOn(LoggingOutputs.VisualStudioOutputWindow))
			{
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_loggers = null;
				_settings.Dispose();
				_loggerEventLog.Dispose();
			}
		}

		private bool CheckLoggingType(LoggingType currentType)
		{
			bool result = true;
			switch (currentType)
			{
			case LoggingType.Information:
				if (!_settings.IsLevelOn(LoggingLevels.Information))
				{
					result = false;
				}
				break;
			case LoggingType.Warning:
				if (!_settings.IsLevelOn(LoggingLevels.Warning))
				{
					result = false;
				}
				break;
			case LoggingType.Error:
				if (!_settings.IsLevelOn(LoggingLevels.Error))
				{
					result = false;
				}
				break;
			case LoggingType.Debug:
				if (!_settings.IsLevelOn(LoggingLevels.Debug))
				{
					result = false;
				}
				break;
			}
			return result;
		}

		public void WriteMethodInfo()
		{
			string text = "";
			string str = "";
			if (_settings.WriteCallerInfo)
			{
				StackTrace stackTrace = new StackTrace();
				string name = stackTrace.GetFrame(1).GetMethod().Name;
				string name2 = stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
				text = name2 + "." + name;
				int num = -1;
				for (int i = 0; i < _lastCallers.Count; i++)
				{
					for (int j = 2; j < stackTrace.FrameCount; j++)
					{
						if (stackTrace.GetFrame(j).GetMethod() != null && stackTrace.GetFrame(j).GetMethod().ReflectedType != null)
						{
							string name3 = stackTrace.GetFrame(j).GetMethod().Name;
							string name4 = stackTrace.GetFrame(j).GetMethod().ReflectedType.Name;
							if (_lastCallers[i] == name4 + "." + name3)
							{
								num = i;
							}
						}
					}
				}
				if (num != -1)
				{
					str = new string(' ', num * 2);
					_lastCallers.RemoveRange(num + 1, _lastCallers.Count - num - 1);
					_lastCallers.Add(text);
				}
				else
				{
					_lastCallers.Clear();
					_lastCallers.Add(text);
				}
			}
			Write(str + text, "", LoggingType.Information);
		}
	}
}
