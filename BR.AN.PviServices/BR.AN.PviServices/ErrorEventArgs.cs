namespace BR.AN.PviServices
{
	public class ErrorEventArgs : PviEventArgs
	{
		private string propErrorInfo;

		public string ErrorInfo => propErrorInfo;

		public ErrorEventArgs(string name, string address, int errorCode, string language, Action action)
			: base(name, address, errorCode, language, action)
		{
			propErrorInfo = "";
		}

		public ErrorEventArgs(string name, string address, int errorCode, string language, Action action, string errorInfo)
			: base(name, address, errorCode, language, action)
		{
			propErrorInfo = errorInfo;
		}
	}
}
