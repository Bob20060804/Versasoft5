using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektAusDataRow<T> : INF_ObjektAusDataRow<T> where T : class
	{
		public T FUN_edcLeseObjektAusDataRow(DataRow i_fdcDataRow)
		{
			T val = Activator.CreateInstance(typeof(T)) as T;
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(val);
			List<EDC_DynamischeSpalte> list = null;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item2 in dictionary)
			{
				if (!item2.Value.PRO_blnIsDynamischeSpalte)
				{
					val.SUB_LeseDataRowInProperty(i_fdcDataRow, item2.Key, item2.Value.PRO_strName);
				}
				else
				{
					list = new List<EDC_DynamischeSpalte>();
					item2.Key.SetValue(val, list);
				}
			}
			if (list != null)
			{
				object[] itemArray = i_fdcDataRow.ItemArray;
				for (int i = 0; i < itemArray.Length; i++)
				{
					DataColumn dataColumn = (DataColumn)itemArray[i];
					string strSpalte = dataColumn.ColumnName.ToLower();
					Type dataType = dataColumn.DataType;
					if (!dictionary.Any(delegate(KeyValuePair<PropertyInfo, EDC_SpaltenInformation> fdcPaar)
					{
						if (!fdcPaar.Value.PRO_blnIsDynamischeSpalte)
						{
							return fdcPaar.Value.PRO_strName.ToLower().Equals(strSpalte);
						}
						return false;
					}))
					{
						EDC_DynamischeSpalte item = new EDC_DynamischeSpalte(strSpalte, dataType.Name, EDC_PropertySetzenHelfer.FUN_objKonvertiereDatentyp(null, i_fdcDataRow[dataColumn], dataType.Name));
						list.Add(item);
					}
				}
			}
			return val;
		}
	}
}
