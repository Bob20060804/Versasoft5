using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ResponseInfo
	{
		[CLSCompliant(false)]
		public uint LinkId;

		public int Mode;

		public int Type;

		public int Error;

		public int Status;

		public ResponseInfo(int linkID, int mode, int type, int error, int status)
		{
			LinkId = (uint)linkID;
			Mode = mode;
			Type = type;
			Error = error;
			Status = status;
		}
	}
}
