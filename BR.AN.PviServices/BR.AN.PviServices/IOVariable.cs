using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	internal class IOVariable : Variable
	{
		private Cpu propCpu;

		private IOVariableTypes propIOType;

		private bool propForce;

		private bool propStatusGot;

		public IOVariableTypes IOType => propIOType;

		internal bool Force
		{
			get
			{
				return propForce;
			}
			set
			{
				propForce = value;
				if (propForce)
				{
					WriteRequest(Service.hPvi, propLinkId, AccessTypes.ForceOn, IntPtr.Zero, 0, 815u);
				}
				else
				{
					WriteRequest(Service.hPvi, propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 816u);
				}
			}
		}

		public event PviValueEventHandler StatusChanged;

		public IOVariable(Cpu cpu, string ioName, IOVariableTypes ioType)
			: base(cpu)
		{
			propStatusGot = false;
			propForce = false;
			propIOType = ioType;
			propCpu = cpu;
			switch (ioType)
			{
			case IOVariableTypes.VALUE:
				propName = "C+" + ioName;
				break;
			case IOVariableTypes.PHYSICAL:
				propName = "P+" + ioName;
				break;
			default:
				propName = "F+" + ioName;
				break;
			}
			Initialize(cpu, cpu, expandMembers: true, automaticWrite: true);
			Init(propName);
		}

		internal override int Disconnect(uint internalAction, bool noResponse)
		{
			propStatusGot = false;
			return base.Disconnect(internalAction, noResponse);
		}

		private int InternalWriteIOValue()
		{
			int num = 0;
			num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, propPviValue.pData, propPviValue.propDataSize, 818u);
			if (num != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
			}
			return num;
		}

		internal void WriteIOValue(Value val)
		{
			if (IsConnected)
			{
				Value value = val;
				if (base.Convert != null && propPviValue.propObjValue != null)
				{
					value = base.Convert.ValueToPviValue(val);
				}
				switch (propPviValue.propDataType)
				{
				case DataType.Boolean:
					ResizePviDataPtr(1);
					if (value == value2: false || value.ToInt32(null) == 0)
					{
						Marshal.WriteByte(propPviValue.pData, 0);
					}
					else
					{
						Marshal.WriteByte(propPviValue.pData, 1);
					}
					break;
				case DataType.SByte:
					if (CastModes.PG2000String == (base.CastMode & CastModes.PG2000String) && 1 < base.Value.ArrayLength)
					{
						propPviValue.Assign(value.ToString());
						break;
					}
					ResizePviDataPtr(1);
					Marshal.WriteByte(propPviValue.pData, (byte)(sbyte)value);
					break;
				case DataType.Int16:
					ResizePviDataPtr(2);
					Marshal.WriteInt16(propPviValue.pData, value);
					break;
				case DataType.Int32:
					ResizePviDataPtr(4);
					Marshal.WriteInt32(propPviValue.pData, value);
					break;
				case DataType.Int64:
					ResizePviDataPtr(8);
					PviMarshal.WriteInt64(propPviValue.pData, value);
					break;
				case DataType.Byte:
				case DataType.UInt8:
					if (CastModes.PG2000String == (base.CastMode & CastModes.PG2000String) && 1 < base.Value.ArrayLength)
					{
						propPviValue.Assign(value.ToString());
						break;
					}
					ResizePviDataPtr(1);
					Marshal.WriteByte(propPviValue.pData, value);
					break;
				case DataType.UInt16:
				case DataType.WORD:
					ResizePviDataPtr(2);
					Marshal.WriteInt16(propPviValue.pData, (short)(ushort)value);
					break;
				case DataType.UInt32:
				case DataType.DWORD:
					ResizePviDataPtr(4);
					Marshal.WriteInt32(propPviValue.pData, (int)(uint)value);
					break;
				case DataType.UInt64:
					ResizePviDataPtr(8);
					PviMarshal.WriteInt64(propPviValue.pData, (long)(ulong)value);
					break;
				case DataType.Single:
					ResizePviDataPtr(4);
					Marshal.Copy(new float[1]
					{
						value
					}, 0, propPviValue.pData, 1);
					break;
				case DataType.Double:
					ResizePviDataPtr(8);
					Marshal.Copy(new double[1]
					{
						value
					}, 0, propPviValue.pData, 1);
					break;
				}
				InternalWriteIOValue();
			}
			else
			{
				if (Service.WaitForParentConnection)
				{
					base.Requests |= Actions.SetValue;
				}
				if (base.Convert != null)
				{
					propInternalValue = base.Convert.ValueToPviValue(val);
				}
				else
				{
					propInternalValue = val;
				}
			}
		}

		protected override string GetLinkParameters(ConnectionType conType, string dt, string fs, string lp, string va, string cm, string vL, string vN)
		{
			switch (propIOType)
			{
			case IOVariableTypes.VALUE:
				return "EV=sedf";
			case IOVariableTypes.PHYSICAL:
				return "EV=edf";
			default:
				return "EV=sedf";
			}
		}

		protected override string GetEventMaskParameters(ConnectionType conType, bool useParamMarker)
		{
			string text = "";
			if (useParamMarker)
			{
				text = "EV=";
			}
			if (ConnectionType.Create != conType)
			{
				text = ((!Service.UserTagEvents) ? (text + "elfs") : (text + "eulfs"));
			}
			if ((!Service.IsStatic || ConnectionType.Link == conType) && propActive)
			{
				text += "d";
			}
			return text;
		}

		protected override string GetObjectParameters(string rf, string hy, string at, string fs, string ut, string dt, string vL, string vN)
		{
			switch (propIOType)
			{
			case IOVariableTypes.VALUE:
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					return $"\"{base.Name}\" AT=r RF={base.RefreshTime}";
				}
				return $"\"{propCpu.LinkName}\"/\"{base.Name}\" AT=r RF={base.RefreshTime}";
			case IOVariableTypes.PHYSICAL:
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					return $"\"{base.Name}\" AT=r RF={base.RefreshTime}";
				}
				return $"\"{propCpu.LinkName}\"/\"{base.Name}\" AT=r RF={base.RefreshTime}";
			default:
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					return $"\"{base.Name}\" RF={base.RefreshTime}";
				}
				return $"\"{propCpu.LinkName}\"/\"{base.Name}\" RF={base.RefreshTime}";
			}
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			string text = "";
			switch (eventType)
			{
			case EventTypes.Status:
			{
				if (errorCode != 0 || dataLen == 0)
				{
					break;
				}
				text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				bool flag = propForce;
				int num = text.IndexOf("\0");
				if (-1 != num)
				{
					text = text.Substring(0, num);
				}
				Variable.GetForceStatus(text, ref propForce);
				OnStatusChanged(text);
				if (IsConnected)
				{
					if (!flag && propForce)
					{
						OnForcedOn(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.IODataPointForceOn, Service));
					}
					else if (flag && !propForce)
					{
						OnForcedOff(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.IODataPointForceOff, Service));
					}
				}
				else if (propPviValue.DataType == DataType.Unknown)
				{
					Read_FormatEX(propLinkId);
				}
				else if (ConnectionStates.Connecting == propConnectionState)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				}
				break;
			}
			case EventTypes.Data:
				UpdateValueData(pData, dataLen, errorCode);
				break;
			case EventTypes.Dataform:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				if (IOVariableTypes.PHYSICAL == IOType || IOVariableTypes.FORCE == IOType || IOVariableTypes.VALUE == IOType)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				}
				break;
			default:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				break;
			}
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			switch (accessType)
			{
			case PVIWriteAccessTypes.ForceOn:
				if (errorCode == 0 && 1 == option)
				{
					OnForcedOn(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.IODataPointForceOn, Service));
				}
				break;
			case PVIWriteAccessTypes.ForceOff:
				if (errorCode == 0 && 2 == option)
				{
					OnForcedOff(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.IODataPointForceOff, Service));
				}
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		protected override void OnValueChanged(VariableEventArgs e)
		{
			Fire_ValueChanged(this, e);
		}

		protected override void OnValueWritten(PviEventArgs e)
		{
			Fire_ValueWritten(this, e);
		}

		protected override void OnValueRead(PviEventArgs e)
		{
			Fire_ValueRead(this, e);
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (errorCode == 0 && PVIReadAccessTypes.State == accessType)
			{
				OnPviEvent(errorCode, EventTypes.Status, dataState, pData, dataLen, option);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		protected override void OnActivated(PviEventArgs e)
		{
			if (Actions.FireActivated == (base.Requests & Actions.FireActivated))
			{
				base.Requests &= ~Actions.FireActivated;
				Fire_Activated(this, e);
				if (propPviValue.DataType == DataType.Unknown || DataType.Structure == propPviValue.DataType || 1 < propPviValue.ArrayLength)
				{
					Read_FormatEX(propLinkId);
				}
			}
		}

		protected override void OnDeactivated(PviEventArgs e)
		{
			Fire_Deactivated(this, e);
		}

		protected virtual void OnStatusChanged(string statutsText)
		{
			propStatusGot = true;
			if (this.StatusChanged != null)
			{
				this.StatusChanged(this, statutsText);
			}
		}

		protected override void OnConnected(PviEventArgs e)
		{
			if (Fire_Connected(e))
			{
				CheckActiveRequests(e);
				if (!propStatusGot && IOVariableTypes.VALUE == IOType)
				{
					Read_State(base.LinkId, 557u);
				}
				if (!base.Active)
				{
					DeactivateInternal();
				}
			}
		}
	}
}
