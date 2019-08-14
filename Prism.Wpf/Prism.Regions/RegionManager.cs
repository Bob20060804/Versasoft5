using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Events;
using Prism.Properties;
using Prism.Regions.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace Prism.Regions
{
	public class RegionManager : IRegionManager
	{
		private class RegionCollection : IRegionCollection, IEnumerable<IRegion>, IEnumerable, INotifyCollectionChanged
		{
			private readonly IRegionManager regionManager;

			private readonly List<IRegion> regions;

			public IRegion this[string regionName]
			{
				get
				{
					UpdateRegions();
					IRegion regionByName = GetRegionByName(regionName);
					if (regionByName == null)
					{
						throw new KeyNotFoundException(string.Format(CultureInfo.CurrentUICulture, Prism.Properties.Resources.RegionNotInRegionManagerException, new object[1]
						{
							regionName
						}));
					}
					return regionByName;
				}
			}

			public event NotifyCollectionChangedEventHandler CollectionChanged;

			public RegionCollection(IRegionManager regionManager)
			{
				this.regionManager = regionManager;
				regions = new List<IRegion>();
			}

			public IEnumerator<IRegion> GetEnumerator()
			{
				UpdateRegions();
				return regions.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Add(IRegion region)
			{
				if (region == null)
				{
					throw new ArgumentNullException("region");
				}
				UpdateRegions();
				if (region.Name == null)
				{
					throw new InvalidOperationException(Prism.Properties.Resources.RegionNameCannotBeEmptyException);
				}
				if (GetRegionByName(region.Name) != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Prism.Properties.Resources.RegionNameExistsException, new object[1]
					{
						region.Name
					}));
				}
				regions.Add(region);
				region.RegionManager = regionManager;
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, region, 0));
			}

			public bool Remove(string regionName)
			{
				UpdateRegions();
				bool result = false;
				IRegion regionByName = GetRegionByName(regionName);
				if (regionByName != null)
				{
					result = true;
					regions.Remove(regionByName);
					regionByName.RegionManager = null;
					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, regionByName, 0));
				}
				return result;
			}

			public bool ContainsRegionWithName(string regionName)
			{
				UpdateRegions();
				return GetRegionByName(regionName) != null;
			}

			public void Add(string regionName, IRegion region)
			{
				if (region == null)
				{
					throw new ArgumentNullException("region");
				}
				if (region.Name != null && region.Name != regionName)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.RegionManagerWithDifferentNameException, new object[2]
					{
						region.Name,
						regionName
					}), "regionName");
				}
				if (region.Name == null)
				{
					region.Name = regionName;
				}
				Add(region);
			}

			private IRegion GetRegionByName(string regionName)
			{
				return regions.FirstOrDefault((IRegion r) => r.Name == regionName);
			}

			private void OnCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
			{
				this.CollectionChanged?.Invoke(this, notifyCollectionChangedEventArgs);
			}
		}

		private static readonly WeakDelegatesManager updatingRegionsListeners = new WeakDelegatesManager();

		public static readonly DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new PropertyMetadata(OnSetRegionNameCallback));

		private static readonly DependencyProperty ObservableRegionProperty = DependencyProperty.RegisterAttached("ObservableRegion", typeof(ObservableObject<IRegion>), typeof(RegionManager), null);

		public static readonly DependencyProperty RegionManagerProperty = DependencyProperty.RegisterAttached("RegionManager", typeof(IRegionManager), typeof(RegionManager), null);

		public static readonly DependencyProperty RegionContextProperty = DependencyProperty.RegisterAttached("RegionContext", typeof(object), typeof(RegionManager), new PropertyMetadata(OnRegionContextChanged));

		private readonly RegionCollection regionCollection;

		public IRegionCollection Regions => regionCollection;

		public static event EventHandler UpdatingRegions
		{
			add
			{
				updatingRegionsListeners.AddListener(value);
			}
			remove
			{
				updatingRegionsListeners.RemoveListener(value);
			}
		}

		public static void SetRegionName(DependencyObject regionTarget, string regionName)
		{
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			regionTarget.SetValue(RegionNameProperty, regionName);
		}

		public static string GetRegionName(DependencyObject regionTarget)
		{
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			return regionTarget.GetValue(RegionNameProperty) as string;
		}

		public static ObservableObject<IRegion> GetObservableRegion(DependencyObject view)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}
			ObservableObject<IRegion> observableObject = view.GetValue(ObservableRegionProperty) as ObservableObject<IRegion>;
			if (observableObject == null)
			{
				observableObject = new ObservableObject<IRegion>();
				view.SetValue(ObservableRegionProperty, observableObject);
			}
			return observableObject;
		}

		private static void OnSetRegionNameCallback(DependencyObject element, DependencyPropertyChangedEventArgs args)
		{
			if (!IsInDesignMode(element))
			{
				CreateRegion(element);
			}
		}

		private static void CreateRegion(DependencyObject element)
		{
			DelayedRegionCreationBehavior instance = ServiceLocator.Current.GetInstance<DelayedRegionCreationBehavior>();
			instance.TargetElement = element;
			instance.Attach();
		}

		public static IRegionManager GetRegionManager(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return (IRegionManager)target.GetValue(RegionManagerProperty);
		}

		public static void SetRegionManager(DependencyObject target, IRegionManager value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			target.SetValue(RegionManagerProperty, value);
		}

		private static void OnRegionContextChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
		{
			if (RegionContext.GetObservableContext(depObj).Value != e.NewValue)
			{
				RegionContext.GetObservableContext(depObj).Value = e.NewValue;
			}
		}

		public static object GetRegionContext(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return target.GetValue(RegionContextProperty);
		}

		public static void SetRegionContext(DependencyObject target, object value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			target.SetValue(RegionContextProperty, value);
		}

		public static void UpdateRegions()
		{
			try
			{
				updatingRegionsListeners.Raise(null, EventArgs.Empty);
			}
			catch (TargetInvocationException ex)
			{
				Exception rootException = ex.GetRootException();
				throw new UpdateRegionsException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.UpdateRegionException, new object[1]
				{
					rootException
				}), ex.InnerException);
			}
		}

		private static bool IsInDesignMode(DependencyObject element)
		{
			return DesignerProperties.GetIsInDesignMode(element);
		}

		public RegionManager()
		{
			regionCollection = new RegionCollection(this);
		}

		public IRegionManager CreateRegionManager()
		{
			return new RegionManager();
		}

		public IRegionManager AddToRegion(string regionName, object view)
		{
			if (!Regions.ContainsRegionWithName(regionName))
			{
				throw new ArgumentException(string.Format(Thread.CurrentThread.CurrentCulture, Prism.Properties.Resources.RegionNotFound, new object[1]
				{
					regionName
				}), "regionName");
			}
			return Regions[regionName].Add(view);
		}

		public IRegionManager RegisterViewWithRegion(string regionName, Type viewType)
		{
			ServiceLocator.Current.GetInstance<IRegionViewRegistry>().RegisterViewWithRegion(regionName, viewType);
			return this;
		}

		public IRegionManager RegisterViewWithRegion(string regionName, Func<object> getContentDelegate)
		{
			ServiceLocator.Current.GetInstance<IRegionViewRegistry>().RegisterViewWithRegion(regionName, getContentDelegate);
			return this;
		}

		public void RequestNavigate(string regionName, Uri source, Action<NavigationResult> navigationCallback)
		{
			if (navigationCallback == null)
			{
				throw new ArgumentNullException("navigationCallback");
			}
			if (Regions.ContainsRegionWithName(regionName))
			{
				Regions[regionName].RequestNavigate(source, navigationCallback);
			}
			else
			{
				navigationCallback(new NavigationResult(new NavigationContext(null, source), false));
			}
		}

		public void RequestNavigate(string regionName, Uri source)
		{
			RequestNavigate(regionName, source, delegate
			{
			});
		}

		public void RequestNavigate(string regionName, string source, Action<NavigationResult> navigationCallback)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			RequestNavigate(regionName, new Uri(source, UriKind.RelativeOrAbsolute), navigationCallback);
		}

		public void RequestNavigate(string regionName, string source)
		{
			RequestNavigate(regionName, source, delegate
			{
			});
		}

		public void RequestNavigate(string regionName, Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			if (navigationCallback == null)
			{
				throw new ArgumentNullException("navigationCallback");
			}
			if (Regions.ContainsRegionWithName(regionName))
			{
				Regions[regionName].RequestNavigate(target, navigationCallback, navigationParameters);
			}
			else
			{
				navigationCallback(new NavigationResult(new NavigationContext(null, target, navigationParameters), false));
			}
		}

		public void RequestNavigate(string regionName, string target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			RequestNavigate(regionName, new Uri(target, UriKind.RelativeOrAbsolute), navigationCallback, navigationParameters);
		}

		public void RequestNavigate(string regionName, Uri target, NavigationParameters navigationParameters)
		{
			RequestNavigate(regionName, target, delegate
			{
			}, navigationParameters);
		}

		public void RequestNavigate(string regionName, string target, NavigationParameters navigationParameters)
		{
			RequestNavigate(regionName, new Uri(target, UriKind.RelativeOrAbsolute), delegate
			{
			}, navigationParameters);
		}
	}
}
