using System;
using System.Windows;

namespace Prism.Common
{
	public static class MvvmHelpers
	{
		public static void ViewAndViewModelAction<T>(object view, Action<T> action) where T : class
		{
			T val = view as T;
			if (val != null)
			{
				action(val);
			}
			FrameworkElement frameworkElement = view as FrameworkElement;
			if (frameworkElement != null)
			{
				T val2 = frameworkElement.DataContext as T;
				if (val2 != null)
				{
					action(val2);
				}
			}
		}

		public static T GetImplementerFromViewOrViewModel<T>(object view) where T : class
		{
			T val = view as T;
			if (val != null)
			{
				return val;
			}
			FrameworkElement frameworkElement = view as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.DataContext as T;
			}
			return null;
		}
	}
}
