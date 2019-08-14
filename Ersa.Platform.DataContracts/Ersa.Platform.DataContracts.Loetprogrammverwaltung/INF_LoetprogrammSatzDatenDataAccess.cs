using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammSatzDatenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_LoetprogrammSatzDatenData>> FUN_enuAlleSatzDatenZuVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleSatzdatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSatzDatenListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammSatzDatenData> i_enuSatzDatenListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcSatzDatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcSatzDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);
	}
}
