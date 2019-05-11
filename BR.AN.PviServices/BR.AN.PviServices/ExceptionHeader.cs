using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionHeader
	{
		public uint traceRec;

		public uint excInfoSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string arVersion;

		public uint options;

		public uint reserve;
	}
}
