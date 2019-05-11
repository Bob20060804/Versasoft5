using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_RLogbookRes_entry
	{
		public ushort errcode;

		public uint errinfo;

		public char errtask_0;

		public char errtask_1;

		public char errtask_2;

		public char errtask_3;

		public APIFCerrtyp errtyp;

		public byte timestamp_0;

		public byte timestamp_1;

		public byte timestamp_2;

		public byte timestamp_3;

		public byte timestamp_4;

		public byte res;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string errstring;
	}
}
