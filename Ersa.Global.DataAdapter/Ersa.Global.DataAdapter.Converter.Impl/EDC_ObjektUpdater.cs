using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Converter.Impl
{
	public class EDC_ObjektUpdater<T> : INF_SqlStatementErsteller<T>
	{
		public string FUN_strErstelleSqlStatement(T i_edcItem)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			string i_strWhereStatement = FUN_strWhereStatement(i_edcItem);
			return i_edcItem.FUN_strErstelleUpdateStatement(i_dicMapping, i_edcItem.FUN_strHoleTabellenName(), i_strWhereStatement);
		}

		public void SUB_ErstelleSqlStatement(T i_edcItem, DbCommand i_fdcCommand)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			string i_strWhereStatement = FUN_strWhereStatement(i_edcItem);
			i_fdcCommand.CommandText = i_edcItem.FUN_strErstelleUpdateStatement(dictionary, i_edcItem.FUN_strHoleTabellenName(), i_strWhereStatement);
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in dictionary)
			{
				if (!item.Value.PRO_blnIsUniqueIndex && !item.Value.PRO_blnIsPrimary)
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

		private static string FUN_strWhereStatement(T i_edcItem)
		{
			string text = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleWhereStatement(i_edcItem);
			if (string.IsNullOrEmpty(text))
			{
				text = EDC_SqlErstellungsHelfer.FUN_strErstelleWhereStatementFuerPrimaryKeys(i_edcItem, text);
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentOutOfRangeException("i_edcItem", "Using EDC_ObjektUpdater without 'where-statement' is not allowed!");
				}
			}
			return text;
		}
	}
}
