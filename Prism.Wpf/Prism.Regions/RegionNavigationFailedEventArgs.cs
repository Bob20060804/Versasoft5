using System;

namespace Prism.Regions
{
	public class RegionNavigationFailedEventArgs : EventArgs
	{
		public NavigationContext NavigationContext
		{
			get;
			private set;
		}

		public Exception Error
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

		public RegionNavigationFailedEventArgs(NavigationContext navigationContext)
		{
			if (navigationContext == null)
			{
				throw new ArgumentNullException("navigationContext");
			}
			NavigationContext = navigationContext;
		}

		public RegionNavigationFailedEventArgs(NavigationContext navigationContext, Exception error)
			: this(navigationContext)
		{
			Error = error;
		}
	}
}
