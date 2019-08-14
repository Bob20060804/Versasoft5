using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammParameterDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_LoetprogrammParameterData>> FUN_enuAlleParameterZuVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleParameterInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcParameterListeSpeichernAsync(IEnumerable<EDC_LoetprogrammParameterData> i_enuParameterListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcParameterListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammParameterData> i_enuParameterListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcParameterVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcParameterDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammParameterDiffData>> FUN_fdcErmittleParameterAenderungenZwischenZweiVersionenAsync(long i_i64VersionAltId, long i_i64VersionNeuId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammParameterDiffData>> FUN_fdcErmittleParameterWerteEinerVersionenAsync(long i_i64VersionId, IEnumerable<string> i_enuVariablen, IDbTransaction i_fdcTransaktion = null);
	}
}
