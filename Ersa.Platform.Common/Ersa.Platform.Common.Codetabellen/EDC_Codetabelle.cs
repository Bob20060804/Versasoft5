using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Codetabellen
{
	public class EDC_Codetabelle
	{
		[JsonIgnore]
		public string PRO_strName
		{
			get;
			set;
		}

		[JsonProperty("CodeEntries")]
		public List<EDC_Codeeintrag> PRO_lstEintraege
		{
			get;
			set;
		}

		[JsonIgnore]
		public long PRO_i64Id
		{
			get;
			set;
		}
	}
}
