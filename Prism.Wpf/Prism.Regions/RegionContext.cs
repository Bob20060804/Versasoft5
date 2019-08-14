using Prism.Common;
using System;
using System.Windows;

namespace Prism.Regions
{
	public static class RegionContext
	{
		private static readonly DependencyProperty ObservableRegionContextProperty = DependencyProperty.RegisterAttached("ObservableRegionContext", typeof(ObservableObject<object>), typeof(RegionContext), null);

		public static ObservableObject<object> GetObservableContext(DependencyObject view)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}
			ObservableObject<object> observableObject = view.GetValue(ObservableRegionContextProperty) as ObservableObject<object>;
			if (observableObject == null)
			{
				observableObject = new ObservableObject<object>();
				view.SetValue(ObservableRegionContextProperty, observableObject);
			}
			return observableObject;
		}
	}
}
