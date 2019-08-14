using Microsoft.Practices.ServiceLocation;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionBehaviorFactory))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefRegionBehaviorFactory : RegionBehaviorFactory
	{
		[ImportingConstructor]
		public MefRegionBehaviorFactory(IServiceLocator serviceLocator)
			: base(serviceLocator)
		{
		}
	}
}
