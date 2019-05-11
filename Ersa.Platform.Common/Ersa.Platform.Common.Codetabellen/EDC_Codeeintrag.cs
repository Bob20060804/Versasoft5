using Newtonsoft.Json;

namespace Ersa.Platform.Common.Codetabellen
{
	public class EDC_Codeeintrag
	{
		[JsonProperty("Code")]
		public string PRO_strCode
		{
			get;
			set;
		}

		[JsonProperty("ProgramLibrary")]
		public string PRO_strBibliothek
		{
			get;
			set;
		}

		[JsonProperty("ProgramName")]
		public string PRO_strProgramm
		{
			get;
			set;
		}

		[JsonIgnore]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}
	}
}
