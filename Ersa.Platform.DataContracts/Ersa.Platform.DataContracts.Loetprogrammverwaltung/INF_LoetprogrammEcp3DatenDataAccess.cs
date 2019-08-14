using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammEcp3DatenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_LoetprogrammEcp3DatenData>> FUN_enuAlleEcp3DatenZuVersionLadenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleEcp3DatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcEcp3DatenListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammEcp3DatenData> i_enuEcp3Daten, long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcEcp3DatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcEcp3DatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);
	}
}
