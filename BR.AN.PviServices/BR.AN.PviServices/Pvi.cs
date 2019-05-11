using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	internal class Pvi
	{
		private struct VS_FIXEDFILEINFO
		{
			public uint dwSignature;

			public uint dwStrucVersion;

			public uint dwFileVersionMS;

			public uint dwFileVersionLS;

			public uint dwProductVersionMS;

			public uint dwProductVersionLS;

			public uint dwFileFlagsMask;

			public uint dwFileFlags;

			public uint dwFileOS;

			public uint dwFileType;

			public uint dwFileSubtype;

			public uint dwFileDateMS;

			public uint dwFileDateLS;
		}

		public const int APIFC_STRLEN = 36;

		public const uint SET_PVICALLBACK_DATA = 4294967294u;

		public const string KWPVTYPE_INT8 = "i8";

		public const string KWPVTYPE_Int16 = "i16";

		public const string KWPVTYPE_Int32 = "i32";

		public const string KWPVTYPE_Int64 = "i64";

		public const string KWPVTYPE_UINT8 = "u8";

		public const string KWPVTYPE_BYTE = "byte";

		public const string KWPVTYPE_UInt16 = "u16";

		public const string KWPVTYPE_WORD = "WORD";

		public const string KWPVTYPE_UInt32 = "u32";

		public const string KWPVTYPE_DWORD = "DWORD";

		public const string KWPVTYPE_UInt64 = "u64";

		public const string KWPVTYPE_FLOAT32 = "f32";

		public const string KWPVTYPE_FLOAT64 = "f64";

		public const string KWPVTYPE_Boolean = "boolean";

		public const string KWPVTYPE_String = "string";

		public const string KWPVTYPE_WideString = "wstring";

		public const string KWPVTYPE_Structure = "struct";

		public const string KWPVTYPE_DATA = "data";

		public const string KWPVTYPE_TIME = "time";

		public const string KWPVTYPE_DATI = "dt";

		public const string KWPVTYPE_DATE = "date";

		public const string KWPVTYPE_TOD = "tod";

		public const string KWPVI_LINKTYPE_PRCDATA = "prc";

		public const string KWPVI_LINKTYPE_RAWDATA = "raw";

		public const string KWPVI_EVENTMASK_ERROR = "e";

		public const string KWPVI_EVENTMASK_DATA = "d";

		public const string KWPVI_EVENTMASK_DATAFORM = "f";

		public const string KWPVI_EVENTMASK_CONNECT = "c";

		public const string KWPVI_EVENTMASK_STATUS = "s";

		public const string KWPVI_EVENTMASK_PROCEEDING = "p";

		public const string KWPVI_EVENTMASK_LINE = "l";

		public const string KWPVI_EVENTMASK_USERTAG = "u";

		public const string KW_ATTRIBUTE_EVENT = "e";

		public const string KW_ATTRIBUTE_READ = "r";

		public const string KW_ATTRIBUTE_WRITE = "w";

		public const string KW_ATTRIBUTE_SIMULATED = "s";

		public const string KW_ATTRIBUTE_DIRECT = "d";

		public const string KW_ATTRIBUTE_READWRITE = "rw";

		public const string KW_ATTRIBUTE_ECHO = "h";

		public const int SECSPERMIN = 60;

		public const int MINSPERHOUR = 60;

		public const int HOURSPERDAY = 24;

		public const int LEAPYEARDAYS = 366;

		public const int YEARDAYS = 365;

		public const uint SECSPERYEAR = 31536000u;

		public const uint SECSPERLEAPYEAR = 31622400u;

		public const int SECSPERHOUR = 3600;

		public const uint SECSPERDAY = 86400u;

		public const int DAYSPERWEEK = 7;

		public const int MONSPERYEAR = 12;

		public const int YEAR_BASE = 1900;

		public const int EPOCH_YEAR = 1970;

		public const int EPOCH_WDAY = 4;

		public const int CALLBACK_DATA = -2;

		public const string KWPVI_CONNECT = "CD";

		public const string KWPVI_ALIGNMENT = "AL";

		public const string KWPVI_PVTYPE = "VT";

		public const string KWPVI_PVLEN = "VL";

		public const string KWPVI_PVCNT = "VN";

		public const string KWPVI_PVOFFS = "VO";

		public const string KWPVI_PVSPEC = "VS";

		public const string KWPVI_TYPE_NAME = "TN";

		public const string KWPVI_INIT_VALUE = "IV";

		public const string KWPVI_PVADDR = "VA";

		public const string KWPVI_ATTRIBUTE = "AT";

		public const string KWPVI_REFRESH = "RF";

		public const string KWPVI_HYSTERESE = "HY";

		public const string KWPVI_FUNCTION = "FS";

		public const string KWPVI_DEFAULT = "DV";

		public const string KWPVI_CASTMODE = "CM";

		public const string KWPVI_EVMASK = "EV";

		public const string KWPVI_LINKTYPE = "LT";

		public const string KWPVI_USERTAG = "UT";

		public const string KWPVI_OBJTYPE = "OT";

		public const string KWPVI_SCOPE = "SC";

		public const string KWPVI_SNAME = "SN";

		public const string KWPVI_LOADTYPE = "LD";

		public const string KWPVI_INSTMODE = "IM";

		public const string KWPVI_MODNAME = "MN";

		public const string KWPVI_STATUS = "ST";

		public const string KWPVI_FORCESTATE = "FC";

		public const string KWPVI_IOTYPE = "IO";

		public const string KWPVI_MODTYPE = "MT";

		public const string KWPVI_MODLEN = "ML";

		public const string KWPVI_MODOFFSET = "MO";

		public const string KWPVI_MEMLEN = "SL";

		public const string KWPVI_MEMFREELEN = "SF";

		public const string KWPVI_MEMBLOCKLEN = "SB";

		public const string KWPVI_DATALEN = "DL";

		public const string KWPVI_DATACNT = "DN";

		public const string KWPVI_CPUNAME = "CN";

		public const string KWPVI_CPUTYPE = "CT";

		public const string KWPVI_AWSTYPE = "AW";

		public const string KWPVI_UNRESLKN = "UL";

		public const string KWPVI_IDENTIFY = "ID";

		public const string KWPVI_VERSION = "VI";

		public const string KWPVI_MEM_SYS_RAM = "SysRam";

		public const string KWPVI_MEM_RAM = "Ram";

		public const string KWPVI_MEM_SYS_ROM = "SysRom";

		public const string KWPVI_MEM_ROM = "Rom";

		public const string KWPVI_MEM_MEMCARD = "MemCard";

		public const string KWPVI_MEM_FIX_RAM = "FixRam";

		public const string KWPVI_MEM_DRAM = "DRam";

		public const string KWPVI_MEM_PER_MEM = "PerMem";

		public const string KWPVI_MEM_DELETE = "Delete";

		public const string KWPVI_MEM_CLEAR = "Clear";

		public const string KWPVI_MEM_OVERLOAD = "Overload";

		public const string KWPVI_MEM_COPY = "Copy";

		public const string KWPVI_MEM_ONECYCLE = "OneCycle";

		public const string KWPVI_RUN_WARMSTART = "WarmStart";

		public const string KWPVI_RUN_COLDSTART = "ColdStart";

		public const string KWPVI_RUN_START = "Start";

		public const string KWPVI_RUN_STOP = "Stop";

		public const string KWPVI_RUN_RESUME = "Resume";

		public const string KWPVI_RUN_CYCLE = "Cycle";

		public const string KWPVI_RUN_CYCLE_ARG = "Cycle(%u)";

		public const string KWPVI_RUN_RESET = "Reset";

		public const string KWPVI_RUN_RECONF = "Reconfiguration";

		public const string KWPVI_RUN_NMI = "NMI";

		public const string KWPVI_RUN_DIAGNOSE = "Diagnose";

		public const string KWPVI_RUN_ERROR = "Error";

		public const string KWPVI_RUN_EXISTS = "Exists";

		public const string KWPVI_RUN_LOADING = "Loading";

		public const string KWPVI_RUN_INCOMPLETE = "Incomplete";

		public const string KWPVI_RUN_COMPLETE = "Complete";

		public const string KWPVI_RUN_READY = "Ready";

		public const string KWPVI_RUN_INUSE = "InUse";

		public const string KWPVI_RUN_NONEXISTING = "NonExisting";

		public const string KWPVI_RUN_UNRUNNABLE = "Unrunnable";

		public const string KWPVI_RUN_IDLE = "Idle";

		public const string KWPVI_RUN_RUNNING = "Running";

		public const string KWPVI_RUN_STOPPED = "Stopped";

		public const string KWPVI_RUN_STARTING = "Starting";

		public const string KWPVI_RUN_STOPPING = "Stopping";

		public const string KWPVI_RUN_RESUMING = "Resuming";

		public const string KWPVI_RUN_RESETING = "Reseting";

		public const string KWPVI_RUN_CONSTANT = "Const";

		public const string KWPVI_RUN_VARIABLE = "Var";

		public const string KW_VAR_TYPE = "VT=";

		public const string KW_VAR_LENGTH = "VL=";

		public const string KW_NUM_OF_ELEMENTS = "VN=";

		public const string KW_EXTENDED_TYPE = "VS=";

		public const string KW_EXTENDED_TYPE_ENUM = "VS=e";

		public const string KW_EXTENDED_TYPE_BITSTRING = "VS=b";

		public const string KW_EXTENDED_TYPE_MARRAY = "VS=a";

		public const string KW_EXTENDED_TYPE_SEMI_E = ";e";

		public const string KW_EXTENDED_TYPE_SEMI_B = ";b";

		public const string KW_EXTENDED_TYPE_SEMI_A = ";a";

		public const string KW_EXTENDED_TYPE_SEMI_V = ";v";

		public const string KW_EXTENDED_TYPE_DERIVED = "VS=v";

		public const string KW_EXTENDED_ENUM = "e";

		public const string KW_EXTENDED_BITSTRING = "b";

		public const string KW_EXTENDED_MARRAY = "a";

		public const string KW_EXTENDED_DERIVED = "v";

		public const string KW_DERIVED_FROM = "TN=";

		public const string KW_BIT_ADDRESS = "VA=";

		public const string KW_HYSTERESIS = "HY=";

		public const string KW_CAST_MODE = "CM=";

		public const string KW_ATTRIBUTE = "AT=";

		public const string KW_USER_TAG = "UT=";

		public const string KW_SCALING_FUNCTION = "FS=";

		public const string KW_ROCESS_ACCESS = "LT=";

		public const string KW_ROCESS_ACCESS_RAW = "raw";

		public const string KW_ROCESS_ACCESS_PRC = "prc";

		public const string KW_FORCE_STATE = "FC=";

		public const string KW_IO_STATE = "IO=";

		public const string KW_STATE = "ST=";

		public const string KW_UNRESOLVED_LINK = "UL=";

		public const string KW_SCOPE = "SC=";

		public const string KW_SCOPE_GLOBAL = "g";

		public const string KW_SCOPE_LOCAL = "l";

		public const string KW_SCOPE_DYNAMIC = "d";

		public const string KW_STRUCT_NAME = "SN=";

		public const string KW_IS_BIT_STRING = "b";

		public const string KW_IS_ENUM = "e";

		public const string KW_IS_DERIVED = "v";

		public const string KW_IS_ARRAY = "a";

		public const string KW_ALIGNMENT = "AL=";

		public const string KW_BIT_OFFSET = "VO=";

		public const string KW_EVENT_MASK = "EV=";

		public const string KW_REFRESH = "RF=";

		public const string KW_CONNECTION_DESC = "CD=";

		private const string COREDLL = "version.dll";

		internal static int GetTimeSpanInt32(object value)
		{
			if (value is int)
			{
				return (int)value;
			}
			if (value is uint)
			{
				return (int)(uint)value;
			}
			if (value is short)
			{
				return (short)value;
			}
			if (value is long)
			{
				return (int)(long)value;
			}
			if (value is ulong)
			{
				return (int)(ulong)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (int)(float)value;
			}
			if (value is double)
			{
				return (int)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toInt32(((Value)value).propObjValue);
			}
			if (!(value is string) && !(value is Array))
			{
				if (value is DateTime)
				{
					return (int)(((DateTime)value).Ticks / 10000);
				}
				if (value is TimeSpan)
				{
					return (int)(((TimeSpan)value).Ticks / 10000);
				}
				return (int)value;
			}
			object obj = value;
			if (value is Array)
			{
				obj = ((Array)value).GetValue(0);
			}
			if (obj is DateTime)
			{
				return (int)(((DateTime)obj).Ticks / 10000);
			}
			if (obj is TimeSpan)
			{
				return (int)(((TimeSpan)obj).Ticks / 10000);
			}
			if (obj is string)
			{
				if (-1 != ((string)obj).IndexOf('.') || -1 != ((string)obj).IndexOf(':'))
				{
					return (int)(TimeSpan.Parse(obj.ToString()).Ticks / 10000);
				}
				return Convert.ToInt32(GetIntVal(Convert.ToInt64(obj) / 10000));
			}
			string text = obj.ToString();
			if (-1 != text.IndexOf('.') || -1 != text.IndexOf(':'))
			{
				return (int)(TimeSpan.Parse(obj.ToString()).Ticks / 10000);
			}
			return Convert.ToInt32(GetIntVal(Convert.ToInt64(text) / 10000));
		}

		internal static uint GetTimeSpanUInt32(object value)
		{
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is int)
			{
				return (uint)(int)value;
			}
			if (value is short)
			{
				return (uint)(short)value;
			}
			if (value is long)
			{
				return (uint)(long)value;
			}
			if (value is ulong)
			{
				return (uint)(ulong)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (uint)(sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (uint)(float)value;
			}
			if (value is double)
			{
				return (uint)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toUInt32(((Value)value).propObjValue);
			}
			if (!(value is string) && !(value is Array))
			{
				if (value is DateTime)
				{
					return (uint)(((DateTime)value).Ticks / 10000);
				}
				if (value is TimeSpan)
				{
					return (uint)(((TimeSpan)value).Ticks / 10000);
				}
				return (uint)value;
			}
			object obj = value;
			if (value is Array)
			{
				obj = ((Array)value).GetValue(0);
			}
			if (obj is DateTime)
			{
				return (uint)(((DateTime)obj).Ticks / 10000);
			}
			if (obj is TimeSpan)
			{
				return (uint)(((TimeSpan)obj).Ticks / 10000);
			}
			if (obj is string)
			{
				if (-1 != ((string)obj).IndexOf('.') || -1 != ((string)obj).IndexOf(':'))
				{
					return (uint)(TimeSpan.Parse(obj.ToString()).Ticks / 10000);
				}
				return Convert.ToUInt32(GetIntVal(Convert.ToUInt64(obj) / 10000uL));
			}
			string text = obj.ToString();
			if (-1 != text.IndexOf('.') || -1 != text.IndexOf(':'))
			{
				return (uint)(TimeSpan.Parse(obj.ToString()).Ticks / 10000);
			}
			return Convert.ToUInt32(GetIntVal(Convert.ToUInt64(text) / 10000uL));
		}

		internal static long GetIntVal(object value)
		{
			long num = 0L;
			if (value is long)
			{
				return (long)value;
			}
			if (value is int)
			{
				return (int)value;
			}
			if (value is short)
			{
				return (short)value;
			}
			if (value is ulong)
			{
				return (long)(ulong)value;
			}
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (long)(float)value;
			}
			if (value is double)
			{
				return (long)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toInt64(((Value)value).propObjValue);
			}
			int num2 = -1;
			int num3 = -1;
			string text = "";
			text = ((!(value is Array)) ? value.ToString() : ((Array)value).GetValue(0).ToString());
			if (text.ToLower().CompareTo("true") == 0)
			{
				return 1L;
			}
			if (text.ToLower().CompareTo("false") == 0)
			{
				return 0L;
			}
			if (NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.CompareTo(",") == 0)
			{
				num2 = text.IndexOf(',');
				if (-1 == num2)
				{
					num2 = text.IndexOf('.');
					if (-1 != num2)
					{
						num3 = text.LastIndexOf('.');
						text = ((num2 == num3) ? text.Replace('.', ',') : (text.Substring(0, num3) + ',' + text.Substring(num3 + 1)));
					}
				}
			}
			else
			{
				text.Replace(',', '.');
			}
			if (-1 != text.IndexOf("0x") || -1 != text.IndexOf("0X"))
			{
				return Convert.ToInt64(text, 16);
			}
			return Convert.ToInt64(Convert.ToDouble(text));
		}

		internal static ulong GetUIntVal(object value)
		{
			ulong num = 0uL;
			if (value is ulong)
			{
				return (ulong)value;
			}
			if (value is int)
			{
				return (ulong)(int)value;
			}
			if (value is short)
			{
				return (ulong)(short)value;
			}
			if (value is long)
			{
				return (ulong)(long)value;
			}
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (ulong)(sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (ulong)(float)value;
			}
			if (value is double)
			{
				return (ulong)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toUInt64(((Value)value).propObjValue);
			}
			int num2 = -1;
			int num3 = -1;
			string text = "";
			text = ((!(value is Array)) ? value.ToString() : ((Array)value).GetValue(0).ToString());
			if (text.ToLower().CompareTo("true") == 0)
			{
				return 1uL;
			}
			if (text.ToLower().CompareTo("false") == 0)
			{
				return 0uL;
			}
			if (NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.CompareTo(",") == 0)
			{
				num2 = text.IndexOf(',');
				if (-1 == num2)
				{
					num2 = text.IndexOf('.');
					if (-1 != num2)
					{
						num3 = text.LastIndexOf('.');
						text = ((num2 == num3) ? text.Replace('.', ',') : (text.Substring(0, num3) + ',' + text.Substring(num3 + 1)));
					}
				}
			}
			else
			{
				text.Replace(',', '.');
			}
			if (-1 != text.IndexOf("0x") || -1 != text.IndexOf("0X"))
			{
				return Convert.ToUInt64(text, 16);
			}
			return Convert.ToUInt64(Convert.ToDouble(text));
		}

		internal static bool IsLeapYear(int year)
		{
			if (year % 4 == 0)
			{
				if (year < 1582)
				{
					return true;
				}
				if (year % 100 != 0)
				{
					return true;
				}
				if (year % 400 == 0)
				{
					return true;
				}
			}
			return false;
		}

		internal static DateTime ToDateTime(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec)
		{
			return ToDateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, 0);
		}

		internal static DateTime ToDateTime(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_Msec)
		{
			try
			{
				return new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_Msec);
			}
			catch
			{
				if (1 > tm_year)
				{
					tm_year = 1;
				}
				if (9999 < tm_year)
				{
					tm_year = 9999;
				}
				if (1 > tm_mon)
				{
					tm_mon = 1;
				}
				if (12 < tm_mon)
				{
					tm_mon = 12;
				}
				if (1 > tm_mday)
				{
					tm_mday = 1;
				}
				if (31 < tm_mday)
				{
					tm_mday = 31;
				}
				if (28 < tm_mday)
				{
					if (2 == tm_mon)
					{
						tm_mday = 29;
						if (IsLeapYear(tm_year))
						{
							tm_mday = 29;
						}
					}
					else if (30 < tm_mday && (4 == tm_mon || 6 == tm_mon || 9 == tm_mon || 11 == tm_mon))
					{
						tm_mday = 30;
					}
				}
				if (0 > tm_hour || 23 < tm_hour)
				{
					tm_hour = 0;
				}
				if (0 > tm_min || 59 < tm_min)
				{
					tm_min = 0;
				}
				if (0 > tm_sec || 59 < tm_sec)
				{
					tm_sec = 0;
				}
				if (0 > tm_Msec || 999 < tm_Msec)
				{
					tm_Msec = 0;
				}
				return new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_Msec);
			}
		}

		internal static uint GetDateTimeUInt32(object value)
		{
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is int)
			{
				return (uint)(int)value;
			}
			if (value is short)
			{
				return (uint)(short)value;
			}
			if (value is long)
			{
				return (uint)(long)value;
			}
			if (value is ulong)
			{
				return (uint)(ulong)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (uint)(sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (uint)(float)value;
			}
			if (value is double)
			{
				return (uint)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toUInt32(((Value)value).propObjValue);
			}
			if (!(value is string) && !(value is Array))
			{
				if (value is DateTime)
				{
					return DateTimeToUInt32((DateTime)value);
				}
				if (value is TimeSpan)
				{
					long ticks = ((TimeSpan)value).Ticks;
					return (uint)(ticks / 10000000);
				}
				return (uint)value;
			}
			object obj = value;
			if (value is Array)
			{
				obj = ((Array)value).GetValue(0);
			}
			if (obj is DateTime)
			{
				return DateTimeToUInt32((DateTime)obj);
			}
			if (obj is TimeSpan)
			{
				long ticks2 = ((TimeSpan)obj).Ticks;
				return (uint)(ticks2 / 10000000);
			}
			if (obj is string)
			{
				DateTime dt = DateTime.Parse(obj.ToString());
				return DateTimeToUInt32(dt);
			}
			DateTime dt2 = DateTime.Parse(obj.ToString());
			return DateTimeToUInt32(dt2);
		}

		internal static uint GetDateUInt32(object value)
		{
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is int)
			{
				return (uint)(int)value;
			}
			if (value is short)
			{
				return (uint)(short)value;
			}
			if (value is long)
			{
				return (uint)(long)value;
			}
			if (value is ulong)
			{
				return (uint)(ulong)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is sbyte)
			{
				return (uint)(sbyte)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is float)
			{
				return (uint)(float)value;
			}
			if (value is double)
			{
				return (uint)(double)value;
			}
			if (value is Value)
			{
				return PviMarshal.toUInt32(((Value)value).propObjValue);
			}
			if (!(value is string) && !(value is Array))
			{
				if (value is DateTime)
				{
					return DateTimeToUInt32((DateTime)value);
				}
				if (value is TimeSpan)
				{
					long ticks = ((TimeSpan)value).Ticks;
					return (uint)(ticks / 10000000);
				}
				return (uint)value;
			}
			object obj = value;
			if (value is Array)
			{
				obj = ((Array)value).GetValue(0);
			}
			if (obj is DateTime)
			{
				return DateTimeToUInt32((DateTime)obj);
			}
			if (obj is TimeSpan)
			{
				long ticks2 = ((TimeSpan)obj).Ticks;
				return (uint)(ticks2 / 10000000);
			}
			if (obj is string)
			{
				DateTime dt = DateTime.Parse(obj.ToString());
				return DateTimeToUInt32(dt);
			}
			DateTime dt2 = DateTime.Parse(obj.ToString());
			return DateTimeToUInt32(dt2);
		}

		internal static uint DateTimeToUInt32(DateTime dt)
		{
			uint num = (uint)((long)(dt.DayOfYear - 1) * 86400L);
			num = (uint)((int)num + dt.Hour * 3600);
			num = (uint)((int)num + dt.Minute * 60);
			num = (uint)((int)num + dt.Second);
			num = (uint)((int)num + (int)((long)(dt.Year - 1970) * 31536000L));
			int num2 = dt.Year - 1900;
			int num3 = num2 / 4 - 17;
			num3 -= num2 / 100;
			num3 += dt.Year / 400 - 4;
			num = (uint)((int)num + (int)((long)num3 * 86400L));
			if (IsLeapYear(dt.Year))
			{
				num -= 86400;
			}
			return num;
		}

		internal static uint ToUInt32(string value)
		{
			uint num = 0u;
			int num2 = 0;
			if (-1 != value.IndexOf("0x"))
			{
				return Convert.ToUInt32(value, 16);
			}
			if (-1 != value.IndexOf("-"))
			{
				return (uint)int.Parse(value);
			}
			return Convert.ToUInt32(value);
		}

		internal static bool IsLeap(uint year)
		{
			if (year % 4u == 0 && (year % 100u != 0 || year % 400u == 0))
			{
				return true;
			}
			return false;
		}

		internal static DateTime UInt32ToDateTime(uint timeValue)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			uint[] array = new uint[2]
			{
				365u,
				366u
			};
			uint[,] array2 = new uint[2, 12]
			{
				{
					31u,
					28u,
					31u,
					30u,
					31u,
					30u,
					31u,
					31u,
					30u,
					31u,
					30u,
					31u
				},
				{
					31u,
					29u,
					31u,
					30u,
					31u,
					30u,
					31u,
					31u,
					30u,
					31u,
					30u,
					31u
				}
			};
			uint num7 = timeValue / 86400u;
			uint num8 = timeValue % 86400u;
			while (num8 < 0)
			{
				num8 += 86400;
				num7--;
			}
			while (num8 >= 86400)
			{
				num8 -= 86400;
				num7++;
			}
			num4 = num8 / 3600u;
			num8 %= 3600u;
			num5 = num8 / 60u;
			num6 = num8 % 60u;
			num = 1970u;
			uint num9;
			if (num7 >= 0)
			{
				while (true)
				{
					num9 = Convert.ToUInt32(IsLeap(num));
					if (num7 < array[num9])
					{
						break;
					}
					num++;
					num7 -= array[num9];
				}
			}
			else
			{
				do
				{
					num--;
					num9 = Convert.ToUInt32(IsLeap(num));
					num7 += array[num9];
				}
				while (num7 < 0);
			}
			for (num2 = 0u; num7 >= array2[num9, num2]; num2++)
			{
				num7 -= array2[num9, num2];
			}
			num3 = num7 + 1;
			return ToDateTime((int)num, (int)(num2 + 1), (int)num3, (int)num4, (int)num5, (int)num6);
		}

		internal static bool IsNullOrEmpty(object strInput)
		{
			if (strInput == null)
			{
				return true;
			}
			return string.IsNullOrEmpty(strInput.ToString());
		}

		[DllImport("version.dll", SetLastError = true)]
		private static extern int GetFileVersionInfoSize(string lptstrFilename, out IntPtr lpdwHandle);

		[DllImport("version.dll", SetLastError = true)]
		private static extern bool GetFileVersionInfo(string lptstrFilename, IntPtr dwHandle, int dwLen, IntPtr lpData);

		[DllImport("version.dll", SetLastError = true)]
		private static extern bool VerQueryValue(IntPtr pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);

		internal static int GetNativeDLLVersions(ref string productVersion, ref string fileVersion)
		{
			int result = 0;
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			productVersion = versionInfo.ProductVersion;
			fileVersion = versionInfo.FileVersion;
			return result;
		}

		internal static string GetExecutingAssemblyFileVersion(Assembly execAsmbly)
		{
			string text = "";
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
		}
	}
}
