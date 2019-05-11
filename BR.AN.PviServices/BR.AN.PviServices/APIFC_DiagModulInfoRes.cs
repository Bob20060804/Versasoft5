using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_DiagModulInfoRes
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string name;

		public ushort dm_index;

		public uint address;

		public byte version;

		public byte revision;

		public uint erz_time;

		public uint and_time;

		public MemoryType mem_location;

		public byte dm_state;
	}
}
