using System.Data;

namespace Ersa.Global.DataAdapter.Converter.Interfaces
{
	public interface INF_ObjektAusDataRow<out T>
	{
		T FUN_edcLeseObjektAusDataRow(DataRow i_fdcDataRow);
	}
}
