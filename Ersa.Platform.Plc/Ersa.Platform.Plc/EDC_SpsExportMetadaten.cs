using System;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Plc
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class EDC_SpsExportMetadaten : ExportAttribute
	{
		public string PRO_strSpsTyp
		{
			get;
			set;
		}

		public EDC_SpsExportMetadaten()
			: base(typeof(INF_Sps))
		{
		}
	}
}
