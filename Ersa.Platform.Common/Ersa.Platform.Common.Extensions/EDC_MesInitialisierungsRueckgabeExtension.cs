using Ersa.Platform.Common.Mes;

namespace Ersa.Platform.Common.Extensions
{
	public static class EDC_MesInitialisierungsRueckgabeExtension
	{
		public static EDC_MesInitialisierungsRueckgabe FUN_edcRueckgabe(this EDC_MesInitialisierungsRueckgabe i_edcRueckgabe, ENUM_MesInitialisierungsStatus i_enuInitialisierungsStatus, int i_i32Fehlerkey, string i_strFehlerErweiterung = "", string i_strStacktrace = "")
		{
			return new EDC_MesInitialisierungsRueckgabe
			{
				PRO_enuInitialisierungsStatus = i_enuInitialisierungsStatus,
				PRO_i32FehlerKey = i_i32Fehlerkey,
				PRO_strFehlerErweiterung = i_strFehlerErweiterung,
				PRO_strStacktrace = i_strStacktrace
			};
		}

		public static EDC_MesInitialisierungsRueckgabe FUN_edcRueckgabeErfolgreich(this EDC_MesInitialisierungsRueckgabe i_edcRueckgabe)
		{
			return new EDC_MesInitialisierungsRueckgabe
			{
				PRO_enuInitialisierungsStatus = ENUM_MesInitialisierungsStatus.Erfolgreich,
				PRO_i32FehlerKey = 0,
				PRO_strFehlerErweiterung = string.Empty,
				PRO_strStacktrace = string.Empty
			};
		}

		public static EDC_MesInitialisierungsRueckgabe FUN_edcRueckgabeFehlerhaft(this EDC_MesInitialisierungsRueckgabe i_edcRueckgabe, int i_i32Fehlerkey, string i_strFehlerErweiterung = "", string i_strStacktrace = "")
		{
			return new EDC_MesInitialisierungsRueckgabe
			{
				PRO_enuInitialisierungsStatus = ENUM_MesInitialisierungsStatus.Fehlerhaft,
				PRO_i32FehlerKey = i_i32Fehlerkey,
				PRO_strFehlerErweiterung = i_strFehlerErweiterung,
				PRO_strStacktrace = i_strStacktrace
			};
		}
	}
}
