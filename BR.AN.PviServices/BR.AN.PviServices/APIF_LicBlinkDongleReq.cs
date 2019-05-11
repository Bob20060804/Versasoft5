using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIF_LicBlinkDongleReq
	{
		internal ushort boxMask;

		internal uint serialNumber;

		internal uint colour;

		internal uint count;

		internal uint delay;
	}
}
