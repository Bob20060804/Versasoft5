using System;

namespace Ersa.Platform.Common.Meldungen
{
	public interface INF_ZyklischeMeldung
	{
		int PRO_i32Id
		{
			get;
		}

		ENUM_ZyklischeMeldungIntervall PRO_enmIntervall
		{
			get;
			set;
		}

		int PRO_i32Zeitpunkt
		{
			get;
			set;
		}

		TimeSpan PRO_sttUhrzeit
		{
			get;
			set;
		}

		bool PRO_blnAktiv
		{
			get;
			set;
		}

		bool PRO_blnEinlaufSperreAktiv
		{
			get;
			set;
		}

		bool PRO_blnGeloescht
		{
			get;
			set;
		}

		bool PRO_blnHatAenderung
		{
			get;
		}

		TimeSpan PRO_sttEinlaufsperreNachZeit
		{
			get;
			set;
		}

		int PRO_i32AnzahlRueckstellungen
		{
			get;
			set;
		}

		TimeSpan PRO_sttZeitRueckstellung
		{
			get;
			set;
		}
	}
}
