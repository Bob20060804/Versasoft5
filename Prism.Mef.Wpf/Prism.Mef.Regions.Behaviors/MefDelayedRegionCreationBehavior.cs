using Prism.Regions;
using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(DelayedRegionCreationBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefDelayedRegionCreationBehavior : DelayedRegionCreationBehavior
	{
		[ImportingConstructor]
		public MefDelayedRegionCreationBehavior(RegionAdapterMappings regionAdapterMappings)
			: base(regionAdapterMappings)
		{
		}
	}
}
