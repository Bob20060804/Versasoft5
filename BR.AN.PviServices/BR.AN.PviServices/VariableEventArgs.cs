namespace BR.AN.PviServices
{
	public class VariableEventArgs : PviEventArgs
	{
		private string[] changedMembers;

		public string[] ChangedMembers => changedMembers;

		public VariableEventArgs(string name, string address, int error, string language, Action action, string[] changedMembers)
			: base(name, address, error, language, action)
		{
			this.changedMembers = changedMembers;
		}
	}
}
