using Prism.Common;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class RegionActiveAwareBehavior : IRegionBehavior
	{
		public const string BehaviorKey = "ActiveAware";

		public IRegion Region
		{
			get;
			set;
		}

		public void Attach()
		{
			INotifyCollectionChanged collection = GetCollection();
			if (collection != null)
			{
				collection.CollectionChanged += OnCollectionChanged;
			}
		}

		public void Detach()
		{
			INotifyCollectionChanged collection = GetCollection();
			if (collection != null)
			{
				collection.CollectionChanged -= OnCollectionChanged;
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (object newItem in e.NewItems)
				{
					Action<IActiveAware> action = delegate(IActiveAware activeAware)
					{
						activeAware.IsActive = true;
					};
					MvvmHelpers.ViewAndViewModelAction(newItem, action);
					InvokeOnSynchronizedActiveAwareChildren(newItem, action);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (object oldItem in e.OldItems)
				{
					Action<IActiveAware> action2 = delegate(IActiveAware activeAware)
					{
						activeAware.IsActive = false;
					};
					MvvmHelpers.ViewAndViewModelAction(oldItem, action2);
					InvokeOnSynchronizedActiveAwareChildren(oldItem, action2);
				}
			}
		}

		private void InvokeOnSynchronizedActiveAwareChildren(object item, Action<IActiveAware> invocation)
		{
			DependencyObject dependencyObject = item as DependencyObject;
			if (dependencyObject != null)
			{
				IRegionManager regionManager = RegionManager.GetRegionManager(dependencyObject);
				if (regionManager != null && regionManager != Region.RegionManager)
				{
					foreach (object item2 in regionManager.Regions.SelectMany((IRegion e) => e.ActiveViews).Where(ShouldSyncActiveState))
					{
						MvvmHelpers.ViewAndViewModelAction(item2, invocation);
					}
				}
			}
		}

		private bool ShouldSyncActiveState(object view)
		{
			if (Attribute.IsDefined(view.GetType(), typeof(SyncActiveStateAttribute)))
			{
				return true;
			}
			FrameworkElement frameworkElement = view as FrameworkElement;
			if (frameworkElement != null)
			{
				object dataContext = frameworkElement.DataContext;
				if (dataContext != null)
				{
					return Attribute.IsDefined(dataContext.GetType(), typeof(SyncActiveStateAttribute));
				}
				return false;
			}
			return false;
		}

		private INotifyCollectionChanged GetCollection()
		{
			return Region.ActiveViews;
		}
	}
}
