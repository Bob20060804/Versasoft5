using System.Runtime.InteropServices;

namespace BR.AN
{
	internal delegate bool CBFindKeys(int pCBData, [MarshalAs(UnmanagedType.LPStr)] string keySerial, [MarshalAs(UnmanagedType.LPStr)] string keyPort, [MarshalAs(UnmanagedType.LPStr)] string dsID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos);
}
