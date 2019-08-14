using Prism.Properties;
using Prism.Regions.Behaviors;
using System;
using System.Globalization;
using System.Windows;

namespace Prism.Regions
{
	public abstract class RegionAdapterBase<T> : IRegionAdapter where T : class
	{
		protected IRegionBehaviorFactory RegionBehaviorFactory
		{
			get;
			set;
		}

		protected RegionAdapterBase(IRegionBehaviorFactory regionBehaviorFactory)
		{
			RegionBehaviorFactory = regionBehaviorFactory;
		}

		public IRegion Initialize(T regionTarget, string regionName)
		{
			if (regionName == null)
			{
				throw new ArgumentNullException("regionName");
			}
			IRegion region = CreateRegion();
			region.Name = regionName;
			SetObservableRegionOnHostingControl(region, regionTarget);
			Adapt(region, regionTarget);
			AttachBehaviors(region, regionTarget);
			AttachDefaultBehaviors(region, regionTarget);
			return region;
		}

		IRegion IRegionAdapter.Initialize(object regionTarget, string regionName)
		{
			return Initialize(GetCastedObject(regionTarget), regionName);
		}

		protected virtual void AttachDefaultBehaviors(IRegion region, T regionTarget)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			IRegionBehaviorFactory regionBehaviorFactory = RegionBehaviorFactory;
			if (regionBehaviorFactory != null)
			{
				DependencyObject dependencyObject = regionTarget as DependencyObject;
				foreach (string item in regionBehaviorFactory)
				{
					if (!region.Behaviors.ContainsKey(item))
					{
						IRegionBehavior regionBehavior = regionBehaviorFactory.CreateFromKey(item);
						if (dependencyObject != null)
						{
							IHostAwareRegionBehavior hostAwareRegionBehavior = regionBehavior as IHostAwareRegionBehavior;
							if (hostAwareRegionBehavior != null)
							{
								hostAwareRegionBehavior.HostControl = dependencyObject;
							}
						}
						region.Behaviors.Add(item, regionBehavior);
					}
				}
			}
		}

		protected virtual void AttachBehaviors(IRegion region, T regionTarget)
		{
		}

		protected abstract void Adapt(IRegion region, T regionTarget);

		protected abstract IRegion CreateRegion();

		private static T GetCastedObject(object regionTarget)
		{
			if (regionTarget == null)
			{
				throw new ArgumentNullException("regionTarget");
			}
			T val = regionTarget as T;
			if (val == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Prism.Properties.Resources.AdapterInvalidTypeException, new object[1]
				{
					typeof(T).Name
				}));
			}
			return val;
		}

		private static void SetObservableRegionOnHostingControl(IRegion region, T regionTarget)
		{
			DependencyObject dependencyObject = regionTarget as DependencyObject;
			if (dependencyObject != null)
			{
				RegionManager.GetObservableRegion(dependencyObject).Value = region;
			}
		}
	}
}
