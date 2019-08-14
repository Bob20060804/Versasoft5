using Prism.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class RegionMemberLifetimeBehavior : RegionBehavior
	{
		public const string BehaviorKey = "RegionMemberLifetimeBehavior";

		protected override void OnAttach()
		{
			base.Region.ActiveViews.CollectionChanged += OnActiveViewsChanged;
		}

		private void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (object oldItem in e.OldItems)
				{
					if (!ShouldKeepAlive(oldItem) && base.Region.Views.Contains(oldItem))
					{
						base.Region.Remove(oldItem);
					}
				}
			}
		}

		private static bool ShouldKeepAlive(object inactiveView)
		{
			return MvvmHelpers.GetImplementerFromViewOrViewModel<IRegionMemberLifetime>(inactiveView)?.KeepAlive ?? GetItemOrContextLifetimeAttribute(inactiveView)?.KeepAlive ?? true;
		}

		private static RegionMemberLifetimeAttribute GetItemOrContextLifetimeAttribute(object inactiveView)
		{
			RegionMemberLifetimeAttribute regionMemberLifetimeAttribute = GetCustomAttributes<RegionMemberLifetimeAttribute>(inactiveView.GetType()).FirstOrDefault();
			if (regionMemberLifetimeAttribute != null)
			{
				return regionMemberLifetimeAttribute;
			}
			FrameworkElement frameworkElement = inactiveView as FrameworkElement;
			if (frameworkElement != null && frameworkElement.DataContext != null)
			{
				return GetCustomAttributes<RegionMemberLifetimeAttribute>(frameworkElement.DataContext.GetType()).FirstOrDefault();
			}
			return null;
		}

		private static IEnumerable<T> GetCustomAttributes<T>(Type type)
		{
			return type.GetCustomAttributes(typeof(T), inherit: true).OfType<T>();
		}
	}
}
