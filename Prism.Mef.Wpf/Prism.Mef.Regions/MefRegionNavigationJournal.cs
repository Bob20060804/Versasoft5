using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionNavigationJournal))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionNavigationJournal : RegionNavigationJournal
	{
	}
}
