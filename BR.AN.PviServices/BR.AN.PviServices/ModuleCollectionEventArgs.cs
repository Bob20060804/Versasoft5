namespace BR.AN.PviServices
{
	public class ModuleCollectionEventArgs : CollectionEventArgs
	{
		private ModuleCollection propModules;

		public ModuleCollection Modules => propModules;

		public ModuleCollectionEventArgs(string name, string address, int error, string language, Action action, ModuleCollection modules)
			: base(name, address, error, language, action, modules)
		{
			propModules = modules;
		}
	}
}
