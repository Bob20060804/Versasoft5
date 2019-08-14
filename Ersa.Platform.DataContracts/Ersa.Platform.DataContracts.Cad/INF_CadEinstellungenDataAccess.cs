using Ersa.Platform.Common.Data.Cad;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Cad
{
	public interface INF_CadEinstellungenDataAccess : INF_DataAccess
	{
		Task<EDC_CadEinstellungenData> FUN_fdcHoleCadDatenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSpeichereCadDatenAsync(long i_i64VersionId, string i_strDaten, string i_strEinstellungen, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcCadDatenLoeschenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcExportiereCadDatenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcImportiereCadDatenAsync(DataSet i_fdcDataSet, long i_i64NeueVersionsId, IDbTransaction i_fdcTransaktion);
	}
}
