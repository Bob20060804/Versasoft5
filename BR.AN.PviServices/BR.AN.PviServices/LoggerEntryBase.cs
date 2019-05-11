using BR.AN.PviServices.EventDescription;
using System;

namespace BR.AN.PviServices
{
	public class LoggerEntryBase : IDisposable
	{
		internal string propRuntimeVersion;

		internal int propFacilityCode;

		internal int propCustomerCode;

		internal int propEventId;

		private ulong propUKey;

		protected string propLoggerModuleName;

		internal LevelType propLevelType;

		internal string TimestampUtc;

		internal DateTime propDateTime;

		internal uint propErrorInfo;

		internal uint propErrorNumber;

		internal string propASCIIData;

		internal byte[] propBinary;

		internal Exception propException;

		internal string propTask;

		internal uint propID;

		internal uint _internId;

		internal int propArrayIndex;

		internal int propSArrayIndex;

		internal bool propDisposed;

		internal object propParent;

		private string propCurrentCulture = string.Empty;

		private string propErrorDescription = string.Empty;

		public LevelType LevelType => propLevelType;

		public DateTime DateTime => propDateTime;

		[CLSCompliant(false)]
		public uint ErrorNumber
		{
			get
			{
				return propErrorNumber;
			}
		}

		public long ErrorInfo => propErrorInfo;

		[CLSCompliant(false)]
		public uint ModuleIndex
		{
			get
			{
				if (propException != null && propException.Backtrace != null)
				{
					return propException.Backtrace.propInfo;
				}
				return 0u;
			}
		}

		[CLSCompliant(false)]
		public uint TaskIndex
		{
			get
			{
				if (propException != null && propException.Backtrace != null)
				{
					return propException.Backtrace.propTaskIdx;
				}
				return uint.MaxValue;
			}
		}

		[CLSCompliant(false)]
		public uint CodeOffset
		{
			get
			{
				if (propException != null && propException.Backtrace != null && propException.Backtrace.PCInfo != null)
				{
					return propException.Backtrace.PCInfo.CodeOffset;
				}
				return 0u;
			}
		}

		public string Task => propTask;

		internal LogBookSnapshot LogBookSnapshot
		{
			get;
			set;
		}

		internal LogEntry LogBookEntry
		{
			get;
			set;
		}

		public virtual string ErrorDescription
		{
			get
			{
				string text = "en";
				if (Service != null)
				{
					text = Service.Language;
				}
				if (propErrorDescription == string.Empty)
				{
					MakeErrorDescriptionText(text);
					return propErrorDescription;
				}
				if (propCurrentCulture.CompareTo(text) == 0)
				{
					return propErrorDescription;
				}
				propCurrentCulture = text;
				MakeErrorDescriptionText(text);
				return propErrorDescription;
			}
		}

		public string ErrorText => propASCIIData;

		public byte[] Binary => propBinary;

		public Exception Exception => propException;

		[CLSCompliant(false)]
		public uint ID
		{
			get
			{
				return propID;
			}
		}

		public string Key => propUKey.ToString();

		[CLSCompliant(false)]
		public ulong UniqueKey
		{
			get
			{
				return propUKey;
			}
		}

		[CLSCompliant(false)]
		public uint InternId
		{
			get
			{
				return _internId;
			}
		}

		public Logger LoggerModule => (Logger)propParent;

		public string LoggerModuleName => propLoggerModuleName;

		public Service Service
		{
			get
			{
				if (propParent is Service)
				{
					return (Service)propParent;
				}
				if (propParent is Cpu)
				{
					return ((Cpu)propParent).Service;
				}
				if (propParent is Logger)
				{
					return ((Logger)propParent).Service;
				}
				return null;
			}
		}

		public event DisposeEventHandler Disposing;

		private void Initialize(object parent, DateTime dateTime, bool addKeyOnly, bool reverseOrder)
		{
			propFacilityCode = 0;
			propEventId = 0;
			propLoggerModuleName = "";
			if (parent is Logger)
			{
				propLoggerModuleName = ((Logger)parent).Name;
			}
			propRuntimeVersion = "";
			propDisposed = false;
			propUKey = 0uL;
			propArrayIndex = 0;
			propSArrayIndex = -1;
			propException = null;
			propParent = parent;
			propDateTime = dateTime;
			propID = 0u;
			propLevelType = LevelType.Success;
			propErrorNumber = 0u;
			propASCIIData = "";
			propBinary = null;
			propTask = "";
			propErrorInfo = 0u;
			if (propParent is Logger)
			{
				if (reverseOrder)
				{
					propID = ((Logger)propParent).DecrementEntryID();
				}
				else
				{
					propID = ((Logger)propParent).IncrementEntryID();
				}
			}
		}

		internal LoggerEntryBase()
		{
			DateTime dateTime = new DateTime(0L);
			Initialize(null, dateTime, addKeyOnly: false, reverseOrder: false);
		}

		[CLSCompliant(false)]
		public LoggerEntryBase(DateTime dateTime)
		{
			Initialize(null, dateTime, addKeyOnly: false, reverseOrder: false);
		}

		[CLSCompliant(false)]
		public LoggerEntryBase(object parent, DateTime dateTime)
		{
			Initialize(parent, dateTime, addKeyOnly: false, reverseOrder: false);
		}

		[CLSCompliant(false)]
		public LoggerEntryBase(object parent, string strDT, short offsetUtc)
		{
			string text = "0";
			TimestampUtc = strDT;
			string text2 = strDT.Replace(',', '.');
			int num = text2.IndexOf(".");
			long value = 0L;
			if (text2.Length - num - 1 > 1)
			{
				text = text2.Substring(num + 1, text2.Length - num - 1);
			}
			text2 = text2.Substring(0, num);
			DateTime dateTime;
			if (text2.IndexOf('-') != -1)
			{
				value = Convert.ToInt32(text);
				string[] array = text2.Split('-');
				dateTime = ((array.Length != 6) ? default(DateTime) : new DateTime(Convert.ToInt32(array.GetValue(0).ToString()), Convert.ToInt32(array.GetValue(1).ToString()), Convert.ToInt32(array.GetValue(2).ToString()), Convert.ToInt32(array.GetValue(3).ToString()), Convert.ToInt32(array.GetValue(4).ToString()), Convert.ToInt32(array.GetValue(5).ToString())));
			}
			else
			{
				if (text != "0")
				{
					value = Convert.ToInt64(text) / 100;
				}
				dateTime = Pvi.UInt32ToDateTime(Convert.ToUInt32(text2));
			}
			dateTime = dateTime.AddTicks(value);
			if (offsetUtc != 0)
			{
				dateTime = dateTime.AddTicks((long)offsetUtc * 60L * 10000000);
			}
			Initialize(parent, dateTime, addKeyOnly: false, reverseOrder: false);
		}

		[CLSCompliant(false)]
		public LoggerEntryBase(object parent, DateTime dateTime, bool addKeyOnly, bool reverseOrder)
		{
			Initialize(parent, dateTime, addKeyOnly, reverseOrder);
		}

		~LoggerEntryBase()
		{
			Dispose(disposing: false);
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public void Dispose()
		{
			if (!propDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			if (propDisposed)
			{
				return;
			}
			OnDisposing(disposing);
			if (disposing)
			{
				propDisposed = true;
				if (propBinary != null)
				{
					propBinary = null;
				}
				propASCIIData = null;
				propException = null;
				propParent = null;
				propTask = null;
				propParent = null;
			}
		}

		public override string ToString()
		{
			string text = "";
			if (propBinary != null)
			{
				for (int i = 0; i < propBinary.GetLength(0); i++)
				{
					text += $"{propBinary.GetValue(i):X2} ";
				}
			}
			return "ID=\"" + propID.ToString() + "\" LevelType=\"" + propLevelType.ToString() + "\" DateTime=\"" + propDateTime.ToString("s") + "." + $"{propDateTime.Millisecond:000}" + "\" ErrorNumber=\"" + propErrorNumber.ToString() + "\" ErrorInfo=\"0x" + $"{ErrorInfo:X8}" + "\" ASCII=\"" + propASCIIData + "\" CodeOffset=\"0x" + $"{CodeOffset:X8}" + "\" ErrorDescription=\"" + ErrorDescription + "\" Task=\"" + propTask + "\"" + ((propException != null) ? propException.ToString() : ("\" Binary=\"" + text));
		}

		internal string LevelToString(int levelType)
		{
			switch (levelType)
			{
			case 0:
				return "Success";
			case 1:
				return "Info";
			case 2:
				return "Warning";
			case 3:
				return "Fatal";
			case 4:
				return "Debug";
			case 129:
				return "FatalUser";
			case 130:
				return "WarningUser";
			default:
				return "Info";
			}
		}

		internal string DateTimeToString()
		{
			string text = "";
			long num = DateTime.Ticks % 10000000 / 10;
			text = $"{num:000000}";
			return $"{DateTime.Year}-{DateTime.Month:00}-{DateTime.Day:00}T{DateTime.Hour:00}:{DateTime.Minute:00}:{DateTime.Second:00}.{text}";
		}

		internal virtual string ToStringHTM(uint contentVersion)
		{
			string result = "";
			if (contentVersion < 4112)
			{
				string text = "<td align=\"right\">";
				result = $"{text}{LevelToString((int)LevelType)}</td>\r\n<td align=\"center\" width=\"220\">{DateTimeToString()}</td>\r\n{text}{Task}</td>\r\n";
			}
			return result;
		}

		internal virtual string ToStringCSV(uint contentVersion)
		{
			string result = "";
			if (contentVersion < 4112)
			{
				result = $"\"{LevelToString((int)LevelType)}\";\"{DateTimeToString()}\";\"{Task}\";";
			}
			return result;
		}

		internal virtual string BinaryToString()
		{
			string text = "";
			if (propBinary != null)
			{
				if (0 < propBinary.GetLength(0))
				{
					text = $"{propBinary.GetValue(0):X2}";
					for (int i = 1; i < propBinary.GetLength(0); i++)
					{
						text += $" {propBinary.GetValue(i):X2}";
					}
				}
			}
			else
			{
				text = "";
			}
			return text;
		}

		private string GetErrorDescription(string language, int eNum)
		{
			if (Service == null)
			{
				return Service.GetErrorTextPCC(eNum, language);
			}
			return Service.Utilities.GetErrorTextPCC(eNum);
		}

		private string GetErrorDescription(string language, uint eNum)
		{
			if (Service == null)
			{
				return Service.GetErrorTextPCC(eNum, language);
			}
			return Service.Utilities.GetErrorTextPCC(eNum);
		}

		private void MakeErrorDescriptionText(string newLanguage)
		{
			propCurrentCulture = newLanguage;
			if (8 == (propErrorInfo & 8))
			{
				propErrorDescription = GetErrorDescription(propCurrentCulture, propEventId);
				return;
			}
			if (1 < propFacilityCode || propCustomerCode != 0)
			{
				propErrorDescription = GetErrorDescription(propCurrentCulture, propEventId);
			}
			else
			{
				propErrorDescription = GetErrorDescription(propCurrentCulture, propErrorNumber);
			}
			if ((propErrorDescription == null || propErrorDescription == string.Empty) && 1 == propFacilityCode && 1 == propCustomerCode)
			{
				propErrorDescription = GetErrorDescription(propCurrentCulture, propErrorNumber);
			}
		}

		public virtual string GetErrorDescription(string language)
		{
			language = (language ?? "en");
			string text = "";
			if (LogBookSnapshot != null && LogBookEntry != null)
			{
				text = LogBookEntry.GetText(LogBookSnapshot, language);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ErrorDescription;
			}
			return text;
		}

		internal void UpdateUKey()
		{
			if (propParent is Module)
			{
				ulong num = ((Module)propParent).ModUID;
				propUKey = num << 32;
				propUKey += _internId;
			}
			else if (propParent is Cpu)
			{
				ulong num = ((Cpu)propParent).ModUID;
				propUKey = num << 32;
				propUKey += _internId;
			}
		}

		internal void UpdateUKey(ulong ulKey)
		{
			propUKey = ulKey;
		}

		internal void SetLoggerModuleName(string modName)
		{
			propLoggerModuleName = modName;
		}
	}
}
