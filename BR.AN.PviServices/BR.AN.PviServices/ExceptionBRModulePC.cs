using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionBRModulePC
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string brmName;

		public uint codeOffset;

		public ulong reserve;
	}
}
