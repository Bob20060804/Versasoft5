using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(ClearChildViewsRegionBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefClearChildViewsRegionBehavior : ClearChildViewsRegionBehavior
	{
	}
}
