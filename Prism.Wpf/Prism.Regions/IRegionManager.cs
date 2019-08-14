using System;

namespace Prism.Regions
{
	public interface IRegionManager
	{
		IRegionCollection Regions
		{
			get;
		}

		IRegionManager CreateRegionManager();

		IRegionManager AddToRegion(string regionName, object view);

		IRegionManager RegisterViewWithRegion(string regionName, Type viewType);

		IRegionManager RegisterViewWithRegion(string regionName, Func<object> getContentDelegate);

		void RequestNavigate(string regionName, Uri source, Action<NavigationResult> navigationCallback);

		void RequestNavigate(string regionName, Uri source);

		void RequestNavigate(string regionName, string source, Action<NavigationResult> navigationCallback);

		void RequestNavigate(string regionName, string source);

		void RequestNavigate(string regionName, Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters);

		void RequestNavigate(string regionName, string target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters);

		void RequestNavigate(string regionName, Uri target, NavigationParameters navigationParameters);

		void RequestNavigate(string regionName, string target, NavigationParameters navigationParameters);
	}
}
