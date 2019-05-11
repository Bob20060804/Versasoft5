using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;

namespace BR.AN.PviServices
{
	[CLSCompliant(true)]
	public class Cpu : Base
	{
		public const string KW_DETECT_SYSLOGBOOK_NAME = "$Detect_SG4_SysLogger$";

		private const string TocRequestCommand = "TYPE=1 ARG=0";

		private bool propFireConChanged;

		private bool propHasARLogSysErrorEVH;

		internal string actDownLoadModuleName;

		internal string actUpLoadModuleName;

		internal ArrayList listOfDownLoadModules;

		internal ArrayList listOfUpLoadModules;

		internal TransferRequest _TransferRequest;

		private bool ignoreEvents;

		private string activeConnSettings;

		private Logger propARLogSys;

		private uint propErrorLogBookModUID;

		private string propApplicationModuleFilter;

		internal bool propNewConnection;

		private TargetObjectCollection propObjectsOnTarget;

		private HardwareInfo propHardwareInfos;

		private BatteryStates propAccu;

		private BatteryStates propBattery;

		internal bool propIsSG4Target;

		private bool propRestarted;

		private TaskClassCollection propTaskClasses;

		private MemoryCollection propMemories;

		private VariableCollection propVariables;

		private TaskCollection propTasks;

		private ModuleCollection propModules;

		private ModuleCollection propTextModules;

		private LibraryCollection propLibraries;

		private IODataPointCollection propIODataPoints;

		private Connection propConnection;

		private string propSWVersion;

		private string propCPUName;

		private string propCPUType;

		private string propAWSType;

		private ushort propNodeNumber;

		private BootMode propInitDescription;

		private CpuState propState;

		private byte propVoltage;

		private byte propCpuUsage;

		private byte propCurrentCyclicSystemUsage;

		private string propHost;

		private DateTime propDateTime;

		private string propSavePath;

		private TcpDestinationSettings propTCPDestinationSettings;

		private LoggerCollection propLoggers;

		internal ConnectionType propCPUState;

		internal Hashtable propUserCollections;

		internal Hashtable propUserModuleCollections;

		internal Hashtable propUserTaskCollections;

		internal Hashtable propUserLoggerCollections;

		internal Profiler propProfiler;

		private bool propModuleInfoRequested;

		internal Hashtable propModuleInfoList;

		private PviObjectBrowser propObjectBrowser;

		private EventWaitHandle hWaitOnDongles;

		private EventWaitHandle hWaitOnListOfExistingLicenses;

		private EventWaitHandle hWaitOnListOfRequiredLicenses;

		private EventWaitHandle hWaitOnReadContext;

		private EventWaitHandle hWaitOnUpdateLicense;

		private EventWaitHandle hWaitOnLicenseStatus;

		private bool licListDongles;

		private bool licListOfExistingLicenses;

		private bool licListOfRequiredLicenses;

		private bool licReadContext;

		private bool licUpdateLicense;

		private bool licBlinkDongle;

		private PviFunction cbLICReadFunc;

		private PviFunction cbLICWriteFunc;

		private string propLicStatus;

		private uint propLicStatusError;

		private MemoryType propClearmemType;

		private string propMemoryInfo;

		private MemoryInformation propMemoryInformationStruct;

		private string propHardwareInfo;

		private HardwareInformation propHardwareInformationStruct;

		private bool propRedundancyCommMode;

		private string propRedundancyInfo;

		private readonly object propRedundancyInfoLock = new object();

		private RedundancyInformation propRedundancyInformationStruct;

		private readonly object propRedundancyInformationStructLock = new object();

		private BondInformation propBondInformation;

		private readonly object propBondInformationStructLock = new object();

		public override bool IsConnected
		{
			get
			{
				if (ConnectionStates.Connected == propConnectionState)
				{
					return true;
				}
				return false;
			}
		}

		internal uint ModUID => propErrorLogBookModUID;

		public string ApplicationModuleFilter
		{
			get
			{
				return propApplicationModuleFilter;
			}
			set
			{
				propApplicationModuleFilter = value;
			}
		}

		public int ResponseTimeout
		{
			get
			{
				return propConnection.Device.ResponseTimeout;
			}
			set
			{
				propConnection.Device.ResponseTimeout = value;
			}
		}

		public Connection Connection
		{
			get
			{
				return propConnection;
			}
			set
			{
				propNewConnection = true;
				if (propConnection != null)
				{
					propConnection.Connected -= ConnectionEvent;
					propConnection.ConnectionChanged -= connection_ConnectionChanged;
					propConnection.Error -= ConnectionEvent;
					propConnection.Disconnected -= ConnectionDisconnected;
				}
				value.pviLineObj.propLinkId = propConnection.pviLineObj.propLinkId;
				value.pviDeviceObj.propLinkId = propConnection.pviDeviceObj.propLinkId;
				value.pviStationObj.propLinkId = propConnection.pviStationObj.propLinkId;
				if (!propConnection.propDeviceIsDirty && (propConnection.DeviceType == value.DeviceType || (propConnection.DeviceType == DeviceType.ANSLTcp && value.DeviceType == DeviceType.TcpIp) || (value.DeviceType == DeviceType.ANSLTcp && propConnection.DeviceType == DeviceType.TcpIp)))
				{
					value.propDeviceIsDirty = false;
				}
				value.propLineDesc = propConnection.propLineDesc;
				value.propDeviceDesc = propConnection.propDeviceDesc;
				propConnection = value;
				propConnection.Connected += ConnectionEvent;
				propConnection.ConnectionChanged += connection_ConnectionChanged;
				propConnection.Disconnected += ConnectionDisconnected;
				propConnection.Error += ConnectionEvent;
			}
		}

		public ModuleCollection TextModules => propTextModules;

		public TargetObjectCollection ObjectsOnTarget => propObjectsOnTarget;

		public ModuleCollection Modules => propModules;

		public TaskCollection Tasks => propTasks;

		public VariableCollection Variables => propVariables;

		public MemoryCollection Memories => propMemories;

		public TaskClassCollection TaskClasses => propTaskClasses;

		public HardwareInfo HardwareInfos => propHardwareInfos;

		public LibraryCollection Libraries => propLibraries;

		public string RuntimeVersion
		{
			get
			{
				if (propSWVersion == null || propSWVersion == "")
				{
					return string.Empty;
				}
				string text = propSWVersion;
				if (-1 == propSWVersion.IndexOf('.'))
				{
					if (propSWVersion.Length == 4)
					{
						text = $"{propSWVersion[0]}{propSWVersion[1]}.{propSWVersion[2]}{propSWVersion[3]}";
					}
					else if (propSWVersion.Length == 5)
					{
						text = $"{propSWVersion[0]}{propSWVersion[1]}.{propSWVersion[2]}{propSWVersion[3]}.{propSWVersion[4]}";
					}
				}
				if ((propSWVersion.Length == 4 || propSWVersion.Length == 5) && '.' == text[3] && '0' == text[1])
				{
					text = text[0] + text.Substring(2);
				}
				return text;
			}
		}

		public string Type => propCPUType;

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		public string ApplicationMemory
		{
			get
			{
				return propAWSType;
			}
		}

		public short NodeNumber => (short)propNodeNumber;

		public BatteryStates Accu => propAccu;

		public byte CpuUsage => propCpuUsage;

		public byte CurrentCyclicSystemUsage => propCurrentCyclicSystemUsage;

		public BatteryStates Battery => propBattery;

		public BootMode BootMode => propInitDescription;

		public CpuState State => propState;

		public override string FullName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return Parent.FullName + "." + base.Name;
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
				if (base.Name != null && 0 < base.Name.Length)
				{
					if (Connection != null)
					{
						return Connection.PviPathName + "/\"" + Service.Name + "." + base.Name + "\" OT=Cpu";
					}
					return Parent.PviPathName + "/\"" + Service.Name + "." + base.Name + "\" OT=Cpu";
				}
				if (Connection != null)
				{
					return Connection.PviPathName;
				}
				return Parent.PviPathName;
			}
		}

		public bool HasErrorLogBook => !propIsSG4Target;

		public bool IsSG4Target => propIsSG4Target;

		public IODataPointCollection IODataPoints => propIODataPoints;

		public string SavePath
		{
			get
			{
				return propSavePath;
			}
			set
			{
				propSavePath = value;
			}
		}

		public LoggerCollection Loggers => propLoggers;

		public Profiler Profiler => propProfiler;

		internal PviObjectBrowser ObjectBrowser => propObjectBrowser;

		public TcpDestinationSettings TcpDestinationSettings => propTCPDestinationSettings;

		public string LicenseStatusInfo => propLicStatus;

		[CLSCompliant(false)]
		public uint LicenseStatusError
		{
			get
			{
				return propLicStatusError;
			}
		}

		public string MemoryInfo => propMemoryInfo;

		public MemoryInformation MemoryInformationStruct => propMemoryInformationStruct;

		public string HardwareInfo => propHardwareInfo;

		public HardwareInformation HardwareInformationStruct => propHardwareInformationStruct;

		public bool RedundancyCommMode
		{
			get
			{
				return propRedundancyCommMode;
			}
			set
			{
				propRedundancyCommMode = value;
			}
		}

		public string RedundancyInfo
		{
			get
			{
				lock (propRedundancyInfoLock)
				{
					return propRedundancyInfo;
				}
			}
			internal set
			{
				lock (propRedundancyInfoLock)
				{
					propRedundancyInfo = value;
				}
			}
		}

		public RedundancyInformation RedundancyInformationStruct
		{
			get
			{
				lock (propRedundancyInformationStructLock)
				{
					return propRedundancyInformationStruct;
				}
			}
			internal set
			{
				lock (propRedundancyInformationStructLock)
				{
					propRedundancyInformationStruct = value;
				}
			}
		}

		public BondInformation BondInformationStruct
		{
			get
			{
				lock (propRedundancyInformationStructLock)
				{
					return propBondInformation;
				}
			}
			internal set
			{
				lock (propRedundancyInformationStructLock)
				{
					propBondInformation = value;
				}
			}
		}

		internal bool HasAnslConnection
		{
			get
			{
				if (Connection.DeviceType == DeviceType.ANSLTcp)
				{
					return true;
				}
				return false;
			}
		}

		internal event PviReadResponseHandler PviReadResponse;

		internal event PviWriteResponseHandler PviWriteResponse;

		public event PviEventHandler Restarted;

		public event CpuEventHandler DateTimeRead;

		public event CpuEventHandler DateTimeWritten;

		public event PviEventHandler SavePathWritten;

		public event PviEventHandler SavePathRead;

		public event PviEventHandler ModuleDeleted;

		public event PviEventHandler TCPDestinationSettingsRead;

		public event PviEventHandler CpuInfoUpdated;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public event CpuTTServiceEventHandler TTServiceResponse;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event PviEventHandler PhysicalMemoryWritten;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public event CpuPhysicalMemReadEventHandler PhysicalMemoryRead;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event PviEventHandler GlobalForcedOFF;

		public event PviEventHandler LicenseStatusGot;

		public event PviEventHandler MemoryCleared;

		public event PviEventHandler MemoryInfoRead;

		public event PviEventHandler HardwareInfoRead;

		public event PviEventHandlerXmlApplicationInfo ApplicationInfoRead;

		public event PviEventHandler RedundancyInfoRead = delegate
		{
		};

		public event PviEventHandler RedundancyInfoChanged;

		public event PviEventHandler ActiveCpuChanged;

		public event PviEventHandler ApplicationSynchronizeStarted;

		public event PviProgressHandler ApplicationSyncing;

		public event PviEventHandlerXmlData TOCRead;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public event PviEventHandlerXmlData XMLCommandSent;

		public event PviEventHandlerXmlData BondInfoChanged;

		public event PviEventHandlerXmlData LicenseChanged;

		public Cpu(Service service, string name)
			: base(service, name)
		{
			propFireConChanged = false;
			propHasARLogSysErrorEVH = false;
			propARLogSys = null;
			propIsSG4Target = true;
			propObjectBrowser = null;
			Init(service, name);
		}

		internal Cpu(PviObjectBrowser objBrowser, string name)
			: base(objBrowser.Service, name)
		{
			propFireConChanged = false;
			propHasARLogSysErrorEVH = false;
			propObjectBrowser = objBrowser;
			propARLogSys = null;
			propIsSG4Target = true;
			Init(propObjectBrowser.Service, name);
		}

		internal void Init(Service service, string name)
		{
			propCpuUsage = 0;
			propCurrentCyclicSystemUsage = 0;
			propLicStatusError = uint.MaxValue;
			propLicStatus = "";
			propRedundancyInfo = "";
			propRedundancyInformationStruct = null;
			propRedundancyCommMode = false;
			activeConnSettings = "";
			propTCPDestinationSettings = new TcpDestinationSettings();
			propHardwareInfos = new HardwareInfo(this);
			propConnectionState = ConnectionStates.Unininitialized;
			propState = CpuState.Offline;
			propAccu = BatteryStates.UNDEFINED;
			propBattery = BatteryStates.UNDEFINED;
			propMemories = new MemoryCollection(this, name + ".Memories");
			propTaskClasses = new TaskClassCollection(this, name);
			propVariables = new VariableCollection(service.CollectionType, this, name + ".Variables");
			propVariables.propInternalCollection = true;
			propModules = new ModuleCollection(CollectionType.HashTable, this, name + ".Modules");
			propModules.propInternalCollection = true;
			propTextModules = new ModuleCollection(ModuleType.TextSystemModule, CollectionType.HashTable, this, name + ".TextModules");
			propTextModules.propInternalCollection = true;
			propTasks = new TaskCollection(service.CollectionType, this, name + ".Tasks");
			propTasks.propInternalCollection = true;
			propIODataPoints = new IODataPointCollection(this, name + ".IODatapoints");
			propLoggers = new LoggerCollection(CollectionType.ArrayList, this, base.Name + ".Loggers");
			propLibraries = new LibraryCollection(this, name + ".Libraries");
			propConnection = new Connection(this);
			propConnection.Connected += ConnectionEvent;
			propConnection.ConnectionChanged += connection_ConnectionChanged;
			propConnection.Disconnected += ConnectionDisconnected;
			propConnection.Error += ConnectionEvent;
			propObjectsOnTarget = new TargetObjectCollection(this);
			propRestarted = false;
			propCPUState = ConnectionType.None;
			service.Cpus.Add(this);
			switch (Service.LogicalObjectsUsage)
			{
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
			propErrorLogBookModUID = Service.ModuleUID;
			hWaitOnDongles = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			hWaitOnListOfExistingLicenses = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			hWaitOnListOfRequiredLicenses = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			hWaitOnReadContext = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			hWaitOnUpdateLicense = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			hWaitOnLicenseStatus = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
			licListDongles = false;
			licListOfExistingLicenses = false;
			licListOfRequiredLicenses = false;
			licReadContext = false;
			licUpdateLicense = false;
			licBlinkDongle = false;
			cbLICReadFunc = PVICB_LIC_ReadFunc;
			cbLICWriteFunc = PVICB_LIC_WriteFunc;
		}

		~Cpu()
		{
			Dispose(disposing: false, removeFromCollection: true);
		}

		internal override void reCreateState()
		{
			propConnectionState = ConnectionStates.Unininitialized;
			propReCreateActive = true;
			propLinkId = 0u;
			Connection.ResetLinkIds();
			if (reCreateActive)
			{
				reCreateActive = false;
				Connect();
				if (!Service.WaitForParentConnection)
				{
					reCreateChildState();
				}
			}
		}

		internal void reCreateChildState()
		{
			Library library = null;
			IODataPoint iODataPoint = null;
			if (propTasks.Values != null)
			{
				foreach (object value in propTasks.Values)
				{
					Task task = (Task)value;
					if (task.isObjectConnected || task.reCreateActive)
					{
						task.reCreateState();
						if (!Service.WaitForParentConnection)
						{
							task.reCreateChildState();
						}
					}
				}
			}
			if (propLoggers.Values != null)
			{
				foreach (object value2 in propLoggers.Values)
				{
					Logger logger = (Logger)value2;
					if (logger.isObjectConnected)
					{
						logger.reCreateState();
					}
				}
			}
			if (propModules.Values != null)
			{
				foreach (object value3 in propModules.Values)
				{
					Module module = (Module)value3;
					if (module.isObjectConnected)
					{
						module.reCreateState();
					}
				}
			}
			if (propTextModules.Values != null)
			{
				foreach (object value4 in propTextModules.Values)
				{
					Module module = (Module)value4;
					if (module.isObjectConnected)
					{
						module.reCreateState();
					}
				}
			}
			if (propLibraries.Values != null)
			{
				foreach (object value5 in propLibraries.Values)
				{
					library = (Library)value5;
					if (library.isObjectConnected)
					{
						library.reCreateState();
					}
				}
			}
			if (propVariables.Values != null)
			{
				foreach (object value6 in propVariables.Values)
				{
					Variable variable = (Variable)value6;
					if (variable.isObjectConnected || variable.reCreateActive)
					{
						variable.reCreateState();
					}
				}
			}
			if (propIODataPoints.Values != null)
			{
				foreach (object value7 in propIODataPoints.Values)
				{
					iODataPoint = (IODataPoint)value7;
					if (iODataPoint.isObjectConnected || iODataPoint.reCreateActive)
					{
						iODataPoint.reCreateState();
					}
				}
			}
		}

		public int ReadCommunicationLibraryVersions(ref Hashtable versionInfos)
		{
			int num = 0;
			int num2 = 4096;
			versionInfos.Clear();
			IntPtr intPtr = PviMarshal.AllocCoTaskMem(num2);
			num = PInvokePvicom.PviComRead(Service.hPvi, propConnection.pviLineObj.LinkId, AccessTypes.Version, IntPtr.Zero, 0, intPtr, num2);
			if (num == 0)
			{
				PviMarshal.GetVersionInfos(intPtr, num2, ref versionInfos);
				num = PInvokePvicom.PviComRead(Service.hPvi, propConnection.pviDeviceObj.LinkId, AccessTypes.Version, IntPtr.Zero, 0, intPtr, num2);
			}
			if (num == 0)
			{
				PviMarshal.GetVersionInfos(intPtr, num2, ref versionInfos);
			}
			return num;
		}

		public override void Connect()
		{
			ignoreEvents = false;
			propNoDisconnectedEvent = false;
			propReturnValue = 0;
			Connect(base.ConnectionType);
		}

		internal string RemoveULorDLName(ref ArrayList modQueue, string modName)
		{
			if (0 < modQueue.Count)
			{
				if (modName == modQueue[0].ToString())
				{
					modQueue.RemoveAt(0);
				}
				if (0 < modQueue.Count)
				{
					return modQueue[0].ToString();
				}
			}
			return "";
		}

		public override void Connect(ConnectionType connectionType)
		{
			actDownLoadModuleName = "";
			actUpLoadModuleName = "";
			_TransferRequest = null;
			listOfDownLoadModules = new ArrayList();
			listOfUpLoadModules = new ArrayList();
			ignoreEvents = false;
			if (reCreateActive || (ConnectionStates.Unininitialized < propConnectionState && ConnectionStates.Disconnecting > propConnectionState))
			{
				if (HasPVIConnection && !reCreateActive)
				{
					Fire_ConnectedEvent(this, new PviEventArgs(propName, propAddress, propErrorCode, Service.Language, Action.CpuConnect, Service));
				}
				return;
			}
			propConnectionState = ConnectionStates.Connecting;
			if (base.LinkId != 0)
			{
				if (propErrorCode == 0)
				{
					propErrorCode = (propReturnValue = 12043);
				}
				else
				{
					propReturnValue = propErrorCode;
				}
				OnError(new PviEventArgs(propName, propAddress, propErrorCode, Service.Language, Action.CpuConnect, Service));
				base.OnConnected(new PviEventArgs(propName, propAddress, propErrorCode, Service.Language, Action.CpuConnect, Service));
				return;
			}
			if (propModuleInfoList != null)
			{
				propModuleInfoList.Clear();
			}
			RedundancyInfo = "";
			RedundancyInformationStruct = null;
			propNoDisconnectedEvent = false;
			base.Requests |= Actions.Connect;
			base.Requests |= Actions.GetCpuInfo;
			propReturnValue = 0;
			base.ConnectionType = connectionType;
			if (propAddress == null || propAddress.Length == 0)
			{
				propAddress = propName;
			}
			if (base.ConnectionType != ConnectionType.Link)
			{
				base.Requests |= Actions.GetLBType;
				if (ConnectionStates.Connecting > propConnection.propConnectionState || ConnectionStates.Connecting > propConnectionState || ConnectionStates.Disconnected == propConnectionState || ConnectionStates.Disconnected == propConnection.propConnectionState)
				{
					propConnection.propConnectionState = ConnectionStates.Unininitialized;
					propConnection.Connect();
					propReturnValue = propConnection.ReturnValue;
				}
			}
			else
			{
				if (Actions.GetLBType == (base.Requests & Actions.GetLBType))
				{
					base.Requests &= ~Actions.GetLBType;
				}
				ConnectionEvent(this, new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.CpuConnect, Service));
			}
			if (propCPUState == ConnectionType.Create)
			{
				ConnectionEvent(this, new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.CpuConnect, Service));
			}
			if (!Service.WaitForParentConnection)
			{
				propReturnValue = ConnectCpu(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.CpuConnect, Service));
			}
		}

		public override int ChangeConnection()
		{
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			ConnectionStates propConnectionState = base.propConnectionState;
			if (base.propConnectionState == ConnectionStates.Connecting || base.propConnectionState == ConnectionStates.Disconnecting || base.propConnectionState == ConnectionStates.ConnectionChanging)
			{
				return 15002;
			}
			if (Connection.ConnectionChangeInProgress(base.propConnectionState))
			{
				return 15002;
			}
			if (0 < activeConnSettings.Length && activeConnSettings.CompareTo(Connection.ToString()) == 0)
			{
				return 4804;
			}
			activeConnSettings = Connection.ToString();
			base.propConnectionState = ConnectionStates.ConnectionChanging;
			if (propModuleInfoList != null)
			{
				propModuleInfoList.Clear();
			}
			RedundancyInfo = "";
			RedundancyInformationStruct = null;
			num = CpuEventsOFF();
			base.Requests |= Actions.GetLBType;
			num = Connection.ChangeConnection();
			propFireConChanged = false;
			if (num == 0)
			{
				propFireConChanged = true;
			}
			else
			{
				base.propConnectionState = propConnectionState;
			}
			return num;
		}

		private int CpuEventsOFF()
		{
			int num = 0;
			string text = "";
			if (base.LinkId == 0)
			{
				return 12004;
			}
			ignoreEvents = true;
			Service.BuildRequestBuffer(text);
			num = Write(Service.hPvi, base.LinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length);
			return Connection.TurnOffEvents();
		}

		private int CpuEventsON()
		{
			int num = 0;
			string text = "edlfp";
			if (base.LinkId == 0)
			{
				return 12004;
			}
			ignoreEvents = false;
			Service.BuildRequestBuffer(text);
			num = WriteRequest(Service.hPvi, base.LinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length, _internId);
			return Connection.TurnOnEvents();
		}

		private int ChangeCpuConnection()
		{
			int num = 0;
			string text = GetConnectionDescription();
			if (base.LinkId == 0)
			{
				return 12004;
			}
			num = text.IndexOf("\"/\"");
			if (-1 != num)
			{
				text = text.Substring(num + 2);
			}
			SysLoggerDisconnect();
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.Connect, text, 217u, _internId);
		}

		public override void Disconnect()
		{
			ignoreEvents = false;
			reCreateActive = false;
			int num = Disconnect(202u);
			if (num != 0 && !propNoDisconnectedEvent)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.CpuDisconnect));
				FireDisconnected(num, Action.CpuDisconnect);
			}
		}

		internal void DisconnectWithChilds()
		{
			if (IODataPoints != null)
			{
				IODataPoints.Disconnect(noResponse: true);
			}
			if (Loggers != null)
			{
				Loggers.Disconnect(noResponse: true);
			}
			if (Modules != null)
			{
				Modules.Disconnect(noResponse: true);
			}
			if (Tasks != null)
			{
				Tasks.Disconnect(noResponse: true);
			}
			if (Variables != null)
			{
				Variables.Disconnect(noResponse: true);
			}
			SysLoggerDisconnect();
			base.Disconnect(noResponse: true);
			if (Connection != null)
			{
				Connection.DisconnectNoResponses();
			}
			propConnectionState = ConnectionStates.Disconnected;
			base.Requests = Actions.NONE;
		}

		public override void Disconnect(bool noResponse)
		{
			ignoreEvents = false;
			SysLoggerDisconnect();
			propFireConChanged = false;
			if (noResponse)
			{
				if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState || base.LinkId != 0)
				{
					base.Disconnect(noResponse: true);
					Connection.DisconnectNoResponses();
					propConnectionState = ConnectionStates.Disconnected;
					base.Requests = Actions.NONE;
				}
				else if (ConnectionStates.Connecting == propConnectionState)
				{
					propConnectionState = ConnectionStates.Disconnecting;
					base.Requests |= Actions.Disconnect;
				}
			}
			else
			{
				Disconnect();
			}
		}

		public void DisconnectChildObjects()
		{
			if (propIODataPoints != null && 0 < propIODataPoints.Count)
			{
				foreach (IODataPoint value in propIODataPoints.Values)
				{
					value.Disconnect(noResponse: true);
				}
			}
			if (propVariables != null && 0 < propVariables.Count)
			{
				foreach (Variable value2 in propVariables.Values)
				{
					value2.Disconnect(noResponse: true);
				}
			}
			if (propLoggers != null && 0 < propLoggers.Count)
			{
				foreach (Logger value3 in propLoggers.Values)
				{
					value3.Disconnect(noResponse: true);
				}
			}
			if (propTasks != null && 0 < propTasks.Count)
			{
				foreach (Task value4 in propTasks.Values)
				{
					if (value4.propVariables != null && 0 < value4.propVariables.Count)
					{
						foreach (Variable value5 in value4.propVariables.Values)
						{
							value5.Disconnect(noResponse: true);
						}
					}
					value4.Disconnect(noResponse: true);
				}
			}
			if (propLibraries != null && 0 < propLibraries.Count)
			{
				foreach (Library value6 in propLibraries.Values)
				{
					value6.Disconnect(noResponse: true);
				}
			}
			if (propTextModules != null && 0 < propTextModules.Count && propTextModules.Values != null)
			{
				foreach (Module value7 in propTextModules.Values)
				{
					value7.Disconnect(noResponse: true);
				}
			}
			if (propModules != null && 0 < propModules.Count && propModules.Values != null)
			{
				foreach (Module value8 in propModules.Values)
				{
					value8.Disconnect(noResponse: true);
				}
			}
		}

		internal override int DisconnectRet(uint action)
		{
			return Disconnect(action);
		}

		private void SysLoggerDisconnect()
		{
			if (propARLogSys != null)
			{
				propARLogSys.Disconnect(noResponse: true);
				propARLogSys.Dispose(disposing: true, removeFromCollection: false);
				propARLogSys = null;
			}
		}

		internal int Disconnect(uint internalAction)
		{
			int result = 0;
			propReturnValue = 0;
			propFireConChanged = false;
			SysLoggerDisconnect();
			if (ConnectionStates.Disconnected == propConnectionState)
			{
				OnDisconnected(new PviEventArgs(propName, propAddress, 4808, Service.Language, Action.CpuDisconnect));
				return 0;
			}
			if (ConnectionStates.Connected < propConnectionState || propConnectionState == ConnectionStates.Unininitialized)
			{
				return 4808;
			}
			propConnectionState = ConnectionStates.Disconnecting;
			if (propDisposed)
			{
				return result;
			}
			if (base.Requests != 0)
			{
				base.Requests = Actions.Disconnect;
				CancelRequest();
				result = DisconnectCpuObjects(internalAction);
				if (result != 0)
				{
					OnDisconnected(new PviEventArgs(propName, propAddress, result, Service.Language, Action.CpuDisconnect));
				}
			}
			else
			{
				result = DisconnectCpuObjects(internalAction);
				if (result != 0)
				{
					OnDisconnected(new PviEventArgs(propName, propAddress, result, Service.Language, Action.CpuDisconnect));
				}
			}
			base.Requests = Actions.NONE;
			return result;
		}

		private int DisconnectCpuObjects(uint internalAction)
		{
			int result = 0;
			if (propLinkId != 0)
			{
				result = (propReturnValue = UnlinkRequest(internalAction));
				propLinkId = 0u;
			}
			else
			{
				OnDisconnected(new PviEventArgs(propName, propAddress, 4808, Service.Language, Action.CpuDisconnect));
			}
			return result;
		}

		private void DisconnectChilds()
		{
			foreach (Variable propVariable in propVariables)
			{
				propVariable.Disconnect();
			}
			foreach (Logger propLogger in propLoggers)
			{
				propLogger.Disconnect();
			}
			foreach (Task propTask in propTasks)
			{
				propTask.Disconnect();
			}
			foreach (Library propLibrary in propLibraries)
			{
				propLibrary.Disconnect();
			}
			if (propTextModules.Values != null)
			{
				foreach (Module value in propTextModules.Values)
				{
					value.Disconnect();
				}
			}
			if (propModules.Values != null)
			{
				foreach (Module value2 in propModules.Values)
				{
					value2.Disconnect();
				}
			}
		}

		protected override string GetConnectionDescription()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			if (propSavePath != null && 0 < propSavePath.Length)
			{
				text2 = "/SP='" + propSavePath + "'";
				if (0 < Connection.ConnectionParameter.Length)
				{
					text2 = " /SP='" + propSavePath + "'";
				}
			}
			else if (Connection.Device.SavePath != null && 0 < Connection.Device.SavePath.Length)
			{
				text2 = "/SP='" + Connection.Device.SavePath + "'";
				if (0 < Connection.ConnectionParameter.Length)
				{
					text2 = " /SP='" + Connection.Device.SavePath + "'";
				}
			}
			if (propApplicationModuleFilter != null)
			{
				text3 = "/AM=" + propApplicationModuleFilter;
				if (0 < text2.Length || 0 < Connection.ConnectionParameter.Length)
				{
					text3 = " /AM=" + propApplicationModuleFilter;
				}
			}
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				if (propRedundancyCommMode || Connection.ANSLTcp.RedundancyCommMode)
				{
					propRedundancyCommMode = true;
					text4 = "/RED=1";
				}
				if (0 < text4.Length)
				{
					text4 += " ";
				}
				text4 += "/BONDEV=1";
				if (0 < text4.Length)
				{
					text4 += " ";
				}
				text4 += "/LICEV=1";
				if (0 < text3.Length || 0 < Connection.ConnectionParameter.Length)
				{
					text4 = " " + text4;
				}
			}
			if (0 < Connection.ConnectionParameter.Length)
			{
				text = ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? $"\"{propConnection.pviStationObj.Name}\"/\"{Connection.ConnectionParameter}{text2}{text3}{text4}\"" : $"\"{Connection.ConnectionParameter.Trim()}{text2}{text3}{text4}\"");
			}
			else if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				text = $"\"{text2}{text3}{text4}\"";
				if (text2.Length == 0 && text3.Length == 0 && text4.Length == 0)
				{
					text = $"\"\"";
				}
			}
			else
			{
				text = $"\"{propConnection.pviStationObj.Name}\"/\"{text2}{text3}{text4}\"";
				if (text2.Length == 0 && text3.Length == 0 && text4.Length == 0)
				{
					text = $"\"{propConnection.pviStationObj.Name}\"/";
				}
			}
			return text;
		}

		private int ConnectCpu(PviEventArgs e)
		{
			propLinkParam = "EV=edlfp";
			int num = 0;
			ignoreEvents = false;
			propConnectionState = ConnectionStates.Connecting;
			propObjectParam = "CD=" + GetConnectionDescription();
			activeConnSettings = Connection.ToString();
			if (!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
			{
				if (Connection.DeviceType < DeviceType.TcpIpMODBUS)
				{
					if (base.ConnectionType == ConnectionType.CreateAndLink)
					{
						num = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_CPU, propObjectParam, 703u, propLinkParam, 201u);
					}
					else if (base.ConnectionType != ConnectionType.Link && propCPUState != ConnectionType.Create)
					{
						num = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_CPU, propObjectParam, 0u, "", 201u);
					}
				}
				else if (!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					num = XCreateRequest(Service.hPvi, Connection.pviStationObj.Name, ObjectType.POBJ_STATION, Connection.pviStationObj.ObjectParam, 201u, propLinkParam, 201u);
				}
				else if (base.ConnectionType != ConnectionType.Link && propCPUState != ConnectionType.Create)
				{
					num = XCreateRequest(Service.hPvi, Connection.pviStationObj.Name, ObjectType.POBJ_STATION, Connection.pviStationObj.ObjectParam, 0u, "", 0u);
				}
			}
			else if (!Service.IsStatic)
			{
				num = ((Service.IsStatic && base.ConnectionType != ConnectionType.Link) ? XLinkRequest(Service.hPvi, base.LinkName, 703u, propLinkParam, 4294967294u, 704u) : XLinkRequest(Service.hPvi, base.LinkName, 703u, propLinkParam, 704u));
			}
			else if (Connection.DeviceType < DeviceType.TcpIpMODBUS)
			{
				if (base.ConnectionType == ConnectionType.CreateAndLink)
				{
					num = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_CPU, propObjectParam, 201u, propLinkParam, 201u);
				}
				else if (base.ConnectionType != ConnectionType.Link && propCPUState != ConnectionType.Create)
				{
					num = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_CPU, propObjectParam, 0u, "", 201u);
				}
			}
			else
			{
				Connection.pviStationObj.Initialize(Connection.pviDeviceObj.Name, $"\"{Connection.ConnectionParameter}\"");
				if (!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					num = XCreateRequest(Service.hPvi, Connection.pviStationObj.Name, ObjectType.POBJ_STATION, Connection.pviStationObj.ObjectParam, 201u, propLinkParam, 201u);
				}
				else if (base.ConnectionType != ConnectionType.Link && propCPUState != ConnectionType.Create)
				{
					num = XCreateRequest(Service.hPvi, Connection.pviStationObj.Name, ObjectType.POBJ_STATION, Connection.pviStationObj.ObjectParam, 0u, "", 201u);
				}
			}
			if (num != 0)
			{
				propConnectionState = ConnectionStates.Unininitialized;
				if (Service.ErrorException)
				{
					string message = $"Connection Error: {num.ToString()}";
					PviException ex = new PviException(message, num, this, e);
					throw ex;
				}
				if (Service.ErrorEvent)
				{
					e.propErrorCode = num;
					OnError(e);
				}
			}
			return num;
		}

		private void connection_ConnectionChanged(object sender, PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				OnError(e);
			}
			ChangeCpuConnection();
		}

		private void ConnectionEvent(object sender, PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				OnError(e);
				OnConnected(e);
			}
			else if (Service.WaitForParentConnection)
			{
				ConnectCpu(e);
			}
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propLinkId = linkID;
			if (errorCode == 0 || 12002 == errorCode)
			{
				base.Requests &= ~Actions.Connect;
				if (1 > linkID && Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					propErrorCode = XLinkRequest(Service.hPvi, base.LinkName, 703u, propLinkParam, 704u);
				}
				else
				{
					propErrorCode = errorCode;
					if (base.ConnectionType == ConnectionType.Link)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuConnect, Service));
					}
				}
				if (Actions.Disconnect == (base.Requests & Actions.Disconnect))
				{
					Disconnect(noResponse: true);
				}
			}
			else
			{
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuConnect, Service));
			}
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
			propConnection.Disconnect();
			if (errorCode != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuDisconnect, Service));
			}
			if (propConnection.ReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propConnection.ReturnValue, Service.Language, Action.CpuDisconnect, Service));
			}
			if (ConnectionStates.Connected != propConnectionState)
			{
				propState = CpuState.Offline;
			}
			if (ConnectionStates.Disconnected == propConnection.propConnectionState)
			{
				OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
			}
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			int num = 0;
			propErrorCode = errorCode;
			propLinkId = linkID;
			if (errorCode == 0)
			{
				if (base.Requests == Actions.NONE)
				{
					propConnectionState = ConnectionStates.Connected;
				}
				if (1 == option)
				{
					num = ReadRequest(Service.hPvi, propLinkId, AccessTypes.List, 120u);
					if (num != 0)
					{
						Service.OnPVIObjectsAttached(new PviEventArgs(propName, propAddress, num, Service.Language, Action.CpuReadTasksList));
					}
				}
			}
			OnLinked(errorCode, Action.CpuLink);
			if (errorCode != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LinkObject, Service));
			}
		}

		private void InterpretAdvancedCPUInfo()
		{
			propAccu = BatteryStates.OK;
			propBattery = BatteryStates.OK;
			if (1 == (propVoltage & 1))
			{
				propAccu = BatteryStates.BAD;
			}
			if (4 == (propVoltage & 4))
			{
				propAccu = BatteryStates.NOT_AVAILABLE;
			}
			if (16 == (propVoltage & 0x10))
			{
				propAccu = BatteryStates.NOT_TESTED;
			}
			if (2 == (propVoltage & 2))
			{
				propBattery = BatteryStates.BAD;
			}
			if (8 == (propVoltage & 8))
			{
				propBattery = BatteryStates.NOT_AVAILABLE;
			}
			if (32 == (propVoltage & 0x20))
			{
				propBattery = BatteryStates.NOT_TESTED;
			}
		}

		private void GetANSLCPUInfo(IntPtr pData, uint dataLen)
		{
			uint num = 0u;
			string text = "";
			try
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(pData, array, 0, (int)dataLen);
				MemoryStream input = new MemoryStream(array);
				XmlTextReader xmlTextReader = new XmlTextReader(input);
				propAWSType = "";
				xmlTextReader.MoveToContent();
				while (xmlTextReader.NodeType != XmlNodeType.EndElement)
				{
					switch (xmlTextReader.Name)
					{
					case "SoftwareVers":
						propSWVersion = xmlTextReader.GetAttribute("AutomationRuntime");
						break;
					case "CpuConfiguration":
						propCPUType = xmlTextReader.GetAttribute("ShortName");
						propCPUName = xmlTextReader.GetAttribute("Type");
						num = uint.Parse(xmlTextReader.GetAttribute("Node"));
						propNodeNumber = 0;
						if (65535 > num)
						{
							propNodeNumber = (ushort)num;
						}
						break;
					case "OperationalValues":
						text = xmlTextReader.GetAttribute("CpuBootMode");
						if (text != null && 0 < text.Length)
						{
							propInitDescription = (BootMode)int.Parse(text);
						}
						text = xmlTextReader.GetAttribute("CurrentCpuState");
						if (text != null && 0 < text.Length)
						{
							if (1 < text.Length)
							{
								switch (text)
								{
								case "RUN":
									propState = CpuState.Run;
									break;
								case "SERVICE":
									propState = CpuState.Service;
									break;
								case "STOP":
									propState = CpuState.Stop;
									break;
								default:
									propState = CpuState.Undefined;
									break;
								}
							}
							else
							{
								propState = (CpuState)int.Parse(text);
							}
						}
						else
						{
							text = xmlTextReader.GetAttribute("CurrentCpuMode");
							if (text != null && 0 < text.Length)
							{
								if (1 < text.Length)
								{
									switch (text)
									{
									case "RUN":
										propState = CpuState.Run;
										break;
									case "STOP":
										propState = CpuState.Stop;
										break;
									case "SERVICE":
										propState = CpuState.Service;
										break;
									case "DIAGNOSIS":
										propState = CpuState.Service;
										break;
									default:
										propState = CpuState.Undefined;
										break;
									}
								}
								else
								{
									switch (text)
									{
									case "1":
										propState = CpuState.Stop;
										break;
									case "2":
										propState = CpuState.Service;
										break;
									case "3":
										propState = CpuState.Service;
										break;
									case "4":
										propState = CpuState.Run;
										break;
									}
								}
							}
						}
						propCpuUsage = 0;
						text = xmlTextReader.GetAttribute("CurrentCpuUsage");
						if (text != null && 0 < text.Length)
						{
							propCpuUsage = byte.Parse(text);
						}
						text = xmlTextReader.GetAttribute("BatteryStatus");
						propVoltage = 0;
						if (text.Length == 0)
						{
							propVoltage = byte.Parse(text);
						}
						propCurrentCyclicSystemUsage = 0;
						text = xmlTextReader.GetAttribute("CurrentCyclicSystemUsage");
						if (text != null && 0 < text.Length)
						{
							propCurrentCyclicSystemUsage = byte.Parse(text);
						}
						break;
					case "NetworkDevices":
						propHost = xmlTextReader.GetAttribute("Host");
						break;
					}
					xmlTextReader.Read();
				}
			}
			catch
			{
				byte[] array = null;
			}
		}

		private void GetINA2000CPUInfo(IntPtr pData, uint dataLen)
		{
			APIFC_CpuInfoRes aPIFC_CpuInfoRes = (APIFC_CpuInfoRes)Marshal.PtrToStructure(pData, typeof(APIFC_CpuInfoRes));
			propSWVersion = aPIFC_CpuInfoRes.sw_version;
			propCPUName = aPIFC_CpuInfoRes.cpu_name;
			propCPUType = aPIFC_CpuInfoRes.cpu_typ;
			propAWSType = aPIFC_CpuInfoRes.aws_typ;
			propNodeNumber = aPIFC_CpuInfoRes.node_nr;
			propInitDescription = (BootMode)aPIFC_CpuInfoRes.init_descr;
			propVoltage = aPIFC_CpuInfoRes.voltage;
			switch (aPIFC_CpuInfoRes.state)
			{
			case 0:
				propState = CpuState.Run;
				break;
			case 1:
				propState = CpuState.Service;
				break;
			case 2:
				propState = CpuState.Stop;
				break;
			case 3:
				propState = CpuState.Offline;
				break;
			default:
				propState = CpuState.Undefined;
				break;
			}
		}

		private void OnPviReadCpuInfo(int errorCode, PVIReadAccessTypes accessType, IntPtr pData, uint dataLen)
		{
			if (errorCode == 0)
			{
				if (PVIReadAccessTypes.ANSL_CpuInfo == accessType)
				{
					GetANSLCPUInfo(pData, dataLen);
				}
				else
				{
					GetINA2000CPUInfo(pData, dataLen);
				}
				InterpretAdvancedCPUInfo();
				if (Actions.GetCpuInfo == (base.Requests & Actions.GetCpuInfo))
				{
					base.Requests &= ~Actions.GetCpuInfo;
				}
				if (Connection.DeviceType == DeviceType.ANSLTcp)
				{
					base.Requests &= ~Actions.GetLBType;
					propIsSG4Target = true;
				}
				if (Actions.GetLBType == (base.Requests & Actions.GetLBType))
				{
					DetectSGxLogger("$Detect_SG4_SysLogger$");
				}
				else if (Connection.ConnectionChangeInProgress(propConnectionState))
				{
					OnConnectionChanged(errorCode, Action.ChangeConnection);
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuConnect, Service));
				}
				if (Actions.Upload == (base.Requests & Actions.Upload))
				{
					base.Requests &= ~Actions.Upload;
					RequestModuleList(ModuleListOptions.INA2000CompatibleMode);
				}
			}
			else
			{
				propErrorCode = errorCode;
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuUploadInfo, Service));
				if (Connection.ConnectionChangeInProgress(propConnectionState))
				{
					OnConnectionChanged(propErrorCode, Action.CpuChangeConnection);
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuUploadInfo, Service));
				}
			}
			OnCpuInfoUpdated(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuUploadInfo, Service));
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			if (21 == option)
			{
				this.PviReadResponse?.Invoke(this, errorCode, accessType, dataState, pData, dataLen);
				return;
			}
			switch (accessType)
			{
			case PVIReadAccessTypes.CPUInfo:
			case PVIReadAccessTypes.ANSL_CpuInfo:
				OnPviReadCpuInfo(errorCode, accessType, pData, dataLen);
				break;
			case PVIReadAccessTypes.DateNTime:
				if (errorCode == 0)
				{
					APIFC_CpuDateTime aPIFC_CpuDateTime = (APIFC_CpuDateTime)Marshal.PtrToStructure(pData, typeof(APIFC_CpuDateTime));
					propDateTime = Pvi.ToDateTime(aPIFC_CpuDateTime.year + 1900, aPIFC_CpuDateTime.mon + 1, aPIFC_CpuDateTime.mday, aPIFC_CpuDateTime.hour, aPIFC_CpuDateTime.min, aPIFC_CpuDateTime.sec);
					CpuEventArgs e = new CpuEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadTime, propDateTime);
					OnDateTimeRead(e);
				}
				else
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadTime, Service));
				}
				break;
			case PVIReadAccessTypes.SavePath:
				if (errorCode == 0)
				{
					propSavePath = PviMarshal.ToAnsiString(pData, dataLen);
					OnSavePathRead(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadSavePath, Service));
				}
				else
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadSavePath, Service));
				}
				break;
			case PVIReadAccessTypes.ResolveNodeNumber:
				if (errorCode != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ResolveNodeNumber, Service));
				}
				OnTCPDestinationSettingsRead(PviMarshal.ToAnsiString(pData, dataLen), new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ResolveNodeNumber, Service));
				break;
			case PVIReadAccessTypes.ModuleList:
				propErrorCode = errorCode;
				INAModuleInfoListFromCB(allModules: true, errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.DiagnoseModuleList:
				propErrorCode = errorCode;
				INAModuleInfoListFromCB(allModules: false, errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ANSL_ModuleList:
				propErrorCode = errorCode;
				ANSLModuleInfoListFromCB(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ReadErrorLogBook:
				if (errorCode == 0)
				{
					LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("ErrorLogBook");
					loggerEntryCollection = GetLogBookEntries(pData, dataLen);
					DumpLoggerEntries(loggerEntryCollection);
				}
				else
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ReadError, Service));
				}
				break;
			case PVIReadAccessTypes.ChildObjects:
				propErrorCode = 4815;
				break;
			case PVIReadAccessTypes.TTService:
				if (20 == option)
				{
					OnTTService(errorCode, accessType, dataState, pData, dataLen);
				}
				else
				{
					propProfiler.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
				}
				break;
			case PVIReadAccessTypes.ReadPhysicalMemory:
				OnPhysicalMemoryRead(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ANSL_RedundancyInfo:
				OnRedundancyInfoRead(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ANSL_MemoryInfo:
				OnMemoryInfoRead(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ANSL_HardwareInfo:
				OnHardwareInfoRead(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.ANSL_CpuExtendedInfo:
				if (19 == option)
				{
					OnTOCRead(propName, propAddress, errorCode, pData, dataLen);
				}
				break;
			case PVIReadAccessTypes.ANSL_ApplicationInfo:
				OnApplicationInfoRead(errorCode, pData, dataLen);
				break;
			case PVIReadAccessTypes.LIC_GetLicenseStatus:
				OnLicenseStatusRead(errorCode, pData, dataLen);
				break;
			default:
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
				break;
			}
		}

		private void DumpLoggerEntries(LoggerEntryCollection eventEntries)
		{
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			if (21 == option)
			{
				this.PviWriteResponse?.Invoke(this, errorCode, accessType, dataState, pData, dataLen);
				return;
			}
			switch (accessType)
			{
			case PVIWriteAccessTypes.State:
				break;
			case PVIWriteAccessTypes.Connection:
				if (propConnectionState == ConnectionStates.ConnectionChanging)
				{
					propConnectionState = ConnectionStates.ConnectionChanged;
				}
				if (errorCode != 0)
				{
					propErrorCode = errorCode;
					OnConnectionChanged(propErrorCode, Action.ChangeConnection);
				}
				else
				{
					UploadCpuInfo((DeviceType.ANSLTcp == Connection.DeviceType) ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
				}
				break;
			case PVIWriteAccessTypes.DateNTime:
			{
				CpuEventArgs e = new CpuEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuWriteTime, propDateTime);
				OnDateTimeWritten(e);
				if (errorCode != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuWriteTime, Service));
				}
				break;
			}
			case PVIWriteAccessTypes.SavePath:
				OnSavePathWritten(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuWriteSavePath, Service));
				if (errorCode != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuWriteSavePath, Service));
				}
				break;
			case PVIWriteAccessTypes.CpuModuleDelete:
				OnModuleDeleted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuModuleDelete, Service));
				if (errorCode != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuModuleDelete, Service));
				}
				break;
			case PVIWriteAccessTypes.WritePhysicalMemory:
				OnPhysicalMemoryWritten(errorCode);
				break;
			case PVIWriteAccessTypes.GlobalForceOFF:
				OnGlobalForcedOFF(errorCode);
				break;
			case PVIWriteAccessTypes.ANSL_RedundancyControl:
				if (1 == option)
				{
					OnActiveCpuChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuSwitchActiveCpu, Service));
				}
				else
				{
					OnApplicationSynchronizeStarted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuSynchronizeApplication, Service));
				}
				break;
			case PVIWriteAccessTypes.ClearMemory:
				OnMemoryCleared(errorCode);
				break;
			case PVIWriteAccessTypes.ANSL_COMMAND_Data:
				OnXMLCommand(errorCode, dataState, option, pData, dataLen);
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		private void CheckModuleChanged(APIFC_ModulInfoRes moduleInfoStruct, int errorCode)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfoStruct);
			CheckModuleChanged(moduleInfoDescription, errorCode);
		}

		internal void UpdateModuleAPIFCInfoList(APIFC_ModulInfoRes moduleInfoStruct)
		{
			if (propModuleInfoList != null)
			{
				if (propModuleInfoList.ContainsKey(moduleInfoStruct.name))
				{
					propModuleInfoList[moduleInfoStruct.name] = moduleInfoStruct;
				}
				else
				{
					propModuleInfoList.Add(moduleInfoStruct.name, moduleInfoStruct);
				}
			}
		}

		internal void UpdateModuleInfoList(ModuleInfoDescription moduleInfoDesc)
		{
			APIFC_ModulInfoRes apifcInfo = default(APIFC_ModulInfoRes);
			moduleInfoDesc.UpdateAPIFCModulInfoRes(ref apifcInfo);
			UpdateModuleAPIFCInfoList(apifcInfo);
		}

		private void CheckModuleChanged(ModuleInfoDescription moduleInfoDesc, int errorCode)
		{
			if (errorCode == 0)
			{
				UpdateModuleInfoList(moduleInfoDesc);
			}
			if (Module.isTaskObject(moduleInfoDesc.type) && Tasks.Values != null)
			{
				Task task = Tasks[moduleInfoDesc.name];
				if (task == null)
				{
					if (Tasks.Synchronize && Tasks.isSyncable)
					{
						task = new Task(this, moduleInfoDesc.name);
						Tasks.OnModuleCreated(new ModuleEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleCreatedEvent, task, 0));
						task.updateProperties(moduleInfoDesc);
						Tasks.OnModuleChanged(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleChangedEvent, task, 0));
						if (Modules.Values != null && Modules.Synchronize)
						{
							Modules.Add(task);
							Modules.OnModuleCreated(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleCreatedEvent, task, 0));
							Modules.OnModuleChanged(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleChangedEvent, task, 0));
						}
					}
					else if (Modules.Values != null && Modules.Synchronize)
					{
						Module module = Modules[moduleInfoDesc.name];
						if (module != null)
						{
							module.updateProperties(moduleInfoDesc);
							Modules.OnModuleChanged(new ModuleEventArgs(module.propName, module.Address, errorCode, Service.Language, Action.ModuleChangedEvent, module, 0));
							return;
						}
						task = new Task(this, moduleInfoDesc.name);
						Modules.OnModuleCreated(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleCreatedEvent, task, 0));
						task.updateProperties(moduleInfoDesc);
						Modules.OnModuleChanged(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleChangedEvent, task, 0));
					}
				}
				else
				{
					task.updateProperties(moduleInfoDesc);
					Tasks.OnModuleChanged(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleChangedEvent, task, 0));
					if (Modules.Values != null && Modules.Synchronize)
					{
						Modules.OnModuleChanged(new ModuleEventArgs(task.propName, task.Address, errorCode, Service.Language, Action.ModuleChangedEvent, task, 0));
					}
				}
			}
			else if (moduleInfoDesc.type == ModuleType.Logger && Loggers.Values != null)
			{
				Logger logger = Loggers[moduleInfoDesc.name];
				if (logger == null)
				{
					if (Loggers.Synchronize && Loggers.isSyncable)
					{
						logger = new Logger(this, moduleInfoDesc.name);
						Loggers.OnModuleCreated(new ModuleEventArgs(propName, propAddress, errorCode, Service.Language, Action.ModuleCreatedEvent, logger, 0));
						logger.updateProperties(moduleInfoDesc);
						Loggers.OnModuleChanged(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleChangedEvent, logger, 0));
						if (Modules.Values != null && Modules.Synchronize)
						{
							Modules.Add(logger);
							Modules.OnModuleCreated(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleCreatedEvent, logger, 0));
							Modules.OnModuleChanged(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleChangedEvent, logger, 0));
						}
					}
					else if (Modules.Values != null && Modules.Synchronize)
					{
						Module module = Modules[moduleInfoDesc.name];
						if (module != null)
						{
							module.updateProperties(moduleInfoDesc);
							Modules.OnModuleChanged(new ModuleEventArgs(module.propName, module.Address, errorCode, Service.Language, Action.ModuleChangedEvent, module, 0));
							return;
						}
						logger = new Logger(this, moduleInfoDesc.name);
						Modules.OnModuleCreated(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleCreatedEvent, logger, 0));
						logger.updateProperties(moduleInfoDesc);
						Modules.OnModuleChanged(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleChangedEvent, logger, 0));
					}
				}
				else
				{
					logger.updateProperties(moduleInfoDesc);
					Loggers.OnModuleChanged(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleChangedEvent, logger, 0));
					if (Modules.Values != null && Modules.Synchronize)
					{
						Modules.OnModuleChanged(new ModuleEventArgs(logger.propName, logger.Address, errorCode, Service.Language, Action.ModuleChangedEvent, logger, 0));
					}
				}
			}
			else
			{
				if (Modules.Values == null)
				{
					return;
				}
				Module module2 = null;
				module2 = Modules[moduleInfoDesc.name];
				if (module2 == null)
				{
					if (Modules.Synchronize && Modules.isSyncable)
					{
						module2 = new Module(this, moduleInfoDesc, Modules);
						Modules.OnModuleCreated(new ModuleEventArgs(module2.propName, module2.propAddress, errorCode, Service.Language, Action.ModuleCreatedEvent, module2, 0));
						module2.updateProperties(moduleInfoDesc);
						Modules.OnModuleChanged(new ModuleEventArgs(module2.propName, module2.propAddress, errorCode, Service.Language, Action.ModuleChangedEvent, module2, 0));
					}
				}
				else
				{
					module2.updateProperties(moduleInfoDesc);
					Modules.OnModuleChanged(new ModuleEventArgs(module2.propName, module2.propAddress, errorCode, Service.Language, Action.ModuleChangedEvent, module2, 0));
				}
			}
		}

		private void RaiseProceedingEvent(int nAccess, string moduleName, int percentComplete, int errorCode)
		{
			Module module = null;
			if (21 == nAccess || 27 == nAccess)
			{
				module = Modules[actDownLoadModuleName];
			}
			else if (21 == nAccess || 27 == nAccess)
			{
				module = Modules[actUpLoadModuleName];
			}
			else if (427 == nAccess || 427 == nAccess)
			{
				ObjectsOnTarget.OnObjectTransfering(percentComplete, errorCode);
			}
			module?.OnProceeding(percentComplete, errorCode);
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			ConnectionStates propConnectionState = base.propConnectionState;
			if (base.Name == null)
			{
				return;
			}
			switch (eventType)
			{
			case EventTypes.Connection:
				break;
			case EventTypes.Error:
			case EventTypes.Data:
				if (errorCode == 0 || 12002 == errorCode)
				{
					if (Connection.DeviceType < DeviceType.TcpIpMODBUS)
					{
						if (!propFireConChanged)
						{
							UploadCpuInfo((DeviceType.ANSLTcp == Connection.DeviceType) ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
						}
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuConnect, Service));
					}
				}
				else
				{
					if (Variables != null && Variables.Values != null)
					{
						foreach (Variable value in Variables.Values)
						{
							value.propErrorState = errorCode;
						}
					}
					if (Tasks != null && Tasks.Values != null)
					{
						foreach (Task value2 in Tasks.Values)
						{
							if (value2.Variables != null && value2.Variables.Values != null)
							{
								foreach (Variable value3 in value2.Variables.Values)
								{
									value3.propErrorState = errorCode;
								}
							}
						}
					}
					if (IsConnected)
					{
						OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
						if (ConnectionStates.Connected == propConnectionState)
						{
							base.propConnectionState = ConnectionStates.ConnectedError;
						}
					}
					else if (ConnectionStates.Connecting == base.propConnectionState)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuConnect, Service));
					}
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuConnect, Service));
					if (Connection.ConnectionChangeInProgress(base.propConnectionState) && errorCode != 13097 && errorCode != 11020)
					{
						base.propConnectionState = ConnectionStates.ConnectedError;
						OnConnectionChanged(errorCode, Action.CpuChangeConnection);
					}
				}
				if (ConnectionStates.Connected != base.propConnectionState)
				{
					propState = CpuState.Offline;
				}
				break;
			case EventTypes.Disconnect:
				OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
				break;
			case EventTypes.ModuleChanged:
			{
				APIFC_ModulInfoRes moduleInfoStruct = PviMarshal.PtrToModulInfoStructure(pData, typeof(APIFC_ModulInfoRes));
				CheckModuleChanged(moduleInfoStruct, errorCode);
				break;
			}
			case EventTypes.ModuleDeleted:
			{
				APIFC_ModulInfoRes aPIFC_ModulInfoRes = PviMarshal.PtrToModulInfoStructure(pData, typeof(APIFC_ModulInfoRes));
				CheckModuleDeleted(aPIFC_ModulInfoRes.name, errorCode, Action.ModuleDelete);
				break;
			}
			case EventTypes.Proceeding:
			{
				ProgressInfo progressInfo = PviMarshal.PtrToProgressInfoStructure(pData, typeof(ProgressInfo));
				string info = progressInfo.Info;
				RaiseProceedingEvent(progressInfo.Access, info, progressInfo.Percent, errorCode);
				break;
			}
			case EventTypes.ModuleListChangedXML:
				if (0 < dataLen)
				{
					int updateFlags = 0;
					CheckANSLModuleListChanges(errorCode, pData, dataLen, ref updateFlags);
				}
				break;
			case EventTypes.RedundancyCtrlEventXML:
				OnRedundancyCtrlEvent(errorCode, pData, dataLen);
				break;
			case EventTypes.BondChanged:
				OnBondChanged(errorCode, pData, dataLen);
				break;
			case EventTypes.LicenseChnaged:
				OnLicenseChanged(errorCode, pData, dataLen);
				break;
			default:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				break;
			}
		}

		private void ANSLModuleInfoListFromCB(int errorCode, IntPtr ptrData, uint dataLen)
		{
			int updateFlags = 0;
			InterpretModuleInfoData(errorCode, GetANSLModuleList(errorCode, ptrData, dataLen, ref updateFlags), updateFlags);
		}

		private void INAModuleInfoListFromCB(bool allModules, int errorCode, IntPtr ptrData, uint dataLen)
		{
			int updateFlags = 0;
			InterpretModuleInfoData(errorCode, GetINA2000ModuleList(allModules, errorCode, ptrData, dataLen, ref updateFlags), updateFlags);
		}

		private void InterpretModuleInfoData(int errorCode, Hashtable newModNames, int updateFlags)
		{
			base.Requests &= ~Actions.Upload;
			if (base.Requests == Actions.NONE)
			{
				propModuleInfoRequested = false;
				if (ConnectionStates.Connecting == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuConnect, Service));
				}
				else if (Connection.ConnectionChangeInProgress(propConnectionState))
				{
					OnConnectionChanged(propErrorCode, Action.CpuChangeConnection);
				}
			}
			Modules.CheckUploadedRequest();
			if (Tasks.Synchronize)
			{
				Tasks.DoSynchronize(newModNames);
			}
			if (TextModules.Synchronize)
			{
				TextModules.DoSynchronize(newModNames);
			}
			if (Modules.Synchronize || 2 == (updateFlags & 1))
			{
				Modules.DoSynchronize(newModNames);
			}
			propModuleInfoRequested = false;
			Tasks.CheckFireUploadEvents(errorCode, Action.TasksUpload, Action.TasksConnect);
			Modules.CheckFireUploadEvents(errorCode, Action.ModulesUpload, Action.ModulesConnect);
			TextModules.CheckFireUploadEvents(errorCode, Action.ModulesUpload, Action.ModulesUpload);
		}

		internal void ReadANSLMODLists(IntPtr ptrData, uint dataLen, ref Hashtable delMods, ref Hashtable newMods, ref Hashtable chgMods)
		{
			Hashtable hashtable = null;
			try
			{
				if (0 < dataLen && IntPtr.Zero != ptrData)
				{
					delMods = new Hashtable();
					newMods = new Hashtable();
					chgMods = new Hashtable();
					byte[] array = new byte[dataLen];
					Marshal.Copy(ptrData, array, 0, (int)dataLen);
					MemoryStream input = new MemoryStream(array);
					XmlTextReader xmlTextReader = new XmlTextReader(input);
					xmlTextReader.MoveToContent();
					if (xmlTextReader.Name.CompareTo("ModList") == 0)
					{
						hashtable = newMods;
						while (!xmlTextReader.EOF)
						{
							if (xmlTextReader.NodeType != XmlNodeType.EndElement && xmlTextReader.NodeType != XmlNodeType.Whitespace)
							{
								if (xmlTextReader.Name.CompareTo("Deleted") == 0)
								{
									hashtable = delMods;
								}
								else if (xmlTextReader.Name.CompareTo("New") == 0)
								{
									hashtable = newMods;
								}
								else if (xmlTextReader.Name.CompareTo("Changed") == 0)
								{
									hashtable = chgMods;
								}
								if (xmlTextReader.Name.CompareTo("ModInfo") == 0 || xmlTextReader.Name.CompareTo("TaskInfo") == 0)
								{
									ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
									moduleInfoDescription.ReadFromXML(xmlTextReader);
									if (moduleInfoDescription.name != null && !hashtable.ContainsKey(moduleInfoDescription.name))
									{
										hashtable.Add(moduleInfoDescription.name, moduleInfoDescription);
									}
								}
							}
							xmlTextReader.Read();
						}
					}
				}
			}
			catch
			{
			}
		}

		private void CheckANSLModuleListChanges(int errorCode, IntPtr ptrData, uint dataLen, ref int updateFlags)
		{
			Hashtable delMods = null;
			Hashtable newMods = null;
			Hashtable chgMods = null;
			int errorCode2 = 12055;
			if (dataLen == 0 || errorCode != 0)
			{
				if (errorCode != 0)
				{
					errorCode2 = errorCode;
				}
				OnError(new PviEventArgs(base.Name, base.Address, errorCode2, Service.Language, Action.ModuleChangedEvent, Service));
				return;
			}
			ReadANSLMODLists(ptrData, dataLen, ref delMods, ref newMods, ref chgMods);
			if (delMods != null)
			{
				foreach (ModuleInfoDescription value in delMods.Values)
				{
					CheckModuleDeleted(value.name, errorCode, Action.ModuleDelete);
				}
			}
			if (newMods != null)
			{
				foreach (ModuleInfoDescription value2 in newMods.Values)
				{
					CheckModuleChanged(value2, errorCode);
				}
			}
			if (chgMods != null)
			{
				foreach (ModuleInfoDescription value3 in chgMods.Values)
				{
					CheckModuleChanged(value3, errorCode);
				}
			}
		}

		internal Hashtable ReadANSLMODList(IntPtr ptrData, uint dataLen)
		{
			Hashtable hashtable = new Hashtable();
			try
			{
				if (0 < dataLen && IntPtr.Zero != ptrData)
				{
					byte[] array = new byte[dataLen];
					Marshal.Copy(ptrData, array, 0, (int)dataLen);
					MemoryStream input = new MemoryStream(array);
					XmlTextReader xmlTextReader = new XmlTextReader(input);
					xmlTextReader.MoveToContent();
					if (xmlTextReader.Name.CompareTo("ModList") == 0)
					{
						if (propModuleInfoList != null)
						{
							propModuleInfoList.Clear();
						}
						else
						{
							propModuleInfoList = new Hashtable();
						}
						while (!xmlTextReader.EOF)
						{
							if (xmlTextReader.NodeType != XmlNodeType.EndElement && xmlTextReader.NodeType != XmlNodeType.Whitespace && (xmlTextReader.Name.CompareTo("ModInfo") == 0 || xmlTextReader.Name.CompareTo("TaskInfo") == 0))
							{
								ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
								moduleInfoDescription.ReadFromXML(xmlTextReader);
								if (moduleInfoDescription.name != null)
								{
									if (!hashtable.ContainsKey(moduleInfoDescription.name))
									{
										hashtable.Add(moduleInfoDescription.name, moduleInfoDescription);
									}
									if (propModuleInfoList.ContainsKey(moduleInfoDescription.name))
									{
										propModuleInfoList[moduleInfoDescription.name] = moduleInfoDescription;
									}
									else
									{
										propModuleInfoList.Add(moduleInfoDescription.name, moduleInfoDescription);
									}
								}
							}
							xmlTextReader.Read();
						}
					}
				}
				return hashtable;
			}
			catch
			{
				return new Hashtable();
			}
		}

		private Hashtable GetANSLModuleList(int errorCode, IntPtr ptrData, uint dataLen, ref int updateFlags)
		{
			Hashtable hashtable = new Hashtable();
			int errorCode2 = 12055;
			if (propModuleInfoList == null)
			{
				propModuleInfoList = new Hashtable();
			}
			if (errorCode != 0)
			{
				return null;
			}
			if (dataLen == 0 || errorCode != 0)
			{
				if (errorCode != 0)
				{
					errorCode2 = errorCode;
				}
				OnError(new PviEventArgs(base.Name, base.Address, errorCode2, Service.Language, Action.ModuleInfo, Service));
				return hashtable;
			}
			Hashtable hashtable2 = ReadANSLMODList(ptrData, dataLen);
			foreach (ModuleInfoDescription value in hashtable2.Values)
			{
				hashtable.Add(value.name, value.name);
				UpdateModuleInfoStruct(value, errorCode, ref updateFlags);
			}
			return hashtable;
		}

		private Hashtable GetINA2000ModuleList(bool allModules, int errorCode, IntPtr ptrData, uint dataLen, ref int updateFlags)
		{
			if (propModuleInfoList == null)
			{
				propModuleInfoList = new Hashtable();
			}
			if (errorCode != 0)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			if (allModules)
			{
				ulong num = dataLen / 164u;
				for (ulong num2 = 0uL; num2 < num; num2++)
				{
					ulong intPtrAdr = PviMarshal.GetIntPtrAdr(ptrData, num2 * 164);
					APIFC_ModulInfoRes aPIFC_ModulInfoRes = PviMarshal.PtrToModulInfoStructure((IntPtr)(long)intPtrAdr, typeof(APIFC_ModulInfoRes));
					if (aPIFC_ModulInfoRes.name != null)
					{
						hashtable.Add(aPIFC_ModulInfoRes.name, aPIFC_ModulInfoRes.name);
						if (propModuleInfoList.ContainsKey(aPIFC_ModulInfoRes.name))
						{
							propModuleInfoList[aPIFC_ModulInfoRes.name] = aPIFC_ModulInfoRes;
						}
						else
						{
							propModuleInfoList.Add(aPIFC_ModulInfoRes.name, aPIFC_ModulInfoRes);
						}
						UpdateModuleInfoStruct(aPIFC_ModulInfoRes, errorCode, ref updateFlags);
					}
				}
			}
			else
			{
				ulong num3 = dataLen / 57u;
				for (ulong num4 = 0uL; num4 < num3; num4++)
				{
					ulong intPtrAdr2 = PviMarshal.GetIntPtrAdr(ptrData, num4 * 57);
					APIFC_DiagModulInfoRes aPIFC_DiagModulInfoRes = PviMarshal.PtrToDiagModulInfoStructure((IntPtr)(long)intPtrAdr2, typeof(APIFC_DiagModulInfoRes));
					if (aPIFC_DiagModulInfoRes.name == null)
					{
						continue;
					}
					if (!hashtable.ContainsKey(aPIFC_DiagModulInfoRes.name))
					{
						hashtable.Add(aPIFC_DiagModulInfoRes.name, aPIFC_DiagModulInfoRes.name);
						if (propModuleInfoList.ContainsKey(aPIFC_DiagModulInfoRes.name))
						{
							propModuleInfoList[aPIFC_DiagModulInfoRes.name] = aPIFC_DiagModulInfoRes;
						}
						else
						{
							propModuleInfoList.Add(aPIFC_DiagModulInfoRes.name, aPIFC_DiagModulInfoRes);
						}
					}
					UpdateModuleDiagInfoStruct(aPIFC_DiagModulInfoRes, errorCode, ref updateFlags);
				}
			}
			return hashtable;
		}

		private void UpdateModuleInfoStruct(APIFC_ModulInfoRes moduleInfoStruct, int errorCode, ref int updateFlags)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfoStruct);
			UpdateModuleInfoStruct(moduleInfoDescription, errorCode, ref updateFlags);
		}

		private void UpdateModuleInfoStruct(ModuleInfoDescription moduleInfoStruct, int errorCode, ref int updateFlags)
		{
			Tasks.UpdateModuleInfo(moduleInfoStruct, errorCode, ref updateFlags, (BootMode == BootMode.Diagnostics) ? true : false);
			if (propUserTaskCollections != null)
			{
				foreach (TaskCollection value in propUserTaskCollections.Values)
				{
					if (value.ContainsKey(moduleInfoStruct.name))
					{
						Module module = value[moduleInfoStruct.name];
						module.updateProperties(moduleInfoStruct, (BootMode == BootMode.Diagnostics) ? true : false);
						if (module.CheckModuleInfo(errorCode))
						{
							module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
						}
					}
				}
			}
			TextModules.UpdateTextModuleInfo(moduleInfoStruct, errorCode, ref updateFlags, (BootMode == BootMode.Diagnostics) ? true : false);
			ConnectionStates propConnectionState = base.propConnectionState;
			if (errorCode == 0)
			{
				base.propConnectionState = ConnectionStates.Connected;
			}
			Modules.UpdateModuleInfo(moduleInfoStruct, 0, ref updateFlags, (BootMode == BootMode.Diagnostics) ? true : false);
			base.propConnectionState = propConnectionState;
			bool flag = (BootMode == BootMode.Diagnostics) ? true : false;
			bool flag2 = Modules._UploadOption == ModuleListOptions.INA2000CompatibleMode || Modules._UploadOption == ModuleListOptions.INA2000DiagnosisList;
			if (propUserModuleCollections != null)
			{
				foreach (ModuleCollection value2 in propUserModuleCollections.Values)
				{
					if (value2.ContainsKey(moduleInfoStruct.name))
					{
						Module module2 = value2[moduleInfoStruct.name];
						module2.updateProperties(moduleInfoStruct, flag);
						if (flag && flag2)
						{
							module2.propType = ModuleType.Unknown;
						}
						if (module2.CheckModuleInfo(errorCode))
						{
							module2.Fire_OnConnected(new PviEventArgs(module2.Name, module2.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
						}
					}
				}
			}
		}

		private void UpdateModuleDiagInfoStruct(APIFC_DiagModulInfoRes diagModuleInfoStruct, int errorCode, ref int updateFlags)
		{
			Tasks.DiagnosticModeUpdateModuleInfo(diagModuleInfoStruct, errorCode, ref updateFlags);
			if (propUserTaskCollections != null)
			{
				foreach (TaskCollection value in propUserTaskCollections.Values)
				{
					if (value.ContainsKey(diagModuleInfoStruct.name))
					{
						Module module = value[diagModuleInfoStruct.name];
						module.updateProperties(diagModuleInfoStruct);
						if (module.CheckModuleInfo(errorCode))
						{
							module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
						}
					}
				}
			}
			Modules.DiagnosticModeUpdateModuleInfo(diagModuleInfoStruct, errorCode, ref updateFlags);
			if (propUserModuleCollections != null)
			{
				foreach (ModuleCollection value2 in propUserModuleCollections.Values)
				{
					if (value2.ContainsKey(diagModuleInfoStruct.name))
					{
						Module module2 = value2[diagModuleInfoStruct.name];
						module2.updateProperties(diagModuleInfoStruct);
						if (module2.CheckModuleInfo(errorCode))
						{
							module2.Fire_OnConnected(new PviEventArgs(module2.Name, module2.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
						}
					}
				}
			}
		}

		internal void UpdateLoggerMode(bool isSG4)
		{
			propIsSG4Target = isSG4;
			if (!isSG4)
			{
				if (!Loggers.ContainsKey("$LOG285$"))
				{
					new ErrorLogBook(this);
				}
			}
			else if (Loggers.ContainsKey("$LOG285$"))
			{
				Loggers.RemoveFromCollection(Loggers["$LOG285$"], Action.LoggerDelete);
			}
		}

		private int DetectSGxLogger(string moduleName)
		{
			int num = 0;
			if (propARLogSys == null)
			{
				propARLogSys = new Logger(this, moduleName, doNotAddToCollections: true);
				propARLogSys.Address = "$arlogsys";
				propARLogSys.Error += ARLogSys_Error;
				propHasARLogSysErrorEVH = true;
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					propARLogSys.LinkName = base.LinkName + "/$arlogsys OT=Module";
				}
				else if (propLinkName != null && 0 < propLinkName.Length)
				{
					propARLogSys.LinkName = base.LinkName + "/$arlogsys OT=Module";
				}
			}
			else if (!propHasARLogSysErrorEVH)
			{
				propARLogSys.Error += ARLogSys_Error;
				propHasARLogSysErrorEVH = true;
			}
			bool isStatic = Service.IsStatic;
			Service.IsStatic = false;
			num = propARLogSys.ConnectEx();
			Service.IsStatic = isStatic;
			if (num != 0)
			{
				base.Requests &= ~Actions.GetLBType;
				OnError(new PviEventArgs(moduleName, moduleName, num, Service.Language, Action.ModuleConnect, Service));
				if (Connection.ConnectionChangeInProgress(propConnectionState))
				{
					OnConnectionChanged(propErrorCode, Action.CpuChangeConnection);
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuConnect, Service));
				}
			}
			return num;
		}

		private void ARLogSys_Error(object sender, PviEventArgs e)
		{
			int errorCode = e.ErrorCode;
			if (Service.IsRemoteError(e.ErrorCode))
			{
				return;
			}
			propIsSG4Target = true;
			if (4813 == e.ErrorCode || 4820 == e.ErrorCode)
			{
				errorCode = 0;
				propIsSG4Target = false;
				if (Loggers.Values != null)
				{
					ArrayList arrayList = new ArrayList(Loggers.Values.Count);
					int num = 0;
					foreach (Logger value in Loggers.Values)
					{
						value.CleanLoggerEntries(0);
						value.Disconnect(noResponse: true);
						arrayList.Add(value);
						num++;
					}
					for (num = 0; num < arrayList.Count; num++)
					{
						Loggers.RemoveFromCollection((Base)arrayList[num], Action.LoggerDelete);
					}
					arrayList.Clear();
					arrayList = null;
				}
				UpdateLoggerMode(isSG4: false);
			}
			if (e.ErrorCode == 0 && Action.LoggerGetStatus != e.Action && propARLogSys != null)
			{
				propARLogSys.ReadRequest(Service.hPvi, ((Logger)sender).LinkId, AccessTypes.Status, 917u);
				return;
			}
			propHasARLogSysErrorEVH = false;
			if (propARLogSys != null)
			{
				propARLogSys.Error -= ARLogSys_Error;
				propHasARLogSysErrorEVH = false;
			}
			if (e.ErrorCode == 0 && Loggers.ContainsKey("$LOG285$"))
			{
				Logger logger2 = Loggers["$LOG285$"];
				logger2.CleanLoggerEntries(0);
				logger2.Disconnect(noResponse: true);
				Loggers.RemoveFromCollection(logger2, Action.LoggerDelete);
			}
			base.Requests &= ~Actions.GetLBType;
			if (base.Requests == Actions.NONE)
			{
				OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuConnect, Service));
			}
			if (Actions.GetCpuInfo == (base.Requests & Actions.GetCpuInfo))
			{
				UploadCpuInfo((DeviceType.ANSLTcp == Connection.DeviceType) ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
			}
			else if (Actions.Upload == (base.Requests & Actions.Upload) && !propModuleInfoRequested)
			{
				RequestModuleList(ModuleListOptions.INA2000CompatibleMode);
			}
			if (Actions.Connect == (base.Requests & Actions.Connect))
			{
				base.Requests &= ~Actions.Connect;
				if (base.Requests == Actions.NONE)
				{
					if (Connection.ConnectionChangeInProgress(propConnectionState))
					{
						OnConnectionChanged(errorCode, Action.CpuChangeConnection);
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.CpuConnect, Service));
					}
				}
			}
			else if (Connection.ConnectionChangeInProgress(propConnectionState))
			{
				OnConnectionChanged(errorCode, Action.CpuChangeConnection);
			}
		}

		private LoggerEntryCollection GetLogBookEntries(IntPtr pData, uint dataLen)
		{
			int num = Marshal.SizeOf(typeof(APIFC_RLogbookRes_entry));
			int num2 = (int)((long)dataLen / (long)num);
			APIFC_RLogbookRes_entry[] array = new APIFC_RLogbookRes_entry[num2];
			LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("EventEntries");
			for (uint num3 = 0u; num3 < num2; num3++)
			{
				ulong intPtrAdr = PviMarshal.GetIntPtrAdr(pData, (uint)((int)num3 * num));
				array[num3] = (APIFC_RLogbookRes_entry)Marshal.PtrToStructure((IntPtr)(long)intPtrAdr, typeof(APIFC_RLogbookRes_entry));
			}
			InsertSysLogBookEntries(array, num2, loggerEntryCollection);
			return loggerEntryCollection;
		}

		private void InsertSysLogBookEntries(APIFC_RLogbookRes_entry[] lbEntries, int itemCnt, LoggerEntryCollection eventEntries)
		{
			int num = 0;
			LoggerEntry loggerEntry = null;
			for (num = itemCnt - 1; num > -1; num--)
			{
				LogBookEntry logBookEntry = new LogBookEntry(lbEntries[num]);
				if (logBookEntry.propErrorNumber != 0 || logBookEntry.propTask.Length != 0 || logBookEntry.propErrorInfo != 0 || logBookEntry.propASCIIData.Length != 0)
				{
					if (loggerEntry != null && LevelType.Info == logBookEntry.propLevelType && loggerEntry.propErrorNumber == logBookEntry.propErrorNumber)
					{
						loggerEntry.AppendSGxErrorInfo(logBookEntry, IsSG4Target);
					}
					else if (LevelType.Info != logBookEntry.propLevelType)
					{
						loggerEntry = new LoggerEntry(this, RuntimeVersion, logBookEntry, itemCnt - num, addKeyOnly: true, reverseOrder: false);
						loggerEntry.UpdateForSGx(logBookEntry, IsSG4Target);
						eventEntries.Add(loggerEntry, addKeyOnly: true);
					}
					else
					{
						loggerEntry = null;
					}
				}
			}
		}

		private void CheckModuleDeleted(string name, int error, Action action)
		{
			ArrayList arrayList = new ArrayList(1);
			if (Modules.Values != null)
			{
				Module module = Modules[name];
				if (module != null)
				{
					if (Modules.Synchronize)
					{
						arrayList.Add(module.Name);
					}
					else if (Service != null)
					{
						Modules.OnModuleDeleted(new ModuleEventArgs(name, propAddress, error, Service.Language, Action.ModuleDeletedEvent, module, 0));
					}
				}
			}
			foreach (string item in arrayList)
			{
				if (Tasks.ContainsKey(item))
				{
					Task task = Tasks[item];
					if (Service != null)
					{
						task.Fire_Deleted(new PviEventArgs(task.Name, task.Address, error, Service.Language, action, Service));
					}
					Tasks.Remove(item);
				}
				if (Modules.ContainsKey(item))
				{
					Module module2 = Modules[item];
					if (Service != null)
					{
						module2.Fire_Deleted(new PviEventArgs(module2.Name, module2.Address, error, Service.Language, action, Service));
					}
					if (module2.LinkId != 0)
					{
						module2.Disconnect(noResponse: true);
					}
					Modules.Remove(item);
				}
				if (Loggers.ContainsKey(item))
				{
					Logger logger = Loggers[item];
					if (Service != null)
					{
						logger.Fire_Deleted(new PviEventArgs(logger.Name, logger.Address, error, Service.Language, action, Service));
					}
					Loggers.Remove(item);
				}
			}
			ArrayList arrayList2 = new ArrayList(1);
			if (Tasks.Values != null)
			{
				foreach (Task value in Tasks.Values)
				{
					if (!value.propDisposed && value.Address.CompareTo(name) == 0 && Modules.Synchronize)
					{
						arrayList2.Add(value.Name);
					}
				}
			}
			foreach (string item2 in arrayList2)
			{
				Tasks.Remove(item2);
			}
		}

		public virtual void ReadDateTime()
		{
			int num = ReadRequest(Service.hPvi, base.LinkId, AccessTypes.DateTime, 212u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuReadTime, Service));
			}
		}

		public virtual int DeleteModule(string name)
		{
			int num = 0;
			string text = "MN=" + name;
			Service.BuildRequestBuffer(text);
			num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.CpuModuleDelete, Service.RequestBuffer, text.Length, 219u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuModuleDelete, Service));
			}
			return num;
		}

		public virtual void WriteDateTime(DateTime datetime)
		{
			propDateTime = datetime;
			APIFC_CpuDateTime aPIFC_CpuDateTime = default(APIFC_CpuDateTime);
			aPIFC_CpuDateTime.year = datetime.Year - 1900;
			aPIFC_CpuDateTime.mon = datetime.Month - 1;
			aPIFC_CpuDateTime.mday = datetime.Day;
			aPIFC_CpuDateTime.hour = datetime.Hour;
			aPIFC_CpuDateTime.min = datetime.Minute;
			aPIFC_CpuDateTime.sec = datetime.Second;
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(APIFC_CpuDateTime)));
			Marshal.StructureToPtr((object)aPIFC_CpuDateTime, hMemory, fDeleteOld: false);
			int num = 0;
			num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.DateTime, hMemory, Marshal.SizeOf(typeof(APIFC_CpuDateTime)), 213u);
			PviMarshal.FreeHGlobal(ref hMemory);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuReadTime, Service));
			}
		}

		public virtual void WriteSavePath(string savePath)
		{
			if (savePath.Length != 0)
			{
				int num = 0;
				IntPtr hMemory = PviMarshal.StringToHGlobal(savePath);
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.SavePath, hMemory, savePath.Length, 215u);
				PviMarshal.FreeHGlobal(ref hMemory);
				if (num != 0)
				{
					OnError(new PviEventArgs(base.Name, propAddress, num, Service.Language, Action.CpuWriteSavePath, Service));
				}
			}
		}

		public virtual void ReadSavePath()
		{
			int num = ReadRequest(Service.hPvi, base.LinkId, AccessTypes.SavePath, 216u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuReadSavePath, Service));
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (propDisposed)
			{
				return;
			}
			if (Connection != null)
			{
				DisconnectWithChilds();
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
				propService = ((propParent is Service) ? ((Service)propParent) : propParent.Service);
				base.propLinkName = propLinkName;
				base.propLogicalName = propLogicalName;
				base.propUserData = propUserData;
				base.propName = propName;
				base.propAddress = propAddress;
				propAWSType = null;
				propCPUName = null;
				propCPUType = null;
				if (removeFromCollection && Service != null)
				{
					Service.Cpus.Remove(base.Name);
				}
				if (propIODataPoints != null)
				{
					propIODataPoints.Dispose(disposing, removeFromCollection);
					propIODataPoints = null;
				}
				if (propLibraries != null)
				{
					propLibraries.Dispose(disposing, removeFromCollection);
					propLibraries = null;
				}
				if (propMemories != null)
				{
					propMemories.Dispose(disposing, removeFromCollection);
					propMemories = null;
				}
				if (propVariables != null)
				{
					propVariables.Dispose(disposing, removeFromCollection);
					propVariables = null;
				}
				if (propLoggers != null)
				{
					propLoggers.Dispose(disposing, removeFromCollection);
					propLoggers = null;
				}
				if (propTasks != null)
				{
					propTasks.Dispose(disposing, removeFromCollection);
					propTasks = null;
				}
				if (propTextModules != null)
				{
					propTextModules.Dispose(disposing, removeFromCollection);
					propTextModules = null;
				}
				if (propModules != null)
				{
					propModules.Dispose(disposing, removeFromCollection);
					propModules = null;
				}
				if (propProfiler != null)
				{
					propProfiler.Dispose(disposing);
					propProfiler = null;
				}
				if (propTaskClasses != null)
				{
					propTaskClasses.Dispose(disposing, removeFromCollection);
					propTaskClasses = null;
				}
				if (propUserCollections != null)
				{
					propUserCollections.Clear();
					propUserCollections = null;
				}
				if (propConnection != null)
				{
					propConnection.Connected -= ConnectionEvent;
					propConnection.ConnectionChanged -= connection_ConnectionChanged;
					propConnection.Disconnected -= ConnectionDisconnected;
					propConnection.Error -= ConnectionEvent;
					propConnection.Dispose(disposing, removeFromCollection);
					propConnection = null;
				}
				if (propModuleInfoList != null)
				{
					propModuleInfoList.Clear();
					propModuleInfoList = null;
				}
				if (propObjectBrowser != null)
				{
					propObjectBrowser = null;
				}
				if (propARLogSys != null)
				{
					propARLogSys.Dispose(disposing, removeFromCollection: true);
					propARLogSys = null;
				}
				if (propHardwareInfos != null)
				{
					propHardwareInfos.Dispose(disposing);
					propHardwareInfos = null;
				}
				base.propUserData = null;
				propSavePath = null;
				propSWVersion = null;
				base.propLinkName = null;
				base.propLogicalName = null;
				base.propUserData = null;
				base.propName = null;
				base.propAddress = null;
				base.propParent = null;
				propTCPDestinationSettings = null;
				propUserModuleCollections = null;
				propUserTaskCollections = null;
				propUserLoggerCollections = null;
			}
		}

		public override void Remove()
		{
			base.Remove();
			Service.Cpus.Remove(base.Name);
			if (propUserCollections != null && propUserCollections.Values != null)
			{
				foreach (CpuCollection value in propUserCollections.Values)
				{
					value.Remove(base.Name);
				}
			}
		}

		public virtual void Restart(BootMode bootMode)
		{
			if (!IsConnected)
			{
				OnError(new PviEventArgs(propName, propAddress, 4808, Service.Language, Action.CpuRestart, Service));
				return;
			}
			string text = "";
			switch (bootMode)
			{
			case BootMode.WarmRestart:
				text = "ST=WarmStart";
				break;
			case BootMode.ColdRestart:
				text = "ST=ColdStart";
				break;
			case BootMode.Reset:
				text = "ST=Reset";
				break;
			case BootMode.Diagnostics:
				text = "ST=Diagnose";
				break;
			default:
				OnError(new PviEventArgs(base.Name, base.Address, 12034, Service.Language, Action.CpuRestart, Service));
				return;
			}
			int num = 0;
			Service.BuildRequestBuffer(text);
			num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Status, Service.RequestBuffer, text.Length, 207u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuRestart, Service));
			}
			else
			{
				propRestarted = true;
			}
		}

		public int UpdateCpuInfo()
		{
			return UploadCpuInfo((DeviceType.ANSLTcp == Connection.DeviceType) ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info, doFireError: false);
		}

		internal int UploadCpuInfo(AccessTypes accType)
		{
			return UploadCpuInfo(accType, doFireError: true);
		}

		internal int UploadCpuInfo(AccessTypes accType, bool doFireError)
		{
			int num = 0;
			num = ReadArgumentRequest(Service.hPvi, propLinkId, accType, IntPtr.Zero, 0, 702u);
			if (num != 0 && doFireError)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.NONE, Service));
			}
			return num;
		}

		private int ReadErrorLogbook()
		{
			int num = 0;
			int dataLen = 4;
			int[] source = new int[1]
			{
				0
			};
			IntPtr intPtr = PviMarshal.AllocCoTaskMem(4);
			Marshal.Copy(source, 0, intPtr, 1);
			if (!IsConnected)
			{
				return 4808;
			}
			return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ReadError, intPtr, dataLen, 269u);
		}

		internal void Fire_OnError(PviEventArgs e)
		{
			OnError(e);
		}

		protected internal override void OnError(PviEventArgs e)
		{
			propErrorCode = e.ErrorCode;
			if (!ignoreEvents)
			{
				base.OnError(e);
				if (propUserCollections != null && propUserCollections.Values != null)
				{
					foreach (CpuCollection value in propUserCollections.Values)
					{
						value.OnError(this, e);
					}
				}
				Service.Cpus.OnError(this, e);
			}
		}

		protected virtual void OnCpuInfoUpdated(PviEventArgs e)
		{
			if (this.CpuInfoUpdated != null)
			{
				this.CpuInfoUpdated(this, e);
			}
			if (propUserCollections != null && propUserCollections.Values != null)
			{
				foreach (CpuCollection value in propUserCollections.Values)
				{
					foreach (Cpu value2 in value.Values)
					{
						value2.OnCpuInfoUpdated(e);
					}
				}
			}
		}

		protected override void OnConnectionChanged(int errorCode, Action action)
		{
			SysLoggerDisconnect();
			propErrorCode = errorCode;
			ignoreEvents = false;
			CpuEventsON();
			if (propFireConChanged)
			{
				propFireConChanged = false;
				base.OnConnectionChanged(errorCode, action);
			}
			else if (errorCode != 0)
			{
				propConnectionState = ConnectionStates.ConnectedError;
			}
			else
			{
				propConnectionState = ConnectionStates.Connected;
			}
		}

		protected override void OnConnected(PviEventArgs e)
		{
			bool flag = false;
			propErrorCode = e.ErrorCode;
			SysLoggerDisconnect();
			if (ignoreEvents)
			{
				return;
			}
			flag = propFireConChanged;
			if (propRestarted)
			{
				propConnection.propConnectionState = ConnectionStates.Connected;
			}
			else
			{
				propConnection.propConnectionState = propConnectionState;
			}
			if (propModules.Synchronize)
			{
				int count = Modules.Count;
			}
			if (e.propErrorCode == 0)
			{
				if (Actions.Upload == (propModules.Requests & Actions.Upload))
				{
					propModules.Upload();
				}
				if (Actions.Upload == (propTextModules.Requests & Actions.Upload))
				{
					propTextModules.Upload();
				}
				if (Actions.Upload == (propLoggers.Requests & Actions.Upload))
				{
					propLoggers.Upload();
				}
				else if (!propReCreateActive)
				{
					foreach (Logger value in propLoggers.Values)
					{
						if (Actions.Connected == (value.Requests & Actions.Connected))
						{
							value.Requests &= ~Actions.Connected;
							value.ReadIndex(Action.LoggerIndexForConnect);
						}
						else if (Actions.Connect == (value.Requests & Actions.Connect))
						{
							value.Requests &= ~Actions.Connect;
							value.propConnectionState = ConnectionStates.Unininitialized;
							value.Connect();
						}
					}
				}
				if ((propTasks.Requests & Actions.Upload) != 0)
				{
					propTasks.Upload();
				}
				else if (!propReCreateActive)
				{
					foreach (Task value2 in propTasks.Values)
					{
						if (Actions.Connected == (value2.Requests & Actions.Connected))
						{
							value2.Requests &= ~Actions.Connected;
							value2.Fire_OnConnect();
						}
						else if (Actions.Connect == (value2.Requests & Actions.Connect))
						{
							value2.Requests &= ~Actions.Connect;
							value2.propConnectionState = ConnectionStates.Unininitialized;
							value2.Connect(forceConnection: true);
						}
					}
				}
				if (Actions.Upload == (propTaskClasses.Requests & Actions.Upload))
				{
					propTaskClasses.Upload();
					propTaskClasses.Requests &= ~Actions.Upload;
				}
				if (Actions.Upload == (propVariables.Requests & Actions.Upload))
				{
					propVariables.Upload();
					propVariables.Requests &= ~Actions.Upload;
				}
				if (Actions.Upload == (propMemories.Requests & Actions.Upload))
				{
					propMemories.Upload();
					propMemories.propRequests &= ~Actions.Upload;
				}
				if (Actions.Upload == (propLibraries.Requests & Actions.Upload))
				{
					propLibraries.Upload();
					propLibraries.Requests &= ~Actions.Upload;
				}
			}
			if (!propReCreateActive)
			{
				if (propVariables != null && 0 < propVariables.Count)
				{
					foreach (Variable value3 in Variables.Values)
					{
						if ((value3.Requests & Actions.Connect) != 0)
						{
							value3.Connect(forceConnect: true);
							value3.Requests &= ~Actions.Connect;
						}
						else if ((value3.Requests & Actions.SetActive) != 0)
						{
							value3.Active = true;
							value3.Requests &= ~Actions.SetActive;
						}
						else if (!IsSG4Target && value3.IsConnected)
						{
							value3.Read_State(value3.LinkId, 2812u);
						}
					}
				}
				if (propModules != null && 0 < propModules.Count)
				{
					foreach (Module value4 in Modules.Values)
					{
						if ((value4.Requests & Actions.Connected) != 0)
						{
							value4.Fire_OnConnect();
							value4.Requests &= ~Actions.Connected;
						}
						else if (Actions.Connect == (value4.Requests & Actions.Connect))
						{
							value4.Requests &= ~Actions.Connect;
							value4.propConnectionState = ConnectionStates.Unininitialized;
							value4.Connect(forceConnection: true, propConnectionType, 301u);
						}
					}
				}
			}
			if (!IsSG4Target && Tasks != null && 0 < Tasks.Count)
			{
				foreach (Task value5 in Tasks.Values)
				{
					if (value5.propVariables != null && 0 < value5.propVariables.Count)
					{
						foreach (Variable value6 in value5.Variables.Values)
						{
							if (value6.IsConnected)
							{
								value6.Read_State(value6.LinkId, 2812u);
							}
						}
					}
				}
			}
			if (!propReCreateActive)
			{
				if (IODataPoints != null && 0 < IODataPoints.Count)
				{
					foreach (IODataPoint value7 in IODataPoints.Values)
					{
						if ((value7.Requests & Actions.Connect) != 0)
						{
							value7.Requests &= ~Actions.Connect;
							value7.Connect();
						}
					}
				}
				if (propLibraries != null && 0 < propLibraries.Count)
				{
					foreach (Library value8 in Libraries.Values)
					{
						if ((value8.Requests & Actions.Connect) != 0)
						{
							value8.Connect();
							value8.Requests &= ~Actions.Connect;
						}
					}
				}
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (CpuCollection value9 in propUserCollections.Values)
				{
					value9.OnConnected(this, e);
				}
			}
			if (Service != null && Service.Cpus != null)
			{
				Service.Cpus.Fire_Connected(this, e);
			}
			if (propReCreateActive)
			{
				propReCreateActive = false;
				if (Service.WaitForParentConnection)
				{
					reCreateChildState();
				}
			}
			if (propRestarted)
			{
				OnRestarted(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.CpuRestart, Service));
				base.OnConnected(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.CpuRestart, Service));
				propRestarted = false;
			}
			else
			{
				base.OnConnected(e);
			}
			if (flag)
			{
				OnConnectionChanged(e.ErrorCode, e.Action);
				propFireConChanged = false;
			}
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			SysLoggerDisconnect();
			if (ignoreEvents)
			{
				return;
			}
			if (propNoDisconnectedEvent)
			{
				propConnectionState = ConnectionStates.Disconnected;
				if (Modules != null)
				{
					Modules.propConnectionState = ConnectionStates.Disconnected;
				}
				if (Tasks != null)
				{
					Tasks.propConnectionState = ConnectionStates.Disconnected;
				}
				if (Variables != null)
				{
					Variables.propConnectionState = ConnectionStates.Disconnected;
				}
				if (IODataPoints != null)
				{
					IODataPoints.propConnectionState = ConnectionStates.Disconnected;
				}
				propNoDisconnectedEvent = false;
				return;
			}
			base.Requests |= Actions.GetCpuInfo;
			if (base.ConnectionType != ConnectionType.Link)
			{
				base.Requests |= Actions.GetLBType;
			}
			propModuleInfoRequested = false;
			propState = CpuState.Offline;
			if (propModuleInfoList != null)
			{
				propModuleInfoList.Clear();
			}
			base.OnDisconnected(e);
			if (propConnection != null)
			{
				propConnection.propConnectionState = propConnectionState;
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (CpuCollection value in propUserCollections.Values)
				{
					value.OnDisconnected(this, e);
				}
			}
			if (Service != null && Service.Cpus != null)
			{
				Service.Cpus.Fire_Disconnected(this, e);
			}
			if (Modules != null)
			{
				Modules.propConnectionState = propConnectionState;
			}
			if (Tasks != null)
			{
				Tasks.propConnectionState = propConnectionState;
			}
			if (Variables != null)
			{
				Variables.propConnectionState = propConnectionState;
			}
			if (IODataPoints != null)
			{
				IODataPoints.propConnectionState = propConnectionState;
			}
		}

		protected virtual void OnRestarted(PviEventArgs e)
		{
			if (this.Restarted != null)
			{
				this.Restarted(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (CpuCollection value in propUserCollections.Values)
				{
					value.OnRestarted(this, e);
				}
			}
			Service.Cpus.OnRestarted(this, e);
		}

		protected virtual void OnDateTimeRead(CpuEventArgs e)
		{
			if (this.DateTimeRead != null)
			{
				this.DateTimeRead(this, e);
			}
		}

		protected virtual void OnDateTimeWritten(CpuEventArgs e)
		{
			if (this.DateTimeWritten != null)
			{
				this.DateTimeWritten(this, e);
			}
		}

		protected virtual void OnSavePathWritten(PviEventArgs e)
		{
			if (this.SavePathWritten != null)
			{
				this.SavePathWritten(this, e);
			}
		}

		protected virtual void OnModuleDeleted(PviEventArgs e)
		{
			if (this.ModuleDeleted != null)
			{
				this.ModuleDeleted(this, e);
			}
		}

		protected virtual void OnSavePathRead(PviEventArgs e)
		{
			if (this.SavePathRead != null)
			{
				this.SavePathRead(this, e);
			}
		}

		internal APIFC_CpuInfoRes PtrToCpuInfoStruct(IntPtr pData)
		{
			APIFC_CpuInfoRes result = default(APIFC_CpuInfoRes);
			byte b = 0;
			int num = 0;
			while ((b = Marshal.ReadByte(pData, num++)) != 0)
			{
				result.sw_version += Convert.ToChar(b);
			}
			num = 36;
			while ((b = Marshal.ReadByte(pData, num++)) != 0)
			{
				result.cpu_name += Convert.ToChar(b);
			}
			num = 72;
			while ((b = Marshal.ReadByte(pData, num++)) != 0)
			{
				result.cpu_typ += Convert.ToChar(b);
			}
			num = 108;
			while ((b = Marshal.ReadByte(pData, num++)) != 0)
			{
				result.aws_typ += Convert.ToChar(b);
			}
			result.node_nr = (ushort)Marshal.ReadInt16(pData, 144);
			result.init_descr = Marshal.ReadInt32(pData, 146);
			result.state = Marshal.ReadInt32(pData, 150);
			result.voltage = Marshal.ReadByte(pData, 154);
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			Cpu cpu = (Cpu)baseObj;
			if (cpu == null)
			{
				return -1;
			}
			base.FromXmlTextReader(ref reader, flags, cpu);
			string text = "";
			text = reader.GetAttribute("ApplicationMemory");
			if (text != null && text.Length > 0)
			{
				cpu.propAWSType = text;
			}
			text = "";
			text = reader.GetAttribute("RuntimeVersion");
			if (text != null && text.Length > 0)
			{
				cpu.propSWVersion = text;
			}
			text = "";
			text = reader.GetAttribute("SavePath");
			if (text != null && text.Length > 0)
			{
				cpu.SavePath = text;
			}
			text = "";
			text = reader.GetAttribute("NodeNumber");
			if (text != null && text.Length > 0)
			{
				ushort result = 0;
				if (PviParse.TryParseUInt16(text, out result))
				{
					cpu.propNodeNumber = result;
				}
			}
			text = "";
			text = reader.GetAttribute("State");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "offline":
					cpu.propState = CpuState.Offline;
					break;
				case "run":
					cpu.propState = CpuState.Run;
					break;
				case "service":
					cpu.propState = CpuState.Service;
					break;
				case "stop":
					cpu.propState = CpuState.Stop;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("BootMode");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "boot":
					cpu.propInitDescription = BootMode.Boot;
					break;
				case "coldrestart":
					cpu.propInitDescription = BootMode.ColdRestart;
					break;
				case "diagnostics":
					cpu.propInitDescription = BootMode.Diagnostics;
					break;
				case "error":
					cpu.propInitDescription = BootMode.Error;
					break;
				case "reconfig":
					cpu.propInitDescription = BootMode.Reconfig;
					break;
				case "reset":
					cpu.propInitDescription = BootMode.Reset;
					break;
				case "warmrestart":
					cpu.propInitDescription = BootMode.WarmRestart;
					break;
				case "nmi":
					cpu.propInitDescription = BootMode.NMI;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("IsSG4Target");
			if (text != null && text.ToLower() == "false")
			{
				cpu.propIsSG4Target = false;
			}
			text = "";
			text = reader.GetAttribute("ModUID");
			if (text != null && text.Length > 0)
			{
				uint result2 = 0u;
				if (PviParse.TryParseUInt32(text, out result2))
				{
					cpu.propErrorLogBookModUID = result2;
				}
			}
			text = "";
			text = reader.GetAttribute("Type");
			if (text != null && text.Length > 0)
			{
				cpu.propCPUType = text;
			}
			text = "";
			text = reader.GetAttribute("Accu");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "bad":
					cpu.propAccu = BatteryStates.BAD;
					break;
				case "not_available":
					cpu.propAccu = BatteryStates.NOT_AVAILABLE;
					break;
				case "not_tested":
					cpu.propAccu = BatteryStates.NOT_TESTED;
					break;
				case "ok":
					cpu.propAccu = BatteryStates.OK;
					break;
				case "undefined":
					cpu.propAccu = BatteryStates.UNDEFINED;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("Battery");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "bad":
					cpu.propBattery = BatteryStates.BAD;
					break;
				case "not_available":
					cpu.propBattery = BatteryStates.NOT_AVAILABLE;
					break;
				case "not_tested":
					cpu.propBattery = BatteryStates.NOT_TESTED;
					break;
				case "ok":
					cpu.propBattery = BatteryStates.OK;
					break;
				case "undefined":
					cpu.propBattery = BatteryStates.UNDEFINED;
					break;
				}
			}
			Variable variable = null;
			reader.Read();
			do
			{
				if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Members") == 0)
				{
					variable.ReadMemberVariables(ref reader, flags, variable);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Connection") == 0)
				{
					cpu.Connection.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "ModuleCollection") == 0)
				{
					cpu.Modules.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Module") == 0)
				{
					cpu.Modules.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "IODataPointCollection") == 0)
				{
					cpu.IODataPoints.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "IODataPoint") == 0)
				{
					cpu.IODataPoints.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "MemoryCollection") == 0)
				{
					cpu.Memories.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Memory") == 0)
				{
					cpu.Memories.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "LibraryCollection") == 0)
				{
					cpu.Libraries.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Library") == 0)
				{
					cpu.Libraries.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "LoggerCollection") == 0)
				{
					cpu.Loggers.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Logger") == 0)
				{
					cpu.Loggers.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskCollection") == 0)
				{
					cpu.Tasks.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Task") == 0)
				{
					cpu.Tasks.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "VariableCollection") == 0)
				{
					cpu.Variables.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Variable") == 0)
				{
					cpu.Variables.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskClassCollection") == 0)
				{
					cpu.TaskClasses.FromXmlTextReader(ref reader, flags, cpu);
				}
				else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskClass") == 0)
				{
					cpu.TaskClasses.FromXmlTextReader(ref reader, flags, cpu);
				}
				else
				{
					reader.Read();
				}
			}
			while (reader.NodeType != XmlNodeType.EndElement);
			reader.Read();
			return 0;
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			writer.WriteStartElement("Cpu");
			num = base.ToXMLTextWriter(ref writer, flags);
			if (ApplicationMemory != null && ApplicationMemory.Length > 0)
			{
				writer.WriteAttributeString("ApplicationMemory", ApplicationMemory);
			}
			if (RuntimeVersion != null && RuntimeVersion.Length > 0)
			{
				writer.WriteAttributeString("RuntimeVersion", RuntimeVersion);
			}
			if (propSavePath != null && propSavePath.Length > 0)
			{
				writer.WriteAttributeString("SavePath", SavePath);
			}
			if (propAccu != 0)
			{
				writer.WriteAttributeString("Accu", Accu.ToString());
			}
			if (propBattery != 0)
			{
				writer.WriteAttributeString("Battery", propBattery.ToString());
			}
			if (!IsSG4Target)
			{
				writer.WriteAttributeString("IsSG4Target", propIsSG4Target.ToString());
			}
			if (ModUID != 0)
			{
				writer.WriteAttributeString("ModUID", ModUID.ToString());
			}
			if (NodeNumber != 0)
			{
				writer.WriteAttributeString("NodeNumber", NodeNumber.ToString());
			}
			if (Profiler != null)
			{
				writer.WriteAttributeString("Profiler", Profiler.ToString());
			}
			if (Type != null && Type.Length > 0)
			{
				writer.WriteAttributeString("Type", Type);
			}
			writer.WriteAttributeString("State", State.ToString());
			writer.WriteAttributeString("BootMode", BootMode.ToString());
			Connection.ToXMLTextWriter(ref writer, flags);
			IODataPoints.ToXMLTextWriter(ref writer, flags);
			Libraries.ToXMLTextWriter(ref writer, flags);
			Loggers.ToXMLTextWriter(ref writer, flags);
			Tasks.ToXMLTextWriter(ref writer, flags);
			Memories.ToXMLTextWriter(ref writer, flags);
			Modules.ToXMLTextWriter(ref writer, flags);
			Variables.ToXMLTextWriter(ref writer, flags);
			TaskClasses.ToXMLTextWriter(ref writer, flags);
			writer.WriteEndElement();
			return num;
		}

		internal int ReadModuleList(string moduleName, out APIFC_ModulInfoRes modInfo, out APIFC_DiagModulInfoRes diagModInfo)
		{
			int num = 0;
			object obj = null;
			diagModInfo = default(APIFC_DiagModulInfoRes);
			modInfo = default(APIFC_ModulInfoRes);
			if (propModuleInfoList == null)
			{
				propModuleInfoList = new Hashtable();
			}
			if (propModuleInfoList.ContainsKey(moduleName))
			{
				obj = propModuleInfoList[moduleName];
				if (obj is APIFC_DiagModulInfoRes)
				{
					diagModInfo = (APIFC_DiagModulInfoRes)obj;
				}
				else
				{
					modInfo = (APIFC_ModulInfoRes)obj;
				}
			}
			else
			{
				num = UpdateModuleList(ModuleListOptions.INA2000CompatibleMode);
				if (num == 0)
				{
					return -3;
				}
			}
			return num;
		}

		internal static AccessTypes GetINAUploadAccessType(ModuleListOptions ulOptions, object parentObj)
		{
			if (ModuleListOptions.INA2000List == ulOptions)
			{
				return AccessTypes.ModuleList;
			}
			if (ModuleListOptions.INA2000DiagnosisList == ulOptions)
			{
				return AccessTypes.ReadDiagModList;
			}
			if (parentObj is Cpu && ((Cpu)parentObj).BootMode == BootMode.Diagnostics)
			{
				return AccessTypes.ReadDiagModList;
			}
			return AccessTypes.ModuleList;
		}

		internal int RequestModuleList(ModuleListOptions lstOption)
		{
			int num = 0;
			if (propModuleInfoList != null)
			{
				propModuleInfoList.Clear();
			}
			propModuleInfoRequested = true;
			if (Connection.DeviceType == DeviceType.ANSLTcp)
			{
				return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 622u);
			}
			return ReadArgumentRequest(Service.hPvi, propLinkId, GetINAUploadAccessType(lstOption, this), IntPtr.Zero, 0, 622u);
		}

		internal int UpdateModuleList(ModuleListOptions lstOption)
		{
			if (propModuleInfoRequested)
			{
				return -1;
			}
			if (!IsConnected && 12058 != propErrorCode)
			{
				base.Requests |= Actions.Upload;
				return -2;
			}
			if (12058 == propErrorCode)
			{
				base.Requests |= Actions.Upload;
				return propErrorCode;
			}
			return RequestModuleList(lstOption);
		}

		public int CompareRuntimeVersionTo(string vCompare)
		{
			if (propSWVersion != null)
			{
				if (4 == propSWVersion.Length)
				{
					if (vCompare[1] > propSWVersion[1])
					{
						return -1;
					}
					if (vCompare[1] < propSWVersion[1])
					{
						return 1;
					}
					if (vCompare[2] > propSWVersion[2])
					{
						return -1;
					}
					if (vCompare[2] < propSWVersion[2])
					{
						return 1;
					}
					if (vCompare[3] > propSWVersion[3])
					{
						return -1;
					}
					if (vCompare[3] < propSWVersion[3])
					{
						return 1;
					}
				}
				if (5 == propSWVersion.Length)
				{
					if (vCompare[4] > propSWVersion[4])
					{
						return -1;
					}
					if (vCompare[4] < propSWVersion[4])
					{
						return 1;
					}
				}
				return 0;
			}
			return -2;
		}

		private void ConnectionDisconnected(object sender, PviEventArgs e)
		{
			OnDisconnected(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.CpuDisconnect, Service));
		}

		public int ReadTCPDestinationSettings()
		{
			int num = 0;
			num = ReadRequest(Service.hPvi, base.LinkId, AccessTypes.ResolveNodeNumber, 291u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.ResolveNodeNumber, Service));
			}
			return 0;
		}

		protected virtual void OnTCPDestinationSettingsRead(string strData, PviEventArgs e)
		{
			propTCPDestinationSettings.Parse(strData);
			if (this.TCPDestinationSettingsRead != null)
			{
				this.TCPDestinationSettingsRead(this, e);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[CLSCompliant(false)]
		[Browsable(false)]
		public int CallTTService(ushort ttGroup, byte ttServ, byte ttFormat, byte[] dataBytes, byte dataLen)
		{
			int num = 0;
			byte[] array = new byte[5 + dataLen];
			array[0] = (byte)(ttGroup & 0xFF);
			array[1] = (byte)((ttGroup & 0xFF00) >> 8);
			array[2] = ttServ;
			array[3] = ttFormat;
			array[4] = dataLen;
			for (int i = 0; i < dataLen; i++)
			{
				array[5 + i] = dataBytes[i];
			}
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)array.Length);
			Marshal.Copy(array, 0, hMemory, array.Length);
			num = ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.TTService, hMemory, array.Length, 214u, base.InternId);
			if (num != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, propErrorCode, Service.Language, Action.CpuTTService, Service));
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int WriteDataToPhysicalAddress(int physicalAddress, byte[] dataBytes, int numOfBytesToWrite)
		{
			int num = 0;
			int num2 = numOfBytesToWrite;
			int[] array = new int[2]
			{
				physicalAddress,
				0
			};
			if (numOfBytesToWrite == 0)
			{
				num2 = dataBytes.Length;
			}
			array[1] = num2;
			IntPtr hMemory = PviMarshal.AllocHGlobal(8 + num2);
			Marshal.Copy(array, 0, hMemory, 2);
			IntPtr destination = new IntPtr(hMemory.ToInt64() + 8);
			Marshal.Copy(dataBytes, 0, destination, num2);
			num = WriteRequest(Service.hPvi, base.LinkId, AccessTypes.WritePhysicalMemory, hMemory, 8 + num2, base.InternId);
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int ReadDataFromPhysicalAddress(int physicalAddress, int numOfBytesToRead)
		{
			int num = 0;
			int[] source = new int[2]
			{
				physicalAddress,
				numOfBytesToRead
			};
			IntPtr hMemory = PviMarshal.AllocHGlobal(8);
			Marshal.Copy(source, 0, hMemory, 2);
			num = ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.ReadPhysicalMemory, hMemory, 8, 220u, base.InternId);
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		private void OnTTService(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen)
		{
			int[] array = new int[2];
			byte[] array2 = new byte[2];
			if (5 < dataLen)
			{
				Marshal.Copy(pData, array, 0, 1);
				IntPtr source = new IntPtr(pData.ToInt64() + 2);
				Marshal.Copy(source, array2, 0, 2);
			}
			if (this.TTServiceResponse != null)
			{
				this.TTServiceResponse(this, new CpuTTServiceEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuTTService, (ushort)array[0], array2[0], pData, array2[1]));
			}
		}

		private void OnPhysicalMemoryWritten(int errorCode)
		{
			if (this.PhysicalMemoryWritten != null)
			{
				this.PhysicalMemoryWritten(this, new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuWritePhysicalMemory));
			}
		}

		private void OnPhysicalMemoryRead(int errorCode, IntPtr pData, uint dataLen)
		{
			if (this.PhysicalMemoryRead != null)
			{
				this.PhysicalMemoryRead(this, new CpuPhysicalMemReadEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadPhysicalMemory, pData, (int)dataLen));
			}
		}

		public int GetListOfDongles(ref string dongleData)
		{
			return ListDongles(ref dongleData, syncPVIAccess: true);
		}

		public int GetExistingLicense(string boxMask, string serialNumber, ref string existingLicData)
		{
			return ListOfExistingLicenses(boxMask, serialNumber, ref existingLicData, syncPVIAccess: true);
		}

		public int GetRequiredLicenses(ref string requiredLicData)
		{
			return ListOfRequiredLicenses(ref requiredLicData, syncPVIAccess: true);
		}

		public int GetLicenseContext(string boxMask, string serialNumber, string firmCode, ref string contextLicData)
		{
			return ReadContext(boxMask, serialNumber, new string[1]
			{
				firmCode
			}, ref contextLicData, syncPVIAccess: true);
		}

		public int GetLicenseContext(string boxMask, string serialNumber, string[] firmCodes, ref string contextLicData)
		{
			return ReadContext(boxMask, serialNumber, firmCodes, ref contextLicData, syncPVIAccess: true);
		}

		public int UpdateLicenseData(string licenseData)
		{
			return WriteLicenseData(licenseData, syncPVIAccess: true);
		}

		[CLSCompliant(false)]
		public int BlinkDongle(string boxMask, string serialNumber, uint ledColor, uint blinkCount, uint blinkTime)
		{
			return WriteLicenseBlinkDongle(boxMask, serialNumber, ledColor, blinkCount, blinkTime, syncPVIAccess: true);
		}

		[CLSCompliant(false)]
		public int GetLicenseStatus(uint whichState, ref uint licStatus, bool aSync)
		{
			int num = 0;
			licStatus = 0u;
			if (aSync)
			{
				return GetLicenseStatusASync(whichState);
			}
			return GetLicenseStatusSync(whichState, ref licStatus);
		}

		[CLSCompliant(false)]
		public int GetLicenseStatus(uint whichState, ref uint licStatus)
		{
			return GetLicenseStatusSync(whichState, ref licStatus);
		}

		internal void PVICB_LIC_ReadFunc(int wParam, IntPtr lParam)
		{
			IntPtr zero = IntPtr.Zero;
			PInvokePvicom.PviComFnReadResponse(Service.hPvi, wParam, zero, 0);
		}

		internal void PVICB_LIC_WriteFunc(int wParam, IntPtr lParam)
		{
			PInvokePvicom.PviComFnWriteResponse(Service.hPvi, wParam);
		}

		internal int LicenseDataWrtitten(int wParam, IntPtr lParam, ResponseInfo info)
		{
			int result = 0;
			if (350 == info.Type)
			{
				result = PInvokePvicom.PviComFnWriteResponse(Service.hPvi, wParam);
				licUpdateLicense = true;
			}
			else if (352 == info.Type)
			{
				result = PInvokePvicom.PviComFnWriteResponse(Service.hPvi, wParam);
				licBlinkDongle = true;
			}
			else
			{
				PVICB_LIC_WriteFunc(wParam, lParam);
			}
			return result;
		}

		internal int ReadLicenseData(int wParam, IntPtr lParam, ResponseInfo info, int dataLen, ref uint resultData)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (info.Error == 0)
			{
				intPtr = PviMarshal.AllocHGlobal(dataLen);
			}
			int num = PInvokePvicom.PviComFnReadResponse(Service.hPvi, wParam, intPtr, dataLen);
			if (num == 0)
			{
				if (351 == info.Type)
				{
					PviMarshal.Copy(intPtr, ref resultData);
				}
				else
				{
					PVICB_LIC_ReadFunc(wParam, lParam);
				}
			}
			else if (351 != info.Type)
			{
				PVICB_LIC_ReadFunc(wParam, lParam);
			}
			return num;
		}

		internal int ReadLicenseData(int wParam, IntPtr lParam, ResponseInfo info, int dataLen, ref string resultData)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (info.Error == 0)
			{
				intPtr = PviMarshal.AllocHGlobal(dataLen);
			}
			int num = PInvokePvicom.PviComFnReadResponse(Service.hPvi, wParam, intPtr, dataLen);
			if (num == 0)
			{
				switch (info.Type)
				{
				case 346:
					resultData = PviMarshal.ToAnsiString(intPtr, dataLen);
					licListDongles = true;
					break;
				case 347:
					resultData = PviMarshal.ToAnsiString(intPtr, dataLen);
					licListOfExistingLicenses = true;
					break;
				case 348:
					resultData = PviMarshal.ToAnsiString(intPtr, dataLen);
					licListOfRequiredLicenses = true;
					break;
				case 349:
					resultData = PviMarshal.ToAnsiString(intPtr, dataLen);
					licReadContext = true;
					break;
				case 351:
					resultData = PviMarshal.ToAnsiString(intPtr, dataLen);
					break;
				default:
					PVICB_LIC_ReadFunc(wParam, lParam);
					break;
				}
			}
			else
			{
				switch (info.Type)
				{
				case 346:
					licListDongles = true;
					break;
				case 347:
					licListOfExistingLicenses = true;
					break;
				case 348:
					licListOfRequiredLicenses = true;
					break;
				case 349:
					licReadContext = true;
					break;
				default:
					PVICB_LIC_ReadFunc(wParam, lParam);
					break;
				}
			}
			return num;
		}

		private int GetLicenseStatusASync(uint whichState)
		{
			int num = 0;
			IntPtr hMemory = IntPtr.Zero;
			PviMarshal.Copy(whichState, hMemory);
			num = ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.LIC_GetLicenseStatus, hMemory, 4, 750u);
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		private uint ExtractLicStatus(string strXML)
		{
			uint result = uint.MaxValue;
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(strXML));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.CompareTo("Status") == 0)
					{
						result = uint.Parse(xmlReader.GetAttribute("Error"));
						return result;
					}
				}
				while (xmlReader.Read());
				return result;
			}
			catch
			{
				return result;
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		private int GetLicenseStatusSync(uint whichState, ref uint licStatus)
		{
			int num = 0;
			licStatus = 0u;
			IntPtr hMemory = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			PviMarshal.Copy(whichState, hMemory);
			zero = PviMarshal.AllocCoTaskMem(512);
			propLicStatusError = uint.MaxValue;
			propLicStatus = "";
			num = PInvokePvicom.PviComRead(Service.hPvi, base.LinkId, AccessTypes.LIC_GetLicenseStatus, hMemory, 4, zero, 512);
			propLicStatus = PviMarshal.ToAnsiString(zero, 512);
			propLicStatusError = (licStatus = ExtractLicStatus(propLicStatus));
			PviMarshal.FreeHGlobal(ref zero);
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		private int ListDongles(ref string dongleData, bool syncPVIAccess)
		{
			int num = 0;
			dongleData = "";
			if (syncPVIAccess)
			{
				num = PviReadLicense(AccessTypes.LIC_ListDongles, "", ref dongleData);
				licListDongles = true;
			}
			else
			{
				num = PviLicenseRead(AccessTypes.LIC_ListDongles, ref licListDongles, ref hWaitOnDongles, ref dongleData);
			}
			return num;
		}

		private int ListDongles(ref string dongleData)
		{
			return ListDongles(ref dongleData, syncPVIAccess: false);
		}

		private int ListOfExistingLicenses(string boxMask, string serialNumber, ref string existingLicData, bool syncPVIAccess)
		{
			int num = 0;
			existingLicData = "";
			string requestData = $"BOXMASK={boxMask} SERNUM={serialNumber}";
			if (syncPVIAccess)
			{
				num = PviReadLicense(AccessTypes.LIC_ListOfExistingLicenses, requestData, ref existingLicData);
				licListOfExistingLicenses = true;
			}
			else
			{
				num = PviLicenseRead(AccessTypes.LIC_ListOfExistingLicenses, requestData, ref licListOfExistingLicenses, ref hWaitOnListOfExistingLicenses, ref existingLicData);
			}
			return num;
		}

		private int ListOfExistingLicenses(string boxMask, string serialNumber, ref string existingLicData)
		{
			return ListOfExistingLicenses(boxMask, serialNumber, ref existingLicData, syncPVIAccess: false);
		}

		private int ListOfRequiredLicenses(ref string requiredLicData, bool syncPVIAccess)
		{
			int num = 0;
			requiredLicData = "";
			if (syncPVIAccess)
			{
				num = PviReadLicense(AccessTypes.LIC_ListOfRequiredLicenses, requiredLicData, ref requiredLicData);
				licListOfRequiredLicenses = true;
			}
			else
			{
				num = PviLicenseRead(AccessTypes.LIC_ListOfRequiredLicenses, ref licListOfRequiredLicenses, ref hWaitOnListOfRequiredLicenses, ref requiredLicData);
			}
			return num;
		}

		private int ListOfRequiredLicenses(ref string requiredLicData)
		{
			return ListOfRequiredLicenses(ref requiredLicData, syncPVIAccess: false);
		}

		private int ReadContext(string boxMask, string serialNumber, string[] firmCodes, ref string contextLicData, bool syncPVIAccess)
		{
			int num = 0;
			string text = "";
			string text2 = "";
			for (int i = 0; i < firmCodes.GetLength(0); i++)
			{
				text2 = ((i != 0) ? (text2 + "," + firmCodes.GetValue(i).ToString()) : firmCodes.GetValue(i).ToString());
			}
			text = $"BOXMASK={boxMask} SERNUM={serialNumber} FIRMCODE={text2}";
			if (syncPVIAccess)
			{
				num = PviReadLicense(AccessTypes.LIC_ReadContext, text, ref contextLicData);
				licReadContext = true;
			}
			else
			{
				num = PviLicenseRead(AccessTypes.LIC_ReadContext, text, ref licReadContext, ref hWaitOnReadContext, ref contextLicData);
			}
			return num;
		}

		private int ReadContext(string boxMask, string serialNumber, string[] firmCodes, ref string contextLicData)
		{
			return ReadContext(boxMask, serialNumber, firmCodes, ref contextLicData, syncPVIAccess: false);
		}

		private int WriteLicenseData(string licenseData, bool syncPVIAccess)
		{
			int num = 0;
			if (syncPVIAccess)
			{
				num = PviWriteLicense(AccessTypes.LIC_UpdateLicense, licenseData);
				licUpdateLicense = true;
			}
			else
			{
				num = PviLicenseWrite(AccessTypes.LIC_UpdateLicense, licenseData, ref licUpdateLicense, ref hWaitOnUpdateLicense);
			}
			return num;
		}

		private int WriteLicenseData(string licenseData)
		{
			return WriteLicenseData(licenseData, syncPVIAccess: false);
		}

		private int PviReadLicense(AccessTypes accType, string requestData, ref string resultData)
		{
			int num = 0;
			int argDataLen = 0;
			IntPtr hMemory = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			zero = PviMarshal.AllocCoTaskMem(1048576);
			if (!string.IsNullOrEmpty(requestData))
			{
				argDataLen = requestData.Length;
				hMemory = PviMarshal.StringToHGlobal(requestData);
			}
			num = PInvokePvicom.PviComRead(Service.hPvi, base.LinkId, accType, hMemory, argDataLen, zero, 1048576);
			resultData = PviMarshal.ToAnsiString(zero, 1048576);
			PviMarshal.FreeHGlobal(ref zero);
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return num;
		}

		private int PviLicenseRead(AccessTypes accType, ref bool exitLicLoop, ref EventWaitHandle hWaitOObject, ref string resultData)
		{
			return PviLicenseRead(accType, null, ref exitLicLoop, ref hWaitOObject, ref resultData);
		}

		private int PviLicenseRead(AccessTypes accType, string requestData, ref bool exitLicLoop, ref EventWaitHandle hWaitOObject, ref string licData)
		{
			uint resultUIData = 0u;
			int num = 0;
			return ProcessPviLicenseRead(accType, requestData, ref exitLicLoop, ref hWaitOObject, 0, ref licData, ref resultUIData);
		}

		private int ProcessPviLicenseRead(AccessTypes accType, string requestData, ref bool exitLicLoop, ref EventWaitHandle hWaitOObject, int mode, ref string resultData, ref uint resultUIData)
		{
			int num = 0;
			int num2 = 0;
			IntPtr hMemory = IntPtr.Zero;
			if (!string.IsNullOrEmpty(requestData))
			{
				num2 = requestData.Length;
				hMemory = PviMarshal.StringToHGlobal(requestData);
			}
			exitLicLoop = false;
			num = ((0 >= num2) ? PInvokePvicom.PviComFnReadRequest(Service.hPvi, base.LinkId, accType, cbLICReadFunc, 4294967292u, base.InternId) : PInvokePvicom.PviComFnReadArgumentRequest(Service.hPvi, base.LinkId, accType, hMemory, num2, cbLICReadFunc, 4294967292u, base.InternId));
			PviFunction respFnPtr = null;
			int pParam = 0;
			uint pDataLen = 0u;
			int wParam = 0;
			IntPtr zero = IntPtr.Zero;
			if (num == 0)
			{
				while (!exitLicLoop)
				{
					num = PInvokePvicom.PviComGetNextResponse(Service.hPvi, out wParam, zero, out respFnPtr, hWaitOObject.SafeWaitHandle);
					if (num != 0)
					{
						break;
					}
					if (wParam == 0)
					{
						User32.WaitForSingleObject(hWaitOObject.SafeWaitHandle, 10u);
						continue;
					}
					ResponseInfo pInfo = new ResponseInfo(0, 0, 0, 0, 0);
					num = PInvokePvicom.PviComFnGetResponseInfo(Service.hPvi, wParam, out pParam, out pDataLen, ref pInfo, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
					switch (num)
					{
					case 0:
						if (pInfo.Type == (int)accType && respFnPtr == cbLICReadFunc)
						{
							num = ((mode != 0) ? ReadLicenseData(wParam, zero, pInfo, (int)pDataLen, ref resultUIData) : ReadLicenseData(wParam, zero, pInfo, (int)pDataLen, ref resultData));
						}
						continue;
					case 12055:
						continue;
					}
					break;
				}
			}
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return num;
		}

		private int PviWriteLicense(AccessTypes accType, string requestData)
		{
			int num = 0;
			IntPtr hMemory = IntPtr.Zero;
			int dataLen = 0;
			if (!string.IsNullOrEmpty(requestData))
			{
				dataLen = requestData.Length;
				hMemory = PviMarshal.StringToHGlobal(requestData);
			}
			num = PInvokePvicom.PviComWrite(Service.hPvi, base.LinkId, accType, hMemory, dataLen, IntPtr.Zero, 0);
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return num;
		}

		private int PviLicenseWrite(AccessTypes accType, string requestData, ref bool exitLicLoop, ref EventWaitHandle hWaitOObject)
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			IntPtr pData = IntPtr.Zero;
			int dataLen = 0;
			if (!string.IsNullOrEmpty(requestData))
			{
				pData = PviMarshal.StringToHGlobal(requestData);
			}
			exitLicLoop = false;
			num = PInvokePvicom.PviComFnWriteRequest(Service.hPvi, base.LinkId, accType, pData, dataLen, cbLICWriteFunc, 4294967292u, base.InternId);
			int pParam = 0;
			int wParam = 0;
			uint pDataLen = 0u;
			PviFunction respFnPtr = null;
			if (num == 0)
			{
				while (!exitLicLoop)
				{
					num = PInvokePvicom.PviComGetNextResponse(Service.hPvi, out wParam, zero, out respFnPtr, hWaitOObject.SafeWaitHandle);
					if (num != 0)
					{
						break;
					}
					if (wParam == 0)
					{
						User32.WaitForSingleObject(hWaitOObject.SafeWaitHandle, uint.MaxValue);
						continue;
					}
					ResponseInfo pInfo = new ResponseInfo(0, 0, 0, 0, 0);
					num = PInvokePvicom.PviComFnGetResponseInfo(Service.hPvi, wParam, out pParam, out pDataLen, ref pInfo, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
					switch (num)
					{
					case 0:
						if (pInfo.Type == (int)accType && respFnPtr == cbLICWriteFunc)
						{
							num = LicenseDataWrtitten(wParam, zero, pInfo);
						}
						continue;
					case 12055:
						continue;
					}
					break;
				}
			}
			return num;
		}

		private int WriteLicenseBlinkDongle(string boxMask, string serialNumber, uint ledColor, uint blinkCount, uint blinkTime, bool syncPVIAccess)
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			string requestData = "BOXMASK=" + boxMask + " SERNUM=" + serialNumber + " LEDVIEW=" + ledColor.ToString() + "," + blinkCount.ToString() + "," + blinkTime.ToString();
			if (syncPVIAccess)
			{
				num = PviWriteLicense(AccessTypes.LIC_BlinkDongle, requestData);
				licBlinkDongle = true;
			}
			else
			{
				num = PviLicenseWrite(AccessTypes.LIC_BlinkDongle, requestData, ref licBlinkDongle, ref hWaitOnUpdateLicense);
			}
			return num;
		}

		private int WriteLicenseBlinkDongle(string boxMask, string serialNumber, uint ledColor, uint blinkCount, uint blinkTime)
		{
			return WriteLicenseBlinkDongle(boxMask, serialNumber, ledColor, blinkCount, blinkTime, syncPVIAccess: false);
		}

		private void OnGlobalForcedOFF(int errorCode)
		{
			if (this.GlobalForcedOFF != null)
			{
				this.GlobalForcedOFF(this, new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuGlobalForceOFF));
			}
		}

		public int GlobalForceOFF()
		{
			if (IsSG4Target)
			{
				return WriteRequest(Service.hPvi, propLinkId, AccessTypes.LinkNodeForceOff, IntPtr.Zero, 0, 222u);
			}
			return WriteRequest(Service.hPvi, propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 222u);
		}

		private void OnLicenseStatusRead(int errorCode, IntPtr pData, uint dataLen)
		{
			propLicStatusError = uint.MaxValue;
			propLicStatus = "";
			if (0 < dataLen && errorCode == 0)
			{
				propLicStatus = PviMarshal.ToAnsiString(pData, dataLen);
				propLicStatusError = ExtractLicStatus(propLicStatus);
			}
			if (this.LicenseStatusGot != null)
			{
				this.LicenseStatusGot(this, new PviEventArgs(base.Address, base.Name, errorCode, Service.Language, Action.LIC_GetStatus, Service));
			}
		}

		private void OnMemoryCleared(int errorCode)
		{
			if (this.MemoryCleared != null)
			{
				this.MemoryCleared(this, new PviEventArgs(propClearmemType.ToString(), base.Name, errorCode, Service.Language, Action.ClearMemory, Service));
			}
		}

		public int ClearMemory(MemoryType memType)
		{
			int num = 0;
			int num2 = 0;
			if (BootMode != BootMode.Diagnostics)
			{
				return 4025;
			}
			propClearmemType = memType;
			num2 = Marshal.SizeOf(typeof(int));
			Marshal.WriteInt32(Service.RequestBuffer, (int)memType);
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.ClearMemory, Service.RequestBuffer, num2, 618u);
		}

		public int ReadMemoryInfo()
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				return ReadRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_MemoryInfo, 726u);
			}
			return 12058;
		}

		private void OnMemoryInfoRead(int errorCode, IntPtr pData, uint dataLen)
		{
			if (this.MemoryInfoRead != null)
			{
				propMemoryInfo = "";
				propMemoryInformationStruct = null;
				if (0 < dataLen)
				{
					propMemoryInfo = PviMarshal.PtrToStringAnsi(pData);
					propMemoryInformationStruct = new MemoryInformation(propMemoryInfo);
				}
				PviEventArgs e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadMemoryInfo);
				this.MemoryInfoRead(this, e);
			}
		}

		public int ReadHardwareInfo()
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				return ReadRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_HardwareInfo, 727u);
			}
			return 12058;
		}

		private void OnHardwareInfoRead(int errorCode, IntPtr pData, uint dataLen)
		{
			if (this.HardwareInfoRead != null)
			{
				propHardwareInfo = "";
				propHardwareInformationStruct = null;
				if (0 < dataLen)
				{
					propHardwareInfo = PviMarshal.PtrToStringAnsi(pData);
					propHardwareInformationStruct = new HardwareInformation(propHardwareInfo);
				}
				PviEventArgs e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadHardwareInfo);
				this.HardwareInfoRead(this, e);
			}
		}

		public int ReadApplicationInfo()
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				return ReadRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_ApplicationInfo, 729u);
			}
			return 12058;
		}

		private void OnApplicationInfoRead(int errorCode, IntPtr pData, uint dataLen)
		{
			if (this.ApplicationInfoRead != null)
			{
				PviEventArgsXMLApplicationInfo e = new PviEventArgsXMLApplicationInfo(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadApplicationInfo, pData, dataLen);
				this.ApplicationInfoRead(this, e);
			}
		}

		private void OnRedundancyInfoRead(int errorCode, IntPtr pData, uint dataLen)
		{
			if (dataLen == 0)
			{
				RedundancyInfo = "";
				RedundancyInformationStruct = null;
			}
			else
			{
				RedundancyInfo = PviMarshal.PtrToStringAnsi(pData);
				RedundancyInformationStruct = new RedundancyInformation(RedundancyInfo);
			}
			PviEventArgs e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuReadRedundancyInfo);
			this.RedundancyInfoRead(this, e);
		}

		private void OnRedundancyInfoChanged(int errorCode, IntPtr pData, uint dataLen)
		{
			if (dataLen == 0)
			{
				RedundancyInfo = "";
				RedundancyInformationStruct = null;
			}
			else
			{
				RedundancyInfo = PviMarshal.PtrToStringAnsi(pData);
				RedundancyInformationStruct = new RedundancyInformation(RedundancyInfo);
			}
			if (this.RedundancyInfoChanged != null)
			{
				PviEventArgs e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.CpuEventRedundancyInfo);
				this.RedundancyInfoChanged(this, e);
			}
		}

		private void OnActiveCpuChanged(PviEventArgs e)
		{
			if (this.ActiveCpuChanged != null)
			{
				this.ActiveCpuChanged(this, e);
			}
		}

		private void OnApplicationSynchronizeStarted(PviEventArgs e)
		{
			if (this.ApplicationSynchronizeStarted != null)
			{
				this.ApplicationSynchronizeStarted(this, e);
			}
		}

		private void OnRedundancyCtrlEvent(int errorCode, IntPtr pData, uint dataLen)
		{
			int num = 0;
			int num2 = errorCode;
			int num3 = 0;
			int num4 = 0;
			string text = "";
			text = ((0 >= dataLen) ? "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<CpuRedCtrl Cmd=\"2\" Percent=\"23\" Error=\"12055\" />\n" : PviMarshal.PtrToStringAnsi(pData));
			if (-1 != text.IndexOf("Cmd=\"1\""))
			{
				num3 = text.IndexOf("Error=");
				if (0 < num3)
				{
					num4 = text.IndexOf("\"", num3 + 8);
					num2 = int.Parse(text.Substring(num3 + 7, num4 - num3 - 7));
				}
				OnActiveCpuChanged(new PviEventArgs(base.Name, base.Address, num2, Service.Language, Action.CpuSwitchActiveCpu, Service));
			}
			else if (-1 != text.IndexOf("Cmd=\"16\""))
			{
				OnRedundancyInfoChanged(errorCode, pData, dataLen);
			}
			else if (-1 != text.IndexOf("Cmd=\"2\"") && this.ApplicationSyncing != null)
			{
				num3 = text.IndexOf("Error=");
				if (0 < num3)
				{
					num4 = text.IndexOf("\"", num3 + 8);
					num2 = int.Parse(text.Substring(num3 + 7, num4 - num3 - 7));
				}
				num = 0;
				num3 = text.IndexOf("Percent=");
				if (0 < num3)
				{
					num4 = text.IndexOf("\"", num3 + 10);
					num = int.Parse(text.Substring(num3 + 9, num4 - num3 - 9));
				}
				this.ApplicationSyncing(this, new PviProgessEventArgs(base.Name, base.Address, num2, Service.Language, Action.CpuSynchronizeApplication, num));
			}
		}

		public int ReadRedundancyInfo()
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				return ReadRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_RedundancyInfo, 722u);
			}
			return 12058;
		}

		public int SwitchActiveCpu(bool force)
		{
			int num = 0;
			if (DeviceType.ANSLTcp != propConnection.DeviceType)
			{
				return 12058;
			}
			string strValue = (!force) ? "CMD=\"1\"" : "CMD=\"1\" ARG=\"1\"";
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_RedundancyControl, strValue, 723u, _internId);
		}

		public int SwitchActiveCpu()
		{
			return SwitchActiveCpu(force: false);
		}

		public int SynchronizeRApplication(bool automaticMode)
		{
			int num = 0;
			if (DeviceType.ANSLTcp != propConnection.DeviceType)
			{
				return 12058;
			}
			string strValue = (!automaticMode) ? "CMD=\"2\" ARG=\"1\"" : "CMD=\"2\" ARG=\"0\"";
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_RedundancyControl, strValue, 724u, _internId);
		}

		private void OnTOCRead(string name, string address, int error, IntPtr pData, uint dataLen)
		{
			string xmlData = null;
			if (this.TOCRead != null)
			{
				if (dataLen != 0)
				{
					xmlData = PviMarshal.PtrToStringUTF8(pData, dataLen);
				}
				PviEventArgsXML e = new PviEventArgsXML(name, address, error, Service.Language, Action.CpuExtendedInfoTOC, xmlData);
				this.TOCRead(this, e);
			}
		}

		public int ReadTOC()
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				string reqData = "TYPE=1 ARG=0";
				return ReadArgumentRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_CpuExtendedInfo, reqData, 725u, _internId);
			}
			return 12058;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int SendXMLCommand(string commandData)
		{
			int num = 0;
			if (DeviceType.ANSLTcp == propConnection.DeviceType)
			{
				return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_COMMAND_Data, commandData, 730u, _internId);
			}
			return 12058;
		}

		private void OnXMLCommand(int errorCode, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			int error = errorCode;
			string xmlData = "";
			if (dataLen != 0 && IntPtr.Zero != pData)
			{
				try
				{
					xmlData = PviMarshal.PtrToStringAnsi(pData);
				}
				catch
				{
					error = 12054;
				}
			}
			if (this.XMLCommandSent != null)
			{
				this.XMLCommandSent(this, new PviEventArgsXML(base.Name, base.Address, error, Service.Language, Action.CpuXMLCommand, xmlData));
			}
		}

		private void OnBondChanged(int errorCode, IntPtr pData, uint dataLen)
		{
			string text = "";
			if (0 < dataLen)
			{
				text = PviMarshal.PtrToStringAnsi(pData);
				BondInformationStruct = new BondInformation(text);
			}
			if (this.BondInfoChanged != null)
			{
				PviEventArgsXML e = new PviEventArgsXML(base.Name, base.Address, errorCode, Service.Language, Action.CpuEventBondInfo, text);
				this.BondInfoChanged(this, e);
			}
		}

		private void OnLicenseChanged(int errorCode, IntPtr pData, uint dataLen)
		{
			string xmlData = "";
			if (0 < dataLen)
			{
				xmlData = PviMarshal.PtrToStringAnsi(pData);
			}
			if (this.LicenseChanged != null)
			{
				PviEventArgsXML e = new PviEventArgsXML(base.Name, base.Address, errorCode, Service.Language, Action.CpuEventLicnenseInfo, xmlData);
				this.LicenseChanged(this, e);
			}
		}

		internal int UpdateCpuBasicInfo()
		{
			int num = 0;
			return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_CpuInfo, IntPtr.Zero, 0, 740u);
		}

		internal int UpdateHardwareInfo()
		{
			int num = 0;
			return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_HardwareInfo, IntPtr.Zero, 0, 740u);
		}

		internal int UpdateRedundancyInfo()
		{
			int num = 0;
			return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_RedundancyInfo, IntPtr.Zero, 0, 740u);
		}

		internal int UpdateTableOfContens()
		{
			int num = 0;
			return ReadArgumentRequest(Service.hPvi, propLinkId, AccessTypes.ANSL_CpuExtendedInfo, "TYPE=1 ARG=0", 740u);
		}

		internal int UpdateTechnologyPackages(string techPkgCommandData)
		{
			int num = 0;
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_COMMAND_Data, techPkgCommandData, 740u, _internId);
		}
	}
}
