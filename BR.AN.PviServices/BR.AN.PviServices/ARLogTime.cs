using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ARLogTime
	{
		public uint sec;

		public uint nanoSec;
	}
}
