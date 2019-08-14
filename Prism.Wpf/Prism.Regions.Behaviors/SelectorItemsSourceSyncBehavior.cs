using Prism.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Prism.Regions.Behaviors
{
	public class SelectorItemsSourceSyncBehavior : RegionBehavior, IHostAwareRegionBehavior, IRegionBehavior
	{
		public static readonly string BehaviorKey = "SelectorItemsSourceSyncBehavior";

		private bool updatingActiveViewsInHostControlSelectionChanged;

		private Selector hostControl;

		public DependencyObject HostControl
		{
			get
			{
				return hostControl;
			}
			set
			{
				hostControl = (value as Selector);
			}
		}

		protected override void OnAttach()
		{
			if (hostControl.ItemsSource != null || BindingOperations.GetBinding(hostControl, ItemsControl.ItemsSourceProperty) != null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.ItemsControlHasItemsSourceException);
			}
			SynchronizeItems();
			hostControl.SelectionChanged += HostControlSelectionChanged;
			base.Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
			base.Region.Views.CollectionChanged += Views_CollectionChanged;
		}

		private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				int num = e.NewStartingIndex;
				foreach (object newItem in e.NewItems)
				{
					hostControl.Items.Insert(num++, newItem);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (object oldItem in e.OldItems)
				{
					hostControl.Items.Remove(oldItem);
				}
			}
		}

		private void SynchronizeItems()
		{
			List<object> list = new List<object>();
			foreach (object item in (IEnumerable)hostControl.Items)
			{
				list.Add(item);
			}
			foreach (object view in base.Region.Views)
			{
				hostControl.Items.Add(view);
			}
			foreach (object item2 in list)
			{
				base.Region.Add(item2);
			}
		}

		private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (updatingActiveViewsInHostControlSelectionChanged)
			{
				return;
			}
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (hostControl.SelectedItem != null && hostControl.SelectedItem != e.NewItems[0] && base.Region.ActiveViews.Contains(hostControl.SelectedItem))
				{
					base.Region.Deactivate(hostControl.SelectedItem);
				}
				hostControl.SelectedItem = e.NewItems[0];
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Contains(hostControl.SelectedItem))
			{
				hostControl.SelectedItem = null;
			}
		}

		private void HostControlSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				updatingActiveViewsInHostControlSelectionChanged = true;
				if (e.OriginalSource == sender)
				{
					foreach (object removedItem in e.RemovedItems)
					{
						if (base.Region.Views.Contains(removedItem) && base.Region.ActiveViews.Contains(removedItem))
						{
							base.Region.Deactivate(removedItem);
						}
					}
					foreach (object addedItem in e.AddedItems)
					{
						if (base.Region.Views.Contains(addedItem) && !base.Region.ActiveViews.Contains(addedItem))
						{
							base.Region.Activate(addedItem);
						}
					}
				}
			}
			finally
			{
				updatingActiveViewsInHostControlSelectionChanged = false;
			}
		}
	}
}
