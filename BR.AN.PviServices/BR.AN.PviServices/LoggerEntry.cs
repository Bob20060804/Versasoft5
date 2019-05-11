using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public class LoggerEntry : LoggerEntryBase
	{
		private int extendedInfo;

		private bool propARlessA2850;

		internal uint _AdditionalDataSize;

		internal uint _OldErrorNumber;

		internal int _AdditionalDataFormat;

		internal uint _RecordId;

		internal int propOriginRecordId;

		internal object propEventData;

		internal bool _InfoFlagValid;

		internal EventDataTypes propEventDataType;

		public bool ARlessA2850 => propARlessA2850;

		public int AdditionalDataFormat => _AdditionalDataFormat;

		public int CustomerCode => propCustomerCode;

		public int FacilityCode => propFacilityCode;

		public int EventID => propEventId;

		[CLSCompliant(false)]
		public uint RecordId
		{
			get
			{
				return _RecordId;
			}
		}

		public int OriginRecordId => propOriginRecordId;

		public object EventData => propEventData;

		public EventDataTypes EventDataType => propEventDataType;

		[CLSCompliant(false)]
		public LoggerEntry(object parent, DateTime dateTime)
			: base(parent, dateTime)
		{
			InitMembers();
			propARlessA2850 = false;
		}

		[CLSCompliant(false)]
		public LoggerEntry(object parent, string strDT, short offsetUtc)
			: base(parent, strDT, offsetUtc)
		{
			InitMembers();
			propARlessA2850 = false;
		}

		[CLSCompliant(false)]
		public LoggerEntry(object parent, string strDT)
			: base(parent, strDT, 0)
		{
			InitMembers();
			propARlessA2850 = false;
		}

		[CLSCompliant(false)]
		public LoggerEntry(object parent, string loggerName, DateTime dateTime, bool arLess2850)
			: base(parent, dateTime)
		{
			InitMembers();
			if (loggerName != null)
			{
				propLoggerModuleName = loggerName;
			}
			propARlessA2850 = arLess2850;
		}

		internal LoggerEntry()
		{
			InitMembers();
			propARlessA2850 = false;
		}

		internal LoggerEntry(string arV, LogBookEntry eLog)
			: base(eLog.propDateTime)
		{
			InitMembers();
			propErrorNumber = eLog.propErrorNumber;
			propASCIIData = eLog.propASCIIData;
			propErrorInfo = eLog.propErrorInfo;
			propLevelType = eLog.propLevelType;
			propTask = eLog.propTask;
			propBinary = (byte[])eLog.propBinary.Clone();
			propException = new Exception();
			propException.propBacktrace = new Backtrace();
			propException.propProcessorData = new ProcessorData();
			propException.propBacktrace.propPCInfo = new PCInfo();
			propException.propARVersion = arV;
			propRuntimeVersion = arV;
		}

		internal LoggerEntry(ErrorLogBook parent, string runtimeVersion, LogBookEntry eLog, int internalid, bool addKeyOnly, bool reverseOrder)
			: base(parent, eLog.propDateTime, addKeyOnly, reverseOrder)
		{
			InitMembers();
			propErrorNumber = eLog.propErrorNumber;
			propASCIIData = eLog.propASCIIData;
			propErrorInfo = eLog.propErrorInfo;
			propLevelType = eLog.propLevelType;
			propTask = eLog.propTask;
			propBinary = (byte[])eLog.propBinary.Clone();
			propRuntimeVersion = runtimeVersion;
			_internId = (uint)internalid;
			UpdateUKey();
		}

		internal LoggerEntry(Cpu cpu, string runtimeVersion, LogBookEntry eLog, int internalid, bool addKeyOnly, bool reverseOrder)
			: base(cpu, eLog.propDateTime, addKeyOnly, reverseOrder)
		{
			InitMembers();
			propErrorNumber = eLog.propErrorNumber;
			propASCIIData = eLog.propASCIIData;
			propErrorInfo = eLog.propErrorInfo;
			propLevelType = eLog.propLevelType;
			propTask = eLog.propTask;
			propBinary = (byte[])eLog.propBinary.Clone();
			propRuntimeVersion = runtimeVersion;
			_internId = (uint)internalid;
			UpdateUKey();
		}

		private void InitMembers()
		{
			_OldErrorNumber = 0u;
			_RecordId = 0u;
			_AdditionalDataFormat = 0;
			propOriginRecordId = 0;
			extendedInfo = 0;
			propEventId = 0;
			propEventData = null;
			propEventDataType = EventDataTypes.EmptyEventData;
			propARlessA2850 = true;
			propErrorNumber = 0u;
			propEventId = 0;
			propASCIIData = null;
			propErrorInfo = 0u;
			propLevelType = LevelType.Undefined;
			propTask = null;
			propBinary = null;
			propException = null;
			propRuntimeVersion = null;
			_internId = 0u;
		}

		internal void UpdateForSGx(LogBookEntry entry, bool sg4)
		{
			if (LevelType.Fatal == entry.propLevelType && sg4)
			{
				ValidateExcepitonMember();
				if (entry.propTask.CompareTo("PC") == 0)
				{
					propException.propProcessorData.propProgramCounter = entry.propErrorInfo;
					return;
				}
				propException.propBacktrace.propPCInfo.propModuleName = entry.propASCIIData;
				propException.propBacktrace.propPCInfo.propCodeOffset = entry.propErrorInfo;
			}
		}

		internal void AppendSGxErrorInfo(LogBookEntry entry, bool sg4)
		{
			extendedInfo++;
			if (sg4)
			{
				ValidateExcepitonMember();
				propException.propBacktrace.propFunctionName = entry.propASCIIData;
				return;
			}
			switch (extendedInfo)
			{
			case 3:
				ValidateExcepitonMember();
				propException.propBacktrace.propFunctionName = entry.propASCIIData;
				propException.propBacktrace.propTaskIdx = Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
				break;
			case 2:
			{
				ValidateExcepitonMember();
				uint num = Convert.ToUInt32(PviMarshal.LowWord(entry.propErrorInfo));
				if (num == 0)
				{
					propException.propBacktrace.propFunctionName = entry.propASCIIData;
					propException.propBacktrace.propTaskIdx = Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
				}
				else
				{
					propException.propBacktrace.propPCInfo.propModuleName = entry.propASCIIData;
					propException.propBacktrace.propInfo = Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
					propException.propBacktrace.propPCInfo.propCodeOffset = num;
				}
				break;
			}
			case 1:
				ValidateExcepitonMember();
				if (propException.propProcessorData == null)
				{
					propException.propProcessorData = new ProcessorData();
				}
				propException.propProcessorData.propProgramCounter = entry.propErrorInfo;
				break;
			}
		}

		private void ValidateExcepitonMember()
		{
			if (propException == null)
			{
				propException = new Exception();
				propException.propBacktrace = new Backtrace();
				propException.propProcessorData = new ProcessorData();
				propException.propBacktrace.propPCInfo = new PCInfo();
				propException.propARVersion = propRuntimeVersion;
			}
		}

		private DateTime T5TimeStampToDT(APIFC_RLogbookRes_entry eLog)
		{
			int num = 0;
			int num2 = eLog.timestamp_0 >> 1;
			if (num2 < 90)
			{
				num2 += 100;
			}
			num2 += 1900;
			int tm_mon = ((eLog.timestamp_0 & 1) << 3) + (eLog.timestamp_1 >> 5);
			int tm_mday = eLog.timestamp_1 & 0x1F;
			int tm_hour = eLog.timestamp_2 >> 3;
			int tm_min = ((eLog.timestamp_2 & 7) << 3) + (eLog.timestamp_3 >> 5);
			int tm_sec = ((eLog.timestamp_3 & 0x1F) << 1) + (eLog.timestamp_4 >> 7);
			num = (eLog.timestamp_4 & 0x7F);
			return Pvi.ToDateTime(num2, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, num);
		}

		private string GetTaskName(APIFC_RLogbookRes_entry eInfo)
		{
			string text = "";
			if (eInfo.errtask_1 == '\0')
			{
				return $"{eInfo.errtask_0}";
			}
			if (eInfo.errtask_2 == '\0')
			{
				return $"{eInfo.errtask_0}{eInfo.errtask_1}";
			}
			if (eInfo.errtask_3 == '\0')
			{
				return $"{eInfo.errtask_0}{eInfo.errtask_1}{eInfo.errtask_2}";
			}
			return $"{eInfo.errtask_0}{eInfo.errtask_1}{eInfo.errtask_2}{eInfo.errtask_3}";
		}

		public override string ToString()
		{
			return base.ToString() + " ARlessA2850=\"" + propARlessA2850.ToString() + "\" ExtendedInfo=\"0x" + $"{extendedInfo:X8}" + "\" " + base.ToString();
		}

		internal void Dump()
		{
			string str = "    ";
			str += $"{propBinary.GetValue(0):X2} {propBinary.GetValue(1):X2} {propBinary.GetValue(2):X2} {propBinary.GetValue(3):X2}";
		}

		private bool HasExceptionData()
		{
			if (base.Binary.GetLength(0) < Marshal.SizeOf(typeof(ExceptionHeader)))
			{
				return false;
			}
			if (AdditionalDataFormat == 254)
			{
				return true;
			}
			if (AdditionalDataFormat == 255)
			{
				return true;
			}
			if (0 != (base.ErrorInfo & 2))
			{
				return true;
			}
			if (0 != (base.ErrorInfo & 1))
			{
				return true;
			}
			return false;
		}

		internal void GetExceptionData(bool isARM, bool useBinForI386exc)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (!HasExceptionData() || !useBinForI386exc)
			{
				return;
			}
			num4 = 0;
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionHeader)));
			for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionHeader)); num2++)
			{
				Marshal.WriteByte(hMemory, num2, base.Binary[num2]);
			}
			num4 = num2;
			ExceptionHeader header = (ExceptionHeader)Marshal.PtrToStructure(hMemory, typeof(ExceptionHeader));
			propException = new Exception(header);
			PviMarshal.FreeHGlobal(ref hMemory);
			propException.propType = (((propErrorInfo & 2) == 0) ? ExceptionType.Processor : ExceptionType.System);
			_InfoFlagValid = false;
			if (propErrorInfo == 0)
			{
				if (isARM && (header.options & 1) == 1)
				{
					_InfoFlagValid = true;
				}
				if ((header.options & 2) == 2)
				{
					propException.propType = ExceptionType.System;
				}
				else
				{
					propException.propType = ExceptionType.Processor;
				}
			}
			IntPtr hMemory2 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionProcessorInfo)));
			for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionProcessorInfo)); num2++)
			{
				Marshal.WriteByte(hMemory2, num2, base.Binary[num2 + num4]);
			}
			num4 += num2;
			ExceptionProcessorInfo processorInfo = (ExceptionProcessorInfo)Marshal.PtrToStructure(hMemory2, typeof(ExceptionProcessorInfo));
			base.Exception.propProcessorData = new ProcessorData(processorInfo, isARM);
			PviMarshal.FreeHGlobal(ref hMemory2);
			IntPtr hMemory3;
			if (isARM)
			{
				hMemory3 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ARMExceptionTaskInfo)));
				for (num2 = 0; num2 < Marshal.SizeOf(typeof(ARMExceptionTaskInfo)); num2++)
				{
					Marshal.WriteByte(hMemory3, num2, base.Binary[num2 + num4]);
				}
				ARMExceptionTaskInfo taskInfo = (ARMExceptionTaskInfo)Marshal.PtrToStructure(hMemory3, typeof(ARMExceptionTaskInfo));
				base.Exception.propTaskData = new TaskData(taskInfo);
			}
			else
			{
				hMemory3 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionTaskInfo)));
				for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionTaskInfo)); num2++)
				{
					Marshal.WriteByte(hMemory3, num2, base.Binary[num2 + num4]);
				}
				ExceptionTaskInfo taskInfo2 = (ExceptionTaskInfo)Marshal.PtrToStructure(hMemory3, typeof(ExceptionTaskInfo));
				base.Exception.propTaskData = new TaskData(taskInfo2);
			}
			PviMarshal.FreeHGlobal(ref hMemory3);
			num4 += num2;
			IntPtr hMemory4 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionMemoryInfo)));
			for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionMemoryInfo)); num2++)
			{
				Marshal.WriteByte(hMemory4, num2, base.Binary[num2 + num4]);
			}
			num4 += num2;
			ExceptionMemoryInfo memoryInfo = (ExceptionMemoryInfo)Marshal.PtrToStructure(hMemory4, typeof(ExceptionMemoryInfo));
			base.Exception.propMemoryData = new MemoryData(memoryInfo);
			PviMarshal.FreeHGlobal(ref hMemory4);
			Backtrace backtrace = null;
			for (num = 0; num < base.Exception.BacktraceCount; num++)
			{
				IntPtr hMemory5 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionTraceRecord)));
				for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionTraceRecord)); num2++)
				{
					Marshal.WriteByte(hMemory5, num2, base.Binary[num2 + num4]);
				}
				num4 += num2;
				ExceptionTraceRecord traceRecord = (ExceptionTraceRecord)Marshal.PtrToStructure(hMemory5, typeof(ExceptionTraceRecord));
				PviMarshal.FreeHGlobal(ref hMemory5);
				if (base.Exception.propBacktrace == null)
				{
					base.Exception.propBacktrace = new Backtrace(traceRecord);
					backtrace = base.Exception.propBacktrace;
				}
				else
				{
					backtrace.propNextBacktrace = new Backtrace(traceRecord);
					backtrace = backtrace.NextBacktrace;
				}
				if ((backtrace.propInfo & 1) != 0)
				{
					IntPtr hMemory6 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionBRModuleFunction)));
					for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionBRModuleFunction)); num2++)
					{
						Marshal.WriteByte(hMemory6, num2, base.Binary[num2 + num4]);
					}
					ExceptionBRModuleFunction functionInfo = (ExceptionBRModuleFunction)Marshal.PtrToStructure(hMemory6, typeof(ExceptionBRModuleFunction));
					num4 += num2;
					backtrace.propFunctionInfo = new FunctionInfo(functionInfo);
					PviMarshal.FreeHGlobal(ref hMemory6);
				}
				if ((backtrace.propInfo & 4) != 0)
				{
					IntPtr hMemory7 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionBRModulePC)));
					for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionBRModulePC)); num2++)
					{
						Marshal.WriteByte(hMemory7, num2, base.Binary[num2 + num4]);
					}
					ExceptionBRModulePC pcInfo = (ExceptionBRModulePC)Marshal.PtrToStructure(hMemory7, typeof(ExceptionBRModulePC));
					num4 += num2;
					backtrace.propPCInfo = new PCInfo(pcInfo);
					PviMarshal.FreeHGlobal(ref hMemory7);
				}
				for (num2 = 0; num2 < traceRecord.paramCnt; num2++)
				{
					IntPtr hMemory8 = PviMarshal.AllocHGlobal((IntPtr)4);
					for (num3 = 0; num3 < 4; num3++)
					{
						Marshal.WriteByte(hMemory8, num3, base.Binary[num4++]);
					}
					backtrace.propParameter[num2] = (uint)Marshal.ReadInt32(hMemory8);
					PviMarshal.FreeHGlobal(ref hMemory8);
				}
				if ((backtrace.propInfo & 2) != 0)
				{
					IntPtr hMemory9 = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ExceptionBRModuleCallstack)));
					for (num2 = 0; num2 < Marshal.SizeOf(typeof(ExceptionBRModuleCallstack)); num2++)
					{
						Marshal.WriteByte(hMemory9, num2, base.Binary[num2 + num4]);
					}
					ExceptionBRModuleCallstack callstack = (ExceptionBRModuleCallstack)Marshal.PtrToStructure(hMemory9, typeof(ExceptionBRModuleCallstack));
					num4 += num2;
					backtrace.propCallstack = new Callstack(callstack);
					PviMarshal.FreeHGlobal(ref hMemory9);
				}
			}
		}

		internal string GetXMLString(string ascii)
		{
			string result = "";
			if (ascii != null)
			{
				result = ascii.Replace("&", "&amp;");
				result = result.Replace("&amp;amp", "&amp;");
				result = result.Replace("<", "&lt;");
				result = result.Replace(">", "&gt;");
				result = result.Replace("\"", "&quot;");
				result = result.Replace("'", "&apos;");
				result = result.Replace("Ä", "&#196;");
				result = result.Replace("Ö", "&#214;");
				result = result.Replace("Ü", "&#220;");
				result = result.Replace("ä", "&#228;");
				result = result.Replace("ö", "&#246;");
				result = result.Replace("ü", "&#252;");
				result = result.Replace("ß", "&#223;");
			}
			return result;
		}

		internal override string ToStringHTM(uint contentVersion)
		{
			string ascii = propASCIIData;
			if (EventDataType == EventDataTypes.ArLoggerAPI && EventData != null)
			{
				ascii = EventData.ToString();
			}
			string arg = "<td align=\"right\">";
			string str;
			if (contentVersion < 4112)
			{
				str = base.ToStringHTM(contentVersion);
				str += $"{arg}{GetXMLString(ascii)}</td>\r\n";
				str += $"{arg}{BinaryToString()}</td>\r\n";
				str += $"{arg}{base.LoggerModuleName}</td>\r\n";
				str += $"{arg}{base.ErrorNumber.ToString()}</td>\r\n";
				return str + $"{arg}{base.ErrorInfo.ToString()}</td>\r\n";
			}
			str = $"{arg}{LevelToString((int)base.LevelType)}</td>\r\n";
			str += $"{arg}{((0 < _RecordId) ? _RecordId.ToString() : _internId.ToString())}</td>\r\n";
			str += $"<td align=\"center\" width=\"220\">{DateTimeToString()}</td>\r\n";
			string arg2 = base.ErrorNumber.ToString();
			if (0 < EventID && (8 == (propErrorInfo & 8) || contentVersion >= 4128))
			{
				arg2 = EventID.ToString();
			}
			str += $"{arg}{arg2}</td>\r\n";
			str += $"{arg}{CustomerCode.ToString()}</td>\r\n";
			str += $"{arg}{base.Task}</td>\r\n";
			str = ((0 >= OriginRecordId) ? (str + string.Format("{0}{1}</td>\r\n", arg, " ")) : (str + $"{arg}{OriginRecordId.ToString()}</td>\r\n"));
			str += $"{arg}{GetXMLString(ascii)}</td>\r\n";
			str += $"{arg}{BinaryToString()}</td>\r\n";
			str += $"{arg}{base.LoggerModuleName}</td>\r\n";
			str = ((contentVersion < 4128) ? (str + ";") : ((8 != (propErrorInfo & 8)) ? (str + string.Format("{0}{1}</td>\r\n", arg, "ErrorNumber")) : (str + string.Format("{0}{1}</td>\r\n", arg, "EventId"))));
			str += $"{arg}{FacilityCode.ToString()}</td>\r\n";
			str += $"{arg}{base.ErrorNumber.ToString()}</td>\r\n";
			str += $"{arg}{AdditionalDataFormat.ToString()}</td>\r\n";
			return str + $"{arg}{base.ErrorInfo.ToString()}</td>\r\n";
		}

		internal override string ToStringCSV(uint contentVersion)
		{
			string str = base.ToStringCSV(contentVersion);
			string arg = propASCIIData;
			if (EventDataType == EventDataTypes.ArLoggerAPI && EventData != null)
			{
				arg = EventData.ToString();
			}
			if (contentVersion < 4112)
			{
				str += $"\"{arg}\";";
				str += $"\"{BinaryToString()}\";";
				str += $"\"{base.LoggerModuleName}\";";
				str += $"\"{base.ErrorNumber.ToString()}\";";
				return str + $"\"{base.ErrorInfo.ToString()}\";";
			}
			str = $"\"{LevelToString((int)base.LevelType)}\";";
			str = ((0 >= _internId && 0 >= _RecordId) ? (str + ";") : (str + $"\"{((0 < _RecordId) ? _RecordId.ToString() : _internId.ToString())}\";"));
			str += $"\"{DateTimeToString()}\";";
			bool flag = false;
			if (EventID != 0)
			{
				if (contentVersion >= 4128)
				{
					if (8 == (propErrorInfo & 8))
					{
						str += $"\"{EventID.ToString()}\";";
						flag = true;
					}
				}
				else
				{
					str += $"\"{EventID.ToString()}\";";
					flag = true;
				}
			}
			if (!flag)
			{
				str = ((base.ErrorNumber == 0) ? (str + ";") : (str + $"\"{base.ErrorNumber.ToString()}\";"));
			}
			str += $"\"{CustomerCode}\";";
			str += $"\"{propTask}\";";
			str = ((0 >= OriginRecordId) ? (str + ";") : (str + $"\"{OriginRecordId.ToString()}\";"));
			str += $"\"{arg}\";";
			str += $"\"{BinaryToString()}\";";
			str += $"\"{base.LoggerModuleName}\";";
			str = ((contentVersion < 4128) ? (str + ";") : ((8 != (propErrorInfo & 8)) ? (str + string.Format("\"{0}\";", "ErrorNumber")) : (str + string.Format("\"{0}\";", "EventId"))));
			str = ((0 >= FacilityCode) ? (str + ";") : (str + $"\"{FacilityCode}\";"));
			str += $"\"{base.ErrorNumber}\";";
			str += ";";
			str = ((0 >= AdditionalDataFormat) ? (str + ";") : (str + $"\"{AdditionalDataFormat.ToString()}\";"));
			return str + $"\"{base.ErrorInfo}\";";
		}

		internal void FixTimeStamp(bool fixForMsecSaving)
		{
			if (fixForMsecSaving)
			{
				long num = base.DateTime.Ticks % 10000000;
				propDateTime = base.DateTime.AddTicks(-1 * num);
				propDateTime = base.DateTime.AddTicks(num * 10);
			}
		}
	}
}
