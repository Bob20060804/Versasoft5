using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(BindRegionContextToDependencyObjectBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefBindRegionContextToDependencyObjectBehavior : BindRegionContextToDependencyObjectBehavior
	{
	}
}
