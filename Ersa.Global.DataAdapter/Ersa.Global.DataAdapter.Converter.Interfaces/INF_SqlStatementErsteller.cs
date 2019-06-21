using System.Data.Common;

namespace Ersa.Global.DataAdapter.Converter.Interfaces
{
	public interface INF_SqlStatementErsteller<in T>
	{
		string FUN_strErstelleSqlStatement(T i_edcItem);

		void SUB_ErstelleSqlStatement(T i_edcItem, DbCommand i_fdcCommand);
	}
}
