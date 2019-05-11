using System.Runtime.InteropServices;

namespace BR.AN
{
	[StructLayout(LayoutKind.Sequential, Size = 79)]
	public struct BRSECComponentEntry
	{
		public char RequiresBRIPC;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string OrderId;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
		public string LicenseText;
	}
}
