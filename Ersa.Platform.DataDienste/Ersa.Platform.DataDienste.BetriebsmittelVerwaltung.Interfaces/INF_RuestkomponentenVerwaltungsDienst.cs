using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces
{
	public interface INF_RuestkomponentenVerwaltungsDienst
	{
		Task<EDC_RuestkomponentenData> FUN_fdcLeseRuestkomponentenDatenFuerFuerKomponentenIdAsync(long i_i64RuestkomponentenId);

		Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcLeseRuestkomponentenDatenFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp);

		Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcRuestkomponentenDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestkomponentenDatenSaetzeAendernAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestkomponentenDatenSaetzeLoeschenAsync(IEnumerable<EDC_RuestkomponentenData> i_enuRuestkomponentenData, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcLeseRuestwerkzeugDatenFuerKomponentenIdAsync(long i_i64RuestkomponentenId);

		Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcRuestwerkzeugDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestwerkzeugDatenSaetzeAendernAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestwerkzeugDatenSaetzeLoeschenAsync(IEnumerable<EDC_RuestwerkzeugeData> i_enuRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp);

		Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerNameAsync(string i_strName);

		Task<EDC_RuestkomponentenAbfrageData> FUN_fdcLeseRuestkomponentenUndWerkzeugDatenFuerIdentifikationAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp, string i_strIdentifikation);

		Task<IEnumerable<EDC_Ruestkomponente>> FUN_fdcLeseRuestkomponenteFuerTypAsync(ENUM_RuestkomponentenTyp i_enmRuestkomponentenTyp);

		Task<IEnumerable<EDC_Ruestkomponente>> FUN_fdcAktualisiereRuestkomponentenAsync(IEnumerable<EDC_Ruestkomponente> i_lstRuestkomponenten);
	}
}
