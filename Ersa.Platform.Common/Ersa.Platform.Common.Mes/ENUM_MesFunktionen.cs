using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
    /// <summary>
    /// Enum_MesFunction
    /// </summary>
	public enum ENUM_MesFunktionen
	{
		[Description("4_12129")]
		[EDC_MesFunktionenAttribut("SendOeeCyclical")]
		OeeZyklischSenden,
		[Description("4_12130")]
		[EDC_MesFunktionenAttribut("SendOeeChanged")]
		OeeAenderungSenden,
		[Description("4_12131")]
		[EDC_MesFunktionenAttribut("SendMessageAppeared")]
		MeldungAufgetretenSenden,
		[Description("4_12132")]
		[EDC_MesFunktionenAttribut("SendMessageQuitted")]
		MeldungQuittiertSenden,
		[Description("4_12133")]
		[EDC_MesFunktionenAttribut("SendMessageAutomaticQuitted")]
		MeldungAutomatischQuittiertSenden,
		[Description("4_12134")]
		[EDC_MesFunktionenAttribut("SendMessageReset")]
		MeldungZurueckgesetztSenden,
		[Description("4_12135")]
		[EDC_MesFunktionenAttribut("RequestRecipe")]
		RezeptAnfordern,
		[Description("4_12136")]
		[EDC_MesFunktionenAttribut("SendRequestedRecipeAcknowledged")]
		AngefordertesRezeptBestaetigenSenden,
		[Description("4_12137")]
		[EDC_MesFunktionenAttribut("SendRecipeChanged")]
		RezeptAenderungSenden,
		[Description("4_12138")]
		[EDC_MesFunktionenAttribut("RequestInfeedRelease")]
		EinlaufFreigabeAnfordern,
		[Description("4_12139")]
		[EDC_MesFunktionenAttribut("RequestBadBaordInformation")]
		BadBoardInformationenAnfordern,
		[Description("4_12140")]
		[EDC_MesFunktionenAttribut("SendPcbProcessingStarted")]
		StartPcbBearbeitungSenden,
		[Description("4_12141")]
		[EDC_MesFunktionenAttribut("RequestOutfeedRelease")]
		AuslaufFreigabeAnfordern,
		[Description("4_12142")]
		[EDC_MesFunktionenAttribut("SendPcbProcessingEnded")]
		EndePcbBearbeitungSenden,
		[Description("4_12143")]
		[EDC_MesFunktionenAttribut("SendPcbProcessParameter")]
		PcbProzessParameterSenden,
		[Description("4_12144")]
		[EDC_MesFunktionenAttribut("RequestMaterialChange")]
		RuestmaterialAenderungAnfordern,
		[Description("4_12145")]
		[EDC_MesFunktionenAttribut("SendMaterialUnlaoded")]
		RuestmaterialAbmeldungSenden,
		[Description("4_12146")]
		[EDC_MesFunktionenAttribut("SendMaterialLoaded")]
		RuestmaterialAnmeldungSenden,
		[Description("4_12147")]
		[EDC_MesFunktionenAttribut("RequestToolChange")]
		RuestwerkzeugAenderungAnfordern,
		[Description("4_12148")]
		[EDC_MesFunktionenAttribut("SendToolUnloaded")]
		RuestwerkzeugAbmeldungSenden,
		[Description("4_12149")]
		[EDC_MesFunktionenAttribut("SendToolLoaded")]
		RuestwerkzeugAnmeldungSenden,
		[Description("4_12150")]
		[EDC_MesFunktionenAttribut("SendPing")]
		PingSenden,
		[Description("4_12163")]
		[EDC_MesFunktionenAttribut("RequestUserLogin")]
		UserLoginAnfordern,
		[Description("4_12164")]
		[EDC_MesFunktionenAttribut("SendUserLogout")]
		UserLogoutSenden,
		[Description("4_316")]
		[EDC_MesFunktionenAttribut("SendConveyorWidth")]
		SendConveyorWidth,
		[Description("4_12197")]
		[EDC_MesFunktionenAttribut("CarrierUnassign")]
		CarrierUnassignSenden,
		[Description("4_401")]
		[EDC_MesFunktionenAttribut("SendCodes")]
		SendCodes
	}
}
