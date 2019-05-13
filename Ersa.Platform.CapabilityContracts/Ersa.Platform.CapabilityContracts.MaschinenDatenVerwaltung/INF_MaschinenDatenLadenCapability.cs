using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung
{
	public interface INF_MaschinenDatenLadenCapability
	{
		void SUB_AlteVariablenAufNeueMappen(IEnumerable<EDC_ParameterDaten> i_enuParameterDaten);

		void SUB_KonfigurationAktualisieren(EDC_ParameterDatenKonfig i_edcKonfiguration);

		Task FUN_fdcUebertragungAufSteuerungVorbereitenAsync(IEnumerable<EDC_ParameterDaten> i_enuParameterDaten);

		Task FUN_fdcDatenAufSteuerungUebertragenAsync(IEnumerable<EDC_ParameterDaten> i_enuParameterDaten);

		Task FUN_fdcMaschinenDatenLadenAbschliessenAsync();
	}
}
