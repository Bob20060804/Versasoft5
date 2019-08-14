using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammVersionDataAccess : INF_DataAccess
	{
		Task<EDC_LoetprogrammVersionData> FUN_fdcVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammVersionAbfrageData> FUN_fdcVersionLadenAsync(long i_i64VersionsId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleVersionInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammVersionAbfrageData>> FUN_fdcHoleVersionenStapelAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammVersionAbfrageData>> FUN_fdcSichtbareVersionenLadenAsync(long i_i64ProgrammIdd, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<ENUM_LoetprogrammStatus>> FUN_fdcErmittleSichtbareVersionenFuerProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammVersionData> FUN_fdcArbeitsVersionLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammVersionData> FUN_fdcFreigegebeneVersionLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammVersionData> FUN_fdcArbeitsversionErstellenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcArbeitsversionAktualisierenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcArbeitsversionLoeschenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcVersionImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcVersionFreigebenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcFreigeabeWegnehmenAsync(long i_i64ProgrammId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcVersionFreigebeStatusUndNotizenSetzenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, ENUM_LoetprogrammFreigabeStatus i_enmFreigabeStatus, string i_strFreigabeNotiz, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcVersionSichtbarkeitEntfernenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);
	}
}
