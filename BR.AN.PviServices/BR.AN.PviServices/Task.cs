using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Task : Module
	{
		internal TracePointCollection propTracePoints;

		internal VariableCollection propVariables;

		internal VariableCollection propGlobals;

		private bool propTracePoints_Enabled;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public TracePointCollection TracePoints
		{
			get
			{
				return propTracePoints;
			}
		}

		public VariableCollection Variables => propVariables;

		public VariableCollection Globals => propGlobals;

		public override string FullName
		{
			get
			{
				if (propName != null && 0 < base.Name.Length)
				{
					if (Parent != null)
					{
						return Parent.FullName + "." + base.Name;
					}
					return base.Name;
				}
				if (Parent != null)
				{
					return Parent.FullName;
				}
				return base.Name;
			}
		}

		public override string PviPathName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					if (Parent != null)
					{
						return Parent.PviPathName + "/\"" + propName + "\" OT=Task";
					}
					return "\"" + propName + "\" OT=Task";
				}
				return Parent.PviPathName;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public bool TracePoints_Enabled
		{
			get
			{
				return propTracePoints_Enabled;
			}
			set
			{
				if (propConnectionState == ConnectionStates.Unininitialized || propConnectionState == ConnectionStates.Disconnected)
				{
					propTracePoints_Enabled = value;
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public event TPRegisterEventHandler TracePoints_Registered;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event PviEventHandler TracePoints_Unregistered;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event TPDataEventHandler TracePoints_DataRead;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event PviEventHandler TracePoints_DataChanged;

		public Task(Cpu cpu, string name)
			: base(cpu, name)
		{
			if (cpu != null)
			{
				Task task = cpu.Tasks[name];
				if (task != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Tasks\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Tasks\".", name);
				}
			}
			Init(cpu, name);
			propCpu.Tasks.Add(this);
		}

		internal Task(string name)
			: base(null, name)
		{
			Init(null, name);
		}

		public Task(Cpu cpu, string name, ref XmlTextReader reader, ConfigurationFlags flags)
			: base(cpu, name)
		{
			Init(cpu, name);
			FromXmlTextReader(ref reader, flags, this);
			propCpu.Tasks.Add(this);
		}

		internal Task(Cpu cpu, string name, TaskCollection collection)
			: base(cpu, name)
		{
			Init(cpu, name);
			collection.Add(this);
		}

		internal Task(Cpu cpu, PviObjectBrowser objBrowser, string name)
			: base(cpu, objBrowser, name)
		{
			Init(cpu, name);
			propCpu.Tasks.Add(this);
		}

		internal void Initialize(Task task)
		{
			propAddress = task.propAddress;
		}

		internal void Init(Cpu cpu, string name)
		{
			propTracePoints_Enabled = false;
			reCreateActive = false;
			propCpu = cpu;
			propParent = cpu;
			propMODULEState = ConnectionType.None;
			propTracePoints = new TracePointCollection(this);
			propVariables = null;
			propGlobals = null;
			if (cpu != null)
			{
				propVariables = new VariableCollection(cpu.Service.CollectionType, this, name + ".Variables");
				propGlobals = new VariableCollection(cpu.Service.CollectionType, this, name + ".Variables");
			}
			else
			{
				propVariables = new VariableCollection(CollectionType.HashTable, this, name + ".InternalVariables");
				propGlobals = new VariableCollection(CollectionType.HashTable, this, name + ".InternalVariables");
			}
			if (propVariables != null)
			{
				propVariables.propInternalCollection = true;
				propGlobals.propInternalCollection = true;
			}
			if (Service != null)
			{
				switch (Service.LogicalObjectsUsage)
				{
				case LogicalObjectsUsage.None:
				case (LogicalObjectsUsage)3:
					break;
				case LogicalObjectsUsage.FullName:
					Service.AddLogicalObject(FullName, this);
					break;
				case LogicalObjectsUsage.ObjectName:
					Service.AddLogicalObject(name, this);
					break;
				case LogicalObjectsUsage.ObjectNameWithType:
					Service.AddLogicalObject(PviPathName, this);
					break;
				}
			}
		}

		internal void reCreateChildState()
		{
			if (0 < propVariables.Count)
			{
				foreach (object value in propVariables.Values)
				{
					Variable variable = (Variable)value;
					if (variable.isObjectConnected || variable.reCreateActive)
					{
						variable.reCreateState();
					}
				}
			}
		}

		public override void Connect()
		{
			propReturnValue = 0;
			Connect(base.ConnectionType);
		}

		internal override void Connect(bool forceConnection)
		{
			propReturnValue = 0;
			Connect(forceConnection, base.ConnectionType);
		}

		public override void Connect(ConnectionType connectionType)
		{
			Connect(forceConnection: false, connectionType);
		}

		protected override string getLinkDescription()
		{
			if (propTracePoints_Enabled)
			{
				return "EV=edpsl";
			}
			return "EV=edps";
		}

		internal void Connect(bool forceConnection, ConnectionType connectionType)
		{
			base.ConnectionType = connectionType;
			propReturnValue = 0;
			if (reCreateActive || base.LinkId != 0)
			{
				return;
			}
			if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
			{
				Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.TaskConnect, Service));
			}
			else
			{
				if (ConnectionStates.Connecting == propConnectionState)
				{
					return;
				}
				if (propAddress == null || propAddress.Length == 0)
				{
					propAddress = propName;
				}
				propConnectionState = ConnectionStates.Connecting;
				if (!propCpu.HasPVIConnection && Service.WaitForParentConnection)
				{
					if (!forceConnection)
					{
						base.Requests |= Actions.Connect;
						return;
					}
					if (Actions.Connect == (base.Requests & Actions.Connect))
					{
						base.Requests &= ~Actions.Connect;
					}
				}
				propObjectParam = "CD=" + GetConnectionDescription();
				string linkDescription = getLinkDescription();
				if (!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					propReturnValue = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_TASK, propObjectParam, 705u, linkDescription, 401u);
				}
				else if (base.ConnectionType != ConnectionType.Link && propMODULEState != ConnectionType.Create)
				{
					propReturnValue = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_TASK, propObjectParam, 0u, "", 401u);
				}
				else
				{
					propReturnValue = PviLinkObject(706u);
				}
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskConnect, Service));
				}
			}
		}

		protected override string GetConnectionDescription()
		{
			string text = "";
			if (propTracePoints_Enabled)
			{
				string propAddress = base.propAddress;
				propAddress = ((-1 == propAddress.IndexOf("::")) ? ("TP+" + base.propAddress) : base.propAddress.Replace("::", "::TP+"));
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					return $"\"{propAddress}\"";
				}
				return $"\"{propParent.LinkName}\"/\"{propAddress}\"";
			}
			return base.GetConnectionDescription();
		}

		internal override int PviLinkObject(uint action)
		{
			int num = 0;
			string linkDescription = getLinkDescription();
			if (!Service.IsStatic || base.ConnectionType == ConnectionType.Link)
			{
				return XLinkRequest(Service.hPvi, base.LinkName, 705u, linkDescription, action);
			}
			return XLinkRequest(Service.hPvi, base.LinkName, 705u, linkDescription, action);
		}

		public override void Disconnect()
		{
			propReturnValue = 0;
			propConnectionState = ConnectionStates.Disconnecting;
			propModuleInfoRequested = false;
			propReturnValue = UnlinkRequest(402u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskDisconnect, Service));
			}
		}

		internal override void Resume()
		{
			propReturnValue = 0;
			propStartOrStopRequest = true;
			string text = "ST=Resume";
			IntPtr hMemory = PviMarshal.StringToHGlobal(text);
			propReturnValue = WriteRequest(base.Cpu.Service.hPvi, propLinkId, AccessTypes.Status, hMemory, text.Length, 403u);
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
			}
		}

		public void Start(int numberOfCycles)
		{
			propStartOrStopRequest = true;
			if (base.ProgramState == ProgramState.Running)
			{
				propReturnValue = 4124;
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
				return;
			}
			propReturnValue = 0;
			string text = "ST=Cycle(" + numberOfCycles.ToString() + ")";
			IntPtr hMemory = PviMarshal.StringToHGlobal(text);
			propReturnValue = WriteRequest(base.Cpu.Service.hPvi, base.LinkId, AccessTypes.Status, hMemory, text.Length, 410u);
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart));
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is deprecated use \"Start(0)\" instead")]
		public override void Start()
		{
			Start(0);
		}

		public int RunCycles(int numberOfCycles)
		{
			propStartOrStopRequest = true;
			if (base.ProgramState == ProgramState.Running)
			{
				propReturnValue = 4124;
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
				return propReturnValue;
			}
			propReturnValue = 0;
			string text = "ST=Cycle(" + numberOfCycles.ToString() + ")";
			IntPtr hMemory = PviMarshal.StringToHGlobal(text);
			propReturnValue = WriteRequest(base.Cpu.Service.hPvi, base.LinkId, AccessTypes.Status, hMemory, text.Length, 405u);
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart));
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStart, Service));
			}
			return propReturnValue;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int RunCylcles(int numberOfCycles)
		{
			return RunCycles(numberOfCycles);
		}

		public override void Stop()
		{
			propStartOrStopRequest = true;
			if (base.ProgramState == ProgramState.Stopped)
			{
				propReturnValue = 4134;
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
				OnStopped(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
				return;
			}
			propReturnValue = 0;
			string text = "ST=Stop";
			IntPtr hMemory = PviMarshal.StringToHGlobal(text);
			propReturnValue = WriteRequest(base.Cpu.Service.hPvi, base.LinkId, AccessTypes.Status, hMemory, text.Length, 404u);
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop));
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
				OnStopped(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.TaskStop, Service));
			}
		}

		protected override void OnConnected(PviEventArgs e)
		{
			bool flag = false;
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				if (ConnectionStates.Connected != propConnectionState)
				{
					flag = true;
				}
			}
			else if (ConnectionStates.ConnectedError > propConnectionState && ConnectionStates.Unininitialized < propConnectionState)
			{
				flag = true;
			}
			e.propAction = Action.TaskConnect;
			base.OnConnected(e);
			if (flag)
			{
				base.Cpu.Tasks.OnConnected(this, e);
				if (0 < propVariables.Count)
				{
					foreach (Variable value in propVariables.Values)
					{
						if ((value.Requests & Actions.Connect) != 0)
						{
							value.Requests &= ~Actions.Connect;
							value.propConnectionState = ConnectionStates.Unininitialized;
							value.Connect(forceConnect: true);
						}
					}
					if ((propVariables.Requests & Actions.Upload) != 0)
					{
						propVariables.Upload();
						propVariables.Requests &= ~Actions.Upload;
					}
				}
				if (propUserCollections != null && propUserCollections.Count > 0)
				{
					foreach (BaseCollection value2 in propUserCollections.Values)
					{
						if (value2 is TaskCollection)
						{
							((TaskCollection)value2).OnConnected(this, e);
						}
						if (value2 is ModuleCollection)
						{
							((ModuleCollection)value2).OnConnected(this, e);
						}
					}
				}
			}
			if (propReCreateActive)
			{
				propReCreateActive = false;
				if (Service.WaitForParentConnection)
				{
					reCreateChildState();
				}
			}
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			if (Service.IsRemoteError(e.ErrorCode) && ConnectionStates.Unininitialized < propConnectionState && ConnectionStates.Disconnecting > propConnectionState)
			{
				reCreateActive = true;
			}
			e.propAction = Action.TaskDisconnect;
			if (base.Cpu != null && base.Cpu.Tasks != null)
			{
				base.Cpu.Tasks.OnDisconnected(this, e);
			}
			base.OnDisconnected(e);
		}

		protected internal override void OnError(PviEventArgs e)
		{
			base.OnError(e);
			if (base.Cpu != null)
			{
				base.Cpu.Tasks.OnError(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					value.OnError(this, e);
				}
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed && disposing)
			{
				if (propTracePoints != null)
				{
					propTracePoints.Dispose();
					propTracePoints = null;
				}
				if (propVariables != null)
				{
					propVariables.Dispose(disposing, removeFromCollection);
					propVariables = null;
				}
				if (propGlobals != null)
				{
					propGlobals.Dispose(disposing, removeFromCollection);
					propGlobals = null;
				}
				if (removeFromCollection)
				{
					RemoveObject();
				}
				propErrorText = null;
				base.Dispose(disposing, removeFromCollection);
			}
		}

		internal override void RemoveObject()
		{
			Remove();
			if (base.Cpu != null && propUserCollections != null && 0 < propUserCollections.Count)
			{
				foreach (object value in propUserCollections.Values)
				{
					if (value is ModuleCollection)
					{
						((ModuleCollection)value).Remove(base.Name);
					}
					else if (value is TaskCollection)
					{
						((TaskCollection)value).Remove(base.Name);
					}
					else if (value is LoggerCollection)
					{
						((LoggerCollection)value).Remove(base.Name);
					}
					else if (value is LibraryCollection)
					{
						((LibraryCollection)value).Remove(base.Name);
					}
				}
			}
		}

		internal override void RemoveFromBaseCollections()
		{
			if (Service != null)
			{
				Service.LogicalObjects.Remove(base.LogicalName);
				if (Service.Services != null)
				{
					Service.Services.LogicalObjects.Remove(base.LogicalName);
				}
			}
			if (propUserCollections != null && 0 < propUserCollections.Count)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					value.Remove(base.Name);
				}
			}
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			Task task = (Task)baseObj;
			if (task == null)
			{
				return -1;
			}
			base.FromXmlTextReader(ref reader, flags, (Base)task);
			do
			{
				if (reader.NodeType == XmlNodeType.Comment)
				{
					reader.Read();
					continue;
				}
				if (reader.Name.ToLower().CompareTo("variablecollection") == 0)
				{
					task.Variables.FromXmlTextReader(ref reader, flags, task);
					reader.Read();
					continue;
				}
				if (reader.Name.ToLower().CompareTo("variable") != 0)
				{
					break;
				}
				task.Variables.FromXmlTextReader(ref reader, flags, task);
			}
			while (reader.NodeType != XmlNodeType.EndElement);
			if (reader.NodeType == XmlNodeType.EndElement && reader.Name.ToLower().CompareTo("task") == 0)
			{
				reader.Read();
			}
			return 0;
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteStartElement("Task");
			int result = SaveModuleConfiguration(ref writer, flags);
			Variables.ToXMLTextWriter(ref writer, flags);
			writer.WriteEndElement();
			return result;
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int propErrorCode = propErrorCode;
			propErrorCode = errorCode;
			if (eventType == EventTypes.TracePointsDataChanged)
			{
				OnTracePointsDataChanged(errorCode, option);
			}
			else
			{
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			switch (accessType)
			{
			case PVIReadAccessTypes.ChildObjects:
				propErrorCode = 2;
				break;
			case PVIReadAccessTypes.ANSL_TaskInfo:
				ANSLModuleDescriptionRead(pData, dataLen, errorCode);
				Fire_OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				break;
			case PVIReadAccessTypes.ANSL_TracePointsReadData:
				OnTracePointsRead(pData, dataLen, errorCode, option);
				break;
			default:
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
				break;
			}
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			propErrorCode = errorCode;
			switch (accessType)
			{
			case PVIWriteAccessTypes.ANSL_TracePointsRegister:
				OnTracePointsRegistered(pData, dataLen, errorCode, option);
				break;
			case PVIWriteAccessTypes.ANSL_TracePointsUnregister:
				OnTracePointsUnRegistered(errorCode, option);
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		protected override int ModuleInfoRequest()
		{
			int result = 0;
			if (!propModuleInfoRequested)
			{
				propModuleInfoRequested = true;
				result = ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_TaskInfo, IntPtr.Zero, 0, 411u);
			}
			return result;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int TracePoints_Register(TracePointDescriptionCollection tracePoints)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			IntPtr intPtr = IntPtr.Zero;
			int dataLen = 0;
			char c = ' ';
			if (tracePoints != null && 0 < tracePoints.Count)
			{
				dataLen = tracePoints.PVIDataSize;
				intPtr = Marshal.AllocHGlobal(tracePoints.PVIDataSize);
				for (num3 = 0; num3 < tracePoints.Count; num3++)
				{
					num4 = 0;
					TracePointDescription tracePointDescription = tracePoints[num3];
					PviMarshal.WriteUInt32(intPtr, num2, tracePointDescription.RecordLen);
					num2 += 4;
					PviMarshal.WriteUInt32(intPtr, num2, tracePointDescription.ID);
					num2 += 4;
					Marshal.WriteInt32(intPtr, num2, num4);
					num2 += 4;
					Marshal.WriteInt32(intPtr, num2, num4);
					num2 += 4;
					PviMarshal.WriteUInt64(intPtr, num2, tracePointDescription.Offset);
					num2 += 8;
					for (num4 = 0; num4 < tracePointDescription.ListOfVariables.Count; num4++)
					{
						if (0 < num4)
						{
							Marshal.WriteByte(intPtr, num2, (byte)c);
							num2++;
						}
						string text = tracePointDescription.ListOfVariables[num4].ToString();
						PviMarshal.WriteString(intPtr, num2, text);
						num2 += text.Length;
					}
					Marshal.WriteByte(intPtr, num2, 0);
					num2++;
				}
			}
			num = WriteRequest(base.Cpu.Service.hPvi, propLinkId, AccessTypes.ANSL_TracePointsRegister, intPtr, dataLen, 412u);
			if (IntPtr.Zero != intPtr)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return num;
		}

		private void OnTracePointsRegistered(IntPtr pData, uint dataLen, int errorCode, int option)
		{
			if (this.TracePoints_Registered != null)
			{
				this.TracePoints_Registered(this, new TPFormatEventArgs(propName, propAddress, errorCode, Service.Language, Action.TaskRegisterTPs, pData, dataLen));
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int TracePoints_Unregister()
		{
			int num = 0;
			return WriteRequest(base.Cpu.Service.hPvi, propLinkId, AccessTypes.ANSL_TracePointsUnregister, IntPtr.Zero, 0, 413u);
		}

		private void OnTracePointsUnRegistered(int errorCode, int option)
		{
			if (this.TracePoints_Unregistered != null)
			{
				this.TracePoints_Unregistered(this, new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.TaskUnregisterTPs));
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int TracePoints_ReadData()
		{
			int num = 0;
			return ReadRequest(base.Cpu.Service.hPvi, propLinkId, AccessTypes.ANSL_TracePointsReadData, 414u);
		}

		private void OnTracePointsRead(IntPtr pData, uint dataLen, int errorCode, int option)
		{
			if (this.TracePoints_DataRead != null)
			{
				TPDataEventArgs e = new TPDataEventArgs(propName, propAddress, errorCode, Service.Language, Action.TaskReadTPsData, pData, dataLen);
				this.TracePoints_DataRead(this, e);
			}
		}

		private void OnTracePointsDataChanged(int errorCode, int option)
		{
			if (this.TracePoints_DataChanged != null)
			{
				this.TracePoints_DataChanged(this, new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.TracePointDataChanged));
			}
		}
	}
}
