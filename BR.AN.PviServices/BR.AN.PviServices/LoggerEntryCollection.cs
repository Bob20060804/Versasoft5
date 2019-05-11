using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	public class LoggerEntryCollection : BaseCollection
	{
		private const uint ARLOG_MODULEKENN = 11159u;

		private const uint ARLOG_MODULETYPE = 83u;

		private const uint ARLOG_RECTAGBEGIN = 3735901816u;

		protected ArrayList arrayOfLoggerEntries;

		internal short _OffsetUtc;

		internal short _DaylightSaving;

		internal ushort _LogAttributes;

		internal uint _ActIndex;

		internal uint _ActOffset;

		internal uint _InvalidLength;

		internal uint _LogDataLength;

		internal uint _ReferenceIndex;

		internal uint _ReferenceOffset;

		internal uint _Wrap;

		internal uint _WriteOffset;

		internal uint propContentVersion;

		private bool _ARMExceptions;

		private string propXMLData;

		[CLSCompliant(false)]
		public uint ContentVersion
		{
			get
			{
				return propContentVersion;
			}
		}

		[CLSCompliant(false)]
		public LoggerEntry this[string indexer]
		{
			get
			{
				try
				{
					return this[Convert.ToInt32(indexer)];
				}
				catch
				{
					return null;
				}
			}
		}

		[CLSCompliant(false)]
		public LoggerEntry this[int index]
		{
			get
			{
				if (index < arrayOfLoggerEntries.Count)
				{
					return (LoggerEntry)arrayOfLoggerEntries[index];
				}
				return (LoggerEntry)ElementAt(index);
			}
		}

		[CLSCompliant(false)]
		public LoggerEntry this[ulong index]
		{
			get
			{
				return (LoggerEntry)base[index];
			}
		}

		public string XMLData => propXMLData;

		public DaylightSaving DaylightSaving
		{
			get
			{
				if (_DaylightSaving != 0)
				{
					return DaylightSaving.ON;
				}
				return DaylightSaving.OFF;
			}
		}

		public short OffsetUtc => _OffsetUtc;

		[CLSCompliant(false)]
		public ushort LogAttributes
		{
			get
			{
				return _LogAttributes;
			}
		}

		public LoggerEntryCollection(Base parent, string name)
			: base(CollectionType.SortedDictionary, parent, name)
		{
			_ARMExceptions = false;
			_OffsetUtc = 0;
			_DaylightSaving = 0;
			_LogAttributes = 0;
			_ActIndex = 0u;
			_ActOffset = 0u;
			_InvalidLength = 0u;
			_LogDataLength = 0u;
			_ReferenceIndex = 0u;
			_ReferenceOffset = 0u;
			_Wrap = 0u;
			_WriteOffset = 0u;
			propContentVersion = 0u;
			arrayOfLoggerEntries = new ArrayList(1);
		}

		public LoggerEntryCollection(string name, LoggerEntryCollection eventEntries)
			: base(CollectionType.SortedDictionary, null, name)
		{
			_ARMExceptions = false;
			_OffsetUtc = 0;
			_DaylightSaving = 0;
			_LogAttributes = 0;
			_ActIndex = 0u;
			_ActOffset = 0u;
			_InvalidLength = 0u;
			_LogDataLength = 0u;
			_ReferenceIndex = 0u;
			_ReferenceOffset = 0u;
			_Wrap = 0u;
			_WriteOffset = 0u;
			propContentVersion = eventEntries.ContentVersion;
			arrayOfLoggerEntries = new ArrayList(1);
			InitItems(eventEntries);
		}

		public LoggerEntryCollection(string name)
			: base(CollectionType.SortedDictionary, null, name)
		{
			_ARMExceptions = false;
			_OffsetUtc = 0;
			_DaylightSaving = 0;
			_LogAttributes = 0;
			_ActIndex = 0u;
			_ActOffset = 0u;
			_InvalidLength = 0u;
			_LogDataLength = 0u;
			_ReferenceIndex = 0u;
			_ReferenceOffset = 0u;
			_Wrap = 0u;
			_WriteOffset = 0u;
			propContentVersion = 0u;
			arrayOfLoggerEntries = new ArrayList(1);
		}

		~LoggerEntryCollection()
		{
			Dispose(disposing: false, removeFromCollection: true);
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				CleanUp(disposing);
				base.Dispose(disposing, removeFromCollection);
				propParent = null;
				propUserData = null;
				propName = null;
			}
		}

		internal void CleanUp(bool disposing)
		{
			ArrayList arrayList = new ArrayList();
			propCounter = 0;
			if (Values != null)
			{
				foreach (LoggerEntryBase value in Values)
				{
					arrayList.Add(value);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				((LoggerEntryBase)obj).Dispose(disposing);
				obj = null;
			}
			Clear(!disposing);
			if (disposing)
			{
				arrayOfLoggerEntries = null;
			}
		}

		private static uint ConvertToUInt32(byte[] bytearray)
		{
			uint num = 0u;
			int num2 = 0;
			while (bytearray.Length == 4 && num2 < bytearray.Length)
			{
				num += bytearray[3 - num2] * (uint)Math.Pow(2.0, num2 * 8);
				num2++;
			}
			return num;
		}

		private static ushort ConvertToUInt16(byte[] bytearray)
		{
			ushort num = 0;
			int num2 = 0;
			while (bytearray.Length == 2 && num2 < bytearray.Length)
			{
				num = (ushort)(num + (ushort)(bytearray[1 - num2] * (ushort)Math.Pow(2.0, num2 * 8)));
				num2++;
			}
			return num;
		}

		private unsafe void ModuleNameBytesToAsciiString(byte[] brString, ref char[] asciiString, ref int asciiLength)
		{
			byte[] array = new byte[10]
			{
				6,
				0,
				2,
				4,
				6,
				0,
				2,
				4,
				6,
				0
			};
			byte[] array2 = new byte[10]
			{
				0,
				0,
				1,
				2,
				3,
				3,
				4,
				5,
				6,
				6
			};
			byte[] array3 = new byte[2];
			byte[] array4 = array3;
			asciiLength = brString[0] >> 4;
			if (asciiLength > 10)
			{
				return;
			}
			for (int i = 0; i < asciiLength; i++)
			{
				fixed (byte* ptr = &brString[0])
				{
					array4[0] = ptr[(int)array2[i]];
					array4[1] = (ptr + (int)array2[i])[1];
				}
				ushort num = ConvertToUInt16(array4);
				byte b = (byte)(num >> (int)array[i]);
				b = (byte)(b & 0x3F);
				if (b >= 0 && b <= 9)
				{
					asciiString[i] = (char)(48 + b);
					continue;
				}
				if (b >= 10 && b <= 35)
				{
					asciiString[i] = (char)(65 + b - 10);
					continue;
				}
				if (b >= 36 && b <= 61)
				{
					asciiString[i] = (char)(97 + b - 36);
					continue;
				}
				switch (b)
				{
				case 62:
					asciiString[i] = '_';
					break;
				case 63:
					asciiString[i] = '$';
					break;
				}
			}
			asciiString[asciiLength] = '\0';
		}

		public int Load(XmlTextReader reader)
		{
			int result = 0;
			try
			{
				LoadArlEntries(null, reader, isServiceBased: false, onlyOneLogger: true);
				return result;
			}
			catch
			{
				return 12054;
			}
		}

		private Logger GetLoggerObject(string keyName, string loggerName, string fileName)
		{
			Logger logger = null;
			LoggerCollection loggerCollection = null;
			if (propParent is Service)
			{
				foreach (LoggerCollection loggerCollection2 in ((Service)propParent).LoggerCollections)
				{
					if (keyName.CompareTo(loggerCollection2.Name) == 0)
					{
						loggerCollection = loggerCollection2;
						break;
					}
				}
			}
			if (loggerCollection == null)
			{
				loggerCollection = new LoggerCollection(CollectionType.ArrayList, propParent, keyName);
			}
			logger = loggerCollection[fileName];
			if (logger == null)
			{
				if (propParent is Service)
				{
					logger = new Logger((Service)propParent, loggerName, isArchive: true);
					logger.GlobalMerge = true;
				}
				else
				{
					logger = (Logger)propParent;
				}
			}
			loggerCollection.Add(logger);
			return logger;
		}

		private int LoadBRHeaderSection(BinaryReader binReader, ref uint bytesRead, out string strModName, out uint offVWSection, out uint offLogDataSection)
		{
			byte[] bytearray = binReader.ReadBytes(2);
			bytesRead += 2u;
			byte b = binReader.ReadByte();
			bytesRead++;
			strModName = null;
			offVWSection = 0u;
			offLogDataSection = 0u;
			if (ConvertToUInt16(bytearray) != 11159 || b != 83)
			{
				return 11;
			}
			binReader.ReadBytes(1);
			bytesRead++;
			byte[] brString = binReader.ReadBytes(8);
			bytesRead += 8u;
			char[] asciiString = new char[11];
			int asciiLength = 0;
			ModuleNameBytesToAsciiString(brString, ref asciiString, ref asciiLength);
			strModName = new string(asciiString, 0, asciiLength);
			binReader.ReadBytes(2);
			bytesRead += 2u;
			byte[] bytearray2 = binReader.ReadBytes(4);
			bytesRead += 4u;
			ConvertToUInt32(bytearray2);
			binReader.ReadBytes(14);
			bytesRead += 14u;
			binReader.ReadBytes(4);
			bytesRead += 4u;
			byte[] bytearray3 = binReader.ReadBytes(4);
			bytesRead += 4u;
			byte[] bytearray4 = binReader.ReadBytes(4);
			bytesRead += 4u;
			offVWSection = ConvertToUInt32(bytearray3);
			offLogDataSection = ConvertToUInt32(bytearray4);
			binReader.ReadBytes((int)(offVWSection - bytesRead));
			bytesRead += offVWSection - bytesRead;
			return 0;
		}

		private int LoadBRVWSection(BinaryReader binReader, uint offLogDataSection, uint offVWSection, ref uint bytesRead)
		{
			int num = Marshal.SizeOf(typeof(VWSection));
			if (bytesRead != offVWSection)
			{
				binReader.ReadBytes((int)(offVWSection - bytesRead));
				bytesRead = offVWSection;
			}
			byte[] array = binReader.ReadBytes(num);
			bytesRead += (uint)num;
			if (array.Length != num)
			{
				return 11;
			}
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)num);
			for (int i = 0; i < num; i++)
			{
				Marshal.WriteByte(hMemory, i, array[i]);
			}
			VWSection vWSection = (VWSection)Marshal.PtrToStructure(hMemory, typeof(VWSection));
			PviMarshal.FreeHGlobal(ref hMemory);
			propContentVersion = vWSection.version;
			if (bytesRead < offLogDataSection)
			{
				binReader.ReadBytes((int)(offLogDataSection - bytesRead));
				bytesRead += offLogDataSection - bytesRead;
			}
			_DaylightSaving = vWSection.daylightSaving;
			_OffsetUtc = vWSection.offsetUtc;
			_LogAttributes = vWSection.attributes;
			_ActIndex = vWSection.actIdx;
			_ActOffset = vWSection.actOff;
			_InvalidLength = vWSection.invLen;
			_LogDataLength = vWSection.lenOfLogData;
			_ReferenceIndex = vWSection.refIdx;
			_ReferenceOffset = vWSection.refOff;
			_Wrap = vWSection.wrap;
			_WriteOffset = vWSection.writeOff;
			return 0;
		}

		private uint LoadBRBinary(LoggerEntry entry, ARLoggerEntry arLoggerEntry, uint dataOffset, byte[] bytes)
		{
			if (arLoggerEntry.lenOfBinData != 0)
			{
				entry.propBinary = new byte[arLoggerEntry.lenOfBinData];
				for (int i = 0; i < arLoggerEntry.lenOfBinData; i++)
				{
					entry.propBinary[i] = bytes[dataOffset];
					dataOffset++;
				}
			}
			if (arLoggerEntry.lenOfAsciiString != 0)
			{
				IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)(long)arLoggerEntry.lenOfAsciiString);
				for (int j = 0; j < arLoggerEntry.lenOfAsciiString; j++)
				{
					Marshal.WriteByte(hMemory, j, bytes[dataOffset]);
					dataOffset++;
				}
				entry.propASCIIData = PviMarshal.PtrToStringAnsi(hMemory);
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return dataOffset;
		}

		private uint LoadBRWrapedData(LoggerEntry entry, ARLoggerEntry arLoggerEntry, uint dataOffset, int wrapLen, byte[] bytes)
		{
			wrapLen -= Marshal.SizeOf(typeof(ARLoggerEntry));
			int num = (int)arLoggerEntry.lenOfBinData;
			if (wrapLen < num)
			{
				num = wrapLen;
			}
			if (arLoggerEntry.lenOfBinData != 0)
			{
				int num2 = 0;
				entry.propBinary = new byte[arLoggerEntry.lenOfBinData];
				for (num2 = 0; num2 < num; num2++)
				{
					entry.propBinary[num2] = bytes[dataOffset];
					dataOffset++;
				}
				if (dataOffset == _LogDataLength)
				{
					dataOffset = 0u;
				}
				for (int i = num2; i < arLoggerEntry.lenOfBinData; i++)
				{
					entry.propBinary[i] = bytes[dataOffset];
					dataOffset++;
				}
			}
			int lenOfAsciiString = (int)arLoggerEntry.lenOfAsciiString;
			lenOfAsciiString = ((wrapLen > arLoggerEntry.lenOfBinData) ? (wrapLen - (int)arLoggerEntry.lenOfBinData) : 0);
			if (arLoggerEntry.lenOfAsciiString != 0)
			{
				int num3 = 0;
				IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)(long)arLoggerEntry.lenOfAsciiString);
				for (num3 = 0; num3 < lenOfAsciiString; num3++)
				{
					Marshal.WriteByte(hMemory, num3, bytes[dataOffset]);
					dataOffset++;
				}
				if (dataOffset == _LogDataLength)
				{
					dataOffset = 0u;
				}
				for (int j = num3; j < arLoggerEntry.lenOfAsciiString; j++)
				{
					Marshal.WriteByte(hMemory, j, bytes[dataOffset]);
					dataOffset++;
				}
				entry.propASCIIData = PviMarshal.PtrToStringAnsi(hMemory);
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return dataOffset;
		}

		private ARLoggerEntry LoadBRFileReadAREntryStruct(bool bWrap, ref uint dataOffset, int wrapLen, byte[] bytes)
		{
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(ARLoggerEntry)));
			if (bWrap)
			{
				int num = Marshal.SizeOf(typeof(ARLoggerEntry));
				int num2 = 0;
				for (num2 = 0; num2 < wrapLen && num2 < num; num2++)
				{
					Marshal.WriteByte(hMemory, num2, bytes[dataOffset]);
					dataOffset++;
				}
				if (num2 < num)
				{
					dataOffset = 0u;
					int num3 = num2;
					for (num2 = num3; num2 < num; num2++)
					{
						Marshal.WriteByte(hMemory, num2, bytes[dataOffset]);
						dataOffset++;
					}
				}
			}
			else
			{
				for (int i = 0; i < Marshal.SizeOf(typeof(ARLoggerEntry)); i++)
				{
					Marshal.WriteByte(hMemory, i, bytes[dataOffset]);
					dataOffset++;
				}
			}
			ARLoggerEntry result = (ARLoggerEntry)Marshal.PtrToStructure(hMemory, typeof(ARLoggerEntry));
			PviMarshal.FreeHGlobal(ref hMemory);
			return result;
		}

		private int LoadBRFile(string keyName, string filePath)
		{
			int severityCode = 0;
			int customerCode = 0;
			int facilityCode = 0;
			uint bytesRead = 0u;
			uint num = 0u;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			uint num4 = 0u;
			uint num5 = 0u;
			LoggerXMLInterpreter loggerXMLInterpreter = new LoggerXMLInterpreter();
			if (!(propParent is Service) && !(propParent is Logger))
			{
				return 5;
			}
			FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			File.GetAttributes(filePath);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			num2 = LoadBRHeaderSection(binaryReader, ref bytesRead, out string strModName, out uint offVWSection, out uint offLogDataSection);
			try
			{
				if (num2 == 0)
				{
					num2 = LoadBRVWSection(binaryReader, offLogDataSection, offVWSection, ref bytesRead);
				}
				Logger logger = null;
				if (num2 == 0)
				{
					logger = GetLoggerObject(keyName, strModName, filePath);
					num2 = 14;
				}
				if (logger == null)
				{
					binaryReader.Close();
					fileStream.Close();
					return num2;
				}
				byte[] array = binaryReader.ReadBytes((int)_LogDataLength);
				bytesRead += _LogDataLength;
				uint logDataLength = _LogDataLength;
				if (array != null)
				{
					num = _ActOffset;
					num3 = 0;
					flag = false;
					num4 = 0u;
					num5 = 0u;
					while (true)
					{
						num4 = num;
						ARLoggerEntry arLoggerEntry = LoadBRFileReadAREntryStruct(flag, ref num, num3, array);
						flag = false;
						if (num4 + arLoggerEntry.entLen > logDataLength)
						{
							num3 = (int)(logDataLength - num4 - _InvalidLength);
							flag = true;
						}
						if (arLoggerEntry.recTagBegin != 3735901816u)
						{
							break;
						}
						DateTime dateTime = Pvi.UInt32ToDateTime(Convert.ToUInt32(arLoggerEntry.logTime.sec));
						long value = Convert.ToInt64(arLoggerEntry.logTime.nanoSec) / 100;
						dateTime = dateTime.AddTicks(value);
						LoggerEntry loggerEntry = new LoggerEntry(logger, dateTime);
						loggerEntry.SetLoggerModuleName(strModName);
						loggerEntry.propLevelType = (LevelType)arLoggerEntry.logLevel;
						loggerEntry.propErrorNumber = arLoggerEntry.errorNumber;
						loggerEntry.propTask = arLoggerEntry.taskName;
						loggerEntry.propTask = arLoggerEntry.taskName.RemoveAsciiControlChars();
						loggerEntry.propErrorInfo = arLoggerEntry.infoFlags;
						loggerEntry._AdditionalDataFormat = 0;
						if (propContentVersion >= 4112)
						{
							loggerEntry._RecordId = arLoggerEntry.index;
							loggerEntry._AdditionalDataFormat = (int)((arLoggerEntry.infoFlags & 0xFF0000) >> 16);
							loggerEntry.propEventId = (int)arLoggerEntry.ulEventId;
							loggerEntry.propOriginRecordId = (int)arLoggerEntry.ulOriginRecordId;
							if (loggerEntry.propEventId != 0)
							{
								loggerEntry.propErrorNumber = loggerXMLInterpreter.DecodeEventID(loggerEntry.propEventId, loggerEntry.propErrorInfo, propContentVersion, ref severityCode, ref customerCode, ref facilityCode);
								if (8 != (loggerEntry.propErrorInfo & 8))
								{
									loggerEntry.propErrorNumber = arLoggerEntry.errorNumber;
								}
								loggerEntry.propLevelType = (LevelType)severityCode;
								loggerEntry.propCustomerCode = customerCode;
								loggerEntry.propFacilityCode = facilityCode;
							}
							else
							{
								loggerEntry.propCustomerCode = 0;
								loggerEntry.propFacilityCode = 0;
							}
						}
						loggerEntry._internId = arLoggerEntry.index;
						loggerEntry.UpdateUKey();
						logger.LoggerEntries.propContentVersion = (logger.propContentVersion = propContentVersion);
						logger.LoggerEntries.Add(loggerEntry.UniqueKey, loggerEntry);
						if (flag)
						{
							num = LoadBRWrapedData(loggerEntry, arLoggerEntry, num, num3, array);
						}
						else
						{
							num = LoadBRBinary(loggerEntry, arLoggerEntry, num, array);
						}
						if (arLoggerEntry.lenOfBinData != 0)
						{
							if (propContentVersion >= 4112 && 0 < loggerEntry.AdditionalDataFormat)
							{
								loggerXMLInterpreter.DecodeAdditionalData(propContentVersion, loggerEntry, loggerEntry.AdditionalDataFormat, loggerEntry.Binary.GetLength(0), loggerEntry.Binary, useBinForI386exc: true);
							}
							else if ((arLoggerEntry.infoFlags & 2) != 0 || (arLoggerEntry.infoFlags & 1) != 0)
							{
								loggerEntry._AdditionalDataFormat = 254;
								loggerEntry.GetExceptionData(isARM: false, useBinForI386exc: true);
							}
						}
						num5 += arLoggerEntry.entLen;
						if (arLoggerEntry.index == _ReferenceIndex || arLoggerEntry.prevLen == 0 || num5 >= _LogDataLength)
						{
							break;
						}
						if (num4 < arLoggerEntry.prevLen)
						{
							num3 = (int)(arLoggerEntry.prevLen - num4);
							num = (uint)(logDataLength - num3 - _InvalidLength);
							flag = true;
						}
						else
						{
							flag = false;
							num4 -= arLoggerEntry.prevLen;
							num = num4;
						}
					}
				}
			}
			catch
			{
				binaryReader.Close();
				fileStream.Close();
				return 12;
			}
			binaryReader.Close();
			fileStream.Close();
			return 0;
		}

		public int Load(string filePath)
		{
			return LoadEntries(filePath, filePath);
		}

		public int Load(string filePath, string keyName)
		{
			return LoadEntries(keyName, filePath);
		}

		private void AddNewEntriesToServiceCollection(LoggerEntryCollection logEntries)
		{
			if (Parent is Logger && ((Logger)Parent).GlobalMerge)
			{
				((Logger)Parent).AddEntriesToServiceCollection(logEntries);
			}
		}

		private int LoadEntries(string keyName, string filePath)
		{
			int result = 2;
			if (!File.Exists(filePath))
			{
				return result;
			}
			try
			{
				propName = keyName;
				result = (Path.GetExtension(filePath).ToLower().Equals(".br") ? LoadBRFile(keyName, filePath) : ((!(propParent is Service)) ? LoadLocalArl(keyName, filePath) : LoadServiceBasedArl(keyName, filePath)));
				if (result != 0)
				{
					return result;
				}
				AddNewEntriesToServiceCollection(this);
				return result;
			}
			catch
			{
				return 11;
			}
		}

		private string ValidateTimestampString(string timeStamp)
		{
			int num = 0;
			string text = timeStamp;
			num = timeStamp.LastIndexOf('.');
			if (0 < num)
			{
				if (timeStamp.Substring(num + 1).Length == 0)
				{
					text = timeStamp.Substring(0, num) + ".000" + timeStamp.Substring(num + 1);
				}
				else if (1 == timeStamp.Substring(num + 1).Length)
				{
					text = timeStamp.Substring(0, num) + ".00" + timeStamp.Substring(num + 1);
				}
				else if (2 == timeStamp.Substring(num + 1).Length)
				{
					text = timeStamp.Substring(0, num) + ".0" + timeStamp.Substring(num + 1);
				}
			}
			else
			{
				text += ".000000";
			}
			return text;
		}

		private void LoadArlBackTraceEntries(XmlTextReader xmlReader, Backtrace backtrace)
		{
			do
			{
				if (xmlReader.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				if (string.Compare(xmlReader.Name, "Backtrace") == 0 && !xmlReader.IsStartElement())
				{
					break;
				}
				if (string.Compare(xmlReader.Name.ToLower(), "parameters") == 0 && xmlReader.IsStartElement())
				{
					string text = xmlReader.ReadElementString();
					if (text.Length > 0)
					{
						text = text.Trim(' ');
						string[] array = text.Split(' ');
						backtrace.propParameter = new uint[backtrace.propParamCount];
						for (int i = 0; i < array.Length; i++)
						{
							backtrace.propParameter[i] = Convert.ToUInt32(array[i]);
						}
					}
				}
				if (string.Compare(xmlReader.Name.ToLower(), "functioninfo") == 0 && xmlReader.IsStartElement())
				{
					backtrace.propFunctionInfo = new FunctionInfo();
					backtrace.propFunctionInfo.codeOffset = Convert.ToUInt32(xmlReader.GetAttribute("CodeOffset"));
					backtrace.propFunctionInfo.moduleName = xmlReader.GetAttribute("ModuleName");
				}
				if (string.Compare(xmlReader.Name.ToLower(), "callstack") == 0 && xmlReader.IsStartElement())
				{
					backtrace.propCallstack = new Callstack();
					backtrace.propCallstack.propCodeOffset = Convert.ToUInt32(xmlReader.GetAttribute("CodeOffset"));
					backtrace.propCallstack.propModuleName = xmlReader.GetAttribute("ModuleName");
				}
				if (string.Compare(xmlReader.Name.ToLower(), "pcinfo") == 0 && xmlReader.IsStartElement())
				{
					backtrace.propPCInfo = new PCInfo();
					backtrace.propPCInfo.propCodeOffset = Convert.ToUInt32(xmlReader.GetAttribute("CodeOffset"));
					backtrace.propPCInfo.propModuleName = xmlReader.GetAttribute("ModuleName");
				}
			}
			while ((string.Compare(xmlReader.Name, "Backtrace") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
		}

		private void LoadArlBackTrace(XmlTextReader xmlReader, Backtrace backtrace, LoggerEntry entry)
		{
			if (string.Compare(xmlReader.Name.ToLower(), "backtrace") != 0 || !xmlReader.IsStartElement())
			{
				return;
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			text = xmlReader.GetAttribute("Address");
			text2 = xmlReader.GetAttribute("FunctionName");
			text3 = xmlReader.GetAttribute("ParamCount");
			text4 = xmlReader.GetAttribute("Info");
			if (entry.propException.propBacktrace == null)
			{
				entry.propException.propBacktrace = new Backtrace();
				backtrace = entry.propException.propBacktrace;
			}
			else
			{
				Backtrace backtrace2 = null;
				backtrace = entry.propException.propBacktrace;
				for (backtrace2 = backtrace.propNextBacktrace; backtrace2 != null; backtrace2 = backtrace2.NextBacktrace)
				{
					backtrace = backtrace2;
				}
				backtrace.propNextBacktrace = new Backtrace();
				backtrace = backtrace.propNextBacktrace;
			}
			backtrace.propAddress = Convert.ToUInt32(text);
			backtrace.propFunctionName = text2;
			backtrace.propParamCount = Convert.ToUInt32(text3);
			if (text4 != null)
			{
				if (-1 != text4.ToLower().IndexOf('x'))
				{
					backtrace.propInfo = Convert.ToUInt32(text4, 16);
				}
				else
				{
					backtrace.propInfo = Convert.ToUInt32(text4);
				}
			}
			LoadArlBackTraceEntries(xmlReader, backtrace);
		}

		private void LoadArlMemoryData(XmlTextReader xmlReader, LoggerEntry entry)
		{
			string text = "";
			string text2 = "";
			if (string.Compare(xmlReader.Name.ToLower(), "memorydata") != 0 || !xmlReader.IsStartElement())
			{
				return;
			}
			entry.propException.propMemoryData = new MemoryData();
			while (string.Compare(xmlReader.Name.ToLower(), "memorydata") != 0 || xmlReader.IsStartElement())
			{
				if (string.Compare(xmlReader.Name.ToLower(), "pc") == 0 && xmlReader.IsStartElement())
				{
					text = xmlReader.ReadElementString();
					text = text.Trim(' ');
					string[] array = text.Split(' ');
					entry.propException.propMemoryData.propMemPc = new byte[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						entry.propException.propMemoryData.propMemPc[i] = Convert.ToByte(array[i]);
					}
				}
				if (string.Compare(xmlReader.Name.ToLower(), "esp") == 0 && xmlReader.IsStartElement())
				{
					text2 = xmlReader.ReadElementString();
					text2 = text2.Trim(' ');
					string[] array2 = text2.Split(' ');
					entry.propException.propMemoryData.propMemESP = new byte[array2.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						entry.propException.propMemoryData.propMemESP[j] = Convert.ToByte(array2[j]);
					}
				}
				if ((string.Compare(xmlReader.Name.ToLower(), "memorydata") == 0 && !xmlReader.IsStartElement()) || !xmlReader.Read())
				{
					break;
				}
			}
		}

		private bool LoadArlTaskEAX(XmlTextReader xmlReader, LoggerEntry entry, string attrRegisterEAX)
		{
			if (attrRegisterEAX != null && 0 < attrRegisterEAX.Length)
			{
				string attribute = xmlReader.GetAttribute("RegisterEBX");
				string attribute2 = xmlReader.GetAttribute("RegisterECX");
				string attribute3 = xmlReader.GetAttribute("RegisterEDX");
				string attribute4 = xmlReader.GetAttribute("RegisterESI");
				string attribute5 = xmlReader.GetAttribute("RegisterEDI");
				string attribute6 = xmlReader.GetAttribute("RegisterEIP");
				string attribute7 = xmlReader.GetAttribute("RegisterEBP");
				string attribute8 = xmlReader.GetAttribute("RegisterESP");
				string attribute9 = xmlReader.GetAttribute("RegisterEFLAGS");
				string attribute10 = xmlReader.GetAttribute("StackSize");
				entry.propException.propTaskData.propRegisterEax = Convert.ToUInt32(attrRegisterEAX);
				entry.propException.propTaskData.propRegisterEbx = Convert.ToUInt32(attribute);
				entry.propException.propTaskData.propRegisterEcx = Convert.ToUInt32(attribute2);
				entry.propException.propTaskData.propRegisterEdx = Convert.ToUInt32(attribute3);
				entry.propException.propTaskData.propRegisterEsi = Convert.ToUInt32(attribute4);
				entry.propException.propTaskData.propRegisterEdi = Convert.ToUInt32(attribute5);
				entry.propException.propTaskData.propRegisterEip = Convert.ToUInt32(attribute6);
				entry.propException.propTaskData.propRegisterEbp = Convert.ToUInt32(attribute7);
				entry.propException.propTaskData.propRegisterEsp = Convert.ToUInt32(attribute8);
				entry.propException.propTaskData.propRegisterEflags = Convert.ToUInt32(attribute9);
				entry.propException.propTaskData.propStackSize = Convert.ToUInt32(attribute10);
				return true;
			}
			return false;
		}

		private void LoadArlTaskGPR(XmlTextReader xmlReader, LoggerEntry entry, string attrGeneralPurposeRegister00)
		{
			if (attrGeneralPurposeRegister00 != null && 0 < attrGeneralPurposeRegister00.Length)
			{
				string attribute = xmlReader.GetAttribute("GPRegister01");
				string attribute2 = xmlReader.GetAttribute("GPRegister02");
				string attribute3 = xmlReader.GetAttribute("GPRegister03");
				string attribute4 = xmlReader.GetAttribute("GPRegister04");
				string attribute5 = xmlReader.GetAttribute("GPRegister05");
				string attribute6 = xmlReader.GetAttribute("GPRegister06");
				string attribute7 = xmlReader.GetAttribute("GPRegister07");
				string attribute8 = xmlReader.GetAttribute("GPRegister08");
				string attribute9 = xmlReader.GetAttribute("GPRegister09");
				string attribute10 = xmlReader.GetAttribute("GPRegister10");
				string attribute11 = xmlReader.GetAttribute("FramePointer");
				string attribute12 = xmlReader.GetAttribute("GPRegister12");
				string attribute13 = xmlReader.GetAttribute("StackPointer");
				string attribute14 = xmlReader.GetAttribute("LinkRegister");
				string attribute15 = xmlReader.GetAttribute("ProgramCounter");
				string attribute16 = xmlReader.GetAttribute("CurrentProgramStatusRegister");
				string attribute17 = xmlReader.GetAttribute("TranslationTableBaseControlRegister");
				_ARMExceptions = true;
				entry.propException.propTaskData._ARMRegisters = new ARMRegisters();
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister00 = Convert.ToUInt32(attrGeneralPurposeRegister00);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister01 = Convert.ToUInt32(attribute);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister02 = Convert.ToUInt32(attribute2);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister03 = Convert.ToUInt32(attribute3);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister04 = Convert.ToUInt32(attribute4);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister05 = Convert.ToUInt32(attribute5);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister06 = Convert.ToUInt32(attribute6);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister07 = Convert.ToUInt32(attribute7);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister08 = Convert.ToUInt32(attribute8);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister09 = Convert.ToUInt32(attribute9);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister10 = Convert.ToUInt32(attribute10);
				entry.propException.propTaskData._ARMRegisters.FramePointer = Convert.ToUInt32(attribute11);
				entry.propException.propTaskData._ARMRegisters.GeneralPurposeRegister12 = Convert.ToUInt32(attribute12);
				entry.propException.propTaskData._ARMRegisters.StackPointer = Convert.ToUInt32(attribute13);
				entry.propException.propTaskData._ARMRegisters.LinkRegister = Convert.ToUInt32(attribute14);
				entry.propException.propTaskData._ARMRegisters.ProgramCounter = Convert.ToUInt32(attribute15);
				entry.propException.propTaskData._ARMRegisters.CurrentProgramStatusRegister = Convert.ToUInt32(attribute16);
				entry.propException.propTaskData._ARMRegisters.TranslationTableBaseControlRegister = Convert.ToUInt32(attribute17);
			}
		}

		private void LoadArlTaskData(XmlTextReader xmlReader, LoggerEntry entry)
		{
			if (string.Compare(xmlReader.Name.ToLower(), "taskdata") == 0 && xmlReader.IsStartElement())
			{
				string attribute = xmlReader.GetAttribute("ID");
				string attribute2 = xmlReader.GetAttribute("Priority");
				string attribute3 = xmlReader.GetAttribute("Name");
				string attribute4 = xmlReader.GetAttribute("StackBegin");
				string attribute5 = xmlReader.GetAttribute("StackEnd");
				string attribute6 = xmlReader.GetAttribute("RegisterEAX");
				string attribute7 = xmlReader.GetAttribute("GPRegister00");
				entry.propException.propTaskData = new TaskData();
				entry.propException.propTaskData.propId = Convert.ToUInt32(attribute);
				entry.propException.propTaskData.propPriority = Convert.ToUInt32(attribute2);
				entry.propException.propTaskData.propName = attribute3;
				entry.propException.propTaskData.propStackBegin = Convert.ToUInt32(attribute4);
				entry.propException.propTaskData.propStackEnd = Convert.ToUInt32(attribute5);
				if (!LoadArlTaskEAX(xmlReader, entry, attribute6))
				{
					LoadArlTaskGPR(xmlReader, entry, attribute7);
				}
			}
		}

		private void LoadArlProcessorData(XmlTextReader xmlReader, LoggerEntry entry)
		{
			if (string.Compare(xmlReader.Name.ToLower(), "processordata") == 0 && xmlReader.IsStartElement())
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				text = xmlReader.GetAttribute("ProgramCounter");
				text2 = xmlReader.GetAttribute("EFlags");
				text3 = xmlReader.GetAttribute("CPSR");
				text4 = xmlReader.GetAttribute("ErrorCode");
				entry.propException.propProcessorData = new ProcessorData();
				entry.propException.propProcessorData.propProgramCounter = Convert.ToUInt32(text);
				if (text2 != null && 0 < text2.Length)
				{
					entry.propException.propProcessorData.propEFlags = Convert.ToUInt32(text2);
				}
				entry.propException.propProcessorData.propErrorCode = Convert.ToUInt32(text4);
				if (text3 != null && 0 < text3.Length)
				{
					entry.Exception.ProcessorData._CurrentProgramStatusRegister = Convert.ToUInt32(text3);
				}
			}
		}

		private void LoadArlExceptionData(XmlTextReader xmlReader, LoggerEntry entry, uint frmtVersion)
		{
			if (frmtVersion < 1 && string.Compare(xmlReader.Name.ToLower(), "exception") == 0)
			{
				entry._AdditionalDataFormat = 254;
				Backtrace backtrace = null;
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				text = xmlReader.GetAttribute("Type");
				text2 = xmlReader.GetAttribute("BacktraceCount");
				text3 = xmlReader.GetAttribute("DataLength");
				text4 = xmlReader.GetAttribute("ArVersion");
				entry.propException = new Exception();
				switch (text)
				{
				case "Processor":
					entry.propException.propType = ExceptionType.Processor;
					entry.propErrorInfo |= 1u;
					break;
				case "System":
					entry.propException.propType = ExceptionType.System;
					entry.propErrorInfo |= 2u;
					break;
				default:
					entry.propException.propType = ExceptionType.System;
					entry.propErrorInfo |= 2u;
					break;
				}
				entry.propException.propBacktraceCount = Convert.ToUInt32(text2);
				entry.propException.propARVersion = text4;
				entry.propRuntimeVersion = text4;
				entry.propException.propDataLength = Convert.ToUInt32(text3);
				do
				{
					LoadArlProcessorData(xmlReader, entry);
					LoadArlTaskData(xmlReader, entry);
					LoadArlMemoryData(xmlReader, entry);
					LoadArlBackTrace(xmlReader, backtrace, entry);
				}
				while ((string.Compare(xmlReader.Name, "Exception") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
			}
		}

		private void LoadArlASCIIData(XmlTextReader xmlReader, LoggerEntry entry)
		{
			if (string.Compare(xmlReader.Name.ToLower(), "ascii") == 0)
			{
				string s = xmlReader.ReadElementString();
				Encoding encoding = Encoding.GetEncoding(1252);
				byte[] bytes = encoding.GetBytes(s);
				string @string = encoding.GetString(bytes);
				if (entry.EventDataType == EventDataTypes.ArLoggerAPI)
				{
					entry.propEventData = @string;
				}
				else
				{
					entry.propASCIIData = @string;
				}
			}
		}

		private void CutOffAsciiDataFromAPIEntry(LoggerEntry entry)
		{
			if (EventDataTypes.ArLoggerAPI != entry.EventDataType || entry.EventData == null || entry.EventData.ToString().Length > entry.Binary.Length)
			{
				return;
			}
			string text = entry.EventData.ToString();
			int num = 0;
			for (int i = 0; i < text.Length && entry.Binary[i] == (byte)text[i]; i++)
			{
				num++;
			}
			if (text.Length == num)
			{
				object obj = entry.propBinary.Clone();
				entry.propBinary = new byte[entry.propBinary.Length - text.Length - 1];
				int num2 = 0;
				for (int j = 1 + text.Length; j < ((byte[])obj).Length; j++)
				{
					entry.propBinary.SetValue(((byte[])obj).GetValue(j), num2);
					num2++;
				}
			}
		}

		private void LoadArlBinaryData(XmlTextReader xmlReader, LoggerEntry entry, LoggerXMLInterpreter logXMLParser, bool useBinForI386exc)
		{
			if (string.Compare(xmlReader.Name.ToLower(), "binary") != 0 || !xmlReader.IsStartElement())
			{
				return;
			}
			string text = "";
			text = xmlReader.ReadElementString();
			text = text.Trim(' ');
			if (0 >= text.Length)
			{
				return;
			}
			string[] array = text.Split(' ');
			entry.propBinary = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length == 0)
				{
					entry.propBinary[i] = 0;
				}
				else
				{
					entry.propBinary[i] = Convert.ToByte(array[i]);
				}
			}
			logXMLParser.DecodeAdditionalData(propContentVersion, entry, entry._AdditionalDataFormat, entry.Binary.Length, entry.Binary, useBinForI386exc);
			CutOffAsciiDataFromAPIEntry(entry);
		}

		private void DecodeARLLogLevel(string attributeLevel, LoggerEntry entry)
		{
			switch (attributeLevel)
			{
			case "Success":
			case "0":
				entry.propLevelType = LevelType.Success;
				break;
			case "Warning":
			case "2":
				entry.propLevelType = LevelType.Warning;
				break;
			case "Debug":
			case "4":
				entry.propLevelType = LevelType.Debug;
				break;
			case "Info":
			case "1":
				entry.propLevelType = LevelType.Info;
				break;
			case "Error":
			case "Fatal":
			case "3":
				entry.propLevelType = LevelType.Fatal;
				break;
			default:
				entry.propLevelType = LevelType.Info;
				break;
			}
		}

		private void UpdateARLEntryId(Logger tmpLogger, LoggerEntry entry, bool isServiceBased, ulong ulKey)
		{
			if (isServiceBased)
			{
				entry.UpdateUKey();
				tmpLogger.LoggerEntries.propContentVersion = (tmpLogger.propContentVersion = propContentVersion);
				tmpLogger.LoggerEntries.Add(entry.UniqueKey, entry);
			}
			else
			{
				entry.UpdateUKey(ulKey);
				Add(entry.UniqueKey, entry);
			}
		}

		internal LoggerEntry MakeNewLoggEntry(XmlTextReader xmlReader, LoggerXMLInterpreter logXMLParser, Logger loggerObj, string loggerName, bool isServiceBased, bool adjustTimeStamp, ref string attributeLevel)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			string text10 = "";
			int severityCode = 0;
			int customerCode = 0;
			int facilityCode = 0;
			attributeLevel = xmlReader.GetAttribute("Level");
			text = xmlReader.GetAttribute("Time");
			text2 = xmlReader.GetAttribute("ErrorNumber");
			text3 = xmlReader.GetAttribute("ErrorInfo");
			text4 = xmlReader.GetAttribute("Task");
			text6 = xmlReader.GetAttribute("RecordId");
			text5 = xmlReader.GetAttribute("AdditionalDataFormat");
			text7 = xmlReader.GetAttribute("Info");
			text8 = xmlReader.GetAttribute("OriginRecordId");
			text9 = xmlReader.GetAttribute("EventId");
			if (adjustTimeStamp)
			{
				text = ValidateTimestampString(text);
			}
			LoggerEntry loggerEntry = new LoggerEntry(loggerObj, loggerName, Convert.ToDateTime(text, DateTimeFormatInfo.InvariantInfo), (loggerName.CompareTo("$LOG285$") == 0) ? true : false);
			text10 = xmlReader.GetAttribute("LoggerModule");
			if (!Pvi.IsNullOrEmpty(text10))
			{
				loggerEntry.SetLoggerModuleName(text10);
			}
			loggerEntry.propErrorNumber = Pvi.ToUInt32(text2);
			if (!Pvi.IsNullOrEmpty(text5))
			{
				loggerEntry._AdditionalDataFormat = Convert.ToInt32(text5);
			}
			if (!Pvi.IsNullOrEmpty(text8))
			{
				loggerEntry.propOriginRecordId = Convert.ToInt32(text8);
			}
			if (!Pvi.IsNullOrEmpty(text6))
			{
				loggerEntry._RecordId = Pvi.ToUInt32(text6);
			}
			if (!Pvi.IsNullOrEmpty(text9))
			{
				loggerEntry.propEventId = Convert.ToInt32(text9);
				if (loggerEntry.propEventId != 0)
				{
					loggerObj.UpdateHasEventLogEntries(newValue: true);
				}
			}
			if (!Pvi.IsNullOrEmpty(text3))
			{
				loggerEntry.propErrorInfo = Pvi.ToUInt32(text3);
			}
			else if (!Pvi.IsNullOrEmpty(text7))
			{
				loggerEntry.propErrorInfo = Pvi.ToUInt32(text7);
			}
			if (loggerEntry.propEventId != 0)
			{
				loggerEntry.propErrorNumber = logXMLParser.DecodeEventID(loggerEntry.propEventId, loggerEntry.propErrorInfo, propContentVersion, ref severityCode, ref customerCode, ref facilityCode);
				if (propContentVersion >= 4128 && 8 != (loggerEntry.propErrorInfo & 8))
				{
					loggerEntry.propErrorNumber = Pvi.ToUInt32(text2);
				}
				loggerEntry.propLevelType = (LevelType)severityCode;
				loggerEntry.propCustomerCode = customerCode;
				loggerEntry.propFacilityCode = facilityCode;
			}
			else
			{
				loggerEntry.propCustomerCode = 0;
				loggerEntry.propFacilityCode = 0;
				if (!Pvi.IsNullOrEmpty(attributeLevel))
				{
					DecodeARLLogLevel(attributeLevel, loggerEntry);
				}
				if (!Pvi.IsNullOrEmpty(text3))
				{
					loggerEntry._AdditionalDataFormat = (int)((loggerEntry.propErrorInfo & 0xFF0000) >> 16);
				}
			}
			loggerEntry.propTask = text4;
			loggerEntry._internId = loggerEntry.propID;
			loggerEntry.propEventDataType = EventDataTypes.Undefined;
			if (2 == loggerEntry._AdditionalDataFormat)
			{
				if (loggerEntry.Binary != null && Enum.IsDefined(typeof(EventDataTypes), loggerEntry.Binary.GetValue(0).ToString()))
				{
					loggerEntry.propEventDataType = (EventDataTypes)Enum.Parse(typeof(EventDataTypes), loggerEntry.Binary.GetValue(0).ToString(), ignoreCase: true);
				}
			}
			else if (3 == loggerEntry._AdditionalDataFormat)
			{
				loggerEntry.propEventDataType = EventDataTypes.ArLoggerAPI;
			}
			return loggerEntry;
		}

		private sbyte ReadDaylightSaving(XmlTextReader xmlReader)
		{
			sbyte result = 0;
			string attribute = xmlReader.GetAttribute("DaylightSaving");
			if (attribute != null && 0 < attribute.Length)
			{
				result = (sbyte)((attribute.ToLower() == "on") ? 60 : ((!(attribute.ToLower() == "off")) ? sbyte.Parse(attribute) : 60));
			}
			return result;
		}

		internal uint ReadLoggerEntriesSection(XmlTextReader xmlReader)
		{
			uint result = 0u;
			propContentVersion = 0u;
			string attribute = xmlReader.GetAttribute("formatVersion");
			if (attribute != null && 0 < attribute.Length)
			{
				result = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("Version");
			if (attribute != null && 0 < attribute.Length)
			{
				propContentVersion = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("OffsetUtc");
			if (attribute != null && 0 < attribute.Length)
			{
				_OffsetUtc = Convert.ToInt16(attribute);
			}
			_DaylightSaving = ReadDaylightSaving(xmlReader);
			attribute = xmlReader.GetAttribute("Attributes");
			if (attribute != null && 0 < attribute.Length)
			{
				_LogAttributes = Convert.ToUInt16(attribute);
			}
			attribute = xmlReader.GetAttribute("ActIndex");
			if (attribute != null && 0 < attribute.Length)
			{
				_ActIndex = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("ActOffset");
			if (attribute != null && 0 < attribute.Length)
			{
				_ActOffset = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("InvalidLength");
			if (attribute != null && 0 < attribute.Length)
			{
				_InvalidLength = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("LogDataLength");
			if (attribute != null && 0 < attribute.Length)
			{
				_LogDataLength = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("ReferenceIndex");
			if (attribute != null && 0 < attribute.Length)
			{
				_ReferenceIndex = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("ReferenceOffset");
			if (attribute != null && 0 < attribute.Length)
			{
				_ReferenceOffset = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("Wrap");
			if (attribute != null && 0 < attribute.Length)
			{
				_Wrap = Convert.ToUInt32(attribute);
			}
			attribute = xmlReader.GetAttribute("WriteOffset");
			if (attribute != null && 0 < attribute.Length)
			{
				_WriteOffset = Convert.ToUInt32(attribute);
			}
			return result;
		}

		private void LoadArlEntries(LoggerCollection loggerCollection, XmlTextReader xmlReader, bool isServiceBased, bool onlyOneLogger)
		{
			ulong num = 0uL;
			string attributeLevel = "";
			Logger logger = null;
			LoggerXMLInterpreter logXMLParser = new LoggerXMLInterpreter();
			string text = "";
			uint num2 = 0u;
			bool fixForMsecSaving = false;
			_ARMExceptions = false;
			if (!isServiceBased)
			{
				num = 0uL;
			}
			do
			{
				if (string.Compare(xmlReader.Name, "LoggerEntries") == 0 && xmlReader.IsStartElement())
				{
					num2 = ReadLoggerEntriesSection(xmlReader);
					if (num2 < 1)
					{
						fixForMsecSaving = true;
					}
					continue;
				}
				num++;
				if (string.Compare(xmlReader.Name, "Entry") != 0 || !xmlReader.IsStartElement())
				{
					continue;
				}
				text = xmlReader.GetAttribute("LoggerModule");
				if (isServiceBased && loggerCollection != null)
				{
					if ((logger = loggerCollection[text]) == null)
					{
						logger = new Logger((Service)propParent, text);
						logger.GlobalMerge = true;
						loggerCollection.Add(logger);
					}
					else
					{
						logger.UpdateHasEventLogEntries(newValue: false);
					}
				}
				else
				{
					logger = (Logger)propParent;
				}
				LoggerEntry loggerEntry = MakeNewLoggEntry(xmlReader, logXMLParser, logger, logger.Name, isServiceBased, adjustTimeStamp: true, ref attributeLevel);
				loggerEntry.FixTimeStamp(fixForMsecSaving);
				UpdateARLEntryId(logger, loggerEntry, isServiceBased, num);
				DecodeARLLogLevel(attributeLevel, loggerEntry);
				if (!xmlReader.IsEmptyElement)
				{
					do
					{
						LoadArlASCIIData(xmlReader, loggerEntry);
						LoadArlExceptionData(xmlReader, loggerEntry, num2);
						LoadArlBinaryData(xmlReader, loggerEntry, logXMLParser, (num2 >= 1) ? true : false);
					}
					while ((string.Compare(xmlReader.Name, "Entry") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
				}
			}
			while (xmlReader.Read() && (onlyOneLogger ? string.Compare(xmlReader.Name, "Logger") : ((!onlyOneLogger) ? 1 : 0)) != 0);
		}

		private int LoadServiceBasedArl(string keyName, string file)
		{
			int result = 0;
			XmlTextReader xmlTextReader = null;
			XmlSanitizerStream xmlSanitizerStream = null;
			LoggerCollection loggerCollection = new LoggerCollection(CollectionType.ArrayList, propParent, keyName);
			try
			{
				xmlSanitizerStream = new XmlSanitizerStream(file);
				xmlTextReader = new XmlTextReader(xmlSanitizerStream);
				LoadArlEntries(loggerCollection, xmlTextReader, isServiceBased: true, onlyOneLogger: false);
				return result;
			}
			catch
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
					xmlSanitizerStream.Close();
					xmlTextReader = null;
				}
				return 12054;
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
					xmlSanitizerStream.Close();
				}
			}
		}

		private int LoadLocalArl(string keyName, string file)
		{
			int result = 0;
			XmlTextReader xmlTextReader = null;
			try
			{
				CleanUp(disposing: false);
				xmlTextReader = new XmlTextReader(file);
				LoadArlEntries(null, xmlTextReader, isServiceBased: false, onlyOneLogger: false);
				return result;
			}
			catch
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
					xmlTextReader = null;
				}
				return 12054;
			}
			finally
			{
				xmlTextReader?.Close();
			}
		}

		private void SaveArlAsciiData(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			string text = (entry.EventDataType != EventDataTypes.ArLoggerAPI || entry.EventData == null) ? entry.ErrorText : entry.EventData.ToString();
			if (text != null)
			{
				text = text.Replace("&", "&amp;");
				char[] array = text.ToCharArray();
				text = "";
				for (int i = 0; i < array.Length; i++)
				{
					char c = array[i];
					string str = c.ToString();
					if (' ' > c || '~' < c)
					{
						str = $"&#{(byte)c};";
					}
					text += str;
				}
				text = text.Replace("<", "&lt;");
				text = text.Replace(">", "&gt;");
				text = text.Replace("\"", "&quot;");
				text = text.Replace("'", "&apos;");
				text = text.Replace("\u0012", "&#18;");
				text = text.Replace("\r", "&#13;");
				text = text.Replace("\u007f", "&#127;");
				text = text.Replace("\u0015", "&#21;");
				text = text.Replace("\u001b", "&#27;");
				text = text.Replace("\n", "&#10;");
			}
			else
			{
				text = "";
			}
			xmlTextBlock.Append($"<ASCII>{text}</ASCII>\r\n");
		}

		private void SaveArlBinaryData(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			if (entry.Binary == null || entry.Binary.Length == 0)
			{
				return;
			}
			xmlTextBlock.Append("<Binary>");
			int num = 0;
			byte[] binary = entry.Binary;
			for (int i = 0; i < binary.Length; i++)
			{
				byte b = binary[i];
				xmlTextBlock.Append(b.ToString());
				num++;
				if (num == entry.Binary.Length)
				{
					break;
				}
				xmlTextBlock.Append(" ");
			}
			xmlTextBlock.Append("</Binary>\r\n");
		}

		private void SaveArlExceptionBackTrace(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			Backtrace backtrace = entry.Exception.Backtrace;
			while (backtrace != null)
			{
				Backtrace backtrace2 = backtrace;
				backtrace = backtrace2.NextBacktrace;
				xmlTextBlock.Append($"<Backtrace Address=\"{backtrace2.Address.ToString()}\" FunctionName=\"{backtrace2.FunctionName}\" ParamCount=\"{backtrace2.Paramcount.ToString()}\"");
				if (0 < backtrace2.Info)
				{
					xmlTextBlock.Append($" Info=\"0x{backtrace2.Info:X8}\">\r\n");
				}
				else
				{
					xmlTextBlock.Append(">\r\n");
				}
				if (backtrace2.Parameter != null && 0 < backtrace2.Parameter.Length)
				{
					xmlTextBlock.Append("<Parameters>");
					for (int i = 0; i < backtrace2.Parameter.Length; i++)
					{
						xmlTextBlock.Append(backtrace2.Parameter[i].ToString());
						if (i == backtrace2.Parameter.Length - 1)
						{
							break;
						}
						xmlTextBlock.Append(" ");
					}
					xmlTextBlock.Append("</Parameters>\r\n");
				}
				if (backtrace2.FunctionInfo != null)
				{
					xmlTextBlock.Append($"<FunctionInfo ModuleName=\"{backtrace2.FunctionInfo.ModuleName}\" CodeOffset=\"{backtrace2.FunctionInfo.CodeOffset}\"/>\r\n");
				}
				if (backtrace2.propCallstack != null)
				{
					string moduleName = backtrace2.Callstack.ModuleName;
					moduleName = moduleName.Replace("<", "&lt;");
					moduleName = moduleName.Replace(">", "&gt;");
					moduleName = moduleName.Replace("\"", "&quot;");
					moduleName = moduleName.Replace("'", "&apos;");
					moduleName = moduleName.Replace("\u0012", "&#18;");
					moduleName = moduleName.Replace("\r", "&#13;");
					moduleName = moduleName.Replace("\u007f", "&#127;");
					moduleName = moduleName.Replace("\u0015", "&#21;");
					moduleName = moduleName.Replace("\u001b", "&#27;");
					moduleName = moduleName.Replace("\n", "&#10;");
					xmlTextBlock.Append($"<Callstack ModuleName=\"{moduleName}\" CodeOffset=\"{backtrace2.Callstack.CodeOffset}\"/>\r\n");
				}
				if (backtrace2.PCInfo != null)
				{
					xmlTextBlock.Append($"<PCInfo ModuleName=\"{backtrace2.PCInfo.ModuleName}\" CodeOffset=\"{backtrace2.PCInfo.CodeOffset}\"/>\r\n");
				}
				xmlTextBlock.Append("</Backtrace>\r\n");
			}
		}

		private void SaveArlExceptionMemData(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			if (entry.Exception.MemoryData == null)
			{
				return;
			}
			xmlTextBlock.Append("<MemoryData>\r\n");
			xmlTextBlock.Append("<PC>");
			int num = 0;
			byte[] pC = entry.Exception.MemoryData.PC;
			for (int i = 0; i < pC.Length; i++)
			{
				byte b = pC[i];
				xmlTextBlock.Append(b.ToString());
				num++;
				if (num == entry.Exception.MemoryData.PC.Length)
				{
					break;
				}
				xmlTextBlock.Append(" ");
			}
			xmlTextBlock.Append("</PC>\r\n");
			xmlTextBlock.Append("<ESP>");
			num = 0;
			byte[] eSP = entry.Exception.MemoryData.ESP;
			for (int j = 0; j < eSP.Length; j++)
			{
				byte b2 = eSP[j];
				xmlTextBlock.Append(b2.ToString());
				num++;
				if (num == entry.Exception.MemoryData.ESP.Length)
				{
					break;
				}
				xmlTextBlock.Append(" ");
			}
			xmlTextBlock.Append("</ESP>\r\n");
			xmlTextBlock.Append("</MemoryData>\r\n");
		}

		private void SaveArlExceptionRegisters(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			if (entry.Exception.TaskData.ARMRegisters != null)
			{
				xmlTextBlock.Append($"GPRegister00=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister00.ToString()}\" GPRegister01=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister01.ToString()}\" GPRegister02=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister02.ToString()}\" GPRegister03=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister03.ToString()}\" GPRegister04=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister04.ToString()}\" GPRegister05=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister05.ToString()}\" GPRegister06=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister06.ToString()}\" GPRegister07=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister07.ToString()}\" GPRegister08=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister08.ToString()}\" GPRegister09=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister09.ToString()}\" GPRegister10=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister10.ToString()}\" FramePointer=\"{entry.Exception.TaskData.ARMRegisters.FramePointer.ToString()}\" GPRegister12=\"{entry.Exception.TaskData.ARMRegisters.GeneralPurposeRegister12.ToString()}\" StackPointer=\"{entry.Exception.TaskData.ARMRegisters.StackPointer.ToString()}\" LinkRegister=\"{entry.Exception.TaskData.ARMRegisters.LinkRegister.ToString()}\" ProgramCounter=\"{entry.Exception.TaskData.ARMRegisters.ProgramCounter.ToString()}\" CurrentProgramStatusRegister=\"{entry.Exception.TaskData.ARMRegisters.CurrentProgramStatusRegister.ToString()}\" TranslationTableBaseControlRegister=\"{entry.Exception.TaskData.ARMRegisters.TranslationTableBaseControlRegister.ToString()}\"/>\r\n");
			}
			else
			{
				xmlTextBlock.Append($"RegisterEAX=\"{entry.Exception.TaskData.RegisterEAX.ToString()}\" RegisterEBX=\"{entry.Exception.TaskData.RegisterEBX.ToString()}\" RegisterECX=\"{entry.Exception.TaskData.RegisterECX.ToString()}\" RegisterEDX=\"{entry.Exception.TaskData.RegisterEDX.ToString()}\" RegisterESI=\"{entry.Exception.TaskData.RegisterESI.ToString()}\" RegisterEDI=\"{entry.Exception.TaskData.RegisterEDI.ToString()}\" RegisterEIP=\"{entry.Exception.TaskData.RegisterEIP.ToString()}\" RegisterESP=\"{entry.Exception.TaskData.RegisterESP.ToString()}\" RegisterEBP=\"{entry.Exception.TaskData.RegisterEBP.ToString()}\" RegisterEFLAGS=\"{entry.Exception.TaskData.RegisterEFLAGS.ToString()}\"/>\r\n");
			}
		}

		private void SaveArlException(LoggerEntry entry, ref StringBuilder xmlTextBlock)
		{
			if (propContentVersion >= 4112 || entry.Exception != null)
			{
				return;
			}
			xmlTextBlock.Append($"<Exception Type=\"{entry.Exception.Type}\" BacktraceCount=\"{entry.Exception.BacktraceCount.ToString()}\" DataLength=\"{entry.Exception.DataLength.ToString()}\" ArVersion=\"{entry.Exception.ArVersion}\">\r\n");
			if (entry.Exception.ProcessorData != null)
			{
				xmlTextBlock.Append($"<ProcessorData ProgramCounter=\"{entry.Exception.ProcessorData.ProgramCounter.ToString()}\"");
				if (entry.Exception.ProcessorData._CurrentProgramStatusRegister != 0)
				{
					xmlTextBlock.Append($" CPSR=\"{entry.Exception.ProcessorData._CurrentProgramStatusRegister.ToString()}\"/>\r\n");
				}
				else
				{
					xmlTextBlock.Append($" EFlags=\"{entry.Exception.ProcessorData.EFlags.ToString()}\" ErrorCode=\"{entry.Exception.ProcessorData.ErrorCode.ToString()}\"/>\r\n");
				}
			}
			if (entry.Exception.TaskData != null)
			{
				xmlTextBlock.Append($"<TaskData ID=\"{entry.Exception.TaskData.Id.ToString()}\" Priority=\"{entry.Exception.TaskData.Priority.ToString()}\" Name=\"{entry.Exception.TaskData.Name}\" StackBegin=\"{entry.Exception.TaskData.StackBegin.ToString()}\" StackEnd=\"{entry.Exception.TaskData.StackEnd.ToString()}\" StackSize=\"{entry.Exception.TaskData.StackSize.ToString()}\" ");
				SaveArlExceptionRegisters(entry, ref xmlTextBlock);
			}
			SaveArlExceptionMemData(entry, ref xmlTextBlock);
			SaveArlExceptionBackTrace(entry, ref xmlTextBlock);
			xmlTextBlock.Append("</Exception>\r\n");
		}

		public int SaveArlEntries(ref StringBuilder xmlTextBlock)
		{
			int result = 0;
			if (Count == 0)
			{
				return result;
			}
			foreach (LoggerEntry value in Values)
			{
				string text = "";
				long num = value.DateTime.Ticks % 10000000 / 10;
				text = $"{num:000000}";
				if (value.ErrorText != null || ContentVersion >= 4112)
				{
					string text2 = value.LoggerModuleName;
					if (value.propParent is Logger && (text2 == null || text2.Length == 0))
					{
						text2 = ((Logger)value.propParent).Name;
					}
					xmlTextBlock.Append(string.Format("<Entry {0}=\"{1}\"", "Level", value.LevelType.ToString()));
					xmlTextBlock.Append(string.Format(" {0}=\"{1}.{2}\"", "Time", value.DateTime.ToString("s", DateTimeFormatInfo.InvariantInfo), text));
					xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "ErrorNumber", value.ErrorNumber.ToString()));
					if (0 != value.ErrorInfo)
					{
						xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "ErrorInfo", value.ErrorInfo.ToString()));
					}
					if (value.Task != null)
					{
						string arg = value.Task.TransformIntoValidXmlString();
						xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "Task", arg));
					}
					if (propContentVersion >= 4112 || (propContentVersion == 0 && 0 < value.EventID))
					{
						if (value.EventID != 0)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "EventId", value.EventID.ToString()));
						}
						if (0 < value._internId || value._RecordId != 0)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "RecordId", value.RecordId.ToString()));
						}
						if (0 < value.OriginRecordId)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "OriginRecordId", value.OriginRecordId.ToString()));
						}
						if (0 < value.CustomerCode)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "CustomerCode", value.CustomerCode.ToString()));
						}
						if (0 < value.FacilityCode)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "FacilityCode", value.FacilityCode.ToString()));
						}
						if (0 < value.AdditionalDataFormat)
						{
							xmlTextBlock.Append(string.Format(" {0}=\"{1}\"", "AdditionalDataFormat", value.AdditionalDataFormat.ToString()));
						}
					}
					xmlTextBlock.Append(string.Format(" {0}=\"{1}\">\r\n", "LoggerModule", text2.ReplaceInvalidXmlChars().TransformIntoValidXmlString()));
					SaveArlAsciiData(value, ref xmlTextBlock);
					SaveArlBinaryData(value, ref xmlTextBlock);
				}
				else
				{
					string text2 = (!(value.propParent is Logger)) ? "NA" : ((Logger)value.propParent).Name;
					string instance = "";
					if (0 != (value.ErrorInfo & 2) || 0 != (value.ErrorInfo & 1))
					{
						instance = $" ErrorInfo=\"{value.ErrorInfo}\"";
					}
					xmlTextBlock.Append(string.Format("<Entry Level=\"{0}\" Time=\"{1}.{2}\" ErrorNumber=\"{3}\" Task=\"{4}\" LoggerModule=\"{5}\"{6}>\r\n", value.LevelType.ToString(), value.DateTime.ToString("s", DateTimeFormatInfo.InvariantInfo), text, value.ErrorNumber.ToString(), value.propTask.TransformIntoValidXmlString(), text2.ReplaceInvalidXmlChars().TransformIntoValidXmlString(), instance.ReplaceInvalidXmlChars().TransformIntoValidXmlString()));
					SaveArlAsciiData(value, ref xmlTextBlock);
					SaveArlBinaryData(value, ref xmlTextBlock);
				}
				xmlTextBlock.Append("</Entry>\r\n");
			}
			return result;
		}

		public int SaveAs(string file, LogExportFormat fileFormat)
		{
			int num = 0;
			switch (fileFormat)
			{
			case LogExportFormat.HTML:
				return SaveAsHTML(file);
			case LogExportFormat.CSV:
				return SaveAsCSV(file);
			default:
				return SaveAsArl(file);
			}
		}

		internal string FormatLogBookInfo(bool useXMLTags)
		{
			string text = "";
			string format;
			if (useXMLTags)
			{
				format = " {0}=\"{1}\"";
				text = string.Format("{0}=\"{1}\" {2}=\"{3}\" {4}=\"{5}\" {6}=\"{7}\" {8}=\"{9}\"", "Count", Count, "Version", ContentVersion, "OffsetUtc", OffsetUtc, "DaylightSaving", _DaylightSaving.ToString(), "Attributes", LogAttributes);
			}
			else
			{
				format = " {0}={1}";
				text = string.Format("{0}={1} {2}={3} {4}={5} {6}={7} {8}={9}", "Count", Count, "Version", ContentVersion, "OffsetUtc", OffsetUtc, "DaylightSaving", _DaylightSaving.ToString(), "Attributes", LogAttributes);
			}
			if (0 < _ActIndex)
			{
				string str = string.Format(format, "ActIndex", _ActIndex);
				text += str;
			}
			if (0 < _ActOffset)
			{
				string str = string.Format(format, "ActOffset", _ActOffset);
				text += str;
			}
			if (0 < _InvalidLength)
			{
				string str = string.Format(format, "InvalidLength", _InvalidLength);
				text += str;
			}
			if (0 < _LogDataLength)
			{
				string str = string.Format(format, "LogDataLength", _LogDataLength);
				text += str;
			}
			if (0 < _ReferenceIndex)
			{
				string str = string.Format(format, "ReferenceIndex", _ReferenceIndex);
				text += str;
			}
			if (0 < _ReferenceOffset)
			{
				string str = string.Format(format, "ReferenceOffset", _ReferenceOffset);
				text += str;
			}
			if (0 < _Wrap)
			{
				string str = string.Format(format, "Wrap", _Wrap);
				text += str;
			}
			if (0 < _WriteOffset)
			{
				string str = string.Format(format, "WriteOffset", _WriteOffset);
				text += str;
			}
			return text;
		}

		private void SaveHTMColumns(string fileName, StringBuilder htmlTextBlock)
		{
			string value = "<html>\r\n";
			htmlTextBlock.Append(value);
			value = "<head>\r\n";
			htmlTextBlock.Append(value);
			value = $"<title>{fileName}</title>\r\n";
			htmlTextBlock.Append(value);
			value = "</head>\r\n";
			htmlTextBlock.Append(value);
			value = "<body>\r\n";
			htmlTextBlock.Append(value);
			string arg = FormatLogBookInfo(useXMLTags: false);
			value = $"<h4>{fileName} {arg}</h4>\r\n";
			htmlTextBlock.Append(value);
			value = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\">\r\n";
			htmlTextBlock.Append(value);
			value = "<tr bgcolor=\"#E5E5E5\">\r\n";
			htmlTextBlock.Append(value);
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Severity");
			htmlTextBlock.Append(value);
			if (propContentVersion >= 4112)
			{
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "RecordID");
				htmlTextBlock.Append(value);
			}
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Time");
			htmlTextBlock.Append(value);
			if (propContentVersion >= 4112)
			{
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "ID");
				htmlTextBlock.Append(value);
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Area");
				htmlTextBlock.Append(value);
			}
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Entered by");
			htmlTextBlock.Append(value);
			if (propContentVersion >= 4112)
			{
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Origin");
				htmlTextBlock.Append(value);
			}
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "ASCII Data");
			htmlTextBlock.Append(value);
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Binary Data");
			htmlTextBlock.Append(value);
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Logbook");
			htmlTextBlock.Append(value);
			if (propContentVersion >= 4112)
			{
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Format");
				htmlTextBlock.Append(value);
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Facility");
				htmlTextBlock.Append(value);
			}
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Code");
			htmlTextBlock.Append(value);
			if (propContentVersion >= 4112)
			{
				value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "AddDataFormat");
				htmlTextBlock.Append(value);
			}
			value = string.Format("<td align=\"center\"><b>{0}</b></td>\r\n", "Info");
			htmlTextBlock.Append(value);
			value = "</tr>\r\n";
			htmlTextBlock.Append(value);
		}

		private int SaveAsHTML(string file)
		{
			int result = 0;
			StringBuilder stringBuilder = new StringBuilder();
			Backtrace backtrace = null;
			try
			{
				SaveHTMColumns(file, stringBuilder);
				string value;
				if (0 < Count)
				{
					foreach (LoggerEntry value2 in Values)
					{
						string arg;
						switch (value2.LevelType)
						{
						case LevelType.Fatal:
							arg = " bgcolor=\"FFD7D7\"";
							break;
						case LevelType.Warning:
							arg = " bgcolor=\"#FFFFCE\"";
							break;
						default:
							arg = "";
							break;
						}
						value = $"<tr{arg}>\r\n";
						stringBuilder.Append(value);
						value = value2.ToStringHTM(propContentVersion);
						stringBuilder.Append(value);
						string errorDescription = value2.GetErrorDescription((Service != null) ? Service.Language : null);
						if (value2.Exception == null)
						{
							if (errorDescription != null && 0 < errorDescription.Length)
							{
								value = ((propContentVersion < 4112) ? $"<tr><td colspan=\"7\">{errorDescription}</td></tr>" : $"<tr><td colspan=\"15\">{errorDescription}</td></tr>");
								stringBuilder.Append(value);
							}
						}
						else
						{
							value = ((propContentVersion < 4112) ? $"<tr>\r\n<td valign=\"top\" colspan=\"4\">{errorDescription}</td>\r\n<td align=\"right\">\r\n" : $"<tr>\r\n<td valign=\"top\" colspan=\"8\">{errorDescription}</td>\r\n<td align=\"right\">\r\n");
							stringBuilder.Append(value);
							value = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">\r\n";
							stringBuilder.Append(value);
							value = $"<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Exception</td><td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">{value2.Exception.Type.ToString()}</td></tr>\r\n";
							stringBuilder.Append(value);
							value = value2.Exception.ToStringHTM();
							stringBuilder.Append(value);
							if (value2.Exception.ProcessorData != null)
							{
								value = value2.Exception.ProcessorData.ToStringHTM();
								stringBuilder.Append(value);
							}
							if (value2.Exception.TaskData != null)
							{
								value = value2.Exception.TaskData.ToStringHTM();
								stringBuilder.Append(value);
							}
							if (value2.Exception.MemoryData != null)
							{
								value = value2.Exception.MemoryData.ToStringHTM();
								stringBuilder.Append(value);
							}
							int num = 0;
							for (backtrace = value2.Exception.Backtrace; backtrace != null; backtrace = backtrace.NextBacktrace)
							{
								num++;
								value = $"<tr>\r\n<td align=\"left\" valign=\"top\" style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Backtrace #{num}</td>\r\n<td bordercolor=\"#C0C0C0\">\r\n";
								stringBuilder.Append(value);
								value = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">\r\n";
								stringBuilder.Append(value);
								value = backtrace.ToStringHTM();
								stringBuilder.Append(value);
								if (backtrace.FunctionInfo != null)
								{
									value = backtrace.FunctionInfo.ToStringHTM();
									stringBuilder.Append(value);
								}
								if (backtrace.Callstack != null)
								{
									value = backtrace.Callstack.ToStringHTM();
									stringBuilder.Append(value);
								}
								if (backtrace.PCInfo != null)
								{
									value = backtrace.PCInfo.ToStringHTM();
									stringBuilder.Append(value);
								}
								value = "</table>\r\n</td>\r\n</tr>\r\n";
								stringBuilder.Append(value);
							}
							value = "</table>\r\n</td>\r\n<td colspan=\"6\"></td>\r\n</tr>\r\n";
							stringBuilder.Append(value);
						}
					}
				}
				value = "</table>\r\n";
				stringBuilder.Append(value);
				value = "</body>\r\n";
				stringBuilder.Append(value);
				value = "</html>\r\n";
				stringBuilder.Append(value);
				FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(stringBuilder.ToString());
				streamWriter.Close();
				fileStream.Close();
				return result;
			}
			catch (System.Exception)
			{
				return -2;
			}
		}

		private void SaveCSVHeader(StringBuilder csvTextBlock)
		{
			string arg = FormatLogBookInfo(useXMLTags: true);
			string value = $"Origin=\"{Name}\" {arg}\r\n\r\n";
			csvTextBlock.Append(value);
			value = ((propContentVersion < 4112) ? "Severity;Time;Entered by;ASCII Data;Binary Data;Logbook;Code;Info;" : "Severity;RecordID;Time;ID;Area;Entered by;Origin;ASCII Data;Binary Data;Logbook;Format;Facility;Code;Location;AddDataFormat;Info;");
			value += "Exception;BackTraces;DataLen;ARVersion;";
			value = (_ARMExceptions ? (value + "ProgramCounter;CurrentProgramStatusRegister;") : (value + "ProgramCounter;EFlags;ErrorCode;"));
			value += "TaskID;Priority;TaskName;StackBegin;StackEnd;StackSize;";
			value = ((!_ARMExceptions) ? (value + "RegisterEAX;RegisterEBX;RegisterECX;RegisterEDX;RegisterESI;RegisterEDI;RegisterEIP;RegisterESP;RegisterEBP;RegisterEFLAGS;") : (value + "GPRegister00;GPRegister01;GPRegister02;GPRegister03;GPRegister04;GPRegister05;GPRegister06;GPRegister07;GPRegister08;GPRegister09;GPRegister10;FramePointer;GPRegister12;StackPointer;LinkRegister;ProgramCount;CurrentProgramStatusRegister;TranslationTableBaseControlRegister"));
			value += "PCMemory;ESPMemory;";
			value += "BackTraceAddress;BackTraceFunction;BackTraceInfo;BackTraceTask;";
			value += "BackTraceParameters;";
			value += "FnModule;FnCodeOffset;";
			value += "CallStackModule;CallStackCodeOffset;";
			value += "PCInfoModule;PCInfoCodeOffset;";
			value += "Help\r\n";
			csvTextBlock.Append(value);
		}

		private void SaveCSVEntry(StringBuilder csvTextBlock, string strErr)
		{
			string str = "\"\";\"\";\"\";\"\";";
			str += "\"\";\"\";";
			if (!_ARMExceptions)
			{
				str += "\"\";";
			}
			str += "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";";
			if (_ARMExceptions)
			{
				str += "\"\";\"\";";
			}
			str += "\"\";\"\";";
			str += "\"\";\"\";\"\";\"\";";
			str += "\"\";";
			str += "\"\";\"\";";
			str += "\"\";\"\";";
			str += "\"\";\"\";\"";
			str += ((strErr != null) ? strErr : "");
			str += "\"\r\n";
			csvTextBlock.Append(str);
		}

		private void SaveCSVException(StringBuilder csvTextBlock, string strErr, LoggerEntry logEntry)
		{
			string value = logEntry.Exception.ToStringCSV();
			csvTextBlock.Append(value);
			if (logEntry.Exception.ProcessorData != null)
			{
				value = logEntry.Exception.ProcessorData.ToStringCSV();
				csvTextBlock.Append(value);
			}
			else
			{
				value = "\"\";\"\";";
				if (!_ARMExceptions)
				{
					value = "\"\";";
				}
				csvTextBlock.Append(value);
			}
			if (logEntry.Exception.TaskData != null)
			{
				value = logEntry.Exception.TaskData.ToStringCSV();
				csvTextBlock.Append(value);
			}
			else
			{
				value = "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";";
				if (_ARMExceptions)
				{
					value += "\"\";\"\";";
				}
				csvTextBlock.Append(value);
			}
			if (logEntry.Exception.MemoryData != null)
			{
				value = logEntry.Exception.MemoryData.ToStringCSV();
				csvTextBlock.Append(value);
			}
			else
			{
				value = "\"\";\"\";";
				csvTextBlock.Append(value);
			}
			Backtrace backtrace = logEntry.Exception.Backtrace;
			if (backtrace != null)
			{
				value = "";
				while (backtrace != null)
				{
					csvTextBlock.Append(value);
					value = backtrace.ToStringCSV();
					csvTextBlock.Append(value);
					value = "\"";
					csvTextBlock.Append(value);
					for (uint num = 0u; num < backtrace.Paramcount; num++)
					{
						value = $"{backtrace.Parameter[num]} ";
						csvTextBlock.Append(value);
					}
					value = "\";";
					csvTextBlock.Append(value);
					if (backtrace.FunctionInfo != null)
					{
						value = backtrace.FunctionInfo.ToStringCSV();
						csvTextBlock.Append(value);
					}
					else
					{
						value = "\"\";\"\";";
						csvTextBlock.Append(value);
					}
					if (backtrace.Callstack != null)
					{
						value = backtrace.Callstack.ToStringCSV();
						csvTextBlock.Append(value);
					}
					else
					{
						value = "\"\";\"\";";
						csvTextBlock.Append(value);
					}
					if (backtrace.PCInfo != null)
					{
						value = backtrace.PCInfo.ToStringCSV();
						csvTextBlock.Append(value);
					}
					else
					{
						value = "\"\";\"\";";
						csvTextBlock.Append(value);
					}
					value = $"\"{strErr}\"\r\n";
					strErr = "";
					csvTextBlock.Append(value);
					value = "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";";
					value += "\"\";\"\";\"\";\"\";";
					value += "\"\";\"\";\"\";";
					value += "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";";
					value += "\"\";\"\";";
					backtrace = backtrace.NextBacktrace;
				}
			}
			else
			{
				value = "\"\";\"\";\"\";\"\";";
				value += "\"\";";
				value += "\"\";\"\";";
				value += "\"\";\"\";";
				value += "\"\";\"\";";
				value += strErr;
				value += "\r\n";
				csvTextBlock.Append(value);
			}
		}

		private int SaveAsCSV(string file)
		{
			int result = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			try
			{
				SaveCSVHeader(stringBuilder);
				if (0 < Count)
				{
					foreach (LoggerEntry value2 in Values)
					{
						string value = value2.ToStringCSV(propContentVersion);
						stringBuilder.Append(value);
						text = value2.GetErrorDescription((Service != null) ? Service.Language : null);
						if (value2.Exception == null)
						{
							SaveCSVEntry(stringBuilder, text);
						}
						else
						{
							SaveCSVException(stringBuilder, text, value2);
						}
					}
				}
				FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(stringBuilder.ToString());
				streamWriter.Close();
				fileStream.Close();
				return result;
			}
			catch (System.Exception)
			{
				return -2;
			}
		}

		public int Save(ref StringBuilder xmlTextBlock)
		{
			return SaveArlEntries(ref xmlTextBlock);
		}

		public int Save(string file)
		{
			return SaveAsArl(file);
		}

		private int SaveAsArl(string file)
		{
			int result = 0;
			StringBuilder xmlTextBlock = new StringBuilder();
			string executingAssemblyFileVersion = Pvi.GetExecutingAssemblyFileVersion(Assembly.GetExecutingAssembly());
			xmlTextBlock.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<?AutomationStudio Version=\"2.6\"?>\r\n<?AutomationRuntimeIOSystem Version=\"1.0\"?>\r\n<?LoggerControl PviSVersion=\"");
			xmlTextBlock.Append(executingAssemblyFileVersion + "\"?>\r\n");
			string arg = FormatLogBookInfo(useXMLTags: true);
			xmlTextBlock.Append($"<LoggerEntries formatVersion=\"1\" {arg}>\r\n");
			SaveArlEntries(ref xmlTextBlock);
			xmlTextBlock.Append("</LoggerEntries>");
			FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(xmlTextBlock.ToString());
			streamWriter.Close();
			fileStream.Close();
			return result;
		}

		public override void Clear()
		{
			Clear(fireEvent: true);
		}

		internal void Clear(bool fireEvent)
		{
			LoggerEventArgs loggerEventArgs = null;
			if (Parent is Logger && ((Logger)Parent).Service != null && ((Logger)Parent).Service.LoggerEntries != null)
			{
				if (!((Base)Parent).propDisposed && fireEvent)
				{
					loggerEventArgs = new LoggerEventArgs("EntriesRemoved", "EntriesRemoved", 0, ((Logger)Parent).Service.Language, Action.LoggerGlobalRemoved, new LoggerEntryCollection("EntriesRemoved", this));
				}
				if (0 < Count)
				{
					((Logger)Parent).Service.LoggerEntries.RemoveCollection(this);
				}
				if (loggerEventArgs != null)
				{
					((Logger)Parent).CallOnEntriesRemoved(loggerEventArgs);
				}
			}
			if (arrayOfLoggerEntries != null)
			{
				arrayOfLoggerEntries.Clear();
			}
			base.Clear();
		}

		public override bool ContainsKey(object key)
		{
			if (key is LoggerEntryBase)
			{
				return base.ContainsKey(((LoggerEntryBase)key).UniqueKey);
			}
			if (key is string)
			{
				return base.ContainsKey(Convert.ToUInt64(key));
			}
			return base.ContainsKey(key);
		}

		public int Add(LoggerEntryBase entry, bool addKeyOnly)
		{
			if (propParent is Logger && ((Logger)propParent).GlobalMerge)
			{
				((Logger)propParent).Service.LoggerEntries.Add(entry, addKeyOnly);
			}
			SetArrayIndex(entry);
			arrayOfLoggerEntries.Add(entry);
			base.Add(entry.UniqueKey, entry);
			return 0;
		}

		public int Add(LoggerEntryBase entry)
		{
			if (entry == null)
			{
				return -1;
			}
			if (propParent is Logger && ((Logger)propParent).GlobalMerge)
			{
				((Logger)propParent).Service.LoggerEntries.Add(entry);
				((Logger)propParent).Service.LoggerEntries.propContentVersion = ContentVersion;
			}
			SetArrayIndex(entry);
			arrayOfLoggerEntries.Add(entry);
			base.Add(entry.UniqueKey, entry);
			return 0;
		}

		private string GetEntryIDString(uint id)
		{
			return $"{id:0000000000}";
		}

		[CLSCompliant(false)]
		public void Remove(ulong key)
		{
			if (base.ContainsKey(key))
			{
				int arrayIndex = GetArrayIndex((LoggerEntryBase)base[key]);
				if (arrayIndex < arrayOfLoggerEntries.Count)
				{
					arrayOfLoggerEntries.RemoveAt(arrayIndex);
					UpdateArrayIndices(arrayIndex);
				}
				base.Remove(key);
			}
		}

		public override void Remove(string key)
		{
			ulong num = Convert.ToUInt64(key);
			if (base.ContainsKey(num))
			{
				int arrayIndex = GetArrayIndex((LoggerEntryBase)base[num]);
				if (arrayIndex < arrayOfLoggerEntries.Count)
				{
					arrayOfLoggerEntries.RemoveAt(arrayIndex);
					UpdateArrayIndices(arrayIndex);
				}
				base.Remove(num);
			}
		}

		public void RemoveCollection(ICollection iKeys)
		{
			if (arrayOfLoggerEntries.Count <= 0)
			{
				return;
			}
			List<int> list = new List<int>(iKeys.Count);
			foreach (LoggerEntryBase iKey in iKeys)
			{
				base.Remove(iKey.UniqueKey);
				if (iKey.propSArrayIndex != -1)
				{
					list.Add(iKey.propSArrayIndex);
				}
			}
			list.Sort((int a, int b) => b - a);
			for (int i = 0; i < list.Count; i++)
			{
				if (arrayOfLoggerEntries.Count == 0)
				{
					break;
				}
				arrayOfLoggerEntries.RemoveAt(list[i]);
			}
			AdjustSPropArrayIndicies(arrayOfLoggerEntries);
		}

		private void AdjustSPropArrayIndicies(ArrayList ofLoggerEntries)
		{
			for (int i = 0; i < ofLoggerEntries.Count; i++)
			{
				((LoggerEntry)ofLoggerEntries[i]).propSArrayIndex = i;
			}
		}

		public void RemoveCollection(LoggerEntryCollection loggerEntries)
		{
			if (arrayOfLoggerEntries == null || arrayOfLoggerEntries.Count <= 0)
			{
				return;
			}
			ArrayList arrayList = (ArrayList)arrayOfLoggerEntries.Clone();
			arrayOfLoggerEntries = null;
			arrayOfLoggerEntries = new ArrayList(arrayList.Count);
			base.Clear();
			for (int i = 0; i < arrayList.Count; i++)
			{
				LoggerEntry loggerEntry = (LoggerEntry)arrayList[i];
				if (!loggerEntries.ContainsKey(loggerEntry))
				{
					loggerEntry.propSArrayIndex = Count;
					Add(loggerEntry);
				}
			}
			arrayList = null;
		}

		public override void Remove(object key)
		{
			if (key is LoggerEntryBase)
			{
				int arrayIndex = GetArrayIndex((LoggerEntryBase)key);
				base.Remove(((LoggerEntryBase)key).UniqueKey);
				if (arrayIndex < arrayOfLoggerEntries.Count)
				{
					arrayOfLoggerEntries.RemoveAt(arrayIndex);
					UpdateArrayIndices(arrayIndex);
				}
				return;
			}
			ulong num = Convert.ToUInt64(key);
			if (base.ContainsKey(num))
			{
				int arrayIndex = GetArrayIndex((LoggerEntryBase)base[num]);
				if (arrayIndex < arrayOfLoggerEntries.Count)
				{
					arrayOfLoggerEntries.RemoveAt(arrayIndex);
					UpdateArrayIndices(arrayIndex);
				}
				base.Remove(num);
			}
		}

		protected virtual void SetArrayIndex(LoggerEntryBase eEntry)
		{
			eEntry.propArrayIndex = arrayOfLoggerEntries.Count;
		}

		protected virtual int GetArrayIndex(LoggerEntryBase eEntry)
		{
			return eEntry.propArrayIndex;
		}

		protected virtual void UpdateArrayIndices(int idxRemoved)
		{
			int num = 0;
			for (num = idxRemoved; num < arrayOfLoggerEntries.Count; num++)
			{
				((LoggerEntryBase)arrayOfLoggerEntries[num]).propArrayIndex--;
			}
		}

		internal void Initialize(string xmlData)
		{
			propXMLData = xmlData;
		}
	}
}
