using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Exeptions;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Factories
{
	public static class EDC_TabellenErstellungsStringFactory
	{
		public static string FUN_strHoleTabellenErstellungsString<T>(this T i_edcItem, INF_DatenbankDialekt i_edcDialekt)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			string i_strTabellenName = i_edcItem.FUN_strHoleTabellenName();
			return i_edcItem.FUN_strHoleTabellenErstellungsString(i_edcDialekt, i_dicModel, i_strTabellenName);
		}

		public static string FUN_strHoleTabellenErstellungsString(INF_DatenbankDialekt i_edcDialekt, Type i_fdcType)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_fdcType);
			string i_strTabellenName = EDC_TabellenAttributHelfer.FUN_strHoleTabellenName(i_fdcType);
			return FUN_strHoleTabellenErstellungsString(i_edcDialekt, i_dicModel, i_strTabellenName);
		}

		private static string FUN_strHoleTabellenErstellungsString<T>(this T i_edcItem, INF_DatenbankDialekt i_edcDialekt, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel, string i_strTabellenName)
		{
			string arg = $"CREATE TABLE {i_strTabellenName}";
			string arg2 = i_edcItem.FUN_strErstelleSpaltenMapping(i_edcDialekt, i_dicModel);
			string arg3 = FUN_strHolePrimaryConstraintString(i_edcDialekt, i_strTabellenName, i_dicModel);
			return $"{arg} ({arg2},{arg3})";
		}

		private static string FUN_strErstelleSpaltenMapping<T>(this T i_edcItem, INF_DatenbankDialekt i_edcDialekt, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel)
		{
			string text = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicModel)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					text += FUN_strHoleDenDatenTypen(i_edcDialekt, item.Key, item.Value);
				}
				else
				{
					IEnumerable<EDC_DynamischeSpalte> i_lstMapping = item.Key.GetValue(i_edcItem) as IEnumerable<EDC_DynamischeSpalte>;
					text += FUN_strErstelleDynamischesSpaltenMapping(i_edcDialekt, i_lstMapping);
				}
			}
			return text;
		}

		private static string FUN_strErstelleDynamischesSpaltenMapping(INF_DatenbankDialekt i_edcDialekt, IEnumerable<EDC_DynamischeSpalte> i_lstMapping)
		{
			string text = string.Empty;
			foreach (EDC_DynamischeSpalte item in i_lstMapping)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += FUN_strDynamischesDatenTyp(i_edcDialekt, item);
			}
			return text;
		}

		private static string FUN_strDynamischesDatenTyp(INF_DatenbankDialekt i_edcDialekt, EDC_DynamischeSpalte i_edcSpalte)
		{
			i_edcDialekt.FUN_dicDatenTypenMapping().TryGetValue(i_edcSpalte.PRO_strDatenTyp, out string value);
			if ("String".Equals(i_edcSpalte.PRO_strDatenTyp))
			{
				value = $"{value}({i_edcSpalte.PRO_i32DatenLaenge})";
			}
			return $"{i_edcSpalte.PRO_strSpaltenName} {value} NULL";
		}

		private static string FUN_strHoleTabellenErstellungsString(INF_DatenbankDialekt i_edcDialekt, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel, string i_strTabellenName)
		{
			string arg = $"CREATE TABLE {i_strTabellenName}";
			string arg2 = FUN_strErstelleSpaltenMapping(i_edcDialekt, i_dicModel);
			string arg3 = FUN_strHolePrimaryConstraintString(i_edcDialekt, i_strTabellenName, i_dicModel);
			return $"{arg} ({arg2},{arg3})";
		}

		private static string FUN_strErstelleSpaltenMapping(INF_DatenbankDialekt i_edcDialekt, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel)
		{
			string text = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicModel)
			{
				if (!item.Value.PRO_blnIsDynamischeSpalte)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ",";
					}
					text += FUN_strHoleDenDatenTypen(i_edcDialekt, item.Key, item.Value);
				}
			}
			return text;
		}

		private static string FUN_strHoleDenDatenTypen(INF_DatenbankDialekt i_edcDialekt, PropertyInfo i_fdcProperty, EDC_SpaltenInformation i_edcSpaltenInfo)
		{
			Dictionary<string, string> dictionary = i_edcDialekt.FUN_dicDatenTypenMapping();
			string name = i_fdcProperty.PropertyType.Name;
			if (i_fdcProperty != null && i_fdcProperty.PropertyType.IsGenericType && i_fdcProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				name = Nullable.GetUnderlyingType(i_fdcProperty.PropertyType).Name;
			}
			dictionary.TryGetValue(name, out string value);
			if ("String".Equals(i_fdcProperty.PropertyType.Name) && i_edcSpaltenInfo.PRO_i32Length > 0)
			{
				value = $"{value}({i_edcSpaltenInfo.PRO_i32Length})";
			}
			else if ("String".Equals(i_fdcProperty.PropertyType.Name) && i_edcSpaltenInfo.PRO_i32Length == 0)
			{
				dictionary.TryGetValue("Text", out value);
			}
			return string.Format(i_edcSpaltenInfo.PRO_blnIsRequired ? "{0} {1} NOT NULL" : "{0} {1} NULL", i_edcSpaltenInfo.PRO_strName, value);
		}

		private static string FUN_strHolePrimaryConstraintString(INF_DatenbankDialekt i_edcDialekt, string i_strTabellenName, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel)
		{
			string text = string.Empty;
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicModel)
			{
				if (item.Value.PRO_blnIsPrimary)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ",";
					}
					text += item.Value.PRO_strName;
				}
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new EDC_DatenbankErstellungsExeption($"No primary key set for table {i_strTabellenName}");
			}
			return i_edcDialekt.FUN_strHoleConstraintErstellungsString(i_strTabellenName, text);
		}
	}
}
