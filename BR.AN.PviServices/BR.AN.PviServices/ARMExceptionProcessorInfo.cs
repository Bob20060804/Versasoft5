using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ARMExceptionProcessorInfo
	{
		public uint pc;

		public uint curPSR;

		public uint reserve1;

		public uint reserve2;

		public uint reserve3;
	}
}
