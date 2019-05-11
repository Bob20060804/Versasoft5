using System;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Logger : Module
	{
		private const int INVALID_LOG_ID = -2;

		public const string KW_SYSLOGBOOK_NAME = "$arlogsys";

		public const string KW_USRLOGBOOK_NAME = "$arlogusr";

		private bool eIndexValid;

		private bool waitOnCancel;

		private LoggerEntry lastLoggerEntry;

		internal uint propContentVersion;

		internal uint _Persistence;

		private bool readStatusInProgress;

		private bool isClean;

		protected bool propReadRequestActive;

		private bool propIsArchive;

		private uint _entryIDInc;

		private uint _entryIDDec;

		internal static int DEFAULT_READ_BLOCKSIZE = 20;

		private uint propRequestContentVersion;

		private LoggerEntryCollection propLoggerEntries;

		private bool propContinuousActive;

		private bool propGlobalMerge;

		internal LoggerCollection propParentCollection;

		internal long readStartIndex;

		internal long entriesToRead;

		internal long entriesRead;

		private long logActID;

		private long oldLogID;

		private long readActID;

		private int readSize;

		private int readBlockSize;

		private bool isSGXDetectionLogger
		{
			get
			{
				if (propName.CompareTo("$Detect_SG4_SysLogger$") != 0)
				{
					return false;
				}
				return true;
			}
		}

		[CLSCompliant(false)]
		public uint ContentVersion
		{
			get
			{
				return propContentVersion;
			}
		}

		public LoggerEntryCollection LoggerEntries => propLoggerEntries;

		public bool ReadRequestActive => propReadRequestActive;

		public bool IsArchive => propIsArchive;

		public bool ContinuousActive
		{
			get
			{
				return propContinuousActive;
			}
			set
			{
				if (propContinuousActive != value)
				{
					propContinuousActive = value;
					if (IsConnected)
					{
						SetContinuousActive(propContinuousActive);
					}
					else if (Actions.SetActive != (base.Requests & Actions.SetActive))
					{
						base.Requests |= Actions.SetActive;
					}
				}
			}
		}

		public DaylightSaving DaylightSaving => LoggerEntries.DaylightSaving;

		public short OffsetUtc => LoggerEntries._OffsetUtc;

		public bool GlobalMerge
		{
			get
			{
				return propGlobalMerge;
			}
			set
			{
				if (propGlobalMerge != value)
				{
					if (value)
					{
						propGlobalMerge = value;
						AddEntriesToServiceCollection(LoggerEntries);
					}
					else if (Service.LoggerEntries != null)
					{
						Service.LoggerEntries.RemoveCollection(LoggerEntries);
						OnGlobalRemoved(new LoggerEventArgs(base.Name, base.Address, 0, Service.Language, Action.LoggerGlobalRemoved, LoggerEntries));
						propGlobalMerge = value;
					}
				}
			}
		}

		public LoggerCollection ParentCollection => propParentCollection;

		internal bool HasEventLogEntries
		{
			get;
			private set;
		}

		public int ReadBlockSize
		{
			get
			{
				return readBlockSize;
			}
			set
			{
				readBlockSize = value;
			}
		}

		public event PviEventHandlerXmlData LoggerInfoRead;

		public event PviEventHandler ContinuousActivated;

		public event PviEventHandler ContinuousDeactivated;

		public event LoggerEventHandler EntriesRead;

		public event LoggerEventHandler EntryBlockRead;

		public event PviEventHandler Cleared;

		public event LoggerEventHandler EntryAdded;

		public event LoggerEventHandler EntriesRemoved;

		public event LoggerEventHandler GlobalAdded;

		public event LoggerEventHandler GlobalRemoved;

		internal Logger(Cpu cpu, string name, bool doNotAddToCollections)
			: base(cpu, name, doNotAddToCollections)
		{
			propLoggerEntries = null;
			CleanInit();
			lastLoggerEntry = null;
		}

		internal Logger(string name)
			: base(null, name)
		{
			lastLoggerEntry = null;
			CleanInit();
		}

		public Logger(Cpu cpu, string name)
			: base(cpu, name)
		{
			if (cpu != null)
			{
				Logger logger = cpu.Loggers[name];
				if (logger != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Loggers\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Loggers\".", name);
				}
			}
			lastLoggerEntry = null;
			propLoggerEntries = null;
			CleanInit();
			if (cpu != null)
			{
				propCpu.Loggers.Add(this);
			}
		}

		~Logger()
		{
			Dispose(disposing: false, removeFromCollection: true);
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
			Service service = Service;
			base.Dispose(disposing, removeFromCollection);
			if (disposing)
			{
				propService = service;
				base.propParent = propParent;
				base.propLinkName = propLinkName;
				base.propLogicalName = propLogicalName;
				base.propUserData = propUserData;
				base.propName = propName;
				base.propAddress = propAddress;
				if (removeFromCollection)
				{
					RemoveObject();
				}
				if (propLoggerEntries != null)
				{
					propLoggerEntries.Dispose(disposing, removeFromCollection);
					propLoggerEntries = null;
				}
				if (propUserCollections != null)
				{
					propUserCollections.Clear();
					propUserCollections = null;
				}
				propParentCollection = null;
				base.propParent = null;
				base.propLinkName = null;
				base.propLogicalName = null;
				base.propUserData = null;
				propService = null;
			}
		}

		private void ResetReadIDs()
		{
			logActID = 0L;
			readSize = 0;
			readActID = -1L;
			readStartIndex = 0L;
			entriesToRead = 0L;
			entriesRead = 0L;
			oldLogID = -1L;
		}

		internal Logger(Service service, string name)
			: base(service, name)
		{
			CleanInit();
			propConnectionState = ConnectionStates.Unininitialized;
			propIsArchive = false;
			lastLoggerEntry = new LoggerEntry();
			isClean = false;
			eIndexValid = false;
			readBlockSize = DEFAULT_READ_BLOCKSIZE;
			waitOnCancel = false;
			ResetReadIDs();
			propReadRequestActive = false;
			propLoggerEntries = new LoggerEntryCollection(this, "LoggerEntries");
			propGlobalMerge = false;
		}

		internal Logger(Service service, string name, bool isArchive)
			: base(service, name)
		{
			CleanInit();
			propConnectionState = ConnectionStates.Unininitialized;
			propIsArchive = isArchive;
			lastLoggerEntry = new LoggerEntry();
			isClean = false;
			eIndexValid = false;
			readBlockSize = DEFAULT_READ_BLOCKSIZE;
			waitOnCancel = false;
			ResetReadIDs();
			propReadRequestActive = false;
			propLoggerEntries = new LoggerEntryCollection(this, "LoggerEntries");
			propGlobalMerge = false;
		}

		internal override void RemoveObject()
		{
			LoggerEntries.Clear();
			Remove();
			if (base.Cpu != null)
			{
				if (base.Cpu.Modules != null)
				{
					base.Cpu.Modules.Remove(base.Name);
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

		private int ReadALL()
		{
			propReturnValue = 12011;
			CleanLoggerEntries(0);
			readActID = logActID;
			readStartIndex = logActID;
			entriesToRead = 1 + readStartIndex;
			entriesRead = 0L;
			oldLogID = -1L;
			readSize = SetReadSize(entriesToRead);
			if (!propReadRequestActive)
			{
				propReturnValue = Read(readSize, readStartIndex, Action.LoggerReadBlock);
			}
			return propReturnValue;
		}

		public int ReadLoggerInfo()
		{
			int num = 0;
			return ReadRequest(Service.hPvi, base.LinkId, AccessTypes.ANSL_LoggerModuleInfo, 4460u);
		}

		private int OnLoggerInfoRead(int errorCode, PVIReadAccessTypes accessType, IntPtr pData, uint dataLen)
		{
			int num = errorCode;
			string text = "";
			XmlReader xmlReader = null;
			propContentVersion = 0u;
			if (dataLen != 0 && IntPtr.Zero != pData)
			{
				try
				{
					text = PviMarshal.PtrToStringAnsi(pData, dataLen);
					xmlReader = XmlReader.Create(new StringReader(text));
					xmlReader.MoveToContent();
					if (xmlReader.Name.CompareTo("LoggerInfo") == 0)
					{
						string attribute = xmlReader.GetAttribute("Version");
						if (!string.IsNullOrEmpty(attribute))
						{
							propContentVersion = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("Persistence");
						if (!string.IsNullOrEmpty(attribute))
						{
							_Persistence = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("OffsetUtc");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._OffsetUtc = Convert.ToInt16(attribute);
						}
						LoggerEntries._DaylightSaving = 0;
						attribute = xmlReader.GetAttribute("DaylightSaving");
						if (!string.IsNullOrEmpty(attribute) && ("ON" == attribute.ToUpper() || "1" == attribute.ToUpper()))
						{
							LoggerEntries._DaylightSaving = 1;
						}
						attribute = xmlReader.GetAttribute("Attributes");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._LogAttributes = Convert.ToUInt16(attribute);
						}
						attribute = xmlReader.GetAttribute("ActIndex");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._ActIndex = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("ActOffset");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._ActOffset = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("InvalidLength");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._InvalidLength = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("LogDataLength");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._LogDataLength = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("ReferenceIndex");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._ReferenceIndex = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("ReferenceOffset");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._ReferenceOffset = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("Wrap");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._Wrap = Convert.ToUInt32(attribute);
						}
						attribute = xmlReader.GetAttribute("WriteOffset");
						if (!string.IsNullOrEmpty(attribute))
						{
							LoggerEntries._WriteOffset = Convert.ToUInt32(attribute);
						}
					}
				}
				catch
				{
					num = 12054;
				}
				finally
				{
					xmlReader?.Close();
				}
			}
			if (this.LoggerInfoRead != null)
			{
				this.LoggerInfoRead(this, new PviEventArgsXML(base.Name, base.Address, num, Service.Language, Action.LoggerINFORead, text));
			}
			return num;
		}

		[CLSCompliant(false)]
		public virtual void Read(uint contentVersion)
		{
			if (contentVersion == 0)
			{
				Read();
			}
			else
			{
				ReadEntries(contentVersion);
			}
		}

		public virtual void Read()
		{
			ReadEntries(0u);
		}

		private void ReadEntries(uint contentVersion)
		{
			propRequestContentVersion = contentVersion;
			propReturnValue = 0;
			if (propReadRequestActive)
			{
				propReturnValue = CancelRequests();
				if (waitOnCancel)
				{
					return;
				}
			}
			if (!propContinuousActive)
			{
				ReadIndex(Action.LoggerIndexForUpdate);
			}
			else
			{
				ReadIndex(Action.LoggerIndexForRead);
			}
		}

		[CLSCompliant(false)]
		public virtual void Read(int count, uint contentVersion)
		{
			ReadEntriesOldStyle(count, contentVersion);
		}

		public virtual void Read(int count)
		{
			ReadEntriesOldStyle(count, 0u);
		}

		private void ReadEntriesOldStyle(int count, uint contentVersion)
		{
			propReturnValue = 0;
			string text = "DN=" + count.ToString();
			propReadRequestActive = true;
			propRequestContentVersion = contentVersion;
			if (0 < propRequestContentVersion)
			{
				text = $"DN={count.ToString()} VI={propRequestContentVersion.ToString()}";
			}
			Service.BuildRequestBuffer(text);
			if (0 < propRequestContentVersion)
			{
				propReturnValue = ReadArgumentRequest(((Cpu)propParent).Service.hPvi, propLinkId, AccessTypes.ANSL_LoggerModuleData, Service.RequestBuffer, text.Length, 900u, base.InternId);
			}
			else
			{
				propReturnValue = ReadArgumentRequest(((Cpu)propParent).Service.hPvi, propLinkId, AccessTypes.ModuleData, Service.RequestBuffer, text.Length, 900u, base.InternId);
			}
		}

		internal virtual void ReadEntry(long id)
		{
			propReturnValue = 0;
			string text = "DN=1 ID=" + id.ToString();
			propReadRequestActive = true;
			Service.BuildRequestBuffer(text);
			propReturnValue = ReadArgumentRequest(((Cpu)propParent).Service.hPvi, propLinkId, AccessTypes.ModuleData, Service.RequestBuffer, text.Length, 901u, base.InternId);
		}

		internal virtual int Read(int count, long id, Action action)
		{
			int num = 0;
			string text = $"DN={count.ToString()} ID={id.ToString()}";
			propReadRequestActive = true;
			if (0 < propRequestContentVersion)
			{
				text = $"DN={count.ToString()} ID={id.ToString()} VI={propRequestContentVersion.ToString()}";
			}
			Service.BuildRequestBuffer(text);
			if (0 < propRequestContentVersion)
			{
				return ReadArgumentRequest(((Cpu)propParent).Service.hPvi, propLinkId, AccessTypes.ANSL_LoggerModuleData, Service.RequestBuffer, text.Length, (uint)action, base.InternId);
			}
			return ReadArgumentRequest(((Cpu)propParent).Service.hPvi, propLinkId, AccessTypes.ModuleData, Service.RequestBuffer, text.Length, (uint)action, base.InternId);
		}

		internal virtual void ReadIndexForUpdate(Action action)
		{
			readStatusInProgress = true;
			propReturnValue = ReadRequest(Service.hPvi, propLinkId, AccessTypes.Status, (uint)action);
		}

		internal virtual void ReadIndex(Action action)
		{
			if (readStatusInProgress)
			{
				if (action == Action.LoggerIndexForUpdate)
				{
					base.Requests |= Actions.ReadIndex;
				}
			}
			else
			{
				ReadIndexForUpdate(action);
			}
		}

		public override void Disconnect()
		{
			readStatusInProgress = false;
			propConnectionState = ConnectionStates.Disconnecting;
			base.Disconnect();
		}

		public override void Disconnect(bool noResponse)
		{
			readStatusInProgress = false;
			propConnectionState = ConnectionStates.Disconnecting;
			base.Disconnect(noResponse);
		}

		public override void Connect(ConnectionType connectionType)
		{
			if (!reCreateActive && base.LinkId == 0)
			{
				if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.LoggerConnect, Service));
				}
				else if (ConnectionStates.Unininitialized >= propConnectionState || ConnectionStates.Disconnecting <= propConnectionState)
				{
					propReturnValue = 0;
					base.ConnectionType = connectionType;
					Connect(forceConnection: false, connectionType, 909u);
				}
			}
		}

		internal int ConnectEx()
		{
			int result = 0;
			if (HasPVIConnection)
			{
				return ReadRequest(Service.hPvi, base.LinkId, AccessTypes.Status, 917u);
			}
			if (ConnectionStates.Connecting != propConnectionState)
			{
				propConnectionState = ConnectionStates.Connecting;
				if (propAddress == null || propAddress.Length == 0)
				{
					propAddress = propName;
				}
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					propObjectParam = "CD=" + $"\"{propAddress}\"";
				}
				else
				{
					propObjectParam = "CD=" + $"\"{propParent.LinkName}\"/\"{propAddress}\"";
				}
				propLinkParam = getLinkDescription();
				result = XCreateRequest(Service.hPvi, base.LinkName, ObjectType.POBJ_MODULE, propObjectParam, 916u, propLinkParam, 916u);
			}
			return result;
		}

		internal void CleanInit()
		{
			HasEventLogEntries = false;
			readStatusInProgress = false;
			_entryIDInc = 0u;
			_entryIDDec = uint.MaxValue;
			propConnectionState = ConnectionStates.Unininitialized;
			propIsArchive = false;
			lastLoggerEntry = null;
			lastLoggerEntry = new LoggerEntry();
			isClean = false;
			eIndexValid = false;
			readBlockSize = DEFAULT_READ_BLOCKSIZE;
			waitOnCancel = false;
			ResetReadIDs();
			propReadRequestActive = false;
			if (propLoggerEntries != null && Service != null)
			{
				if (propGlobalMerge)
				{
					if (Service.LoggerEntries != null && 0 < Service.LoggerEntries.Count && 0 < Service.LoggerEntries.Count)
					{
						foreach (LoggerEntry value in LoggerEntries.Values)
						{
							Service.LoggerEntries.Remove(value);
						}
					}
					OnGlobalRemoved(new LoggerEventArgs(base.Name, base.Address, 0, Service.Language, Action.LoggerGlobalRemoved, LoggerEntries));
				}
				propLoggerEntries.Clear();
				propLoggerEntries = null;
			}
			propLoggerEntries = new LoggerEntryCollection(this, "LoggerEntries");
		}

		public virtual void Clear()
		{
			propReturnValue = 0;
			string text = "LD=Clear";
			Service.BuildRequestBuffer(text);
			if (!readStatusInProgress)
			{
				propReturnValue = WriteRequest(base.Cpu.Service.hPvi, propLinkId, AccessTypes.Status, Service.RequestBuffer, text.Length, 905u);
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.LoggerClear, Service));
				}
			}
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propLinkId = linkID;
			propErrorCode = errorCode;
			if (errorCode == 0 || 12002 == errorCode)
			{
				if (Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					propErrorCode = XLinkRequest(Service.hPvi, base.LinkName, 909u, propLinkParam, 909u);
					return;
				}
				logActID = -2L;
				if (!isSGXDetectionLogger)
				{
					ResetReadIDs();
					ReadIndex(Action.LoggerIndexForConnect);
				}
			}
			else
			{
				OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerConnect, Service));
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerConnect, Service));
			}
		}

		private int SetReadSize(long totalEntries)
		{
			int num = 0;
			if (totalEntries > readBlockSize)
			{
				return readBlockSize;
			}
			return (int)totalEntries;
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			readStatusInProgress = false;
			base.OnPviLinked(errorCode, linkID, option);
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
			readStatusInProgress = false;
			base.OnPviUnLinked(errorCode, option);
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			switch (eventType)
			{
			case EventTypes.Status:
				if (readStatusInProgress)
				{
					break;
				}
				logActID = GetID(pData, dataLen);
				if (logActID < oldLogID)
				{
					ReadALL();
				}
				eIndexValid = true;
				if (errorCode == 0 && !propReadRequestActive)
				{
					if (readActID != logActID)
					{
						readStartIndex = logActID;
						entriesRead = 0L;
						entriesToRead = readStartIndex - readActID;
						readActID = readStartIndex;
						readSize = SetReadSize(entriesToRead);
						Read(readSize, readStartIndex, Action.LoggerReadBlock);
					}
					else
					{
						Read(1, logActID, Action.LoggerReadEntryForCompare);
					}
				}
				break;
			case EventTypes.Error:
				if (1 == option)
				{
					OnError(this, new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerDetectSGType, Service));
				}
				else if (errorCode != 0)
				{
					if (IsConnected)
					{
						if (4813 == errorCode || 4812 == errorCode)
						{
							Disconnect();
							OnDeleted(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerDelete, Service));
						}
						else
						{
							OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerConnect, Service));
						}
					}
				}
				else if (ConnectionStates.Connecting == propConnectionState)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
				else
				{
					ReadModuleInfo();
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
			{
				readStatusInProgress = false;
				long previousActID = logActID;
				logActID = GetID(pData, dataLen);
				if (logActID < oldLogID)
				{
					if (!isSGXDetectionLogger)
					{
						ReadALL();
					}
					break;
				}
				switch (option)
				{
				case 1:
					if (0 > logActID)
					{
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerClear, Service));
						propReadRequestActive = false;
					}
					else if (!propReadRequestActive)
					{
						ReadALL();
					}
					break;
				case 2:
					if (0 > logActID)
					{
						base.OnError(new PviEventArgs("Incompatible PVI version for connecting logger", "", 0, "", Action.NONE, Service));
						break;
					}
					eIndexValid = true;
					oldLogID = -1L;
					logActID = 0L;
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
					break;
				case 3:
					if (0 > logActID)
					{
						propReadRequestActive = false;
						break;
					}
					OnUpdateLoggerEntriesRequest(previousActID, errorCode, option);
					if (errorCode == 0 && LoggerEntries.Count == 0 && ContinuousActive)
					{
						propReadRequestActive = false;
						Read(propRequestContentVersion);
					}
					break;
				case 4:
					if (0 > logActID)
					{
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerIndexForUpdate, Service));
						propReadRequestActive = false;
					}
					else
					{
						OnUpdateLoggerEntriesRequest(previousActID, errorCode, option);
					}
					break;
				default:
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerGetStatus, Service));
					propReadRequestActive = false;
					break;
				}
				break;
			}
			case PVIReadAccessTypes.ModuleData:
			case PVIReadAccessTypes.ANSL_LoggerModuleData:
				switch (option)
				{
				case 1:
					accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadBlockForAdded);
					break;
				case 2:
					accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadBlock);
					break;
				case 3:
					accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadEntry);
					break;
				case 4:
					accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadEntryForCompare);
					break;
				default:
					accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerRead);
					break;
				}
				break;
			case PVIReadAccessTypes.ModuleList:
				if (errorCode == 0)
				{
					UpdateModuleInfo(pData, dataLen);
				}
				break;
			case PVIReadAccessTypes.ANSL_LoggerModuleInfo:
				OnLoggerInfoRead(errorCode, accessType, pData, dataLen);
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
			case PVIWriteAccessTypes.Cancel:
				if (waitOnCancel)
				{
					waitOnCancel = false;
					ReadIndex(Action.LoggerIndexForRead);
				}
				break;
			case PVIWriteAccessTypes.State:
				ResetReadIDs();
				if (CleanLoggerEntries(errorCode))
				{
					OnCleared(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerClear, Service));
				}
				break;
			case PVIWriteAccessTypes.EventMask:
				if (errorCode == 0)
				{
					if (option == 0)
					{
						OnContinuousActivated(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerContinuousActivate, Service));
					}
					else
					{
						OnContinuousDeactivated(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerContinuousDeactivate, Service));
					}
				}
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		private long GetID(IntPtr ptrData, uint dataLen)
		{
			string text = "";
			string text2 = "";
			oldLogID = logActID;
			if (dataLen == 0 || IntPtr.Zero == ptrData)
			{
				return -2L;
			}
			string text3 = "";
			text3 = PviMarshal.PtrToStringAnsi(ptrData, dataLen);
			string[] array = text3.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				text = array.GetValue(i).ToString();
				if (text.IndexOf("ID=") == 0)
				{
					string[] array2 = text.Split('=');
					text2 = array2.GetValue(1).ToString();
					return Convert.ToInt64(text2);
				}
			}
			return -2L;
		}

		private string GetIDData(PviResponseData cb)
		{
			string result = "";
			if (cb.DataLen != 0 && IntPtr.Zero != cb.PtrData)
			{
				result = PviMarshal.PtrToStringAnsi(cb.PtrData, cb.DataLen);
			}
			return result;
		}

		private int CancelRequests()
		{
			int result = 0;
			if (propReadRequestActive)
			{
				waitOnCancel = true;
				result = CancelRequest();
				propReadRequestActive = false;
			}
			return result;
		}

		private int OnUpdateLoggerEntriesRequest(long previousActID, int eCode, int option)
		{
			if (!propReadRequestActive)
			{
				if (previousActID < logActID)
				{
					readActID = logActID;
					readStartIndex = logActID;
					if (0 == previousActID)
					{
						entriesToRead = 1 + readStartIndex;
					}
					else
					{
						entriesToRead = readStartIndex - previousActID;
					}
					entriesRead = 0L;
					readSize = SetReadSize(entriesToRead);
					return Read(readSize, readStartIndex, Action.LoggerReadBlock);
				}
				if (previousActID <= logActID)
				{
					if (propLoggerEntries.Count < logActID)
					{
						readActID = logActID;
						readStartIndex = logActID;
						entriesToRead = 1 + (previousActID - propLoggerEntries.Count);
						entriesRead = 0L;
						readSize = SetReadSize(entriesToRead);
						return Read(readSize, readStartIndex, Action.LoggerReadBlock);
					}
					return Read(1, logActID, Action.LoggerReadEntryForCompare);
				}
				ReadALL();
			}
			return 0;
		}

		internal bool CleanLoggerEntries(int eCode)
		{
			if (!isClean)
			{
				isClean = true;
				if (LoggerEntries != null && 0 < LoggerEntries.Count)
				{
					LoggerEntries.Clear();
				}
				OnCleared(new PviEventArgs(base.Name, base.Address, eCode, Service.Language, Action.LoggerClear, Service));
			}
			return isClean;
		}

		internal override int ReadModuleInfo()
		{
			int result = 0;
			if (((Cpu)propParent).propModuleInfoList != null && ((Cpu)propParent).propModuleInfoList.ContainsKey(base.Name))
			{
				updateProperties(((Cpu)propParent).propModuleInfoList[base.Name], (((Cpu)propParent).BootMode == BootMode.Diagnostics) ? true : false);
				OnConnected(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ConnectedEvent, Service));
			}
			else if (((Cpu)propParent).Loggers.UpdateLoggerList(ModuleListOptions.INA2000CompatibleMode) == 0)
			{
				base.Requests |= Actions.ModuleInfo;
				result = -3;
			}
			else
			{
				result = base.ReadModuleInfo();
			}
			return result;
		}

		private bool ReadOutstandingData(int readCount, ref int retVal, Action action)
		{
			retVal = 0;
			if (readCount == 0)
			{
				entriesRead = entriesToRead;
				readStartIndex = readActID;
			}
			else
			{
				entriesRead += readCount;
				readStartIndex -= readCount;
			}
			if (entriesToRead <= entriesRead)
			{
				return FinaleReadData(ref retVal, action);
			}
			readSize = SetReadSize(entriesToRead - entriesRead);
			retVal = Read(readSize, readStartIndex, action);
			return true;
		}

		private bool FinaleReadData(ref int retVal, Action action)
		{
			retVal = 0;
			if (readActID < logActID)
			{
				readStartIndex = logActID;
				entriesToRead = logActID - readActID;
				readActID = logActID;
				readSize = SetReadSize(entriesToRead);
				retVal = Read(readSize, readStartIndex, action);
				return true;
			}
			return false;
		}

		private int LoggerEntriesDifffer(LoggerEntry lEntry1, LoggerEntry entryTo)
		{
			if (lEntry1 == null || entryTo == null)
			{
				return 1;
			}
			if (LoggerEntries.ContainsKey(lEntry1.UniqueKey))
			{
				if (lEntry1.DateTime != entryTo.DateTime)
				{
					return 1;
				}
				if (lEntry1.ErrorNumber != entryTo.ErrorNumber)
				{
					return 2;
				}
				if (lEntry1.ErrorInfo != entryTo.ErrorInfo)
				{
					return 3;
				}
				if (lEntry1.CodeOffset != entryTo.CodeOffset)
				{
					return 4;
				}
				if (lEntry1.LevelType != entryTo.LevelType)
				{
					return 5;
				}
				return 0;
			}
			return -1;
		}

		private int accModuleData(int errorCode, PVIReadAccessTypes accessType, IntPtr pData, uint dataLen, Action action)
		{
			int retVal = errorCode;
			int num = errorCode;
			int readCount = 0;
			LoggerEntry loggerEntry = null;
			LoggerEntryCollection eventEntries = null;
			if (errorCode == 0 || 11191 == errorCode || 11004 == errorCode)
			{
				if (11191 != errorCode && 11004 != errorCode)
				{
					retVal = ((PVIReadAccessTypes.ANSL_LoggerModuleData != accessType) ? DoInterpretLoggerData(errorCode, pData, dataLen, action, ref readCount, ref eventEntries) : DoInterpretANSLLoggerData(errorCode, pData, dataLen, action, ref readCount, ref eventEntries));
				}
				else
				{
					retVal = 0;
					eventEntries = new LoggerEntryCollection("EventEventEntries");
					eventEntries.propContentVersion = ContentVersion;
					num = 0;
				}
				if (retVal != 0)
				{
					OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, retVal, Service.Language, Action.LoggerRead, new LoggerEntryCollection("EventEntries")));
					return retVal;
				}
				switch (action)
				{
				case Action.LoggerRead:
					OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerRead, eventEntries));
					break;
				case Action.LoggerReadBlockForAdded:
					if (!ReadOutstandingData(readCount, ref retVal, Action.LoggerReadBlockForAdded))
					{
						OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerReadBlockForAdded, new LoggerEntryCollection("EventEntries")));
					}
					break;
				case Action.LoggerReadBlock:
					if (!ReadOutstandingData(readCount, ref retVal, Action.LoggerReadBlock))
					{
						OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerReadBlock, eventEntries));
					}
					else
					{
						OnEntryBlockRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerReadBlock, eventEntries));
					}
					break;
				case Action.LoggerReadEntry:
					OnEntryAdded(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerReadEntry, eventEntries));
					break;
				case Action.LoggerReadEntryForCompare:
					if (1 != eventEntries.Count)
					{
						break;
					}
					loggerEntry = eventEntries[0];
					if (0 < propLoggerEntries.Count)
					{
						if (LoggerEntriesDifffer(lastLoggerEntry, loggerEntry) == 0)
						{
							propReadRequestActive = false;
							entriesToRead = 0L;
							if (4808 == num)
							{
								ResetReadIDs();
							}
							else if (num == 0 && ConnectionStates.Connected != propConnectionState)
							{
								OnConnected(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerIndexForUpdate));
							}
							else
							{
								OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, Action.LoggerIndexForUpdate, new LoggerEntryCollection("EventEntries")));
							}
						}
						else
						{
							propReadRequestActive = false;
							ReadALL();
						}
					}
					else
					{
						propReadRequestActive = false;
						ReadALL();
					}
					break;
				}
			}
			else
			{
				logActID -= readCount;
				if (Action.LoggerReadEntryForCompare == action || Action.LoggerReadBlock == action)
				{
					logActID -= entriesToRead;
					if (0 > logActID)
					{
						logActID = 0L;
					}
					OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, num, Service.Language, action, new LoggerEntryCollection("EventEntries")));
				}
			}
			return retVal;
		}

		private int DoInterpretANSLLoggerData(int errorCode, IntPtr pData, uint dataLen, Action action, ref int readCount, ref LoggerEntryCollection eventEntries)
		{
			int num = 0;
			int num2 = 0;
			uint readVersion = 0u;
			readCount = 0;
			if (dataLen == 0 || IntPtr.Zero == pData)
			{
				return -1;
			}
			try
			{
				string text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				num = text.IndexOf('\0');
				if (-1 != num)
				{
					num = 0;
				}
				if (num != 0)
				{
					num = 0;
				}
				eventEntries = new LoggerEntryCollection("EventLogEntries");
				eventEntries.Initialize(text);
				eventEntries._OffsetUtc = LoggerEntries.OffsetUtc;
				LoggerXMLInterpreter loggerXMLInterpreter = new LoggerXMLInterpreter();
				num = loggerXMLInterpreter.ParseXMLContent(this, text, ref eventEntries, ref readVersion);
				if (num != 0)
				{
					return num;
				}
				HasEventLogEntries = false;
				LoggerEntries.propContentVersion = (propContentVersion = (eventEntries.propContentVersion = readVersion));
				for (num2 = 0; num2 < eventEntries.Count; num2++)
				{
					LoggerEntry loggerEntry = eventEntries[num2];
					if (loggerEntry.EventID != 0)
					{
						HasEventLogEntries = true;
					}
					LoggerEntries.Add(loggerEntry);
					readCount++;
					if (Action.LoggerReadBlockForAdded == action)
					{
						LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("EventLogEntries");
						loggerEntryCollection.Add(loggerEntry);
						loggerEntryCollection.propContentVersion = propContentVersion;
						OnEntryAdded(new LoggerEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerReadEntry, loggerEntryCollection));
					}
				}
				return num;
			}
			catch
			{
				return 11;
			}
		}

		private int DoInterpretLoggerData(int errorCode, IntPtr pData, uint dataLen, Action action, ref int readCount, ref LoggerEntryCollection eventEntries)
		{
			int result = 0;
			uint num = 0u;
			HasEventLogEntries = false;
			string text = "";
			readCount = 0;
			if (dataLen == 0 || IntPtr.Zero == pData)
			{
				return -1;
			}
			try
			{
				LoggerEntries.propContentVersion = (propContentVersion = 0u);
				text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				int num2 = text.IndexOf("DN=");
				int num3 = text.IndexOf("\0");
				if (-1 == num3)
				{
					num3 = text.Length;
				}
				string value = text.Substring(num2 + 3, num3 - num2 - 3);
				readCount = Convert.ToInt32(value);
				string text2 = text;
				char[] separator = new char[1];
				string[] array = text2.Split(separator);
				num = 0u;
				eventEntries = new LoggerEntryCollection("EventEventEntries");
				eventEntries.propContentVersion = propContentVersion;
				for (int i = 1; i < array.Length - 1; i++)
				{
					if (i % 3 == 1)
					{
						LoggerEntry loggerEntry = null;
						uint propErrorNumber = 0u;
						LevelType propLevelType = LevelType.Success;
						string instance = "";
						uint internId = 0u;
						uint propErrorInfo = 0u;
						string text3 = array[i];
						num++;
						string[] array2 = text3.Split(' ');
						for (int j = 0; j < array2.Length; j++)
						{
							string text4 = array2[j];
							if (-1 != text4.IndexOf('"'))
							{
								for (int k = j + 1; k < array2.Length; k++)
								{
									text4 = array2[k];
									string[] array3;
									string[] array4 = array3 = array2;
									int num4 = j;
									IntPtr intPtr = (IntPtr)num4;
									array4[num4] = array3[(long)intPtr] + " " + text4;
									array2[k] = "";
									if (-1 != text4.IndexOf('"'))
									{
										break;
									}
								}
							}
						}
						for (int l = 0; l < array2.Length; l++)
						{
							if (array2[l].StartsWith("TIME="))
							{
								int num5 = array2[l].IndexOf("=");
								loggerEntry = new LoggerEntry(this, array2[l].Substring(num5 + 1, array2[l].Length - num5 - 1));
							}
							if (array2[l].StartsWith("E="))
							{
								int num5 = array2[l].IndexOf("=");
								propErrorNumber = Pvi.ToUInt32(array2[l].Substring(num5 + 1, array2[l].Length - num5 - 1));
							}
							if (array2[l].StartsWith("LEV="))
							{
								int num5 = array2[l].IndexOf("=");
								propLevelType = (LevelType)Convert.ToInt32(array2[l].Substring(num5 + 1, array2[l].Length - num5 - 1));
							}
							if (array2[l].StartsWith("TN="))
							{
								int num5 = array2[l].IndexOf("=");
								instance = array2[l].Substring(num5 + 2, array2[l].Length - num5 - 3);
							}
							if (array2[l].StartsWith("ID="))
							{
								int num5 = array2[l].IndexOf("=");
								internId = Convert.ToUInt32(array2[l].Substring(num5 + 1, array2[l].Length - num5 - 1));
							}
							if (array2[l].StartsWith("INFO="))
							{
								int num5 = array2[l].IndexOf("=");
								propErrorInfo = Convert.ToUInt32(array2[l].Substring(num5 + 1, array2[l].Length - num5 - 1));
							}
						}
						if (loggerEntry == null && text3.IndexOf("TIME=") == -1)
						{
							loggerEntry = new LoggerEntry(this, DateTime.Now);
						}
						if (loggerEntry != null)
						{
							loggerEntry.propErrorNumber = propErrorNumber;
							loggerEntry.propLevelType = propLevelType;
							loggerEntry.propTask = instance.ConvertToAsciiCompatible();
							loggerEntry._internId = internId;
							loggerEntry.UpdateUKey();
							loggerEntry.propErrorInfo = propErrorInfo;
							loggerEntry.propASCIIData = array[++i];
							i++;
							loggerEntry.propBinary = HexConvert.ToBytes(array[i]);
							if ((loggerEntry.propErrorInfo & 2) != 0 || (loggerEntry.propErrorInfo & 1) != 0)
							{
								loggerEntry.GetExceptionData(isARM: false, useBinForI386exc: true);
							}
							LoggerEntries.Add(loggerEntry);
							eventEntries.Add(loggerEntry);
							if (Action.LoggerReadBlockForAdded == action)
							{
								LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("EventEntries");
								loggerEntryCollection.Add(loggerEntry);
								OnEntryAdded(new LoggerEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.LoggerReadEntry, loggerEntryCollection));
							}
						}
					}
				}
				return result;
			}
			catch
			{
				return 11;
			}
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			readStatusInProgress = false;
			eIndexValid = false;
			base.OnDisconnected(e);
			if (propReadRequestActive)
			{
				OnEntriesRead(new LoggerEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.LoggerRead, new LoggerEntryCollection("EventEntries")));
				propReadRequestActive = false;
			}
		}

		protected override string getLinkDescription()
		{
			if (propContinuousActive)
			{
				return "EV=es";
			}
			return "EV=e";
		}

		internal override bool CheckModuleInfo(int errorCode)
		{
			if (Actions.Connected == (base.Requests & Actions.Connected))
			{
				base.Requests &= ~Actions.Connected;
				return true;
			}
			return base.CheckModuleInfo(errorCode);
		}

		protected override void OnDeleted(PviEventArgs e)
		{
			base.OnDeleted(e);
		}

		protected override void OnConnected(PviEventArgs e)
		{
			if (e.ErrorCode == 0 && !((Cpu)propParent).IsConnected)
			{
				base.Requests |= Actions.Connected;
			}
			else if (4819 == e.ErrorCode)
			{
				ResetReadIDs();
				Connect();
			}
			else if (eIndexValid)
			{
				base.OnConnected(e);
				if (Actions.SetActive == (base.Requests & Actions.SetActive))
				{
					base.Requests &= ~Actions.SetActive;
					SetContinuousActive(propContinuousActive);
				}
				else if (Actions.ReadIndex == (base.Requests & Actions.ReadIndex))
				{
					ReadIndexForUpdate(Action.LoggerIndexForUpdate);
					base.Requests &= ~Actions.ReadIndex;
				}
			}
			else
			{
				base.OnConnected(e);
				if (propContinuousActive)
				{
					ResetReadIDs();
					ReadIndex(Action.LoggerIndexForContinuousActivated);
				}
			}
		}

		private void UpdateContinuouseActive()
		{
			if (ContinuousActive)
			{
				propContinuousActive = false;
				ContinuousActive = true;
			}
		}

		protected virtual void OnContinuousActivated(PviEventArgs e)
		{
			propContinuousActive = true;
			ReadIndex(Action.LoggerIndexForContinuousActivated);
			if (this.ContinuousActivated != null)
			{
				this.ContinuousActivated(this, e);
			}
		}

		protected virtual void OnContinuousDeactivated(PviEventArgs e)
		{
			propContinuousActive = false;
			if (this.ContinuousDeactivated != null)
			{
				this.ContinuousDeactivated(this, e);
			}
		}

		protected virtual void OnEntriesRead(LoggerEventArgs e)
		{
			if (0 < e.Entries.Count)
			{
				lastLoggerEntry = e.Entries[0];
			}
			else if (propLoggerEntries != null && 0 < propLoggerEntries.Count)
			{
				lastLoggerEntry = propLoggerEntries[0];
			}
			propReadRequestActive = false;
			entriesToRead = 0L;
			if (4808 == e.ErrorCode)
			{
				ResetReadIDs();
			}
			else if (e.ErrorCode == 0 && ConnectionStates.Connected != propConnectionState)
			{
				OnConnected(e);
			}
			if (this.EntriesRead != null)
			{
				this.EntriesRead(this, e);
			}
			OnGlobalAdded(e);
		}

		protected virtual void OnEntryBlockRead(LoggerEventArgs e)
		{
			if (this.EntryBlockRead != null)
			{
				this.EntryBlockRead(this, e);
			}
			OnGlobalAdded(e);
		}

		protected virtual void OnCleared(PviEventArgs e)
		{
			if (this.Cleared != null)
			{
				this.Cleared(this, e);
			}
		}

		protected internal override void OnError(PviEventArgs e)
		{
			if (Action.LoggerDetectSGType == e.Action || Action.LoggerGetStatus == e.Action)
			{
				OnSingleError(e);
			}
			else
			{
				base.OnError(e);
			}
		}

		protected virtual void OnEntryAdded(LoggerEventArgs e)
		{
			if (this.EntryAdded != null)
			{
				this.EntryAdded(this, e);
			}
			OnGlobalAdded(e);
		}

		internal void CallOnEntriesRemoved(LoggerEventArgs e)
		{
			OnGlobalRemoved(e);
			OnEntriesRemoved(e);
		}

		protected virtual void OnEntriesRemoved(LoggerEventArgs e)
		{
			if (this.EntriesRemoved != null)
			{
				this.EntriesRemoved(this, e);
			}
		}

		protected virtual void OnGlobalAdded(LoggerEventArgs e)
		{
			if (GlobalMerge && this.GlobalAdded != null)
			{
				this.GlobalAdded(this, new LoggerEventArgs(e, Action.LoggerGlobalAdded));
			}
		}

		protected virtual void OnGlobalRemoved(LoggerEventArgs e)
		{
			if (GlobalMerge && this.GlobalRemoved != null)
			{
				this.GlobalRemoved(this, e);
			}
		}

		internal void UpdateLogUID(uint logUID)
		{
			if (logUID != 0)
			{
				propModUID = logUID;
			}
			else if (Service != null)
			{
				propModUID = Service.ModuleUID;
			}
			else
			{
				propModUID++;
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

		private void SetContinuousActive(bool activate)
		{
			int num = 0;
			Action action = Action.LoggerContinuousActivate;
			string text = "es";
			if (!activate)
			{
				text = "e";
				action = Action.LoggerContinuousDeactivate;
			}
			Service.BuildRequestBuffer(text);
			num = (activate ? WriteRequest(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length, (uint)action) : WriteRequestA(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length, (uint)action));
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, action, Service));
			}
		}

		internal void AddEntriesToServiceCollection(LoggerEntryCollection loggerEntries)
		{
			if (Service != null && loggerEntries != null && loggerEntries.Count != 0)
			{
				if (this is ErrorLogBook)
				{
					foreach (LoggerEntry value in loggerEntries.Values)
					{
						Service.LoggerEntries.Add(value, addKeyOnly: true);
					}
				}
				else
				{
					foreach (LoggerEntry value2 in loggerEntries.Values)
					{
						Service.LoggerEntries.Add(value2);
					}
				}
				OnGlobalAdded(new LoggerEventArgs(base.Name, base.Address, 0, Service.Language, Action.LoggerGlobalAdded, loggerEntries));
			}
		}

		internal void UpdateHasEventLogEntries(bool newValue)
		{
			HasEventLogEntries = newValue;
		}

		internal uint DecrementEntryID()
		{
			if (Service == null)
			{
				return --_entryIDDec;
			}
			return --Service.EntryIDDec;
		}

		internal uint IncrementEntryID()
		{
			if (Service == null)
			{
				return ++_entryIDInc;
			}
			return ++Service.EntryIDInc;
		}
	}
}
