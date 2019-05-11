using System;

namespace Ersa.Platform.Common.Mes
{
	public static class EDC_MesRueckgabeArgumentErweiterung
	{
		public static ENUM_MesFunktionen? FUN_objHoleFunktion(this ENUM_MesRueckgabeArgumente i_enmWert)
		{
			EDC_MesRueckgabeArgumenteAttribut[] array = i_enmWert.GetType().GetField(i_enmWert.ToString()).GetCustomAttributes(typeof(EDC_MesRueckgabeArgumenteAttribut), inherit: false) as EDC_MesRueckgabeArgumenteAttribut[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0].PRO_enmMesFunktion;
		}

		public static Type FUN_objHoleTyp(this ENUM_MesRueckgabeArgumente i_enmWert)
		{
			EDC_MesRueckgabeArgumenteAttribut[] array = i_enmWert.GetType().GetField(i_enmWert.ToString()).GetCustomAttributes(typeof(EDC_MesRueckgabeArgumenteAttribut), inherit: false) as EDC_MesRueckgabeArgumenteAttribut[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0].PRO_objTyp;
		}

		public static string FUN_objHoleBezeichner(this ENUM_MesRueckgabeArgumente i_enmWert)
		{
			EDC_MesRueckgabeArgumenteAttribut[] array = i_enmWert.GetType().GetField(i_enmWert.ToString()).GetCustomAttributes(typeof(EDC_MesRueckgabeArgumenteAttribut), inherit: false) as EDC_MesRueckgabeArgumenteAttribut[];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[0].PRO_strBezeichner;
		}
	}
}
