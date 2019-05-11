using System;
using System.Collections;

namespace BR.AN
{
	public class LCEventArgs : EventArgs
	{
		private int propErrorCode;

		private ArrayList propLicenseInfos;

		public int ErrorCode => propErrorCode;

		public ArrayList LicenseInfos => propLicenseInfos;

		public LCEventArgs()
		{
			Initialize(0);
		}

		internal LCEventArgs(int errorCode)
		{
			Initialize(errorCode);
		}

		internal LCEventArgs(ArrayList listOfLCInfos)
		{
			Initialize(0);
			propLicenseInfos = listOfLCInfos;
		}

		private void Initialize(int errorCode)
		{
			propErrorCode = errorCode;
			propLicenseInfos = new ArrayList(1);
		}
	}
}
