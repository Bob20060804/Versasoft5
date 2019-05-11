using System;

namespace BR.AN.PviServices
{
	public class TracePointData : IDisposable
	{
		private bool disposed;

		private IECDataTypes propPVFormat;

		private uint propPVLength;

		private byte[] propDataBytes;

		public IECDataTypes IECType => propPVFormat;

		[CLSCompliant(false)]
		public uint TypeLength
		{
			get
			{
				return propPVLength;
			}
		}

		public byte[] DataBytes => propDataBytes;

		private void InitMembers()
		{
			propPVFormat = IECDataTypes.UNDEFINED;
			propPVLength = 0u;
			propDataBytes = null;
		}

		public TracePointData()
		{
			InitMembers();
		}

		[CLSCompliant(false)]
		public TracePointData(uint iecFormat, uint typeLenght, IntPtr pData, uint dataLen, ref int dataOffset)
		{
			int num = 0;
			InitMembers();
			propPVFormat = (IECDataTypes)iecFormat;
			propPVLength = typeLenght;
			propDataBytes = new byte[typeLenght];
			for (num = 0; num < (int)typeLenght; num++)
			{
				propDataBytes[num] = PviMarshal.ReadByte(pData, ref dataOffset);
			}
		}

		internal void UpdateFormat(uint formatType, uint typeLength)
		{
			propPVFormat = (IECDataTypes)formatType;
			propPVLength = typeLength;
		}

		internal void UpdateData(IECDataTypes formatType, uint typeLength, byte[] dataBytes)
		{
			int num = 0;
			propPVFormat = formatType;
			propPVLength = typeLength;
			propDataBytes = new byte[typeLength];
			for (num = 0; num < (int)typeLength; num++)
			{
				propDataBytes[num] = (byte)dataBytes.GetValue(num);
			}
		}

		public object DataTo(TypeCode conversionType)
		{
			object obj = null;
			switch (conversionType)
			{
			case TypeCode.Byte:
				return propDataBytes[0];
			case TypeCode.SByte:
			{
				sbyte b = (sbyte)propDataBytes[0];
				return b;
			}
			case TypeCode.UInt16:
				return (ushort)0;
			case TypeCode.Int16:
				return (short)0;
			case TypeCode.Int32:
				return 0;
			case TypeCode.UInt32:
				return 0u;
			case TypeCode.Char:
				return '\0';
			case TypeCode.DateTime:
				return default(DateTime);
			case TypeCode.Double:
				return 0.0;
			case TypeCode.Single:
				return 0f;
			case TypeCode.UInt64:
				return 0uL;
			case TypeCode.Int64:
				return 0L;
			default:
				throw new ArgumentOutOfRangeException("\"" + conversionType.ToString() + "\" is NOT supported!");
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					propDataBytes = null;
				}
				disposed = true;
			}
		}

		~TracePointData()
		{
			Dispose(disposing: false);
		}

		public override string ToString()
		{
			string text = "0x";
			int num = 0;
			if (propDataBytes != null)
			{
				for (num = 0; num < propDataBytes.GetLength(0); num++)
				{
					text += $"{Convert.ToInt32(propDataBytes.GetValue(num).ToString()):X2}";
				}
			}
			return "Format=\"" + propPVFormat.ToString() + "\" Length=\"" + propPVLength + "\" Data=\"" + text + "\"";
		}
	}
}
