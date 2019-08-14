using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Betriebsmittelverwaltung
{
	public interface INF_RuestkomponentenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcLeseRuestkomponentenFuerTypeAusDatenbankAsync(long i_i64MachineGroupId, int i_i32RuestkomponentenTyp, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_RuestkomponentenData> FUN_fdcLeseRuestkomponenteFuerIdAusDatenbankAsync(long i_i64RuestkomponentenlId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_RuestkomponentenData> FUN_fdcRuestkomponentenDatenSatzHinzufuegenAsync(EDC_RuestkomponentenData i_edcRuestkomponentenData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestkomponentenDatenSatzAendernAsync(EDC_RuestkomponentenData i_edcRuestkomponentenData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestkomponentenDatenSatzLoeschenAsync(long i_i64RuestkomponentenlId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerTypAsync(long i_i64MachineGroupId, ENUM_RuestkomponentenTyp i_enmTyp, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerKomponentenNameAsync(string i_strName, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_RuestkomponentenAbfrageData> FUN_fdcLeseRuestkomponenteUndWerkzeugFuerIdentifikationAsync(ENUM_RuestkomponentenTyp i_enmTyp, string i_strIdentifikation, IDbTransaction i_fdcTransaktion = null);
	}
}
