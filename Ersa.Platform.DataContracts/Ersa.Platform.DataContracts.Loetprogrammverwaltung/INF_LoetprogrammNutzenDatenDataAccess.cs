using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammNutzenDatenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_LoetprogrammNutzenData>> FUN_fdcLadeNutzenDatenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleNutzendatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSpeichereNutzenDatenAsync(long i_i64VersionsId, IEnumerable<EDC_LoetprogrammNutzenData> i_enuNutzenDaten, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcNutzenDatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcNutzenDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);
	}
}
