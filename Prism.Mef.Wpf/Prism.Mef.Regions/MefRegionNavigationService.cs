using Microsoft.Practices.ServiceLocation;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionNavigationService))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class MefRegionNavigationService : RegionNavigationService
	{
		[ImportingConstructor]
		public MefRegionNavigationService(IServiceLocator serviceLocator, IRegionNavigationContentLoader navigationContentLoader, IRegionNavigationJournal journal)
			: base(serviceLocator, navigationContentLoader, journal)
		{
		}
	}
}
