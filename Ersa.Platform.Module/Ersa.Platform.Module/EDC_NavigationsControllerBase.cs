using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Ersa.Platform.Module
{
	public abstract class EDC_NavigationsControllerBase
	{
		[ImportMany(Source = ImportSource.Local)]
		public IEnumerable<Lazy<UserControl, INF_RegionContentMetadata>> PRO_enuViewsToRegionsMapping
		{
			get;
			set;
		}

		[Import]
		public IRegionManager PRO_edcRegionManager
		{
			get;
			set;
		}

		public void SUB_ViewsZuRegionsHinzufuegen()
		{
			foreach (Lazy<UserControl, INF_RegionContentMetadata> item in PRO_enuViewsToRegionsMapping)
			{
				Lazy<UserControl, INF_RegionContentMetadata> fdcTempMapping = item;
				if (fdcTempMapping.Metadata.PRO_blnSollInRegionAufgenommenWerden)
				{
					PRO_edcRegionManager.RegisterViewWithRegion(item.Metadata.PRO_strRegionName, () => fdcTempMapping.Value);
				}
			}
		}
	}
}
