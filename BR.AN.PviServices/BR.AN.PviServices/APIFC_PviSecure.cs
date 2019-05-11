using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_PviSecure
	{
		public int first;

		public int second;
	}
}
