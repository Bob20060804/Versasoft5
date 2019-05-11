using System;

namespace BR.AN.PviServices
{
	public class SyncReadData
	{
		private IntPtr propPtrData;

		private int propDataLength;

		private IntPtr propPtrArgData;

		private int propArgDataLength;

		public IntPtr PtrData
		{
			get
			{
				return propPtrData;
			}
			set
			{
				propPtrData = value;
			}
		}

		public int DataLength
		{
			get
			{
				return propDataLength;
			}
			set
			{
				propDataLength = value;
			}
		}

		public IntPtr PtrArgData
		{
			get
			{
				return propPtrArgData;
			}
			set
			{
				propPtrArgData = value;
			}
		}

		public int ArgDataLength
		{
			get
			{
				return propArgDataLength;
			}
			set
			{
				propArgDataLength = value;
			}
		}

		public SyncReadData(int dataLength)
		{
			propPtrData = IntPtr.Zero;
			if (0 < dataLength)
			{
				propPtrData = PviMarshal.AllocHGlobal(dataLength);
			}
			propDataLength = dataLength;
			propPtrArgData = IntPtr.Zero;
			propArgDataLength = 0;
		}

		public void FreeBuffers()
		{
			PviMarshal.FreeHGlobal(ref propPtrData);
		}
	}
}
