using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionTraceRecord
	{
		public uint brmInfoFlag;

		public uint paramCnt;

		public uint funcAddr;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string funcName;

		public ulong reserve;
	}
}
