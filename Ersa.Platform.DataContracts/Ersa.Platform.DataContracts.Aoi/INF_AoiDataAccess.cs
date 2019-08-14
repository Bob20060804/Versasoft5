using Ersa.Platform.Common.Data.Aoi;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Aoi
{
	public interface INF_AoiDataAccess : INF_DataAccess
	{
		Task<EDC_AoiProgramData> FUN_fdcHoleAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSpeichereAoiProgrammAsync(long i_i64ProgrammId, string i_strDaten, string i_strEinstellungen, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoescheAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcExportiereAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcImportiereAoiProgrammAsync(DataSet i_fdcDataSet, long i_i64NeueProgrammId, IDbTransaction i_fdcTransaktion);

		Task<EDC_AoiSchrittData> FUN_fdcHoleAoiSchrittDatenAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, IDbTransaction i_fdcTransaktion);

		Task<IEnumerable<EDC_AoiSchrittData>> FUN_fdcHoleAoiSchrittDatenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcSpeichereAoiSchrittDatenAsync(long i_i64ProgrammId, IEnumerable<EDC_AoiSchrittData> i_enuAoiSchrittData, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcSpeichereAoiSchrittDatenAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, byte[] i_bytBinaries, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcLoescheAoiSchrittDataAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcLoescheAoiSchrittDataAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion);

		Task<DataTable> FUN_fdcExportiereAoiSchrittDatenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcImportiereAoiSchrittDatenAsync(DataSet i_fdcDataSet, long i_i64NeueProgrammId, IDbTransaction i_fdcTransaktion);

		Task<IEnumerable<EDC_AoiErgebnisData>> FUN_fdcHoleAoiErgebnisMitHashAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_AoiErgebnisData>> FUN_fdcHoleAoiErgebnisMitHashUndAoiTypAsync(string i_strHash, int i_i32AoiType, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSpeichereAoiErgebnisDatenListAsync(IEnumerable<EDC_AoiErgebnisData> i_enuErgebnisData, IDbTransaction i_fdcTransaktion = null);
	}
}
