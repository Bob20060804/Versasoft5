using Ersa.Platform.Common.Data.Duesentabelle;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Duesentabelle
{
	public interface INF_DuesentabelleDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_DuesenData>> FUN_fdcDuesenDatenLadenAsync(string i_strAggregatName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_DuesenData> FUN_fdcHoleDuesenDatenMitDuesenIdAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_Loetduese>> FUN_fdcHoleAlleLoetduesenAsync(long i_i64MaschinenId, string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleDuesentabellenDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcImportiereDuesentabellenDataAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion, long i_i64NeueMaschinenId = 0L);

		Task<long> FUN_fdcDuesenDatenSatzHinzufuegenAsync(EDC_DuesenData i_edcDuese, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcDuesenDatenSatzAendernAsync(EDC_DuesenData i_edcDuese, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcDuesenDatenSatzLoeschenAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_DuesenGeometrienData> FUN_fdcHoleDuesenGeometrieZuEinerDuesenParameterIdAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleVonMaschineVerwendetenStandardGeometrienAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync(IDbTransaction i_fdcTransaktion = null);

		Task<bool> FUN_fdcAktualisiereDuesenGeometrienAusDateiAsync(string i_strImportDatei);

		Task<IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData>> FUN_fdcHoleAlleDuesenGeometrienUndSollwerteAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_DuesenGeometrieUndSollwerteAbfrageData> FUN_fdcHoleDuesenGeometrieUndSollwerteZuEinerGeometrieAsync(long i_i64GeometrieId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcDuesenSollwerteDatenSatzHinzufuegenAsync(EDC_DuesenSollwerteData i_edcDueseSollwerte, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcDuesenSollwerteDatenSatzAendernAsync(EDC_DuesenSollwerteData i_edcDueseSollwerte, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleAlleLoetduesenStandardGeometrienDataTableAsync();

		Task FUN_fdcImportiereDuesenGeometrienDataAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion);
	}
}
