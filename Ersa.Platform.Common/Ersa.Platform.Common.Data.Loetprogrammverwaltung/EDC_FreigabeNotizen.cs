using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	public class EDC_FreigabeNotizen
	{
		private const string Version = "0.0.1";

		[JsonProperty(PropertyName = "version")]
		public string PRO_strVersionId
		{
			get
			{
				return "0.0.1";
			}
		}

		[JsonProperty(PropertyName = "releaseTyp")]
		public int PRO_i32Freigabeart
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "releaseSteps")]
		public List<EDC_FreigabeSchritt> PRO_lstFreigabeSchritte
		{
			get;
			set;
		}

		public ENUM_LoetprogrammFreigabeArt PRO_enmFreigabeart
		{
			get
			{
				return (ENUM_LoetprogrammFreigabeArt)PRO_i32Freigabeart;
			}
			set
			{
				PRO_i32Freigabeart = (int)value;
			}
		}
	}
}
