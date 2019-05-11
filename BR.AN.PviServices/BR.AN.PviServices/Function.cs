namespace BR.AN.PviServices
{
	public class Function
	{
		private string propName;

		private Library propLibrary;

		internal byte propReference;

		public string Name => propName;

		public Library Library => propLibrary;

		public byte Reference => propReference;

		public Function(Library library, string name)
		{
			propName = name;
			library.Functions.Add(name, this);
			propLibrary = library;
		}
	}
}
