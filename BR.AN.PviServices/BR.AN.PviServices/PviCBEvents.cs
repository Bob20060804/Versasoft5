using System;

namespace BR.AN.PviServices
{
	public abstract class PviCBEvents : PInvokePvicom
	{
		internal uint _internId;

		internal virtual void OnPviCreated(int errorCode, uint linkID)
		{
		}

		internal virtual void OnPviLinked(int errorCode, uint linkID, int option)
		{
		}

		internal virtual void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
		}

		internal virtual void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
		}

		internal virtual void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
		}

		internal virtual void OnPviUnLinked(int errorCode, int option)
		{
		}

		internal virtual void OnPviDeleted(int errorCode)
		{
		}

		internal virtual void OnPviChangedLink(int errorCode)
		{
		}

		internal virtual void OnPviCancelled(int errorCode, int type)
		{
		}
	}
}
