using System;

namespace Ersa.Platform.Common
{
	public static class EDC_Konstanten
	{
		public const double C_dblStandardMinimalWert = -10000.0;

		public const double C_dblStandardMaximalWert = 10000.0;

		public const int gC_i32MaxLaengeCode = 399;

		public const int mC_i32MaximaleZeichenOrdnerName = 31;

		public const string mC_strDefaultDatenDirectory = "Default";

		public const string mC_strPlcDateienSicherungsVerzeichnis = "Plc";

		public const string mC_strLokaleDateienSicherungsVerzeichnis = "Ersasoft";

		public const string gC_strCodetabellenxportUnterverzeichnisName = "Code";

		public const string gC_strProduktionexportUnterverzeichnisName = "ProductionSettings";

		public const string gC_strMesExportUnterverzeichnisName = "Mes";

		public const string gC_strLoetprogrammBibliothekExportUnterverzeichnisName = "Bibs";

		public const string mC_strServiceTabContainer = "ServiceTabContainer";

		public const string mC_strAllgemeineEinstellungenTabContainer = "AllgemeineEinstellungenTabContainer";

		public const string mC_strDashboardTabContainer = "DashboardTabContainer";

		public const string mC_strProgrammeTabContainer = "ProgrammeTabContainer";

		public const string mC_strPfadExport = "PfadExport";

		public const string mC_strPfadBackup = "PfadBackup";

		public const string mC_strPfadHilfe = "PfadHilfe";

		public const string mC_strPfadVideoaufzeichnung = "PfadVideos";

		public const string mC_strSprache = "Sprache";

		public const string mC_strExportSprache = "ExportSprache";

		public const string mC_strPfadLogging = "PfadLogging";

		public const string mC_strLogDateinamePrefix = "ERSASOFT";

		public const string mC_strLogDateinameEndung = "log";

		public const string mC_strLoggingLevel = "LoggingLevel";

		public const string mC_strConfigKeyMaschinenBasisTyp = "MachineBaseType";

		public const string mC_strConfigKeyMaschinenTyp = "MaschinenTyp";

		public const string mC_strConfigKeySpsIdAdresse = "SpsIpAdresse";

		public const string mC_strConfigKeySpsTyp = "SpsTyp";

		public const string mC_strConfigKeySpsFtpPartition = "SpsFtpPartition";

		public const string mC_strTransferZeit = "TransferTimeInMilliSek";

		public const string mC_strZeigeSpsVariablen = "ZeigeSpsVariablen";

		public const string mC_strStarteDiagnoseServer = "StartDiagnosticServer";

		public const string mC_strConfigKeyZveiXsdDatei = "UnitDataXsd";

		public const string mC_strConfigKeyZveiRelativeToleranzen = "RelativeLimits";

		public const string mC_strConfigKeyZveiBearbeitungsStatusOk = "ProcessingStateOk";

		public const string mC_strConfigKeyZveiBearbeitungsStatusNok = "ProcessingStateNok";

		public const string mC_strConfigKeyZveiEquipment = "Equipment";

		public const string gC_strConfigKeyEditorAnwendung = "EditorApplication";

		public const string gC_strConfigKeyBetriebsfallImport = "BetriebsfallImport";

		public const string gC_strConfigKeySettingsDateipfad = "SettingsFilePath";

		public const string gC_strConfigKeySicherungsIntervall = "BackupInterval";

		public const string gC_strConfigKeyMesKommunikationsTyp = "MesUrlMesKommunikationsTyp";

		public const string gC_strConfigKeyPrgImmerVollstaendigUebertragen = "PrgImmerVollstaendigUebertragen";

		public const string gC_strConfigKeyGemeinsamePrgsNutzbar = "UseSharedPrograms";

		public const string gC_strConfigKeyDuesenzaehlerEinblenden = "UseNozzleTimer";

		public const string gC_strConfigKeyAnleitungDateipfad = "ManualFilePath";

		public const string gC_strConfigKeyWebVisuPort = "WebVisuPort";

		public const string gC_strConfigKeyWebVisuStartPage = "WebVisuStartPage";

		public const string gC_strCloudServiceAvailable = "CloudServiceAvailable";

		public const string mC_strIconOkStatusGruen = "/Ersa.Global.Controls;component/Bilder/Icons/Icon_Status_Gruen_24x24.png";

		public const string gC_strIdentifierSerialNumber = "enmProt|enmCodes|enmSerialNumber";

		public const string gC_strIdentifierProgram = "enmProt|enmCodes|enmProgram";

		public const string gC_strIdentifierCarrier = "enmProt|enmCodes|enmCarrier";

		public const string gC_strIdentifierInfeedStopper = "enmProt|enmCodes|enmInfeedStopper";

		public const string gC_strIdentifierProtocol = "enmProt|enmCodes|enmProtocol";

		public const string gC_strIdentifierMesInfeedReleaseAndRecipe = "enmProt|enmCodes|enmMesInfeedReleaseAndRecipe";

		public const string gC_strIdentifierUndefined = "enmProt|enmCodes|enmUndefined";

		public const string gC_strIdentifierFallback = "enmProt|enmCodes|enmFallback";

		public const string gC_strIdentifierOrder = "enmProt|enmCodes|enmOrder";

		public const string gC_strIdentifierCharge = "enmProt|enmCodes|enmCharge";

		public const string gC_strIdentifierPanel = "enmProt|enmCodes|enmPanel";

		public const string gC_strIdentifierProduct = "enmProt|enmCodes|enmProduct";

		public const string gC_strIdentifierRecipeCode = "enmProt|enmCodes|enmRecipeCode";

		public const string gC_strLoetgutPcbFertigGeloetet = "enmProt|enmLoetgut|enmBlnPcbFertigGeloet";

		public const string gC_strLoetgutPcbAusgelaufen = "enmProt|enmLoetgut|enmBlnPcbAusgelaufen";

		public const string gC_strLoetgutTaktzeit = "enmProt|enmLoetgut|enmSngTaktzeit";

		public const string gC_strLoetgutDurchlaufzeit = "enmProt|enmLoetgut|enmSngDurchlaufzeit";

		public const string gC_strLoetgutBearbeitenExternIdentifier = "enmProt|enmLoetgut|enmBearbeitenExtern";

		public const string gC_strLoetgutNutzenPositionIdentifier = "enmProt|enmLoetgut|enmNutzenPosition";

		public const string gC_strLoetgutBearbeitenExternParameter = "ProcessExternal";

		public const string gC_strLoetgutNutzenPositionParameter = "PanelPosition";

		public const string gC_strCodeVideoLink = "VideoLink";

		public static readonly DateTime ms_dtmDefaultDatumUndZeit = new DateTime(1970, 1, 1, 0, 0, 0);

		public static readonly TimeSpan ms_sttUhrzeitMin = TimeSpan.Zero;

		public static readonly TimeSpan ms_sttUhrzeitMax = new TimeSpan(23, 59, 59);
	}
}
