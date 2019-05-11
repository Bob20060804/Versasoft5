using System;

namespace BR.AN.PviServices
{
	internal delegate void PviWriteResponseHandler(object sender, int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen);
}
