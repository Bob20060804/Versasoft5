using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Service : Base
	{
		private delegate void DisposeMSGWindowCB(int mode);

		internal delegate void SetPviDisconnectCallback(uint hPvi);

		internal const int ExternalResponseHandlerOption = 21;

		internal uint propPendingObjectBrowserEvents;

		private bool propConnectionInterupted;

		private uint propModuleUIDBuilder;

		private Utilities propUtils;

		private SimpleNetworkManagementProtocol propSNMP;

		private PviObjectBrowser propServicePviObj;

		private ArrayList propClientNames;

		private LicenceInfo propLicenseInfo;

		private int propRetryTime;

		private int propProcessTimeout;

		private int propMessageLimitation;

		private int propPVIAutoStart;

		private IntPtr propWindowHandle;

		private bool propAddMembersToVariableCollection;

		private bool propExtendedTypeInfoAutoUpdateForVariables;

		private bool propAddStructMembersToMembersToo;

		private bool propUserTagEvents;

		internal PviMessageFunction _pviGlobalCB;

		internal PviCallback callback;

		internal PviCallback cbCreate;

		internal PviCallback cbLink;

		internal PviCallback cbLinkA;

		internal PviCallback cbLinkB;

		internal PviCallback cbLinkC;

		internal PviCallback cbLinkD;

		internal PviCallback cbLinkE;

		internal PviCallback cbEvent;

		internal PviCallback cbEventA;

		internal PviCallback cbRead;

		internal PviCallback cbReadA;

		internal PviCallback cbReadB;

		internal PviCallback cbReadC;

		internal PviCallback cbReadD;

		internal PviCallback cbReadE;

		internal PviCallback cbReadF;

		internal PviCallback cbReadG;

		internal PviCallback cbReadS;

		internal PviCallback cbReadT;

		internal PviCallback cbReadU;

		internal PviCallback cbWrite;

		internal PviCallback cbWriteA;

		internal PviCallback cbWriteB;

		internal PviCallback cbWriteC;

		internal PviCallback cbWriteD;

		internal PviCallback cbWriteE;

		internal PviCallback cbWriteF;

		internal PviCallback cbWriteU;

		internal PviCallback cbUnlink;

		internal PviCallback cbUnlinkA;

		internal PviCallback cbUnlinkB;

		internal PviCallback cbUnlinkC;

		internal PviCallback cbUnlinkD;

		internal PviCallback cbDelete;

		internal PviCallback cbChangeLink;

		internal PviCallback cbSNMP;

		internal IntPtr RequestBuffer;

		private char[] propEventChars;

		internal uint hPvi;

		private ServiceCollection propServices;

		private int propTimeout;

		private string propLanguage;

		private bool propErrorexception;

		private bool propErrorevent;

		private bool isStatic;

		private string propServer;

		private int propPort;

		private CpuCollection propCpus;

		private CollectionType propCollectionType;

		private LogicalCollection propLogicalObjects;

		private Cpu propCpu;

		private LogicalObjectsUsage propLogicalUsage;

		private EventMessageType propEventMsgType;

		private VariableCollection propVariables;

		internal PlcMessageWindow propMSGWindow;

		internal Hashtable InternIDs;

		internal uint ActID;

		internal Version propUserTagMinVersion;

		private bool propWaitForParentConnection;

		internal LoggerEntryCollection propLoggerEntries;

		private ArrayList propLoggerCollections;

		internal uint EntryIDInc;

		internal uint EntryIDDec = uint.MaxValue;

		private IntPtr iptr2Byte;

		private IntPtr iptr4Byte;

		private IntPtr marshStrPtr;

		private IntPtr iptr8Byte;

		private float[] marshF32;

		private double[] marshF64;

		private long uint64Val;

		private byte[] byteBuffer;

		internal Utilities Utilities => propUtils;

		public SimpleNetworkManagementProtocol SNMP => propSNMP;

		public TraceWriter Trace => Services.Trace;

		public ArrayList ClientNames => propClientNames;

		public LicenceInfo LicenceInfo => propLicenseInfo;

		internal EventMessageType EventMessageType => propEventMsgType;

		public LogicalCollection LogicalObjects => propLogicalObjects;

		internal uint ModuleUID => propModuleUIDBuilder++;

		public virtual int Timeout
		{
			get
			{
				return propTimeout;
			}
			set
			{
				propTimeout = value;
			}
		}

		public int RetryTime
		{
			get
			{
				return propRetryTime;
			}
			set
			{
				propRetryTime = value;
			}
		}

		public int ProcessTimeout
		{
			get
			{
				return propProcessTimeout;
			}
			set
			{
				propProcessTimeout = value;
			}
		}

		public int MessageLimitation
		{
			get
			{
				return propMessageLimitation;
			}
			set
			{
				propMessageLimitation = value;
			}
		}

		public int PVIAutoStart
		{
			get
			{
				return propPVIAutoStart;
			}
			set
			{
				propPVIAutoStart = value;
			}
		}

		internal IntPtr WindowHandle => propWindowHandle;

		public string Language
		{
			get
			{
				return propLanguage;
			}
			set
			{
				propLanguage = value;
				propUtils.ActiveCulture = propLanguage;
			}
		}

		public bool EnhancedTransfer
		{
			get;
			set;
		}

		internal virtual bool ErrorException
		{
			get
			{
				return propErrorexception;
			}
			set
			{
				propErrorexception = value;
			}
		}

		internal virtual bool ErrorEvent
		{
			get
			{
				return propErrorevent;
			}
			set
			{
				propErrorevent = value;
			}
		}

		public virtual int Port
		{
			get
			{
				return propPort;
			}
			set
			{
				propPort = value;
			}
		}

		public virtual string Server
		{
			get
			{
				return propServer;
			}
			set
			{
				propServer = value;
			}
		}

		public CpuCollection Cpus => propCpus;

		internal CollectionType CollectionType => propCollectionType;

		public bool IsStatic
		{
			get
			{
				return isStatic;
			}
			set
			{
				isStatic = value;
			}
		}

		public bool AddMembersToVariableCollection
		{
			get
			{
				return propAddMembersToVariableCollection;
			}
			set
			{
				propAddMembersToVariableCollection = value;
			}
		}

		public override string FullName => base.Name;

		public override string PviPathName => "@Pvi";

		public bool ExtendedTypeInfoAutoUpdateForVariables
		{
			get
			{
				return propExtendedTypeInfoAutoUpdateForVariables;
			}
			set
			{
				propExtendedTypeInfoAutoUpdateForVariables = value;
			}
		}

		public bool AddStructMembersToMembersToo
		{
			get
			{
				return propAddStructMembersToMembersToo;
			}
			set
			{
				propAddStructMembersToMembersToo = value;
			}
		}

		public LogicalObjectsUsage LogicalObjectsUsage
		{
			get
			{
				return propLogicalUsage;
			}
			set
			{
				propLogicalUsage = value;
			}
		}

		public bool UserTagEvents
		{
			get
			{
				return propUserTagEvents;
			}
			set
			{
				propUserTagEvents = value;
			}
		}

		public VariableCollection Variables => propVariables;

		internal ServiceCollection Services
		{
			get
			{
				return propServices;
			}
			set
			{
				propServices = value;
			}
		}

		public bool WaitForParentConnection
		{
			get
			{
				return propWaitForParentConnection;
			}
			set
			{
				propWaitForParentConnection = value;
			}
		}

		public LoggerEntryCollection LoggerEntries => propLoggerEntries;

		public ArrayList LoggerCollections => propLoggerCollections;

		internal byte[] ByteBuffer => byteBuffer;

		public event PVIObjectsAttachedEventHandler PVIObjectsAttached;

		public event PviEventHandler LicencInfoUpdated;

		public int GetAssemblyVersions(ref string productVersion, ref string fileVersion)
		{
			int num = 0;
			return Pvi.GetNativeDLLVersions(ref productVersion, ref fileVersion);
		}

		internal Service()
		{
			InitEmpty();
			InitBuffers();
			InternIDs = new Hashtable(1000);
			propUserTagMinVersion = new Version("2.50");
			propUserTagEvents = true;
		}

		public Service(EventMessageType commMethod, string name)
			: base(name)
		{
			Init(commMethod, name, null);
		}

		public Service(EventMessageType commMethod, string name, ServiceCollection services)
			: base(name)
		{
			Init(commMethod, name, services);
		}

		public Service(string name)
			: base(name)
		{
			Init(EventMessageType.CallBack, name, null);
		}

		public Service(string name, ServiceCollection services)
			: base(name)
		{
			Init(EventMessageType.CallBack, name, services);
		}

		internal void InitEmpty()
		{
			EnhancedTransfer = false;
			propAddStructMembersToMembersToo = false;
			propEventMsgType = EventMessageType.CallBack;
			propExtendedTypeInfoAutoUpdateForVariables = false;
			propAddMembersToVariableCollection = false;
			ActID = 0u;
			hPvi = 0u;
			propUtils = null;
			propClientNames = null;
			propWindowHandle = IntPtr.Zero;
			propLicenseInfo = new LicenceInfo(this);
			propConnectionInterupted = false;
			propErrorexception = false;
			propErrorevent = true;
			propTimeout = 0;
			propRetryTime = 0;
			propProcessTimeout = 0;
			propMessageLimitation = 1;
			propPVIAutoStart = 1;
			propLanguage = "en-us";
			propWaitForParentConnection = true;
			callback = null;
			cbCreate = null;
			cbLink = null;
			cbLinkA = null;
			cbLinkB = null;
			cbLinkC = null;
			cbLinkD = null;
			cbLinkE = null;
			cbEvent = null;
			cbEventA = null;
			cbRead = null;
			cbReadA = null;
			cbReadB = null;
			cbReadC = null;
			cbReadD = null;
			cbReadE = null;
			cbReadF = null;
			cbReadG = null;
			cbReadS = null;
			cbReadT = null;
			cbReadU = null;
			cbWrite = null;
			cbWriteA = null;
			cbWriteB = null;
			cbWriteC = null;
			cbWriteD = null;
			cbWriteE = null;
			cbWriteF = null;
			cbWriteU = null;
			cbUnlink = null;
			cbUnlinkA = null;
			cbUnlinkB = null;
			cbUnlinkC = null;
			cbUnlinkD = null;
			cbDelete = null;
			cbChangeLink = null;
			cbSNMP = null;
			propMSGWindow = null;
			RequestBuffer = IntPtr.Zero;
			propEventChars = null;
			byteBuffer = null;
			iptr2Byte = IntPtr.Zero;
			iptr4Byte = IntPtr.Zero;
			iptr8Byte = IntPtr.Zero;
			marshStrPtr = IntPtr.Zero;
			marshF32 = null;
			marshF64 = null;
			propWindowHandle = IntPtr.Zero;
			propCollectionType = CollectionType.HashTable;
			propCpus = null;
			propVariables = null;
			propCpu = null;
			propLoggerEntries = null;
			propLoggerCollections = null;
			propServices = null;
			LogicalObjectsUsage = LogicalObjectsUsage.FullName;
			propLogicalObjects = null;
			propSNMP = null;
		}

		internal void InitBuffers()
		{
			RequestBuffer = PviMarshal.AllocHGlobal(256);
			propEventChars = new char[256];
			byteBuffer = new byte[8];
			iptr2Byte = PviMarshal.AllocHGlobal(2);
			iptr4Byte = PviMarshal.AllocHGlobal(4);
			iptr8Byte = PviMarshal.AllocHGlobal(8);
			marshStrPtr = IntPtr.Zero;
			marshF32 = new float[1];
			marshF64 = new double[1];
		}

		internal void Init(EventMessageType commMethod, string name, ServiceCollection services)
		{
			InitEmpty();
			InternIDs = new Hashtable(1000);
			propUserTagMinVersion = new Version("2.50");
			propUserTagEvents = true;
			propService = this;
			propModuleUIDBuilder = 1u;
			propEventMsgType = commMethod;
			ServiceCollection.Services.Add(base.Name, this);
			propUtils = new Utilities();
			propUtils.ActiveCulture = propLanguage;
			InitBuffers();
			propClientNames = new ArrayList(1);
			propLicenseInfo = new LicenceInfo(this);
			propUtils.ActiveCulture = propLanguage;
			propMSGWindow = new PlcMessageWindow(this);
			_pviGlobalCB = this.PviGlobalEvents;
			callback = this.Callback;
			cbCreate = this.PVICB_Create;
			cbLink = this.PVICB_Link;
			cbLinkA = this.PVICB_LinkA;
			cbLinkB = this.PVICB_LinkB;
			cbLinkC = this.PVICB_LinkC;
			cbLinkD = this.PVICB_LinkD;
			cbLinkE = this.PVICB_LinkE;
			cbEvent = this.PVICB_Event;
			cbEventA = this.PVICB_EventA;
			cbRead = this.PVICB_Read;
			cbReadA = this.PVICB_ReadA;
			cbReadB = this.PVICB_ReadB;
			cbReadC = this.PVICB_ReadC;
			cbReadD = this.PVICB_ReadD;
			cbReadE = this.PVICB_ReadE;
			cbReadF = this.PVICB_ReadF;
			cbReadG = this.PVICB_ReadG;
			cbReadS = this.PVICB_ReadS;
			cbReadT = this.PVICB_ReadT;
			cbReadU = this.PVICB_ReadU;
			cbWrite = this.PVICB_Write;
			cbWriteA = this.PVICB_WriteA;
			cbWriteB = this.PVICB_WriteB;
			cbWriteC = this.PVICB_WriteC;
			cbWriteD = this.PVICB_WriteD;
			cbWriteE = this.PVICB_WriteE;
			cbWriteF = this.PVICB_WriteF;
			cbWriteU = this.PVICB_WriteU;
			cbUnlink = this.PVICB_Unlink;
			cbUnlinkA = this.PVICB_UnlinkA;
			cbUnlinkB = this.PVICB_UnlinkB;
			cbUnlinkC = this.PVICB_UnlinkC;
			cbUnlinkD = this.PVICB_UnlinkD;
			cbDelete = this.PVICB_Delete;
			cbChangeLink = this.PVICB_ChangeLink;
			cbSNMP = this.PVICB_SNMP;
			propMSGWindow.CreateControl();
			propWindowHandle = propMSGWindow.Handle;
			propCollectionType = CollectionType.HashTable;
			propCpus = new CpuCollection(CollectionType.HashTable, this, "");
			propCpus.propInternalCollection = true;
			propVariables = new VariableCollection(CollectionType.HashTable, this, base.Name + ".Variables");
			propCpu = null;
			propLoggerEntries = new ServiceLoggerEntryCollection(this, base.Name + ".LogEntries");
			propLoggerCollections = new ArrayList(1);
			propServices = services;
			if (propServices != null)
			{
				propServices.Add(this);
				LogicalObjectsUsage = propServices.LogicalObjectsUsage;
			}
			else
			{
				LogicalObjectsUsage = LogicalObjectsUsage.FullName;
			}
			propLogicalObjects = new LogicalCollection(this, base.Name + ".Logicals");
			propSNMP = new SimpleNetworkManagementProtocol(this);
		}

		internal bool ContainsIDKey(uint internID)
		{
			if (InternIDs != null)
			{
				InternIDs.ContainsKey(internID);
			}
			return false;
		}

		internal object GetObjectForId(uint internID)
		{
			if (InternIDs != null && InternIDs.ContainsKey(internID))
			{
				return InternIDs[internID];
			}
			return null;
		}

		internal bool AddID(object objTarget, ref uint internID)
		{
			if (InternIDs != null)
			{
				if (ActID == uint.MaxValue)
				{
					uint num;
					for (num = 1u; InternIDs.ContainsKey(num); num++)
					{
					}
					internID = num;
					InternIDs.Add(num, objTarget);
					return true;
				}
				internID = ++ActID;
				InternIDs.Add(internID, objTarget);
				return true;
			}
			return false;
		}

		internal void RemoveID(uint internID)
		{
			if (InternIDs != null)
			{
				InternIDs.Remove(internID);
			}
		}

		internal int Initialize(ref uint pviHandle, int ComTimeout, int RetryTimeMessage, string pInitParam, IntPtr pRes2)
		{
			int num = 0;
			StringMarshal stringMarshal = new StringMarshal();
			byte[] bytes = stringMarshal.GetBytes(pInitParam);
			if (EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComInitialize(out pviHandle, ComTimeout, RetryTimeMessage, pInitParam, pRes2);
			}
			return PInvokePvicom.PviComMsgInitialize(out pviHandle, ComTimeout, RetryTimeMessage, bytes, pRes2);
		}

		internal void BuildRequestBuffer(string request)
		{
			propEventChars = request.ToCharArray();
			for (int i = 0; i < propEventChars.GetLength(0); i++)
			{
				Marshal.WriteByte(Service.RequestBuffer, i, (byte)(char)propEventChars.GetValue(i));
			}
		}

		public string GetErrorText(int errNo)
		{
			return propUtils.GetErrorText(errNo);
		}

		internal int SetGlobEventMsg(uint pviHandle, uint globalEvents, uint eventMsgNo, uint eventParam)
		{
			int num = 0;
			switch (EventMessageType)
			{
			case EventMessageType.CallBack:
				return PInvokePvicom.PviComSetGlobEventMsg(pviHandle, globalEvents, callback, eventMsgNo, eventParam);
			case EventMessageType.WindowMessage:
				return PInvokePvicom.PviComMsgSetGlobEventMsg(pviHandle, globalEvents, WindowHandle, eventMsgNo, eventParam);
			case EventMessageType.MessageFunction:
				return PInvokePvicom.PviComFunctionSetGlobEventMsg(pviHandle, globalEvents, _pviGlobalCB, 4294967292u, eventParam);
			default:
				return PInvokePvicom.PviComSetGlobEventMsg(pviHandle, globalEvents, callback, eventMsgNo, eventParam);
			}
		}

		internal static int Deinitialize(uint hPvi)
		{
			int num = 0;
			if (8 == IntPtr.Size)
			{
				return PInvokePvicom.Pvi64XDeinitialize(hPvi);
			}
			return PInvokePvicom.PviXDeinitialize(hPvi);
		}

		public virtual void Connect(string server, int port)
		{
			propServer = server;
			propPort = port;
			Connect();
		}

		private void DisposeMSGWnd(int mode)
		{
			if (propMSGWindow.InvokeRequired)
			{
				DisposeMSGWindowCB method = DisposeMSGWnd;
				propMSGWindow.Invoke(method, mode);
			}
			else
			{
				propMSGWindow.Dispose();
				propMSGWindow = null;
			}
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
				if (removeFromCollection && Services != null)
				{
					Services.Remove(base.Name);
				}
				byteBuffer = null;
				if (propCpus != null)
				{
					propCpus.Dispose(disposing, removeFromCollection);
					propCpus = null;
				}
				if (IntPtr.Zero != iptr2Byte)
				{
					PviMarshal.FreeHGlobal(ref iptr2Byte);
					iptr2Byte = IntPtr.Zero;
				}
				if (IntPtr.Zero != iptr4Byte)
				{
					PviMarshal.FreeHGlobal(ref iptr4Byte);
					iptr4Byte = IntPtr.Zero;
				}
				if (IntPtr.Zero != iptr8Byte)
				{
					PviMarshal.FreeHGlobal(ref iptr8Byte);
					iptr8Byte = IntPtr.Zero;
				}
				propLanguage = null;
				if (propLoggerCollections != null)
				{
					propLoggerCollections.Clear();
					propLoggerCollections = null;
				}
				if (propLoggerEntries != null)
				{
					propLoggerEntries.Dispose(disposing, removeFromCollection);
					propLoggerEntries = null;
				}
				if (propVariables != null)
				{
					propVariables.Dispose(disposing, removeFromCollection);
					propVariables = null;
				}
				if (propServicePviObj != null)
				{
					propServicePviObj = null;
				}
				if (propSNMP != null)
				{
					propSNMP.Cleanup();
					propSNMP = null;
				}
				if (propUtils != null)
				{
					propUtils.Dispose(disposing);
					propUtils = null;
				}
				if (propMSGWindow != null)
				{
					DisposeMSGWnd(0);
				}
				if (propLicenseInfo != null)
				{
					propLicenseInfo.Dispose(disposing);
					propLicenseInfo = null;
				}
				if (propLogicalObjects != null)
				{
					propLogicalObjects.Dispose(disposing, removeFromCollection);
					propLogicalObjects = null;
				}
				if (marshF32 != null)
				{
					marshF32 = null;
				}
				if (marshF64 != null)
				{
					marshF64 = null;
				}
				if (IntPtr.Zero != marshStrPtr)
				{
					PviMarshal.FreeHGlobal(ref marshStrPtr);
					marshStrPtr = IntPtr.Zero;
				}
				PviMarshal.FreeHGlobal(ref marshStrPtr);
				if (propClientNames != null)
				{
					propClientNames.Clear();
					propClientNames = null;
				}
				propCpu = null;
				base.propParent = null;
				propUserTagMinVersion = null;
				propServer = null;
				propServices = null;
				propWindowHandle = IntPtr.Zero;
				propEventChars = null;
				if (IntPtr.Zero != RequestBuffer)
				{
					PviMarshal.FreeHGlobal(ref RequestBuffer);
					RequestBuffer = IntPtr.Zero;
				}
			}
			callback = this.Callback;
			cbCreate = null;
			cbLink = null;
			cbLinkA = null;
			cbLinkB = null;
			cbLinkC = null;
			cbLinkD = null;
			cbLinkE = null;
			cbEvent = null;
			cbEventA = null;
			cbRead = null;
			cbReadA = null;
			cbReadB = null;
			cbReadC = null;
			cbReadD = null;
			cbReadE = null;
			cbReadF = null;
			cbReadG = null;
			cbReadS = null;
			cbReadT = null;
			cbReadU = null;
			cbWrite = null;
			cbWriteA = null;
			cbWriteB = null;
			cbWriteC = null;
			cbWriteD = null;
			cbWriteE = null;
			cbWriteF = null;
			cbUnlink = null;
			cbUnlinkA = null;
			cbUnlinkB = null;
			cbUnlinkC = null;
			cbUnlinkD = null;
			cbDelete = null;
			cbChangeLink = null;
			cbSNMP = null;
			Deinitialize(hPvi);
			base.propParent = null;
			base.propLinkName = null;
			base.propLogicalName = null;
			base.propUserData = null;
			base.propName = null;
			base.propAddress = null;
			byteBuffer = null;
			callback = null;
			_pviGlobalCB = null;
			propErrorText = null;
			propEventChars = null;
			if (InternIDs != null)
			{
				InternIDs.Clear();
			}
			InternIDs = null;
			propLanguage = null;
			base.propLinkName = null;
			propLinkParam = null;
			base.propLogicalName = null;
			this.LicencInfoUpdated = null;
			this.PVIObjectsAttached = null;
			if (propMSGWindow != null)
			{
				DisposeMSGWnd(0);
			}
			propSNMP = null;
			propServer = null;
			base.propUserData = null;
			propUserTagMinVersion = null;
		}

		public override void Connect()
		{
			Connect(ConnectionType.CreateAndLink);
		}

		public override void Connect(ConnectionType connectionType)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			if (ConnectionStates.Connecting == propConnectionState)
			{
				return;
			}
			propConnectionState = ConnectionStates.Connecting;
			if (IsConnected)
			{
				propErrorCode = (propReturnValue = 12002);
				OnError(new PviEventArgs(propName, propAddress, 12002, Service.Language, Action.ServiceConnect, this));
				return;
			}
			text = "LM=" + propMessageLimitation.ToString();
			if (1 != propPVIAutoStart)
			{
				text3 = " AS=" + propPVIAutoStart.ToString();
			}
			if (0 < propProcessTimeout)
			{
				text2 = " PT=" + propProcessTimeout.ToString();
			}
			string pInitParam = (Server == null || Server.Length <= 0) ? $"{text}{text2}{text3}" : $"{text} IP={Server} PN={Port}{text2}{text3}";
			StringBuilder stringBuilder = new StringBuilder(256);
			int num = PInvokePvicom.GetActiveWindow();
			for (int parent = PInvokePvicom.GetParent(num); parent != 0; parent = PInvokePvicom.GetParent(num))
			{
				num = parent;
			}
			PInvokePvicom.GetWindowText(num, stringBuilder, stringBuilder.Capacity + 1);
			string text4 = stringBuilder.ToString();
			int windowContextHelpId = PInvokePvicom.GetWindowContextHelpId(num);
			if (((windowContextHelpId == 131571 || windowContextHelpId == 131570) && -1 != text4.IndexOf("Automation Studio")) || (windowContextHelpId == 913354 && -1 != text4.IndexOf("Runtime Utility Center")))
			{
				APIFC_PviSecure aPIFC_PviSecure = default(APIFC_PviSecure);
				aPIFC_PviSecure.first = 1381322324;
				aPIFC_PviSecure.second = -1381322325;
				IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)Marshal.SizeOf(typeof(APIFC_PviSecure)));
				Marshal.StructureToPtr((object)aPIFC_PviSecure, hMemory, fDeleteOld: false);
				PInvokePvicom.PviComServerClient(hMemory, 0);
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			if (EventMessageType.WindowMessage == Service.EventMessageType)
			{
				AddID(this, ref _internId);
			}
			propReturnValue = Initialize(ref hPvi, propTimeout, propRetryTime, pInitParam, new IntPtr(0));
			if (Server != null && Server.Length > 0)
			{
				propReturnValue = XLinkRequest(Service.hPvi, "Pvi", 99u, "", 98u);
			}
			if (propReturnValue == 0)
			{
				if (EventMessageType == EventMessageType.CallBack)
				{
					propReturnValue = SetGlobEventMsg(hPvi, 240u, 4294967294u, 101u);
					propReturnValue = SetGlobEventMsg(hPvi, 241u, 4294967294u, 102u);
					propReturnValue = SetGlobEventMsg(hPvi, 242u, 4294967294u, 103u);
				}
				else
				{
					propReturnValue = SetGlobEventMsg(hPvi, 240u, 101u, 101u);
					propReturnValue = SetGlobEventMsg(hPvi, 241u, 102u, 102u);
					propReturnValue = SetGlobEventMsg(hPvi, 242u, 103u, 103u);
				}
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ServiceConnect, this));
			}
		}

		public int GetPVIVersionInfo(ref Hashtable versionInfos)
		{
			int num = 0;
			int num2 = 4096;
			IntPtr intPtr = PviMarshal.AllocCoTaskMem(num2);
			versionInfos.Clear();
			PInvokePvicom.PviComGetVersionInfo(intPtr, num2);
			PviMarshal.GetVersionInfos(intPtr, num2, ref versionInfos, "§LOCAL§ ");
			if (PInvokePvicom.PviComRead(hPvi, base.LinkId, AccessTypes.Version, IntPtr.Zero, 0, intPtr, num2) == 0)
			{
				PviMarshal.GetVersionInfos(intPtr, num2, ref versionInfos);
			}
			num = PInvokePvicom.PviComRead(hPvi, base.LinkId, AccessTypes.PVIVersion, IntPtr.Zero, 0, intPtr, num2);
			if (num == 0)
			{
				PviMarshal.GetVersionInfos(intPtr, num2, ref versionInfos, "§PVI Setup§");
			}
			return num;
		}

		private void PviDisconnect(uint pviHandle)
		{
			if (propMSGWindow != null && propMSGWindow.InvokeRequired)
			{
				SetPviDisconnectCallback method = PviDisconnect;
				propMSGWindow.Invoke(method, pviHandle);
			}
			else if (PInvokePvicom.PostMessage(WindowHandle, Base.MakeWindowMessage(Action.ServiceDisconnectPOSTMSG), (int)pviHandle, (int)_internId))
			{
				propReturnValue = 0;
			}
			else
			{
				DisconnectEx(hPvi);
			}
		}

		public override void Disconnect()
		{
			if (ConnectionStates.Disconnecting == propConnectionState)
			{
				return;
			}
			if (propConnectionState == ConnectionStates.Unininitialized)
			{
				PviDisconnect(hPvi);
				return;
			}
			propConnectionState = ConnectionStates.Disconnecting;
			if (ConnectionStates.Disconnected == propConnectionState || Cpus == null || ConnectionStates.Disconnected == Cpus.propConnectionState)
			{
				OnDisconnected(new PviEventArgs(base.Name, base.Address, 0, Language, Action.ServiceDisconnect, this));
				return;
			}
			ConnectionStates propConnectionState = propConnectionState;
			propConnectionState = ConnectionStates.Disconnecting;
			PviDisconnect(hPvi);
		}

		public override void Disconnect(bool noResponse)
		{
			if (ConnectionStates.Connected >= propConnectionState)
			{
				propNoDisconnectedEvent = noResponse;
				Disconnect();
			}
		}

		internal int DisconnectEx(uint pviHandle)
		{
			propReturnValue = Deinitialize(pviHandle);
			hPvi = 0u;
			OnDisconnected(new PviEventArgs(base.Name, base.Address, propReturnValue, Language, Action.ServiceDisconnect, this));
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Language, Action.ServiceDisconnect, this));
			}
			return propReturnValue;
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			if (InternIDs != null)
			{
				foreach (PviCBEvents value in InternIDs.Values)
				{
					if (value != null && value is Base)
					{
						((Base)value).UpdateServiceCreateState();
					}
				}
			}
			base.OnDisconnected(e);
		}

		protected override void OnConnected(PviEventArgs e)
		{
			if (e.ErrorCode == 0 && Action.ServiceArrange == e.Action)
			{
				RefreshPviClientsList();
				if (0 < propCpus.Count)
				{
					lock (propCpus.SyncRoot)
					{
						foreach (Cpu value in propCpus.Values)
						{
							if (Actions.Connect == (value.Requests & Actions.Connect))
							{
								value.Connect();
							}
							else if (value.reCreateActive)
							{
								value.reCreateState();
							}
						}
					}
				}
				e.propAction = Action.ConnectedEvent;
			}
			if (!propConnectionInterupted || !IsConnected)
			{
				base.OnConnected(e);
			}
		}

		public int AttachPVIObjects()
		{
			if (propServicePviObj != null)
			{
				propServicePviObj.Deinit();
				propServicePviObj = null;
			}
			propServicePviObj = new PviObjectBrowser("@Pvi", useIDName: false, this, null);
			propPendingObjectBrowserEvents = 0u;
			base.ConnectionType = ConnectionType.Link;
			return propServicePviObj.Link();
		}

		protected internal void OnPVIObjectsAttached(PviEventArgs e)
		{
			propPendingObjectBrowserEvents--;
			if (propPendingObjectBrowserEvents == 0 && this.PVIObjectsAttached != null)
			{
				if (e.propErrorCode != 0)
				{
					OnError(e);
				}
				this.PVIObjectsAttached(this, e);
			}
		}

		private int CreatePviLinkObject()
		{
			int result = 0;
			string text = "@Pvi";
			string text2 = "";
			if (propLinkId == 0)
			{
				result = ((Service.EventMessageType != 0) ? XMLink(hPvi, out propLinkId, text, 6u, 6u, text2) : PInvokePvicom.PviComLink(hPvi, out propLinkId, text, callback, 6u, 6u, text2));
			}
			return result;
		}

		public int RefreshPviClientsList()
		{
			int num = 0;
			int num2 = 20;
			propClientNames.Clear();
			IntPtr zero = IntPtr.Zero;
			IntPtr hMemory = PviMarshal.AllocHGlobal(num2);
			num = CreatePviLinkObject();
			if (num == 0)
			{
				num = PInvokePvicom.PviComRead(hPvi, propLinkId, AccessTypes.Clients, zero, 0, hMemory, num2);
				while (num == 0 && Marshal.ReadByte(hMemory, num2 - 1) != 0)
				{
					PviMarshal.FreeHGlobal(ref hMemory);
					num2 += 100;
					hMemory = PviMarshal.AllocHGlobal(num2);
					num = PInvokePvicom.PviComRead(hPvi, propLinkId, AccessTypes.Clients, zero, 0, hMemory, num2);
				}
				if (num == 0)
				{
					byte[] array = new byte[num2];
					Marshal.Copy(hMemory, array, 0, num2);
					string @string = Encoding.ASCII.GetString(array);
					string text = @string.Substring(0, @string.IndexOf('\0'));
					string[] array2 = text.Split('\t');
					for (int i = 0; i < array2.Length; i++)
					{
						propClientNames.Add(array2.GetValue(i).ToString());
					}
				}
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		public int UpdateLicenceInfo()
		{
			int num = 0;
			num = CreatePviLinkObject();
			if (num == 0)
			{
				num = ((Service.EventMessageType != 0) ? PInvokePvicom.PviComMsgReadRequest(hPvi, propLinkId, AccessTypes.License, WindowHandle, 104u, _internId) : PInvokePvicom.PviComReadRequest(hPvi, propLinkId, AccessTypes.License, callback, 4294967294u, 104u));
			}
			return num;
		}

		protected virtual void OnLicencInfoUpdated(LicenceInfo licInfo, int error)
		{
			if (this.LicencInfoUpdated != null)
			{
				this.LicencInfoUpdated(this, new PviEventArgs(base.Name, base.Address, error, propLanguage, Action.LicenceInfoUpdate, this));
			}
		}

		internal bool PVICB_Create(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			((PviCBEvents)InternIDs[(uint)lParam])?.OnPviCreated(info.Error, info.LinkId);
			return true;
		}

		internal bool PVICB_Link(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(0, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_LinkA(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(1, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_LinkB(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(2, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_LinkC(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(3, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_LinkD(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(4, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_LinkE(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_LinkResponse(5, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_Event(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_Event(0, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_EventA(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_Event(1, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_Read(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(0, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadA(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(1, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadB(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(2, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadC(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(3, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadD(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(4, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadE(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(5, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadF(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(6, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadG(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(7, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadS(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(19, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadT(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(20, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_ReadU(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_ReadResponse(21, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_Write(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(0, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteA(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(1, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteB(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(2, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteC(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(3, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteD(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(4, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteE(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(5, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteF(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(6, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_WriteU(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_WriteResponse(21, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_Unlink(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_UnlinkResponse(0, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_UnlinkA(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_UnlinkResponse(1, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_UnlinkB(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_UnlinkResponse(2, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_UnlinkC(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_UnlinkResponse(3, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_UnlinkD(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return PVI_UnlinkResponse(4, wParam, lParam, pData, dataLen, ref info);
		}

		internal bool PVICB_Delete(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			Base @base = null;
			((Base)InternIDs[(uint)lParam])?.OnPviDeleted(info.Error);
			return true;
		}

		internal bool PVICB_ChangeLink(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			return true;
		}

		internal bool PVICB_SNMP(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			pviCBEvents = (PviCBEvents)InternIDs[(uint)lParam];
			if (pviCBEvents != null)
			{
				switch (info.Mode)
				{
				}
			}
			return true;
		}

		[CLSCompliant(false)]
		protected internal bool Callback(int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			AccessModes mode = (AccessModes)info.Mode;
			AccessTypes type = (AccessTypes)info.Type;
			propErrorCode = info.Error;
			if (propDisposed)
			{
				return true;
			}
			if (info.LinkId != 0)
			{
				propLinkId = info.LinkId;
			}
			int error = info.Error;
			switch (mode)
			{
			case AccessModes.Event:
				switch (info.Type)
				{
				case 3:
					OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ErrorEvent, this));
					return true;
				case 240:
					OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
					if (info.Error != 0)
					{
						OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						propConnectionInterupted = true;
					}
					return true;
				case 241:
					propLinkId = 0u;
					if (IsConnected)
					{
						OnDisconnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceDisconnect, this));
						propConnectionInterupted = true;
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						propConnectionInterupted = true;
					}
					CallDisconnectedForChildObjects(info.Error, Language, Action.ServiceDisconnect);
					return true;
				case 242:
					if (!propConnectionInterupted)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceArrange, this));
					}
					else
					{
						if (hPvi == 0)
						{
							Connect();
						}
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceArrange, this));
						propConnectionInterupted = false;
						if (0 < Cpus.Count)
						{
							foreach (object value in Cpus.Values)
							{
								((Cpu)value).reCreateState();
							}
						}
					}
					propConnectionState = ConnectionStates.Connected;
					return true;
				}
				break;
			case AccessModes.Link:
				ReadRequest(Service.hPvi, info.LinkId, AccessTypes.Version, 110u);
				break;
			case AccessModes.Read:
				switch (type)
				{
				case AccessTypes.Version:
					if (info.Error == 0 && 0 < dataLen)
					{
						string text = "";
						text = PviMarshal.ToAnsiString(pData, dataLen);
						if (text.IndexOf("\r\n") != -1)
						{
							text = text.Substring(text.IndexOf("\r\n") - 4, 4);
						}
						else
						{
							text = text.Substring(text.IndexOf("\n") - 4, 4);
						}
					}
					break;
				case AccessTypes.License:
					propLicenseInfo.UpdateRuntimeState(pData);
					OnLicencInfoUpdated(propLicenseInfo, info.Error);
					break;
				}
				break;
			}
			return false;
		}

		private void CallDisconnectedForChildObjects(int errorCode, string languageName, Action actionCode)
		{
			if (InternIDs != null)
			{
				ArrayList arrayList = new ArrayList();
				foreach (PviCBEvents value in InternIDs.Values)
				{
					if (value is Cpu || value is Module || value is Variable || value is IODataPoint)
					{
						arrayList.Add(value);
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					((PviCBEvents)arrayList[i]).OnPviEvent(errorCode, EventTypes.Error, PVIDataStates.InheratedError, IntPtr.Zero, 0u, 0);
				}
			}
		}

		protected internal override void OnError(PviEventArgs e)
		{
			base.OnError(this, e);
		}

		protected internal override void OnError(object sender, PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				base.OnError(sender, e);
			}
		}

		public int SaveConfiguration(string fileName, ConfigurationFlags flags)
		{
			int result = 0;
			FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteRaw("<?PviServices Version=1.0?>");
			writer.WriteStartElement("Services");
			ToXMLTextWriter(ref writer, flags);
			writer.WriteEndElement();
			writer.Close();
			fileStream.Close();
			return result;
		}

		public virtual int SaveConfiguration(string fileName)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return SaveConfiguration(fileName, configurationFlags);
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			int num2 = 0;
			writer.WriteStartElement("Service");
			num = base.ToXMLTextWriter(ref writer, flags);
			if (propTimeout != 0)
			{
				writer.WriteAttributeString("Timeout", propTimeout.ToString());
			}
			if (1 != propPVIAutoStart)
			{
				writer.WriteAttributeString("PviAutoStart", propPVIAutoStart.ToString());
			}
			if (0 < propProcessTimeout)
			{
				writer.WriteAttributeString("ProcessTimeout", propProcessTimeout.ToString());
			}
			if (1 != propMessageLimitation)
			{
				writer.WriteAttributeString("MessageLimitation", propMessageLimitation.ToString());
			}
			if (0 < propRetryTime)
			{
				writer.WriteAttributeString("RetryTime", propRetryTime.ToString());
			}
			if (!propWaitForParentConnection)
			{
				writer.WriteAttributeString("WaitForParent", propWaitForParentConnection.ToString());
			}
			if (!(propLanguage == "en-us"))
			{
				writer.WriteAttributeString("Language", Language);
			}
			if (Port != 0)
			{
				writer.WriteAttributeString("Port", Port.ToString());
			}
			if (propServer != null && propServer.Length > 0)
			{
				writer.WriteAttributeString("Server", propServer.ToString());
			}
			writer.WriteAttributeString("IsStatic", IsStatic.ToString());
			switch (propLogicalUsage)
			{
			case LogicalObjectsUsage.None:
				writer.WriteAttributeString("LogicalUsage", "None");
				break;
			case LogicalObjectsUsage.FullName:
				writer.WriteAttributeString("LogicalUsage", "FullName");
				break;
			case LogicalObjectsUsage.ObjectName:
				writer.WriteAttributeString("LogicalUsage", "ObjectName");
				break;
			case LogicalObjectsUsage.ObjectNameWithType:
				writer.WriteAttributeString("LogicalUsage", "ObjectNameWithType");
				break;
			}
			if (propExtendedTypeInfoAutoUpdateForVariables)
			{
				writer.WriteAttributeString("ExtendedTypeInfoAutoUpdateForVariables", propExtendedTypeInfoAutoUpdateForVariables.ToString());
			}
			num2 = Variables.ToXMLTextWriter(ref writer, flags);
			if (num2 != 0)
			{
				num = num2;
			}
			if (0 < Cpus.Count)
			{
				foreach (Cpu value in Cpus.Values)
				{
					num2 = value.ToXMLTextWriter(ref writer, flags);
					if (num2 != 0)
					{
						num = num2;
					}
				}
			}
			writer.WriteEndElement();
			return num;
		}

		internal int AddLogicalObject(string logical, object obj)
		{
			int result = 0;
			if (propServices == null)
			{
				if (propLogicalObjects == null)
				{
					propLogicalObjects = new LogicalCollection(this, base.Name + ".Logicals");
				}
				if (propLogicalObjects.ContainsKey(logical))
				{
					return -1;
				}
				propLogicalObjects.Add(logical, obj);
				if (obj is Base)
				{
					((Base)obj).propLogicalName = logical;
				}
				return result;
			}
			if (propServices.ContainsLogicalObject(logical))
			{
				return -1;
			}
			if (obj is Base)
			{
				((Base)obj).propLogicalName = logical;
			}
			return propServices.AddLogicalObject(logical, obj);
		}

		internal void RemoveLogicalObject(string logical)
		{
			if (propServices == null && propLogicalObjects != null)
			{
				propLogicalObjects.Remove(logical);
			}
			else
			{
				propServices.RemoveLogicalObject(logical);
			}
		}

		private int FromXmlTextReader(XmlTextReader reader, object parent, ConfigurationFlags flags)
		{
			string text = "";
			bool flag = false;
			int result = 0;
			if (reader == null)
			{
				return -1;
			}
			if (reader.Name != "Service")
			{
				reader.MoveToContent();
				reader.Read();
			}
			while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "Services")
			{
				if (string.Compare(reader.Name, "Service") == 0)
				{
					base.FromXmlTextReader(ref reader, flags, this);
					text = "";
					text = reader.GetAttribute("Connected");
					if (text != null && text.Length > 0 && text.ToLower() == "true")
					{
						flag = true;
					}
					string attribute = reader.GetAttribute("IsStatic");
					if (attribute.Equals("true"))
					{
						isStatic = true;
					}
					else
					{
						isStatic = false;
					}
					text = "";
					text = reader.GetAttribute("Timeout");
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propTimeout);
					}
					text = "";
					text = reader.GetAttribute("PviAutoStart");
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propPVIAutoStart);
					}
					text = "";
					text = reader.GetAttribute("ProcessTimeout");
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propProcessTimeout);
					}
					text = "";
					text = reader.GetAttribute("MessageLimitation");
					propMessageLimitation = 1;
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propMessageLimitation);
					}
					text = "";
					text = reader.GetAttribute("RetryTime");
					propRetryTime = 0;
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propRetryTime);
					}
					text = "";
					text = reader.GetAttribute("WaitForParent");
					if (text != null && text.ToLower().CompareTo("false") == 0)
					{
						propWaitForParentConnection = false;
					}
					text = "";
					text = reader.GetAttribute("Language");
					if (text != null && text.Length > 0)
					{
						Language = text;
					}
					text = "";
					text = reader.GetAttribute("Server");
					if (text != null && text.Length > 0)
					{
						Server = text;
					}
					text = "";
					text = reader.GetAttribute("Port");
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseInt32(text, out propPort);
					}
					text = "";
					text = reader.GetAttribute("LogicalUsage");
					if (text != null && text.Length > 0)
					{
						switch (text)
						{
						case "None":
							propLogicalUsage = LogicalObjectsUsage.None;
							break;
						case "FullName":
							propLogicalUsage = LogicalObjectsUsage.FullName;
							break;
						case "ObjectName":
							propLogicalUsage = LogicalObjectsUsage.ObjectName;
							break;
						case "ObjectNameWithType":
							propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
							break;
						}
					}
					text = "";
					text = reader.GetAttribute("ExtendedTypeInfoAutoUpdateForVariables");
					propExtendedTypeInfoAutoUpdateForVariables = false;
					if (text != null && 0 < text.Length && "true" == text.ToLower())
					{
						propExtendedTypeInfoAutoUpdateForVariables = true;
					}
					reader.Read();
				}
				else if (reader.Name.ToLower().CompareTo("variablecollection") == 0)
				{
					Variables.FromXmlTextReader(ref reader, flags, this);
				}
				else if (reader.Name.ToLower().CompareTo("variable") == 0)
				{
					Variables.FromXmlTextReader(ref reader, flags, this);
				}
				else if (reader.Name.ToLower().CompareTo("cpu") == 0)
				{
					string attribute2 = reader.GetAttribute("Name");
					if (attribute2 != null && attribute2.Length > 0)
					{
						propCpu = new Cpu(this, attribute2);
						propCpu.FromXmlTextReader(ref reader, flags, propCpu);
					}
					if (reader.Name.ToLower().CompareTo("service") == 0 && reader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
				}
				else
				{
					reader.Read();
				}
			}
			if (flag)
			{
				if (propServer != null && propServer.Length > 0 && propPort > 0)
				{
					Connect(propServer, propPort);
				}
				Connect();
			}
			return result;
		}

		public virtual int LoadConfiguration(string fileName)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return LoadConfiguration(fileName, configurationFlags);
		}

		public virtual int LoadConfiguration(StreamReader stream)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return LoadConfiguration(stream, configurationFlags);
		}

		protected internal int LoadConfiguration(XmlTextReader reader, ConfigurationFlags flags)
		{
			return FromXmlTextReader(reader, this, flags);
		}

		protected virtual int LoadConfiguration(string fileName, ConfigurationFlags flags)
		{
			int num = 0;
			if (File.Exists(fileName))
			{
				XmlTextReader xmlTextReader = new XmlTextReader(fileName);
				xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
				num = LoadConfiguration(xmlTextReader, flags);
				xmlTextReader.Close();
				return num;
			}
			return -1;
		}

		protected virtual int LoadConfiguration(StreamReader stream, ConfigurationFlags flags)
		{
			int num = 0;
			if (stream != null)
			{
				XmlTextReader xmlTextReader = new XmlTextReader(stream);
				xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
				return LoadConfiguration(xmlTextReader, flags);
			}
			return -1;
		}

		public override void Remove()
		{
			if (propSNMP != null)
			{
				propSNMP.Cleanup();
			}
			base.Remove();
			if (Services != null && base.Name != null)
			{
				Services.Remove(base.Name);
			}
			if (base.Name != null)
			{
				ServiceCollection.Services.Remove(base.Name);
			}
		}

		public void RemoveArchive(string path)
		{
			LoggerCollection obj = null;
			foreach (LoggerCollection loggerCollection in LoggerCollections)
			{
				if (loggerCollection.Name.Equals(path))
				{
					obj = loggerCollection;
					break;
				}
			}
			LoggerCollections.Remove(obj);
		}

		internal static string GetErrorText(int error, string language)
		{
			uint num = (uint)error;
			string text = "";
			if (error == 0)
			{
				return "";
			}
			if (language != "")
			{
				try
				{
					ResourceManager resourceManager = (language.ToLower().IndexOf("de") == 0) ? new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly()) : ((language.ToLower().IndexOf("fr") != 0) ? new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly()));
					text = ((num < int.MaxValue) ? resourceManager.GetString($"{num:0000}") : resourceManager.GetString(num.ToString()));
					if (text != null)
					{
						return text;
					}
					resourceManager = null;
					resourceManager = ((language.ToLower().IndexOf("de") == 0) ? new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly()) : ((language.ToLower().IndexOf("fr") != 0) ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly())));
					if (num < int.MaxValue)
					{
						text = resourceManager.GetString($"{num:0000}");
						return text;
					}
					text = resourceManager.GetString(num.ToString());
					return text;
				}
				catch (System.Exception)
				{
					return text;
				}
			}
			return text;
		}

		internal static string GetErrorTextEx(string errorNumber, string language)
		{
			string result = "";
			if (errorNumber == null || errorNumber.Length == 0 || errorNumber == "0")
			{
				return "";
			}
			if (language != "")
			{
				try
				{
					ResourceManager resourceManager = (language.ToLower().IndexOf("de") == 0) ? new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly()) : ((language.ToLower().IndexOf("fr") != 0) ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly()));
					result = resourceManager.GetString(errorNumber);
					return result;
				}
				catch (System.Exception)
				{
					return result;
				}
			}
			return result;
		}

		internal static string GetErrorTextPCC(int error, string language)
		{
			return GetErrorTextEx(error.ToString(), language);
		}

		internal static string GetErrorTextPCC(uint error, string language)
		{
			return GetErrorTextEx($"{error:0000}", language);
		}

		internal void cpyDblToBuffer(object value)
		{
			Marshal.Copy(new double[1]
			{
				Convert.ToDouble(value)
			}, 0, iptr8Byte, 1);
			Marshal.Copy(iptr8Byte, byteBuffer, 0, 8);
		}

		internal void cpyFltToBuffer(object value)
		{
			Marshal.Copy(new float[1]
			{
				Convert.ToSingle(value)
			}, 0, iptr4Byte, 1);
			Marshal.Copy(iptr4Byte, byteBuffer, 0, 4);
		}

		internal short toInt16(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr2Byte, 2);
			return Marshal.ReadInt16(iptr2Byte);
		}

		internal ushort toUInt16(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr2Byte, 2);
			return (ushort)Marshal.ReadInt16(iptr2Byte);
		}

		internal int toInt32(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr4Byte, 4);
			return Marshal.ReadInt32(iptr4Byte);
		}

		internal uint toUInt32(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr4Byte, 4);
			return (uint)Marshal.ReadInt32(iptr4Byte);
		}

		internal long toInt64(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr8Byte, 8);
			return PviMarshal.ReadInt64(iptr8Byte);
		}

		internal ulong toUInt64(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr8Byte, 8);
			return (ulong)PviMarshal.ReadInt64(iptr8Byte);
		}

		internal float toSingle(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr4Byte, 4);
			Marshal.Copy(iptr4Byte, marshF32, 0, 1);
			return marshF32[0];
		}

		internal double toDouble(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr8Byte, 8);
			Marshal.Copy(iptr8Byte, marshF64, 0, 1);
			return marshF64[0];
		}

		internal TimeSpan toTimeSpan(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr4Byte, 4);
			uint64Val = Marshal.ReadInt32(iptr4Byte);
			return new TimeSpan(10000 * uint64Val);
		}

		internal DateTime toDateTime(byte[] bBuffer, int byteOffset)
		{
			Marshal.Copy(bBuffer, byteOffset, iptr4Byte, 4);
			return Pvi.UInt32ToDateTime((uint)Marshal.ReadInt32(iptr4Byte));
		}

		internal string toString(byte[] bBuffer, int byteOffset, int strLen)
		{
			string text = null;
			for (int i = byteOffset; i < byteOffset + strLen; i++)
			{
				byte b = (byte)bBuffer.GetValue(i);
				if (b == 0)
				{
					break;
				}
				text += (char)b;
				if (i + 1 > bBuffer.GetLength(0))
				{
					break;
				}
			}
			return text;
		}

		internal bool PVI_UnlinkResponse(int fnNumber, int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			Base @base = null;
			@base = (Base)InternIDs[(uint)lParam];
			if (@base != null)
			{
				if (info.Type == 128)
				{
					@base.OnPviCancelled(info.Error, 8);
				}
				else
				{
					@base.OnPviUnLinked(info.Error, fnNumber);
				}
			}
			return true;
		}

		internal bool PVI_Event(int fnNumber, int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			((PviCBEvents)InternIDs[(uint)lParam])?.OnPviEvent(info.Error, (EventTypes)info.Type, (PVIDataStates)info.Status, pData, dataLen, fnNumber);
			return true;
		}

		internal bool PVI_LinkResponse(int fnNumber, int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			pviCBEvents = (PviCBEvents)InternIDs[(uint)lParam];
			if (pviCBEvents != null)
			{
				if (info.Type == 128)
				{
					pviCBEvents.OnPviCancelled(info.Error, 7);
				}
				else
				{
					pviCBEvents.OnPviLinked(info.Error, info.LinkId, fnNumber);
				}
			}
			return true;
		}

		internal bool PVI_ReadResponse(int fnNumber, int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			pviCBEvents = (PviCBEvents)InternIDs[(uint)lParam];
			if (pviCBEvents != null)
			{
				if (info.Type == 128)
				{
					pviCBEvents.OnPviCancelled(info.Error, 9);
				}
				else
				{
					pviCBEvents.OnPviRead(info.Error, (PVIReadAccessTypes)info.Type, (PVIDataStates)info.Status, pData, dataLen, fnNumber);
				}
			}
			return true;
		}

		internal bool PVI_WriteResponse(int fnNumber, int wParam, int lParam, IntPtr pData, uint dataLen, ref ResponseInfo info)
		{
			PviCBEvents pviCBEvents = null;
			pviCBEvents = (PviCBEvents)InternIDs[(uint)lParam];
			if (pviCBEvents != null)
			{
				if (info.Type == 128)
				{
					pviCBEvents.OnPviCancelled(info.Error, 10);
				}
				else
				{
					pviCBEvents.OnPviWritten(info.Error, (PVIWriteAccessTypes)info.Type, (PVIDataStates)info.Status, fnNumber, pData, dataLen);
				}
			}
			return true;
		}

		internal static bool IsRemoteError(int errorCode)
		{
			if (12040 == errorCode || 12059 == errorCode || 12094 == errorCode || 12095 == errorCode)
			{
				return true;
			}
			return false;
		}

		[CLSCompliant(false)]
		protected internal bool PviGlobalEvents(int wParam, int lParam, ref ResponseInfo info, uint dataLen)
		{
			AccessModes mode = (AccessModes)info.Mode;
			AccessTypes type = (AccessTypes)info.Type;
			propErrorCode = info.Error;
			PInvokePvicom.PviComReadResponse(hPvi, (IntPtr)wParam, IntPtr.Zero, 0);
			if (propDisposed)
			{
				return true;
			}
			if (info.LinkId != 0)
			{
				propLinkId = info.LinkId;
			}
			int error = info.Error;
			switch (mode)
			{
			case AccessModes.Event:
				switch (info.Type)
				{
				case 3:
					OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ErrorEvent, this));
					return true;
				case 240:
					OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
					if (info.Error != 0)
					{
						OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						propConnectionInterupted = true;
					}
					return true;
				case 241:
					propLinkId = 0u;
					if (IsConnected)
					{
						OnDisconnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceDisconnect, this));
						propConnectionInterupted = true;
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						OnError(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceConnect, this));
						propConnectionInterupted = true;
					}
					CallDisconnectedForChildObjects(info.Error, Language, Action.ServiceDisconnect);
					return true;
				case 242:
					if (!propConnectionInterupted)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceArrange, this));
					}
					else
					{
						if (hPvi == 0)
						{
							Connect();
						}
						OnConnected(new PviEventArgs(base.Name, base.Address, info.Error, Language, Action.ServiceArrange, this));
						propConnectionInterupted = false;
						if (0 < Cpus.Count)
						{
							foreach (object value in Cpus.Values)
							{
								((Cpu)value).reCreateState();
							}
						}
					}
					propConnectionState = ConnectionStates.Connected;
					return true;
				}
				break;
			case AccessModes.Link:
				ReadRequest(Service.hPvi, info.LinkId, AccessTypes.Version, 110u);
				break;
			case AccessModes.Read:
				switch (type)
				{
				case AccessTypes.Version:
					if (info.Error == 0 && 0 >= dataLen)
					{
					}
					break;
				case AccessTypes.License:
					OnLicencInfoUpdated(propLicenseInfo, info.Error);
					break;
				}
				break;
			}
			return false;
		}
	}
}
