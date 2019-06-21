using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektInserter<T> : INF_SqlStatementErsteller<T>
	{
		public string FUN_strErstelleSqlStatement(T i_edcItem)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			return i_edcItem.FUN_strErstelleInsertStatement(i_dicMapping, i_edcItem.FUN_strHoleTabellenName());
		}

		public void SUB_ErstelleSqlStatement(T i_edcItem, DbCommand i_fdcCommand)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			string text2 = i_fdcCommand.CommandText = i_edcItem.FUN_strErstelleInsertStatement(dictionary, i_edcItem.FUN_strHoleTabellenName());
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in dictionary)
			{
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					i_edcItem.SUB_SetzteCommandParameter(i_fdcCommand, item.Key, item.Value.PRO_strName);
				}
				else
				{
					foreach (EDC_DynamischeSpalte item2 in item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>)
					{
						EDC_PropertySetzenHelfer.SUB_SetzteCommandParameter(i_fdcCommand, null, item2.PRO_strSpaltenName, item2.PRO_objWert, item2.PRO_strDatenTyp);
					}
				}
			}
		}
	}
}
