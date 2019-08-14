using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammValideDataAccess : INF_DataAccess
	{
		Task<EDC_LoetprogrammVersionValideData> FUN_fdcHoleLoetprogrammVersionValideDataObjektAsync(long i_i64VersionsId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcIstLoetprogrammVersionFuerMaschineValideAktualisierenAsync(long i_i64VersionsId, long i_i64MaschinenId, bool i_blnValide, IDbTransaction i_fdcTransaktion = null);
	}
}
