using System;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Zeitschaltuhr
{
	public interface INF_WochenuhrZeile
	{
		ENUM_WochenuhrZeileZustaende PRO_enmZustand
		{
			get;
			set;
		}

		DayOfWeek PRO_enmWochentagEin
		{
			get;
			set;
		}

		DayOfWeek PRO_enmWochentagAus
		{
			get;
			set;
		}

		TimeSpan PRO_sttVon
		{
			get;
			set;
		}

		TimeSpan PRO_sttBis
		{
			get;
			set;
		}
	}
}
