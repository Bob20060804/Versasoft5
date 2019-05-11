namespace BR.AN.PviServices
{
	public class LoggerCollectionEventArgs : CollectionEventArgs
	{
		private LoggerCollection propLoggers;

		public LoggerCollection Loggers => propLoggers;

		public LoggerCollectionEventArgs(string name, string address, int error, string language, Action action, LoggerCollection loggerMods)
			: base(name, address, error, language, action, loggerMods)
		{
			propLoggers = loggerMods;
		}
	}
}
