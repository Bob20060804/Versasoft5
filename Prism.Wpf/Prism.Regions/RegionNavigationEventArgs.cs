using System;

namespace Prism.Regions
{
	public class RegionNavigationEventArgs : EventArgs
	{
		public NavigationContext NavigationContext
		{
			get;
			private set;
		}

		public Uri Uri
		{
			get
			{
				if (NavigationContext != null)
				{
					return NavigationContext.Uri;
				}
				return null;
			}
		}

		public RegionNavigationEventArgs(NavigationContext navigationContext)
		{
			if (navigationContext == null)
			{
				throw new ArgumentNullException("navigationContext");
			}
			NavigationContext = navigationContext;
		}
	}
}
