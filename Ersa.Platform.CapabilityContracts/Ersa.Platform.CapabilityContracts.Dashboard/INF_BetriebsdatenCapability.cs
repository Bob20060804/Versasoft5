using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Model;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public interface INF_BetriebsdatenCapability
	{
		Dictionary<ENUM_BetriebsdatenTyp, IEnumerable<object>> PRO_dicBetriebsdaten
		{
			get;
		}

		IEnumerable<EDC_PrimitivParameter> PRO_enuZeitEinstellungen
		{
			get;
		}

		bool PRO_blnHatAenderung
		{
			get;
		}

		void SUB_AenderungenSpeichern();

		void SUB_AenderungenVerwerfen();

		void SUB_BetriebsdatenAktualisierungStarten();

		void SUB_BetriebsdatenAktualisierungStoppen();

		void SUB_BetriebsdatenUebernehmen(Dictionary<ENUM_BetriebsdatenTyp, IEnumerable<object>> i_dicBetriebsdaten);
	}
}
