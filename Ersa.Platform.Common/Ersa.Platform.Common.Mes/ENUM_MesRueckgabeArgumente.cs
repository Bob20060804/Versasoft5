using Ersa.Platform.Common.Data.Nutzen;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Mes
{
	public enum ENUM_MesRueckgabeArgumente
	{
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.RezeptAnfordern, typeof(EDC_CodeMitVerwendungUndBedeutung), "Recipecode")]
		RezeptAnfordernRezeptcode,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(ENUM_EinlaufStatus), "State")]
		EinlaufFreigabeAnfordernEinlaufStatus,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(string), "ErrorDescription")]
		EinlaufFreigabeAnfordernFehlerbeschreibung,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(EDC_CodeMitVerwendungUndBedeutung), "Recipecode")]
		EinlaufFreigabeAnfordernRezeptcode,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(IEnumerable<EDC_NutzenData>), "PanelData")]
		EinlaufFreigabeAnfordernNutzendaten,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.PcbProzessParameterSenden, typeof(ENUM_EinlaufStatus), "State")]
		PcbProzessParameterSendenEinlaufStatus,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.UserLoginAnfordern, typeof(EDC_RechteKonstanten), "UserPermission")]
		UserLoginAnfordernBenutzerRechte,
		[EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen.UserLoginAnfordern, typeof(string), "UserName")]
		UserLoginAnfordernUserName
	}
}
