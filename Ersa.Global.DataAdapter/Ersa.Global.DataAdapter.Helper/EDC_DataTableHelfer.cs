using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Factories;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_DataTableHelfer
	{
		public static IEnumerable<T> FUN_enmErstelleObjektListeAusDataTable<T>(this DataTable i_fdcTable) where T : class
		{
			List<T> list = new List<T>();
			if (i_fdcTable == null || i_fdcTable.Rows.Count == 0)
			{
				return list;
			}
			INF_ObjektAusDataRow<T> edcConverter = (INF_ObjektAusDataRow<T>)EDC_ConverterFactory.FUN_edcErstelleObjektAusDataRowConverter<T>();
			list.AddRange(from DataRow fdcRow in i_fdcTable.Rows
			select ((INF_ObjektAusDataRow<T>)edcConverter).FUN_edcLeseObjektAusDataRow(fdcRow));
			return list;
		}
	}
}
