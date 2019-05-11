namespace BR.AN.PviServices
{
	public class CollectionEventArgs : PviEventArgs
	{
		private BaseCollection objects;

		public BaseCollection Objects => objects;

		public CollectionEventArgs(string name, string address, int error, string language, Action action, BaseCollection objects)
			: base(name, address, error, language, action)
		{
			this.objects = objects;
		}
	}
}
