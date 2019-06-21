using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_SqlErstellungsHelfer
	{
		public static string FUN_strErstelleSelectStatement<T>(this T i_edcItem, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping, string i_strTabellenName, string i_strWhereStatement)
		{
			string text = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicMapping)
			{
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					text = FUN_strFuegeSelectSpaltenAn(text, item.Value.PRO_strName);
				}
				else
				{
					foreach (EDC_DynamischeSpalte item2 in item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>)
					{
						text = FUN_strFuegeSelectSpaltenAn(text, item2.PRO_strSpaltenName);
					}
				}
			}
			return $"Select {text} from {i_strTabellenName} {i_strWhereStatement}";
		}

		public static string FUN_strErstelleDeleteStatement(string i_strTabellenName, string i_strWhereStatement)
		{
			return $"Delete from {i_strTabellenName} {i_strWhereStatement}";
		}

		public static string FUN_strErstelleUpdateStatement<T>(this T i_edcItem, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping, string i_strTabellenName, string i_strWhereStatement)
		{
			string text = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicMapping)
			{
				if (!item.Value.PRO_blnIsUniqueIndex && !item.Value.PRO_blnIsPrimary)
				{
					if (!item.Value.PRO_blnIsDynamischeSpalte)
					{
						text = FUN_strFuegeUpdateSpaltenAn(text, item.Value.PRO_strName);
					}
					else
					{
						foreach (EDC_DynamischeSpalte item2 in item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>)
						{
							text = FUN_strFuegeUpdateSpaltenAn(text, item2.PRO_strSpaltenName);
						}
					}
				}
			}
			return $"Update {i_strTabellenName} Set {text} {i_strWhereStatement}";
		}

		public static string FUN_strErstelleInsertStatement<T>(this T i_edcItem, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicMapping, string i_strTabellenName)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicMapping)
			{
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					text = FUN_strFuegeInsertSpaltenAn(text, item.Value.PRO_strName);
					text2 = FUN_strFuegeInsertWertAn(text2, item.Value.PRO_strName);
				}
				else
				{
					foreach (EDC_DynamischeSpalte item2 in item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>)
					{
						text = FUN_strFuegeInsertSpaltenAn(text, item2.PRO_strSpaltenName);
						text2 = FUN_strFuegeInsertWertAn(text2, item2.PRO_strSpaltenName);
					}
				}
			}
			return $"Insert into {i_strTabellenName} ({text}) values ({text2})";
		}

		public static string FUN_strErstelleWhereStatementFuerPrimaryKeys<T>(T i_edcItem, string i_strWhereStatement)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = (from i_fdcPair in EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem)
			where i_fdcPair.Value.PRO_blnIsPrimary
			select i_fdcPair).ToDictionary((KeyValuePair<PropertyInfo, EDC_SpaltenInformation> i_fdcPair) => i_fdcPair.Key, (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> i_fdcPair) => i_fdcPair.Value);
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in dictionary)
				{
					string text = item.Key.GetValue(i_edcItem).ToString();
					if ("String".Equals(item.Key.PropertyType.Name))
					{
						text = $"'{text}'";
					}
					i_strWhereStatement = FUN_strWhereStatementSpalteAn(i_strWhereStatement, item.Value.PRO_strName, text);
				}
				return i_strWhereStatement;
			}
			return i_strWhereStatement;
		}

		public static string FUN_strErstellePrimaryKeyMaxStatement<T>(this T i_edcItem)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = (from i_fdcPair in EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem)
			where i_fdcPair.Value.PRO_blnIsPrimary
			select i_fdcPair).ToDictionary((KeyValuePair<PropertyInfo, EDC_SpaltenInformation> i_fdcPair) => i_fdcPair.Key, (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> i_fdcPair) => i_fdcPair.Value);
			if (dictionary.Count == 1)
			{
				return $"Select Max({dictionary.First().Value.PRO_strName}) From {i_edcItem.FUN_strHoleTabellenName()}";
			}
			return string.Empty;
		}

		private static string FUN_strFuegeSelectSpaltenAn(string i_strSpalten, string i_strSpaltenName)
		{
			i_strSpalten = ((!string.IsNullOrEmpty(i_strSpalten)) ? (i_strSpalten + $", {i_strSpaltenName}") : i_strSpaltenName);
			return i_strSpalten;
		}

		private static string FUN_strFuegeUpdateSpaltenAn(string i_strSpaltenPaare, string i_strSpaltenName)
		{
			if (!string.IsNullOrEmpty(i_strSpaltenPaare))
			{
				i_strSpaltenPaare += ",";
			}
			i_strSpaltenPaare += string.Format(" {0}=@{0}", i_strSpaltenName);
			return i_strSpaltenPaare;
		}

		private static string FUN_strFuegeInsertSpaltenAn(string i_strSpalten, string i_strSpaltenName)
		{
			i_strSpalten = ((!string.IsNullOrEmpty(i_strSpalten)) ? (i_strSpalten + $", {i_strSpaltenName}") : i_strSpaltenName);
			return i_strSpalten;
		}

		private static string FUN_strFuegeInsertWertAn(string i_strWerte, string i_strSpaltenWert)
		{
			i_strWerte = ((!string.IsNullOrEmpty(i_strWerte)) ? (i_strWerte + $", @{i_strSpaltenWert}") : $"@{i_strSpaltenWert}");
			return i_strWerte;
		}

		private static string FUN_strWhereStatementSpalteAn(string i_strItem, string i_strSpaltenName, string i_objSpaltenWert)
		{
			i_strItem = ((!string.IsNullOrEmpty(i_strItem)) ? (i_strItem + $" and {i_strSpaltenName} = {i_objSpaltenWert}") : $" where {i_strSpaltenName} = {i_objSpaltenWert}");
			return i_strItem;
		}
	}
}
