namespace BR.AN.PviServices
{
	public class ProfilerEventArgs
	{
		private object propInfo;

		private string propName;

		private Action propAction;

		public object Info => propInfo;

		public string Name => propName;

		public Action Action => propAction;

		public ProfilerEventArgs(string name, Action action, object info)
		{
			propName = name;
			propAction = action;
			propInfo = info;
		}
	}
}
