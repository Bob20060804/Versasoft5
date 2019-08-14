using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(ItemsControlRegionAdapter))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefItemsControlRegionAdapter : ItemsControlRegionAdapter
	{
		[ImportingConstructor]
		public MefItemsControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}
	}
}
