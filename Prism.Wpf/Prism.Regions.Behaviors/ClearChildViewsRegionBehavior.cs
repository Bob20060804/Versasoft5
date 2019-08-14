using System;
using System.ComponentModel;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class ClearChildViewsRegionBehavior : RegionBehavior
	{
		public const string BehaviorKey = "ClearChildViews";

		public static readonly DependencyProperty ClearChildViewsProperty = DependencyProperty.RegisterAttached("ClearChildViews", typeof(bool), typeof(ClearChildViewsRegionBehavior), new PropertyMetadata(false));

		public static bool GetClearChildViews(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return (bool)target.GetValue(ClearChildViewsProperty);
		}

		public static void SetClearChildViews(DependencyObject target, bool value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			target.SetValue(ClearChildViewsProperty, value);
		}

		protected override void OnAttach()
		{
			base.Region.PropertyChanged += Region_PropertyChanged;
		}

		private static void ClearChildViews(IRegion region)
		{
			foreach (object view in region.Views)
			{
				DependencyObject dependencyObject = view as DependencyObject;
				if (dependencyObject != null && GetClearChildViews(dependencyObject))
				{
					dependencyObject.ClearValue(RegionManager.RegionManagerProperty);
				}
			}
		}

		private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RegionManager" && base.Region.RegionManager == null)
			{
				ClearChildViews(base.Region);
			}
		}
	}
}
