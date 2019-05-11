namespace BR.AN.PviServices
{
	public class VariableCollectionEventArgs : CollectionEventArgs
	{
		private VariableCollection propVariables;

		public VariableCollection Variables => propVariables;

		public VariableCollectionEventArgs(string name, string address, int error, string language, Action action, VariableCollection variables)
			: base(name, address, error, language, action, variables)
		{
			propVariables = variables;
		}
	}
}
