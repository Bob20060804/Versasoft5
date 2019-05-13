using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Ersa.Platform.Module
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class EDC_RegionContentAttribute : ExportAttribute, INF_RegionContentMetadata
	{
		protected Func<bool> m_delSollInRegionAufgenommenWerden;

		public string PRO_strRegionName
		{
			get;
			private set;
		}

		public bool PRO_blnSollInRegionAufgenommenWerden
		{
			get
			{
				if (m_delSollInRegionAufgenommenWerden != null)
				{
					return m_delSollInRegionAufgenommenWerden();
				}
				return true;
			}
		}

		public EDC_RegionContentAttribute(string i_strRegionName)
			: base(typeof(UserControl))
		{
			PRO_strRegionName = i_strRegionName;
		}
	}
}
