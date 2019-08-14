using Microsoft.Practices.ServiceLocation;
using Prism.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Prism.Regions
{
	public class Region : IRegion, INavigateAsync, INotifyPropertyChanged
	{
		private ObservableCollection<ItemMetadata> itemMetadataCollection;

		private string name;

		private ViewsCollection views;

		private ViewsCollection activeViews;

		private object context;

		private IRegionManager regionManager;

		private IRegionNavigationService regionNavigationService;

		private Comparison<object> sort;

		public IRegionBehaviorCollection Behaviors
		{
			get;
			private set;
		}

		public object Context
		{
			get
			{
				return context;
			}
			set
			{
				if (context != value)
				{
					context = value;
					OnPropertyChanged("Context");
				}
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != null && name != value)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.CannotChangeRegionNameException, new object[1]
					{
						name
					}));
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(Prism.Properties.Resources.RegionNameCannotBeEmptyException);
				}
				name = value;
				OnPropertyChanged("Name");
			}
		}

		public virtual IViewsCollection Views
		{
			get
			{
				if (views == null)
				{
					views = new ViewsCollection(ItemMetadataCollection, (ItemMetadata x) => true);
					views.SortComparison = sort;
				}
				return views;
			}
		}

		public virtual IViewsCollection ActiveViews
		{
			get
			{
				if (activeViews == null)
				{
					activeViews = new ViewsCollection(ItemMetadataCollection, (ItemMetadata x) => x.IsActive);
					activeViews.SortComparison = sort;
				}
				return activeViews;
			}
		}

		public Comparison<object> SortComparison
		{
			get
			{
				return sort;
			}
			set
			{
				sort = value;
				if (activeViews != null)
				{
					activeViews.SortComparison = sort;
				}
				if (views != null)
				{
					views.SortComparison = sort;
				}
			}
		}

		public IRegionManager RegionManager
		{
			get
			{
				return regionManager;
			}
			set
			{
				if (regionManager != value)
				{
					regionManager = value;
					OnPropertyChanged("RegionManager");
				}
			}
		}

		public IRegionNavigationService NavigationService
		{
			get
			{
				if (regionNavigationService == null)
				{
					regionNavigationService = ServiceLocator.Current.GetInstance<IRegionNavigationService>();
					regionNavigationService.Region = this;
				}
				return regionNavigationService;
			}
			set
			{
				regionNavigationService = value;
			}
		}

		protected virtual ObservableCollection<ItemMetadata> ItemMetadataCollection
		{
			get
			{
				if (itemMetadataCollection == null)
				{
					itemMetadataCollection = new ObservableCollection<ItemMetadata>();
				}
				return itemMetadataCollection;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Region()
		{
			Behaviors = new RegionBehaviorCollection(this);
			sort = DefaultSortComparison;
		}

		public IRegionManager Add(object view)
		{
			return Add(view, null, createRegionManagerScope: false);
		}

		public IRegionManager Add(object view, string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StringCannotBeNullOrEmpty, new object[1]
				{
					"viewName"
				}));
			}
			return Add(view, viewName, createRegionManagerScope: false);
		}

		public virtual IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
		{
			IRegionManager regionManager = createRegionManagerScope ? RegionManager.CreateRegionManager() : RegionManager;
			InnerAdd(view, viewName, regionManager);
			return regionManager;
		}

		public virtual void Remove(object view)
		{
			ItemMetadata itemMetadataOrThrow = GetItemMetadataOrThrow(view);
			ItemMetadataCollection.Remove(itemMetadataOrThrow);
			DependencyObject dependencyObject = view as DependencyObject;
			if (dependencyObject != null && Prism.Regions.RegionManager.GetRegionManager(dependencyObject) == RegionManager)
			{
				dependencyObject.ClearValue(Prism.Regions.RegionManager.RegionManagerProperty);
			}
		}

		public void RemoveAll()
		{
			foreach (object view in Views)
			{
				Remove(view);
			}
		}

		public virtual void Activate(object view)
		{
			ItemMetadata itemMetadataOrThrow = GetItemMetadataOrThrow(view);
			if (!itemMetadataOrThrow.IsActive)
			{
				itemMetadataOrThrow.IsActive = true;
			}
		}

		public virtual void Deactivate(object view)
		{
			ItemMetadata itemMetadataOrThrow = GetItemMetadataOrThrow(view);
			if (itemMetadataOrThrow.IsActive)
			{
				itemMetadataOrThrow.IsActive = false;
			}
		}

		public virtual object GetView(string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.StringCannotBeNullOrEmpty, new object[1]
				{
					"viewName"
				}));
			}
			return ItemMetadataCollection.FirstOrDefault((ItemMetadata x) => x.Name == viewName)?.Item;
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
		{
			RequestNavigate(target, navigationCallback, null);
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			NavigationService.RequestNavigate(target, navigationCallback, navigationParameters);
		}

		private void InnerAdd(object view, string viewName, IRegionManager scopedRegionManager)
		{
			if (ItemMetadataCollection.FirstOrDefault((ItemMetadata x) => x.Item == view) != null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.RegionViewExistsException);
			}
			ItemMetadata itemMetadata = new ItemMetadata(view);
			if (!string.IsNullOrEmpty(viewName))
			{
				if (ItemMetadataCollection.FirstOrDefault((ItemMetadata x) => x.Name == viewName) != null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Prism.Properties.Resources.RegionViewNameExistsException, new object[1]
					{
						viewName
					}));
				}
				itemMetadata.Name = viewName;
			}
			DependencyObject dependencyObject = view as DependencyObject;
			if (dependencyObject != null)
			{
				Prism.Regions.RegionManager.SetRegionManager(dependencyObject, scopedRegionManager);
			}
			ItemMetadataCollection.Add(itemMetadata);
		}

		private ItemMetadata GetItemMetadataOrThrow(object view)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}
			ItemMetadata itemMetadata = ItemMetadataCollection.FirstOrDefault((ItemMetadata x) => x.Item == view);
			if (itemMetadata == null)
			{
				throw new ArgumentException(Prism.Properties.Resources.ViewNotInRegionException, "view");
			}
			return itemMetadata;
		}

		private void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static int DefaultSortComparison(object x, object y)
		{
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			Type type = x.GetType();
			Type type2 = y.GetType();
			ViewSortHintAttribute x2 = type.GetCustomAttributes(typeof(ViewSortHintAttribute), inherit: true).FirstOrDefault() as ViewSortHintAttribute;
			ViewSortHintAttribute y2 = type2.GetCustomAttributes(typeof(ViewSortHintAttribute), inherit: true).FirstOrDefault() as ViewSortHintAttribute;
			return ViewSortHintAttributeComparison(x2, y2);
		}

		private static int ViewSortHintAttributeComparison(ViewSortHintAttribute x, ViewSortHintAttribute y)
		{
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return string.Compare(x.Hint, y.Hint, StringComparison.Ordinal);
		}
	}
}
