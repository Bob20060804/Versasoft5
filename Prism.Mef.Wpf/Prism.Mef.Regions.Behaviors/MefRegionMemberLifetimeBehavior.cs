using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(RegionMemberLifetimeBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionMemberLifetimeBehavior : RegionMemberLifetimeBehavior
	{
		[ImportingConstructor]
		public MefRegionMemberLifetimeBehavior()
		{
		}
	}
}
