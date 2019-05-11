using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class CpuTTServiceEventArgs : PviEventArgs
	{
		private ushort propTTGroup;

		private byte propTTServiceID;

		private byte[] propTTData;

		[Browsable(false)]
		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ushort TTGroup
		{
			get
			{
				return propTTGroup;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public byte TTServiceID
		{
			get
			{
				return propTTServiceID;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public byte[] TTData
		{
			get
			{
				return propTTData;
			}
		}

		[CLSCompliant(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public CpuTTServiceEventArgs(string name, string address, int error, string language, Action action, ushort ttGroup, byte ttServiceID, IntPtr pData, byte ttDataLen)
			: base(name, address, error, language, action)
		{
			propTTGroup = ttGroup;
			propTTServiceID = ttServiceID;
			propTTData = new byte[ttDataLen];
			if (0 < ttDataLen)
			{
				IntPtr source = new IntPtr(pData.ToInt64() + 4);
				Marshal.Copy(source, propTTData, 0, ttDataLen);
			}
		}
	}
}
