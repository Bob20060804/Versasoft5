using Newtonsoft.Json;
using System;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	public class EDC_FreigabeSchritt
	{
		[JsonProperty(PropertyName = "fromReleaseState")]
		public int PRO_i32VonReleaseState
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "toReleaseState")]
		public int PRO_i32NachReleaseState
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "comment")]
		public string PRO_strKommentar
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "user")]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime PRO_fdcDatum
		{
			get;
			set;
		}

		public ENUM_LoetprogrammFreigabeStatus PRO_enmVonReleaseState
		{
			get
			{
				return (ENUM_LoetprogrammFreigabeStatus)PRO_i32VonReleaseState;
			}
			set
			{
				PRO_i32VonReleaseState = (int)value;
			}
		}

		public ENUM_LoetprogrammFreigabeStatus PRO_enmNachReleaseStatus
		{
			get
			{
				return (ENUM_LoetprogrammFreigabeStatus)PRO_i32NachReleaseState;
			}
			set
			{
				PRO_i32NachReleaseState = (int)value;
			}
		}
	}
}
