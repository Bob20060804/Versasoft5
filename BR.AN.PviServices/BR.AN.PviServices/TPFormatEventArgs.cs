using System;

namespace BR.AN.PviServices
{
	public class TPFormatEventArgs : ErrorEventArgs
	{
		private TracePointFormatCollection propTracePointsFormat;

		public TracePointFormatCollection TracePointsFormat => propTracePointsFormat;

		[CLSCompliant(false)]
		public TPFormatEventArgs(string name, string address, int errorCode, string language, Action action, IntPtr pData, uint dataLen)
			: base(name, address, errorCode, language, action)
		{
			propTracePointsFormat = new TracePointFormatCollection();
			propTracePointsFormat.ReadResponseData(pData, dataLen);
		}
	}
}
