using Prism.Common;
using System;
using System.Collections.Generic;

namespace Prism.Regions
{
	public class NavigationContext
	{
		public IRegionNavigationService NavigationService
		{
			get;
			private set;
		}

		public Uri Uri
		{
			get;
			private set;
		}

		public NavigationParameters Parameters
		{
			get;
			private set;
		}

		public NavigationContext(IRegionNavigationService navigationService, Uri uri)
			: this(navigationService, uri, null)
		{
		}

		public NavigationContext(IRegionNavigationService navigationService, Uri uri, NavigationParameters navigationParameters)
		{
			NavigationService = navigationService;
			Uri = uri;
			Parameters = ((uri != null) ? UriParsingHelper.ParseQuery(uri) : null);
			GetNavigationParameters(navigationParameters);
		}

		private void GetNavigationParameters(NavigationParameters navigationParameters)
		{
			if (Parameters == null || NavigationService == null || NavigationService.Region == null)
			{
				Parameters = new NavigationParameters();
			}
			else if (navigationParameters != null)
			{
				foreach (KeyValuePair<string, object> navigationParameter in navigationParameters)
				{
					Parameters.Add(navigationParameter.Key, navigationParameter.Value);
				}
			}
		}
	}
}
