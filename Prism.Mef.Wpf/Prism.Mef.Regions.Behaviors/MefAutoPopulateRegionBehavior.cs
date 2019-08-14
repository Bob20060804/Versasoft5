using Prism.Regions;
using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(AutoPopulateRegionBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefAutoPopulateRegionBehavior : AutoPopulateRegionBehavior
	{
		[ImportingConstructor]
		public MefAutoPopulateRegionBehavior(IRegionViewRegistry regionViewRegistry)
			: base(regionViewRegistry)
		{
		}
	}
}
