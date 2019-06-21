using System.Data.Common;

namespace Ersa.Global.DataAdapter.Converter.Interfaces
{
	public interface INF_ObjektAusReader<out T>
	{
		T FUN_edcLeseObjektAusReader(DbDataReader i_fdcReader);
	}
}
