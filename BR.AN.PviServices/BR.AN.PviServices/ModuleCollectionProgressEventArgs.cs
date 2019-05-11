namespace BR.AN.PviServices
{
	public class ModuleCollectionProgressEventArgs : PviEventArgs
	{
		private Module module;

		private int percentage;

		public Module Module
		{
			get
			{
				return module;
			}
			set
			{
				module = value;
			}
		}

		public int Percentage => percentage;

		public ModuleCollectionProgressEventArgs(string name, string address, int error, string language, Action action, Module module, int percentage)
			: base(name, address, error, language, action)
		{
			this.module = module;
			this.percentage = percentage;
		}
	}
}
