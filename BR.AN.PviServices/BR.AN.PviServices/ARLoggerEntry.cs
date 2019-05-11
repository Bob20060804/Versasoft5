using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ARLoggerEntry
	{
		public uint recTagBegin;

		public uint index;

		public uint prevLen;

		public uint entLen;

		public uint lenOfBinData;

		public uint lenOfAsciiString;

		public uint infoFlags;

		public ARLogTime logTime;

		public uint logLevel;

		public uint errorNumber;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string taskName;

		public uint ulEventId;

		public uint ulOriginRecordId;
	}
}
