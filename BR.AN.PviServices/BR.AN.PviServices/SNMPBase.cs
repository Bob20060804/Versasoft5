using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class SNMPBase : PviCBEvents
	{
		internal uint propLinkID;

		protected Service propService;

		protected SNMPBase propParent;

		internal bool propIsConnected;

		internal uint propServiceArrayIndex;

		internal Actions propRequestQueue;

		private string propName;

		private object propData;

		public SNMPBase Parent => propParent;

		public Service Service => propService;

		public string Name => propName;

		public string FullName
		{
			get
			{
				if (propParent == null)
				{
					if (Service == null)
					{
						return propName;
					}
					return Service.Name + "." + propName;
				}
				return propParent.FullName + "." + propName;
			}
		}

		public object UserData
		{
			get
			{
				return propData;
			}
			set
			{
				propData = value;
			}
		}

		public event ErrorEventHandler SearchCompleted;

		public event ErrorEventHandler Error;

		internal SNMPBase(string name, Service serviceOBJ)
		{
			propData = null;
			propName = name;
			propLinkID = 0u;
			propRequestQueue = Actions.NONE;
			propIsConnected = false;
			propParent = null;
			propService = serviceOBJ;
			AddToCBReceivers();
		}

		internal SNMPBase(string name, SNMPBase parentOBJ)
		{
			propData = null;
			propName = name;
			propLinkID = 0u;
			propRequestQueue = Actions.NONE;
			propIsConnected = false;
			propParent = parentOBJ;
			propService = propParent.Service;
			AddToCBReceivers();
		}

		internal void InitConnect(Service serviceOBJ)
		{
			propService = serviceOBJ;
			if (Service != null && Service.ContainsIDKey(propServiceArrayIndex))
			{
				AddToCBReceivers();
			}
		}

		internal void InitConnect(SNMPBase parentOBJ)
		{
			propParent = parentOBJ;
			propService = propParent.Service;
			if (Service != null && Service.ContainsIDKey(propServiceArrayIndex))
			{
				AddToCBReceivers();
			}
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			return 0;
		}

		public virtual int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags)
		{
			return 0;
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propLinkID = linkID;
			if (errorCode == 0 || 12002 == errorCode)
			{
				propIsConnected = true;
			}
		}

		public virtual void Cleanup()
		{
			RemoveFromCBReceivers();
			CancelRequest();
			propIsConnected = false;
			propLinkID = 0u;
			propRequestQueue = Actions.NONE;
			propIsConnected = false;
			propParent = null;
			propService = null;
		}

		internal int CancelRequest()
		{
			int result = 0;
			if (propLinkID != 0 && Service != null)
			{
				int[] source = new int[1]
				{
					0
				};
				IntPtr hMemory = PviMarshal.AllocHGlobal(4);
				Marshal.Copy(source, 0, hMemory, 1);
				result = ((Service.EventMessageType != 0) ? PInvokePvicom.PviComMsgWriteRequest(Service.hPvi, propLinkID, AccessTypes.Cancel, hMemory, 4, IntPtr.Zero, 0u, 0u) : PInvokePvicom.PviComWriteRequest(Service.hPvi, propLinkID, AccessTypes.Cancel, hMemory, 4, null, 0u, 0u));
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			return result;
		}

		internal int DeleteRequest(string objName)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkID, null, 0u, 0u);
			}
			return PInvokePvicom.PviComUnlinkRequest(Service.hPvi, propLinkID, null, 0u, 0u);
		}

		internal int DeleteRequest()
		{
			return DeleteRequest(FullName);
		}

		internal int ConnectPviObject(bool withEvents, string objName, string objDesc, string lnkDesc, ObjectType objType, int action, out uint linkID)
		{
			int num = 0;
			if (!withEvents)
			{
				return PInvokePvicom.PviComCreate(Service.hPvi, out linkID, objName, objType, objDesc, Service.cbSNMP, 0u, 0u, lnkDesc);
			}
			return PInvokePvicom.PviComCreate(Service.hPvi, out linkID, objName, objType, objDesc, Service.cbSNMP, (uint)action, propServiceArrayIndex, lnkDesc);
		}

		internal int UnlinkPviObject(uint linkID)
		{
			if (Service != null && 0 < linkID)
			{
				return PInvokePvicom.PviComUnlink(Service.hPvi, linkID);
			}
			return 0;
		}

		internal virtual int Connect()
		{
			return -1;
		}

		public virtual int Disconnect()
		{
			return 12063;
		}

		public virtual int Disconnect(bool synchronous)
		{
			return 12063;
		}

		protected int GetSNMPVariables(int linkID, int action)
		{
			int num = 0;
			AccessTypes nAccess = AccessTypes.ListVariable;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComReadRequest(propService.hPvi, (uint)linkID, nAccess, Service.cbRead, 4294967294u, propServiceArrayIndex);
			}
			return PInvokePvicom.PviComMsgReadRequest(propService.hPvi, (uint)linkID, nAccess, propService.WindowHandle, (uint)action, propServiceArrayIndex);
		}

		protected virtual void OnSearchCompleted(ErrorEventArgs e)
		{
			if (this.SearchCompleted != null)
			{
				this.SearchCompleted(this, e);
			}
		}

		protected virtual void OnError(ErrorEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(this, e);
			}
		}

		private void RemoveFromCBReceivers()
		{
			if (Service != null)
			{
				Service.RemoveID(propServiceArrayIndex);
			}
		}

		private bool AddToCBReceivers()
		{
			if (Service != null)
			{
				return Service.AddID(this, ref propServiceArrayIndex);
			}
			return false;
		}
	}
}
