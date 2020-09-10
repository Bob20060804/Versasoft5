using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class EDC_AuthentifizierungsProviderExportMetadaten : ExportAttribute
	{
		public string PRO_strAuthentifizierungsProvider
		{
			get;
			set;
		}

		public EDC_AuthentifizierungsProviderExportMetadaten()
			: base(typeof(INF_AuthentifizierungsProvider))
		{
		}
	}
}
