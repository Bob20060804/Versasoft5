#define TRACE
using System.Diagnostics;

namespace BR.AN.PviServices
{
	public class TraceWriter
	{
		public void WriteLine(string context, string logMessage)
		{
			Trace.WriteLine(logMessage, context);
		}

		public void WriteLine(string logMessage)
		{
			Trace.WriteLine(logMessage);
		}
	}
}
