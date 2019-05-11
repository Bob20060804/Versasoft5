using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.Common.Loetprotokoll;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Mes
{
	[Serializable]
	public enum ENUM_MesMaschinenDatenArgumente
	{
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.OeeZyklischSenden, typeof(int), "Code")]
		OeeZyklischSendenCode,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.OeeZyklischSenden, typeof(string), "Name")]
		OeeZyklischSendenName,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.OeeAenderungSenden, typeof(int), "Code")]
		OeeAenderungSendenCode,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.OeeAenderungSenden, typeof(string), "Name")]
		OeeAenderungSendenName,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.RezeptAnfordern, typeof(IEnumerable<EDC_CodeMitVerwendungUndBedeutung>), "Codes")]
		RezeptAnfordernCodes,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(IEnumerable<EDC_CodeMitVerwendungUndBedeutung>), "Codes")]
		EinlaufFreigabeAnfordernCodes,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(string), "Library")]
		EinlaufFreigabeAnfordernBibliothek,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.EinlaufFreigabeAnfordern, typeof(string), "Program")]
		EinlaufFreigabeAnfordernProgramm,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.PcbProzessParameterSenden, typeof(IEnumerable<EDC_LoetprotokollDaten>), "Protocol")]
		PcbProzessParameterSendenProtokoll,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.RuestmaterialAbmeldungSenden, typeof(string), "MaterialNumber")]
		RuestmaterialAbmeldungSendenMaterialnummer,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.RuestmaterialAbmeldungSenden, typeof(string), "Position")]
		RuestmaterialAbmeldungSendenPosition,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.RuestmaterialAnmeldungSenden, typeof(string), "MaterialNumber")]
		RuestmaterialAnmeldungSendenMaterialnummer,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.RuestmaterialAnmeldungSenden, typeof(string), "Position")]
		RuestmaterialAnmeldungSendenPosition,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.UserLoginAnfordern, typeof(string), "UserId")]
		LoginUserId,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.UserLogoutSenden, typeof(string), "UserId")]
		LogOutUserId,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.SendConveyorWidth, typeof(float), "ConveyorWidth")]
		ConveyorWidth,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.CarrierUnassignSenden, typeof(IEnumerable<EDC_LoetprotokollDaten>), "Protocol")]
		CarrierUnassignSendenProtokoll,
		[EDC_MesMaschinenDatenArgumenteAttribut(ENUM_MesFunktionen.SendCodes, typeof(IEnumerable<EDC_CodeMitVerwendungUndBedeutung>), "Codes")]
		SendCodesCode
	}
}
