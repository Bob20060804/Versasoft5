using System.Runtime.InteropServices;

namespace BR.AN.Logging
{
	[Guid("AA36BFF1-6187-43f9-8EDB-23ED6D7CF7B4")]
	[ComVisible(true)]
	public class ComLogger : IComLogger
	{
		public void Write(string uniqueLoggerName, string caller, string text)
		{
			Logger.GetLogger(uniqueLoggerName).Write(caller, text, LoggingType.Information);
		}
	}
}
