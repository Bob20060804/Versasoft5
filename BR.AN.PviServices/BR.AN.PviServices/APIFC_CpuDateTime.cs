using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_CpuDateTime
	{
		public int sec;

		public int min;

		public int hour;

		public int mday;

		public int mon;

		public int year;

		public int wday;

		public int yday;

		public int isdst;
	}
}
