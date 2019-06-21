using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ersa.Global.DataAdapter.Helper
{
	public static class EDC_AttributHelfer
	{
		public static Dictionary<PropertyInfo, EDC_SpaltenInformation> FUN_fdcHoleSpaltenInformationen<T>(this T i_edcItem)
		{
			return FUN_fdcHoleSpaltenInformationen(i_edcItem.GetType());
		}

		public static Dictionary<PropertyInfo, EDC_FilterInformationen> FUN_fdcHoleFilterInformationen<T>(this T i_edcItem)
		{
			return FUN_fdcHoleFilterInformationen(i_edcItem.GetType());
		}

		public static EDC_TabellenInformation FUN_strHoleTabellenInformationen<T>(this T i_edcItem)
		{
			return FUN_strHoleTabellenInformationen(i_edcItem.GetType());
		}

		public static PropertyInfo FUN_fdcHoleWhereStatementProperty<T>(this T i_edcItem)
		{
			return (from edcProperty in i_edcItem.GetType().GetProperties()
			let lstArttribute = edcProperty.GetCustomAttributes(inherit: false)
			let lstVorhanden = lstArttribute.FirstOrDefault((object a) => a.GetType() == typeof(EDC_AbfrageInformation)) as EDC_AbfrageInformation
			where lstVorhanden != null
			select edcProperty).FirstOrDefault();
		}

		public static Dictionary<PropertyInfo, EDC_SpaltenInformation> FUN_fdcHoleSpaltenInformationen(Type i_edcType)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> dictionary = new Dictionary<PropertyInfo, EDC_SpaltenInformation>();
			PropertyInfo[] properties = i_edcType.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				EDC_SpaltenInformation eDC_SpaltenInformation = propertyInfo.GetCustomAttributes(inherit: false).FirstOrDefault((object i_fdcObject) => i_fdcObject.GetType() == typeof(EDC_SpaltenInformation)) as EDC_SpaltenInformation;
				if (eDC_SpaltenInformation != null)
				{
					dictionary.Add(propertyInfo, eDC_SpaltenInformation);
				}
			}
			return dictionary;
		}

		public static Dictionary<PropertyInfo, EDC_FilterInformationen> FUN_fdcHoleFilterInformationen(Type i_edcType)
		{
			Dictionary<PropertyInfo, EDC_FilterInformationen> dictionary = new Dictionary<PropertyInfo, EDC_FilterInformationen>();
			PropertyInfo[] properties = i_edcType.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				EDC_FilterInformationen eDC_FilterInformationen = propertyInfo.GetCustomAttributes(inherit: false).FirstOrDefault((object i_fdcObject) => i_fdcObject.GetType() == typeof(EDC_FilterInformationen)) as EDC_FilterInformationen;
				if (eDC_FilterInformationen != null)
				{
					dictionary.Add(propertyInfo, eDC_FilterInformationen);
				}
			}
			return dictionary;
		}

		public static EDC_TabellenInformation FUN_strHoleTabellenInformationen(Type i_edcType)
		{
			EDC_TabellenInformation eDC_TabellenInformation = i_edcType.GetCustomAttributes(typeof(EDC_TabellenInformation), inherit: true).FirstOrDefault() as EDC_TabellenInformation;
			if (eDC_TabellenInformation != null)
			{
				return eDC_TabellenInformation;
			}
			return null;
		}
	}
}
