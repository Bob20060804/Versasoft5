namespace BR.AN.Logging
{
	public class DbgKey
	{
		private string key;

		public string Name
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public DbgKey(string s)
		{
			key = s;
		}
	}
}
