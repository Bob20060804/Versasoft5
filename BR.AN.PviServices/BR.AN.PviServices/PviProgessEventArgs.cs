namespace BR.AN.PviServices
{
	public class PviProgessEventArgs : PviEventArgs
	{
		private int percentage;

		public int Percentage => percentage;

		public PviProgessEventArgs(string name, string address, int error, string language, Action action, int percentage)
			: base(name, address, error, language, action)
		{
			this.percentage = percentage;
		}
	}
}
