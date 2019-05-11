using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ARMExceptionTaskInfo
	{
		public uint taskId;

		public uint taskPrio;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string taskName;

		public uint stackBottom;

		public uint stackEnd;

		public uint stackSize;

		public uint ulR0;

		public uint ulR1;

		public uint ulR2;

		public uint ulR3;

		public uint ulR4;

		public uint ulR5;

		public uint ulR6;

		public uint ulR7;

		public uint ulR8;

		public uint ulR9;

		public uint ulR10;

		public uint ulFp;

		public uint ulR12;

		public uint ulSp;

		public uint ulLr;

		public uint ulPc;

		public uint ulCpsr;

		public uint ulTtbase;

		public uint reserve1;

		public uint reserve2;
	}
}
