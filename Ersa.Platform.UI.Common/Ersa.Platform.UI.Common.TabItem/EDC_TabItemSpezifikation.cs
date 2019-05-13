using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.UI.Common.ViewModels;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Common.TabItem
{
	public class EDC_TabItemSpezifikation
	{
		public string PRO_strNameKey
		{
			get;
			set;
		}

		public string PRO_strRecht
		{
			get;
			set;
		}

		public string PRO_strRechtNameKey
		{
			get;
			set;
		}

		public object PRO_objTabView
		{
			get;
			set;
		}

		public int PRO_i32Reihenfolge
		{
			get;
			set;
		}

		public List<ENUM_SoftwareFeatures> PRO_lstSoftwareFeatures
		{
			get;
			set;
		}

		public EDC_NavigationsViewModelBasis PRO_edcNavigationsViewModel
		{
			get;
			set;
		}

		public bool FUN_blnPruefeSoftwareFeature(ENUM_SoftwareFeatures i_enmFeature)
		{
			if (PRO_lstSoftwareFeatures == null)
			{
				return false;
			}
			return PRO_lstSoftwareFeatures.Contains(i_enmFeature);
		}
	}
}
