using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.MeldeAmpel
{
	public interface INF_MeldeAmpelCapability
	{
		IEnumerable<EDC_AmpelEinstellungen> FUN_enuAmpeleinstellungenLaden();

		void SUB_LampenTestAnfordern();

		bool FUN_blnKannLampenTestAnfordern();

		void SUB_Speichern();

		void SUB_Verwerfen();
	}
}
