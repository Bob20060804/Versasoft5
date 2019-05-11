using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public abstract class Base : PviCBEvents, IDisposable
	{
		internal const int WM_USER = 10000;

		internal const uint SET_PVICALLBACK = uint.MaxValue;

		internal const uint SET_PVICALLBACK_DATA = 4294967294u;

		internal const uint SET_PVICALLBACK_ASYNC = 4294967293u;

		internal const uint SET_PVIFUNCTION = 4294967292u;

		internal bool propReadingFormat;

		protected bool propNoDisconnectedEvent;

		protected int propReturnValue;

		internal bool propDisposed;

		internal bool isObjectConnected;

		internal Service propService;

		protected bool propHasLinkObject;

		protected ConnectionType propConnectionType;

		internal bool reCreateActive;

		internal string propErrorText = string.Empty;

		internal string propCurLanguage = string.Empty;

		internal string propName;

		internal string propAddress;

		internal string propLinkName;

		internal SNMPBase propSNMPParent;

		internal Base propParent;

		internal string propLogicalName;

		internal object propUserData;

		internal int propErrorCode;

		internal uint propLinkId;

		internal string propObjectParam;

		internal string propLinkParam;

		internal bool propReCreateActive;

		internal ConnectionStates propConnectionState;

		internal Actions propResponses;

		internal Actions propRequests;

		internal bool propAddToLogicalObjects;

		internal MethodType propMethodType;

		public int ReturnValue => propReturnValue;

		public string Address
		{
			get
			{
				return propAddress;
			}
			set
			{
				propAddress = value;
			}
		}

		internal string AddressEx
		{
			get
			{
				if (propAddress != null && 0 < propAddress.Length)
				{
					return propAddress;
				}
				return propName;
			}
		}

		public string Name => propName;

		public virtual Base Parent => propParent;

		public abstract string FullName
		{
			get;
		}

		public abstract string PviPathName
		{
			get;
		}

		public virtual Service Service => propService;

		public object UserData
		{
			get
			{
				return propUserData;
			}
			set
			{
				propUserData = value;
			}
		}

		internal MethodType MethodType
		{
			get
			{
				return propMethodType;
			}
			set
			{
				propMethodType = value;
			}
		}

		public virtual bool HasPVIConnection
		{
			get
			{
				if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					return true;
				}
				return false;
			}
		}

		public virtual bool IsConnected
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

		public virtual bool HasLinkObject => propHasLinkObject;

		public bool HasError => 0 != ErrorCode;

		internal string LogicalName
		{
			get
			{
				if (propLogicalName == null || propLogicalName.Length == 0)
				{
					switch (Service.LogicalObjectsUsage)
					{
					case LogicalObjectsUsage.ObjectName:
						return Name;
					case LogicalObjectsUsage.ObjectNameWithType:
						return PviPathName;
					default:
						return FullName;
					}
				}
				return propLogicalName;
			}
		}

		internal string ObjectParam
		{
			get
			{
				return propObjectParam;
			}
			set
			{
				propObjectParam = value;
			}
		}

		internal Actions Requests
		{
			get
			{
				return propRequests;
			}
			set
			{
				propRequests = value;
			}
		}

		internal Actions Responses
		{
			get
			{
				return propResponses;
			}
			set
			{
				propResponses = value;
			}
		}

		internal uint LinkId => propLinkId;

		internal uint InternId => _internId;

		public ConnectionType ConnectionType
		{
			get
			{
				return propConnectionType;
			}
			set
			{
				propConnectionType = value;
			}
		}

		public int ErrorCode => propErrorCode;

		public string ErrorText
		{
			get
			{
				string text = "en";
				if (Service != null)
				{
					text = Service.Language;
				}
				if (propErrorText == string.Empty)
				{
					propCurLanguage = text;
					if (Service == null)
					{
						propErrorText = Service.GetErrorText(ErrorCode, text);
					}
					else
					{
						propErrorText = Service.Utilities.GetErrorText(ErrorCode);
					}
					return propErrorText;
				}
				if (text.CompareTo(propCurLanguage) == 0)
				{
					return propErrorText;
				}
				propCurLanguage = text;
				if (Service == null)
				{
					propErrorText = Service.GetErrorText(ErrorCode, text);
				}
				else
				{
					propErrorText = Service.Utilities.GetErrorText(ErrorCode);
				}
				return propErrorText;
			}
		}

		public string LinkName
		{
			get
			{
				if (propLinkName == null || propLinkName.Length == 0)
				{
					if (Service != null)
					{
						switch (Service.LogicalObjectsUsage)
						{
						case LogicalObjectsUsage.ObjectName:
							return Name;
						case LogicalObjectsUsage.FullName:
							return FullName;
						case LogicalObjectsUsage.ObjectNameWithType:
							return PviPathName;
						default:
							return LogicalName;
						}
					}
					return Name;
				}
				return propLinkName;
			}
			set
			{
				propLinkName = value;
			}
		}

		public event DisposeEventHandler Disposing;

		public event PviEventHandler Connected;

		internal event PviEventHandler Linked;

		public event PviEventHandler ConnectionChanged;

		public event PviEventHandler Removed;

		public event PviEventHandler Disconnected;

		public event PviEventHandler Error;

		public event PviEventHandler PropertyChanged;

		internal int CancelRequest()
		{
			return CancelRequest(silent: true);
		}

		internal int CancelRequest(bool silent)
		{
			int num = 0;
			int[] source = new int[1]
			{
				0
			};
			IntPtr hMemory = PviMarshal.AllocHGlobal(4);
			Marshal.Copy(source, 0, hMemory, 1);
			num = (silent ? WriteRequest(Service.hPvi, propLinkId, AccessTypes.Cancel, hMemory, 4, 0u) : WriteRequest(Service.hPvi, propLinkId, AccessTypes.Cancel, hMemory, 4, 1100u));
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		internal void UpdateServiceCreateState()
		{
			if (propConnectionState != 0 && ConnectionStates.Disconnecting != propConnectionState && ConnectionStates.Disconnected != propConnectionState)
			{
				propConnectionState = ConnectionStates.Unininitialized;
				reCreateActive = true;
			}
		}

		private void Initialize(string name, bool addToVColls)
		{
			propReadingFormat = false;
			propReCreateActive = false;
			reCreateActive = false;
			propNoDisconnectedEvent = false;
			propSNMPParent = null;
			propService = null;
			propDisposed = false;
			isObjectConnected = false;
			propConnectionType = ConnectionType.CreateAndLink;
			propAddress = "";
			propName = name;
			propObjectParam = "";
			propConnectionState = ConnectionStates.Unininitialized;
			propErrorCode = 0;
			propLinkName = "";
			propAddToLogicalObjects = addToVColls;
		}

		public Base(Base parentObj)
		{
			Initialize("", addToVColls: true);
			propParent = parentObj;
			if (parentObj is Cpu)
			{
				propService = ((Cpu)parentObj).Service;
			}
			else if (parentObj is Task)
			{
				propService = ((Task)parentObj).Service;
			}
			else if (parentObj is Variable)
			{
				propService = ((Variable)parentObj).Service;
			}
			else if (parentObj is Service)
			{
				propService = ((Service)parentObj).Service;
			}
			AddToCBReceivers();
		}

		public Base()
		{
			Initialize("", addToVColls: false);
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
			RemoveFromCBReceivers();
			if (!propDisposed)
			{
				Dispose(disposing: true, removeFromCollection: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing, bool removeFromCollection)
		{
			RemoveFromCBReceivers();
			if (propDisposed)
			{
				return;
			}
			OnDisposing(disposing);
			if (disposing)
			{
				if (Service != null)
				{
					Service.RemoveLogicalObject(LogicalName);
				}
				if (ConnectionStates.Disconnecting != propConnectionState)
				{
					Unlink();
				}
				propDisposed = true;
				propLinkName = null;
				propLogicalName = null;
				propUserData = null;
				propObjectParam = null;
				propLinkParam = null;
				propName = null;
				propAddress = null;
				propParent = null;
				propSNMPParent = null;
				propService = null;
				propCurLanguage = null;
			}
		}

		internal Base(string name)
		{
			Initialize(name, addToVColls: true);
		}

		public Base(Base parentObj, string name)
		{
			Initialize(name, addToVColls: true);
			propParent = parentObj;
			propSNMPParent = null;
			if (parentObj != null)
			{
				propService = parentObj.Service;
			}
			AddToCBReceivers();
		}

		internal Base(string name, bool addToVColls)
		{
			Initialize(name, addToVColls);
		}

		public Base(Base parentObj, string name, bool addToVColls)
		{
			Initialize(name, addToVColls);
			propParent = parentObj;
			if (parentObj is Cpu)
			{
				propService = ((Cpu)parentObj).Service;
			}
			else if (parentObj is Task)
			{
				propService = ((Task)parentObj).Service;
			}
			else if (parentObj is Variable)
			{
				propService = ((Variable)parentObj).Service;
			}
			else if (parentObj is Service)
			{
				propService = ((Service)parentObj).Service;
			}
			AddToCBReceivers();
		}

		~Base()
		{
			if (Service != null && (!Service.IsStatic && LinkId != 0))
			{
				PInvokePvicom.PviComUnlinkRequest(Service.hPvi, LinkId, null, 0u, 0u);
			}
			Dispose(disposing: false, removeFromCollection: true);
		}

		internal bool AddToCBReceivers()
		{
			if (Service != null)
			{
				return Service.AddID(this, ref _internId);
			}
			return false;
		}

		internal void RemoveFromCBReceivers()
		{
			if (Service != null)
			{
				Service.RemoveID(_internId);
			}
		}

		internal int XCreateRequest(uint hPvi, string pObjName, ObjectType nPObjType, string pPObjDesc, uint eventParam, string pLinkDesc, uint respParam)
		{
			int num = 0;
			propHasLinkObject = true;
			if (eventParam == 0 && respParam == 0)
			{
				propHasLinkObject = false;
			}
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				if (Service.IsStatic && (this is Cpu || this is Module || this is Variable))
				{
					return PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, null, 4294967294u, InternId, pLinkDesc, Service.cbCreate, 4294967294u, InternId);
				}
				if (eventParam == 916)
				{
					return PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, Service.cbEventA, 4294967294u, InternId, pLinkDesc, Service.cbCreate, 4294967294u, InternId);
				}
				return PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbCreate, 4294967294u, InternId);
			}
			if (this is Variable && Service.IsStatic)
			{
				return PInvokePvicom.PviComMsgCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, IntPtr.Zero, eventParam, InternId, pLinkDesc, Service.WindowHandle, respParam, InternId);
			}
			return PInvokePvicom.PviComMsgCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, Service.WindowHandle, eventParam, InternId, pLinkDesc, Service.WindowHandle, respParam, InternId);
		}

		internal int XLinkRequest(uint hPvi, string pObjName, uint eventParam, string pLinkDesc, uint respParam)
		{
			return XLinkRequest(hPvi, pObjName, eventParam, pLinkDesc, respParam, InternId);
		}

		internal int XLinkRequest(uint hPvi, string pObjName, uint eventParam, string pLinkDesc, uint respParam, uint internalID)
		{
			propHasLinkObject = true;
			StringMarshal stringMarshal = new StringMarshal();
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				if (711 == eventParam)
				{
					if (709 == respParam)
					{
						return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEventA, 4294967294u, InternId, pLinkDesc, Service.cbLinkA, 4294967294u, InternId);
					}
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLink, 4294967294u, InternId);
				}
				switch (respParam)
				{
				case 112u:
				case 113u:
				case 115u:
				case 117u:
				case 119u:
				case 121u:
				case 123u:
				case 124u:
				case 151u:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLinkA, 4294967294u, InternId);
				case 709u:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLinkB, 4294967294u, InternId);
				case 2713u:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLinkC, 4294967294u, InternId);
				case 2712u:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLinkD, 4294967294u, InternId);
				case 2718u:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLinkE, 4294967294u, InternId);
				default:
					return PInvokePvicom.PviComLinkRequest(hPvi, pObjName, Service.cbEvent, 4294967294u, InternId, pLinkDesc, Service.cbLink, 4294967294u, InternId);
				}
			}
			byte[] bytes = stringMarshal.GetBytes(pObjName);
			byte[] bytes2 = stringMarshal.GetBytes(pLinkDesc);
			return PInvokePvicom.PviComMsgLinkRequest(hPvi, bytes, Service.WindowHandle, eventParam, internalID, bytes2, Service.WindowHandle, respParam, internalID);
		}

		internal int XMLinkRequest(uint hPvi, out uint pLinkId, string objName, uint eventMsgNo, uint eventParam, string linkDesc)
		{
			int num = 0;
			return PInvokePvicom.PviComMsgLink(hPvi, out pLinkId, objName, Service.WindowHandle, 6u, 6u, linkDesc);
		}

		internal int XMLink(uint hPvi, out uint pLinkId, string objName, uint eventMsgNo, uint eventParam, string linkDesc)
		{
			int num = 0;
			return PInvokePvicom.PviComMsgLink(hPvi, out pLinkId, objName, Service.WindowHandle, 6u, 6u, linkDesc);
		}

		internal int Write(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen)
		{
			IntPtr zero = IntPtr.Zero;
			int rstDataLen = 0;
			return PInvokePvicom.PviComWrite(hPvi, linkID, nAccess, pData, dataLen, zero, rstDataLen);
		}

		internal int WriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, string strValue, uint respParam, uint objID)
		{
			int num = 0;
			uint respParam2 = respParam;
			IntPtr hMemory = IntPtr.Zero;
			if (-1 != (int)objID)
			{
				respParam2 = objID;
			}
			if (0 < strValue.Length)
			{
				hMemory = PviMarshal.StringToHGlobal(strValue);
			}
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				switch (respParam)
				{
				case 723u:
					num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, Service.cbWriteA, 4294967294u, respParam2);
					break;
				case 740u:
					num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, Service.cbWriteU, 4294967294u, respParam2);
					break;
				default:
					num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, Service.cbWrite, 4294967294u, respParam2);
					break;
				}
			}
			else
			{
				num = PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, Service.WindowHandle, 1300u, InternId);
			}
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return num;
		}

		internal int WriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, string strValue, uint respParam)
		{
			return WriteRequest(hPvi, linkID, nAccess, strValue, respParam, uint.MaxValue);
		}

		internal int WriteRequestA(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respParam)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteA, 4294967294u, InternId);
			}
			return PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.WindowHandle, respParam, InternId);
		}

		internal int WriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respParam)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				switch (respParam)
				{
				case 0u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, null, 0u, 0u);
				case 303u:
				case 503u:
				case 815u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteA, 4294967294u, InternId);
				case 403u:
				case 555u:
				case 816u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteB, 4294967294u, InternId);
				case 304u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteC, 4294967294u, InternId);
				case 404u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteD, 4294967294u, InternId);
				case 405u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteE, 4294967294u, InternId);
				case 312u:
				case 410u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteF, 4294967294u, InternId);
				case 412u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteF, 4294967294u, InternId);
				case 740u:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWriteU, 4294967294u, InternId);
				default:
					return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWrite, 4294967294u, InternId);
				}
			}
			return PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.WindowHandle, respParam, InternId);
		}

		internal int Read(uint hPvi, uint linkID, AccessTypes nAccess, SyncReadData readData)
		{
			int num = 0;
			return PInvokePvicom.PviComRead(hPvi, linkID, nAccess, readData.PtrArgData, readData.ArgDataLength, readData.PtrData, readData.DataLength);
		}

		internal int ReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, uint respParam)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				switch (respParam)
				{
				case 150u:
				case 700u:
				case 907u:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbReadA, 4294967294u, InternId);
				case 908u:
				case 2810u:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbReadB, 4294967294u, InternId);
				case 915u:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbReadC, 4294967294u, InternId);
				case 913u:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbReadD, 4294967294u, InternId);
				case 2809u:
				case 2811u:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbReadE, 4294967294u, InternId);
				default:
					return PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, Service.cbRead, 4294967294u, InternId);
				}
			}
			return PInvokePvicom.PviComMsgReadRequest(hPvi, linkID, nAccess, Service.WindowHandle, respParam, InternId);
		}

		internal int ReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, string reqData, uint respParam)
		{
			return ReadArgumentRequest(hPvi, linkID, nAccess, reqData, respParam, InternId);
		}

		internal int ReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, string reqData, uint respParam, uint internalId)
		{
			int num = 0;
			IntPtr hMemory = PviMarshal.StringToHGlobal(reqData);
			int dataLen = reqData?.Length ?? 0;
			num = ReadArgumentRequest(hPvi, linkID, nAccess, hMemory, dataLen, respParam, internalId);
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return num;
		}

		internal int ReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respParam)
		{
			return ReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, respParam, InternId);
		}

		internal int ReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respParam, uint internalID)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				switch (respParam)
				{
				case 912u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadA, 4294967294u, internalID);
				case 904u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadB, 4294967294u, internalID);
				case 901u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadC, 4294967294u, internalID);
				case 914u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadD, 4294967294u, internalID);
				case 214u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadT, 4294967294u, internalID);
				case 725u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadS, 4294967294u, internalID);
				case 740u:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbReadU, 4294967294u, internalID);
				default:
					return PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbRead, 4294967294u, internalID);
				}
			}
			return PInvokePvicom.PviComMsgReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, Service.WindowHandle, respParam, internalID);
		}

		internal int PviDelete()
		{
			propHasLinkObject = false;
			propConnectionState = ConnectionStates.Unininitialized;
			return PInvokePvicom.PviComDelete(Service.hPvi, FullName);
		}

		internal int DeleteRequest(bool silent)
		{
			int num = 0;
			propHasLinkObject = false;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				if (silent)
				{
					propNoDisconnectedEvent = true;
					return PInvokePvicom.PviComDeleteRequest(Service.hPvi, FullName, Service.cbDelete, 0u, 0u);
				}
				return PInvokePvicom.PviComDeleteRequest(Service.hPvi, FullName, Service.cbDelete, 4294967294u, InternId);
			}
			if (silent)
			{
				propNoDisconnectedEvent = true;
				return PInvokePvicom.PviComMsgDeleteRequest(Service.hPvi, FullName, Service.WindowHandle, 0u, InternId);
			}
			return PInvokePvicom.PviComMsgDeleteRequest(Service.hPvi, FullName, Service.WindowHandle, 6u, InternId);
		}

		internal int Unlink()
		{
			int result = 0;
			propHasLinkObject = false;
			if (propLinkId != 0)
			{
				result = PInvokePvicom.PviComUnlink(Service.hPvi, propLinkId);
				propLinkId = 0u;
			}
			else
			{
				Requests |= Actions.Disconnect;
			}
			return result;
		}

		internal int UnlinkRequest(uint respParam)
		{
			int result = 0;
			propHasLinkObject = false;
			if (propLinkId != 0)
			{
				if (Service.EventMessageType != 0)
				{
					result = ((respParam != 0) ? PInvokePvicom.PviComMsgUnlinkRequest(Service.hPvi, propLinkId, Service.WindowHandle, respParam, InternId) : PInvokePvicom.PviComMsgUnlinkRequest(Service.hPvi, propLinkId, IntPtr.Zero, 0u, 0u));
				}
				else if (respParam == 0)
				{
					result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, null, 0u, 0u);
				}
				else
				{
					switch (respParam)
					{
					case 502u:
					case 2713u:
						result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, Service.cbUnlinkA, 4294967294u, InternId);
						break;
					case 602u:
					case 2712u:
						result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, Service.cbUnlinkB, 4294967294u, InternId);
						break;
					case 2718u:
						result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, Service.cbUnlinkC, 4294967294u, InternId);
						break;
					case 2813u:
						result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, Service.cbUnlinkD, 4294967294u, InternId);
						break;
					default:
						result = PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkId, Service.cbUnlink, 4294967294u, InternId);
						break;
					}
				}
			}
			else
			{
				Requests |= Actions.Disconnect;
			}
			return result;
		}

		internal virtual void RemoveObject()
		{
			Remove();
		}

		internal virtual void RemoveReferences()
		{
		}

		internal virtual void RemoveFromBaseCollections()
		{
			if (Service != null)
			{
				Service.LogicalObjects.Remove(LogicalName);
				if (Service.Services != null)
				{
					Service.Services.LogicalObjects.Remove(LogicalName);
				}
			}
		}

		public virtual void Remove()
		{
			if (Service != null)
			{
				Service.LogicalObjects.Remove(LogicalName);
				if (Service.Services != null)
				{
					Service.Services.LogicalObjects.Remove(LogicalName);
				}
			}
			if (this.Removed != null)
			{
				if (Service == null)
				{
					this.Removed(this, new PviEventArgs(Name, Address, 0, "en", Action.ObjectRemoved));
				}
				else
				{
					this.Removed(this, new PviEventArgs(Name, Address, 0, Service.Language, Action.ObjectRemoved, Service));
				}
			}
		}

		internal void Fire_Deleted(PviEventArgs e)
		{
			OnDeleted(e);
		}

		protected virtual void OnDeleted(PviEventArgs e)
		{
			if (e.ErrorCode == 0)
			{
				Remove();
			}
		}

		protected bool Fire_Connected(PviEventArgs e)
		{
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				if (ConnectionStates.Connected == propConnectionState)
				{
					return false;
				}
				isObjectConnected = true;
				propConnectionState = ConnectionStates.Connected;
				if (this.Connected != null)
				{
					this.Connected(this, e);
				}
			}
			else if (ConnectionStates.ConnectedError > propConnectionState && ConnectionStates.Unininitialized < propConnectionState && this.Connected != null)
			{
				this.Connected(this, e);
			}
			return true;
		}

		protected virtual void OnLinked(int errorCode, Action actionCode)
		{
			propErrorCode = errorCode;
			if (this.Linked != null)
			{
				this.Linked(this, new PviEventArgs(Name, Address, errorCode, Service.Language, actionCode));
			}
		}

		internal void Fire_OnConnected(PviEventArgs e)
		{
			OnConnected(e);
		}

		internal void Fire_OnDisconnected(PviEventArgs e)
		{
			OnDisconnected(e);
		}

		private void SendConnectedEvent(PviEventArgs e, ConnectionStates connState)
		{
			propConnectionState = connState;
			if (this.Connected != null)
			{
				this.Connected(this, e);
			}
		}

		protected virtual void OnConnected(PviEventArgs e)
		{
			propErrorCode = e.ErrorCode;
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				propErrorCode = 0;
				if (ConnectionStates.Connected != propConnectionState)
				{
					isObjectConnected = true;
					if (Connection.ConnectionChangeInProgress(propConnectionState))
					{
						OnConnectionChanged(e.ErrorCode, Action.ChangeConnection);
					}
					else
					{
						SendConnectedEvent(e, ConnectionStates.Connected);
					}
				}
			}
			else if ((ConnectionStates.ConnectedError > propConnectionState && ConnectionStates.Unininitialized < propConnectionState) || propConnectionState == ConnectionStates.Unininitialized)
			{
				SendConnectedEvent(e, ConnectionStates.ConnectedError);
			}
		}

		protected virtual void OnConnectionChanged(int errorCode, Action action)
		{
			if (errorCode != 0)
			{
				propConnectionState = ConnectionStates.ConnectedError;
			}
			else
			{
				propConnectionState = ConnectionStates.Connected;
			}
			if (this.ConnectionChanged != null)
			{
				this.ConnectionChanged(this, new PviEventArgs(Name, Address, errorCode, Service.Language, action));
			}
		}

		internal virtual void reCreateState()
		{
			if (reCreateActive)
			{
				propConnectionState = ConnectionStates.Unininitialized;
				propReCreateActive = true;
				reCreateActive = false;
				propLinkId = 0u;
				Connect(forceConnection: true);
			}
		}

		protected virtual string getLinkDescription()
		{
			return "EV=ed";
		}

		protected virtual string GetConnectionDescription()
		{
			if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				return $"\"{propAddress}\"";
			}
			return $"\"{propParent.LinkName}\"/\"{propAddress}\"";
		}

		public virtual void Connect()
		{
			if (!reCreateActive && LinkId == 0)
			{
				Connect(ConnectionType.CreateAndLink);
			}
		}

		internal virtual void Connect(bool forceConnection)
		{
			if (!reCreateActive && LinkId == 0)
			{
				Connect(ConnectionType.CreateAndLink);
			}
		}

		public virtual void Connect(ConnectionType connectionType)
		{
			propNoDisconnectedEvent = false;
			propReturnValue = 0;
		}

		public virtual int ChangeConnection()
		{
			int num = 0;
			return WriteRequest(Service.hPvi, LinkId, AccessTypes.Connect, GetConnectionDescription(), 3000u, _internId);
		}

		public virtual void Disconnect()
		{
			Disconnect(noResponse: false);
		}

		internal virtual int DisconnectRet(uint action)
		{
			Disconnect();
			return 0;
		}

		public virtual void Disconnect(bool noResponse)
		{
			propReturnValue = 0;
			propNoDisconnectedEvent = noResponse;
			propConnectionState = ConnectionStates.Disconnecting;
			if (noResponse)
			{
				propReturnValue = UnlinkRequest(0u);
				propLinkId = 0u;
				propConnectionState = ConnectionStates.Disconnected;
			}
			else
			{
				propReturnValue = UnlinkRequest(InternId);
			}
		}

		protected virtual void Call_Connected(PviEventArgs e)
		{
			if (this.Connected != null)
			{
				this.Connected(this, e);
			}
		}

		protected virtual void Fire_Connected(object sender, PviEventArgs e)
		{
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				if (ConnectionStates.Connected != propConnectionState)
				{
					isObjectConnected = true;
					propConnectionState = ConnectionStates.Connected;
					if (this.Connected != null)
					{
						this.Connected(sender, e);
					}
				}
			}
			else if (ConnectionStates.ConnectedError > propConnectionState && ConnectionStates.Unininitialized < propConnectionState)
			{
				propConnectionState = ConnectionStates.ConnectedError;
				if (this.Connected != null)
				{
					this.Connected(sender, e);
				}
			}
		}

		internal void Fire_ConnectedEvent(object sender, PviEventArgs e)
		{
			if (this.Connected != null)
			{
				this.Connected(sender, e);
			}
		}

		internal void FireDisconnected(int errorCode, Action action)
		{
			OnDisconnected(new PviEventArgs(Name, Address, errorCode, Service.Language, action));
		}

		protected virtual void OnDisconnected(PviEventArgs e)
		{
			if (Service.IsRemoteError(e.ErrorCode) && propConnectionState != 0 && ConnectionStates.Disconnected != propConnectionState && ConnectionStates.Disconnecting != propConnectionState && Address.CompareTo("MacAddresses") != 0)
			{
				reCreateActive = true;
			}
			if (Action.ErrorEvent == e.Action && (12039 > e.ErrorCode || 12900 < e.ErrorCode))
			{
				propConnectionState = ConnectionStates.Disconnected;
				isObjectConnected = false;
			}
			else if (ConnectionStates.Connected == propConnectionState)
			{
				propConnectionState = ConnectionStates.ConnectedError;
			}
			else
			{
				propConnectionState = ConnectionStates.Disconnected;
			}
			if (!propNoDisconnectedEvent)
			{
				propNoDisconnectedEvent = false;
				if (this.Disconnected != null)
				{
					this.Disconnected(this, e);
				}
			}
		}

		protected internal virtual void OnError(PviEventArgs e)
		{
			propErrorCode = e.ErrorCode;
			if (Service != null)
			{
				if (Service.ErrorException)
				{
					PviException ex = new PviException(e.ErrorText, e.ErrorCode, this, e);
					throw ex;
				}
				if (Service.ErrorEvent && this.Error != null)
				{
					this.Error(this, e);
				}
				if (Service != null)
				{
					Service.OnError(this, e);
				}
			}
			else if (this.Error != null)
			{
				this.Error(this, e);
			}
		}

		protected internal void OnSingleError(PviEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(this, e);
			}
		}

		protected internal virtual void OnError(object sender, PviEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(sender, e);
			}
		}

		protected virtual void OnPropertyChanged(PviEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		public virtual int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			string text = "";
			text = reader.GetAttribute("Name");
			if (text != null && text.Length > 0)
			{
				baseObj.propName = text;
			}
			text = "";
			text = reader.GetAttribute("Address");
			if (text != null && text.Length > 0)
			{
				baseObj.propAddress = text;
			}
			text = "";
			text = reader.GetAttribute("UserData");
			if (text != null && text.Length > 0)
			{
				baseObj.propUserData = text;
			}
			text = "";
			text = reader.GetAttribute("LinkName");
			if (text != null && text.Length > 0)
			{
				baseObj.propLinkName = text;
			}
			text = "";
			text = reader.GetAttribute("ConnectionType");
			if (text != null && text.Length > 0 && (ConfigurationFlags.ConnectionState & flags) != 0)
			{
				switch (text.ToLower())
				{
				case "create":
					baseObj.propConnectionType = ConnectionType.Create;
					break;
				case "createandlink":
					baseObj.propConnectionType = ConnectionType.CreateAndLink;
					break;
				case "link":
					baseObj.propConnectionType = ConnectionType.Link;
					break;
				case "none":
					baseObj.propConnectionType = ConnectionType.None;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("ErrorCode");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					baseObj.propErrorCode = result;
				}
			}
			text = "";
			text = reader.GetAttribute("Connected");
			if (text != null && text.ToLower() == "true" && (ConfigurationFlags.ConnectionState & flags) != 0)
			{
				baseObj.propRequests |= Actions.Connect;
			}
			return 0;
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			if (propName != null && propName.Length > 0)
			{
				writer.WriteAttributeString("Name", propName);
			}
			if (propName != propAddress && propAddress != null && propAddress.Length > 0)
			{
				writer.WriteAttributeString("Address", propAddress);
			}
			if (propUserData is string && propUserData != null && ((string)propUserData).Length > 0)
			{
				writer.WriteAttributeString("UserData", propUserData.ToString());
			}
			if (propLinkName != null && propLinkName.Length > 0)
			{
				writer.WriteAttributeString("LinkName", propLinkName);
			}
			writer.WriteAttributeString("Connected", propConnectionState.ToString());
			if (propErrorCode > 0)
			{
				writer.WriteAttributeString("ErrorCode", propErrorCode.ToString());
			}
			if ((ConfigurationFlags.ConnectionState & flags) != 0 && ConnectionType != ConnectionType.CreateAndLink)
			{
				writer.WriteAttributeString("ConnectionType", ConnectionType.ToString());
			}
			return 0;
		}

		internal static uint MakeWindowMessage(uint pvisAction)
		{
			if (0 < pvisAction)
			{
				return pvisAction + 10000;
			}
			return 0u;
		}

		internal static uint MakeWindowMessage(Action pvisAction)
		{
			return MakeWindowMessage((uint)pvisAction);
		}

		protected virtual string GetObjectName()
		{
			if (propSNMPParent != null)
			{
				return propSNMPParent.FullName + "." + Name;
			}
			return LinkName;
		}

		internal virtual int PviLinkObject(uint action)
		{
			int num = 0;
			if (propLinkName == null || propLinkName.Length == 0)
			{
				propLinkName = GetObjectName();
			}
			if (!Service.IsStatic || ConnectionType == ConnectionType.Link)
			{
				return XLinkRequest(Service.hPvi, LinkName, 550u, propLinkParam, action);
			}
			return XLinkRequest(Service.hPvi, GetObjectName(), 550u, propLinkParam, action);
		}

		internal int Read_FormatEX(uint lnkID)
		{
			int result = 0;
			if (!propReadingFormat && propHasLinkObject)
			{
				propReadingFormat = true;
				result = ((!(propParent is Service)) ? ReadRequest(Service.hPvi, lnkID, AccessTypes.TypeExtern, 2810u) : ReadRequest(Service.hPvi, lnkID, AccessTypes.TypeIntern, 2810u));
			}
			return result;
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propErrorCode = errorCode;
			propLinkId = linkID;
			if (errorCode == 0 || 12002 == errorCode)
			{
				if (1 > linkID && Service.IsStatic && ConnectionType == ConnectionType.CreateAndLink)
				{
					propErrorCode = XLinkRequest(Service.hPvi, LinkName, 703u, propLinkParam, 704u);
					return;
				}
				propErrorCode = errorCode;
				if (ConnectionType == ConnectionType.Link)
				{
					OnConnected(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			else
			{
				OnError(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
			}
		}

		internal override void OnPviCancelled(int errorCode, int type)
		{
			propErrorCode = errorCode;
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			propErrorCode = errorCode;
			PviEventArgs e = new PviEventArgs(Name, Address, errorCode, Service.Language, Action.LinkObject, Service);
			OnConnected(e);
			if (errorCode == 0)
			{
				propLinkId = linkID;
				propConnectionState = ConnectionStates.Connected;
			}
			if (this.Linked != null)
			{
				this.Linked(this, e);
			}
			if (errorCode != 0)
			{
				OnError(e);
			}
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			switch (eventType)
			{
			case EventTypes.Error:
			case EventTypes.Data:
				if (ConnectionStates.Connected > propConnectionState || errorCode == 0)
				{
					OnConnected(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
				if (errorCode == 0 || 12002 == errorCode)
				{
					OnError(new PviEventArgs(Name, Address, 0, Service.Language, Action.ConnectedEvent, Service));
				}
				else
				{
					OnError(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
				break;
			case EventTypes.Connection:
				OnConnected(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				break;
			case EventTypes.Disconnect:
				OnDisconnected(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
				break;
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			propErrorCode = errorCode;
			if (PVIWriteAccessTypes.Connection == accessType)
			{
				OnConnectionChanged(errorCode, (Action)accessType);
			}
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
			propErrorCode = errorCode;
			if (ConnectionStates.Disconnecting == propConnectionState)
			{
				propLinkId = 0u;
				OnDisconnected(new PviEventArgs(Name, Address, errorCode, Service.Language, Action.DisconnectedEvent));
			}
		}

		internal override void OnPviDeleted(int errorCode)
		{
			propErrorCode = errorCode;
		}

		internal override void OnPviChangedLink(int errorCode)
		{
			propErrorCode = errorCode;
		}
	}
}
