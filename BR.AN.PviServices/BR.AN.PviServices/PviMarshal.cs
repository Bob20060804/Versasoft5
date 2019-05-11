using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BR.AN.PviServices
{
	internal class PviMarshal
	{
		internal class Convert
		{
			internal static byte ToByte(sbyte value)
			{
				byte b = 0;
				if (0 > value)
				{
					return (byte)(256 + value);
				}
				return (byte)value;
			}

			internal static byte ToByte(char value)
			{
				byte b = 0;
				if ('\0' > value)
				{
					return (byte)(256 + value);
				}
				return (byte)value;
			}

			internal static ushort BytesToUShort(byte value1, byte value2)
			{
				byte[] array = new byte[2]
				{
					value1,
					value2
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array);
				}
				return BitConverter.ToUInt16(array, 0);
			}

			internal static short ToInt16(ushort value)
			{
				short num = 0;
				if (32767 < value)
				{
					return (short)(32767 - value);
				}
				return (short)value;
			}

			internal static int ToInt32(uint value)
			{
				int num = 0;
				if (int.MaxValue < value)
				{
					return (int)(int.MaxValue - value);
				}
				return (int)value;
			}
		}

		private const uint LMEM_FIXED = 0u;

		private const uint LMEM_ZEROINIT = 64u;

		private const uint LPTR = 64u;

		[DllImport("coredll.dll")]
		internal static extern IntPtr LocalAlloc(uint uFlags, uint uBytes);

		[DllImport("coredll.dll")]
		internal static extern IntPtr LocalFree(IntPtr hMem);

		internal static string PtrUpToNullToString(IntPtr cb, int size)
		{
			if (IntPtr.Zero == cb || size == 0)
			{
				return null;
			}
			string text = "";
			char[] array = new char[size];
			Marshal.Copy(cb, array, 0, size);
			for (uint num = 0u; num < size && array[num] != 0; num++)
			{
				text += array[num];
			}
			return text;
		}

		internal static string PtrToStringAnsi(IntPtr cb, int size)
		{
			if (IntPtr.Zero == cb || size == 0)
			{
				return null;
			}
			return Marshal.PtrToStringAnsi(cb, size);
		}

		internal static string PtrToStringUTF8(IntPtr cb, uint size)
		{
			if (IntPtr.Zero == cb || size == 0)
			{
				return null;
			}
			byte[] array = new byte[size];
			Marshal.Copy(cb, array, 0, (int)size);
			return Encoding.UTF8.GetString(array);
		}

		internal static IntPtr GetIntPtr(IntPtr ptrSrc, ulong offset)
		{
			return (IntPtr)((long)ptrSrc + (long)offset);
		}

		internal static ulong GetIntPtrAdr(IntPtr ptrSrc, ulong offset)
		{
			return (ulong)((long)ptrSrc + (long)offset);
		}

		internal static string PtrToStringAnsi(IntPtr cb, uint size)
		{
			return PtrToStringAnsi(cb, (int)size);
		}

		internal static string ToAnsiStringNoTermination(IntPtr cb, int size)
		{
			string text = "";
			if (IntPtr.Zero != cb)
			{
				byte[] array = new byte[size];
				Marshal.Copy(cb, array, 0, size);
				for (int i = 0; i < size; i++)
				{
					byte b = array[i];
					if (b == 0)
					{
						break;
					}
					text += (char)b;
				}
				array = null;
			}
			return text;
		}

		internal static string ToAnsiString(IntPtr cb, int size)
		{
			return ToAnsiString(cb, (uint)size);
		}

		internal static string ToAnsiString(IntPtr cb, uint size)
		{
			string text = "";
			if (IntPtr.Zero != cb && 0 < size)
			{
				byte[] array = new byte[size];
				Marshal.Copy(cb, array, 0, (int)size);
				for (int i = 0; i < size; i++)
				{
					byte b = array[i];
					if (b == 0)
					{
						break;
					}
					text += (char)b;
				}
				array = null;
			}
			return text;
		}

		internal static void GetVersionInfos(IntPtr pVersion, int dataLen, ref Hashtable vInfos)
		{
			GetVersionInfos(pVersion, dataLen, ref vInfos, "");
		}

		internal static void GetVersionInfos(IntPtr pVersion, int dataLen, ref Hashtable vInfos, string addToKey)
		{
			string text = "";
			string text2 = "";
			if (0 >= dataLen)
			{
				return;
			}
			string text3 = ToAnsiString(pVersion, dataLen);
			string[] array = text3.Split('\n');
			for (int i = 0; i < array.GetLength(0); i++)
			{
				text3 = array.GetValue(i).ToString();
				if (0 < text3.Length)
				{
					int num = text3.LastIndexOf(' ');
					if (-1 != num)
					{
						text = addToKey + text3.Substring(0, num);
						text2 = text3.Substring(num + 1);
						vInfos.Add(text, text2);
					}
					else
					{
						text2 = text3;
						vInfos.Add(addToKey, text2);
					}
				}
			}
		}

		internal static string ToWString(IntPtr pBuffer, uint bufferLen)
		{
			return ToWString(pBuffer, (int)bufferLen);
		}

		internal static string ToWString(IntPtr pBuffer, int bufferLen)
		{
			string text = null;
			return Marshal.PtrToStringUni(pBuffer, bufferLen / 2 - 1);
		}

		internal static string ToWString(byte[] bBuffer, int byteOffset, int strLen)
		{
			IntPtr hMemory = AllocHGlobal(strLen);
			string text = "";
			Marshal.Copy(bBuffer, byteOffset, hMemory, strLen);
			text = Marshal.PtrToStringUni(hMemory);
			FreeHGlobal(ref hMemory);
			return text;
		}

		internal static string ToAnsiString(byte[] bytes)
		{
			return ToAnsiString(bytes, 0, bytes.GetLength(0));
		}

		internal static string ToAnsiString(byte[] bytes, int offset, int len)
		{
			string text = "";
			if (bytes != null)
			{
				for (int i = offset; i < offset + len && i < bytes.GetLength(0); i++)
				{
					byte b = (byte)bytes.GetValue(i);
					if (b == 0)
					{
						break;
					}
					text += (char)b;
				}
			}
			return text;
		}

		internal static string PtrToStringAnsi(IntPtr cb)
		{
			return Marshal.PtrToStringAnsi(cb);
		}

		internal static IntPtr AllocCoTaskMem(int cb)
		{
			IntPtr intPtr = default(IntPtr);
			return Marshal.AllocHGlobal(cb);
		}

		internal static IntPtr AllocHGlobal(IntPtr cb)
		{
			IntPtr intPtr = default(IntPtr);
			return Marshal.AllocHGlobal(cb);
		}

		internal static IntPtr AllocHGlobal(int cb)
		{
			IntPtr intPtr = default(IntPtr);
			if (0 < cb)
			{
				return Marshal.AllocHGlobal(cb);
			}
			return IntPtr.Zero;
		}

		internal static IntPtr AllocHGlobal(uint cb)
		{
			IntPtr intPtr = default(IntPtr);
			return Marshal.AllocHGlobal((int)cb);
		}

		internal static void FreeHGlobal(ref IntPtr hMemory)
		{
			if (hMemory != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(hMemory);
			}
			hMemory = IntPtr.Zero;
		}

		internal static long ReadInt64(IntPtr ptr)
		{
			return ReadInt64(ptr, 0);
		}

		internal static long ReadInt64(IntPtr ptr, int offset)
		{
			return Marshal.ReadInt64(ptr, offset);
		}

		internal static ulong ReadUInt64(IntPtr ptr, ref int offset)
		{
			long result = Marshal.ReadInt64(ptr, offset);
			offset += 8;
			return (ulong)result;
		}

		internal static byte ReadByte(IntPtr ptr, ref int offset)
		{
			byte result = Marshal.ReadByte(ptr, offset);
			offset++;
			return result;
		}

		internal static ushort ReadUInt16(IntPtr ptr, ref int offset)
		{
			short num = Marshal.ReadInt16(ptr, offset);
			offset += 2;
			return (ushort)num;
		}

		internal static uint ReadUInt32(IntPtr ptr, ref int offset)
		{
			int result = Marshal.ReadInt32(ptr, offset);
			offset += 4;
			return (uint)result;
		}

		internal static uint WmMsgToUInt32(IntPtr ptr)
		{
			int result = 0;
			if (IntPtr.Zero != ptr)
			{
				result = (int)ptr;
			}
			return (uint)result;
		}

		internal static void WmMsgToInt32(ref Message msg, ref int iWParam, ref int iLParam)
		{
			iWParam = (int)msg.WParam;
			iLParam = (int)msg.LParam;
		}

		internal static void WriteSByte(IntPtr ptr, sbyte val)
		{
			WriteSByte(ptr, 0, val);
		}

		internal static void WriteSByte(IntPtr ptr, int offset, sbyte val)
		{
			Marshal.WriteByte(ptr, offset, (byte)val);
		}

		internal static void WriteUInt16(IntPtr ptr, ushort val)
		{
			WriteUInt16(ptr, 0, val);
		}

		internal static void WriteUInt16(IntPtr ptr, int offset, ushort val)
		{
			Marshal.WriteInt16(ptr, offset, (short)val);
		}

		internal static void WriteUInt32(IntPtr ptr, uint val)
		{
			WriteUInt32(ptr, 0, val);
		}

		internal static void WriteUInt32(IntPtr ptr, int offset, uint val)
		{
			Marshal.WriteInt32(ptr, offset, (int)val);
		}

		internal static void WriteUInt64(IntPtr ptr, ulong val)
		{
			WriteUInt64(ptr, 0, val);
		}

		internal static void WriteUInt64(IntPtr ptr, int offset, ulong val)
		{
			WriteInt64(ptr, offset, (long)val);
		}

		internal static void WriteInt64(IntPtr ptr, long val)
		{
			WriteInt64(ptr, 0, val);
		}

		internal static void WriteInt64(IntPtr ptr, int offset, long val)
		{
			Marshal.WriteInt64(ptr, offset, val);
		}

		internal static void WriteSingle(IntPtr ptr, int offset, float val)
		{
			float[] source = new float[1]
			{
				val
			};
			byte[] array = new byte[4];
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			Marshal.Copy(hMemory, array, 0, 4);
			for (int i = 0; i < 4; i++)
			{
				Marshal.WriteByte(ptr, offset + i, array[i]);
			}
			FreeHGlobal(ref hMemory);
		}

		internal static void WriteSingle(IntPtr ptr, int offset, Value val)
		{
			if (IntPtr.Zero != val.pData && 4 == val.DataSize)
			{
				byte[] array = new byte[4];
				Marshal.Copy(val.pData, array, 0, 4);
				for (int i = 0; i < 4; i++)
				{
					Marshal.WriteByte(ptr, offset + i, array[i]);
				}
			}
			else
			{
				WriteSingle(ptr, offset, System.Convert.ToSingle(val.ToString()));
			}
		}

		internal static void WriteDouble(IntPtr ptr, int offset, double val)
		{
			double[] source = new double[1]
			{
				val
			};
			byte[] array = new byte[8];
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 1);
			Marshal.Copy(hMemory, array, 0, 8);
			for (int i = 0; i < 8; i++)
			{
				Marshal.WriteByte(ptr, offset + i, array[i]);
			}
			FreeHGlobal(ref hMemory);
		}

		internal static void WriteDouble(IntPtr ptr, int offset, Value val)
		{
			if (IntPtr.Zero != val.pData && 8 == val.DataSize)
			{
				byte[] array = new byte[8];
				Marshal.Copy(val.pData, array, 0, 8);
				for (int i = 0; i < 8; i++)
				{
					Marshal.WriteByte(ptr, offset + i, array[i]);
				}
			}
			else
			{
				WriteDouble(ptr, offset, System.Convert.ToDouble(val.ToString()));
			}
		}

		internal static void WriteString(IntPtr ptr, int offset, string val)
		{
			int num = 0;
			for (num = 0; num < val.Length; num++)
			{
				Marshal.WriteByte(ptr, offset + num, (byte)val[num]);
			}
		}

		internal static int HighDWord(long value)
		{
			int num = 0;
			long[] source = new long[2]
			{
				value,
				0L
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 1);
			num = Marshal.ReadInt32(hMemory, 4);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static uint HighDWord(ulong value)
		{
			uint num = 0u;
			long[] source = new long[2]
			{
				(long)value,
				0L
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 1);
			num = (uint)Marshal.ReadInt32(hMemory, 4);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static ushort HighWord(uint value)
		{
			ushort num = 0;
			int[] source = new int[2]
			{
				(int)value,
				0
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			num = (ushort)Marshal.ReadInt16(hMemory, 2);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static short HighWord(int value)
		{
			short num = 0;
			int[] source = new int[2]
			{
				value,
				0
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			num = Marshal.ReadInt16(hMemory, 2);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static short LowWord(int value)
		{
			short num = 0;
			int[] source = new int[2]
			{
				value,
				0
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			num = Marshal.ReadInt16(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static ushort LowWord(uint value)
		{
			ushort num = 0;
			int[] source = new int[2]
			{
				(int)value,
				0
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			num = (ushort)Marshal.ReadInt16(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static uint LowDWord(ulong value)
		{
			uint num = 0u;
			long[] source = new long[2]
			{
				(long)value,
				0L
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 1);
			num = (uint)Marshal.ReadInt32(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static int LowDWord(long value)
		{
			int num = 0;
			long[] source = new long[2]
			{
				value,
				0L
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 1);
			num = Marshal.ReadInt32(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return num;
		}

		internal static ulong ToDWord(ushort hH, ushort lH, ushort hL, ushort lL)
		{
			short[] source = new short[4]
			{
				(short)lL,
				(short)hL,
				(short)lH,
				(short)hH
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 4);
			ulong result = (ulong)ReadInt64(hMemory);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static long ToDWord(short hH, short lH, short hL, short lL)
		{
			short[] source = new short[4]
			{
				lL,
				hL,
				lH,
				hH
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 4);
			long result = ReadInt64(hMemory);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static ulong ToDWord(uint high, uint low)
		{
			int[] source = new int[2]
			{
				(int)low,
				(int)high
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 2);
			ulong result = (ulong)ReadInt64(hMemory);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static long ToDWord(int high, int low)
		{
			int[] source = new int[2]
			{
				low,
				high
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 2);
			long result = ReadInt64(hMemory);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static ushort ToUInt16(byte bH, byte bL)
		{
			int offset = 0;
			byte[] source = new byte[2]
			{
				bH,
				bL
			};
			IntPtr hMemory = AllocHGlobal(2);
			Marshal.Copy(source, 0, hMemory, 2);
			ushort result = ReadUInt16(hMemory, ref offset);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static short ToInt16(byte bH, byte bL)
		{
			int ofs = 0;
			byte[] source = new byte[2]
			{
				bH,
				bL
			};
			IntPtr hMemory = AllocHGlobal(2);
			Marshal.Copy(source, 0, hMemory, 2);
			short result = Marshal.ReadInt16(hMemory, ofs);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static uint ToUInt32(byte bHH, byte bLH, byte bHL, byte bLL)
		{
			int offset = 0;
			byte[] source = new byte[4]
			{
				bHH,
				bLH,
				bHL,
				bLL
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 4);
			uint result = ReadUInt32(hMemory, ref offset);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static int ToInt32(byte bHH, byte bLH, byte bHL, byte bLL)
		{
			byte[] source = new byte[4]
			{
				bHH,
				bLH,
				bHL,
				bLL
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 4);
			int result = Marshal.ReadInt32(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static float ToFloat32(byte bHH, byte bLH, byte bHL, byte bLL)
		{
			float[] array = new float[1];
			byte[] source = new byte[4]
			{
				bHH,
				bLH,
				bHL,
				bLL
			};
			IntPtr hMemory = AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 4);
			Marshal.Copy(hMemory, array, 0, 1);
			FreeHGlobal(ref hMemory);
			source = null;
			return array[0];
		}

		internal static ulong ToUInt64(byte bHHH, byte bLHH, byte bHLH, byte bLLH, byte bHHL, byte bLHL, byte bHLL, byte bLLL)
		{
			int offset = 0;
			byte[] source = new byte[8]
			{
				bHHH,
				bLHH,
				bHLH,
				bLLH,
				bHHL,
				bLHL,
				bHLL,
				bLLL
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 8);
			ulong result = ReadUInt64(hMemory, ref offset);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static long ToInt64(byte bHHH, byte bLHH, byte bHLH, byte bLLH, byte bHHL, byte bLHL, byte bHLL, byte bLLL)
		{
			byte[] source = new byte[8]
			{
				bHHH,
				bLHH,
				bHLH,
				bLLH,
				bHHL,
				bLHL,
				bHLL,
				bLLL
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 8);
			long result = Marshal.ReadInt64(hMemory, 0);
			FreeHGlobal(ref hMemory);
			source = null;
			return result;
		}

		internal static double ToDouble64(byte bHHH, byte bLHH, byte bHLH, byte bLLH, byte bHHL, byte bLHL, byte bHLL, byte bLLL)
		{
			double[] array = new double[1];
			byte[] source = new byte[8]
			{
				bHHH,
				bLHH,
				bHLH,
				bLLH,
				bHHL,
				bLHL,
				bHLL,
				bLLL
			};
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 8);
			Marshal.Copy(hMemory, array, 0, 1);
			FreeHGlobal(ref hMemory);
			source = null;
			return array[0];
		}

		internal static byte toByte(object value)
		{
			byte result = 0;
			long intVal = Pvi.GetIntVal(value);
			if (0 > intVal)
			{
				if (-129 < intVal)
				{
					result = (byte)(intVal + 256);
				}
			}
			else
			{
				result = byte.MaxValue;
				if (256 > intVal)
				{
					result = (byte)intVal;
				}
			}
			return result;
		}

		internal static ushort toUInt16(object value)
		{
			ushort result = 0;
			if (value is ushort)
			{
				result = (ushort)value;
			}
			else
			{
				long intVal = Pvi.GetIntVal(value);
				if (0 > intVal)
				{
					if (-32769 < intVal)
					{
						result = (ushort)(intVal + 32767);
					}
				}
				else
				{
					result = ushort.MaxValue;
					if (65536 > intVal)
					{
						result = (ushort)intVal;
					}
				}
			}
			return result;
		}

		internal static uint toUInt32(object value)
		{
			uint result = 0u;
			if (value is uint)
			{
				result = (uint)value;
			}
			else if (value is int)
			{
				result = (uint)(int)value;
			}
			else if (value is short)
			{
				result = (uint)(short)value;
			}
			else if (value is long)
			{
				result = (uint)(long)value;
			}
			else if (value is ulong)
			{
				result = (uint)(ulong)value;
			}
			else if (value is ushort)
			{
				result = (ushort)value;
			}
			else if (value is sbyte)
			{
				result = (uint)(sbyte)value;
			}
			else if (value is byte)
			{
				result = (byte)value;
			}
			else if (value is float)
			{
				result = (uint)(float)value;
			}
			else if (value is double)
			{
				result = (uint)(double)value;
			}
			else if (value is Value)
			{
				result = toUInt32(((Value)value).propObjValue);
			}
			else if (value is DateTime)
			{
				result = toUInt32(((DateTime)value).Ticks);
			}
			else if (-1 != value.ToString().IndexOf(':'))
			{
				result = toUInt32(DateTime.Parse(value.ToString()));
			}
			else
			{
				long intVal = Pvi.GetIntVal(value);
				if (0 > intVal)
				{
					if (-2147483649L < intVal)
					{
						result = (uint)(intVal + 2147483648u);
					}
				}
				else
				{
					result = uint.MaxValue;
					if (4294967296L > intVal)
					{
						result = (uint)intVal;
					}
				}
			}
			return result;
		}

		internal static sbyte toSByte(object value)
		{
			sbyte result = sbyte.MinValue;
			if (value is sbyte)
			{
				result = (sbyte)value;
			}
			else
			{
				long intVal = Pvi.GetIntVal(value);
				if (-129 < intVal)
				{
					result = ((127 >= intVal) ? ((sbyte)intVal) : sbyte.MaxValue);
				}
			}
			return result;
		}

		internal static short toInt16(object value)
		{
			short result = short.MinValue;
			if (value is short)
			{
				result = (short)value;
			}
			else
			{
				long intVal = Pvi.GetIntVal(value);
				if (-32769 < intVal)
				{
					result = ((32767 >= intVal) ? ((short)intVal) : short.MaxValue);
				}
			}
			return result;
		}

		internal static int toInt32(object value)
		{
			int result = int.MinValue;
			if (value is int)
			{
				result = (int)value;
			}
			else if (value is long)
			{
				result = (int)(long)value;
			}
			else if (value is short)
			{
				result = (short)value;
			}
			else if (value is ulong)
			{
				result = (int)(ulong)value;
			}
			else if (value is uint)
			{
				result = (int)(uint)value;
			}
			else if (value is ushort)
			{
				result = (ushort)value;
			}
			else if (value is sbyte)
			{
				result = (sbyte)value;
			}
			else if (value is byte)
			{
				result = (byte)value;
			}
			else if (value is float)
			{
				result = (int)(float)value;
			}
			else if (value is double)
			{
				result = (int)(double)value;
			}
			else if (value is Value)
			{
				result = toInt32(((Value)value).propObjValue);
			}
			else
			{
				long[] array = new long[2];
				int[] array2 = new int[2];
				array[0] = Pvi.GetIntVal(value);
				if (-2147483649L < array[0])
				{
					if (int.MaxValue < array[0])
					{
						IntPtr hMemory = AllocHGlobal(8);
						Marshal.Copy(array, 0, hMemory, 1);
						Marshal.Copy(hMemory, array2, 0, 1);
						result = array2[0];
						FreeHGlobal(ref hMemory);
					}
					else
					{
						result = (int)array[0];
					}
				}
				array = null;
				array2 = null;
			}
			return result;
		}

		internal static byte[] UInt64ToBytes(ulong u64Val)
		{
			long[] array = new long[2];
			byte[] array2 = new byte[8];
			array[0] = (long)u64Val;
			IntPtr hMemory = AllocHGlobal(8);
			Marshal.Copy(array, 0, hMemory, 1);
			Marshal.Copy(hMemory, array2, 0, 8);
			FreeHGlobal(ref hMemory);
			return array2;
		}

		internal static long toInt64(object value)
		{
			return Pvi.GetIntVal(value);
		}

		internal static ulong toUInt64(object value)
		{
			return Pvi.GetUIntVal(value);
		}

		internal static APIFC_ModulInfoRes PtrToModulInfoStructure(IntPtr ptr, Type structureType)
		{
			return (APIFC_ModulInfoRes)Marshal.PtrToStructure(ptr, structureType);
		}

		internal static APIFC_DiagModulInfoRes PtrToDiagModulInfoStructure(IntPtr ptr, Type structureType)
		{
			return (APIFC_DiagModulInfoRes)Marshal.PtrToStructure(ptr, structureType);
		}

		internal static ProgressInfo PtrToProgressInfoStructure(IntPtr ptr, Type structureType)
		{
			return (ProgressInfo)Marshal.PtrToStructure(ptr, structureType);
		}

		internal static IntPtr StringToHGlobal(string str)
		{
			if (str.Length < 0)
			{
				return IntPtr.Zero;
			}
			return Marshal.StringToHGlobalAnsi(str);
		}

		internal static IntPtr StringToHGlobalUni(string str)
		{
			if (str.Length < 0)
			{
				return IntPtr.Zero;
			}
			return Marshal.StringToHGlobalUni(str);
		}

		internal static void Copy(IntPtr ptrSource, int srcOffset, ref int[] dataDest, int destElements)
		{
			int num = 0;
			for (num = 0; num < destElements; num++)
			{
				dataDest[num] = Marshal.ReadInt32(ptrSource, srcOffset + 4 * num);
			}
		}

		internal static void Copy(IntPtr ptrSource, int srcOffset, ref byte[] dataDest, int destElements)
		{
			int num = 0;
			for (num = 0; num < destElements; num++)
			{
				dataDest[num] = Marshal.ReadByte(ptrSource, srcOffset + num);
			}
		}

		internal static void Copy(IntPtr ptrSource, int srcOffset, int destOffset, ref byte[] dataDest, int destElements)
		{
			int num = 0;
			for (num = 0; num < destElements; num++)
			{
				dataDest[destOffset + num] = Marshal.ReadByte(ptrSource, srcOffset + num);
			}
		}

		internal static void Copy(ushort ui16Src, int destOffset, ref byte[] dataDest)
		{
			short[] source = new short[2]
			{
				(short)ui16Src,
				0
			};
			IntPtr intPtr = Marshal.AllocHGlobal(2);
			Marshal.Copy(source, 0, intPtr, 1);
			Copy(intPtr, 0, destOffset, ref dataDest, 2);
			Marshal.FreeHGlobal(intPtr);
		}

		internal static void Copy(uint ui32Src, int destOffset, ref byte[] dataDest)
		{
			int[] source = new int[2]
			{
				(int)ui32Src,
				0
			};
			IntPtr intPtr = Marshal.AllocHGlobal(4);
			Marshal.Copy(source, 0, intPtr, 1);
			Copy(intPtr, 0, destOffset, ref dataDest, 4);
			Marshal.FreeHGlobal(intPtr);
		}

		internal static void Copy(uint ui32Src, IntPtr dataDest)
		{
			if (8 == IntPtr.Size)
			{
				long[] source = new long[2]
				{
					ui32Src,
					0L
				};
				dataDest = Marshal.AllocHGlobal(8);
				Marshal.Copy(source, 0, dataDest, 1);
			}
			else
			{
				int[] source2 = new int[2]
				{
					(int)ui32Src,
					0
				};
				dataDest = Marshal.AllocHGlobal(4);
				Marshal.Copy(source2, 0, dataDest, 1);
			}
		}

		internal static void Copy(IntPtr dataSrc, ref uint ui32Dest)
		{
			if (8 == IntPtr.Size)
			{
				long[] array = new long[2];
				Marshal.Copy(dataSrc, array, 0, 1);
				ui32Dest = (uint)array[0];
			}
			else
			{
				int[] array2 = new int[2];
				Marshal.Copy(dataSrc, array2, 0, 1);
				ui32Dest = (uint)array2[0];
			}
		}

		internal static uint TimeToUInt32(string plcTimeCode)
		{
			uint num = 0u;
			int num2 = 0;
			byte b = 0;
			byte b2 = 0;
			byte b3 = 0;
			byte b4 = 0;
			byte b5 = 0;
			string[] array = plcTimeCode.Split('-');
			DateTime dateTime = new DateTime(1900, 1, 1);
			b = System.Convert.ToByte(array[0], 16);
			b2 = System.Convert.ToByte(array[1], 16);
			b3 = System.Convert.ToByte(array[2], 16);
			b4 = System.Convert.ToByte(array[3], 16);
			b5 = System.Convert.ToByte(array[4], 16);
			num2 = b >> 1;
			dateTime = dateTime.AddYears(num2);
			num2 = ((b & 1) << 3) + (b2 >> 5);
			dateTime = dateTime.AddMonths(num2 - 1);
			num2 = (b2 & 0x1F);
			dateTime = dateTime.AddDays(num2 - 1);
			num2 = b3 >> 3;
			dateTime = dateTime.AddHours(num2);
			num2 = ((b3 & 7) << 3) + (b4 >> 5);
			dateTime = dateTime.AddMinutes(num2);
			num2 = ((b4 & 0x1F) << 1) + (b5 >> 7);
			dateTime = dateTime.AddSeconds(num2);
			num2 = (b5 & 0x7F);
			dateTime = dateTime.AddMilliseconds(num2);
			return Pvi.DateTimeToUInt32(dateTime);
		}

		internal static int ToInt32(string strValue)
		{
			int num = 0;
			if (-1 != strValue.ToLower().IndexOf("0x"))
			{
				return System.Convert.ToInt32(strValue, 16);
			}
			return System.Convert.ToInt32(strValue);
		}

		internal static uint ToUInt32(string strValue)
		{
			uint num = 0u;
			if (-1 != strValue.ToLower().IndexOf("0x"))
			{
				return System.Convert.ToUInt32(strValue, 16);
			}
			return System.Convert.ToUInt32(strValue);
		}

		internal static byte ToByte(string strValue)
		{
			byte b = 0;
			if (-1 != strValue.ToLower().IndexOf("0x"))
			{
				return System.Convert.ToByte(strValue, 16);
			}
			return System.Convert.ToByte(strValue);
		}

		internal static bool HexCharToByte(char hexChar, ref byte byteVal)
		{
			byteVal = Convert.ToByte(hexChar);
			switch (hexChar)
			{
			case '0':
				byteVal = 0;
				break;
			case '1':
				byteVal = 1;
				break;
			case '2':
				byteVal = 2;
				break;
			case '3':
				byteVal = 3;
				break;
			case '4':
				byteVal = 4;
				break;
			case '5':
				byteVal = 5;
				break;
			case '6':
				byteVal = 6;
				break;
			case '7':
				byteVal = 7;
				break;
			case '8':
				byteVal = 8;
				break;
			case '9':
				byteVal = 9;
				break;
			case 'A':
				byteVal = 10;
				break;
			case 'B':
				byteVal = 11;
				break;
			case 'C':
				byteVal = 12;
				break;
			case 'D':
				byteVal = 13;
				break;
			case 'E':
				byteVal = 14;
				break;
			case 'F':
				byteVal = 15;
				break;
			default:
				return false;
			}
			return true;
		}
	}
}
