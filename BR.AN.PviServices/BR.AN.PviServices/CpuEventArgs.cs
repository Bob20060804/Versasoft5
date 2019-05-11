using System;

namespace BR.AN.PviServices
{
	public class CpuEventArgs : PviEventArgs
	{
		private DateTime propDateTime;

		public DateTime DateTime => propDateTime;

		public CpuEventArgs(string name, string address, int error, string language, Action action, DateTime datetime)
			: base(name, address, error, language, action)
		{
			propDateTime = datetime;
		}
	}
}
