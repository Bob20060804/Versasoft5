using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct APIFC_CPmemInfoRes
	{
		public ushort flags;

		public MemoryType type;

		public uint start_adr;

		public uint total_len;

		public uint free_len;

		public uint freeblk_len;
	}
}
