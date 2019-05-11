using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIF_GetLicenseStatusRes
	{
		internal ushort errorcode;

		internal uint licStatus;
	}
}
