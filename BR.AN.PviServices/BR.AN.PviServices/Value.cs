using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public class Value : IDisposable, IConvertible, IFormattable
	{
		internal bool isAssigned;

		private int propByteOffset;

		internal uint propUInt32Val;

		internal bool propHasOwnDataPtr;

		private bool propTypePreset;

		internal sbyte propIsEnum;

		internal DerivationBase propDerivedFrom;

		internal sbyte propIsDerived;

		internal sbyte propIsBitString;

		internal EnumArray propEnumerations;

		internal bool propArryOne;

		internal ArrayDimensionArray propDimensions;

		internal object propObjValue;

		internal byte[] propByteField;

		internal Variable propParent;

		internal DataType propDataType;

		internal int propArrayMinIndex;

		internal int propArrayMaxIndex;

		internal int propTypeLength;

		internal int propArrayLength;

		internal bool propDisposed;

		internal IntPtr pData;

		internal int propDataSize;

		private int BinaryDataLen => propArrayLength * propTypeLength;

		public bool IsPG2000String
		{
			get
			{
				if (propParent != null && CastModes.PG2000String == (propParent.CastMode & CastModes.PG2000String) && 1 < propArrayLength)
				{
					return true;
				}
				return false;
			}
		}

		internal bool TypePreset
		{
			get
			{
				return propTypePreset;
			}
			set
			{
				propTypePreset = value;
			}
		}

		public int DataSize => propArrayLength * propTypeLength;

		public bool IsOfTypeArray
		{
			get
			{
				if (propArryOne || 1 < propArrayLength)
				{
					return true;
				}
				return false;
			}
		}

		public int ArrayMinIndex => propArrayMinIndex;

		public int ArrayMaxIndex => propArrayMaxIndex;

		public DataType DataType
		{
			get
			{
				return propDataType;
			}
			set
			{
				PresetDataType(value);
			}
		}

		public IECDataTypes IECDataType
		{
			get
			{
				switch (propDataType)
				{
				case DataType.Boolean:
					return IECDataTypes.BOOL;
				case DataType.UInt8:
					return IECDataTypes.USINT;
				case DataType.Byte:
					return IECDataTypes.BYTE;
				case DataType.DateTime:
					return IECDataTypes.DATE_AND_TIME;
				case DataType.Date:
					return IECDataTypes.DATE;
				case DataType.DT:
					return IECDataTypes.DT;
				case DataType.Double:
					return IECDataTypes.LREAL;
				case DataType.Int16:
					return IECDataTypes.INT;
				case DataType.Int32:
					return IECDataTypes.DINT;
				case DataType.Int64:
					return IECDataTypes.LINT;
				case DataType.SByte:
					return IECDataTypes.SINT;
				case DataType.Single:
					return IECDataTypes.REAL;
				case DataType.String:
					return IECDataTypes.STRING;
				case DataType.Structure:
					return IECDataTypes.STRUCT;
				case DataType.TimeSpan:
					return IECDataTypes.TIME;
				case DataType.TimeOfDay:
					return IECDataTypes.TIME_OF_DAY;
				case DataType.TOD:
					return IECDataTypes.TOD;
				case DataType.UInt16:
					return IECDataTypes.UINT;
				case DataType.WORD:
					return IECDataTypes.WORD;
				case DataType.UInt32:
					return IECDataTypes.UDINT;
				case DataType.DWORD:
					return IECDataTypes.DWORD;
				case DataType.UInt64:
					return IECDataTypes.ULINT;
				case DataType.LWORD:
					return IECDataTypes.LWORD;
				case DataType.WString:
					return IECDataTypes.WSTRING;
				default:
					return IECDataTypes.UNDEFINED;
				}
			}
		}

		public TypeCode SystemDataType
		{
			get
			{
				switch (propDataType)
				{
				case DataType.Boolean:
					return TypeCode.Boolean;
				case DataType.UInt8:
					return TypeCode.Byte;
				case DataType.Byte:
					return TypeCode.Byte;
				case DataType.DT:
					return TypeCode.DateTime;
				case DataType.DateTime:
					return TypeCode.DateTime;
				case DataType.Date:
					return TypeCode.DateTime;
				case DataType.Double:
					return TypeCode.Double;
				case DataType.Int16:
					return TypeCode.Int16;
				case DataType.Int32:
					return TypeCode.Int32;
				case DataType.Int64:
					return TypeCode.Int64;
				case DataType.SByte:
					return TypeCode.SByte;
				case DataType.Single:
					return TypeCode.Single;
				case DataType.String:
					return TypeCode.String;
				case DataType.Structure:
					return TypeCode.Object;
				case DataType.TimeSpan:
					return TypeCode.DateTime;
				case DataType.TOD:
					return TypeCode.DateTime;
				case DataType.TimeOfDay:
					return TypeCode.DateTime;
				case DataType.UInt16:
					return TypeCode.UInt16;
				case DataType.WORD:
					return TypeCode.UInt16;
				case DataType.UInt32:
					return TypeCode.UInt32;
				case DataType.DWORD:
					return TypeCode.UInt32;
				case DataType.UInt64:
					return TypeCode.UInt64;
				default:
					return TypeCode.Empty;
				}
			}
		}

		public int TypeLength
		{
			get
			{
				return propTypeLength;
			}
			set
			{
				propTypeLength = value;
			}
		}

		public int ArrayLength => propArrayLength;

		public ArrayDimensionArray ArrayDimensions => propDimensions;

		public sbyte IsEnum => propIsEnum;

		public DerivationBase DerivedFrom => propDerivedFrom;

		public sbyte IsDerived => propIsDerived;

		public sbyte IsBitString => propIsBitString;

		public EnumArray Enumerations => propEnumerations;

		public Array ArrayData
		{
			get
			{
				if (propParent != null && propParent.PVRoot.propPviValue.propByteField != null)
				{
					return ToSystemDataTypeArray(propParent.PVRoot.propPviValue.propByteField, propParent.propOffset);
				}
				return ToSystemDataTypeArray(propByteField, 0);
			}
			set
			{
				propParent.WriteValue(value, propParent.propOffset);
			}
		}

		internal IntPtr DataPtr => pData;

		public Value this[string varName]
		{
			get
			{
				return propParent.GetStructureMemberValue(varName);
			}
			set
			{
				propParent.Value[varName].Assign(value.propObjValue);
				if (propParent.WriteValueAutomatic)
				{
					propParent.WriteValue();
				}
			}
		}

		public Value this[params int[] indices]
		{
			get
			{
				return GetFromByteBuffer(indices);
			}
			set
			{
				propParent.SetStructureMemberValue(value, indices);
			}
		}

		public Value this[int index]
		{
			get
			{
				return GetFromByteBuffer(index);
			}
			set
			{
				propParent.SetStructureMemberValue(value, index);
			}
		}

		public Variable Parent
		{
			get
			{
				return propParent;
			}
			set
			{
				propParent = value;
			}
		}

		public event DisposeEventHandler Disposing;

		private void InitMembers(Variable parentVar, IntPtr dataPtr, DataType type, int typeLen, int arrayLen)
		{
			InitMembers(parentVar, dataPtr, type, typeLen);
			propByteOffset = 0;
			propArrayLength = arrayLen;
			propDataSize = typeLen * arrayLen;
			propUInt32Val = 0u;
		}

		private void InitMembers(Variable parentVar, IntPtr dataPtr, DataType type, int typeLen)
		{
			propDisposed = false;
			propArryOne = false;
			propDimensions = null;
			propIsEnum = -1;
			propIsDerived = -1;
			propDerivedFrom = null;
			propIsBitString = -1;
			propEnumerations = null;
			propArrayLength = 1;
			propByteField = null;
			propDataSize = typeLen;
			propDataType = type;
			isAssigned = false;
			propParent = parentVar;
			propTypeLength = typeLen;
			pData = dataPtr;
		}

		private void Initialize(Variable parentVar, object val, DataType type, int typeLen, int arrayLen)
		{
			propHasOwnDataPtr = true;
			InitMembers(parentVar, IntPtr.Zero, type, typeLen, arrayLen);
			propObjValue = val;
			if (type != 0)
			{
				pData = PviMarshal.AllocHGlobal(DataSize);
			}
			Assign(val);
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
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
				}
				if (propHasOwnDataPtr && IntPtr.Zero != pData)
				{
					PviMarshal.FreeHGlobal(ref pData);
				}
				pData = IntPtr.Zero;
				propByteField = null;
				propObjValue = null;
				propParent = null;
			}
		}

		~Value()
		{
			Dispose(disposing: false);
		}

		internal Value()
		{
			Initialize(null, null, DataType.Unknown, 0, 1);
		}

		internal Value(Value parentVal, int[] indices)
		{
			int num = 0;
			propHasOwnDataPtr = false;
			num = (propByteOffset = Variable.CalculateByteOffset(indices, parentVal));
			bool flag = true;
			Service service;
			if (parentVal.propParent != null)
			{
				service = parentVal.propParent.Service;
				flag = false;
			}
			else
			{
				service = new Service();
			}
			switch (parentVal.propDataType)
			{
			case DataType.Boolean:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Boolean, parentVal.propTypeLength);
				if (parentVal.propByteField[num] == 0)
				{
					propObjValue = false;
				}
				else
				{
					propObjValue = true;
				}
				break;
			case DataType.UInt8:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt8, parentVal.propTypeLength);
				propObjValue = parentVal.propByteField[num];
				break;
			case DataType.Byte:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Byte, parentVal.propTypeLength);
				propObjValue = parentVal.propByteField[num];
				break;
			case DataType.SByte:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.SByte, parentVal.propTypeLength);
				propObjValue = (sbyte)parentVal.propByteField[num];
				break;
			case DataType.Int16:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int16, parentVal.propTypeLength);
				propObjValue = service.toInt16(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt16, parentVal.propTypeLength);
				propObjValue = service.toUInt16(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Int32:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int32, parentVal.propTypeLength);
				propObjValue = service.toInt32(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt32, parentVal.propTypeLength);
				propObjValue = service.toUInt32(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Int64:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int64, parentVal.propTypeLength);
				propObjValue = service.toInt64(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt64:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt64, parentVal.propTypeLength);
				propObjValue = service.toUInt64(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Single:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Single, parentVal.propTypeLength);
				propObjValue = service.toSingle(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Double:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Double, parentVal.propTypeLength);
				propObjValue = service.toDouble(parentVal.propByteField, propByteOffset);
				break;
			case DataType.TimeSpan:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeSpan, parentVal.propTypeLength);
				propObjValue = service.toTimeSpan(parentVal.propByteField, propByteOffset);
				break;
			case DataType.DateTime:
			case DataType.DT:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.DateTime, parentVal.propTypeLength);
				propObjValue = service.toDateTime(parentVal.propByteField, propByteOffset);
				break;
			case DataType.TimeOfDay:
			case DataType.TOD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeOfDay, parentVal.propTypeLength);
				propObjValue = service.toTimeSpan(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Date:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Date, parentVal.propTypeLength);
				propObjValue = service.toDateTime(parentVal.propByteField, propByteOffset);
				break;
			case DataType.String:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.String, parentVal.propTypeLength);
				propObjValue = service.toString(parentVal.propByteField, propByteOffset, parentVal.propTypeLength);
				break;
			case DataType.WString:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.WString, parentVal.propTypeLength);
				propObjValue = PviMarshal.ToWString(parentVal.propByteField, propByteOffset, parentVal.propTypeLength);
				break;
			default:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Unknown, 0);
				break;
			}
			if (flag)
			{
				service.Dispose();
				service = null;
			}
		}

		internal Value(Value parentVal, int arrayIndex)
		{
			propHasOwnDataPtr = false;
			propByteOffset = arrayIndex * parentVal.propTypeLength;
			Service service = (parentVal.propParent == null) ? new Service() : parentVal.propParent.Service;
			switch (parentVal.propDataType)
			{
			case DataType.Boolean:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Boolean, parentVal.propTypeLength);
				if (parentVal.propByteField[arrayIndex] == 0)
				{
					propObjValue = false;
				}
				else
				{
					propObjValue = true;
				}
				break;
			case DataType.UInt8:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt8, parentVal.propTypeLength);
				propObjValue = parentVal.propByteField[arrayIndex];
				break;
			case DataType.Byte:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Byte, parentVal.propTypeLength);
				propObjValue = parentVal.propByteField[arrayIndex];
				break;
			case DataType.SByte:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.SByte, parentVal.propTypeLength);
				propObjValue = (sbyte)parentVal.propByteField[arrayIndex];
				break;
			case DataType.Int16:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int16, parentVal.propTypeLength);
				propObjValue = service.toInt16(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt16, parentVal.propTypeLength);
				propObjValue = service.toUInt16(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Int32:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int32, parentVal.propTypeLength);
				propObjValue = service.toInt32(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt32, parentVal.propTypeLength);
				propObjValue = service.toUInt32(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Int64:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Int64, parentVal.propTypeLength);
				propObjValue = service.toInt64(parentVal.propByteField, propByteOffset);
				break;
			case DataType.UInt64:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt64, parentVal.propTypeLength);
				propObjValue = service.toUInt64(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Single:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Single, parentVal.propTypeLength);
				propObjValue = service.toSingle(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Double:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Double, parentVal.propTypeLength);
				propObjValue = service.toDouble(parentVal.propByteField, propByteOffset);
				break;
			case DataType.TimeSpan:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeSpan, parentVal.propTypeLength);
				propObjValue = service.toTimeSpan(parentVal.propByteField, propByteOffset);
				break;
			case DataType.DateTime:
			case DataType.DT:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.DateTime, parentVal.propTypeLength);
				propObjValue = service.toDateTime(parentVal.propByteField, propByteOffset);
				break;
			case DataType.TimeOfDay:
			case DataType.TOD:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeOfDay, parentVal.propTypeLength);
				propObjValue = service.toTimeSpan(parentVal.propByteField, propByteOffset);
				break;
			case DataType.Date:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Date, parentVal.propTypeLength);
				propObjValue = service.toDateTime(parentVal.propByteField, propByteOffset);
				break;
			case DataType.String:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.String, parentVal.propTypeLength);
				propObjValue = service.toString(parentVal.propByteField, propByteOffset, parentVal.propTypeLength);
				break;
			case DataType.WString:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.WString, parentVal.propTypeLength);
				propObjValue = PviMarshal.ToWString(parentVal.propByteField, propByteOffset, parentVal.propTypeLength);
				break;
			default:
				InitMembers(parentVal.propParent, parentVal.pData, DataType.Unknown, 0);
				break;
			}
		}

		public Value(bool value)
		{
			Initialize(null, value, DataType.Boolean, 1, 1);
		}

		internal Value(bool value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Boolean, 1, 1);
		}

		public Value(bool[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(bool[] value)
		{
			Initialize(null, value, DataType.Boolean, 1, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				bool* ptr2 = (bool*)ptr;
				for (int i = 0; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(sbyte value)
		{
			Initialize(null, value, DataType.SByte, 0, 1);
		}

		internal Value(sbyte value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.SByte, 0, 1);
		}

		public Value(sbyte[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(sbyte[] value)
		{
			Initialize(null, value, DataType.SByte, 1, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				sbyte* ptr2 = (sbyte*)ptr;
				for (int i = 0; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(short value)
		{
			Initialize(null, value, DataType.Int16, 2, 1);
		}

		internal Value(short value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Int16, 2, 1);
		}

		public Value(short[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(short[] value)
		{
			Initialize(null, value, DataType.Int16, 2, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				short* ptr2 = (short*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(int value)
		{
			Initialize(null, value, DataType.Int32, 4, 1);
		}

		internal Value(int value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Int32, 4, 1);
		}

		public Value(int[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(int[] value)
		{
			Initialize(null, value, DataType.Int32, 4, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				int* ptr2 = (int*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		internal unsafe void SetByteField(long[] value)
		{
			Initialize(null, value, DataType.Int64, 8, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				long* ptr2 = (long*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		internal Value(byte value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Byte, 1, 1);
		}

		public Value(byte value)
		{
			Initialize(null, value, DataType.Byte, 1, 1);
		}

		public Value(byte[] value)
		{
			isAssigned = false;
			SetByteField(value);
		}

		internal unsafe void SetByteField(byte[] value)
		{
			Initialize(null, value, DataType.Byte, 1, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				*ptr = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr[i] = value[i];
				}
			}
		}

		public Value(ushort value)
		{
			Initialize(null, value, DataType.UInt16, 2, 1);
		}

		internal Value(ushort value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.UInt16, 2, 1);
		}

		public Value(ushort[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(ushort[] value)
		{
			Initialize(null, value, DataType.UInt16, 2, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				ushort* ptr2 = (ushort*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(uint value)
		{
			Initialize(null, value, DataType.UInt32, 4, 1);
		}

		internal Value(uint value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.UInt32, 4, 1);
		}

		public Value(uint[] value)
		{
			SetByteField(value);
		}

		public Value(ulong value)
		{
			Initialize(null, value, DataType.UInt64, 8, 1);
		}

		internal Value(ulong value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.UInt64, 8, 1);
		}

		public Value(ulong[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(uint[] value)
		{
			Initialize(null, value, DataType.UInt32, 4, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				uint* ptr2 = (uint*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		internal unsafe void SetByteField(ulong[] value)
		{
			Initialize(null, value, DataType.UInt64, 8, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				ulong* ptr2 = (ulong*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(float value)
		{
			Initialize(null, value, DataType.Single, 4, 1);
		}

		internal Value(float value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Single, 4, 1);
		}

		public Value(float[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(float[] value)
		{
			Initialize(null, value, DataType.Single, 4, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				float* ptr2 = (float*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(double value)
		{
			Initialize(null, value, DataType.Double, 8, 1);
		}

		internal Value(double value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Double, 8, 1);
		}

		public Value(double[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(double[] value)
		{
			Initialize(null, value, DataType.Double, 8, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				double* ptr2 = (double*)ptr;
				*ptr2 = value[0];
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = value[i];
				}
			}
		}

		public Value(TimeSpan value)
		{
			Initialize(null, value, DataType.TimeSpan, 4, 1);
		}

		internal Value(TimeSpan value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.TimeSpan, 4, 1);
		}

		public Value(TimeSpan[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(TimeSpan[] value)
		{
			Initialize(null, value, DataType.TimeSpan, 4, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				uint* ptr2 = (uint*)ptr;
				*ptr2 = (uint)value[0].Ticks / 10000u;
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = (uint)value[i].Ticks / 10000u;
				}
			}
		}

		public Value(DateTime value)
		{
			Initialize(null, value, DataType.DateTime, 4, 1);
		}

		internal Value(DateTime value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.DateTime, 4, 1);
		}

		public Value(DateTime[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(DateTime[] value)
		{
			Initialize(null, value, DataType.DateTime, 4, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				uint* ptr2 = (uint*)ptr;
				*ptr2 = (uint)value[0].Ticks / 10000u;
				for (int i = 1; i < propArrayLength; i++)
				{
					ptr2[i] = (uint)value[i].Ticks / 10000u;
				}
			}
		}

		public Value(string value)
		{
			Initialize(null, value, DataType.String, value.Length, 1);
		}

		internal Value(string value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.String, value.Length, 1);
		}

		public Value(string[] value)
		{
			SetByteField(value);
		}

		internal unsafe void SetByteField(string[] value)
		{
			Initialize(null, value, DataType.String, value[0].Length, value.Length);
			propByteField = new byte[propArrayLength * propTypeLength];
			fixed (byte* ptr = &propByteField[0])
			{
				string text = value[0];
				byte* ptr2 = ptr + propTypeLength;
				*ptr2 = Convert.ToByte(text[0]);
				for (int i = 1; i < propTypeLength - 1; i++)
				{
					ptr2[i] = Convert.ToByte(text[i]);
				}
				*(ptr2 + propTypeLength - 1) = 0;
				for (int j = 1; j < propArrayLength; j++)
				{
					text = value[j];
					ptr2 = ptr + (long)j * (long)propTypeLength;
					*ptr2 = Convert.ToByte(text[0]);
					for (int k = 1; k < propTypeLength - 1; k++)
					{
						ptr2[k] = Convert.ToByte(text[k]);
					}
					*(ptr2 + propTypeLength - 1) = 0;
				}
			}
		}

		public Value(object value)
		{
			Initialize(null, value, DataType.Unknown, 0, 1);
			Assign(value);
		}

		internal Value(object value, Variable parentVar)
		{
			Initialize(parentVar, value, DataType.Unknown, 0, 1);
			Assign(value);
		}

		private bool IsBoolTrue(object value)
		{
			if (value == null)
			{
				return false;
			}
			string text = value.ToString();
			if (text.Length == 0)
			{
				return false;
			}
			string text2 = text.ToLower();
			if (text2.CompareTo("false") == 0 || text2.CompareTo("0") == 0)
			{
				return false;
			}
			return true;
		}

		private void InitAssingOneElement()
		{
			if (DataType.Structure != DataType && (Parent == null || !(null != Parent.Value)) && (propByteField == null || propByteField.Length != propArrayLength * propTypeLength))
			{
				propByteField = new byte[propArrayLength * propTypeLength];
			}
		}

		private bool AssingOneElement(int iCnt, object valueObj)
		{
			if (DataType.Structure == DataType)
			{
				if (Parent.StructureMembers != null && iCnt < Parent.StructureMembers.Count)
				{
					string text = "";
					object obj = Parent.StructureMembers[iCnt];
					if (obj is Variable)
					{
						if (((Variable)obj).Value.IsOfTypeArray)
						{
							return false;
						}
						if (((Variable)obj).Value.DataType == DataType.Structure)
						{
							return false;
						}
						text = ((Variable)obj).StructMemberName;
					}
					else
					{
						text = obj.ToString();
					}
					Parent.Value[text].Assign(valueObj);
				}
			}
			else if (Parent != null && null != Parent.Value)
			{
				if (iCnt < Parent.Value.ArrayLength)
				{
					Parent.Value[iCnt].Assign(valueObj);
				}
			}
			else
			{
				Assign(valueObj, iCnt);
			}
			return true;
		}

		private bool AssignArrayToArray(object values)
		{
			if (!(values is Array))
			{
				return false;
			}
			InitAssingOneElement();
			int i = 0;
			for (int j = 0; j < ((Array)values).Length; j++)
			{
				for (; !AssingOneElement(i, ((Array)values).GetValue(j)); i++)
				{
				}
				i++;
			}
			return true;
		}

		private bool AssignStringToArray(object values)
		{
			if (!(values is string))
			{
				return false;
			}
			string text = values.ToString();
			string[] array = text.Split(';');
			InitAssingOneElement();
			int i = 0;
			for (int j = 0; j < array.GetLength(0); j++)
			{
				for (; !AssingOneElement(i, array.GetValue(j).ToString()); i++)
				{
				}
				i++;
			}
			return true;
		}

		private void AssignArrayValues(object values)
		{
			if (DataType == DataType.Unknown)
			{
				isAssigned = false;
			}
			else
			{
				if (AssignArrayToArray(values) || AssignStringToArray(values))
				{
					return;
				}
				if (DataType.Structure == DataType)
				{
					if (Parent.StructureMembers != null)
					{
						Parent.Value[Parent.StructureMembers[0].ToString()].Assign(values);
					}
					return;
				}
				if (Parent != null && null != Parent.Value)
				{
					Parent.Value[0].Assign(values);
					return;
				}
				if (propByteField == null || propByteField.Length != propArrayLength * propTypeLength)
				{
					propByteField = new byte[propArrayLength * propTypeLength];
				}
				this[0].Assign(values);
			}
		}

		private void AssignStrArray(string[] values)
		{
			if (DataType == DataType.Unknown)
			{
				isAssigned = false;
				return;
			}
			if (DataType.Structure == DataType)
			{
				if (Parent.StructureMembers != null)
				{
					for (int i = 0; i < values.Length; i++)
					{
						Parent.Value[Parent.StructureMembers[i].ToString()].Assign(values[i]);
					}
				}
				return;
			}
			if (Parent != null && null != Parent.Value)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Parent.Value[i].Assign(values[i]);
				}
				return;
			}
			if (propByteField == null || propByteField.Length != propArrayLength * propTypeLength)
			{
				propByteField = new byte[propArrayLength * propTypeLength];
			}
			for (int i = 0; i < values.Length; i++)
			{
				this[i].Assign(values[i]);
			}
		}

		private void SetPG2000StringData(string strVal)
		{
			if (BinaryDataLen < strVal.Length)
			{
				strVal = strVal.Substring(0, BinaryDataLen);
				Parent.propSendChangedEvent = true;
			}
			if (propParent != null)
			{
				propParent.ResizePviDataPtr(BinaryDataLen);
			}
			if (IntPtr.Zero == pData)
			{
				pData = PviMarshal.AllocHGlobal(BinaryDataLen);
				propHasOwnDataPtr = true;
			}
			for (int i = 0; i < propDataSize; i++)
			{
				if (i < strVal.Length)
				{
					Marshal.WriteByte(pData, i, (byte)strVal[i]);
				}
				else
				{
					Marshal.WriteByte(pData, i, 0);
				}
			}
		}

		private bool CheckBuffers(ulong uiValue)
		{
			if (propParent != null)
			{
				propParent.Value.isAssigned = true;
			}
			isAssigned = true;
			if (1 < propArrayLength)
			{
				if (IsPG2000String)
				{
					SetPG2000StringData(uiValue.ToString());
				}
				else
				{
					AssignArrayValues(uiValue);
				}
				return false;
			}
			if (IntPtr.Zero == pData)
			{
				if (DataSize == 0)
				{
					if (propParent != null)
					{
						propParent.Value.isAssigned = false;
					}
					isAssigned = false;
					return false;
				}
				pData = PviMarshal.AllocHGlobal(DataSize);
				propHasOwnDataPtr = true;
			}
			if (IsPG2000String)
			{
				SetPG2000StringData(uiValue.ToString());
				return false;
			}
			if (IntPtr.Zero == pData)
			{
				return false;
			}
			return true;
		}

		private void AssignValue(float value)
		{
			switch (DataType)
			{
			case DataType.Single:
				propDataSize = Marshal.SizeOf(typeof(float));
				if (propParent != null)
				{
					propParent.Service.cpyFltToBuffer(value);
					for (int j = 0; j < 4; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, propParent.Service.ByteBuffer[j]);
					}
				}
				else
				{
					Marshal.Copy(new float[1]
					{
						Convert.ToSingle(value)
					}, 0, pData, 1);
				}
				break;
			case DataType.Double:
				propDataSize = Marshal.SizeOf(typeof(double));
				if (propParent != null)
				{
					propParent.Service.cpyDblToBuffer(value);
					for (int i = 0; i < 8; i++)
					{
						Marshal.WriteByte(pData, propByteOffset + i, propParent.Service.ByteBuffer[i]);
					}
				}
				else
				{
					Marshal.Copy(new double[1]
					{
						Convert.ToDouble(value)
					}, 0, pData, 1);
				}
				break;
			default:
			{
				ulong value2 = Convert.ToUInt64(value);
				AssignValue(value2);
				break;
			}
			}
		}

		private void AssignValue(double value)
		{
			switch (DataType)
			{
			case DataType.Single:
				propDataSize = Marshal.SizeOf(typeof(float));
				if (propParent != null)
				{
					propParent.Service.cpyFltToBuffer(value);
					for (int j = 0; j < 4; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, propParent.Service.ByteBuffer[j]);
					}
				}
				else
				{
					Marshal.Copy(new float[1]
					{
						Convert.ToSingle(value)
					}, 0, pData, 1);
				}
				break;
			case DataType.Double:
				propDataSize = Marshal.SizeOf(typeof(double));
				if (propParent != null)
				{
					propParent.Service.cpyDblToBuffer(value);
					for (int i = 0; i < 8; i++)
					{
						Marshal.WriteByte(pData, propByteOffset + i, propParent.Service.ByteBuffer[i]);
					}
				}
				else
				{
					Marshal.Copy(new double[1]
					{
						Convert.ToDouble(value)
					}, 0, pData, 1);
				}
				break;
			default:
			{
				ulong value2 = Convert.ToUInt64(value);
				AssignValue(value2);
				break;
			}
			}
		}

		private void AssignValue(ulong value)
		{
			switch (DataType)
			{
			case DataType.Boolean:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				if (0 == value)
				{
					Marshal.WriteByte(pData, propByteOffset, 0);
				}
				else
				{
					Marshal.WriteByte(pData, propByteOffset, 1);
				}
				break;
			case DataType.SByte:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				PviMarshal.WriteSByte(pData, propByteOffset, (sbyte)value);
				break;
			case DataType.Int16:
				propDataSize = Marshal.SizeOf(typeof(short));
				Marshal.WriteInt16(pData, propByteOffset, (short)value);
				break;
			case DataType.Int32:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, propByteOffset, (int)value);
				break;
			case DataType.Int64:
				propDataSize = Marshal.SizeOf(typeof(long));
				PviMarshal.WriteInt64(pData, propByteOffset, (long)value);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				propDataSize = Marshal.SizeOf(typeof(byte));
				Marshal.WriteByte(pData, propByteOffset, (byte)value);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				propDataSize = Marshal.SizeOf(typeof(ushort));
				PviMarshal.WriteUInt16(pData, propByteOffset, (ushort)value);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				propDataSize = Marshal.SizeOf(typeof(uint));
				PviMarshal.WriteUInt32(pData, propByteOffset, (uint)value);
				break;
			case DataType.UInt64:
				propDataSize = Marshal.SizeOf(typeof(ulong));
				PviMarshal.WriteUInt64(pData, propByteOffset, value);
				break;
			case DataType.Single:
				propDataSize = Marshal.SizeOf(typeof(float));
				if (propParent != null)
				{
					propParent.Service.cpyFltToBuffer(value);
					for (int l = 0; l < 4; l++)
					{
						Marshal.WriteByte(pData, propByteOffset + l, propParent.Service.ByteBuffer[l]);
					}
				}
				else
				{
					Marshal.Copy(new float[1]
					{
						Convert.ToSingle(value)
					}, 0, pData, 1);
				}
				break;
			case DataType.Double:
				propDataSize = Marshal.SizeOf(typeof(double));
				if (propParent != null)
				{
					propParent.Service.cpyDblToBuffer(value);
					for (int k = 0; k < 8; k++)
					{
						Marshal.WriteByte(pData, propByteOffset + k, propParent.Service.ByteBuffer[k]);
					}
				}
				else
				{
					Marshal.Copy(new double[1]
					{
						Convert.ToDouble(value)
					}, 0, pData, 1);
				}
				break;
			case DataType.Date:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, propByteOffset, Pvi.GetDateUInt32(value));
				break;
			case DataType.DateTime:
			case DataType.DT:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, propByteOffset, Pvi.GetDateTimeUInt32(value));
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, propByteOffset, Pvi.GetTimeSpanInt32(value));
				break;
			case DataType.String:
			{
				byte val2 = 0;
				propDataSize = DataSize;
				for (int m = 0; m < DataSize; m++)
				{
					Marshal.WriteByte(pData, propByteOffset + m, val2);
				}
				if (0 < value.ToString().Length)
				{
					string text = value.ToString();
					for (int n = 0; n < text.Length && n < DataSize; n++)
					{
						val2 = (byte)text[n];
						Marshal.WriteByte(pData, propByteOffset + n, val2);
					}
				}
				break;
			}
			case DataType.WString:
			{
				byte val = 0;
				propDataSize = DataSize;
				for (int i = 0; i < DataSize; i++)
				{
					Marshal.WriteByte(pData, propByteOffset + i, val);
				}
				if (0 < value.ToString().Length)
				{
					byte[] array = new byte[DataSize];
					IntPtr hMemory = PviMarshal.StringToHGlobalUni(value.ToString());
					if (value.ToString().Length * 2 > DataSize)
					{
						Marshal.Copy(hMemory, array, 0, propDataSize - 2);
						array[DataSize - 2] = 0;
						array[DataSize - 1] = 0;
					}
					else
					{
						Marshal.Copy(hMemory, array, 0, value.ToString().Length * 2);
					}
					for (int j = 0; j < array.Length; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, array[j]);
					}
					PviMarshal.FreeHGlobal(ref hMemory);
				}
				break;
			}
			default:
				if (propParent != null)
				{
					propParent.Value.isAssigned = true;
				}
				isAssigned = false;
				return;
			}
			propArrayLength = 1;
		}

		public void Assign(uint value)
		{
			if (CheckBuffers(value))
			{
				AssignValue(value);
				propObjValue = value;
			}
		}

		public void Assign(int value)
		{
			if (CheckBuffers((ulong)value))
			{
				AssignValue((ulong)value);
				propObjValue = value;
			}
		}

		public void Assign(ushort value)
		{
			if (CheckBuffers(value))
			{
				AssignValue(value);
				propObjValue = value;
			}
		}

		public void Assign(short value)
		{
			if (CheckBuffers((ulong)value))
			{
				AssignValue((ulong)value);
				propObjValue = value;
			}
		}

		public void Assign(byte value)
		{
			if (CheckBuffers(value))
			{
				AssignValue(value);
				propObjValue = value;
			}
		}

		public void Assign(sbyte value)
		{
			if (CheckBuffers((ulong)value))
			{
				AssignValue((ulong)value);
				propObjValue = value;
			}
		}

		public void Assign(float value)
		{
			if (CheckBuffers((ulong)value))
			{
				AssignValue(value);
				propObjValue = value;
			}
		}

		public void Assign(double value)
		{
			if (CheckBuffers((ulong)value))
			{
				AssignValue(value);
				propObjValue = value;
			}
		}

		public void Assign(bool value)
		{
			ulong num = 0uL;
			if (value)
			{
				num = 1uL;
			}
			if (CheckBuffers(num))
			{
				AssignValue(num);
				propObjValue = value;
			}
		}

		public void Assign(object value)
		{
			object obj = value;
			if (obj == null)
			{
				return;
			}
			if (propParent != null)
			{
				propParent.Value.isAssigned = true;
			}
			isAssigned = true;
			if (1 < propArrayLength)
			{
				if (IsPG2000String)
				{
					string text = "";
					if (obj is Array)
					{
						for (int i = 0; i < ((Array)obj).Length; i++)
						{
							text += ((Array)obj).GetValue(i).ToString();
						}
					}
					else
					{
						text = obj.ToString();
					}
					SetPG2000StringData(text);
				}
				else
				{
					AssignArrayValues(obj);
				}
				return;
			}
			if (IntPtr.Zero == pData)
			{
				if (DataSize == 0)
				{
					if (propParent != null)
					{
						propParent.Value.isAssigned = false;
					}
					isAssigned = false;
					return;
				}
				pData = PviMarshal.AllocHGlobal(DataSize);
				propHasOwnDataPtr = true;
			}
			if (IsPG2000String)
			{
				SetPG2000StringData(obj.ToString());
				return;
			}
			if (obj is string && 1 == propIsEnum && propEnumerations != null)
			{
				obj = propEnumerations.EnumValue((string)value);
				if (obj == null)
				{
					obj = value;
				}
			}
			if (IntPtr.Zero == pData)
			{
				return;
			}
			switch (DataType)
			{
			case DataType.Boolean:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				if (obj is bool)
				{
					if (!(bool)obj)
					{
						Marshal.WriteByte(pData, propByteOffset, 0);
					}
					else
					{
						Marshal.WriteByte(pData, propByteOffset, 1);
					}
				}
				else if (!IsBoolTrue(obj))
				{
					Marshal.WriteByte(pData, propByteOffset, 0);
				}
				else
				{
					Marshal.WriteByte(pData, propByteOffset, 1);
				}
				break;
			case DataType.SByte:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				PviMarshal.WriteSByte(pData, propByteOffset, PviMarshal.toSByte(obj));
				break;
			case DataType.Int16:
				propDataSize = Marshal.SizeOf(typeof(short));
				Marshal.WriteInt16(pData, propByteOffset, PviMarshal.toInt16(obj));
				break;
			case DataType.Int32:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, propByteOffset, PviMarshal.toInt32(obj));
				break;
			case DataType.Int64:
				propDataSize = Marshal.SizeOf(typeof(long));
				PviMarshal.WriteInt64(pData, propByteOffset, PviMarshal.toInt64(obj));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				propDataSize = Marshal.SizeOf(typeof(byte));
				Marshal.WriteByte(pData, propByteOffset, PviMarshal.toByte(obj));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				propDataSize = Marshal.SizeOf(typeof(ushort));
				PviMarshal.WriteUInt16(pData, propByteOffset, PviMarshal.toUInt16(obj));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				propDataSize = Marshal.SizeOf(typeof(uint));
				PviMarshal.WriteUInt32(pData, propByteOffset, PviMarshal.toUInt32(obj));
				break;
			case DataType.UInt64:
				propDataSize = Marshal.SizeOf(typeof(ulong));
				PviMarshal.WriteUInt64(pData, propByteOffset, PviMarshal.toUInt64(obj));
				break;
			case DataType.Single:
				propDataSize = Marshal.SizeOf(typeof(float));
				if (propParent != null)
				{
					propParent.Service.cpyFltToBuffer(obj);
					for (int j = 0; j < 4; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, propParent.Service.ByteBuffer[j]);
					}
				}
				else
				{
					Marshal.Copy(new float[1]
					{
						Convert.ToSingle(obj)
					}, 0, pData, 1);
				}
				break;
			case DataType.Double:
				propDataSize = Marshal.SizeOf(typeof(double));
				if (propParent != null)
				{
					propParent.Service.cpyDblToBuffer(obj);
					for (int j = 0; j < 8; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, propParent.Service.ByteBuffer[j]);
					}
				}
				else
				{
					Marshal.Copy(new double[1]
					{
						Convert.ToDouble(obj)
					}, 0, pData, 1);
				}
				break;
			case DataType.Date:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, propByteOffset, Pvi.GetDateUInt32(obj));
				break;
			case DataType.DateTime:
			case DataType.DT:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, propByteOffset, Pvi.GetDateTimeUInt32(obj));
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, propByteOffset, Pvi.GetTimeSpanInt32(obj));
				break;
			case DataType.String:
			{
				byte val2 = 0;
				propDataSize = DataSize;
				for (int j = 0; j < DataSize; j++)
				{
					Marshal.WriteByte(pData, propByteOffset + j, val2);
				}
				if (obj != null && 0 < obj.ToString().Length)
				{
					string text2 = obj.ToString();
					for (int j = 0; j < text2.Length && j < DataSize; j++)
					{
						val2 = (byte)text2[j];
						Marshal.WriteByte(pData, propByteOffset + j, val2);
					}
				}
				break;
			}
			case DataType.WString:
			{
				byte val = 0;
				propDataSize = DataSize;
				for (int j = 0; j < DataSize; j++)
				{
					Marshal.WriteByte(pData, propByteOffset + j, val);
				}
				if (obj != null && 0 < obj.ToString().Length)
				{
					byte[] array = new byte[DataSize];
					IntPtr hMemory = PviMarshal.StringToHGlobalUni(obj.ToString());
					if (obj.ToString().Length * 2 > DataSize)
					{
						Marshal.Copy(hMemory, array, 0, propDataSize - 2);
						array[DataSize - 2] = 0;
						array[DataSize - 1] = 0;
					}
					else
					{
						Marshal.Copy(hMemory, array, 0, obj.ToString().Length * 2);
					}
					for (int j = 0; j < array.Length; j++)
					{
						Marshal.WriteByte(pData, propByteOffset + j, array[j]);
					}
					PviMarshal.FreeHGlobal(ref hMemory);
				}
				break;
			}
			default:
				if (propParent != null)
				{
					propParent.Value.isAssigned = true;
				}
				isAssigned = false;
				AssignArrayValues(obj);
				return;
			}
			propArrayLength = 1;
			propObjValue = obj;
		}

		public void Assign(object value, int index)
		{
			object obj = value;
			int num = 0;
			if (obj == null || index >= ArrayLength)
			{
				return;
			}
			if (propParent != null)
			{
				propParent.Value.isAssigned = true;
			}
			isAssigned = true;
			num = index * TypeLength;
			if (IntPtr.Zero == pData)
			{
				if (DataSize == 0)
				{
					if (propParent != null)
					{
						propParent.Value.isAssigned = false;
					}
					isAssigned = false;
					return;
				}
				pData = PviMarshal.AllocHGlobal(DataSize);
				propHasOwnDataPtr = true;
			}
			if (IsPG2000String)
			{
				SetPG2000StringData(obj.ToString());
				return;
			}
			if (obj is string && 1 == propIsEnum && propEnumerations != null)
			{
				obj = propEnumerations.EnumValue((string)value);
				if (obj == null)
				{
					obj = value;
				}
			}
			if (IntPtr.Zero == pData)
			{
				return;
			}
			switch (DataType)
			{
			case DataType.Boolean:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				propByteField[num] = 0;
				if (obj is bool)
				{
					if (!(bool)obj)
					{
						Marshal.WriteByte(pData, num, 0);
						break;
					}
					Marshal.WriteByte(pData, num, 1);
					propByteField[num] = 1;
				}
				else if (!IsBoolTrue(obj))
				{
					Marshal.WriteByte(pData, num, 0);
				}
				else
				{
					Marshal.WriteByte(pData, num, 1);
					propByteField[num] = 1;
				}
				break;
			case DataType.SByte:
				propDataSize = Marshal.SizeOf(typeof(sbyte));
				PviMarshal.WriteSByte(pData, num, PviMarshal.toSByte(obj));
				break;
			case DataType.Int16:
				propDataSize = Marshal.SizeOf(typeof(short));
				Marshal.WriteInt16(pData, num, PviMarshal.toInt16(obj));
				break;
			case DataType.Int32:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, num, PviMarshal.toInt32(obj));
				break;
			case DataType.Int64:
				propDataSize = Marshal.SizeOf(typeof(long));
				PviMarshal.WriteInt64(pData, num, PviMarshal.toInt64(obj));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				propDataSize = Marshal.SizeOf(typeof(byte));
				Marshal.WriteByte(pData, num, PviMarshal.toByte(obj));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				propDataSize = Marshal.SizeOf(typeof(ushort));
				PviMarshal.WriteUInt16(pData, num, PviMarshal.toUInt16(obj));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				propDataSize = Marshal.SizeOf(typeof(uint));
				PviMarshal.WriteUInt32(pData, num, PviMarshal.toUInt32(obj));
				break;
			case DataType.UInt64:
				propDataSize = Marshal.SizeOf(typeof(ulong));
				PviMarshal.WriteUInt64(pData, num, PviMarshal.toUInt64(obj));
				break;
			case DataType.Single:
				propDataSize = Marshal.SizeOf(typeof(float));
				if (propParent != null)
				{
					propParent.Service.cpyFltToBuffer(obj);
					for (int i = 0; i < 4; i++)
					{
						Marshal.WriteByte(pData, num + i, propParent.Service.ByteBuffer[i]);
					}
				}
				else
				{
					Marshal.Copy(new float[1]
					{
						Convert.ToSingle(obj)
					}, 0, pData, 1);
				}
				break;
			case DataType.Double:
				propDataSize = Marshal.SizeOf(typeof(double));
				if (propParent != null)
				{
					propParent.Service.cpyDblToBuffer(obj);
					for (int i = 0; i < 8; i++)
					{
						Marshal.WriteByte(pData, num + i, propParent.Service.ByteBuffer[i]);
					}
				}
				else
				{
					Marshal.Copy(new double[1]
					{
						Convert.ToDouble(obj)
					}, 0, pData, 1);
				}
				break;
			case DataType.Date:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, num, Pvi.GetDateUInt32(obj));
				break;
			case DataType.DateTime:
			case DataType.DT:
				propDataSize = Marshal.SizeOf(typeof(int));
				PviMarshal.WriteUInt32(pData, num, Pvi.GetDateTimeUInt32(obj));
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				propDataSize = Marshal.SizeOf(typeof(int));
				Marshal.WriteInt32(pData, num, Pvi.GetTimeSpanInt32(obj));
				break;
			case DataType.String:
			{
				byte val2 = 0;
				propDataSize = DataSize;
				for (int i = 0; i < DataSize; i++)
				{
					Marshal.WriteByte(pData, num + i, val2);
				}
				if (obj != null && 0 < obj.ToString().Length)
				{
					string text = obj.ToString();
					for (int i = 0; i < text.Length && i < DataSize; i++)
					{
						val2 = (byte)text[i];
						Marshal.WriteByte(pData, num + i, val2);
					}
				}
				break;
			}
			case DataType.WString:
			{
				byte val = 0;
				propDataSize = DataSize;
				for (int i = 0; i < DataSize; i++)
				{
					Marshal.WriteByte(pData, num + i, val);
				}
				if (obj != null && 0 < obj.ToString().Length)
				{
					byte[] array = new byte[DataSize];
					IntPtr hMemory = PviMarshal.StringToHGlobalUni(obj.ToString());
					if (obj.ToString().Length * 2 > DataSize)
					{
						Marshal.Copy(hMemory, array, 0, propDataSize - 2);
						array[DataSize - 2] = 0;
						array[DataSize - 1] = 0;
					}
					else
					{
						Marshal.Copy(hMemory, array, 0, obj.ToString().Length * 2);
					}
					for (int i = 0; i < array.Length; i++)
					{
						Marshal.WriteByte(pData, num + i, array[i]);
					}
					PviMarshal.FreeHGlobal(ref hMemory);
				}
				break;
			}
			default:
				if (propParent != null)
				{
					propParent.Value.isAssigned = true;
				}
				isAssigned = false;
				return;
			}
			PviMarshal.Copy(pData, num, num, ref propByteField, propDataSize);
		}

		public Value(object[] value)
		{
			isAssigned = false;
			Assign(value);
		}

		public void Assign(object[] value)
		{
			switch (DataType)
			{
			case DataType.Boolean:
			{
				bool[] array10 = new bool[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array10, 0, value.Length);
				}
				else
				{
					array10.SetValue(value, 0);
				}
				SetByteField(array10);
				break;
			}
			case DataType.SByte:
			{
				sbyte[] array4 = new sbyte[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array4, 0, value.Length);
				}
				else
				{
					array4.SetValue(value, 0);
				}
				SetByteField(array4);
				break;
			}
			case DataType.Int16:
			{
				short[] array11 = new short[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array11, 0, value.Length);
				}
				else
				{
					array11.SetValue(value, 0);
				}
				SetByteField(array11);
				break;
			}
			case DataType.Int32:
			{
				int[] array7 = new int[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array7, 0, value.Length);
				}
				else
				{
					array7.SetValue(value, 0);
				}
				SetByteField(array7);
				break;
			}
			case DataType.Int64:
			{
				long[] array2 = new long[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array2, 0, value.Length);
				}
				else
				{
					array2.SetValue(value, 0);
				}
				SetByteField(array2);
				break;
			}
			case DataType.Byte:
			case DataType.UInt8:
			{
				byte[] array8 = new byte[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array8, 0, value.Length);
				}
				else
				{
					array8.SetValue(value, 0);
				}
				SetByteField(array8);
				break;
			}
			case DataType.UInt16:
			case DataType.WORD:
			{
				ushort[] array6 = new ushort[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array6, 0, value.Length);
				}
				else
				{
					array6.SetValue(value, 0);
				}
				SetByteField(array6);
				break;
			}
			case DataType.UInt32:
			case DataType.DWORD:
			{
				uint[] array14 = new uint[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array14, 0, value.Length);
				}
				else
				{
					array14.SetValue(value, 0);
				}
				SetByteField(array14);
				break;
			}
			case DataType.UInt64:
			{
				ulong[] array12 = new ulong[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array12, 0, value.Length);
				}
				else
				{
					array12.SetValue(value, 0);
				}
				SetByteField(array12);
				break;
			}
			case DataType.Single:
			{
				float[] array3 = new float[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array3, 0, value.Length);
				}
				else
				{
					array3.SetValue(value, 0);
				}
				SetByteField(array3);
				break;
			}
			case DataType.Double:
			{
				double[] array13 = new double[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array13, 0, value.Length);
				}
				else
				{
					array13.SetValue(value, 0);
				}
				SetByteField(array13);
				break;
			}
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
			{
				TimeSpan[] array9 = new TimeSpan[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array9, 0, value.Length);
				}
				else
				{
					array9.SetValue(value, 0);
				}
				SetByteField(array9);
				break;
			}
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
			{
				DateTime[] array5 = new DateTime[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array5, 0, value.Length);
				}
				else
				{
					array5.SetValue(value, 0);
				}
				SetByteField(array5);
				break;
			}
			case DataType.String:
			{
				string[] array = new string[value.Length];
				if (1 < value.Length)
				{
					Array.Copy(value, 0, array, 0, value.Length);
				}
				else
				{
					array.SetValue(value, 0);
				}
				SetByteField(array);
				break;
			}
			default:
				throw new InvalidOperationException();
			}
		}

		private void CopyToSystemDataTypeArray(IntPtr pNewData, Array dataArray, ref ArrayList changedMembers)
		{
			if (propDataType == DataType.Unknown)
			{
				return;
			}
			switch (propDataType)
			{
			case (DataType)6:
			case (DataType)11:
			case DataType.TimeSpan:
			case DataType.DateTime:
			case DataType.Structure:
			case DataType.WString:
			case DataType.TimeOfDay:
			case DataType.Date:
			case DataType.Data:
			case DataType.LWORD:
				break;
			case DataType.Boolean:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					if (Marshal.ReadByte(pNewData, i) == 0)
					{
						if (((bool[])dataArray)[i])
						{
							changedMembers.Add("[" + i + "]");
						}
						((bool[])dataArray)[i] = false;
					}
					else
					{
						if (!((bool[])dataArray)[i])
						{
							changedMembers.Add("[" + i + "]");
						}
						((bool[])dataArray)[i] = true;
					}
				}
				break;
			case DataType.SByte:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					sbyte b = (sbyte)Marshal.ReadByte(pNewData, i);
					if (((sbyte[])dataArray)[i] != b)
					{
						changedMembers.Add("[" + i + "]");
					}
					((sbyte[])dataArray)[i] = b;
				}
				break;
			case DataType.Int16:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					short num6 = Marshal.ReadInt16(pNewData, i * 2);
					if (((short[])dataArray)[i] != num6)
					{
						changedMembers.Add("[" + i + "]");
					}
					((short[])dataArray)[i] = num6;
				}
				break;
			case DataType.Int32:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					int num2 = Marshal.ReadInt32(pNewData, i * 4);
					if (((int[])dataArray)[i] != num2)
					{
						changedMembers.Add("[" + i + "]");
					}
					((int[])dataArray)[i] = num2;
				}
				break;
			case DataType.Int64:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					long num4 = PviMarshal.ReadInt64(pNewData, i * 8);
					if (((long[])dataArray)[i] != num4)
					{
						changedMembers.Add("[" + i + "]");
					}
					((long[])dataArray)[i] = num4;
				}
				break;
			case DataType.Byte:
			case DataType.UInt8:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					byte b2 = Marshal.ReadByte(pNewData, i);
					if (((byte[])dataArray)[i] != b2)
					{
						changedMembers.Add("[" + i + "]");
					}
					((byte[])dataArray)[i] = b2;
				}
				break;
			case DataType.UInt16:
			case DataType.WORD:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					ushort num5 = (ushort)Marshal.ReadInt16(pNewData, i * 2);
					if (((ushort[])dataArray)[i] != num5)
					{
						changedMembers.Add("[" + i + "]");
					}
					((ushort[])dataArray)[i] = num5;
				}
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					uint num3 = (uint)Marshal.ReadInt32(pNewData, i * 4);
					if (((uint[])dataArray)[i] != num3)
					{
						changedMembers.Add("[" + i + "]");
					}
					((uint[])dataArray)[i] = num3;
				}
				break;
			case DataType.UInt64:
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				for (int i = 0; i < propArrayLength; i++)
				{
					ulong num7 = (ulong)PviMarshal.ReadInt64(pNewData, i * 8);
					if (((ulong[])dataArray)[i] != num7)
					{
						changedMembers.Add("[" + i + "]");
					}
					((ulong[])dataArray)[i] = num7;
				}
				break;
			case DataType.Single:
			{
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				float[] array3 = new float[propArrayLength];
				Marshal.Copy(pNewData, array3, 0, propArrayLength);
				for (int i = 0; i < propArrayLength; i++)
				{
					if (((float[])dataArray)[i] != array3[i])
					{
						changedMembers.Add("[" + i + "]");
					}
					((float[])dataArray)[i] = array3[i];
				}
				break;
			}
			case DataType.Double:
			{
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				double[] array2 = new double[propArrayLength];
				Marshal.Copy(pNewData, array2, 0, propArrayLength);
				for (int i = 0; i < propArrayLength; i++)
				{
					if (((double[])dataArray)[i] != array2[i])
					{
						changedMembers.Add("[" + i + "]");
					}
					((double[])dataArray)[i] = array2[i];
				}
				break;
			}
			case DataType.String:
			{
				if (!(IntPtr.Zero != pNewData))
				{
					break;
				}
				byte[] array = new byte[DataSize];
				Marshal.Copy(pNewData, array, 0, DataSize);
				IntPtr hMemory = PviMarshal.AllocHGlobal(propTypeLength);
				for (int i = 0; i < propArrayLength; i++)
				{
					Marshal.Copy(array, propTypeLength * i, hMemory, propTypeLength);
					string text = PviMarshal.PtrToStringAnsi(hMemory, propTypeLength);
					int num = text.IndexOf("\0");
					if (-1 != num)
					{
						text = text.Substring(0, num);
					}
					if (text.CompareTo(((string[])dataArray)[i]) != 0)
					{
						changedMembers.Add("[" + i + "]");
					}
					((string[])dataArray)[i] = text;
				}
				PviMarshal.FreeHGlobal(ref hMemory);
				break;
			}
			}
		}

		private Array ToSystemDataTypeArray(byte[] bytes, int offset)
		{
			if (bytes != null && 0 < propArrayLength)
			{
				IntPtr zero = IntPtr.Zero;
				if (propDataType != 0)
				{
					switch (propDataType)
					{
					case DataType.Boolean:
					{
						bool[] array6 = new bool[propArrayLength];
						for (int i = 0; i < propArrayLength; i++)
						{
							if (bytes[offset + i] == 0)
							{
								array6[i] = false;
							}
							else
							{
								array6[i] = true;
							}
						}
						return array6;
					}
					case DataType.SByte:
					{
						sbyte[] array5 = new sbyte[propArrayLength];
						for (int i = 0; i < propArrayLength; i++)
						{
							array5.SetValue((sbyte)bytes[offset + i], i);
						}
						return array5;
					}
					case DataType.Int16:
					{
						short[] array13 = new short[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 2);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 2);
						Marshal.Copy(zero, array13, 0, propArrayLength);
						PviMarshal.FreeHGlobal(ref zero);
						return array13;
					}
					case DataType.Int32:
					{
						int[] array11 = new int[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 4);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 4);
						Marshal.Copy(zero, array11, 0, propArrayLength);
						PviMarshal.FreeHGlobal(ref zero);
						return array11;
					}
					case DataType.Byte:
					case DataType.UInt8:
					{
						byte[] array2 = new byte[propArrayLength];
						for (int i = 0; i < propArrayLength && bytes.GetLength(0) >= offset + i; i++)
						{
							array2.SetValue(bytes[offset + i], i);
						}
						return array2;
					}
					case DataType.UInt16:
					case DataType.WORD:
					{
						ushort[] array10 = new ushort[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 2);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 2);
						for (int i = 0; i < propArrayLength; i++)
						{
							array10.SetValue((ushort)Marshal.ReadInt16(zero, i * 2), i);
						}
						PviMarshal.FreeHGlobal(ref zero);
						return array10;
					}
					case DataType.UInt32:
					case DataType.DWORD:
					{
						uint[] array3 = new uint[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 4);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 4);
						for (int i = 0; i < propArrayLength; i++)
						{
							array3.SetValue((uint)Marshal.ReadInt32(zero, i * 4), i);
						}
						PviMarshal.FreeHGlobal(ref zero);
						return array3;
					}
					case DataType.DateTime:
					case DataType.Date:
					case DataType.DT:
					{
						DateTime[] array12 = new DateTime[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 4);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 4);
						for (int i = 0; i < propArrayLength; i++)
						{
							array12.SetValue(Pvi.UInt32ToDateTime((uint)Marshal.ReadInt32(zero, i * 4)), i);
						}
						PviMarshal.FreeHGlobal(ref zero);
						return array12;
					}
					case DataType.TimeSpan:
					case DataType.TimeOfDay:
					case DataType.TOD:
					{
						TimeSpan[] array9 = new TimeSpan[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 4);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 4);
						for (int i = 0; i < propArrayLength; i++)
						{
							array9.SetValue(new TimeSpan(10000L * (long)Marshal.ReadInt32(zero, i * 4)), i);
						}
						PviMarshal.FreeHGlobal(ref zero);
						return array9;
					}
					case DataType.Single:
					{
						float[] array8 = new float[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 4);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 4);
						Marshal.Copy(zero, array8, 0, propArrayLength);
						PviMarshal.FreeHGlobal(ref zero);
						return array8;
					}
					case DataType.Double:
					{
						double[] array7 = new double[propArrayLength];
						zero = PviMarshal.AllocHGlobal(propArrayLength * 8);
						Marshal.Copy(bytes, offset, zero, propArrayLength * 8);
						Marshal.Copy(zero, array7, 0, propArrayLength);
						PviMarshal.FreeHGlobal(ref zero);
						return array7;
					}
					case DataType.String:
					{
						string[] array4 = new string[propArrayLength];
						for (int i = 0; i < propArrayLength; i++)
						{
							array4.SetValue(PviMarshal.ToAnsiString(bytes, offset + propTypeLength * i, propTypeLength), i);
						}
						return array4;
					}
					case DataType.WString:
					{
						string[] array = new string[propArrayLength];
						for (int i = 0; i < propArrayLength; i++)
						{
							string value = PviMarshal.ToWString(bytes, offset + propTypeLength * i, propTypeLength);
							array.SetValue(value, i);
						}
						return array;
					}
					}
				}
			}
			return null;
		}

		private object NonSimpleToType(Type objType, IFormatProvider provider)
		{
			if (DataType.Structure == propDataType)
			{
				return ((IConvertible)propParent.Members.FirstSimpleTyped.Value).ToType(objType, provider);
			}
			return ((IConvertible)propParent.propPviValue[0]).ToType(objType, provider);
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			bool flag = false;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (bool)NonSimpleToType(flag.GetType(), provider);
			}
			if (propObjValue != null)
			{
				switch (propDataType)
				{
				case DataType.Boolean:
					if (propObjValue is Array)
					{
						return ((IConvertible)((Array)propObjValue).GetValue(0)).ToBoolean(provider);
					}
					return ((IConvertible)propObjValue).ToBoolean(provider);
				case DataType.SByte:
				case DataType.Int16:
				case DataType.Int32:
				case DataType.Int64:
				case DataType.Byte:
				case DataType.UInt16:
				case DataType.UInt32:
				case DataType.UInt64:
				case DataType.Single:
				case DataType.Double:
				case DataType.String:
				case DataType.WORD:
				case DataType.DWORD:
				case DataType.UInt8:
					if (propObjValue is Array)
					{
						if (((Array)propObjValue).GetValue(0).ToString().CompareTo("0") != 0)
						{
							return true;
						}
						return false;
					}
					if (propObjValue.ToString().CompareTo("0") != 0)
					{
						return true;
					}
					return false;
				default:
					return ((IConvertible)propObjValue).ToBoolean(provider);
				}
			}
			return false;
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			sbyte b = 0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (sbyte)NonSimpleToType(b.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToSByte(provider);
				}
				return ((IConvertible)propObjValue).ToSByte(provider);
			default:
				return ((IConvertible)propObjValue).ToSByte(provider);
			}
		}

		public short ToInt16(IFormatProvider provider)
		{
			short num = 0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (short)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToInt16(provider);
				}
				return ((IConvertible)propObjValue).ToInt16(provider);
			default:
				return ((IConvertible)propObjValue).ToInt16(provider);
			}
		}

		public int ToInt32(IFormatProvider provider)
		{
			int num = 0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (int)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToInt32(provider);
				}
				return ((IConvertible)propObjValue).ToInt32(provider);
			case DataType.String:
				if ("true" == propObjValue.ToString().ToLower())
				{
					return 1;
				}
				if ("false" == propObjValue.ToString().ToLower())
				{
					return 0;
				}
				return ((IConvertible)propObjValue).ToInt32(provider);
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return PviMarshal.toInt32(propUInt32Val);
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return PviMarshal.toInt32(propUInt32Val);
			default:
				return ((IConvertible)propObjValue).ToInt32(provider);
			}
		}

		public byte ToByte(IFormatProvider provider)
		{
			byte b = 0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (byte)NonSimpleToType(b.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToByte(provider);
				}
				return ((IConvertible)propObjValue).ToByte(provider);
			case DataType.String:
			{
				string text = propObjValue.ToString();
				return (byte)text[0];
			}
			default:
				return ((IConvertible)propObjValue).ToByte(provider);
			}
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			ushort num = 0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (ushort)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToUInt16(provider);
				}
				return ((IConvertible)propObjValue).ToUInt16(provider);
			default:
				throw new InvalidCastException();
			}
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			uint num = 0u;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (uint)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1u;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1u;
				}
				return 0u;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToUInt32(provider);
				}
				return ((IConvertible)propObjValue).ToUInt32(provider);
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return propUInt32Val;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return propUInt32Val;
			default:
				throw new InvalidOperationException();
			}
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			ulong num = 0uL;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (ulong)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1uL;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1uL;
				}
				return 0uL;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToUInt64(provider);
				}
				return ((IConvertible)propObjValue).ToUInt64(provider);
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return Convert.ToUInt64(propUInt32Val);
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return Convert.ToUInt64(propUInt32Val);
			default:
				throw new InvalidOperationException();
			}
		}

		public float ToSingle(IFormatProvider provider)
		{
			float num = 0f;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (float)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1f;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1f;
				}
				return 0f;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToSingle(provider);
				}
				return ((IConvertible)propObjValue).ToSingle(provider);
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return Convert.ToSingle(propUInt32Val);
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return Convert.ToSingle(propUInt32Val);
			default:
				return ((IConvertible)propObjValue).ToSingle(provider);
			}
		}

		public object ToSystemDataTypeValue(IFormatProvider provider)
		{
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				throw new InvalidCastException();
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue.ToString().ToLower().CompareTo("0") == 0 || propObjValue.ToString().ToLower().CompareTo("false") == 0)
				{
					return false;
				}
				return true;
			case DataType.SByte:
				return ((IConvertible)propObjValue).ToSByte(provider);
			case DataType.Int16:
				return ((IConvertible)propObjValue).ToInt16(provider);
			case DataType.Int32:
				return ((IConvertible)propObjValue).ToInt32(provider);
			case DataType.Int64:
				return ((IConvertible)propObjValue).ToInt64(provider);
			case DataType.Byte:
			case DataType.UInt8:
				return ((IConvertible)propObjValue).ToByte(provider);
			case DataType.UInt16:
			case DataType.WORD:
				return ((IConvertible)propObjValue).ToUInt16(provider);
			case DataType.UInt32:
			case DataType.DWORD:
				return ((IConvertible)propObjValue).ToUInt32(provider);
			case DataType.UInt64:
				return ((IConvertible)propObjValue).ToUInt64(provider);
			case DataType.Single:
				return ((IConvertible)propObjValue).ToSingle(provider);
			case DataType.Double:
				return ((IConvertible)propObjValue).ToDouble(provider);
			case DataType.String:
				return propObjValue.ToString();
			default:
				throw new InvalidCastException();
			}
		}

		public double ToDouble(IFormatProvider provider)
		{
			double num = 0.0;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (double)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToDouble(provider);
				}
				return ((IConvertible)propObjValue).ToDouble(provider);
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return Convert.ToDouble(propUInt32Val);
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return Convert.ToDouble(propUInt32Val);
			default:
				return ((IConvertible)propObjValue).ToDouble(provider);
			}
		}

		public TimeSpan ToTimeSpan(IFormatProvider provider)
		{
			TimeSpan timeSpan = new TimeSpan(0L);
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (TimeSpan)NonSimpleToType(timeSpan.GetType(), provider);
			}
			if (propObjValue == null)
			{
				return new TimeSpan(0L);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return new TimeSpan(((IConvertible)((Array)propObjValue).GetValue(0)).ToInt32(provider));
				}
				return new TimeSpan(((IConvertible)propObjValue).ToInt32(provider));
			case DataType.TimeSpan:
				return (TimeSpan)propObjValue;
			default:
				throw new InvalidCastException();
			}
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			DateTime dateTime = new DateTime(0L);
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (DateTime)NonSimpleToType(dateTime.GetType(), provider);
			}
			if (propObjValue == null)
			{
				return new DateTime(0L);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return new DateTime(((IConvertible)((Array)propObjValue).GetValue(0)).ToUInt32(provider));
				}
				return new DateTime(((IConvertible)propObjValue).ToUInt32(provider));
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return (DateTime)propObjValue;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				if (0 < ((TimeSpan)propObjValue).Days)
				{
					return new DateTime(((TimeSpan)propObjValue).Ticks);
				}
				return Convert.ToDateTime(propObjValue.ToString());
			default:
				return new DateTime(((IConvertible)propObjValue).ToUInt32(provider));
			}
		}

		public string ToString(IFormatProvider provider)
		{
			if (propObjValue != null)
			{
				if (propDataType == DataType.TimeSpan)
				{
					string text = "";
					text = ((TimeSpan)propObjValue).Days.ToString() + "." + ((TimeSpan)propObjValue).Hours.ToString() + ":" + ((TimeSpan)propObjValue).Minutes.ToString() + ":" + ((TimeSpan)propObjValue).Seconds.ToString();
					if (0 < ((TimeSpan)propObjValue).Milliseconds)
					{
						text = text + "." + ((TimeSpan)propObjValue).Milliseconds.ToString("000");
					}
					return text;
				}
				if (propDataType == DataType.TimeOfDay || propDataType == DataType.TOD)
				{
					string text2 = "";
					text2 = ((TimeSpan)propObjValue).Hours.ToString() + ":" + ((TimeSpan)propObjValue).Minutes.ToString() + ":" + ((TimeSpan)propObjValue).Seconds.ToString();
					if (0 < ((TimeSpan)propObjValue).Milliseconds)
					{
						text2 = text2 + "." + ((TimeSpan)propObjValue).Milliseconds.ToString("000");
					}
					return text2;
				}
				if (propDataType == DataType.DateTime || propDataType == DataType.DT)
				{
					return propObjValue.ToString();
				}
				if (propDataType == DataType.Date)
				{
					return DateTime.Parse(propObjValue.ToString()).ToShortDateString();
				}
				return ((IConvertible)propObjValue).ToString(provider);
			}
			return ToString(null, provider);
		}

		public string BinaryToStringUNI()
		{
			IntPtr hMemory;
			if (propByteField != null)
			{
				hMemory = PviMarshal.AllocHGlobal(propByteField.Length);
				Marshal.Copy(propByteField, 0, hMemory, propByteField.Length);
			}
			else
			{
				hMemory = pData;
			}
			string result = Marshal.PtrToStringUni(hMemory);
			if (propByteField != null)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return result;
		}

		public string BinaryToAnsiString()
		{
			string result = "";
			if (propByteField != null)
			{
				result = PviMarshal.ToAnsiString(propByteField);
			}
			return result;
		}

		public virtual string ToEnum()
		{
			string text = null;
			if (propObjValue != null)
			{
				ToInt32(null);
				text = ToString(null);
				if (1 == propIsEnum && propEnumerations != null)
				{
					text = propEnumerations.EnumName(ToString(null));
					if (text == null)
					{
						text = ToString(null);
					}
				}
			}
			else
			{
				text = ToString(null);
			}
			return text;
		}

		public override string ToString()
		{
			if (IsPG2000String)
			{
				return BinaryToAnsiString();
			}
			return ToString(null);
		}

		private string StructMembersToString(string format, IFormatProvider provider)
		{
			string text = null;
			switch (propDataType)
			{
			case DataType.Boolean:
				text = "";
				if (IsOfTypeArray)
				{
					if (propParent.Members != null && 0 < propParent.Members.Count)
					{
						for (int i = 0; i < propParent.Members.Count; i++)
						{
							text = text + propParent.Members[i].Value.ToString(format, provider) + ";";
						}
					}
					else if (propObjValue != null)
					{
						text = ((IFormattable)propObjValue).ToString(format, provider);
					}
				}
				else
				{
					if (propObjValue == null)
					{
						break;
					}
					if (TypeCode.Boolean == Type.GetTypeCode(propObjValue.GetType()))
					{
						text = ((bool)propObjValue).ToString(provider);
						break;
					}
					text = "false";
					if (propObjValue.ToString().CompareTo("0") != 0)
					{
						text = "true";
					}
				}
				break;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.DateTime:
			case DataType.Date:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
			case DataType.DT:
				text = "";
				if (IsOfTypeArray)
				{
					if (propParent.Members != null && 0 < propParent.Members.Count)
					{
						for (int i = 0; i < propParent.Members.Count; i++)
						{
							text = text + propParent.Members[i].Value.ToString(format, provider) + ";";
						}
					}
					else if (propObjValue != null)
					{
						text = ((IFormattable)propObjValue).ToString(format, provider);
					}
				}
				else if (propObjValue != null)
				{
					text = ((!(propObjValue is string)) ? ((IFormattable)propObjValue).ToString(format, provider) : propObjValue.ToString());
				}
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				text = "";
				if (1 < propArrayLength && propParent != null && propParent.Members != null)
				{
					if (0 < propParent.Members.Count)
					{
						for (int i = 0; i < propParent.Members.Count; i++)
						{
							text = text + propParent.Members[i].Value.ToString(format, provider) + ";";
						}
					}
					else if (propObjValue != null)
					{
						text = ((IFormattable)propObjValue).ToString(format, provider);
					}
				}
				else if (propObjValue != null)
				{
					text = propObjValue.ToString();
				}
				break;
			case DataType.Structure:
				text = "";
				if (propParent.Members != null)
				{
					for (int i = 0; i < propParent.Members.Count; i++)
					{
						text = text + propParent.Members[i].Value.ToString(format, provider) + ";";
					}
				}
				else if (propByteField != null)
				{
					for (int i = 0; i < propByteField.GetLength(0); i++)
					{
						text = text + propByteField.GetValue(i).ToString() + ";";
					}
				}
				break;
			case DataType.String:
				text = "";
				if (propObjValue != null)
				{
					text = propObjValue.ToString();
				}
				else if (1 < propArrayLength)
				{
					for (int i = 0; i < propArrayLength; i++)
					{
						text = text + propParent.Value[i].ToString(format, provider) + ";";
					}
				}
				else if (propObjValue != null)
				{
					text = ((IFormattable)propObjValue).ToString(format, provider);
				}
				break;
			case DataType.WString:
				text = "";
				if (propObjValue != null)
				{
					text = propObjValue.ToString();
				}
				else if (1 < propArrayLength)
				{
					for (int i = 0; i < propArrayLength; i++)
					{
						text = text + propParent.Value[i].ToString(format, provider) + ";";
					}
				}
				else if (propObjValue != null)
				{
					text = ((IFormattable)propObjValue).ToString(format, provider);
				}
				break;
			default:
				text = null;
				break;
			}
			return text;
		}

		public string ToStringUNI(string format, IFormatProvider provider)
		{
			return BinaryToStringUNI();
		}

		public string ToAnsiString(string format, IFormatProvider provider)
		{
			return BinaryToAnsiString();
		}

		public string ToString(string format, IFormatProvider provider)
		{
			Array array = null;
			string text = null;
			if (IsPG2000String)
			{
				return BinaryToAnsiString();
			}
			if (IsOfTypeArray && DataType.Structure != propDataType)
			{
				array = ArrayData;
				if (array != null)
				{
					for (int i = 0; i < propArrayLength; i++)
					{
						text = text + array.GetValue(i).ToString() + ";";
					}
				}
				else
				{
					text = StructMembersToString(format, provider);
				}
			}
			else
			{
				text = StructMembersToString(format, provider);
			}
			if (text != null && 0 < text.Length)
			{
				while (text.Length - 1 == text.LastIndexOf(";"))
				{
					if (1 == text.Length)
					{
						return "";
					}
					text = text.Substring(0, text.Length - 1);
				}
			}
			return text;
		}

		public char ToChar(IFormatProvider provider)
		{
			char c = '\0';
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (char)NonSimpleToType(c.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return '\u0001';
					}
				}
				else if ((bool)propObjValue)
				{
					return '\u0001';
				}
				return '\0';
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToChar(provider);
				}
				return ((IConvertible)propObjValue).ToChar(provider);
			case DataType.String:
			{
				string text = propObjValue.ToString();
				return text[0];
			}
			default:
				return ((IConvertible)propObjValue).ToChar(provider);
			}
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			decimal num = 0m;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (decimal)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				return ((IConvertible)propObjValue).ToDecimal(provider);
			case DataType.String:
			{
				string text = propObjValue.ToString();
				return text[0];
			}
			default:
				return ((IConvertible)propObjValue).ToDecimal(provider);
			}
		}

		public long ToInt64(IFormatProvider provider)
		{
			long num = 0L;
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return (long)NonSimpleToType(num.GetType(), provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1L;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1L;
				}
				return 0L;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				if (propObjValue is Array)
				{
					return ((IConvertible)((Array)propObjValue).GetValue(0)).ToInt64(provider);
				}
				return ((IConvertible)propObjValue).ToInt64(provider);
			case DataType.String:
			{
				string text = propObjValue.ToString();
				return text[0];
			}
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return Convert.ToInt64(propUInt32Val);
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return Convert.ToInt64(((TimeSpan)propObjValue).Ticks);
			default:
				return ((IConvertible)propObjValue).ToInt64(provider);
			}
		}

		public TypeCode GetTypeCode()
		{
			switch (propDataType)
			{
			case DataType.Boolean:
				return TypeCode.Boolean;
			case DataType.SByte:
				return TypeCode.SByte;
			case DataType.Int16:
				return TypeCode.Int16;
			case DataType.Int32:
				return TypeCode.Int32;
			case DataType.Int64:
				return TypeCode.Int64;
			case DataType.Byte:
			case DataType.UInt8:
				return TypeCode.Byte;
			case DataType.UInt16:
			case DataType.WORD:
				return TypeCode.UInt16;
			case DataType.UInt32:
			case DataType.DWORD:
				return TypeCode.UInt32;
			case DataType.UInt64:
				return TypeCode.UInt64;
			case DataType.Single:
				return TypeCode.Single;
			case DataType.Double:
				return TypeCode.Double;
			case DataType.String:
				return TypeCode.String;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				return TypeCode.DateTime;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				return TypeCode.DateTime;
			case DataType.Structure:
				return TypeCode.Object;
			default:
				return TypeCode.Empty;
			}
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			if (1 < propArrayLength || DataType.Structure == propDataType)
			{
				return NonSimpleToType(conversionType, provider);
			}
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue is Array)
				{
					if ((bool)((Array)propObjValue).GetValue(0))
					{
						return 1;
					}
				}
				else if ((bool)propObjValue)
				{
					return 1;
				}
				return 0;
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.String:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
				return ((IConvertible)propObjValue).ToType(conversionType, provider);
			case DataType.TimeSpan:
			case DataType.DateTime:
			case DataType.TimeOfDay:
			case DataType.Date:
			case DataType.TOD:
			case DataType.DT:
				return propObjValue;
			default:
				throw new InvalidCastException();
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object value)
		{
			if (value is Value)
			{
				if (((Value)value).DataType == DataType)
				{
					if (propObjValue != null && ((Value)value).propObjValue != null)
					{
						return propObjValue.Equals(((Value)value).propObjValue);
					}
				}
				else
				{
					if (1 == propArrayLength && 1 == ((Value)value).propArrayLength && propObjValue != null && ((Value)value).propObjValue != null)
					{
						return propObjValue.Equals(((Value)value).propObjValue);
					}
					if (1 < propArrayLength && 1 < ((Value)value).propArrayLength && propArrayLength == ((Value)value).propArrayLength)
					{
						for (int i = 0; i < propArrayLength; i++)
						{
							if (this[i] != ((Value)value)[i])
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		public bool Equals(Value value)
		{
			if (value.DataType == DataType)
			{
				if (propObjValue != null && value.propObjValue != null)
				{
					return propObjValue.Equals(value.propObjValue);
				}
			}
			else
			{
				if (1 == propArrayLength && 1 == value.propArrayLength && propObjValue != null && value.propObjValue != null)
				{
					return propObjValue.Equals(value.propObjValue);
				}
				if (1 < propArrayLength && 1 < value.propArrayLength && propArrayLength == value.propArrayLength)
				{
					for (int i = 0; i < propArrayLength; i++)
					{
						if (this[i] != value[i])
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		public bool Equals(bool value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(float value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(double value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(sbyte value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(short value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(int value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(long value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(byte value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(ushort value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(uint value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		public bool Equals(ulong value)
		{
			if (propObjValue == null)
			{
				return false;
			}
			return propObjValue.Equals(value);
		}

		internal void SetArrayLength(int length)
		{
			propArrayLength = length;
		}

		internal void SetMinMax()
		{
			if (propParent.Scaling != null && propParent.Scaling.ScalingType != ScalingType.LimitValues && propParent.Scaling.ScalingType != ScalingType.LimitValuesAndFactor)
			{
				switch (propDataType)
				{
				case (DataType)6:
				case (DataType)11:
				case DataType.TimeSpan:
				case DataType.DateTime:
				case DataType.String:
				case DataType.Structure:
				case DataType.WString:
				case DataType.TimeOfDay:
				case DataType.Date:
				case DataType.Data:
				case DataType.LWORD:
					break;
				case DataType.Byte:
				case DataType.UInt8:
					propParent.Scaling.propMinValue = (byte)0;
					propParent.Scaling.propMaxValue = byte.MaxValue;
					break;
				case DataType.SByte:
					propParent.Scaling.propMinValue = sbyte.MinValue;
					propParent.Scaling.propMaxValue = sbyte.MaxValue;
					break;
				case DataType.Int16:
					propParent.Scaling.propMinValue = short.MinValue;
					propParent.Scaling.propMaxValue = short.MaxValue;
					break;
				case DataType.UInt16:
				case DataType.WORD:
					propParent.Scaling.propMinValue = (ushort)0;
					propParent.Scaling.propMaxValue = ushort.MaxValue;
					break;
				case DataType.Int32:
					propParent.Scaling.propMinValue = int.MinValue;
					propParent.Scaling.propMaxValue = int.MaxValue;
					break;
				case DataType.Int64:
					propParent.Scaling.propMinValue = long.MinValue;
					propParent.Scaling.propMaxValue = long.MaxValue;
					break;
				case DataType.UInt32:
				case DataType.DWORD:
					propParent.Scaling.propMinValue = 0u;
					propParent.Scaling.propMaxValue = uint.MaxValue;
					break;
				case DataType.UInt64:
					propParent.Scaling.propMinValue = 0uL;
					propParent.Scaling.propMaxValue = ulong.MaxValue;
					break;
				case DataType.Single:
					propParent.Scaling.propMinValue = -3.40282347E+38f;
					propParent.Scaling.propMaxValue = 3.40282347E+38f;
					break;
				case DataType.Double:
					propParent.Scaling.propMinValue = -3.40282347E+38f;
					propParent.Scaling.propMaxValue = 3.40282347E+38f;
					break;
				}
			}
		}

		private void PresetDataType(DataType type)
		{
			bool flag = false;
			if (propDataType != 0)
			{
				flag = true;
			}
			if (type == DataType.Unknown)
			{
				propTypePreset = false;
				return;
			}
			propTypePreset = true;
			propDataType = type;
			propArrayMinIndex = 0;
			propArrayMaxIndex = 0;
			switch (type)
			{
			case DataType.SByte:
			case DataType.Byte:
			case DataType.UInt8:
				propArrayLength = 1;
				propTypeLength = 1;
				break;
			case DataType.Int16:
			case DataType.UInt16:
			case DataType.WORD:
				propArrayLength = 1;
				propTypeLength = 2;
				break;
			case DataType.Int32:
			case DataType.UInt32:
			case DataType.Single:
			case DataType.DWORD:
				propArrayLength = 1;
				propTypeLength = 4;
				break;
			case DataType.Int64:
			case DataType.UInt64:
			case DataType.Double:
				propArrayLength = 1;
				propTypeLength = 8;
				break;
			}
			SetMinMax();
			if (flag && propParent.IsConnected && propParent.ConnectionType == ConnectionType.Link)
			{
				propParent.Disconnect(2716u);
				propParent.Connect(ConnectionType.Link, 2718);
			}
		}

		internal bool SetArrayIndex(string typeDescription)
		{
			int num = 0;
			int num2 = typeDescription.IndexOf("VS=");
			int num3 = typeDescription.IndexOf(' ', num2);
			int num4 = num2 + "VS=".Length;
			string text = (-1 != num3) ? typeDescription.Substring(num4, num3 - num4) : typeDescription.Substring(num4);
			string[] array = text.Split(';');
			if (1 == array.Length && array.GetValue(0).ToString().IndexOf("a") == 0)
			{
				array = text.Split(',');
				if (3 == array.Length)
				{
					propArrayMinIndex = Convert.ToInt32(array[1]);
					propArrayMaxIndex = Convert.ToInt32(array[2]);
					propArryOne = true;
					return true;
				}
				if (1 == array.Length)
				{
					propArrayMinIndex = 0;
					propArrayMaxIndex = 0;
					propArryOne = true;
					return true;
				}
			}
			else
			{
				InitializeArrayDimensions();
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array.GetValue(i).ToString().Split(',');
					if ("a".CompareTo(array2.GetValue(0).ToString()) == 0 && -1 != ArrayDimensions.Add(array2))
					{
						num++;
					}
				}
			}
			return false;
		}

		internal bool SetDataType(DataType type)
		{
			bool flag = false;
			if (propDataType != 0)
			{
				flag = true;
			}
			propDataType = type;
			if (propParent == null)
			{
				return flag;
			}
			SetMinMax();
			if (flag && propParent.IsConnected && propParent.ConnectionType == ConnectionType.Link)
			{
				propParent.Disconnect(2716u);
				propParent.Connect(ConnectionType.Link, 2718);
			}
			return flag;
		}

		public static Value operator +(Value value1, Value value2)
		{
			switch (value1.propDataType)
			{
			case DataType.SByte:
				return new Value((sbyte)value1.propObjValue + (byte)value2, value1.Parent);
			case DataType.Int16:
				return new Value((short)value1.propObjValue + (short)value2, value1.Parent);
			case DataType.Int32:
				return new Value((int)value1.propObjValue + (int)value2, value1.Parent);
			case DataType.Int64:
				return new Value((long)value1.propObjValue + (long)value2, value1.Parent);
			case DataType.Byte:
			case DataType.UInt8:
				return new Value((byte)value1.propObjValue + (byte)value2, value1.Parent);
			case DataType.UInt16:
			case DataType.WORD:
				return new Value((ushort)value1.propObjValue + (ushort)value2, value1.Parent);
			case DataType.UInt32:
			case DataType.DWORD:
				return new Value((uint)value1.propObjValue + (uint)value2, value1.Parent);
			case DataType.UInt64:
				return new Value((ulong)value1.propObjValue + (ulong)value2, value1.Parent);
			case DataType.Single:
				return new Value((float)value1.propObjValue + (float)value2, value1.Parent);
			case DataType.Double:
				return new Value((double)value1.propObjValue + (double)value2, value1.Parent);
			case DataType.TimeSpan:
				return new Value((TimeSpan)value1.propObjValue + (TimeSpan)value2, value1.Parent);
			case DataType.TimeOfDay:
			case DataType.TOD:
				return new Value((TimeSpan)value1.propObjValue + (TimeSpan)value2, value1.Parent);
			case DataType.String:
				return new Value(value1.propObjValue.ToString() + value2.propObjValue.ToString(), value1.Parent);
			default:
				throw new InvalidOperationException();
			}
		}

		public static Value operator /(Value value1, Value value2)
		{
			double num = value2;
			switch (value1.propDataType)
			{
			case DataType.SByte:
				return new Value((sbyte)((double)(sbyte)value1.propObjValue / num), value1.Parent);
			case DataType.Int16:
				return new Value((short)((double)(short)value1.propObjValue / num), value1.Parent);
			case DataType.Int32:
				return new Value((int)((double)(int)value1.propObjValue / num), value1.Parent);
			case DataType.Int64:
				return new Value((long)((double)(long)value1.propObjValue / num), value1.Parent);
			case DataType.Byte:
			case DataType.UInt8:
				return new Value((byte)((double)(int)(byte)value1.propObjValue / num), value1.Parent);
			case DataType.UInt16:
			case DataType.WORD:
				return new Value((ushort)((double)(int)(ushort)value1.propObjValue / num), value1.Parent);
			case DataType.UInt32:
			case DataType.DWORD:
				return new Value((uint)((double)(uint)value1.propObjValue / num), value1.Parent);
			case DataType.UInt64:
				return new Value((ulong)((double)(ulong)value1.propObjValue / num), value1.Parent);
			case DataType.Single:
				return new Value((float)((double)(float)value1.propObjValue / num), value1.Parent);
			case DataType.Double:
				return new Value((double)value1.propObjValue / num, value1.Parent);
			default:
				throw new InvalidOperationException();
			}
		}

		public static Value operator *(Value value1, Value value2)
		{
			double num = value2;
			switch (value1.propDataType)
			{
			case DataType.SByte:
				return new Value((sbyte)((double)(sbyte)value1.propObjValue * num), value1.Parent);
			case DataType.Int16:
				return new Value((short)((double)(short)value1.propObjValue * num), value1.Parent);
			case DataType.Int32:
				return new Value((int)((double)(int)value1.propObjValue * num), value1.Parent);
			case DataType.Int64:
				return new Value((long)((double)(long)value1.propObjValue * num), value1.Parent);
			case DataType.Byte:
			case DataType.UInt8:
				return new Value((byte)((double)(int)(byte)value1.propObjValue * num), value1.Parent);
			case DataType.UInt16:
			case DataType.WORD:
				return new Value((ushort)((double)(int)(ushort)value1.propObjValue * num), value1.Parent);
			case DataType.UInt32:
			case DataType.DWORD:
				return new Value((uint)((double)(uint)value1.propObjValue * num), value1.Parent);
			case DataType.UInt64:
				return new Value((ulong)((double)(ulong)value1.propObjValue * num), value1.Parent);
			case DataType.Single:
				return new Value((float)((double)(float)value1.propObjValue * num), value1.Parent);
			case DataType.Double:
				return new Value((double)value1.propObjValue * num, value1.Parent);
			default:
				throw new InvalidOperationException();
			}
		}

		public static Value operator -(Value value1, Value value2)
		{
			switch (value1.propDataType)
			{
			case DataType.SByte:
				return new Value((sbyte)value1.propObjValue - (byte)value2, value1.Parent);
			case DataType.Int16:
				return new Value((short)value1.propObjValue - (short)value2, value1.Parent);
			case DataType.Int32:
				return new Value((int)value1.propObjValue - (int)value2, value1.Parent);
			case DataType.Int64:
				return new Value((long)value1.propObjValue - (long)value2, value1.Parent);
			case DataType.Byte:
			case DataType.UInt8:
				return new Value((byte)value1.propObjValue - (byte)value2, value1.Parent);
			case DataType.UInt16:
			case DataType.WORD:
				return new Value((ushort)value1.propObjValue - (ushort)value2, value1.Parent);
			case DataType.UInt32:
			case DataType.DWORD:
				return new Value((uint)value1.propObjValue - (uint)value2, value1.Parent);
			case DataType.UInt64:
				return new Value((ulong)value1.propObjValue - (ulong)value2, value1.Parent);
			case DataType.Single:
				return new Value((float)value1.propObjValue - (float)value2, value1.Parent);
			case DataType.Double:
				return new Value((double)value1.propObjValue - (double)value2, value1.Parent);
			case DataType.TimeSpan:
				return new Value((TimeSpan)value1.propObjValue - (TimeSpan)value2, value1.Parent);
			case DataType.TimeOfDay:
			case DataType.TOD:
				return new Value((TimeSpan)value1.propObjValue - (TimeSpan)value2, value1.Parent);
			case DataType.DateTime:
			case DataType.DT:
				return new Value((DateTime)value1.propObjValue - (DateTime)value2, value1.Parent);
			case DataType.Date:
				return new Value((DateTime)value1.propObjValue - (DateTime)value2, value1.Parent);
			default:
				throw new InvalidOperationException();
			}
		}

		public static Value operator ++(Value value)
		{
			switch (value.propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.TimeSpan:
			case DataType.DateTime:
			case DataType.TimeOfDay:
			case DataType.Date:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
			case DataType.TOD:
			case DataType.DT:
				return value + 1;
			default:
				throw new InvalidOperationException();
			}
		}

		public static Value operator --(Value value)
		{
			switch (value.propDataType)
			{
			case DataType.Boolean:
			case DataType.SByte:
			case DataType.Int16:
			case DataType.Int32:
			case DataType.Int64:
			case DataType.Byte:
			case DataType.UInt16:
			case DataType.UInt32:
			case DataType.UInt64:
			case DataType.Single:
			case DataType.Double:
			case DataType.TimeSpan:
			case DataType.DateTime:
			case DataType.TimeOfDay:
			case DataType.Date:
			case DataType.WORD:
			case DataType.DWORD:
			case DataType.UInt8:
			case DataType.TOD:
			case DataType.DT:
				return value - 1;
			default:
				throw new InvalidOperationException();
			}
		}

		public static implicit operator bool(Value value)
		{
			if (1 < value.propArrayLength || value.propObjValue == null)
			{
				return value.ToBoolean(null);
			}
			if (value.propObjValue is bool)
			{
				return (bool)value.propObjValue;
			}
			if (value.propObjValue.ToString().CompareTo("0") == 0)
			{
				return false;
			}
			return true;
		}

		public static implicit operator bool[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidCastException();
			}
			bool[] array = new bool[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator sbyte(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToSByte(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0;
				}
				return 1;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (sbyte)(short)value.propObjValue;
			case DataType.Int32:
				return (sbyte)(int)value.propObjValue;
			case DataType.Int64:
				return (sbyte)(long)value.propObjValue;
			case DataType.UInt8:
				return (sbyte)(byte)value.propObjValue;
			case DataType.Byte:
				return (sbyte)(byte)value.propObjValue;
			case DataType.UInt16:
				return (sbyte)(ushort)value.propObjValue;
			case DataType.WORD:
				return (sbyte)(ushort)value.propObjValue;
			case DataType.UInt32:
				return (sbyte)(uint)value.propObjValue;
			case DataType.DWORD:
				return (sbyte)(uint)value.propObjValue;
			case DataType.UInt64:
				return (sbyte)(ulong)value.propObjValue;
			case DataType.Single:
				return (sbyte)(float)value.propObjValue;
			case DataType.Double:
				return (sbyte)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToSByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToSByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToSByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToSByte(((DateTime)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToSByte(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToSByte(((DateTime)value.propObjValue).Ticks);
			case DataType.String:
			{
				string text = value.propObjValue.ToString();
				return (sbyte)text[0];
			}
			default:
				throw new InvalidCastException();
			}
		}

		public static implicit operator sbyte[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidCastException();
			}
			sbyte[] array = new sbyte[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator short(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToInt16(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0;
				}
				return 1;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (short)value.propObjValue;
			case DataType.Int32:
				return (short)(int)value.propObjValue;
			case DataType.Int64:
				return (short)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (short)(ushort)value.propObjValue;
			case DataType.WORD:
				return (short)(ushort)value.propObjValue;
			case DataType.UInt32:
				return (short)(uint)value.propObjValue;
			case DataType.DWORD:
				return (short)(uint)value.propObjValue;
			case DataType.UInt64:
				return (short)(ulong)value.propObjValue;
			case DataType.Single:
				return (short)(float)value.propObjValue;
			case DataType.Double:
				return (short)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToInt16(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToInt16(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToInt16(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToInt16(value.propObjValue.ToString());
			}
		}

		public static implicit operator short[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidCastException();
			}
			short[] array = new short[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator int(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToInt32(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0;
				}
				return 1;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (short)value.propObjValue;
			case DataType.Int32:
				return (int)value.propObjValue;
			case DataType.Int64:
				return (int)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (ushort)value.propObjValue;
			case DataType.WORD:
				return (ushort)value.propObjValue;
			case DataType.UInt32:
				return (int)(uint)value.propObjValue;
			case DataType.DWORD:
				return (ushort)value.propObjValue;
			case DataType.UInt64:
				return (int)(ulong)value.propObjValue;
			case DataType.Single:
				return (int)(float)value.propObjValue;
			case DataType.Double:
				return (int)(double)value.propObjValue;
			case DataType.String:
				return Convert.ToInt32(value.propObjValue.ToString());
			case DataType.TimeSpan:
				return (int)((TimeSpan)value.propObjValue).Ticks;
			case DataType.TimeOfDay:
				return (int)((TimeSpan)value.propObjValue).Ticks;
			case DataType.TOD:
				return (int)((TimeSpan)value.propObjValue).Ticks;
			case DataType.DateTime:
				return (int)((DateTime)value.propObjValue).Ticks;
			case DataType.Date:
				return (int)((DateTime)value.propObjValue).Ticks;
			case DataType.DT:
				return (int)((DateTime)value.propObjValue).Ticks;
			default:
				return Convert.ToInt32(value.propObjValue.ToString());
			}
		}

		public static implicit operator int[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			int[] array = new int[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator long(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToInt64(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0L;
				}
				return 1L;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (short)value.propObjValue;
			case DataType.Int32:
				return (int)value.propObjValue;
			case DataType.Int64:
				return (long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (ushort)value.propObjValue;
			case DataType.WORD:
				return (ushort)value.propObjValue;
			case DataType.UInt32:
				return (uint)value.propObjValue;
			case DataType.DWORD:
				return (uint)value.propObjValue;
			case DataType.UInt64:
				return (long)(ulong)value.propObjValue;
			case DataType.Single:
				return (long)(float)value.propObjValue;
			case DataType.Double:
				return (long)(double)value.propObjValue;
			case DataType.String:
				return Convert.ToInt64(value.propObjValue.ToString());
			case DataType.TimeSpan:
				return Convert.ToInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToInt64(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToInt64(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToInt64(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToInt64(value.propObjValue.ToString());
			}
		}

		public static implicit operator long[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			long[] array = new long[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator byte(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToByte(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0;
				}
				return 1;
			case DataType.SByte:
				return (byte)(sbyte)value.propObjValue;
			case DataType.Int16:
				return (byte)(short)value.propObjValue;
			case DataType.Int32:
				return (byte)(int)value.propObjValue;
			case DataType.Int64:
				return (byte)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (byte)(ushort)value.propObjValue;
			case DataType.WORD:
				return (byte)(ushort)value.propObjValue;
			case DataType.UInt32:
				return (byte)(uint)value.propObjValue;
			case DataType.DWORD:
				return (byte)(uint)value.propObjValue;
			case DataType.UInt64:
				return (byte)(ulong)value.propObjValue;
			case DataType.Single:
				return (byte)(float)value.propObjValue;
			case DataType.Double:
				return (byte)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToByte(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToByte(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToByte(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToByte(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToByte(value.propObjValue.ToString());
			}
		}

		public static implicit operator byte[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			byte[] array = new byte[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator ushort(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToUInt16(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0;
				}
				return 1;
			case DataType.SByte:
				return (ushort)(sbyte)value.propObjValue;
			case DataType.Int16:
				return (ushort)(short)value.propObjValue;
			case DataType.Int32:
				return (ushort)(int)value.propObjValue;
			case DataType.Int64:
				return (ushort)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (ushort)value.propObjValue;
			case DataType.WORD:
				return (ushort)value.propObjValue;
			case DataType.UInt32:
				return (ushort)(uint)value.propObjValue;
			case DataType.DWORD:
				return (ushort)(uint)value.propObjValue;
			case DataType.UInt64:
				return (ushort)(ulong)value.propObjValue;
			case DataType.Single:
				return (ushort)(float)value.propObjValue;
			case DataType.Double:
				return (ushort)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToUInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToUInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToUInt16(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToUInt16(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToUInt16(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToUInt16(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToUInt16(value.propObjValue.ToString());
			}
		}

		public static implicit operator ushort[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			ushort[] array = new ushort[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator uint(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToUInt32(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0u;
				}
				return 1u;
			case DataType.SByte:
				return (uint)(sbyte)value.propObjValue;
			case DataType.Int16:
				return (uint)(short)value.propObjValue;
			case DataType.Int32:
				return (uint)(int)value.propObjValue;
			case DataType.Int64:
				return (uint)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (ushort)value.propObjValue;
			case DataType.WORD:
				return (ushort)value.propObjValue;
			case DataType.UInt32:
				return (uint)value.propObjValue;
			case DataType.DWORD:
				return (uint)value.propObjValue;
			case DataType.UInt64:
				return (uint)(ulong)value.propObjValue;
			case DataType.Single:
				return (uint)(float)value.propObjValue;
			case DataType.Double:
				return (uint)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToUInt32(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToUInt32(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToUInt32(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToUInt32(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToUInt32(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToUInt32(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToUInt32(value.propObjValue.ToString());
			}
		}

		public static implicit operator uint[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			uint[] array = new uint[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator ulong(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToUInt64(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0uL;
				}
				return 1uL;
			case DataType.SByte:
				return (ulong)(sbyte)value.propObjValue;
			case DataType.Int16:
				return (ulong)(short)value.propObjValue;
			case DataType.Int32:
				return (ulong)(int)value.propObjValue;
			case DataType.Int64:
				return (ulong)(long)value.propObjValue;
			case DataType.UInt8:
				return (byte)value.propObjValue;
			case DataType.Byte:
				return (byte)value.propObjValue;
			case DataType.UInt16:
				return (ushort)value.propObjValue;
			case DataType.WORD:
				return (ushort)value.propObjValue;
			case DataType.UInt32:
				return (uint)value.propObjValue;
			case DataType.DWORD:
				return (uint)value.propObjValue;
			case DataType.UInt64:
				return (ulong)value.propObjValue;
			case DataType.Single:
				return (ulong)(float)value.propObjValue;
			case DataType.Double:
				return (ulong)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToUInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToUInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToUInt64(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToUInt64(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToUInt64(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToUInt64(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToUInt64(value.propObjValue.ToString());
			}
		}

		public static implicit operator ulong[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			ulong[] array = new ulong[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator float(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToSingle(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0f;
				}
				return 1f;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (short)value.propObjValue;
			case DataType.Int32:
				return (int)value.propObjValue;
			case DataType.Int64:
				return (long)value.propObjValue;
			case DataType.UInt8:
				return (int)(byte)value.propObjValue;
			case DataType.Byte:
				return (int)(byte)value.propObjValue;
			case DataType.UInt16:
				return (int)(ushort)value.propObjValue;
			case DataType.WORD:
				return (int)(ushort)value.propObjValue;
			case DataType.UInt32:
				return (float)(double)(uint)value.propObjValue;
			case DataType.DWORD:
				return (float)(double)(uint)value.propObjValue;
			case DataType.UInt64:
				return (float)(double)(ulong)value.propObjValue;
			case DataType.Single:
				return (float)value.propObjValue;
			case DataType.Double:
				return (float)(double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToSingle(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToSingle(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToSingle(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToSingle(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToSingle(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToSingle(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToSingle(value.propObjValue.ToString());
			}
		}

		public static implicit operator float[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			float[] array = new float[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator double(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToDouble(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return 0.0;
				}
				return 1.0;
			case DataType.SByte:
				return (sbyte)value.propObjValue;
			case DataType.Int16:
				return (short)value.propObjValue;
			case DataType.Int32:
				return (int)value.propObjValue;
			case DataType.Int64:
				return (long)value.propObjValue;
			case DataType.UInt8:
				return (int)(byte)value.propObjValue;
			case DataType.Byte:
				return (int)(byte)value.propObjValue;
			case DataType.UInt16:
				return (int)(ushort)value.propObjValue;
			case DataType.WORD:
				return (int)(ushort)value.propObjValue;
			case DataType.UInt32:
				return (uint)value.propObjValue;
			case DataType.DWORD:
				return (uint)value.propObjValue;
			case DataType.UInt64:
				return (ulong)value.propObjValue;
			case DataType.Single:
				return (float)value.propObjValue;
			case DataType.Double:
				return (double)value.propObjValue;
			case DataType.TimeSpan:
				return Convert.ToDouble(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return Convert.ToDouble(((TimeSpan)value.propObjValue).Ticks);
			case DataType.TOD:
				return Convert.ToDouble(((TimeSpan)value.propObjValue).Ticks);
			case DataType.DateTime:
				return Convert.ToDouble(((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return Convert.ToDouble(((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return Convert.ToDouble(((DateTime)value.propObjValue).Ticks);
			default:
				return Convert.ToDouble(value.propObjValue.ToString());
			}
		}

		public static implicit operator double[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			double[] array = new double[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator TimeSpan(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToTimeSpan(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return new TimeSpan(0L);
				}
				return new TimeSpan(1L);
			case DataType.SByte:
				return new TimeSpan((sbyte)value.propObjValue);
			case DataType.Int16:
				return new TimeSpan((short)value.propObjValue);
			case DataType.Int32:
				return new TimeSpan((int)value.propObjValue);
			case DataType.Int64:
				return new TimeSpan((int)value.propObjValue);
			case DataType.UInt8:
				return new TimeSpan((byte)value.propObjValue);
			case DataType.Byte:
				return new TimeSpan((byte)value.propObjValue);
			case DataType.UInt16:
				return new TimeSpan((ushort)value.propObjValue);
			case DataType.WORD:
				return new TimeSpan((ushort)value.propObjValue);
			case DataType.UInt32:
				return new TimeSpan((uint)value.propObjValue);
			case DataType.DWORD:
				return new TimeSpan((uint)value.propObjValue);
			case DataType.UInt64:
				return new TimeSpan((uint)value.propObjValue);
			case DataType.Single:
				return new TimeSpan((uint)value.propObjValue);
			case DataType.Double:
				return new TimeSpan((uint)value.propObjValue);
			case DataType.TimeSpan:
				return (TimeSpan)value.propObjValue;
			case DataType.TimeOfDay:
				return (TimeSpan)value.propObjValue;
			case DataType.TOD:
				return (TimeSpan)value.propObjValue;
			case DataType.DateTime:
				return new TimeSpan((uint)((DateTime)value.propObjValue).Ticks);
			case DataType.Date:
				return new TimeSpan((uint)((DateTime)value.propObjValue).Ticks);
			case DataType.DT:
				return new TimeSpan((uint)((DateTime)value.propObjValue).Ticks);
			default:
				throw new InvalidCastException();
			}
		}

		public static implicit operator TimeSpan[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.Structure)
			{
				throw new InvalidCastException();
			}
			TimeSpan[] array = new TimeSpan[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator DateTime(Value value)
		{
			if (value.propArrayLength > 1 || value.propObjValue == null)
			{
				return value.ToDateTime(null);
			}
			switch (value.propDataType)
			{
			case DataType.Boolean:
				if (value.propObjValue.ToString().ToLower().CompareTo("true") != 0)
				{
					return new DateTime(0L);
				}
				return new DateTime(1L);
			case DataType.SByte:
				return new DateTime((sbyte)value.propObjValue);
			case DataType.Int16:
				return new DateTime((short)value.propObjValue);
			case DataType.Int32:
				return new DateTime((int)value.propObjValue);
			case DataType.Int64:
				return new DateTime((int)value.propObjValue);
			case DataType.UInt8:
				return new DateTime((byte)value.propObjValue);
			case DataType.Byte:
				return new DateTime((byte)value.propObjValue);
			case DataType.UInt16:
				return new DateTime((ushort)value.propObjValue);
			case DataType.WORD:
				return new DateTime((ushort)value.propObjValue);
			case DataType.UInt32:
				return new DateTime((uint)value.propObjValue);
			case DataType.DWORD:
				return new DateTime((uint)value.propObjValue);
			case DataType.UInt64:
				return new DateTime((uint)value.propObjValue);
			case DataType.Single:
				return new DateTime((uint)value.propObjValue);
			case DataType.Double:
				return new DateTime((uint)value.propObjValue);
			case DataType.DateTime:
				return (DateTime)value.propObjValue;
			case DataType.Date:
				return (DateTime)value.propObjValue;
			case DataType.DT:
				return (DateTime)value.propObjValue;
			case DataType.TOD:
				return new DateTime((uint)((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeOfDay:
				return new DateTime((uint)((TimeSpan)value.propObjValue).Ticks);
			case DataType.TimeSpan:
				return new DateTime((uint)((TimeSpan)value.propObjValue).Ticks);
			default:
				throw new InvalidCastException();
			}
		}

		public static implicit operator DateTime[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.Structure)
			{
				throw new InvalidCastException();
			}
			DateTime[] array = new DateTime[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i];
			}
			return array;
		}

		public static implicit operator string(Value value)
		{
			if (value.propDataType == DataType.Structure)
			{
				return value.ToString(null);
			}
			return value.ToString();
		}

		public static implicit operator string[](Value value)
		{
			if (value.propArrayLength <= 0 || value.propDataType == DataType.Structure)
			{
				throw new InvalidOperationException();
			}
			string[] array = new string[value.propArrayLength];
			for (int i = 0; i < value.propArrayLength; i++)
			{
				array[i] = value[i].ToString();
			}
			return array;
		}

		public static implicit operator Value(bool value)
		{
			return new Value(value);
		}

		public static implicit operator Value(bool[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(sbyte value)
		{
			return new Value(value);
		}

		public static implicit operator Value(sbyte[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(short value)
		{
			return new Value(value);
		}

		public static implicit operator Value(short[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(int value)
		{
			return new Value(value);
		}

		public static implicit operator Value(int[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(long value)
		{
			return new Value(value);
		}

		public static implicit operator Value(long[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(byte value)
		{
			return new Value(value);
		}

		public static implicit operator Value(byte[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(ushort value)
		{
			return new Value(value);
		}

		public static implicit operator Value(ushort[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(uint value)
		{
			return new Value(value);
		}

		public static implicit operator Value(uint[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(ulong value)
		{
			return new Value(value);
		}

		public static implicit operator Value(ulong[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(float value)
		{
			return new Value(value);
		}

		public static implicit operator Value(float[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(double value)
		{
			return new Value(value);
		}

		public static implicit operator Value(double[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(TimeSpan value)
		{
			return new Value(value);
		}

		public static implicit operator Value(TimeSpan[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(DateTime value)
		{
			return new Value(value);
		}

		public static implicit operator Value(DateTime[] value)
		{
			return new Value(value);
		}

		public static implicit operator Value(string value)
		{
			return new Value(value);
		}

		public static implicit operator Value(string[] value)
		{
			return new Value(value);
		}

		public static bool operator ==(Value value1, Value value2)
		{
			if ((object)value1 == null || (object)value2 == null)
			{
				if ((object)value1 == null && (object)value2 == null)
				{
					return true;
				}
				return false;
			}
			return value1.Equals(value2);
		}

		public static bool operator ==(Value value1, bool value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, float value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, double value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, sbyte value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, short value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, int value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, long value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, byte value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, ushort value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, uint value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator ==(Value value1, ulong value2)
		{
			return value1?.Equals(value2) ?? false;
		}

		public static bool operator !=(Value value1, Value value2)
		{
			if ((object)value1 == null || (object)value2 == null)
			{
				if ((object)value1 == null && (object)value2 == null)
				{
					return false;
				}
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, byte value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, ushort value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, uint value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, ulong value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, bool value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, float value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, double value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, sbyte value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, short value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, int value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		public static bool operator !=(Value value1, long value2)
		{
			if ((object)value1 == null)
			{
				return true;
			}
			return !value1.Equals(value2);
		}

		internal int CalculateArrayOffset(int dim, int offset)
		{
			int result = offset;
			if (propDimensions != null && dim < propDimensions.Count)
			{
				result = offset + Math.Abs(propDimensions[dim].StartIndex);
			}
			return result;
		}

		internal void InitializeArrayDimensions()
		{
			if (propDimensions == null)
			{
				propDimensions = new ArrayDimensionArray();
			}
		}

		internal void InitializeExtendedAttributes()
		{
			propDimensions = null;
			propArryOne = false;
			propIsDerived = 0;
			propDerivedFrom = null;
			propIsEnum = 0;
			if (propEnumerations != null)
			{
				propEnumerations.Clear();
			}
			propEnumerations = null;
			propIsBitString = 0;
		}

		private void SetInByteBuffer(int index, Value value)
		{
			switch (propDataType)
			{
			case (DataType)6:
			case (DataType)11:
			case DataType.Structure:
			case DataType.WString:
			case DataType.Data:
			case DataType.LWORD:
				break;
			case DataType.Boolean:
				propObjValue = value.ToBoolean(null);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				propObjValue = value.ToByte(null);
				break;
			case DataType.SByte:
				propObjValue = value.ToSByte(null);
				break;
			case DataType.Int16:
				propObjValue = value.ToBoolean(null);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				propObjValue = value.ToUInt16(null);
				break;
			case DataType.Int32:
				propObjValue = value.ToInt32(null);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				propObjValue = value.ToUInt32(null);
				break;
			case DataType.Int64:
				propObjValue = value.ToInt64(null);
				break;
			case DataType.UInt64:
				propObjValue = value.ToUInt64(null);
				break;
			case DataType.Single:
				propObjValue = value.ToSingle(null);
				break;
			case DataType.Double:
				propObjValue = value.ToDouble(null);
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				propObjValue = value.ToTimeSpan(null);
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				propObjValue = value.ToDateTime(null);
				break;
			case DataType.String:
				propObjValue = value.ToString(null);
				break;
			}
		}

		private Value GetFromByteBuffer(params int[] indices)
		{
			if (!IsOfTypeArray)
			{
				return this;
			}
			if (propByteField == null)
			{
				propByteField = new byte[DataSize];
			}
			if (IntPtr.Zero == pData)
			{
				pData = PviMarshal.AllocHGlobal(DataSize);
				propHasOwnDataPtr = true;
			}
			return new Value(this, indices);
		}

		public string ToIECString()
		{
			return ToIECString(10);
		}

		public string ToIECString(int baseOfRepresentation)
		{
			if (IsPG2000String)
			{
				return BinaryToAnsiString();
			}
			return ConvertToIECString(baseOfRepresentation);
		}

		private string ConvertToIECString(int baseOfRepresentation)
		{
			if (propObjValue != null)
			{
				switch (baseOfRepresentation)
				{
				case 2:
					return ToIECBinaryString();
				case 8:
					return ToIECOctalString();
				case 16:
					return ToIECHexadecimalString();
				default:
					return ToIECDecimalString();
				}
			}
			switch (baseOfRepresentation)
			{
			case 2:
				return ToIECBinaryStringEx();
			case 8:
				return ToIECOctalStringEx();
			case 16:
				return ToIECHexadecimalStringEx();
			default:
				return ToIECDecimalStringEx();
			}
		}

		private string ToIECBinaryString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue.ToString().ToLower().CompareTo("0") == 0 || propObjValue.ToString().ToLower().CompareTo("false") == 0)
				{
					stringBuilder.Append("+2#0");
				}
				else
				{
					stringBuilder.Append("+2#1");
				}
				break;
			case DataType.SByte:
				stringBuilder.Append((0 > (sbyte)propObjValue) ? "-2#" : "+2#");
				stringBuilder.Append(Convert.ToString((sbyte)propObjValue, 2).PadLeft(8, '0'));
				break;
			case DataType.Int16:
				stringBuilder.Append((0 > (short)propObjValue) ? "-2#" : "+2#");
				stringBuilder.Append(Convert.ToString((short)propObjValue, 2).PadLeft(16, '0'));
				break;
			case DataType.Int32:
				stringBuilder.Append((0 > (int)propObjValue) ? "-2#" : "+2#");
				stringBuilder.Append(Convert.ToString((int)propObjValue, 2).PadLeft(32, '0'));
				break;
			case DataType.Int64:
				stringBuilder.Append((0 > (long)propObjValue) ? "-2#" : "+2#");
				stringBuilder.Append(Convert.ToString((long)propObjValue, 2).PadLeft(64, '0'));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				stringBuilder.Append("+2#");
				stringBuilder.Append(Convert.ToString((byte)propObjValue, 2).PadLeft(8, '0'));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				stringBuilder.Append("+2#");
				stringBuilder.Append(Convert.ToString((ushort)propObjValue, 2).PadLeft(16, '0'));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				stringBuilder.Append("+2#");
				stringBuilder.Append(Convert.ToString((uint)propObjValue, 2).PadLeft(32, '0'));
				break;
			case DataType.UInt64:
			{
				stringBuilder.Append("+2#");
				string empty = string.Empty;
				byte[] array = PviMarshal.UInt64ToBytes((ulong)propObjValue);
				empty += Convert.ToString(array[0], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[1], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[2], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[3], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[4], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[5], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[6], 2).PadLeft(8, '0');
				empty += Convert.ToString(array[7], 2).PadLeft(8, '0');
				empty = empty.PadLeft(64, '0');
				stringBuilder.Append(empty);
				break;
			}
			case DataType.Single:
				throw new NotSupportedException();
			case DataType.Double:
				throw new NotSupportedException();
			case DataType.String:
				throw new NotSupportedException();
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				stringBuilder.Append("+2#");
				stringBuilder.Append(Convert.ToString(((TimeSpan)propObjValue).Ticks, 2).PadLeft(32, '0'));
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				stringBuilder.Append("+2#");
				stringBuilder.Append(Convert.ToString(((DateTime)propObjValue).Ticks, 2).PadLeft(32, '0'));
				break;
			default:
				stringBuilder.Append("???");
				break;
			}
			return stringBuilder.ToString();
		}

		private string ToIECOctalString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue.ToString().ToLower().CompareTo("0") == 0 || propObjValue.ToString().ToLower().CompareTo("false") == 0)
				{
					stringBuilder.Append("+8#0");
				}
				else
				{
					stringBuilder.Append("+8#1");
				}
				break;
			case DataType.SByte:
				stringBuilder.Append((0 > (sbyte)propObjValue) ? "-8#" : "+8#");
				stringBuilder.Append(Convert.ToString((sbyte)propObjValue, 8).PadLeft(3, '0'));
				break;
			case DataType.Int16:
				stringBuilder.Append((0 > (short)propObjValue) ? "-8#" : "+8#");
				stringBuilder.Append(Convert.ToString((short)propObjValue, 8).PadLeft(6, '0'));
				break;
			case DataType.Int32:
				stringBuilder.Append((0 > (int)propObjValue) ? "-8#" : "+8#");
				stringBuilder.Append(Convert.ToString((int)propObjValue, 8).PadLeft(12, '0'));
				break;
			case DataType.Int64:
				stringBuilder.Append((0 > (long)propObjValue) ? "-8#" : "+8#");
				stringBuilder.Append(Convert.ToString((long)propObjValue, 8).PadLeft(24, '0'));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				stringBuilder.Append("+8#");
				stringBuilder.Append(Convert.ToString((byte)propObjValue, 8).PadLeft(3, '0'));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				stringBuilder.Append("+8#");
				stringBuilder.Append(Convert.ToString((ushort)propObjValue, 8).PadLeft(6, '0'));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				stringBuilder.Append("+8#");
				stringBuilder.Append(Convert.ToString((uint)propObjValue, 8).PadLeft(12, '0'));
				break;
			case DataType.UInt64:
			{
				stringBuilder.Append("+8#");
				string empty = string.Empty;
				byte[] array = PviMarshal.UInt64ToBytes((ulong)propObjValue);
				empty += Convert.ToString(array[0], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[1], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[2], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[3], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[4], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[5], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[6], 8).PadLeft(3, '0');
				empty += Convert.ToString(array[7], 8).PadLeft(3, '0');
				empty = empty.PadLeft(24, '0');
				stringBuilder.Append(empty);
				break;
			}
			case DataType.Single:
				throw new NotSupportedException();
			case DataType.Double:
				throw new NotSupportedException();
			case DataType.String:
				throw new NotSupportedException();
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				stringBuilder.Append("+8#");
				stringBuilder.Append(Convert.ToString(((TimeSpan)propObjValue).Ticks, 8).PadLeft(12, '0'));
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				stringBuilder.Append("+8#");
				stringBuilder.Append(Convert.ToString(((DateTime)propObjValue).Ticks, 8).PadLeft(12, '0'));
				break;
			default:
				stringBuilder.Append("???");
				break;
			}
			return stringBuilder.ToString();
		}

		private string ToIECHexadecimalString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (propDataType)
			{
			case DataType.Boolean:
				if (propObjValue.ToString().ToLower().CompareTo("0") == 0 || propObjValue.ToString().ToLower().CompareTo("false") == 0)
				{
					stringBuilder.Append("+16#0");
				}
				else
				{
					stringBuilder.Append("+16#1");
				}
				break;
			case DataType.SByte:
				stringBuilder.Append((0 > (sbyte)propObjValue) ? "-16#" : "+16#");
				stringBuilder.Append(Convert.ToString((sbyte)propObjValue, 16).ToUpper().PadLeft(2, '0'));
				break;
			case DataType.Int16:
				stringBuilder.Append((0 > (short)propObjValue) ? "-16#" : "+16#");
				stringBuilder.Append(Convert.ToString((short)propObjValue, 16).PadLeft(4, '0'));
				break;
			case DataType.Int32:
				stringBuilder.Append((0 > (int)propObjValue) ? "-16#" : "+16#");
				stringBuilder.Append(Convert.ToString((int)propObjValue, 16).PadLeft(8, '0'));
				break;
			case DataType.Int64:
				stringBuilder.Append((0 > (long)propObjValue) ? "-16#" : "+16#");
				stringBuilder.Append(Convert.ToString((long)propObjValue, 16).ToUpper().PadLeft(16, '0'));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				stringBuilder.Append("+16#");
				stringBuilder.Append(Convert.ToString((byte)propObjValue, 16).ToUpper().PadLeft(2, '0'));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				stringBuilder.Append("+16#");
				stringBuilder.Append(Convert.ToString((ushort)propObjValue, 16).ToUpper().PadLeft(4, '0'));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				stringBuilder.Append("+16#");
				stringBuilder.Append(Convert.ToString((uint)propObjValue, 16).ToUpper().PadLeft(8, '0'));
				break;
			case DataType.UInt64:
			{
				stringBuilder.Append("+16#");
				string empty = string.Empty;
				byte[] array = PviMarshal.UInt64ToBytes((ulong)propObjValue);
				empty += Convert.ToString(array[0], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[1], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[2], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[3], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[4], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[5], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[6], 16).ToUpper().PadLeft(2, '0');
				empty += Convert.ToString(array[7], 16).ToUpper().PadLeft(2, '0');
				empty = empty.PadLeft(16, '0');
				stringBuilder.Append(empty);
				break;
			}
			case DataType.Single:
				throw new NotSupportedException();
			case DataType.Double:
				throw new NotSupportedException();
			case DataType.String:
				throw new NotSupportedException();
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				stringBuilder.Append("+16#");
				stringBuilder.Append(Convert.ToString(((TimeSpan)propObjValue).Ticks, 16).ToUpper().PadLeft(8, '0'));
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				stringBuilder.Append("+16#");
				stringBuilder.Append(Convert.ToString(((DateTime)propObjValue).Ticks, 16).ToUpper().PadLeft(8, '0'));
				break;
			default:
				stringBuilder.Append("???");
				break;
			}
			return stringBuilder.ToString();
		}

		private string ToIECDecimalString()
		{
			if (propDataType == DataType.TimeSpan)
			{
				int num = 0;
				string text = "T#";
				if (0 < ((TimeSpan)propObjValue).Days)
				{
					text = text + ((TimeSpan)propObjValue).Days.ToString("00") + "d";
					num++;
				}
				if (0 < ((TimeSpan)propObjValue).Hours || 0 < ((TimeSpan)propObjValue).Minutes || 0 < ((TimeSpan)propObjValue).Seconds || 0 < ((TimeSpan)propObjValue).Milliseconds)
				{
					if (0 < num)
					{
						text += "_";
					}
					text = text + ((TimeSpan)propObjValue).Hours.ToString("00") + "h";
					num++;
				}
				if (0 < ((TimeSpan)propObjValue).Minutes || 0 < ((TimeSpan)propObjValue).Seconds || 0 < ((TimeSpan)propObjValue).Milliseconds)
				{
					if (0 < num)
					{
						text += "_";
					}
					text = text + ((TimeSpan)propObjValue).Minutes.ToString("00") + "m";
					num++;
				}
				if (0 < ((TimeSpan)propObjValue).Seconds || 0 < ((TimeSpan)propObjValue).Milliseconds)
				{
					if (0 < num)
					{
						text += "_";
					}
					text = text + ((TimeSpan)propObjValue).Seconds.ToString("00") + "s";
				}
				if (0 < ((TimeSpan)propObjValue).Milliseconds)
				{
					if (0 < num)
					{
						text += "_";
					}
					text = text + ((TimeSpan)propObjValue).Milliseconds.ToString("000") + "ms";
				}
				else if (0.0 == ((TimeSpan)propObjValue).TotalMilliseconds)
				{
					text += "0ms";
				}
				return text;
			}
			if (propDataType == DataType.TimeOfDay || propDataType == DataType.TOD)
			{
				string text2 = "TOD#";
				string text3 = text2;
				text2 = text3 + ((TimeSpan)propObjValue).Hours.ToString("00") + ":" + ((TimeSpan)propObjValue).Minutes.ToString("00") + ":" + ((TimeSpan)propObjValue).Seconds.ToString("00");
				if (0 < ((TimeSpan)propObjValue).Milliseconds)
				{
					text2 = text2 + "." + ((TimeSpan)propObjValue).Milliseconds.ToString("000");
				}
				return text2;
			}
			if (propDataType == DataType.DateTime || propDataType == DataType.DT)
			{
				string text4 = "DT#";
				string text5 = text4;
				text4 = text5 + ((DateTime)propObjValue).Year.ToString("0000") + "-" + ((DateTime)propObjValue).Month.ToString("00") + "-" + ((DateTime)propObjValue).Day.ToString("00") + "-" + ((DateTime)propObjValue).Hour.ToString("00") + ":" + ((DateTime)propObjValue).Minute.ToString("00") + ":" + ((DateTime)propObjValue).Second.ToString("00");
				if (0 < ((DateTime)propObjValue).Millisecond)
				{
					text4 = text4 + "." + ((DateTime)propObjValue).Millisecond.ToString("000");
				}
				return text4;
			}
			if (propDataType == DataType.Date)
			{
				string text6 = "D#";
				string text7 = text6;
				return text7 + ((DateTime)propObjValue).Year.ToString("0000") + "-" + ((DateTime)propObjValue).Month.ToString("00") + "-" + ((DateTime)propObjValue).Day.ToString("00");
			}
			return ToString(null);
		}

		private string ToIECBinaryStringEx()
		{
			return ToString(null);
		}

		private string ToIECOctalStringEx()
		{
			return ToString(null);
		}

		private string ToIECHexadecimalStringEx()
		{
			return ToString(null);
		}

		private string ToIECDecimalStringEx()
		{
			return ToString(null);
		}

		internal void SetDerivation(DerivationBase derivation)
		{
			DerivationBase derivationBase = propDerivedFrom;
			DerivationBase derivationBase2 = null;
			if (propDerivedFrom == null)
			{
				propDerivedFrom = derivation;
				return;
			}
			for (derivationBase2 = propDerivedFrom.DerivedFrom; derivationBase2 != null; derivationBase2 = derivationBase.DerivedFrom)
			{
				derivationBase = derivationBase2;
			}
			derivationBase.SetDerivation(derivation);
		}

		internal void Clone(Value cloneValue)
		{
			propDisposed = cloneValue.propDisposed;
			propArryOne = cloneValue.propArryOne;
			propDimensions = cloneValue.propDimensions;
			propIsEnum = cloneValue.propIsEnum;
			propIsDerived = cloneValue.propIsDerived;
			propDerivedFrom = cloneValue.propDerivedFrom;
			propIsBitString = cloneValue.propIsBitString;
			propEnumerations = cloneValue.propEnumerations;
			propArrayLength = cloneValue.propArrayLength;
			propDataSize = cloneValue.propDataSize;
			propDataType = cloneValue.propDataType;
			isAssigned = cloneValue.isAssigned;
			propTypeLength = cloneValue.propTypeLength;
			pData = cloneValue.pData;
			propArrayMaxIndex = cloneValue.propArrayMaxIndex;
			propArrayMinIndex = cloneValue.propArrayMinIndex;
			propByteField = null;
			if (cloneValue.propByteField != null)
			{
				propByteField = (byte[])cloneValue.propByteField.Clone();
			}
			propByteOffset = cloneValue.propByteOffset;
			propDimensions = null;
			if (cloneValue.propDimensions != null)
			{
				propDimensions = cloneValue.propDimensions.Clone();
			}
			propEnumerations = null;
			if (cloneValue.propEnumerations != null)
			{
				propEnumerations = cloneValue.propEnumerations.Clone();
			}
			propHasOwnDataPtr = cloneValue.propHasOwnDataPtr;
			propTypePreset = cloneValue.propTypePreset;
			propUInt32Val = cloneValue.propUInt32Val;
		}

		internal unsafe void UpdateByteField(IntPtr pData, uint dataLen, int ownerOffset)
		{
			int num = 0;
			if (propByteField == null)
			{
				return;
			}
			byte* ptr = (byte*)pData.ToPointer() + ownerOffset;
			if (ArrayLength > 0)
			{
				for (num = 0; num < propByteField.Length && num < dataLen - ownerOffset; num++)
				{
					propByteField[num] = ptr[num];
				}
			}
		}
	}
}
