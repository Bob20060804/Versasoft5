using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(ContentControlRegionAdapter))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefContentControlRegionAdapter : ContentControlRegionAdapter
	{
		[ImportingConstructor]
		public MefContentControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}
	}
}
