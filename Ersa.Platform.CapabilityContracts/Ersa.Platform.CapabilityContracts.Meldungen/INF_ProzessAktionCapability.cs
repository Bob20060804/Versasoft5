using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Meldungen
{
	public interface INF_ProzessAktionCapability
	{
		Task FUN_fdcProzessAktionAnfordernAsync(INF_Meldung i_edcMeldung);

		Task FUN_fdcProzessAktionBeendenAsync(IEnumerable<ENUM_ProzessAktionen> i_enuProzessAktionen);
	}
}
