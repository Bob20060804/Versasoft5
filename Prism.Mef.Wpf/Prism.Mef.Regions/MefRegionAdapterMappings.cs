using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(RegionAdapterMappings))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefRegionAdapterMappings : RegionAdapterMappings
	{
	}
}
