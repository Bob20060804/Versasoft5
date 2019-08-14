using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(RegionActiveAwareBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionActiveAwareBehavior : RegionActiveAwareBehavior
	{
	}
}
