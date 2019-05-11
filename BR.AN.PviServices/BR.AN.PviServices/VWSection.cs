using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct VWSection
	{
		public uint lenOfLogData;

		public uint version;

		public uint wrap;

		public uint actIdx;

		public uint actOff;

		public uint writeOff;

		public uint refIdx;

		public uint refOff;

		public uint invLen;

		public short offsetUtc;

		public short daylightSaving;

		public ushort attributes;

		public ushort reserved;
	}
}
