using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.DataAdapter.Cache;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Factories
{
	public static class EDC_IndexErstellungStringFactory
	{
		public static string FUN_lstHoleIndexErstellungsString<T>(this T i_edcItem, INF_DatenbankDialekt i_edcStrategie)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			EDC_TabellenInformation i_edcTabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_edcItem);
			string i_strTabellenName = i_edcItem.FUN_strHoleTabellenName();
			return FUN_lstHoleIndexErstellungsString(i_edcStrategie, i_edcTabellenInformation, i_dicModel, i_strTabellenName);
		}

		public static string FUN_lstHoleIndexErstellungsString(INF_DatenbankDialekt i_edcDialekt, Type i_fdcType)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_fdcType);
			EDC_TabellenInformation i_edcTabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_fdcType);
			string i_strTabellenName = EDC_TabellenAttributHelfer.FUN_strHoleTabellenName(i_fdcType);
			return FUN_lstHoleIndexErstellungsString(i_edcDialekt, i_edcTabellenInformation, i_dicModel, i_strTabellenName);
		}

		private static string FUN_lstHoleIndexErstellungsString(INF_DatenbankDialekt i_edcDialekt, EDC_TabellenInformation i_edcTabellenInformation, Dictionary<PropertyInfo, EDC_SpaltenInformation> i_dicModel, string i_strTabellenName)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in i_dicModel)
			{
				if (item.Value.PRO_blnIsUniqueIndex)
				{
					string i_strIndexName = FUN_strErstelleIndexName(i_strTabellenName, item.Value.PRO_strName);
					list.Add(i_edcDialekt.FUN_strHoleUniqueIndexErstellungsString(i_strIndexName, i_strTabellenName, item.Value.PRO_strName, i_edcTabellenInformation.PRO_strTablespace));
				}
				if (item.Value.PRO_blnIsNonUniqueIndex)
				{
					string i_strIndexName2 = FUN_strErstelleIndexName(i_strTabellenName, item.Value.PRO_strName);
					list.Add(i_edcDialekt.FUN_strHoleNonUniqueHoleIndexErstellungsString(i_strIndexName2, i_strTabellenName, item.Value.PRO_strName, i_edcTabellenInformation.PRO_strTablespace));
				}
			}
			if (!string.IsNullOrEmpty(i_edcTabellenInformation.PRO_strNonUniqueCombinedIndex))
			{
				string i_strIndexName3 = FUN_strErstelleIndexName(i_strTabellenName, i_edcTabellenInformation.PRO_strNonUniqueCombinedIndex);
				list.Add(i_edcDialekt.FUN_strHoleNonUniqueHoleIndexErstellungsString(i_strIndexName3, i_strTabellenName, i_edcTabellenInformation.PRO_strNonUniqueCombinedIndex, i_edcTabellenInformation.PRO_strTablespace));
			}
			if (!string.IsNullOrEmpty(i_edcTabellenInformation.PRO_strUniqueCombinedIndex))
			{
				string i_strIndexName4 = FUN_strErstelleIndexName(i_strTabellenName, i_edcTabellenInformation.PRO_strUniqueCombinedIndex);
				list.Add(i_edcDialekt.FUN_strHoleUniqueIndexErstellungsString(i_strIndexName4, i_strTabellenName, i_edcTabellenInformation.PRO_strUniqueCombinedIndex, i_edcTabellenInformation.PRO_strTablespace));
			}
			return string.Join(";", list);
		}

		private static string FUN_strErstelleIndexName(string i_strTabellenName, string i_strSpaltenName)
		{
			return $"IDX_{i_strTabellenName}_{i_strSpaltenName}".Replace(",", string.Empty).Replace(";", string.Empty).Replace(" ", string.Empty);
		}
	}
}
