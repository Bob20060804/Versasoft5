using Ersa.Platform.Common.Data.Maschinenkonfiguration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Maschinenkonfiguration
{
	public interface INF_MaschinenkonfigurationDataAccess : INF_DataAccess
	{
		Task<long> FUN_fdcSpeichereKonfigurationAsync(EDC_MaschinenkonfigurationData i_edcMaschinenkonfiguration, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_MaschinenkonfigurationData>> FUN_fdcLadeAlleMaschinenkonfigurationenZuMaschineAsync(long i_i64MaschinenId);

		Task<DataTable> FUN_fdcLadeAlleMaschinenkonfigurationenZuMaschineInDataTableAsync(long i_i64MaschinenId);

		Task<EDC_MaschinenkonfigurationData> FUN_fdcLadeNeusteMaschinenkonfigurationenZuMaschineAsync(long i_i64MaschinenId);

		Task<DataTable> FUN_fdcLadeNeusteMaschinenkonfigurationenZuMaschineInDataTableAsync(long i_i64MaschinenId);

		Task<long> FUN_fdcImportiereKonfigurationAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion, long i_i64NeueMaschinenId = 0L);

		Task<EDC_MaschinenkonfigurationData> FUN_fdcLadeMaschinenkonfigurationAsync(long i_i64KonfigurationsId, IDbTransaction i_fdcTransaktion = null);
	}
}
