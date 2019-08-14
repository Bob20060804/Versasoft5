using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammBibliothekDataAccess : INF_DataAccess
	{
		Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleDefaultBibliothekFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleBibliothekMitIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleBibliothekInDataTableAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBibliothekData> FUN_edcHoleBibliothekMitNamenAsync(string i_strBibliotheksName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammBibliothekData>> FUN_fdcHoleAlleNichtGeloeschtenBibliothekenFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBibliothekData> FUN_fdcBibliothekErstellenAsync(string i_strBibliotheksName, long i_i64BenutzerId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBibliothekGeloeschtSetzenAsync(long i_i64BibliotheksId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBibliothekUmbenennenAsync(long i_i64BibliotheksId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBibliothekData> FUN_fdcBibliothekDuplizierenAsync(long i_i64BibliotheksId, string i_strNeuerName, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBibliothekData> FUN_fdcImportiereBibliothekAsync(DataTable i_fdcTable, string i_strNeuerName, long i_i64BenutzerId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);
	}
}
