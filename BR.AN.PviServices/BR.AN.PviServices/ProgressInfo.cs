using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[CLSCompliant(true)]
	public struct ProgressInfo
	{
		public int Access;

		public int Percent;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string Info;
	}
}
