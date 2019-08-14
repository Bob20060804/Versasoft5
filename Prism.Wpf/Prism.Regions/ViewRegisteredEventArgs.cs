using System;

namespace Prism.Regions
{
	public class ViewRegisteredEventArgs : EventArgs
	{
		public string RegionName
		{
			get;
			private set;
		}

		public Func<object> GetView
		{
			get;
			private set;
		}

		public ViewRegisteredEventArgs(string regionName, Func<object> getViewDelegate)
		{
			GetView = getViewDelegate;
			RegionName = regionName;
		}
	}
}
