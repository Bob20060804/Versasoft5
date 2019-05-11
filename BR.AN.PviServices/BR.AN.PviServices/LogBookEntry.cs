using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	internal class LogBookEntry
	{
		internal LevelType propLevelType;

		internal DateTime propDateTime;

		internal uint propErrorInfo;

		internal uint propErrorNumber;

		internal string propASCIIData;

		internal byte[] propBinary;

		internal string propTask;

		internal LogBookEntry(APIFC_RLogbookRes_entry eLog)
		{
			propErrorNumber = eLog.errcode;
			propASCIIData = eLog.errstring;
			propErrorInfo = eLog.errinfo;
			propLevelType = (LevelType)eLog.errtyp;
			switch (eLog.errtyp)
			{
			case APIFCerrtyp.APIFC_ET_EXCEPTION:
				propLevelType = LevelType.Fatal;
				break;
			case APIFCerrtyp.APIFC_ET_FATAL:
				propLevelType = LevelType.Fatal;
				break;
			case APIFCerrtyp.APIFC_ET_WARNING:
				propLevelType = LevelType.Warning;
				break;
			case APIFCerrtyp.APIFC_ET_INFO:
				propLevelType = LevelType.Info;
				break;
			case (APIFCerrtyp)4:
				propLevelType = LevelType.Debug;
				break;
			default:
				propLevelType = LevelType.Warning;
				break;
			}
			propTask = GetTaskName(eLog);
			propBinary = new byte[4];
			if (eLog.errinfo != 0)
			{
				byte[] array = new byte[4];
				int[] source = new int[1]
				{
					(int)eLog.errinfo
				};
				IntPtr hMemory = PviMarshal.AllocHGlobal(4);
				Marshal.Copy(source, 0, hMemory, 1);
				Marshal.Copy(hMemory, array, 0, 4);
				propBinary[0] = array[3];
				propBinary[1] = array[2];
				propBinary[2] = array[1];
				propBinary[3] = array[0];
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			propDateTime = T5TimeStampToDT(eLog);
		}

		internal bool EqualsTo(LoggerEntry lEntry)
		{
			if (propDateTime == lEntry.propDateTime && propErrorNumber == lEntry.propErrorNumber && propLevelType == lEntry.propLevelType && propErrorInfo == lEntry.propErrorInfo && propASCIIData.CompareTo(lEntry.ErrorText) == 0 && propTask.CompareTo(lEntry.propTask) == 0 && propBinary.Length == lEntry.propBinary.Length)
			{
				for (int i = 0; i < propBinary.Length; i++)
				{
					if (propBinary[i] != lEntry.propBinary[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private string GetHexRepresentation(uint eInfo)
		{
			AnyRadix provider = new AnyRadix();
			long num = eInfo;
			return string.Format(provider, "{0:Ra16}", new object[1]
			{
				num
			});
		}

		private DateTime T5TimeStampToDT(APIFC_RLogbookRes_entry eLog)
		{
			if (eLog.timestamp_0 == 0 && eLog.timestamp_2 == 0 && eLog.timestamp_3 == 0 && eLog.timestamp_4 == 0)
			{
				return new DateTime(0L);
			}
			int num = 0;
			int num2 = eLog.timestamp_0 >> 1;
			if (num2 < 90)
			{
				num2 += 100;
			}
			num2 += 1900;
			int num3 = ((eLog.timestamp_0 & 1) << 3) + (eLog.timestamp_1 >> 5);
			if (num3 == 0)
			{
				num2--;
				num3 = 12;
			}
			int tm_mday = eLog.timestamp_1 & 0x1F;
			int tm_hour = eLog.timestamp_2 >> 3;
			int tm_min = ((eLog.timestamp_2 & 7) << 3) + (eLog.timestamp_3 >> 5);
			int tm_sec = ((eLog.timestamp_3 & 0x1F) << 1) + (eLog.timestamp_4 >> 7);
			num = (eLog.timestamp_4 & 0x7F);
			return Pvi.ToDateTime(num2, num3, tm_mday, tm_hour, tm_min, tm_sec, num);
		}

		private string GetTaskName(APIFC_RLogbookRes_entry eInfo)
		{
			string result = "";
			if (eInfo.errtask_0 != 0)
			{
				result = ((eInfo.errtask_1 == '\0') ? $"{eInfo.errtask_0}" : ((eInfo.errtask_2 == '\0') ? $"{eInfo.errtask_0}{eInfo.errtask_1}" : ((eInfo.errtask_3 != 0) ? $"{eInfo.errtask_0}{eInfo.errtask_1}{eInfo.errtask_2}{eInfo.errtask_3}" : $"{eInfo.errtask_0}{eInfo.errtask_1}{eInfo.errtask_2}")));
			}
			return result;
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < propBinary.GetLength(0); i++)
			{
				text += $"{propBinary.GetValue(i):X2} ";
			}
			return "LevelType=\"" + propLevelType.ToString() + "\" DateTime=\"" + propDateTime.ToString("s") + "\" ErrorNumber=\"" + propErrorNumber.ToString() + "\" ErrorInfo=\"0x" + $"{propErrorInfo:X8}" + "\" Binary=\"0x" + text + "\" ASCII=\"" + propASCIIData + "\" Task=\"" + propTask + "\"";
		}

		internal void Dump()
		{
			string str = "    ";
			str += $"{propBinary.GetValue(0):X2} {propBinary.GetValue(1):X2} {propBinary.GetValue(2):X2} {propBinary.GetValue(3):X2}";
		}
	}
}
