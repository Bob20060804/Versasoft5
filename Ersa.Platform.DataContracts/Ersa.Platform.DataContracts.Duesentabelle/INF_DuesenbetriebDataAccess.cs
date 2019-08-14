using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Selektiv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Duesentabelle
{
	public interface INF_DuesenbetriebDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_DuesenbetriebWerteData>> FUN_fdcHoleAlleAktuellenDuesenBetriebswerteFuerMaschineAsync(long i_i64MaschinenId);

		Task<EDC_DuesenbetriebWerteData> FUN_fdcHoleAktuelleDuesenBetriebswerteFuerDueseAsync(long i_i64MaschinenId, long i_i64GeomertieId, ENUM_SelektivTiegel i_enmTiegel);

		Task<EDC_DuesenbetriebWerteData> FUN_fdcErstelleNeuenDuesenBetriebswertAsync(long i_i64MaschinenId, long i_i64GeomertieId, ENUM_SelektivTiegel i_enmTiegel);

		Task FUN_fdcDuesenBetriebswertSpeichernAsync(EDC_DuesenbetriebWerteData i_edcDuesenWerte);

		Task FUN_fdcTrackeDuesenWechselAsync(EDC_DuesenbetriebWerteData i_edcDuesenDataWert, long i_i64BenutzerId);

		Task<IEnumerable<EDC_DuesenbetriebWechselData>> FUN_fdcHoleDuesenWechselTrackDatenFuerMaschineAsync(long i_i64MaschinenId);
	}
}
