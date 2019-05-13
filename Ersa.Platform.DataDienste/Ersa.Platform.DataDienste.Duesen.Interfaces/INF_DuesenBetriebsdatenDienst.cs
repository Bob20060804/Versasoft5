using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Selektiv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Duesen.Interfaces
{
	public interface INF_DuesenBetriebsdatenDienst
	{
		Task<IEnumerable<EDC_DuesenbetriebWerteData>> FUN_fdcHoleAlleAktuellenDuesenBetriebswerteFuerMaschineAsync();

		Task<EDC_DuesenBetriebWerte> FUN_fdcHoleAktuelleDuesenBetriebswerteFuerDueseAsync(long i_i64DuesenId, ENUM_SelektivTiegel i_enmTiegel);

		Task FUN_fdcSpeichereAktuelleDuesenBetriebswerteAsync(IEnumerable<EDC_DuesenBetriebWerte> i_enuWerteAktuelleDuese);

		Task FUN_fdcSpeichereAktuelleDuesenBetriebswerteFuerDueseAsync(EDC_DuesenBetriebWerte i_edcWerteAktuelleDuese, bool i_blnDueseIstJetztVerbraucht);

		Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleVonMaschineVerwendetenStandardGeometrienAsync();

		Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync();

		Task<bool> FUN_fdcAktualisiereDuesenGeometrienAusDateiAsync(string i_strImportDatei);

		Task<IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData>> FUN_fdcHoleAlleVonMaschineVerwendetenLoetduesenGeometrienUndSollwerteAsync();

		Task<EDC_DuesenGeometrieUndSollwerteAbfrageData> FUN_fdcHoleAlleLoetduesenGeometrienUndSollwerteZuEinerGeometrieAsync(long i_i64GeometrieId);

		Task FUN_fdcSpeichereAktuelleDuesenSollwerteAsync(IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData> i_enuWerteAktuelleSollwerteDuese);
	}
}
