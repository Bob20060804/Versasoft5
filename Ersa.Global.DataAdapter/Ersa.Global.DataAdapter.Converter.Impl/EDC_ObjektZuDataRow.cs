using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektZuDataRow<T> : INF_ObjektZuDataRow<T>
	{
		public DataRow FUN_edcErstelleDataRowAusObjekt(DataTable i_fdcTabelle, T i_edcItem)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			DataRow dataRow = i_fdcTabelle.NewRow();
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in dictionary)
			{
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					dataRow[item.Value.PRO_strName] = item.Key.GetValue(i_edcItem, null);
				}
				else
				{
					foreach (EDC_DynamischeSpalte item2 in item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>)
					{
						dataRow[item2.PRO_strSpaltenName] = item2.PRO_objWert;
					}
				}
			}
			return dataRow;
		}
	}
}
