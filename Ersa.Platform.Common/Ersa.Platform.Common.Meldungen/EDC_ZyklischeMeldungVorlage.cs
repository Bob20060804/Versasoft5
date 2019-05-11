using Newtonsoft.Json;
using System;

namespace Ersa.Platform.Common.Meldungen
{
	public class EDC_ZyklischeMeldungVorlage : INF_ZyklischeMeldung, INF_MeldungTexte
	{
		[JsonProperty("Id")]
		public int PRO_i32Id
		{
			get;
			set;
		}

		[JsonProperty("Meldeort1")]
		public string PRO_strMeldeort1
		{
			get;
			set;
		}

		[JsonProperty("Meldeort2")]
		public string PRO_strMeldeort2
		{
			get;
			set;
		}

		[JsonProperty("Meldeort3")]
		public string PRO_strMeldeort3
		{
			get;
			set;
		}

		[JsonProperty("Meldetext")]
		public string PRO_strMeldetext
		{
			get;
			set;
		}

		[JsonProperty("Intervall")]
		public ENUM_ZyklischeMeldungIntervall PRO_enmIntervall
		{
			get;
			set;
		}

		[JsonProperty("Zeitpunkt1")]
		public int PRO_i32Zeitpunkt
		{
			get;
			set;
		}

		[JsonProperty("Zeitpunkt2")]
		public TimeSpan PRO_sttUhrzeit
		{
			get;
			set;
		}

		[JsonProperty("Aktiv")]
		public bool PRO_blnAktiv
		{
			get;
			set;
		}

		[JsonProperty("EinlaufSperreAktiv")]
		public bool PRO_blnEinlaufSperreAktiv
		{
			get;
			set;
		}

		[JsonIgnore]
		public bool PRO_blnGeloescht
		{
			get;
			set;
		}

		[JsonIgnore]
		public bool PRO_blnHatAenderung
		{
			get;
			set;
		}

		[JsonProperty("Einlaufsperre")]
		public TimeSpan PRO_sttEinlaufsperreNachZeit
		{
			get;
			set;
		}

		[JsonProperty("AnzahlRueckstellungen")]
		public int PRO_i32AnzahlRueckstellungen
		{
			get;
			set;
		}

		[JsonProperty("ZeitRueckstellung")]
		public TimeSpan PRO_sttZeitRueckstellung
		{
			get;
			set;
		}
	}
}
