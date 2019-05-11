using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_CpuInfoRes
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string sw_version;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string cpu_name;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string cpu_typ;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string aws_typ;

		public ushort node_nr;

		public int init_descr;

		public int state;

		public byte voltage;
	}
}
