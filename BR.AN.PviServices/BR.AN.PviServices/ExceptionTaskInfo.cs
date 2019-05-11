using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ExceptionTaskInfo
	{
		public uint taskId;

		public uint taskPrio;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
		public string taskName;

		public uint stackBottom;

		public uint stackEnd;

		public uint stackSize;

		public uint eax;

		public uint ebx;

		public uint ecx;

		public uint edx;

		public uint esi;

		public uint edi;

		public uint eip;

		public uint esp;

		public uint ebp;

		public uint eflags;

		public ulong reserve;
	}
}
