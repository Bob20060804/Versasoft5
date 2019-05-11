using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionMemoryInfo
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] memPc;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
		public byte[] memEsp;

		public ulong reserve;
	}
}
