using Ersa.Platform.Common.Meldungen;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ersa.Platform.Dienste.ZyklischeMeldungen
{
	public class EDC_ZyklischeMeldungVorlageGruppe
	{
		[JsonProperty("Gruppenname")]
		public string PRO_strName
		{
			get;
			set;
		}

		[JsonProperty("ZyklischeMeldungen")]
		public List<EDC_ZyklischeMeldungVorlage> PRO_lstVorlagen
		{
			get;
			set;
		}
	}
}
