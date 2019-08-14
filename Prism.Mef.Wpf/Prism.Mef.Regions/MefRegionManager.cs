using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionManager))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefRegionManager : RegionManager
	{
	}
}
