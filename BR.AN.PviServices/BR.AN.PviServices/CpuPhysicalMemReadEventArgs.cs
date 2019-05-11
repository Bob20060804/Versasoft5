using System;
using System.ComponentModel;

namespace BR.AN.PviServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public class CpuPhysicalMemReadEventArgs : PviEventArgs
	{
		private IntPtr propDataPtr;

		private int propDataLen;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int DataLength
		{
			get
			{
				return propDataLen;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IntPtr DataPtr
		{
			get
			{
				return propDataPtr;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public CpuPhysicalMemReadEventArgs(string name, string address, int error, string language, Action action, IntPtr pData, int dataLen)
			: base(name, address, error, language, action)
		{
			propDataPtr = pData;
			propDataLen = dataLen;
		}
	}
}
