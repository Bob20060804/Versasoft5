using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(RegionManagerRegistrationBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionManagerRegistrationBehavior : RegionManagerRegistrationBehavior
	{
	}
}
