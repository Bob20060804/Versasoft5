using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(SelectorRegionAdapter))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefSelectorRegionAdapter : SelectorRegionAdapter
	{
		[ImportingConstructor]
		public MefSelectorRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}
	}
}
