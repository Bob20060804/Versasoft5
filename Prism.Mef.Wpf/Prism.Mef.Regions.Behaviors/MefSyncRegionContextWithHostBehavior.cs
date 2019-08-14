using Prism.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions.Behaviors
{
	[Export(typeof(SyncRegionContextWithHostBehavior))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefSyncRegionContextWithHostBehavior : SyncRegionContextWithHostBehavior
	{
	}
}
