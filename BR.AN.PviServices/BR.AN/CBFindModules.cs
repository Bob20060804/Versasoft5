using System.Runtime.InteropServices;

namespace BR.AN
{
	internal delegate bool CBFindModules(int pCBData, [MarshalAs(UnmanagedType.LPStr)] string licName, [MarshalAs(UnmanagedType.LPStr)] string licInfo, [MarshalAs(UnmanagedType.LPStr)] string licID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos);
}
