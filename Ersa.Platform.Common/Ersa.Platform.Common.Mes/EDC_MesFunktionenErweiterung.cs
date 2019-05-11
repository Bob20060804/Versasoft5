using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Mes
{
	public static class EDC_MesFunktionenErweiterung
	{
		public static string FUN_objHoleBezeichner(this ENUM_MesFunktionen i_enmWert)
		{
			EDC_MesFunktionenAttribut[] array = i_enmWert.GetType().GetField(i_enmWert.ToString()).GetCustomAttributes(typeof(EDC_MesFunktionenAttribut), inherit: false) as EDC_MesFunktionenAttribut[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0].PRO_strBezeichner;
		}

		public static List<ENUM_MesMaschinenDatenArgumente> FUN_edcHoleMaschinenDatenArgumente(this ENUM_MesFunktionen i_enmWert)
		{
			List<ENUM_MesMaschinenDatenArgumente> list = new List<ENUM_MesMaschinenDatenArgumente>();
			foreach (object value in Enum.GetValues(typeof(ENUM_MesMaschinenDatenArgumente)))
			{
				if (((ENUM_MesMaschinenDatenArgumente)value).FUN_objHoleFunktion() == i_enmWert)
				{
					list.Add((ENUM_MesMaschinenDatenArgumente)value);
				}
			}
			return list;
		}
	}
}
