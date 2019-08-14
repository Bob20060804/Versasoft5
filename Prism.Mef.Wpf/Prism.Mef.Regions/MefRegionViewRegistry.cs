using Microsoft.Practices.ServiceLocation;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionViewRegistry))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefRegionViewRegistry : RegionViewRegistry
	{
		[ImportingConstructor]
		public MefRegionViewRegistry(IServiceLocator serviceLocator)
			: base(serviceLocator)
		{
		}
	}
}
