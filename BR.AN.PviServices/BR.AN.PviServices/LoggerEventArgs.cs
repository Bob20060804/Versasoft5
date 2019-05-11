namespace BR.AN.PviServices
{
	public class LoggerEventArgs : PviEventArgs
	{
		private LoggerEntryCollection entries;

		public LoggerEntryCollection Entries => entries;

		public LoggerEventArgs(string name, string address, int error, string language, Action action, LoggerEntryCollection entries)
			: base(name, address, error, language, action)
		{
			this.entries = entries;
		}

		public LoggerEventArgs(LoggerEventArgs e, Action action)
			: base(e.propName, e.propAddress, e.propErrorCode, e.propLanguage, action)
		{
			entries = e.Entries;
		}
	}
}
