using System;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Zeitschaltuhr
{
	public interface INF_ZeitschaltuhrCapability
	{
		IEnumerable<INF_WochenuhrZeile> PRO_enuWochenuhrZeilen
		{
			get;
		}

		DateTime PRO_sttSpsZeit
		{
			get;
		}

		DateTime PRO_sttUrlaubStart
		{
			get;
			set;
		}

		DateTime PRO_sttUrlaubEnde
		{
			get;
			set;
		}

		bool PRO_blnBetriebsurlaubAktiviert
		{
			get;
			set;
		}

		bool PRO_blnDarfZeitschaltuhrEditieren
		{
			get;
		}

		event EventHandler m_evtAenderungStattgefunden;

		bool FUN_blnAenderungenVorhanden();

		void SUB_ZeitAufSpsSetzen(DateTime i_sttZeit);

		void SUB_AenderungenSpeichern();

		void SUB_AenderungenVerwerfen();
	}
}
