using System.Data;

namespace Ersa.Global.DataAdapter.Converter.Interfaces
{
	public interface INF_ObjektZuDataRow<in T>
	{
		DataRow FUN_edcErstelleDataRowAusObjekt(DataTable i_fdcTabelle, T i_edcItem);
	}
}
