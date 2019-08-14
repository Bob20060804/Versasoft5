using Prism.Properties;
using System;
using System.Globalization;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class DelayedRegionCreationBehavior
	{
		private readonly RegionAdapterMappings regionAdapterMappings;

		private WeakReference elementWeakReference;

		private bool regionCreated;

		public IRegionManagerAccessor RegionManagerAccessor
		{
			get;
			set;
		}

		public DependencyObject TargetElement
		{
			get
			{
				if (elementWeakReference == null)
				{
					return null;
				}
				return elementWeakReference.Target as DependencyObject;
			}
			set
			{
				elementWeakReference = new WeakReference(value);
			}
		}

		public DelayedRegionCreationBehavior(RegionAdapterMappings regionAdapterMappings)
		{
			this.regionAdapterMappings = regionAdapterMappings;
			RegionManagerAccessor = new DefaultRegionManagerAccessor();
		}

		public void Attach()
		{
			RegionManagerAccessor.UpdatingRegions += OnUpdatingRegions;
			WireUpTargetElement();
		}

		public void Detach()
		{
			RegionManagerAccessor.UpdatingRegions -= OnUpdatingRegions;
			UnWireTargetElement();
		}

		public void OnUpdatingRegions(object sender, EventArgs e)
		{
			TryCreateRegion();
		}

		private void TryCreateRegion()
		{
			DependencyObject targetElement = TargetElement;
			if (targetElement == null)
			{
				Detach();
			}
			else if (targetElement.CheckAccess())
			{
				Detach();
				if (!regionCreated)
				{
					string regionName = RegionManagerAccessor.GetRegionName(targetElement);
					CreateRegion(targetElement, regionName);
					regionCreated = true;
				}
			}
		}

		protected virtual IRegion CreateRegion(DependencyObject targetElement, string regionName)
		{
			if (targetElement == null)
			{
				throw new ArgumentNullException("targetElement");
			}
			try
			{
				return regionAdapterMappings.GetMapping(targetElement.GetType()).Initialize(targetElement, regionName);
			}
			catch (Exception ex)
			{
				throw new RegionCreationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.RegionCreationException, new object[2]
				{
					regionName,
					ex
				}), ex);
			}
		}

		private void ElementLoaded(object sender, RoutedEventArgs e)
		{
			UnWireTargetElement();
			TryCreateRegion();
		}

		private void WireUpTargetElement()
		{
			FrameworkElement frameworkElement = TargetElement as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Loaded += ElementLoaded;
			}
		}

		private void UnWireTargetElement()
		{
			FrameworkElement frameworkElement = TargetElement as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Loaded -= ElementLoaded;
			}
		}
	}
}
