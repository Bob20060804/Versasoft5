using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Produktionssteuerung
{
	public interface INF_ProduktionssteuerungCapability
	{
		IEnumerable<EDC_EinstellungsGruppe> FUN_edcEinstellungsGruppenLaden();

		void SUB_Speichern();

		void SUB_Verwerfen();
	}
}
