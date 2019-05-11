using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class TracePoint : Base
	{
		private TraceVariableCollection propTraceVariables;

		private ulong propOffset;

		private Variable.ROIoptions propROI;

		public TraceVariableCollection TraceVariables => propTraceVariables;

		public override string FullName
		{
			get
			{
				if (base.Name != null)
				{
					if (propParent != null)
					{
						return propParent.FullName + "." + base.Name;
					}
					return base.Name;
				}
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return Parent.PviPathName + "/\"" + propName + "\" OT=Pvar";
				}
				return Parent.PviPathName;
			}
		}

		[CLSCompliant(false)]
		public ulong Offset
		{
			get
			{
				return propOffset;
			}
			set
			{
				propOffset = value;
			}
		}

		public Variable.ROIoptions RuntimeObjectIndex
		{
			get
			{
				return propROI;
			}
			set
			{
				propROI = value;
			}
		}

		public event TraceDataEventHandler TraceDataChanged;

		public TracePoint(Task task, string name)
			: base(task, name)
		{
			propROI = Variable.ROIoptions.OFF;
			if (0 < task.TracePoints.Contains(name))
			{
				throw new ArgumentException("There exists already a Tracepoint with the same name!", name);
			}
			propTraceVariables = new TraceVariableCollection(this);
			task.TracePoints.Add(this);
			propOffset = 0uL;
		}

		public int AddTraceVariable(string nameOfTraceVariable)
		{
			return propTraceVariables.Add(nameOfTraceVariable);
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				if (disposing)
				{
					propTraceVariables.Dispose();
					propTraceVariables = null;
				}
				base.Dispose(disposing, removeFromCollection);
			}
		}

		private TraceDataCollection UpdateTraceData(IntPtr pData, uint dataLength)
		{
			int num = 0;
			int num2 = 0;
			TraceDataCollection traceDataCollection = null;
			TracePointVariable tracePointVariable = null;
			int[] dataDest = new int[2];
			if (IntPtr.Zero != pData && propTraceVariables != null)
			{
				traceDataCollection = new TraceDataCollection();
				if (dataLength < 8)
				{
					byte[] array = new byte[dataLength];
					Marshal.Copy(pData, array, 0, (int)dataLength);
					uint dataType = array[0];
					traceDataCollection.Add(new TraceData(array, (IECDataTypes)dataType));
				}
				else
				{
					while (num < dataLength)
					{
						PviMarshal.Copy(pData, num, ref dataDest, 2);
						num += 8;
						uint dataType = PviMarshal.toUInt32(dataDest.GetValue(0));
						byte[] array;
						if (dataType > 25)
						{
							dataType = 0u;
							array = new byte[1]
							{
								0
							};
						}
						else
						{
							uint num3 = PviMarshal.toUInt32(dataDest.GetValue(1));
							if (num3 <= int.MaxValue)
							{
								int num4 = int.Parse(num3.ToString());
								array = new byte[num4];
								PviMarshal.Copy(pData, num, ref array, num4);
								num += num4;
							}
							else
							{
								dataType = 0u;
								array = new byte[1]
								{
									0
								};
							}
						}
						traceDataCollection.Add(new TraceData(array, (IECDataTypes)dataType));
						propTraceVariables[num2]?.SetDataBytes((int)dataType, array);
						num2++;
					}
				}
			}
			return traceDataCollection;
		}

		public override void Connect()
		{
			string text = "";
			string text2 = "";
			propReturnValue = 0;
			if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
			{
				Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.VariableConnect, Service));
			}
			else if (ConnectionStates.Connecting != propConnectionState)
			{
				propLinkParam = "EV=eudf";
				propObjectParam = "CD=" + GetConnectionDescription();
				if (propROI != 0)
				{
					string[] array = new string[5]
					{
						"\"/RO=",
						propAddress,
						" /ROI=",
						null,
						null
					};
					string[] array2 = array;
					int num = (int)propROI;
					array2[3] = num.ToString();
					array[4] = "\"";
					text = string.Concat(array);
					text2 = "\"" + propAddress + "\"";
					propObjectParam = propObjectParam.Replace(text2, text);
				}
				string linkName = base.LinkName;
				propConnectionState = ConnectionStates.Connecting;
				propReturnValue = XCreateRequest(Service.hPvi, linkName, ObjectType.POBJ_PVAR, propObjectParam, 550u, propLinkParam, 501u);
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(propName, propAddress, propReturnValue, Service.Language, Action.VariableConnect, Service));
				}
			}
		}

		protected override string GetConnectionDescription()
		{
			string str = "\"TP+";
			TracePointVariable tracePointVariable = null;
			if (propTraceVariables.Count == 0)
			{
				propTraceVariables.Add(base.Name);
			}
			str += propOffset.ToString();
			for (int i = 0; i < propTraceVariables.Count; i++)
			{
				tracePointVariable = propTraceVariables[i];
				str += ";";
				str += tracePointVariable.Name;
			}
			str += "\"";
			return str + " AT=e";
		}

		public override void Disconnect()
		{
			propReturnValue = 0;
			Disconnect(931u, noResponse: false);
		}

		public override void Disconnect(bool noResponse)
		{
			propReturnValue = 0;
			Disconnect(931u, noResponse);
		}

		internal int Disconnect(uint internalAction, bool noResponse)
		{
			int result = 12004;
			propNoDisconnectedEvent = noResponse;
			propConnectionState = ConnectionStates.Disconnecting;
			if (propLinkId != 0)
			{
				if (Service != null)
				{
					if (propNoDisconnectedEvent)
					{
						result = Unlink();
						propConnectionState = ConnectionStates.Unininitialized;
					}
					else
					{
						result = UnlinkRequest(internalAction);
					}
				}
				else
				{
					propLinkId = 0u;
					result = 0;
				}
			}
			if (propNoDisconnectedEvent)
			{
				propConnectionState = ConnectionStates.Unininitialized;
			}
			return result;
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int propErrorCode = propErrorCode;
			propErrorCode = errorCode;
			if (errorCode != 0 && EventTypes.Error != eventType)
			{
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				return;
			}
			switch (eventType)
			{
			case EventTypes.Error:
				if (errorCode != 0)
				{
					if (IsConnected)
					{
						OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TracePointDisConnect, Service));
					}
					else if (ConnectionStates.Connecting == propConnectionState)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TracePointConnect, Service));
					}
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ErrorEvent, Service));
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TracePointConnect, Service));
				}
				break;
			case EventTypes.Dataform:
				if (!IsConnected && ConnectionStates.Connecting == propConnectionState)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TracePointConnect, Service));
				}
				break;
			case EventTypes.Data:
				OnTraceDataChanged(errorCode, Action.TracePointDataChanged, pData, dataLen);
				break;
			default:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				break;
			}
		}

		[CLSCompliant(false)]
		protected virtual void OnTraceDataChanged(int error, Action action, IntPtr pData, uint dataLength)
		{
			TraceDataCollection traceDataCollection = null;
			traceDataCollection = UpdateTraceData(pData, dataLength);
			if (this.TraceDataChanged != null)
			{
				this.TraceDataChanged(this, new TraceDataEventArgs(base.Name, base.Address, error, Service.Language, action, traceDataCollection));
			}
			traceDataCollection.Dispose();
			traceDataCollection = null;
		}
	}
}
