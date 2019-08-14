using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Prism.Regions
{
	public class ViewsCollection : IViewsCollection, IEnumerable<object>, IEnumerable, INotifyCollectionChanged
	{
		private class MonitorInfo
		{
			public bool IsInList
			{
				get;
				set;
			}
		}

		private class RegionItemComparer : Comparer<object>
		{
			private readonly Comparison<object> comparer;

			public RegionItemComparer(Comparison<object> comparer)
			{
				this.comparer = comparer;
			}

			public override int Compare(object x, object y)
			{
				if (comparer == null)
				{
					return 0;
				}
				return comparer(x, y);
			}
		}

		private readonly ObservableCollection<ItemMetadata> subjectCollection;

		private readonly Dictionary<ItemMetadata, MonitorInfo> monitoredItems = new Dictionary<ItemMetadata, MonitorInfo>();

		private readonly Predicate<ItemMetadata> filter;

		private Comparison<object> sort;

		private List<object> filteredItems = new List<object>();

		public Comparison<object> SortComparison
		{
			get
			{
				return sort;
			}
			set
			{
				if (sort != value)
				{
					sort = value;
					UpdateFilteredItemsList();
					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
				}
			}
		}

		private IEnumerable<object> FilteredItems => filteredItems;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public ViewsCollection(ObservableCollection<ItemMetadata> list, Predicate<ItemMetadata> filter)
		{
			subjectCollection = list;
			this.filter = filter;
			MonitorAllMetadataItems();
			subjectCollection.CollectionChanged += SourceCollectionChanged;
			UpdateFilteredItemsList();
		}

		public bool Contains(object value)
		{
			return FilteredItems.Contains(value);
		}

		public IEnumerator<object> GetEnumerator()
		{
			return FilteredItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			this.CollectionChanged?.Invoke(this, e);
		}

		private void NotifyReset()
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void ResetAllMonitors()
		{
			RemoveAllMetadataMonitors();
			MonitorAllMetadataItems();
		}

		private void MonitorAllMetadataItems()
		{
			foreach (ItemMetadata item in subjectCollection)
			{
				AddMetadataMonitor(item, filter(item));
			}
		}

		private void RemoveAllMetadataMonitors()
		{
			foreach (KeyValuePair<ItemMetadata, MonitorInfo> monitoredItem in monitoredItems)
			{
				monitoredItem.Key.MetadataChanged -= OnItemMetadataChanged;
			}
			monitoredItems.Clear();
		}

		private void AddMetadataMonitor(ItemMetadata itemMetadata, bool isInList)
		{
			itemMetadata.MetadataChanged += OnItemMetadataChanged;
			monitoredItems.Add(itemMetadata, new MonitorInfo
			{
				IsInList = isInList
			});
		}

		private void RemoveMetadataMonitor(ItemMetadata itemMetadata)
		{
			itemMetadata.MetadataChanged -= OnItemMetadataChanged;
			monitoredItems.Remove(itemMetadata);
		}

		private void OnItemMetadataChanged(object sender, EventArgs e)
		{
			ItemMetadata itemMetadata = (ItemMetadata)sender;
			if (!monitoredItems.TryGetValue(itemMetadata, out MonitorInfo value))
			{
				return;
			}
			if (filter(itemMetadata))
			{
				if (!value.IsInList)
				{
					value.IsInList = true;
					UpdateFilteredItemsList();
					NotifyAdd(itemMetadata.Item);
				}
			}
			else
			{
				value.IsInList = false;
				RemoveFromFilteredList(itemMetadata.Item);
			}
		}

		private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				UpdateFilteredItemsList();
				foreach (ItemMetadata newItem in e.NewItems)
				{
					bool flag = filter(newItem);
					AddMetadataMonitor(newItem, flag);
					if (flag)
					{
						NotifyAdd(newItem.Item);
					}
				}
				if (sort != null)
				{
					NotifyReset();
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (ItemMetadata oldItem in e.OldItems)
				{
					RemoveMetadataMonitor(oldItem);
					if (filter(oldItem))
					{
						RemoveFromFilteredList(oldItem.Item);
					}
				}
				break;
			default:
				ResetAllMonitors();
				UpdateFilteredItemsList();
				NotifyReset();
				break;
			}
		}

		private void NotifyAdd(object item)
		{
			int newStartingIndex = filteredItems.IndexOf(item);
			NotifyAdd(new object[1]
			{
				item
			}, newStartingIndex);
		}

		private void RemoveFromFilteredList(object item)
		{
			int originalIndex = filteredItems.IndexOf(item);
			UpdateFilteredItemsList();
			NotifyRemove(new object[1]
			{
				item
			}, originalIndex);
		}

		private void UpdateFilteredItemsList()
		{
			filteredItems = (from i in subjectCollection
			where filter(i)
			select i.Item).OrderBy((object o) => o, new RegionItemComparer(SortComparison)).ToList();
		}

		private void NotifyAdd(IList items, int newStartingIndex)
		{
			if (items.Count > 0)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, newStartingIndex));
			}
		}

		private void NotifyRemove(IList items, int originalIndex)
		{
			if (items.Count > 0)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items, originalIndex));
			}
		}
	}
}
