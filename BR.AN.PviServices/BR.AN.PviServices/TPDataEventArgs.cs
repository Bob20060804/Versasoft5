using System;

namespace BR.AN.PviServices
{
	public class TPDataEventArgs : ErrorEventArgs
	{
		private TracePointsDataCollection propTracePointsData;

		public TracePointsDataCollection TracePointsData => propTracePointsData;

		[CLSCompliant(false)]
		public TPDataEventArgs(string name, string address, int errorCode, string language, Action action, IntPtr pData, uint dataLen)
			: base(name, address, errorCode, language, action)
		{
			propTracePointsData = new TracePointsDataCollection();
			propTracePointsData.ReadResponseData(pData, dataLen);
		}
	}
}
