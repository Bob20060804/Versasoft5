using System;

namespace Prism.Regions
{
	public static class NavigationAsyncExtensions
	{
		public static void RequestNavigate(this INavigateAsync navigation, string target)
		{
			navigation.RequestNavigate(target, delegate
			{
			});
		}

		public static void RequestNavigate(this INavigateAsync navigation, string target, Action<NavigationResult> navigationCallback)
		{
			if (navigation == null)
			{
				throw new ArgumentNullException("navigation");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			Uri target2 = new Uri(target, UriKind.RelativeOrAbsolute);
			navigation.RequestNavigate(target2, navigationCallback);
		}

		public static void RequestNavigate(this INavigateAsync navigation, Uri target)
		{
			if (navigation == null)
			{
				throw new ArgumentNullException("navigation");
			}
			navigation.RequestNavigate(target, delegate
			{
			});
		}

		public static void RequestNavigate(this INavigateAsync navigation, string target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			if (navigation == null)
			{
				throw new ArgumentNullException("navigation");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			Uri target2 = new Uri(target, UriKind.RelativeOrAbsolute);
			navigation.RequestNavigate(target2, navigationCallback, navigationParameters);
		}

		public static void RequestNavigate(this INavigateAsync navigation, Uri target, NavigationParameters navigationParameters)
		{
			if (navigation == null)
			{
				throw new ArgumentNullException("navigation");
			}
			navigation.RequestNavigate(target, delegate
			{
			}, navigationParameters);
		}

		public static void RequestNavigate(this INavigateAsync navigation, string target, NavigationParameters navigationParameters)
		{
			if (navigation == null)
			{
				throw new ArgumentNullException("navigation");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			navigation.RequestNavigate(new Uri(target, UriKind.RelativeOrAbsolute), delegate
			{
			}, navigationParameters);
		}
	}
}
