using Ersa.Platform.Common.Mes;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Extensions
{
	public static class EDC_MesKommunikationsRueckgabeExtension
	{
		public static EDC_MesKommunikationsRueckgabe FUN_edcRueckgabe(this EDC_MesKommunikationsRueckgabe i_edcRueckgabe, ENUM_MesKommunikationsStatus i_enuKommunikationsStatus, int i_i32Fehlerkey, string i_strFehlerErweiterung = "", string i_strStacktrace = "")
		{
			return new EDC_MesKommunikationsRueckgabe
			{
				PRO_enuKommunikationsStatus = i_enuKommunikationsStatus,
				PRO_i32FehlerKey = i_i32Fehlerkey,
				PRO_strFehlerErweiterung = i_strFehlerErweiterung,
				PRO_strStacktrace = i_strStacktrace
			};
		}

		public static EDC_MesKommunikationsRueckgabe FUN_edcRueckgabeErfolgreich(this EDC_MesKommunikationsRueckgabe i_edcRueckgabe)
		{
			return new EDC_MesKommunikationsRueckgabe
			{
				PRO_enuKommunikationsStatus = ENUM_MesKommunikationsStatus.Erfolgreich,
				PRO_i32FehlerKey = 0,
				PRO_strFehlerErweiterung = string.Empty,
				PRO_strStacktrace = string.Empty,
				PRO_dicArgumente = new Dictionary<ENUM_MesRueckgabeArgumente, object>()
			};
		}

		public static EDC_MesKommunikationsRueckgabe FUN_edcRueckgabeErfolgreich(this EDC_MesKommunikationsRueckgabe i_edcRueckgabe, Dictionary<ENUM_MesRueckgabeArgumente, object> i_dicArgumente)
		{
			return new EDC_MesKommunikationsRueckgabe
			{
				PRO_enuKommunikationsStatus = ENUM_MesKommunikationsStatus.Erfolgreich,
				PRO_i32FehlerKey = 0,
				PRO_strFehlerErweiterung = string.Empty,
				PRO_strStacktrace = string.Empty,
				PRO_dicArgumente = i_dicArgumente
			};
		}

		public static EDC_MesKommunikationsRueckgabe FUN_edcRueckgabeFehlerhaft(this EDC_MesKommunikationsRueckgabe i_edcRueckgabe, int i_i32Fehlerkey, string i_strFehlerErweiterung = "", string i_strStacktrace = "")
		{
			return new EDC_MesKommunikationsRueckgabe
			{
				PRO_enuKommunikationsStatus = ENUM_MesKommunikationsStatus.Fehlerhaft,
				PRO_i32FehlerKey = i_i32Fehlerkey,
				PRO_strFehlerErweiterung = i_strFehlerErweiterung,
				PRO_strStacktrace = i_strStacktrace,
				PRO_dicArgumente = new Dictionary<ENUM_MesRueckgabeArgumente, object>()
			};
		}

		public static EDC_MesKommunikationsRueckgabe FUN_edcRueckgabeFehlerhaft(this EDC_MesKommunikationsRueckgabe i_edcRueckgabe, int i_i32Fehlerkey, string i_strFehlerErweiterung, string i_strStacktrace, Dictionary<ENUM_MesRueckgabeArgumente, object> i_dicRueckgabeArgumente)
		{
			return new EDC_MesKommunikationsRueckgabe
			{
				PRO_enuKommunikationsStatus = ENUM_MesKommunikationsStatus.Fehlerhaft,
				PRO_i32FehlerKey = i_i32Fehlerkey,
				PRO_strFehlerErweiterung = i_strFehlerErweiterung,
				PRO_strStacktrace = i_strStacktrace,
				PRO_dicArgumente = i_dicRueckgabeArgumente
			};
		}
	}
}
