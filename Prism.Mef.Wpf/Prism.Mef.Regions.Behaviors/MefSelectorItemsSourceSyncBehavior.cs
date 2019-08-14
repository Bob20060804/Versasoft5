using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(SelectorItemsSourceSyncBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefSelectorItemsSourceSyncBehavior : SelectorItemsSourceSyncBehavior
	{
	}
}
