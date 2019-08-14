using System;

namespace Prism.Regions
{
	public interface INavigateAsync
	{
		void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback);

		void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters);
	}
}
