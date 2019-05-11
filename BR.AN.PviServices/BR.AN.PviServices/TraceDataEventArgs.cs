namespace BR.AN.PviServices
{
	public class TraceDataEventArgs : PviEventArgs
	{
		private TraceDataCollection propTraceData;

		public TraceDataCollection TraceData => propTraceData;

		internal TraceDataEventArgs(string name, string address, int error, string language, Action action, TraceDataCollection traceDataCol)
			: base(name, address, error, language, action)
		{
			propTraceData = traceDataCol;
		}
	}
}
