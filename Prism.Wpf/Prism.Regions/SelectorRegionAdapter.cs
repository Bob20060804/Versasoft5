using Prism.Regions.Behaviors;
using System;
using System.Windows.Controls.Primitives;

namespace Prism.Regions
{
	public class SelectorRegionAdapter : RegionAdapterBase<Selector>
	{
		public SelectorRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, Selector regionTarget)
		{
		}

		protected override void AttachBehaviors(IRegion region, Selector regionTarget)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			region.Behaviors.Add(SelectorItemsSourceSyncBehavior.BehaviorKey, new SelectorItemsSourceSyncBehavior
			{
				HostControl = regionTarget
			});
			base.AttachBehaviors(region, regionTarget);
		}

		protected override IRegion CreateRegion()
		{
			return new Region();
		}
	}
}
