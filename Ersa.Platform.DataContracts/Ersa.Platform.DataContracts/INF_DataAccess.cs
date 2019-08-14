using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts
{
	public interface INF_DataAccess
	{
		IDbTransaction FUN_fdcStarteTransaktion();

		Task<IDbTransaction> FUN_fdcStarteTransaktionAsync();

		void SUB_CommitTransaktion(IDbTransaction i_fdcDbTransaktion);

		void SUB_RollbackTransaktion(IDbTransaction i_fdcDbTransaktion);

		Task<long> FUN_fdcHoleNaechstenSequenzWertAsync(string i_strSequenzName);

		Task FUN_fdcFuehreStatementListeAusAsync(IEnumerable<string> i_enuStatements, IDbTransaction i_fdcTransaktion = null);
	}
}
