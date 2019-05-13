using Ersa.Platform.Common.Data.Benutzer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.BenutzerVerwaltung
{
	public interface INF_BenutzerVerwaltungCapability
	{
		Task<EDC_BenutzerAbfrageData> FUN_fdcBenutzerLadenAsync(long i_i64BenutzerId);

		Task<EDC_BenutzerAbfrageData> FUN_fdcBenutzerLadenAsync(string i_strBenutzerName, string i_strPasswort);

		Task<EDC_BenutzerAbfrageData> FUN_fdcBenutzerAnhandCodeLadenAsync(string i_strCode);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcDefaultBenutzerDatenLadenAsync();

		Task<long> FUN_fdcAnzahlAktiverBenutzerErmittelnAsync();

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcBenutzerDatenLadenAsync();

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcBenutzerDatenAllerMaschinenLadenAsync();

		Task FUN_fdcBenutzerDatenAendernAsync(EDC_BenutzerAbfrageData i_edcBenutzer);

		Task FUN_fdcBenutzerHinzufuegenAsync(EDC_BenutzerAbfrageData i_edcBenutzer);

		Task FUN_fdcBenutzerSynchronisierenAsync(params EDC_ExternerBenutzer[] ia_edcBenutzer);

		Task FUN_fdcBenutzerEntfernenAsync(EDC_BenutzerAbfrageData i_edcBenutzer);

		Task FUN_fdcDefaultBenutzerAnlegenWennNoetigAsync();

		Task FUN_fdcRechteFuerAdminBenutzerErgaenzenWennNoetigAsync();

		long FUN_i64AktuelleBenutzerIdHolen();
	}
}
