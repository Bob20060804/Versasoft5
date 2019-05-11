using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	internal class LoggerXMLInterpreter
	{
		internal LoggerXMLInterpreter()
		{
		}

		private void ParseV100Binary(LoggerEntry logEntry, string attrVal)
		{
			if (!Pvi.IsNullOrEmpty(attrVal))
			{
				logEntry.propBinary = HexConvert.ToBytesNoSwap(attrVal);
				if ((logEntry.propErrorInfo & 2) != 0 || (logEntry.propErrorInfo & 1) != 0)
				{
					logEntry.GetExceptionData(isARM: false, useBinForI386exc: false);
				}
			}
		}

		internal int ParseV1000Content(Logger logParent, string xmlData, ref LoggerEntryCollection eventEntries)
		{
			XmlReader xmlReader = null;
			LoggerEntry loggerEntry = null;
			int result = 0;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(xmlData));
				xmlReader.MoveToContent();
				while (!xmlReader.EOF)
				{
					if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "Logger")
					{
						return result;
					}
					if (xmlReader.NodeType == XmlNodeType.Element || xmlReader.NodeType == XmlNodeType.Text)
					{
						switch (xmlReader.Name)
						{
						case "Entry":
						{
							string value = xmlReader.GetAttribute("Timestamp");
							if (value == null || value.Length == 0)
							{
								value = xmlReader.GetAttribute("Time");
							}
							loggerEntry = new LoggerEntry(logParent, value);
							value = xmlReader.GetAttribute("Error");
							if (!string.IsNullOrEmpty(value))
							{
								loggerEntry.propErrorNumber = Convert.ToUInt32(value);
							}
							value = xmlReader.GetAttribute("Level");
							if (!string.IsNullOrEmpty(value))
							{
								loggerEntry.propLevelType = (LevelType)Convert.ToUInt16(value);
							}
							value = xmlReader.GetAttribute("TaskName");
							if (!string.IsNullOrEmpty(value))
							{
								loggerEntry.propTask = value.ConvertToAsciiCompatible();
							}
							value = xmlReader.GetAttribute("ID");
							if (!string.IsNullOrEmpty(value))
							{
								loggerEntry._internId = Convert.ToUInt32(value);
								loggerEntry.UpdateUKey();
							}
							value = xmlReader.GetAttribute("Info");
							if (!string.IsNullOrEmpty(value))
							{
								if (value.ToLower().IndexOf("x") == -1)
								{
									loggerEntry.propErrorInfo = Convert.ToUInt32(value);
								}
								else
								{
									loggerEntry.propErrorInfo = Convert.ToUInt32(value, 16);
								}
							}
							eventEntries.Add(loggerEntry);
							break;
						}
						case "Text":
						{
							string value = loggerEntry.propASCIIData = xmlReader.ReadElementString();
							break;
						}
						case "Binary":
						{
							string value = xmlReader.ReadElementString();
							ParseV100Binary(loggerEntry, value);
							break;
						}
						case "":
						{
							string value = xmlReader.Value;
							ParseV100Binary(loggerEntry, value);
							break;
						}
						}
					}
					xmlReader.Read();
				}
				return result;
			}
			catch
			{
				return 12054;
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		internal uint DecodeEventID(int eventID, uint errorInfo, uint contentVersion, ref int severityCode, ref int customerCode, ref int facilityCode)
		{
			uint result = (uint)(eventID & 0xFFFF);
			int[] array = new int[2];
			byte[] array2 = new byte[4];
			array[0] = eventID;
			IntPtr hMemory = PviMarshal.AllocHGlobal(4);
			Marshal.Copy(array, 0, hMemory, 1);
			Marshal.Copy(hMemory, array2, 0, 4);
			severityCode = array2[3] >> 6;
			customerCode = ((32 == (array2[3] & 0x20)) ? 1 : 0);
			facilityCode = ((array2[3] & 0xF) << 8) + array2[2];
			if (contentVersion >= 4128)
			{
				result = (uint)((8 != (errorInfo & 8)) ? eventID : (eventID & 0xFFFF));
			}
			else if (1 == facilityCode && customerCode == 0)
			{
				result = (uint)(eventID & 0x1FFFF);
			}
			else if (facilityCode == 0 || 1 == customerCode)
			{
				result = (uint)(eventID & 0xFFFFFFF);
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			array2 = null;
			array = null;
			return result;
		}

		private void Geti386ExceptionData(LoggerEntry logEntry, byte[] binData, bool useBinForI386exc)
		{
			logEntry.propBinary = binData;
			logEntry.GetExceptionData(isARM: false, useBinForI386exc);
		}

		private void GetARMExceptionData(LoggerEntry logEntry, byte[] binData)
		{
			logEntry.propBinary = binData;
			logEntry.GetExceptionData(isARM: true, useBinForI386exc: true);
		}

		private void GetAsciiStringData(LoggerEntry logEntry, byte[] binData, int startOffset)
		{
			if (0 >= binData.Length || (1 == startOffset && binData[0] == 0))
			{
				return;
			}
			logEntry.propEventData = new ArrayList();
			logEntry.propEventDataType = EventDataTypes.ASCIIStrings;
			string @string = Encoding.Default.GetString(binData, startOffset, binData.Length - 1);
			@string = @string.ConvertToAsciiCompatible();
			string text = @string;
			char[] separator = new char[1];
			string[] array = text.Split(separator);
			for (int i = 0; i < array.GetLength(0); i++)
			{
				string instance = array.GetValue(i).ToString();
				instance = instance.RemoveAsciiControl1FChars();
				if (instance.Length == 0)
				{
					break;
				}
				((ArrayList)logEntry.propEventData).Add(instance);
			}
		}

		private int GetStrsEntryCount(byte[] binData, ref int iOffset)
		{
			int num = 0;
			iOffset = 0;
			if (128 != (binData[1] & 0x80))
			{
				iOffset = 2;
				return binData[1];
			}
			if (240 == (binData[1] & 0xF0))
			{
				iOffset = 5;
				return ((binData[1] & 7) << 18) + ((binData[2] & 0x3F) << 12) + ((binData[3] & 0x3F) << 6) + (binData[4] & 0x3F);
			}
			if (224 == (binData[1] & 0xE0))
			{
				iOffset = 4;
				return ((binData[1] & 0xF) << 12) + ((binData[2] & 0x3F) << 6) + (binData[3] & 0x3F);
			}
			if (192 != (binData[1] & 0xC0))
			{
				iOffset = 3;
				return ((binData[1] & 0x1F) << 6) + (binData[2] & 0x3F);
			}
			return 0;
		}

		private bool GetPrefixedStrings(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			int i = 0;
			int num = 0;
			if (136 == binFormat || 138 == binFormat || 139 == binFormat || 140 == binFormat || 141 == binFormat)
			{
				num = GetStrsEntryCount(binData, ref i);
				if (0 < num)
				{
					switch (binFormat)
					{
					case 136:
					{
						logEntry.propEventData = new ArrayList();
						logEntry.propEventDataType = EventDataTypes.MBCSStrings;
						string @string;
						for (; i < binData.Length; i += @string.Length + 1)
						{
							@string = Encoding.Default.GetString(binData, i, binData.Length - i);
							((ArrayList)logEntry.propEventData).Add(@string);
						}
						break;
					}
					case 138:
					{
						logEntry.propEventData = new ArrayList();
						logEntry.propEventDataType = EventDataTypes.UTF16Strings;
						string @string;
						for (; i < binData.Length; i += @string.Length + 1)
						{
							@string = Encoding.Unicode.GetString(binData, i, binData.Length - 1);
							((ArrayList)logEntry.propEventData).Add(@string);
						}
						break;
					}
					case 139:
					{
						logEntry.propEventData = new ArrayList();
						logEntry.propEventDataType = EventDataTypes.UTF16StringsBE;
						string @string;
						for (; i < binData.Length; i += @string.Length + 1)
						{
							@string = Encoding.Unicode.GetString(binData, i, binData.Length - 1);
							((ArrayList)logEntry.propEventData).Add(@string);
						}
						break;
					}
					case 140:
					{
						logEntry.propEventData = new ArrayList();
						logEntry.propEventDataType = EventDataTypes.UTF32Strings;
						string @string;
						for (; i < binData.Length; i += @string.Length + 1)
						{
							@string = Encoding.UTF32.GetString(binData, i, binData.Length - 1);
							((ArrayList)logEntry.propEventData).Add(@string);
						}
						break;
					}
					case 141:
					{
						logEntry.propEventData = new ArrayList();
						logEntry.propEventDataType = EventDataTypes.UTF32Strings;
						string @string;
						for (; i < binData.Length; i += @string.Length + 1)
						{
							@string = Encoding.UTF32.GetString(binData, i, binData.Length - 1);
							((ArrayList)logEntry.propEventData).Add(@string);
						}
						break;
					}
					default:
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private bool GetPrefixedFloatingPoints(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			switch (binFormat)
			{
			case 92:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToFloat32(binData[1], binData[2], binData[3], binData[4]);
					logEntry.propEventDataType = EventDataTypes.Float32;
				}
				break;
			case 93:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToFloat32(binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.Float32BE;
				}
				break;
			case 94:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToDouble64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
					logEntry.propEventDataType = EventDataTypes.Double64;
				}
				break;
			case 95:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToDouble64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.Double64;
				}
				break;
			default:
				return false;
			}
			return true;
		}

		private bool GetPrefixedSpecials(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			switch (binFormat)
			{
			case 96:
				logEntry.propEventData = false;
				logEntry.propEventDataType = EventDataTypes.BooleanFalse;
				break;
			case 97:
				logEntry.propEventData = true;
				logEntry.propEventDataType = EventDataTypes.BooleanTrue;
				break;
			case 100:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt32(binData[1], binData[2], binData[3], binData[4]);
					logEntry.propEventDataType = EventDataTypes.MemAddress;
				}
				break;
			case 101:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt32(binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.MemAddressBE;
				}
				break;
			default:
				return false;
			}
			return true;
		}

		private bool GetPrefixedComplexType(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			if (228 == binFormat)
			{
				logEntry.propEventDataType = EventDataTypes.ComplexType;
				return true;
			}
			return false;
		}

		private bool GetPrefixedQuantityType(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			if (93 < binFormat && 96 > binFormat)
			{
				logEntry.propEventDataType = EventDataTypes.ComplexType;
				return true;
			}
			if (69 == binFormat || 77 == binFormat)
			{
				logEntry.propEventDataType = EventDataTypes.ComplexType;
				return true;
			}
			if (68 == binFormat || 76 == binFormat)
			{
				logEntry.propEventDataType = EventDataTypes.ComplexType;
				return true;
			}
			return false;
		}

		private bool GetPrefixedCharacters(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			switch (binFormat)
			{
			case 80:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = Convert.ToChar(binData[1]);
					logEntry.propEventDataType = EventDataTypes.AsciiChar;
				}
				break;
			case 81:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = Convert.ToChar(binData[1]);
					logEntry.propEventDataType = EventDataTypes.AnsiChar;
				}
				break;
			case 82:
				if (2 < binData.GetLength(0))
				{
					byte[] bytes2 = new byte[2]
					{
						binData[1],
						binData[2]
					};
					logEntry.propEventData = Encoding.Unicode.GetString(bytes2, 0, 2);
					logEntry.propEventDataType = EventDataTypes.UTF16Char;
				}
				break;
			case 83:
				if (2 < binData.GetLength(0))
				{
					byte[] bytes4 = new byte[2]
					{
						binData[2],
						binData[1]
					};
					logEntry.propEventData = Encoding.Unicode.GetString(bytes4, 0, 2);
					logEntry.propEventDataType = EventDataTypes.UTF16CharBE;
				}
				break;
			case 84:
				if (4 < binData.GetLength(0))
				{
					byte[] bytes3 = new byte[4]
					{
						binData[1],
						binData[2],
						binData[3],
						binData[4]
					};
					logEntry.propEventData = Encoding.UTF32.GetString(bytes3);
					logEntry.propEventDataType = EventDataTypes.UTF32Char;
				}
				break;
			case 85:
				if (4 < binData.GetLength(0))
				{
					byte[] bytes = new byte[4]
					{
						binData[4],
						binData[3],
						binData[2],
						binData[1]
					};
					logEntry.propEventData = Encoding.UTF32.GetString(bytes);
					logEntry.propEventDataType = EventDataTypes.UTF32Char;
				}
				break;
			default:
				return false;
			}
			return true;
		}

		private bool GetPrefixedIntegers(LoggerEntry logEntry, byte[] binData, byte binFormat)
		{
			switch (binFormat)
			{
			case 64:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = Convert.ToByte(binData[1]);
					logEntry.propEventDataType = EventDataTypes.UInt8;
				}
				break;
			case 65:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = (sbyte)Convert.ToByte(binData[1]);
					logEntry.propEventDataType = EventDataTypes.Int8;
				}
				break;
			case 66:
				if (2 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt16(binData[1], binData[2]);
					logEntry.propEventDataType = EventDataTypes.UInt16;
				}
				break;
			case 67:
				if (2 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt16(binData[1], binData[2]);
					logEntry.propEventDataType = EventDataTypes.Int16;
				}
				break;
			case 68:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt32(binData[1], binData[2], binData[3], binData[4]);
					logEntry.propEventDataType = EventDataTypes.UInt32;
				}
				break;
			case 69:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt32(binData[1], binData[2], binData[3], binData[4]);
					logEntry.propEventDataType = EventDataTypes.Int32;
				}
				break;
			case 70:
				if (8 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
					logEntry.propEventDataType = EventDataTypes.UInt64;
				}
				break;
			case 71:
				if (8 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
					logEntry.propEventDataType = EventDataTypes.Int64;
				}
				break;
			case 72:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = Convert.ToByte(binData[1]);
					logEntry.propEventDataType = EventDataTypes.UInt8BE;
				}
				break;
			case 73:
				if (1 < binData.GetLength(0))
				{
					logEntry.propEventData = (sbyte)Convert.ToByte(binData[1]);
					logEntry.propEventDataType = EventDataTypes.Int8BE;
				}
				break;
			case 74:
				if (2 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt16(binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.UInt16BE;
				}
				break;
			case 75:
				if (2 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt16(binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.Int16BE;
				}
				break;
			case 76:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt32(binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.UInt32BE;
				}
				break;
			case 77:
				if (4 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt32(binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.Int32BE;
				}
				break;
			case 78:
				if (8 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToUInt64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.UInt64BE;
				}
				break;
			case 79:
				if (8 < binData.GetLength(0))
				{
					logEntry.propEventData = PviMarshal.ToInt64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
					logEntry.propEventDataType = EventDataTypes.Int64BE;
				}
				break;
			default:
				return false;
			}
			return true;
		}

		private void GetAPIASCIIData(uint contentVersion, LoggerEntry logEntry, byte[] binData, bool cutOffAscii)
		{
			if (!Pvi.IsNullOrEmpty(logEntry.propEventData))
			{
				return;
			}
			logEntry.propEventDataType = EventDataTypes.ArLoggerAPI;
			if (0 >= binData.Length || binData[0] == 0)
			{
				return;
			}
			logEntry.propEventData = PviMarshal.ToAnsiString(binData);
			logEntry.propASCIIData = logEntry.propEventData.ToString();
			if (cutOffAscii)
			{
				logEntry.propBinary = new byte[binData.Length - (1 + logEntry.propEventData.ToString().Length)];
				int num = 0;
				for (int i = logEntry.propEventData.ToString().Length + 1; i < binData.Length; i++)
				{
					logEntry.propBinary.SetValue(binData.GetValue(i), num);
					num++;
				}
			}
		}

		private void ExtractStringsAndAdd(LoggerEntry logEntry, string strItem)
		{
			char[] separator = new char[1];
			string[] array = strItem.Split(separator);
			for (int i = 0; i < array.GetLength(0); i++)
			{
				((ArrayList)logEntry.propEventData).Add(array.GetValue(i).ToString());
			}
		}

		private void GetPrefixedData(LoggerEntry logEntry, byte[] binData)
		{
			byte b = 0;
			int num = 0;
			int num2 = 0;
			if (0 >= binData.Length)
			{
				return;
			}
			b = binData[0];
			if (b < 0)
			{
				return;
			}
			if (b < 16)
			{
				num = binData[0];
				if (0 < num)
				{
					logEntry.propEventData = new ArrayList();
					logEntry.propEventDataType = EventDataTypes.ASCIIStrings;
					ExtractStringsAndAdd(logEntry, Encoding.ASCII.GetString(binData, 1, binData.Length - 1));
				}
			}
			else if (b > 16 && b < 32)
			{
				GetAsciiStringData(logEntry, binData, 1);
			}
			else if (b > 16 && b < 32)
			{
				num = binData[0];
				if (0 < num)
				{
					logEntry.propEventData = new ArrayList();
					logEntry.propEventDataType = EventDataTypes.ANSIStrings;
					ExtractStringsAndAdd(logEntry, Encoding.UTF8.GetString(binData, 1, binData.Length - 1));
				}
			}
			else if (b > 127 && b < 136)
			{
				num = binData[0] - 128;
				if (0 < num)
				{
					logEntry.propEventData = new ArrayList();
					logEntry.propEventDataType = EventDataTypes.BytesLigttleEndian;
					for (num2 = 1; num2 < num && num2 < binData.GetLength(0); num2++)
					{
						((ArrayList)logEntry.propEventData).Add(binData[num2]);
					}
				}
			}
			else if (!GetPrefixedIntegers(logEntry, binData, b) && !GetPrefixedCharacters(logEntry, binData, b) && !GetPrefixedSpecials(logEntry, binData, b) && !GetPrefixedFloatingPoints(logEntry, binData, b) && !GetPrefixedStrings(logEntry, binData, b) && !GetPrefixedComplexType(logEntry, binData, b) && !GetPrefixedQuantityType(logEntry, binData, b))
			{
				logEntry.propBinary = binData;
			}
		}

		internal void DecodeAdditionalData(uint contentVersion, LoggerEntry logEntry, int additionalDataFormat, int numOfBytes, byte[] byteBuffer, bool useBinForI386exc)
		{
			DecodeAdditionalData(contentVersion, logEntry, additionalDataFormat, numOfBytes, byteBuffer, useBinForI386exc, cutOffAscii: true);
		}

		internal void DecodeAdditionalData(uint contentVersion, LoggerEntry logEntry, int additionalDataFormat, int numOfBytes, byte[] byteBuffer, bool useBinForI386exc, bool cutOffAscii)
		{
			int num = additionalDataFormat;
			try
			{
				logEntry.propBinary = byteBuffer;
				if (contentVersion < 4112)
				{
					num = 254;
				}
				switch (num)
				{
				case 1:
					GetAsciiStringData(logEntry, byteBuffer, 0);
					break;
				case 2:
					GetPrefixedData(logEntry, byteBuffer);
					break;
				case 3:
					GetAPIASCIIData(contentVersion, logEntry, byteBuffer, cutOffAscii);
					break;
				case 254:
					Geti386ExceptionData(logEntry, byteBuffer, useBinForI386exc);
					break;
				case 255:
					GetARMExceptionData(logEntry, byteBuffer);
					break;
				default:
					logEntry.propBinary = byteBuffer;
					break;
				}
			}
			catch
			{
				logEntry.propBinary = byteBuffer;
			}
		}

		internal void DecodeAdditionalData(uint contentVersion, LoggerEntry logEntry, int additionalDataFormat, string attrVal)
		{
			try
			{
				byte[] array = HexConvert.ToBytesNoSwap(attrVal);
				if (array != null)
				{
					DecodeAdditionalData(contentVersion, logEntry, additionalDataFormat, array.Length, array, useBinForI386exc: true);
				}
			}
			catch
			{
				logEntry.propBinary = null;
			}
		}

		private string DecodeUTF8Data(LoggerEntry logEntry, string xmlData)
		{
			char[] anyOf = new char[35]
			{
				'Ã',
				'Â',
				'\0',
				'\u0001',
				'\u0002',
				'\u0003',
				'\u0004',
				'\u0005',
				'\u0006',
				'\a',
				'\b',
				'\t',
				'\n',
				'\v',
				'\f',
				'\r',
				'\u000e',
				'\u000f',
				'\u0010',
				'\u0011',
				'\u0012',
				'\u0013',
				'\u0014',
				'\u0015',
				'\u0016',
				'\u0017',
				'\u0018',
				'\u0019',
				'\u001a',
				'\u001b',
				'\u001c',
				'\u001d',
				'\u001e',
				'\u001f',
				'\\'
			};
			if (string.IsNullOrEmpty(xmlData))
			{
				return "";
			}
			string @string;
			if (-1 != xmlData.IndexOfAny(anyOf))
			{
				if ('Â' == xmlData[0])
				{
					byte[] bytes = Encoding.Default.GetBytes(xmlData);
					@string = Encoding.UTF8.GetString(bytes);
					if (@string == xmlData || '\u007f' < @string[0])
					{
						return xmlData.ConvertToAsciiCompatible();
					}
					return @string.RemoveAsciiControlChars();
				}
				if ('Ã' == xmlData[0])
				{
					byte[] bytes2 = Encoding.Default.GetBytes(xmlData);
					return Encoding.UTF8.GetString(bytes2);
				}
				return xmlData.RemoveAsciiControl1FChars();
			}
			@string = xmlData.Replace("&lt;", "<");
			return @string.Replace("&gt;", ">");
		}

		internal int ParseV10xxContent(Logger logParent, string xmlData, uint iVersInfo, ref LoggerEntryCollection eventEntries)
		{
			XmlTextReader xmlTextReader = null;
			LoggerEntry loggerEntry = null;
			int severityCode = 0;
			int customerCode = 0;
			int facilityCode = 0;
			byte[] array = new byte[2048];
			int num = 0;
			try
			{
				xmlTextReader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xmlData)));
				xmlTextReader.MoveToContent();
				while (!xmlTextReader.EOF && (xmlTextReader.NodeType != XmlNodeType.EndElement || !(xmlTextReader.Name == "Logger")))
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						switch (xmlTextReader.Name)
						{
						case "Entry":
						{
							string attrVal = xmlTextReader.GetAttribute("TimestampUtc");
							loggerEntry = new LoggerEntry(logParent, attrVal, eventEntries.OffsetUtc);
							attrVal = xmlTextReader.GetAttribute("EventId");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry.propEventId = Convert.ToInt32(attrVal);
							}
							attrVal = xmlTextReader.GetAttribute("Info");
							loggerEntry._AdditionalDataFormat = 0;
							if (!Pvi.IsNullOrEmpty(attrVal))
							{
								loggerEntry.propErrorInfo = Convert.ToUInt32(attrVal);
								loggerEntry._AdditionalDataFormat = (int)((loggerEntry.propErrorInfo & 0xFF0000) >> 16);
							}
							if (loggerEntry.propEventId != 0)
							{
								loggerEntry.propErrorNumber = DecodeEventID(loggerEntry.propEventId, loggerEntry.propErrorInfo, iVersInfo, ref severityCode, ref customerCode, ref facilityCode);
								loggerEntry.propLevelType = (LevelType)severityCode;
								loggerEntry.propCustomerCode = customerCode;
								loggerEntry.propFacilityCode = facilityCode;
							}
							attrVal = xmlTextReader.GetAttribute("RecordId");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry._RecordId = (loggerEntry._internId = Convert.ToUInt32(attrVal));
								loggerEntry.UpdateUKey();
							}
							attrVal = xmlTextReader.GetAttribute("AddDataSize");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry._AdditionalDataSize = Convert.ToUInt32(attrVal);
							}
							attrVal = xmlTextReader.GetAttribute("AddDataFormat");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry._AdditionalDataFormat = Convert.ToInt32(attrVal);
							}
							attrVal = xmlTextReader.GetAttribute("OriginRecordId");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry.propOriginRecordId = Convert.ToInt32(attrVal);
							}
							attrVal = xmlTextReader.GetAttribute("Error");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry._OldErrorNumber = (uint)Convert.ToInt32(attrVal);
								if (loggerEntry._OldErrorNumber != 0)
								{
									loggerEntry.propErrorNumber = loggerEntry._OldErrorNumber;
								}
							}
							attrVal = xmlTextReader.GetAttribute("Severity");
							if (!string.IsNullOrEmpty(attrVal))
							{
								loggerEntry.propLevelType = (LevelType)Convert.ToUInt32(attrVal);
							}
							loggerEntry.propTask = DecodeUTF8Data(loggerEntry, xmlTextReader.GetAttribute("ObjectId"));
							eventEntries.Add(loggerEntry);
							break;
						}
						case "AddData":
							if (loggerEntry != null)
							{
								if (xmlTextReader.CanReadBinaryContent && xmlTextReader.NodeType != XmlNodeType.Element)
								{
									num = xmlTextReader.ReadElementContentAsBinHex(array, 0, array.Length);
									DecodeAdditionalData(4112u, loggerEntry, loggerEntry.AdditionalDataFormat, num, array, useBinForI386exc: true);
								}
								else
								{
									string attrVal = xmlTextReader.ReadElementString();
									DecodeAdditionalData(4112u, loggerEntry, loggerEntry.AdditionalDataFormat, attrVal);
								}
							}
							break;
						}
					}
					xmlTextReader.Read();
				}
			}
			catch
			{
				return 12054;
			}
			finally
			{
				xmlTextReader?.Close();
			}
			return 0;
		}

		internal int ParseXMLContent(Logger logParent, string xmlData, ref LoggerEntryCollection eventEntries, ref uint readVersion)
		{
			int num = 0;
			uint num2 = 0u;
			int num3 = 0;
			num = xmlData.IndexOf("Version=\"");
			if (num > 0)
			{
				num2 = Convert.ToUInt32(xmlData.Substring(num + 9, xmlData.IndexOf('"', num + 9) - (num + 9)));
			}
			else if (-1 != xmlData.IndexOf("Entry ID=\""))
			{
				num2 = 4096u;
			}
			else if (-1 != xmlData.IndexOf("Entry RecordId=\""))
			{
				num2 = 4112u;
			}
			num3 = ((num2 < 4112) ? ParseV1000Content(logParent, xmlData, ref eventEntries) : ParseV10xxContent(logParent, xmlData, num2, ref eventEntries));
			readVersion = num2;
			eventEntries.propContentVersion = num2;
			return num3;
		}
	}
}
