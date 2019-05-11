using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Module : Base
	{
		protected bool propModuleInfoRequested;

		internal uint propModUID;

		private byte propVersion;

		private byte propRevision;

		private byte propPICount;

		private byte propInstallNumber;

		private byte propInstallationPriority;

		private bool propInstPriorityValid;

		private ushort propDMIndex;

		private ushort propPIIndex;

		private ushort propPVIndex;

		private ushort propPVCount;

		private uint propCreateTime;

		private uint propAndTime;

		private uint propStartAddress;

		private uint propLength;

		private uint propAnalogMemoryAddress;

		private uint propDigitalMemoryAddess;

		private string propCreateName;

		private string propAndName;

		private Actions propLastAction;

		private MemoryType propMemoryLocation;

		internal ModuleType propType;

		private TaskClassType propTaskClass;

		private DomainState propDMState;

		protected ProgramState propPIState;

		internal Cpu propCpu;

		internal string propFileName;

		internal ConnectionType propMODULEState;

		internal Hashtable propUserCollections;

		internal bool propStartOrStopRequest;

		private PviObjectBrowser propObjectBrowser;

		private bool EnhancedTransferEnabled
		{
			get
			{
				if (Service.EnhancedTransfer)
				{
					return Cpu.HasAnslConnection;
				}
				return false;
			}
		}

		public short DomainIndex
		{
			get
			{
				if ((short)propDMIndex < 0)
				{
					return 0;
				}
				return (short)propDMIndex;
			}
		}

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		internal short PiIndex
		{
			get
			{
				if ((short)propPIIndex < 0)
				{
					return 0;
				}
				return (short)propPIIndex;
			}
		}

		public DomainState DomainState => propDMState;

		public ProgramState ProgramState => propPIState;

		internal byte PiCount => propPICount;

		[CLSCompliant(false)]
		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public uint StartAddress
		{
			get
			{
				if (propStartAddress < 0)
				{
					return 0u;
				}
				return propStartAddress;
			}
		}

		[CLSCompliant(false)]
		public uint Length
		{
			get
			{
				if (propLength < 0)
				{
					return 0u;
				}
				return propLength;
			}
		}

		public short Version => Convert.ToInt16(((propVersion >> 4) & 0xF) * 1000 + (propVersion & 0xF) * 100 + ((propRevision >> 4) & 0xF) * 10 + (propRevision & 0xF));

		public string VersionText => $"{Convert.ToString((propVersion >> 4) & 0xF)}.{Convert.ToString(propVersion & 0xF)}{Convert.ToString((propRevision >> 4) & 0xF)}.{Convert.ToString(propRevision & 0xF)}";

		public DateTime CreationTime => Pvi.UInt32ToDateTime(propCreateTime);

		public DateTime LastWriteTime => Pvi.UInt32ToDateTime(propAndTime);

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public string CreationName
		{
			get
			{
				return propCreateName;
			}
		}

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public string LastWriteName
		{
			get
			{
				return propAndName;
			}
		}

		public TaskClassType TaskClassType => propTaskClass;

		public byte InstallNumber => propInstallNumber;

		public byte InstallationPriority => propInstallationPriority;

		public bool InstallationPriorityValid => propInstPriorityValid;

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		internal short VariableIndex
		{
			get
			{
				if ((short)propPVIndex < 0)
				{
					return 0;
				}
				return (short)propPVIndex;
			}
		}

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public short VariableCount
		{
			get
			{
				if ((short)propPVCount < 0)
				{
					return 0;
				}
				return (short)propPVCount;
			}
		}

		[CLSCompliant(false)]
		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public uint AnalogMemoryAddress
		{
			get
			{
				if (propAnalogMemoryAddress < 0)
				{
					return 0u;
				}
				return propAnalogMemoryAddress;
			}
		}

		[CLSCompliant(false)]
		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public uint DigitalMemoryAddress
		{
			get
			{
				if (propDigitalMemoryAddess < 0)
				{
					return 0u;
				}
				return propDigitalMemoryAddess;
			}
		}

		public MemoryType MemoryType => propMemoryLocation;

		internal uint ModUID => propModUID;

		public ModuleType Type => propType;

		public Cpu Cpu => propCpu;

		public override string FullName
		{
			get
			{
				if (base.Name.Length > 0)
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
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				if (Parent != null)
				{
					if (base.Name != null && 0 < base.Name.Length)
					{
						return Parent.PviPathName + "/\"" + propName + "\" OT=Module";
					}
					return Parent.PviPathName;
				}
				if (base.Name != null && 0 < base.Name.Length)
				{
					return "/\"" + propName + "\" OT=Module";
				}
				return propName;
			}
		}

		public string FileName
		{
			get
			{
				return propFileName;
			}
			set
			{
				propFileName = value;
			}
		}

		internal PviObjectBrowser ObjectBrowser
		{
			get
			{
				return propObjectBrowser;
			}
			set
			{
				propObjectBrowser = value;
			}
		}

		public event PviEventHandler Stopped;

		public event PviEventHandler Started;

		public event PviEventHandler Deleted;

		public event PviEventHandler Uploaded;

		public event PviEventHandler Downloaded;

		public event PviEventHandler Cancelled;

		public event ModuleEventHandler UploadProgress;

		public event ModuleEventHandler DownloadProgress;

		public event PviEventHandler RunCycleCountSet;

		internal static bool isTaskObject(ModuleType type)
		{
			if (type == ModuleType.PlcTask || type == ModuleType.TimerTask || type == ModuleType.ExceptionTask)
			{
				return true;
			}
			return false;
		}

		internal static bool isLoggerObject(ModuleType type)
		{
			if (type == ModuleType.Logger)
			{
				return true;
			}
			return false;
		}

		internal static bool isTextModuleObject(ModuleType type)
		{
			if (type == ModuleType.TextSystemModule || type == ModuleType.TextSystemAddModule)
			{
				return true;
			}
			return false;
		}

		internal Module(Cpu cpu, string name, bool doNotAddToCollections)
			: base(cpu, name, doNotAddToCollections)
		{
			propObjectBrowser = null;
			Init(cpu, name);
		}

		public Module(Cpu cpu, string name, ModuleType modType)
			: base(cpu, name)
		{
			if (cpu != null && !(this is Logger) && !(this is Task) && cpu != null)
			{
				Module module = cpu.TextModules[name];
				if (module != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".TextModules\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Loggers\".", name);
				}
			}
			propObjectBrowser = null;
			Init(cpu, name);
			if (cpu != null)
			{
				cpu.TextModules.Add(this);
				cpu.Modules.Add(this);
			}
		}

		public Module(Cpu cpu, string name)
			: base(cpu, name)
		{
			if (cpu != null && !(this is Logger) && !(this is Task))
			{
				Module module = cpu.Modules[name];
				if (module != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Modules\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Modules\".", name);
				}
			}
			propObjectBrowser = null;
			Init(cpu, name);
			cpu?.Modules.Add(this);
		}

		public Module(Cpu cpu, string name, ref XmlTextReader reader, ConfigurationFlags flags)
			: base(cpu, name)
		{
			propObjectBrowser = null;
			Init(cpu, name);
			FromXmlTextReader(ref reader, flags, this);
			cpu.Modules.Add(this);
		}

		internal Module(Cpu cpu, string name, ModuleCollection collection)
			: base(cpu, name)
		{
			propObjectBrowser = null;
			Init(cpu, name);
			collection.Add(this);
		}

		internal Module(object parent, string name)
			: base((Base)parent, name)
		{
			propObjectBrowser = null;
			Init(null, name);
			propModUID = 0u;
			if (parent != null)
			{
				propModUID = ((Base)parent).Service.ModuleUID;
			}
		}

		internal Module(Cpu cpu, PviObjectBrowser objBrowser, string name)
			: base(cpu, name)
		{
			propObjectBrowser = objBrowser;
			Init(cpu, name);
		}

		private void Init(Cpu cpu, string name)
		{
			propModuleInfoRequested = false;
			propType = ModuleType.Unknown;
			if (this is Task)
			{
				propType = ModuleType.PlcTask;
			}
			else if (this is ErrorLogBook)
			{
				propType = ModuleType.Logger;
			}
			propCpu = cpu;
			propParent = cpu;
			propMODULEState = ConnectionType.None;
			if (Service != null)
			{
				propModUID = Service.ModuleUID;
			}
			propDMIndex = 0;
			propPIIndex = 0;
			propDMState = DomainState.NonExistent;
			propPIState = ProgramState.NonExistent;
			propPICount = 0;
			propStartAddress = 0u;
			propLength = 0u;
			propAddress = "";
			propName = name;
			propVersion = 0;
			propRevision = 0;
			propCreateTime = 0u;
			propAndTime = 0u;
			propCreateName = "";
			propAndName = "";
			propTaskClass = TaskClassType.NotValid;
			propInstallNumber = 0;
			propInstallationPriority = 128;
			propInstPriorityValid = false;
			propPVIndex = 0;
			propPVCount = 0;
			propAnalogMemoryAddress = 0u;
			propDigitalMemoryAddess = 0u;
			propMemoryLocation = MemoryType.NOTValid;
		}

		internal Module(Cpu cpu, APIFC_ModulInfoRes moduleInfo, ModuleCollection collection)
			: base(cpu)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfo);
			Init(cpu, moduleInfo.name);
			updateProperties(moduleInfoDescription, isDiagnosticData: false);
			collection?.Add(this);
		}

		internal Module(Cpu cpu, ModuleInfoDescription moduleInfo, ModuleCollection collection)
			: base(cpu)
		{
			Init(cpu, moduleInfo.name);
			updateProperties(moduleInfo, (cpu.BootMode == BootMode.Diagnostics) ? true : false);
			collection?.Add(this);
		}

		internal Module(Cpu cpu, APIFC_DiagModulInfoRes moduleInfo, ModuleCollection collection)
			: base(cpu)
		{
			Init(cpu, moduleInfo.name);
			updateProperties(moduleInfo);
			collection.Add(this);
		}

		internal void updateProperties(object moduleInfo)
		{
			updateProperties(moduleInfo, isDiagnosticData: false);
		}

		internal void updateProperties(object moduleInfo, bool isDiagnosticData)
		{
			if (moduleInfo is APIFC_ModulInfoRes)
			{
				ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
				moduleInfoDescription.Init((APIFC_ModulInfoRes)moduleInfo);
				updateProperties(moduleInfoDescription);
			}
			else if (moduleInfo is APIFC_DiagModulInfoRes)
			{
				updateProperties((APIFC_DiagModulInfoRes)moduleInfo);
			}
			else if (moduleInfo is ModuleInfoDescription)
			{
				updateProperties((ModuleInfoDescription)moduleInfo, isDiagnosticData);
			}
		}

		internal void updateProperties(APIFC_ModulInfoRes moduleInfo)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfo);
			updateProperties(moduleInfoDescription);
		}

		internal void updateProperties(ModuleInfoDescription moduleInfo, bool isDiagnosticData)
		{
			propModuleInfoRequested = false;
			propDMIndex = moduleInfo.dm_index;
			propPIIndex = PviMarshal.Convert.BytesToUShort(moduleInfo.instP_valid, moduleInfo.instP_value);
			propInstallationPriority = 128;
			propInstPriorityValid = false;
			if (byte.MaxValue == moduleInfo.instP_valid)
			{
				propInstPriorityValid = true;
				propInstallationPriority = moduleInfo.instP_value;
			}
			propDMState = moduleInfo.dm_state;
			if (Cpu.HasAnslConnection && (isDiagnosticData || Cpu.BootMode == BootMode.Diagnostics))
			{
				if (moduleInfo.dm_state == DomainState.NonExistent)
				{
					propDMState = DomainState.Valid;
				}
				else
				{
					propDMState = DomainState.Invalid;
				}
			}
			if (Cpu != null)
			{
				Cpu.UpdateModuleInfoList(moduleInfo);
			}
			if (propStartOrStopRequest)
			{
				if (moduleInfo.pi_state == ProgramState.Stopped)
				{
					OnStopped(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ModuleEvent, Service));
				}
				else if (moduleInfo.pi_state == ProgramState.Running)
				{
					OnStarted(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ModuleEvent, Service));
				}
			}
			else if (propPIState == ProgramState.Running && moduleInfo.pi_state == ProgramState.Stopped)
			{
				OnStopped(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ModuleEvent, Service));
			}
			else if (propPIState == ProgramState.Stopped && moduleInfo.pi_state == ProgramState.Running)
			{
				OnStarted(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ModuleEvent, Service));
			}
			propPIState = moduleInfo.pi_state;
			propPICount = moduleInfo.pi_count;
			propStartAddress = moduleInfo.address;
			propLength = moduleInfo.length;
			propAddress = moduleInfo.name;
			if (propName == null || propName.Length == 0)
			{
				propName = moduleInfo.name;
			}
			propVersion = moduleInfo.version;
			propRevision = moduleInfo.revision;
			propCreateTime = moduleInfo.erz_time;
			propAndTime = moduleInfo.and_time;
			propCreateName = moduleInfo.erz_name;
			propAndName = moduleInfo.and_name;
			propTaskClass = moduleInfo.task_class;
			propInstallNumber = moduleInfo.install_no;
			propPVIndex = moduleInfo.pv_idx;
			propPVCount = moduleInfo.pv_cnt;
			propAnalogMemoryAddress = moduleInfo.mem_ana_adr;
			propDigitalMemoryAddess = moduleInfo.mem_dig_adr;
			propMemoryLocation = moduleInfo.mem_location;
			propType = moduleInfo.type;
		}

		internal void updateProperties(APIFC_DiagModulInfoRes moduleInfo)
		{
			propModuleInfoRequested = false;
			if (propName == null || propName.Length == 0)
			{
				propName = moduleInfo.name;
			}
			propDMIndex = moduleInfo.dm_index;
			propAddress = moduleInfo.name;
			propVersion = moduleInfo.version;
			propRevision = moduleInfo.revision;
			propCreateTime = moduleInfo.erz_time;
			propAndTime = moduleInfo.and_time;
			propMemoryLocation = moduleInfo.mem_location;
			if (moduleInfo.dm_state == 0)
			{
				propDMState = DomainState.Valid;
			}
			else
			{
				propDMState = DomainState.Invalid;
			}
			propType = ModuleType.Unknown;
		}

		internal static int TicksToInt32(long ticks)
		{
			return Convert.ToInt32(ticks / 10000000);
		}

		internal static uint TicksToUInt32(long ticks)
		{
			return Convert.ToUInt32(ticks / 10000000);
		}

		public override void Connect()
		{
			Connect(base.ConnectionType);
		}

		public override void Connect(ConnectionType connectionType)
		{
			base.ConnectionType = connectionType;
			propReturnValue = Connect(forceConnection: false, connectionType, 301u);
		}

		protected override string getLinkDescription()
		{
			return "EV=edlfps";
		}

		internal override void Connect(bool forceConnection)
		{
			Connect(forceConnection, propConnectionType, 301u);
		}

		internal int Connect(bool forceConnection, ConnectionType connectionType, uint action)
		{
			base.ConnectionType = connectionType;
			int num = 0;
			if (reCreateActive || base.LinkId != 0)
			{
				return -2;
			}
			if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
			{
				Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.ModuleConnect, Service));
				return propErrorCode;
			}
			if (IsConnected && action == 301)
			{
				OnError(new PviEventArgs(propName, propAddress, 12002, Service.Language, (Action)action, Service));
				return 12002;
			}
			if (ConnectionStates.Connecting == propConnectionState)
			{
				return 0;
			}
			if (propAddress == null || propAddress.Length == 0)
			{
				propAddress = propName;
			}
			propObjectParam = "CD=" + GetConnectionDescription();
			propLinkParam = getLinkDescription();
			propConnectionState = ConnectionStates.Connecting;
			if (this is Logger && base.Address.ToLower().CompareTo("$Detect_SG4_SysLogger$") == 0)
			{
				num = 0;
			}
			else if (!propCpu.IsConnected && Service.WaitForParentConnection)
			{
				if (!forceConnection)
				{
					base.Requests |= Actions.Connect;
				}
				else if (Actions.Connect == (base.Requests & Actions.Connect))
				{
					base.Requests &= ~Actions.Connect;
				}
				return 0;
			}
			num = ((!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink) ? XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_MODULE, propObjectParam, action, propLinkParam, action) : ((base.ConnectionType == ConnectionType.Link || propMODULEState == ConnectionType.Create) ? PviLinkObject(action) : XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_MODULE, propObjectParam, 0u, "", action)));
			if (num != 0 && (action == 301 || action == 909))
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, (Action)action, Service));
			}
			return num;
		}

		public override void Disconnect()
		{
			propConnectionState = ConnectionStates.Disconnecting;
			propModuleInfoRequested = false;
			propReturnValue = Disconnect(302u, propNoDisconnectedEvent);
			if (propNoDisconnectedEvent)
			{
				propConnectionState = ConnectionStates.Disconnected;
			}
		}

		public override void Disconnect(bool noResponse)
		{
			propNoDisconnectedEvent = noResponse;
			propModuleInfoRequested = false;
			propConnectionState = ConnectionStates.Disconnecting;
			propReturnValue = Disconnect(302u, propNoDisconnectedEvent);
			if (propNoDisconnectedEvent)
			{
				propConnectionState = ConnectionStates.Disconnected;
			}
		}

		internal override int DisconnectRet(uint action)
		{
			return Disconnect(action, noResponse: false);
		}

		internal int Disconnect(uint action, bool noResponse)
		{
			int num = 0;
			base.Requests = Actions.NONE;
			if (action == 0 && base.LinkId == 0)
			{
				return 12004;
			}
			if (Service != null && base.LinkId != 0)
			{
				if (noResponse)
				{
					num = Unlink();
				}
				else
				{
					num = UnlinkRequest(action);
					if (num != 0 && action == 302)
					{
						OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, (Action)action, Service));
					}
				}
			}
			return num;
		}

		protected virtual int ModuleInfoRequest()
		{
			int result = 0;
			if (!propModuleInfoRequested)
			{
				propModuleInfoRequested = true;
				result = ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_ModuleInfo, IntPtr.Zero, 0, 308u);
			}
			return result;
		}

		internal virtual int ReadModuleInfo()
		{
			int num = 0;
			APIFC_ModulInfoRes modInfo = default(APIFC_ModulInfoRes);
			APIFC_DiagModulInfoRes diagModInfo = default(APIFC_DiagModulInfoRes);
			base.Requests |= Actions.ModuleInfo;
			num = ((!Cpu.HasAnslConnection) ? ((Cpu)propParent).ReadModuleList(propAddress, out modInfo, out diagModInfo) : ModuleInfoRequest());
			if (0 < num)
			{
				return num;
			}
			if (num != 0)
			{
				return 0;
			}
			if (modInfo.name != null && modInfo.name != "")
			{
				updateProperties(modInfo);
				base.Requests &= ~Actions.ModuleInfo;
				OnConnected(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ConnectedEvent, Service));
			}
			else if (diagModInfo.name != null && diagModInfo.name != "")
			{
				updateProperties(diagModInfo);
			}
			return num;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		internal virtual void Resume()
		{
			propReturnValue = 0;
			propStartOrStopRequest = true;
			propReturnValue = WriteRequest(Cpu.Service.hPvi, propLinkId, AccessTypes.ResumeModule, IntPtr.Zero, 0, 303u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStart, Service));
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void Start()
		{
			propReturnValue = 0;
			propStartOrStopRequest = true;
			propReturnValue = WriteRequest(Cpu.Service.hPvi, propLinkId, AccessTypes.ResumeModule, IntPtr.Zero, 0, 303u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStart, Service));
				OnStarted(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStart, Service));
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void Stop()
		{
			propReturnValue = 0;
			propStartOrStopRequest = true;
			propReturnValue = WriteRequest(Cpu.Service.hPvi, propLinkId, AccessTypes.StopModule, IntPtr.Zero, 0, 304u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStop, Service));
				OnStopped(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleStop, Service));
			}
		}

		public virtual void Delete()
		{
			Delete(useParentCpu: false);
		}

		public virtual void Delete(bool useParentCpu)
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			propReturnValue = 0;
			if (useParentCpu)
			{
				string str = "MN=";
				str = ((base.Address == null || 0 >= base.Address.Length) ? (str + base.Name) : (str + base.Address));
				Service.BuildRequestBuffer(str);
				num = str.Length;
				if (Cpu.BootMode == BootMode.Diagnostics && !Cpu.HasAnslConnection)
				{
					PviMarshal.WriteUInt16(Service.RequestBuffer, propDMIndex);
					num = Marshal.SizeOf(typeof(ushort));
					propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.DeleteDiagModule, Service.RequestBuffer, num, 219u);
				}
				else
				{
					propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.CpuModuleDelete, Service.RequestBuffer, num, 219u);
				}
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleDelete, Service));
				}
			}
			else
			{
				if (Cpu.BootMode == BootMode.Diagnostics)
				{
					PviMarshal.WriteUInt16(Service.RequestBuffer, propDMIndex);
					num = Marshal.SizeOf(typeof(ushort));
					propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.DeleteDiagModule, Service.RequestBuffer, num, 305u);
				}
				else
				{
					propReturnValue = WriteRequest(Service.hPvi, propLinkId, AccessTypes.DeleteModule, zero, 0, 305u);
				}
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleDelete, Service));
				}
			}
		}

		public virtual void Upload(string fName)
		{
			Upload(fName, ConversionModes.BR, CommentLanguages.DEFAULT);
		}

		public virtual void Upload(string fName, ConversionModes uploadConversion)
		{
			Upload(fName, uploadConversion, CommentLanguages.DEFAULT);
		}

		public virtual void Upload(string fName, ConversionModes uploadConversion, CommentLanguages commentLanguage)
		{
			string text = "";
			propReturnValue = 0;
			if (!IsConnected && Service.WaitForParentConnection)
			{
				base.Requests |= Actions.Upload;
				propFileName = fName;
				return;
			}
			switch (uploadConversion)
			{
			case ConversionModes.NC_UPLOAD:
			case ConversionModes.CNC:
			case ConversionModes.ZPO:
			case ConversionModes.TDT:
			case ConversionModes.RPT:
			case ConversionModes.CAM:
			case ConversionModes.CAP:
				text = ((commentLanguage != 0) ? (" \"MT=NC_ RL=" + commentLanguage.ToString() + "\" ") : " \"MT=NC_\" ");
				break;
			case ConversionModes.TXT:
				text = " \"MT=BRT\" ";
				break;
			default:
				text = "\"" + fName + "\"";
				break;
			}
			if (EnhancedTransferEnabled)
			{
				text += " ET=1";
			}
			IntPtr hMemory = PviMarshal.StringToHGlobal(text);
			Cpu.actUpLoadModuleName = base.Name;
			Cpu.listOfUpLoadModules.Add(base.Name);
			propLastAction = Actions.Upload;
			if (EnhancedTransferEnabled)
			{
				propReturnValue = ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.Upload, hMemory, text.Length, 306u);
			}
			else
			{
				propReturnValue = ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.UploadStream, hMemory, text.Length, 306u);
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue == 0)
			{
				propFileName = fName;
			}
			if (propReturnValue != 0)
			{
				propLastAction = Actions.NONE;
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleUpload, Service));
				OnUploaded(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleUpload, Service));
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void Download(MemoryType memoryType, InstallOption installOption, string fileName)
		{
			Download(installOption, memoryType, InstallMode.Overload, fileName);
		}

		public virtual void Download(MemoryType memoryType, InstallMode installMode, string fileName)
		{
			Download(InstallOption.Undefined, memoryType, installMode, fileName);
		}

		private void Download(InstallOption installOption, MemoryType memoryType, InstallMode installMode, string fileName)
		{
			int num = fileName.IndexOf(".");
			ConversionModes conversionMode = ConversionModes.BR;
			if (-1 != num && 4 < fileName.Length)
			{
				string text = fileName.Substring(fileName.Length - 4, 4);
				if (text.ToLower().CompareTo(".txt") == 0)
				{
					conversionMode = ConversionModes.TXT;
				}
				else if (text.ToLower().CompareTo(".cnc") == 0)
				{
					conversionMode = ConversionModes.CNC;
				}
				else if (text.ToLower().CompareTo(".zp0") == 0)
				{
					conversionMode = ConversionModes.ZPO;
				}
				else if (text.ToLower().CompareTo(".zpo") == 0)
				{
					conversionMode = ConversionModes.ZPO;
				}
				else if (text.ToLower().CompareTo(".tdt") == 0)
				{
					conversionMode = ConversionModes.TDT;
				}
				else if (text.ToLower().CompareTo(".rpt") == 0)
				{
					conversionMode = ConversionModes.RPT;
				}
				else if (text.ToLower().CompareTo(".cam") == 0)
				{
					conversionMode = ConversionModes.CAM;
				}
				else if (text.ToLower().CompareTo(".cap") == 0)
				{
					conversionMode = ConversionModes.CAP;
				}
				Download(installOption, memoryType, installMode, fileName, conversionMode, null, null);
			}
			else
			{
				OnDownloaded(new PviEventArgs(base.Name, base.Address, 4067, Service.Language, Action.ModuleDownload, Service));
			}
		}

		public virtual void Download()
		{
			Download(MemoryType.UserRam, InstallMode.Overload, propFileName);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void Download(InstallOption installOption)
		{
			Download(MemoryType.UserRam, installOption, propFileName);
		}

		public virtual void Download(MemoryType memoryType)
		{
			Download(memoryType, InstallMode.Overload, propFileName);
		}

		public virtual void Download(MemoryType memoryType, InstallMode installMode)
		{
			Download(memoryType, installMode, propFileName);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void Download(MemoryType memoryType, InstallOption installOption)
		{
			Download(memoryType, installOption, propFileName);
		}

		public void Download(MemoryType memoryType, InstallMode installMode, string srcFileName, string moduleVersion, string moduleName)
		{
			Download(memoryType, installMode, srcFileName, ConversionModes.TXT, moduleVersion, moduleName);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Download(MemoryType memoryType, InstallOption installOption, string srcFileName, string moduleVersion, string moduleName)
		{
			Download(memoryType, installOption, srcFileName, ConversionModes.TXT, moduleVersion, moduleName);
		}

		public void Download(MemoryType memoryType, InstallMode installMode, string srcFileName, ConversionModes conversionMode, string moduleName)
		{
			Download(memoryType, installMode, srcFileName, conversionMode, null, moduleName);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public void Download(MemoryType memoryType, InstallOption installOption, string srcFileName, ConversionModes conversionMode, string moduleName)
		{
			Download(memoryType, installOption, srcFileName, conversionMode, null, moduleName);
		}

		private void Download(MemoryType memoryType, InstallMode installMode, string srcFileName, ConversionModes conversionMode, string moduleVersion, string moduleName)
		{
			Download(InstallOption.Undefined, memoryType, installMode, srcFileName, conversionMode, moduleVersion, moduleName);
		}

		private void Download(MemoryType memoryType, InstallOption installOption, string srcFileName, ConversionModes conversionMode, string moduleVersion, string moduleName)
		{
			Download(installOption, memoryType, InstallMode.Overload, srcFileName, conversionMode, moduleVersion, moduleName);
		}

		private void Download(InstallOption installOption, MemoryType memoryType, InstallMode installMode, string srcFileName, ConversionModes conversionMode, string moduleVersion, string moduleName)
		{
			byte[] moduleData = null;
			if (EnhancedTransferEnabled)
			{
				propFileName = srcFileName;
			}
			else
			{
				FileStream fileStream = new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				moduleData = binaryReader.ReadBytes((int)fileStream.Length);
				binaryReader.Close();
				fileStream.Close();
			}
			Download(installOption, memoryType, installMode, conversionMode, moduleVersion, moduleName, moduleData);
			moduleData = null;
			if (propReturnValue == 0)
			{
				propFileName = srcFileName;
			}
			else
			{
				propFileName = "";
			}
		}

		public void Download(MemoryType memoryType, InstallMode installMode, ConversionModes conversionMode, string moduleVersion, string moduleName, Stream streamData)
		{
			Download(InstallOption.Undefined, memoryType, installMode, conversionMode, moduleVersion, moduleName, streamData);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public void Download(MemoryType memoryType, InstallOption installOption, ConversionModes conversionMode, string moduleVersion, string moduleName, Stream streamData)
		{
			Download(installOption, memoryType, InstallMode.Overload, conversionMode, moduleVersion, moduleName, streamData);
		}

		private void Download(InstallOption installOption, MemoryType memoryType, InstallMode installMode, ConversionModes conversionMode, string moduleVersion, string moduleName, Stream streamData)
		{
			BinaryReader binaryReader = new BinaryReader(streamData);
			byte[] moduleData = binaryReader.ReadBytes((int)streamData.Length);
			binaryReader.Close();
			streamData.Close();
			Download(installOption, memoryType, installMode, conversionMode, moduleVersion, moduleName, moduleData);
			moduleData = null;
		}

		public void Download(MemoryType memoryType, InstallMode installMode, ConversionModes conversionMode, string moduleVersion, string moduleName, byte[] moduleData)
		{
			Download(InstallOption.Undefined, memoryType, installMode, conversionMode, moduleVersion, moduleName, moduleData);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Download(MemoryType memoryType, InstallOption installOption, ConversionModes conversionMode, string moduleVersion, string moduleName, byte[] moduleData)
		{
			Download(installOption, memoryType, InstallMode.Overload, conversionMode, moduleVersion, moduleName, moduleData);
		}

		private string GetConversionSettings(ConversionModes conversionMode, string moduleVersion, string moduleName)
		{
			string text = "";
			switch (conversionMode)
			{
			case ConversionModes.TXT:
				text = " MT=BRT";
				if (moduleVersion != null && 0 < moduleVersion.Length)
				{
					text = text + " MV=" + moduleVersion;
				}
				break;
			case ConversionModes.CNC:
				text = " MT=NC_CNC";
				break;
			case ConversionModes.ZPO:
				text = " MT=NC_ZPO";
				break;
			case ConversionModes.TDT:
				text = " MT=NC_TDT";
				break;
			case ConversionModes.RPT:
				text = " MT=NC_RPT";
				break;
			case ConversionModes.CAM:
				text = " MT=NC_CAM";
				break;
			case ConversionModes.CAP:
				text = " MT=NC_CAP";
				break;
			}
			if (0 < text.Length)
			{
				text += " ";
			}
			if (ConversionModes.BR < conversionMode)
			{
				text = ((moduleName != null && 0 < moduleName.Length) ? (text + "MN=" + moduleName) : ((propAddress == null || 0 >= propAddress.Length) ? (text + "MN=" + propName) : (text + "MN=" + base.Address)));
			}
			else if (moduleName != null && 0 < moduleName.Length)
			{
				text = "MN=" + moduleName;
			}
			return text;
		}

		private static string GetInstallMemSettings(MemoryType memoryType)
		{
			switch (memoryType)
			{
			case MemoryType.UserRom:
				return " LD=Rom";
			case MemoryType.UserRam:
				return " LD=Ram";
			case MemoryType.MemCard:
				return " LD=MemCard";
			case MemoryType.FixRam:
				return " LD=FixRam";
			case MemoryType.Dram:
				return " LD=DRam";
			case MemoryType.SystemRom:
				return " LD=SysRom";
			case MemoryType.Permanent:
				return " LD=PerMem";
			case MemoryType.TransferModule:
				return " LD=Trsf";
			default:
				return " LD=Ram";
			}
		}

		private static string GetInstallOptionSettings(InstallOption installOption, InstallMode installMode)
		{
			if (installOption == InstallOption.Undefined)
			{
				switch (installMode)
				{
				case InstallMode.Overload:
					return " IM=Overload";
				case InstallMode.OneCycle:
					return " IM=OneCycle";
				case InstallMode.Copy:
					return " IM=Copy";
				default:
					return "";
				}
			}
			return " IMH=" + (int)installOption;
		}

		private unsafe static IntPtr AllocateDataForRequest(bool enhancedTransferEnabled, string parameter, byte[] moduleData, out int dataSize)
		{
			dataSize = parameter.Length + 1;
			if (!enhancedTransferEnabled)
			{
				dataSize += moduleData.Length;
			}
			IntPtr result = PviMarshal.AllocHGlobal((IntPtr)dataSize);
			byte* ptr;
			for (int i = 0; i < parameter.Length; i++)
			{
				ptr = (byte*)result.ToPointer() + i;
				*ptr = (byte)parameter[i];
			}
			ptr = (byte*)result.ToPointer() + parameter.Length;
			*ptr = 0;
			if (enhancedTransferEnabled)
			{
				return result;
			}
			for (int j = parameter.Length + 1; j < dataSize; j++)
			{
				ptr = (byte*)result.ToPointer() + j;
				*ptr = moduleData[j - parameter.Length - 1];
			}
			return result;
		}

		private void Download(InstallOption installOption, MemoryType memoryType, InstallMode installMode, ConversionModes conversionMode, string moduleVersion, string moduleName, byte[] moduleData)
		{
			propReturnValue = 0;
			string str = "";
			if (EnhancedTransferEnabled)
			{
				str = "FN=\"" + FileName + "\" ET=1";
			}
			str += GetConversionSettings(conversionMode, moduleVersion, moduleName);
			str += GetInstallMemSettings(memoryType);
			str += GetInstallOptionSettings(installOption, installMode);
			int dataSize;
			IntPtr hMemory = AllocateDataForRequest(EnhancedTransferEnabled, str, moduleData, out dataSize);
			Cpu.actDownLoadModuleName = base.Name;
			Cpu.listOfDownLoadModules.Add(base.Name);
			propLastAction = Actions.Download;
			if (EnhancedTransferEnabled)
			{
				propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.Download, hMemory, dataSize, 307u);
			}
			else
			{
				propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.DownloadStream, hMemory, dataSize, 307u);
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			if (propReturnValue != 0)
			{
				propLastAction = Actions.NONE;
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleDownload, Service));
				OnDownloaded(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ModuleDownload, Service));
			}
			else if (!propCpu.Modules.ContainsKey(propName))
			{
				propCpu.Modules.Add(this);
			}
		}

		public virtual void Cancel()
		{
			propReturnValue = 0;
			int val = 21;
			IntPtr zero = IntPtr.Zero;
			zero = PviMarshal.AllocHGlobal((IntPtr)4);
			Marshal.WriteInt32(zero, val);
			Cpu.actDownLoadModuleName = Cpu.RemoveULorDLName(ref Cpu.listOfDownLoadModules, base.Name);
			Cpu.actUpLoadModuleName = Cpu.RemoveULorDLName(ref Cpu.listOfUpLoadModules, base.Name);
			if (Actions.Download == (propLastAction & Actions.Download))
			{
				propReturnValue = WriteRequest(Service.hPvi, Cpu.LinkId, AccessTypes.Cancel, zero, 4, 1100u);
			}
			else
			{
				propReturnValue = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Cancel, IntPtr.Zero, 0, 1100u);
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.Cancel, Service));
			}
			PviMarshal.FreeHGlobal(ref zero);
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propErrorCode = errorCode;
			if (errorCode == 0 || 12002 == errorCode)
			{
				propLinkId = linkID;
				if (Service != null && Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					propMODULEState = ConnectionType.Create;
					PviLinkObject(301u);
				}
			}
			else if (Service.IsRemoteError(errorCode))
			{
				base.Requests |= Actions.Connect;
			}
		}

		internal override int PviLinkObject(uint action)
		{
			int num = 0;
			if (!Service.IsStatic || base.ConnectionType == ConnectionType.Link)
			{
				if (916 == action)
				{
					return XLinkRequest(Service.hPvi, base.LinkName, action, propLinkParam, action);
				}
				return XLinkRequest(Service.hPvi, base.LinkName, 707u, propLinkParam, 707u);
			}
			if (916 == action)
			{
				return XLinkRequest(Service.hPvi, base.LinkName, action, propLinkParam, action);
			}
			return XLinkRequest(Service.hPvi, base.LinkName, 707u, propLinkParam, 707u);
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			int num = 0;
			propErrorCode = errorCode;
			propLinkId = linkID;
			if (errorCode == 0 && 1 == option)
			{
				num = ReadRequest(Service.hPvi, linkID, AccessTypes.List, 150u);
				if (num != 0)
				{
					Service.OnPVIObjectsAttached(new PviEventArgs(propName, propAddress, num, Service.Language, Action.TaskReadVariablesList));
				}
			}
			base.OnPviLinked(errorCode, linkID, 1);
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int propErrorCode = propErrorCode;
			propErrorCode = errorCode;
			switch (eventType)
			{
			case EventTypes.Data:
			{
				int num = ReadModuleInfo();
				if (num != 0)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.ModuleInfo));
				}
				break;
			}
			case EventTypes.Error:
				if (1 == option)
				{
					OnError(this, new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerDetectSGType, Service));
				}
				else if (errorCode != 0)
				{
					if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
					{
						if (Service != null)
						{
							OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ErrorEvent, Service));
						}
					}
					else if (ConnectionStates.Connecting == propConnectionState)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
					}
					if (Service != null)
					{
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ErrorEvent, Service));
					}
				}
				else
				{
					int num = ReadModuleInfo();
				}
				break;
			case EventTypes.Proceeding:
			{
				ProgressInfo progressInfo = PviMarshal.PtrToProgressInfoStructure(pData, typeof(ProgressInfo));
				ModuleEventArgs e = new ModuleEventArgs(propName, propAddress, base.ErrorCode, Service.Language, Action.ModuleProgressEvent, this, progressInfo.Percent);
				if (Actions.Upload == (propLastAction & Actions.Upload))
				{
					OnUploadProgress(e);
				}
				else
				{
					OnDownloadProgress(e);
				}
				break;
			}
			case EventTypes.ModuleChanged:
				if (errorCode == 0)
				{
					APIFC_ModulInfoRes moduleInfo = PviMarshal.PtrToModulInfoStructure(pData, typeof(APIFC_ModulInfoRes));
					updateProperties(moduleInfo);
					if (Parent is Cpu)
					{
						((Cpu)Parent).Modules.OnModuleChanged(new ModuleEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleChangedEvent, this, 0));
					}
				}
				break;
			case EventTypes.ModuleDeleted:
				if (errorCode != 0)
				{
				}
				break;
			case EventTypes.Status:
				if (errorCode != 0)
				{
				}
				break;
			default:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				break;
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			switch (accessType)
			{
			case PVIReadAccessTypes.State:
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleState, Service));
				break;
			case PVIReadAccessTypes.ModuleList:
				if (errorCode == 0 && UpdateModuleInfo(pData, dataLen) == 0)
				{
					if (1 == option)
					{
						propConnectionState = ConnectionStates.Connected;
						Upload(propFileName);
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
					}
				}
				break;
			case PVIReadAccessTypes.StreamUpload:
				if (errorCode == 0)
				{
					FileStream fileStream = new FileStream(propFileName, FileMode.Create);
					BinaryWriter binaryWriter = new BinaryWriter(fileStream);
					byte[] array = new byte[dataLen];
					Marshal.Copy(pData, array, 0, (int)dataLen);
					binaryWriter.Write(array, 0, (int)dataLen);
					binaryWriter.Close();
					fileStream.Close();
					OnUploaded(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				else
				{
					if (12043 == errorCode)
					{
						OnCancelled(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
					}
					OnUploaded(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				break;
			case PVIReadAccessTypes.Upload:
				if (errorCode == 0)
				{
					OnUploaded(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
					break;
				}
				if (12043 == errorCode)
				{
					OnCancelled(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				OnUploaded(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleUpload, Service));
				break;
			case PVIReadAccessTypes.ANSL_ModuleInfo:
				ANSLModuleDescriptionRead(pData, dataLen, errorCode);
				Fire_OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
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
			case PVIWriteAccessTypes.Download:
			case PVIWriteAccessTypes.StreamDownLoad:
				if (12043 == errorCode)
				{
					OnCancelled(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleDownload, Service));
				}
				OnDownloaded(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleDownload, Service));
				break;
			case PVIWriteAccessTypes.State:
				if (1 == option)
				{
					OnStarted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleStart, Service));
				}
				else if (2 == option)
				{
					OnStarted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TaskStart, Service));
				}
				else if (3 == option)
				{
					OnStopped(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleStop, Service));
				}
				else if (4 == option)
				{
					OnStopped(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TaskStop, Service));
				}
				else if (5 == option)
				{
					OnRunCycleCountSet(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.TaskRunCycle, Service));
				}
				else if (6 == option)
				{
					Resume();
				}
				else
				{
					base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				}
				break;
			case PVIWriteAccessTypes.ResumeModule:
				OnStarted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleResume, Service));
				break;
			case PVIWriteAccessTypes.StartModule:
				OnStarted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleStart, Service));
				break;
			case PVIWriteAccessTypes.StopModule:
				OnStopped(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleStop, Service));
				break;
			case PVIWriteAccessTypes.CpuModuleDelete:
			case PVIWriteAccessTypes.DeleteDiagModule:
			case PVIWriteAccessTypes.DeleteModule:
				OnDeleted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ModuleDelete, Service));
				if (errorCode == 0)
				{
					Remove();
				}
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
			propErrorCode = errorCode;
			if (1 == option)
			{
				propLinkId = 0u;
				if (Service != null)
				{
					OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.UnLinkObject, Service));
				}
				if (errorCode != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.UnLinkObject, Service));
				}
			}
			else
			{
				base.OnPviUnLinked(errorCode, option);
			}
		}

		internal int UpdateModuleInfo(IntPtr ptrData, uint dataLen)
		{
			int num = 0;
			int num2 = 0;
			num = 164;
			if (Cpu.BootMode == BootMode.Diagnostics)
			{
				num = 57;
			}
			num2 = (int)((long)dataLen / (long)num);
			for (int i = 0; i < num2; i++)
			{
				if (Cpu.BootMode == BootMode.Diagnostics)
				{
					APIFC_DiagModulInfoRes moduleInfo = PviMarshal.PtrToDiagModulInfoStructure(PviMarshal.GetIntPtr(ptrData, (ulong)(i * num)), typeof(APIFC_DiagModulInfoRes));
					if (propParent is Cpu && base.Address == moduleInfo.name)
					{
						updateProperties(moduleInfo);
					}
					continue;
				}
				APIFC_ModulInfoRes moduleInfo2 = PviMarshal.PtrToModulInfoStructure(PviMarshal.GetIntPtr(ptrData, (ulong)(i * num)), typeof(APIFC_ModulInfoRes));
				if (propParent is Cpu && base.Address.CompareTo(moduleInfo2.name) == 0)
				{
					updateProperties(moduleInfo2);
					return 0;
				}
			}
			return -1;
		}

		internal virtual bool CheckModuleInfo(int errorCode)
		{
			if ((base.Requests & Actions.ModuleInfo) != 0)
			{
				base.Requests &= ~Actions.ModuleInfo;
				return true;
			}
			return false;
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (propDisposed)
			{
				return;
			}
			Base propParent = base.propParent;
			string propLinkName = base.propLinkName;
			string propLogicalName = base.propLogicalName;
			object propUserData = base.propUserData;
			string propName = base.propName;
			string propAddress = base.propAddress;
			base.Dispose(disposing, removeFromCollection);
			if (disposing)
			{
				base.propParent = propParent;
				base.propLinkName = propLinkName;
				base.propLogicalName = propLogicalName;
				base.propUserData = propUserData;
				base.propName = propName;
				base.propAddress = propAddress;
				if (removeFromCollection)
				{
					RemoveFromBaseCollections();
					RemoveObject();
				}
				propAndName = null;
				propCpu = null;
				propCreateName = null;
				propFileName = null;
				if (propUserCollections != null)
				{
					propUserCollections.Clear();
					propUserCollections = null;
				}
				base.propParent = null;
				base.propLinkName = null;
				base.propLogicalName = null;
				base.propUserData = null;
			}
		}

		internal override void RemoveReferences()
		{
			if (propUserCollections != null && 0 < propUserCollections.Count)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					value.Remove(base.Name);
				}
			}
		}

		internal override void RemoveObject()
		{
			base.RemoveObject();
			if (Cpu != null)
			{
				if (Cpu.Tasks != null && this is Task)
				{
					Cpu.Tasks.Remove(base.Name);
				}
				if (Cpu.Loggers != null && this is Logger)
				{
					Cpu.Loggers.Remove(base.Name);
				}
				if (propUserCollections != null && 0 < propUserCollections.Count)
				{
					foreach (BaseCollection value in propUserCollections.Values)
					{
						value.Remove(base.Name);
					}
				}
			}
		}

		public override void Remove()
		{
			base.Remove();
			if (Cpu != null)
			{
				Cpu.Modules.Remove(base.Name);
				if (this is Task && Cpu.Tasks != null)
				{
					Cpu.Tasks.Remove(base.Name);
				}
				if (this is Logger && Cpu.Loggers != null)
				{
					propCpu.Loggers.Remove(base.Name);
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

		internal override void RemoveFromBaseCollections()
		{
			base.RemoveFromBaseCollections();
			if (this is Task && propCpu != null)
			{
				if (CollectionType.ArrayList == propCpu.Tasks.propCollectionType)
				{
					propCpu.Tasks.Remove(this);
				}
				else
				{
					propCpu.Tasks.Remove(base.Name);
				}
			}
			if (this is Logger && propCpu != null)
			{
				if (CollectionType.ArrayList == propCpu.Loggers.propCollectionType)
				{
					propCpu.Loggers.Remove(this);
				}
				else
				{
					propCpu.Loggers.Remove(base.Name);
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

		internal void Fire_OnConnect()
		{
			OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.ConnectedEvent));
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
			if ((base.Requests & Actions.Upload) != 0)
			{
				base.Requests &= ~Actions.Upload;
				Upload(propFileName);
			}
			base.OnConnected(e);
			if (flag)
			{
				if (Cpu != null)
				{
					Cpu.Modules.OnConnected(this, e);
				}
				if (propUserCollections != null && propUserCollections.Count > 0 && !(this is Task))
				{
					foreach (BaseCollection value in propUserCollections.Values)
					{
						value.OnConnected(this, e);
					}
				}
			}
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			bool flag = (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState || ConnectionStates.Disconnecting == propConnectionState) ? true : false;
			base.OnDisconnected(e);
			propMODULEState = ConnectionType.None;
			if (flag)
			{
				if (Cpu != null && Cpu.Modules != null)
				{
					Cpu.Modules.OnDisconnected(this, e);
				}
				if (propUserCollections != null && 0 < propUserCollections.Count)
				{
					foreach (BaseCollection value in propUserCollections.Values)
					{
						value.OnDisconnected(this, e);
					}
				}
			}
		}

		protected virtual void OnRunCycleCountSet(PviEventArgs e)
		{
			if (this.RunCycleCountSet != null)
			{
				this.RunCycleCountSet(this, e);
			}
		}

		protected virtual void OnStarted(PviEventArgs e)
		{
			if (e.ErrorCode == 0)
			{
				propPIState = ProgramState.Running;
				propStartOrStopRequest = false;
			}
			if (this.Started != null)
			{
				this.Started(this, e);
			}
		}

		protected virtual void OnStopped(PviEventArgs e)
		{
			if (e.ErrorCode == 0)
			{
				propPIState = ProgramState.Stopped;
				propStartOrStopRequest = false;
			}
			if (this.Stopped != null)
			{
				this.Stopped(this, e);
			}
		}

		protected override void OnDeleted(PviEventArgs e)
		{
			if (Cpu != null && Service != null)
			{
				Cpu.Modules.OnModuleDeleted(new ModuleEventArgs(base.Name, propAddress, e.ErrorCode, Service.Language, Action.ModuleDeletedEvent, this, 0));
				if (this is Task)
				{
					Cpu.Tasks.OnTaskDeleted(new ModuleEventArgs(base.Name, propAddress, e.ErrorCode, Service.Language, Action.ModuleDeletedEvent, this, 0));
				}
			}
			if (base.LinkId == 0)
			{
				base.OnDeleted(e);
			}
			if (this.Deleted != null)
			{
				this.Deleted(this, e);
			}
		}

		protected virtual void OnUploaded(PviEventArgs e)
		{
			propLastAction = Actions.NONE;
			Cpu.actUpLoadModuleName = Cpu.RemoveULorDLName(ref Cpu.listOfUpLoadModules, base.Name);
			if (this.Uploaded != null)
			{
				this.Uploaded(this, e);
			}
			if (Cpu != null)
			{
				Cpu.Modules.OnModuleUploaded(this, e);
			}
			if (this is Task && Cpu != null)
			{
				Cpu.Tasks.OnTaskUploaded((Task)this, e);
			}
			if (this is Logger && Cpu != null)
			{
				Cpu.Loggers.OnLoggerUploaded((Logger)this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					if (value is LoggerCollection)
					{
						((LoggerCollection)value).OnLoggerUploaded((Logger)this, e);
					}
					if (value is TaskCollection)
					{
						((TaskCollection)value).OnTaskUploaded((Task)this, e);
					}
					if (value is ModuleCollection)
					{
						((ModuleCollection)value).OnModuleUploaded(this, e);
					}
				}
			}
		}

		protected virtual void OnDownloaded(PviEventArgs e)
		{
			propLastAction = Actions.NONE;
			Cpu.actDownLoadModuleName = Cpu.RemoveULorDLName(ref Cpu.listOfDownLoadModules, base.Name);
			if (this.Downloaded != null)
			{
				this.Downloaded(this, e);
			}
			if (Cpu != null)
			{
				Cpu.Modules.OnModuleDownloaded(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					if (value is TaskCollection)
					{
						((TaskCollection)value).OnTaskDownloaded((Task)this, e);
					}
					if (value is ModuleCollection)
					{
						((ModuleCollection)value).OnModuleDownloaded(this, e);
					}
				}
			}
		}

		protected virtual void OnUploadProgress(ModuleEventArgs e)
		{
			if (this.UploadProgress != null)
			{
				this.UploadProgress(this, e);
			}
			if (Cpu != null)
			{
				Cpu.Modules.OnUploadProgress(this, e);
			}
			if (this is Task && Cpu != null)
			{
				Cpu.Tasks.OnUploadProgress(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				if (this is Task)
				{
					foreach (TaskCollection value in ((Task)this).propUserCollections.Values)
					{
						value.OnUploadProgress((Task)this, e);
					}
				}
				else
				{
					foreach (BaseCollection value2 in propUserCollections.Values)
					{
						if (value2 is TaskCollection)
						{
							((TaskCollection)value2).OnUploadProgress(this, e);
						}
						else
						{
							((ModuleCollection)value2).OnUploadProgress(this, e);
						}
					}
				}
			}
		}

		protected virtual void OnDownloadProgress(ModuleEventArgs e)
		{
			if (this.DownloadProgress != null)
			{
				this.DownloadProgress(this, e);
			}
			if (Cpu != null)
			{
				Cpu.Modules.OnDownloadProgress(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					if (value is TaskCollection)
					{
						((TaskCollection)value).OnDownloadProgress(this, e);
					}
					else
					{
						((ModuleCollection)value).OnDownloadProgress(this, e);
					}
				}
			}
		}

		protected internal override void OnError(PviEventArgs e)
		{
			base.OnError(e);
			if (Cpu != null && Cpu.Modules != null)
			{
				Cpu.Modules.OnError(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0 && !(this is Task))
			{
				foreach (BaseCollection value in propUserCollections.Values)
				{
					value.OnError(this, e);
				}
			}
		}

		protected internal void OnCancelled(PviEventArgs e)
		{
			if (this.Cancelled != null)
			{
				this.Cancelled(this, e);
			}
		}

		private void MemoryTypeFromXmlTextReader(XmlTextReader reader, Module moduleObj)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "MemoryType", ref attributeValue))
			{
				switch (attributeValue.ToLower())
				{
				case "dram":
					moduleObj.propMemoryLocation = MemoryType.Dram;
					break;
				case "fixram":
					moduleObj.propMemoryLocation = MemoryType.FixRam;
					break;
				case "globalanalog":
					moduleObj.propMemoryLocation = MemoryType.GlobalAnalog;
					break;
				case "globaldigital":
					moduleObj.propMemoryLocation = MemoryType.GlobalDigital;
					break;
				case "oo":
					moduleObj.propMemoryLocation = MemoryType.Io;
					break;
				case "memcard":
					moduleObj.propMemoryLocation = MemoryType.MemCard;
					break;
				case "os":
					moduleObj.propMemoryLocation = MemoryType.Os;
					break;
				case "permanent":
					moduleObj.propMemoryLocation = MemoryType.Permanent;
					break;
				case "systemram":
					moduleObj.propMemoryLocation = MemoryType.SystemRam;
					break;
				case "systemrom":
					moduleObj.propMemoryLocation = MemoryType.SystemRom;
					break;
				case "tmp":
					moduleObj.propMemoryLocation = MemoryType.Tmp;
					break;
				case "userram":
					moduleObj.propMemoryLocation = MemoryType.UserRam;
					break;
				case "userrom":
					moduleObj.propMemoryLocation = MemoryType.UserRom;
					break;
				case "sysinternal":
					moduleObj.propMemoryLocation = MemoryType.SysInternal;
					break;
				case "remanent":
					moduleObj.propMemoryLocation = MemoryType.Remanent;
					break;
				case "systemsettings":
					moduleObj.propMemoryLocation = MemoryType.SystemSettings;
					break;
				case "transfermodule":
					moduleObj.propMemoryLocation = MemoryType.TransferModule;
					break;
				}
			}
		}

		private void DomainStateFromXmlTextReader(XmlTextReader reader, Module moduleObj)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "DomainState", ref attributeValue))
			{
				switch (attributeValue.ToLower())
				{
				case "complete":
					moduleObj.propDMState = DomainState.Complete;
					break;
				case "existent":
					moduleObj.propDMState = DomainState.Existent;
					break;
				case "incomplete":
					moduleObj.propDMState = DomainState.Incomplete;
					break;
				case "invalid":
					moduleObj.propDMState = DomainState.Invalid;
					break;
				case "loading":
					moduleObj.propDMState = DomainState.Loading;
					break;
				case "nonexistent":
					moduleObj.propDMState = DomainState.NonExistent;
					break;
				case "ready":
					moduleObj.propDMState = DomainState.Ready;
					break;
				case "use":
					moduleObj.propDMState = DomainState.Use;
					break;
				case "valid":
					moduleObj.propDMState = DomainState.Valid;
					break;
				}
			}
		}

		private void TaskClassTypeFromXmlTextReader(XmlTextReader reader, Module moduleObj)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "TaskClass", ref attributeValue))
			{
				switch (attributeValue.ToLower())
				{
				case "cyclic1":
					moduleObj.propTaskClass = TaskClassType.Cyclic1;
					break;
				case "cyclic2":
					moduleObj.propTaskClass = TaskClassType.Cyclic2;
					break;
				case "cyclic3":
					moduleObj.propTaskClass = TaskClassType.Cyclic3;
					break;
				case "cyclic4":
					moduleObj.propTaskClass = TaskClassType.Cyclic4;
					break;
				case "cyclic5":
					moduleObj.propTaskClass = TaskClassType.Cyclic5;
					break;
				case "cyclic6":
					moduleObj.propTaskClass = TaskClassType.Cyclic6;
					break;
				case "cyclic7":
					moduleObj.propTaskClass = TaskClassType.Cyclic7;
					break;
				case "cyclic8":
					moduleObj.propTaskClass = TaskClassType.Cyclic8;
					break;
				case "timer1":
					moduleObj.propTaskClass = TaskClassType.Timer1;
					break;
				case "timer2":
					moduleObj.propTaskClass = TaskClassType.Timer2;
					break;
				case "timer3":
					moduleObj.propTaskClass = TaskClassType.Timer3;
					break;
				case "timer4":
					moduleObj.propTaskClass = TaskClassType.Timer4;
					break;
				case "exception":
					moduleObj.propTaskClass = TaskClassType.Exception;
					break;
				case "interrupt":
					moduleObj.propTaskClass = TaskClassType.Interrupt;
					break;
				case "notvalid":
					moduleObj.propTaskClass = TaskClassType.NotValid;
					break;
				}
			}
		}

		private void TypeFromXmlTextReader(XmlTextReader reader, Module moduleObj)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "Type", ref attributeValue))
			{
				switch (attributeValue.ToLower())
				{
				case "update":
					moduleObj.propType = ModuleType.Update;
					break;
				case "contents":
					moduleObj.propType = ModuleType.Contents;
					break;
				case "contents2":
					moduleObj.propType = ModuleType.Contents2;
					break;
				case "memoryextension":
					moduleObj.propType = ModuleType.MemoryExtension;
					break;
				case "bootmodule":
					moduleObj.propType = ModuleType.BootModule;
					break;
				case "plctask":
					moduleObj.propType = ModuleType.PlcTask;
					break;
				case "systemtask":
					moduleObj.propType = ModuleType.SystemTask;
					break;
				case "usertask":
					moduleObj.propType = ModuleType.UserTask;
					break;
				case "timertask":
					moduleObj.propType = ModuleType.TimerTask;
					break;
				case "interrupttask":
					moduleObj.propType = ModuleType.InterruptTask;
					break;
				case "exceptiontask":
					moduleObj.propType = ModuleType.ExceptionTask;
					break;
				case "startup":
					moduleObj.propType = ModuleType.Startup;
					break;
				case "avtlib":
					moduleObj.propType = ModuleType.AvtLib;
					break;
				case "syslib":
					moduleObj.propType = ModuleType.SysLib;
					break;
				case "hwlib":
					moduleObj.propType = ModuleType.HwLib;
					break;
				case "comlib":
					moduleObj.propType = ModuleType.ComLib;
					break;
				case "mathlib":
					moduleObj.propType = ModuleType.MathLib;
					break;
				case "lib":
					moduleObj.propType = ModuleType.Lib;
					break;
				case "instlib":
					moduleObj.propType = ModuleType.InstLib;
					break;
				case "addlib":
					moduleObj.propType = ModuleType.AddLib;
					break;
				case "io":
					moduleObj.propType = ModuleType.Io;
					break;
				case "iomap":
					moduleObj.propType = ModuleType.IoMap;
					break;
				case "datamodule":
					moduleObj.propType = ModuleType.DataModule;
					break;
				case "table":
					moduleObj.propType = ModuleType.Table;
					break;
				case "asfwhw":
					moduleObj.propType = ModuleType.AsFwHw;
					break;
				case "assafetymodule":
					moduleObj.propType = ModuleType.AsSafetyModule;
					break;
				case "ncdriver":
					moduleObj.propType = ModuleType.NcDriver;
					break;
				case "acp10":
					moduleObj.propType = ModuleType.ACP10;
					break;
				case "profilere":
					moduleObj.propType = ModuleType.ProfilerE;
					break;
				case "profilerdata":
					moduleObj.propType = ModuleType.ProfilerData;
					break;
				case "tracerdefinition":
					moduleObj.propType = ModuleType.TracerDefinition;
					break;
				case "tracerdata":
					moduleObj.propType = ModuleType.TracerData;
					break;
				case "ncupdate":
					moduleObj.propType = ModuleType.NcUpdate;
					break;
				case "history":
					moduleObj.propType = ModuleType.History;
					break;
				case "error":
					moduleObj.propType = ModuleType.Error;
					break;
				case "logger":
					moduleObj.propType = ModuleType.Logger;
					break;
				case "noc":
					moduleObj.propType = ModuleType.Noc;
					break;
				case "merker":
					moduleObj.propType = ModuleType.Merker;
					break;
				case "tkloc":
					moduleObj.propType = ModuleType.TkLoc;
					break;
				case "plcconfig":
					moduleObj.propType = ModuleType.PlcConfig;
					break;
				case "comconfig":
					moduleObj.propType = ModuleType.ComConfig;
					break;
				case "ppconfig":
					moduleObj.propType = ModuleType.PpConfig;
					break;
				case "ioconfig":
					moduleObj.propType = ModuleType.IOConfig;
					break;
				case "opcconfig":
					moduleObj.propType = ModuleType.OpcConfig;
					break;
				case "opcuaconfig":
					moduleObj.propType = ModuleType.OpcUaConfig;
					break;
				case "config":
					moduleObj.propType = ModuleType.Config;
					break;
				case "exception":
					moduleObj.propType = ModuleType.Exception;
					break;
				case "interrupt":
					moduleObj.propType = ModuleType.Interrupt;
					break;
				case "device":
					moduleObj.propType = ModuleType.Device;
					break;
				case "textsystemmodule":
					moduleObj.propType = ModuleType.TextSystemModule;
					break;
				case "exe":
					moduleObj.propType = ModuleType.Exe;
					break;
				case "probe":
					moduleObj.propType = ModuleType.Probe;
					break;
				case "probeio":
					moduleObj.propType = ModuleType.ProbeIo;
					break;
				case "osexe":
					moduleObj.propType = ModuleType.OsExe;
					break;
				case "data":
					moduleObj.propType = ModuleType.Data;
					break;
				case "unknown":
					moduleObj.propType = ModuleType.Unknown;
					break;
				}
			}
		}

		private void ProgramStateFromXmlTextReader(XmlTextReader reader, Module moduleObj)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "ProgramState", ref attributeValue))
			{
				switch (attributeValue.ToLower())
				{
				case "idle":
					moduleObj.propPIState = ProgramState.Idle;
					break;
				case "nonexistent":
					moduleObj.propPIState = ProgramState.NonExistent;
					break;
				case "resetting":
					moduleObj.propPIState = ProgramState.Resetting;
					break;
				case "resuming":
					moduleObj.propPIState = ProgramState.Resuming;
					break;
				case "running":
					moduleObj.propPIState = ProgramState.Running;
					break;
				case "starting":
					moduleObj.propPIState = ProgramState.Starting;
					break;
				case "stopped":
					moduleObj.propPIState = ProgramState.Stopped;
					break;
				case "stopping":
					moduleObj.propPIState = ProgramState.Stopping;
					break;
				case "unrunnable":
					moduleObj.propPIState = ProgramState.Unrunnable;
					break;
				}
			}
		}

		private bool AttributeFromXmlTextReader(XmlTextReader reader, string nameOfAttribute, ref string attributeValue)
		{
			attributeValue = reader.GetAttribute(nameOfAttribute);
			if (attributeValue != null && attributeValue.Length > 0)
			{
				return true;
			}
			return false;
		}

		private void AttributeFromXmlTextReader(XmlTextReader reader, ref string propertyValue, string nameOfAttribute)
		{
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, nameOfAttribute, ref attributeValue))
			{
				propertyValue = attributeValue;
			}
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			Module module = (Module)baseObj;
			if (module == null)
			{
				return -1;
			}
			base.FromXmlTextReader(ref reader, flags, module);
			DateTime result = DateTime.MinValue;
			string attributeValue = "";
			if (AttributeFromXmlTextReader(reader, "CreationTime", ref attributeValue) && PviParse.TryParseDateTime(attributeValue, out result))
			{
				module.propCreateTime = Pvi.GetDateTimeUInt32(result);
			}
			AttributeFromXmlTextReader(reader, ref module.propCreateName, "CreationName");
			if (AttributeFromXmlTextReader(reader, "LastWriteTime", ref attributeValue) && PviParse.TryParseDateTime(attributeValue, out result))
			{
				module.propAndTime = Pvi.GetDateTimeUInt32(result);
			}
			AttributeFromXmlTextReader(reader, ref module.propAndName, "LastWriteName");
			if (AttributeFromXmlTextReader(reader, "Length", ref attributeValue))
			{
				uint result2 = 0u;
				if (PviParse.TryParseUInt32(attributeValue, out result2))
				{
					module.propLength = result2;
				}
			}
			if (AttributeFromXmlTextReader(reader, "DigitalMemoryAddress", ref attributeValue))
			{
				uint result3 = 0u;
				if (PviParse.TryParseUInt32(attributeValue, out result3))
				{
					module.propDigitalMemoryAddess = result3;
				}
			}
			if (AttributeFromXmlTextReader(reader, "AnalogMemoryAddress", ref attributeValue))
			{
				uint result4 = 0u;
				if (PviParse.TryParseUInt32(attributeValue, out result4))
				{
					module.propAnalogMemoryAddress = result4;
				}
			}
			if (AttributeFromXmlTextReader(reader, "DomainIndex", ref attributeValue))
			{
				ushort result5 = 0;
				if (PviParse.TryParseUInt16(attributeValue, out result5))
				{
					module.propDMIndex = result5;
				}
			}
			DomainStateFromXmlTextReader(reader, module);
			if (AttributeFromXmlTextReader(reader, "InstallNumber", ref attributeValue))
			{
				byte result6 = 0;
				if (PviParse.TryParseByte(attributeValue, out result6))
				{
					module.propInstallNumber = result6;
				}
			}
			if (AttributeFromXmlTextReader(reader, "ModUID", ref attributeValue))
			{
				uint result7 = 0u;
				if (PviParse.TryParseUInt32(attributeValue, out result7))
				{
					module.propModUID = result7;
				}
			}
			if (AttributeFromXmlTextReader(reader, "PiCount", ref attributeValue))
			{
				byte result8 = 0;
				if (PviParse.TryParseByte(attributeValue, out result8))
				{
					module.propPICount = result8;
				}
			}
			if (AttributeFromXmlTextReader(reader, "PiIndex", ref attributeValue))
			{
				ushort result9 = 0;
				if (PviParse.TryParseUInt16(attributeValue, out result9))
				{
					module.propPIIndex = result9;
				}
			}
			if (AttributeFromXmlTextReader(reader, "StartAddress", ref attributeValue))
			{
				uint result10 = 0u;
				if (PviParse.TryParseUInt32(attributeValue, out result10))
				{
					module.propStartAddress = result10;
				}
			}
			if (AttributeFromXmlTextReader(reader, "VariableCount", ref attributeValue))
			{
				ushort result11 = 0;
				if (PviParse.TryParseUInt16(attributeValue, out result11))
				{
					module.propPVCount = result11;
				}
			}
			if (AttributeFromXmlTextReader(reader, "VariableIndex", ref attributeValue))
			{
				ushort result12 = 0;
				if (PviParse.TryParseUInt16(attributeValue, out result12))
				{
					module.propPVIndex = result12;
				}
			}
			if (AttributeFromXmlTextReader(reader, "Version", ref attributeValue))
			{
				byte result13 = 0;
				if (PviParse.TryParseByte(attributeValue, out result13))
				{
					module.propVersion = result13;
				}
			}
			TaskClassTypeFromXmlTextReader(reader, module);
			TypeFromXmlTextReader(reader, module);
			ProgramStateFromXmlTextReader(reader, module);
			if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
			{
				module.Requests |= Actions.Connect;
			}
			if (AttributeFromXmlTextReader(reader, "InstallationPriority", ref attributeValue))
			{
				byte result14 = 0;
				if (PviParse.TryParseByte(attributeValue, out result14))
				{
					module.propInstallationPriority = result14;
				}
			}
			reader.Read();
			return 0;
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteStartElement("Module");
			int result = SaveModuleConfiguration(ref writer, flags);
			writer.WriteEndElement();
			return result;
		}

		public int SaveModuleConfiguration(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			base.ToXMLTextWriter(ref writer, flags);
			if (propCreateName != null && propCreateName.Length > 0)
			{
				writer.WriteAttributeString("CreationName", propCreateName.ToString());
			}
			writer.WriteAttributeString("CreationTime", Pvi.UInt32ToDateTime(propCreateTime).ToString());
			if (propAndName != null && propAndName.Length > 0)
			{
				writer.WriteAttributeString("LastWriteName", propAndName);
			}
			writer.WriteAttributeString("LastWriteTime", Pvi.UInt32ToDateTime(propAndTime).ToString());
			if (propLength != 0)
			{
				writer.WriteAttributeString("Length", propLength.ToString());
			}
			if (propDigitalMemoryAddess != 0)
			{
				writer.WriteAttributeString("DigitalMemoryAddress", propDigitalMemoryAddess.ToString());
			}
			if (propAnalogMemoryAddress != 0)
			{
				writer.WriteAttributeString("AnalogMemoryAddress", propAnalogMemoryAddress.ToString());
			}
			writer.WriteAttributeString("MemoryType", propMemoryLocation.ToString());
			if (propDMIndex != 0)
			{
				writer.WriteAttributeString("DomainIndex", propDMIndex.ToString());
			}
			writer.WriteAttributeString("DomainState", propDMState.ToString());
			if (propInstallNumber != 0)
			{
				writer.WriteAttributeString("InstallNumber", propInstallNumber.ToString());
			}
			if (propModUID != 0)
			{
				writer.WriteAttributeString("ModUID", propModUID.ToString());
			}
			if (propPICount != 0)
			{
				writer.WriteAttributeString("PiCount", propPICount.ToString());
			}
			if (propPIIndex != 0)
			{
				writer.WriteAttributeString("PiIndex", propPIIndex.ToString());
			}
			if (propStartAddress != 0)
			{
				writer.WriteAttributeString("StartAddress", propStartAddress.ToString());
			}
			if (propPVCount != 0)
			{
				writer.WriteAttributeString("VariableCount", propPVCount.ToString());
			}
			if (propPVIndex != 0)
			{
				writer.WriteAttributeString("VariableIndex", propPVIndex.ToString());
			}
			writer.WriteAttributeString("Version", propVersion.ToString());
			writer.WriteAttributeString("TaskClassType", propTaskClass.ToString());
			writer.WriteAttributeString("Type", propType.ToString());
			if (ProgramState != ProgramState.Running)
			{
				writer.WriteAttributeString("ProgramState", ProgramState.ToString());
			}
			if (propInstallationPriority != 0)
			{
				writer.WriteAttributeString("InstallationPriority", propInstallNumber.ToString());
			}
			return 0;
		}

		internal void OnProceeding(int percentComplete, int errorCode)
		{
			ModuleEventArgs e = new ModuleEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleProgressEvent, this, percentComplete);
			if (propLastAction == Actions.Upload)
			{
				OnUploadProgress(e);
			}
			else
			{
				OnDownloadProgress(e);
			}
		}

		[CLSCompliant(false)]
		protected void ANSLModuleDescriptionRead(IntPtr pData, uint dataLen, int errorCode)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			int num = 0;
			int errorCode2 = 12055;
			try
			{
				if (dataLen == 0 || errorCode != 0)
				{
					if (errorCode != 0)
					{
						errorCode2 = errorCode;
					}
					OnError(new PviEventArgs(base.Name, base.Address, errorCode2, Service.Language, Action.ModuleInfo, Service));
				}
				else
				{
					byte[] array = new byte[dataLen];
					Marshal.Copy(pData, array, 0, (int)dataLen);
					MemoryStream input = new MemoryStream(array);
					XmlTextReader xmlTextReader = new XmlTextReader(input);
					xmlTextReader.MoveToContent();
					while (!xmlTextReader.EOF && xmlTextReader.NodeType != XmlNodeType.EndElement)
					{
						if (xmlTextReader.Name.CompareTo("TaskInfo") == 0 || xmlTextReader.Name.CompareTo("ModulInfo") == 0 || xmlTextReader.Name.CompareTo("ModInfo") == 0)
						{
							propModuleInfoRequested = false;
							num = moduleInfoDescription.ReadFromXML(xmlTextReader);
							if (moduleInfoDescription.name != null && (moduleInfoDescription.name.CompareTo(base.Address) == 0 || moduleInfoDescription.name.CompareTo(base.Name) == 0))
							{
								updateProperties(moduleInfoDescription);
								if (num != 0)
								{
									base.OnPropertyChanged(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.ModuleChangedEvent));
								}
							}
							propModuleInfoRequested = false;
						}
						xmlTextReader.Read();
					}
				}
			}
			catch
			{
				errorCode2 = 12054;
			}
		}
	}
}
