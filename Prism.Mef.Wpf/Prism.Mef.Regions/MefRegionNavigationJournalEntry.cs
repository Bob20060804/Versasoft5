using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionNavigationJournalEntry))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionNavigationJournalEntry : RegionNavigationJournalEntry
	{
	}
}
