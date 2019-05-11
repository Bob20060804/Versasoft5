using System;

namespace BR.AN.PviServices
{
	internal delegate void PviReadResponseHandler(object sender, int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen);
}
