using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionProcessorInfo
	{
		public uint pc;

		public uint eFlags;

		public uint excErrFrameCode;

		public ulong reserve;
	}
}
