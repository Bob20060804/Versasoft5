using Microsoft.Practices.ServiceLocation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Prism.Mef.Regions
{
	[Export(typeof(IRegionNavigationContentLoader))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MefRegionNavigationContentLoader : RegionNavigationContentLoader
	{
		[ImportingConstructor]
		public MefRegionNavigationContentLoader(IServiceLocator serviceLocator)
			: base(serviceLocator)
		{
		}

		protected override IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
		{
			if (candidateNavigationContract == null || candidateNavigationContract.Equals(string.Empty))
			{
				throw new ArgumentNullException("candidateNavigationContract");
			}
			IEnumerable<object> enumerable = base.GetCandidatesFromRegion(region, candidateNavigationContract);
			if (!enumerable.Any())
			{
				enumerable = from v in region.Views
				where candidateNavigationContract.Equals(v.GetType().GetCustomAttributes(inherit: false).OfType<ExportAttribute>()
					.FirstOrDefault()
					.ContractName, StringComparison.Ordinal)
					select v;
				}
				return enumerable;
			}
		}
	}
