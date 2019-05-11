using System;
using System.IO;

namespace BR.AN.Logging
{
	[Serializable]
	public class LoggerSettings : IDisposable
	{
		private string _loggingPath;

		private int _loggingLevels;

		private int _loggingOutputs = 1;

		private short _maxLogFileAge = 30;

		private long _maxLogFileSize = 10485760L;

		private bool _writeCallerInfo = true;

		private string _settingsFileName = "";

		public string LoggingDirectory
		{
			get
			{
				return _loggingPath;
			}
			set
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(value);
				_loggingPath = directoryInfo.FullName;
			}
		}

		public int LoggingLevels
		{
			get
			{
				return _loggingLevels;
			}
			set
			{
				_loggingLevels = value;
			}
		}

		public int LoggingOutputs
		{
			get
			{
				return _loggingOutputs;
			}
			set
			{
				_loggingOutputs = value;
			}
		}

		public short MaxLogFileAge
		{
			get
			{
				return _maxLogFileAge;
			}
			set
			{
				_maxLogFileAge = value;
			}
		}

		public long MaxLogFileSize
		{
			get
			{
				return _maxLogFileSize;
			}
			set
			{
				_maxLogFileSize = value;
			}
		}

		public string SettingsFileName => _settingsFileName;

		public bool WriteCallerInfo
		{
			get
			{
				return _writeCallerInfo;
			}
			set
			{
				_writeCallerInfo = value;
			}
		}

		public LoggerSettings()
		{
			try
			{
				if (_loggingPath == null)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(LoggerSettingsProvider.GetSettingsFilePath().Parent.FullName);
					_loggingPath = directoryInfo.FullName + "\\Data";
				}
			}
			catch (Exception)
			{
			}
		}

		public void AddLevel(LoggingLevels level)
		{
			_loggingLevels |= (int)level;
		}

		public void AddLevel(LoggingOutputs level)
		{
			_loggingOutputs |= (int)level;
		}

		public void RemoveLevel(LoggingLevels level)
		{
			_loggingLevels &= (int)(~level);
		}

		public void RemoveLevel(LoggingOutputs level)
		{
			_loggingOutputs &= (int)(~level);
		}

		public void ClearAllLevels()
		{
			_loggingOutputs = 0;
			_loggingLevels = 0;
		}

		public bool IsLevelOn(LoggingLevels level)
		{
			return ((long)_loggingLevels & (long)level) != 0;
		}

		public bool IsLevelOn(LoggingOutputs level)
		{
			return ((long)_loggingOutputs & (long)level) != 0;
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
				_loggingLevels = 0;
				_loggingOutputs = 0;
				_maxLogFileAge = 0;
				_maxLogFileSize = 0L;
				_loggingPath = null;
				_settingsFileName = "";
			}
		}

		internal void SetSettingsFileName(string fullFileName)
		{
			_settingsFileName = fullFileName;
		}
	}
}
