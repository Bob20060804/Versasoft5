using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.DataAdapter.Cache;
using System;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_TabellenAttributHelfer
	{
		public static string FUN_strHoleTabellenName(Type i_fdcType)
		{
			return EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_fdcType).PRO_strName;
		}

		public static string FUN_strHoleTabellenName<T>(this T i_edcItem)
		{
			EDC_TabellenInformation eDC_TabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_edcItem);
			if (!eDC_TabellenInformation.PRO_blnNameIstProperty)
			{
				return eDC_TabellenInformation.PRO_strName;
			}
			PropertyInfo[] properties = i_edcItem.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (eDC_TabellenInformation.PRO_strName.Equals(propertyInfo.Name))
				{
					return (string)propertyInfo.GetValue(i_edcItem);
				}
			}
			throw new ArgumentOutOfRangeException("i_edcItem", $"The PRO_blnNameIstProperty is missconfigured or the tablename Property is not set properly for '{i_edcItem}'.");
		}

		public static string FUN_strHoleDatenbankAbfrage(Type i_fdcType)
		{
			EDC_TabellenInformation eDC_TabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_fdcType);
			if (eDC_TabellenInformation.PRO_blnIsQuery)
			{
				return eDC_TabellenInformation.PRO_strQueryStatement;
			}
			return string.Empty;
		}

		public static string FUN_strHoleDatenbankAbfrage<T>(this T i_edcItem)
		{
			EDC_TabellenInformation i_fdcTabellenInformation = EDC_AttributeCache.PRO_edcInstance.FUN_strHoleTabellenInformation(i_edcItem);
			return i_edcItem.FUN_strHoleDatenbankAbfrage(i_fdcTabellenInformation);
		}

		public static string FUN_strHoleDatenbankAbfrage<T>(this T i_edcItem, EDC_TabellenInformation i_fdcTabellenInformation)
		{
			if (!i_fdcTabellenInformation.PRO_blnIsQuery)
			{
				return string.Empty;
			}
			if (!i_fdcTabellenInformation.PRO_blnQueryIstProperty)
			{
				return i_fdcTabellenInformation.PRO_strQueryStatement;
			}
			PropertyInfo[] properties = i_edcItem.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (i_fdcTabellenInformation.PRO_strQueryStatement.Equals(propertyInfo.Name))
				{
					return (string)propertyInfo.GetValue(i_edcItem);
				}
			}
			throw new ArgumentOutOfRangeException("i_edcItem", $"The PRO_blnQueryIstProperty is missconfigured or the query Property is not set properly for '{i_edcItem}'.");
		}
	}
}
