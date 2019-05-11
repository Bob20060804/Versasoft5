using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	internal struct APIFC_TkInfoRes
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string name;

		public TaskClassType number;

		public ProgramState state;
	}
}
