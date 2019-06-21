using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.DataAdapter.Cache;
using System.Collections.Generic;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Extensions
{
	public static class EDC_DataObjektExtension
	{
		public static Tout FUN_edcCloneDataItem<Tin, Tout>(this Tin i_edcItem) where Tout : new()
		{
			Tout val = new Tout();
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem);
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary2 = EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(val);
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in dictionary)
			{
				foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item2 in dictionary2)
				{
					if (item2.Key.Name.Equals(item.Key.Name))
					{
						item2.Key.SetValue(val, item.Key.GetValue(i_edcItem));
						break;
					}
				}
			}
			return val;
		}

		public static bool FUN_blnCompareDataObjects<T>(this T i_edcItem, T i_edcCompare)
		{
			if (i_edcCompare == null)
			{
				return false;
			}
			foreach (KeyValuePair<PropertyInfo, EDC_SpaltenInformation> item in EDC_AttributeCache.PRO_edcInstance.FUN_fdcHoleSpaltenInformationen(i_edcItem))
			{
				object value = item.Key.GetValue(i_edcItem);
				object value2 = item.Key.GetValue(i_edcCompare);
				if (value2 != null && value == null)
				{
					return false;
				}
				if (value2 == null && value != null)
				{
					return false;
				}
				if (value != null && !value.Equals(value2))
				{
					return false;
				}
			}
			return true;
		}
	}
}
