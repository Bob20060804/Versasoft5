using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.Meldungen
{
	public interface INF_ZyklischeMeldungenCapability
	{
		void SUB_Speichern();

		void SUB_Verwerfen();

		void SUB_Importieren(IEnumerable<INF_ZyklischeMeldung> i_enuVorlagen);

		IEnumerable<INF_ZyklischeMeldung> FUN_enuLaden();

		INF_ZyklischeMeldung FUN_edcErstellen();
	}
}
