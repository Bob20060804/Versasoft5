using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Helfer;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common;
using Ersa.Platform.Data.DataAccess.Aoi;
using Ersa.Platform.Data.DataAccess.Bauteile;
using Ersa.Platform.Data.DataAccess.Benutzer;
using Ersa.Platform.Data.DataAccess.Betriebsfall;
using Ersa.Platform.Data.DataAccess.Betriebsmittelverwaltung;
using Ersa.Platform.Data.DataAccess.Cad;
using Ersa.Platform.Data.DataAccess.CodeCache;
using Ersa.Platform.Data.DataAccess.Codetabelle;
using Ersa.Platform.Data.DataAccess.Datenbankdaten;
using Ersa.Platform.Data.DataAccess.DatenbankVerwaltung;
using Ersa.Platform.Data.DataAccess.Duesentabelle;
using Ersa.Platform.Data.DataAccess.LeseSchreibgeraete;
using Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung;
using Ersa.Platform.Data.DataAccess.Loetprotokoll;
using Ersa.Platform.Data.DataAccess.Maschinenkonfiguration;
using Ersa.Platform.Data.DataAccess.Maschinenverwaltung;
using Ersa.Platform.Data.DataAccess.Meldungen;
using Ersa.Platform.Data.DataAccess.Nutzen;
using Ersa.Platform.Data.DataAccess.Produktionssteuerung;
using Ersa.Platform.Data.DataAccess.Prozessschreiber;
using Ersa.Platform.Data.DataAccess.Sprachen;
using Ersa.Platform.Data.DatenbankModelle;
using Ersa.Platform.Data.DatenModelle.Organisation;
using Ersa.Platform.DataContracts.Aoi;
using Ersa.Platform.DataContracts.Bauteile;
using Ersa.Platform.DataContracts.Benutzer;
using Ersa.Platform.DataContracts.Betriebsfall;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using Ersa.Platform.DataContracts.Cad;
using Ersa.Platform.DataContracts.CodeCache;
using Ersa.Platform.DataContracts.Codetabelle;
using Ersa.Platform.DataContracts.Datenbankdaten;
using Ersa.Platform.DataContracts.DatenbankVerwaltung;
using Ersa.Platform.DataContracts.Duesentabelle;
using Ersa.Platform.DataContracts.LeseSchreibgeraete;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts.Loetprotokoll;
using Ersa.Platform.DataContracts.Maschinenkonfiguration;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataContracts.Meldungen;
using Ersa.Platform.DataContracts.Nutzen;
using Ersa.Platform.DataContracts.Produktionssteuerung;
using Ersa.Platform.DataContracts.Prozessschreiber;
using Ersa.Platform.DataContracts.Sprache;
using Ersa.Platform.DataDienste.Exceptions;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Logging;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Configuration;

namespace Ersa.Platform.DataDienste
{
	[Export(typeof(INF_DatenzugriffInitialisierungsDienst))]
	public class EDC_DatenzugriffInitialisierungsDienst : INF_DatenzugriffInitialisierungsDienst
	{
		private const int mC_i32DatenbankVerbindungsTimeoutInMs = 6000;

		private const int mC_i32DatenbankVerbindungsVersuche = 5;

		private readonly INF_DataAccessProvider m_edcDataAccessProvider;

		private readonly INF_IODienst m_edcIoDienst;

		private EDC_Parameter _edcDatenbankVersionsParameter;

		[Export]
		public EDC_ElementVersion PRO_strModulVersion
		{
			get
			{
				Version i_fdcVersion = new Version((int)((_edcDatenbankVersionsParameter == null) ? 82 : _edcDatenbankVersionsParameter.PRO_i64Wert), 0, 0, 0);
				return new EDC_ElementVersion("Database", i_fdcVersion);
			}
		}

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_DatenzugriffInitialisierungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_IODienst i_edcIoDienst)
		{
			m_edcDataAccessProvider = i_edcDataAccessProvider;
			m_edcIoDienst = i_edcIoDienst;
		}

		public void SUB_ErsasoftInitialisieren()
		{
			SUB_SchliesseInitialisierungAb(ConfigurationManager.AppSettings);
			SUB_PruefeDatenbankversionUndFuehreUpdatesAus(ConfigurationManager.AppSettings);
			SUB_RegistriereDatenzugriff(ConfigurationManager.AppSettings);
		}

		private void SUB_SchliesseInitialisierungAb(NameValueCollection i_fdcAppSettings)
		{
			INF_DatenbankAdapter iNF_DatenbankAdapter = FUN_edcErstelleDatenbankAdapter(i_fdcAppSettings);
			INF_OrganisationsAdapter iNF_OrganisationsAdapter = FUN_edcErstelleDatenbankOrganisationsAdapter(i_fdcAppSettings);
			if (!iNF_OrganisationsAdapter.FUN_blnIstDatenbankDateibasiert() && !iNF_OrganisationsAdapter.FUN_blnPruefeVerbindung(6000, 5))
			{
				string text = $"Cannot connect to database-server:{Environment.NewLine}{iNF_OrganisationsAdapter.FUN_strHoleLetzteVerbindungsFehlermeldung()}";
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, text);
				throw new EDC_DatabaseConnectException(text);
			}
			if (!iNF_OrganisationsAdapter.FUN_blnExistiertDieDatenbank(iNF_DatenbankAdapter.PRO_edcDatenbankModell.PRO_strDatenbankName))
			{
				string i_strEintrag = $"database {iNF_DatenbankAdapter.PRO_edcDatenbankModell.PRO_strDatenbankName} does not exist! Start creation of database";
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag);
				iNF_OrganisationsAdapter.SUB_ErstelleDieDatenbank();
			}
			if (!iNF_DatenbankAdapter.FUN_blnExistiertDieTabelle("Parameter"))
			{
				string i_strEintrag2 = $"tables of database {iNF_DatenbankAdapter.PRO_edcDatenbankModell.PRO_strDatenbankName} does not exist! Start creation of database tables";
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag2);
				iNF_DatenbankAdapter.SUB_ErstelleDieDatenbankKomponenten();
			}
			if (!iNF_DatenbankAdapter.FUN_blnPruefeVerbindung(6000, 5))
			{
				string text2 = $"Cannot connect to database:{iNF_DatenbankAdapter.PRO_edcDatenbankModell.PRO_strDatenbankName}";
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, text2);
				throw new EDC_DatabaseConnectException(text2);
			}
		}

		private void SUB_PruefeDatenbankversionUndFuehreUpdatesAus(NameValueCollection i_fdcAppSettings)
		{
			INF_DatenbankAdapter edcDatenbankAdapter = FUN_edcErstelleDatenbankAdapter(i_fdcAppSettings);
			_edcDatenbankVersionsParameter = FUN_edcHoleDatenbankversionsParameter(edcDatenbankAdapter);
			int num = Convert.ToInt32(_edcDatenbankVersionsParameter.PRO_i64Wert);
			if (num < 82)
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "update database from version {i32AktuelleDatenbankversion} to new version {EDC_DataKonstanten.gC_i32ErforderlicheErsasoftDatenbankVersion}");
				Action<long> i_delParameterUpdateAction = delegate(long i_i64Wert)
				{
					EDC_Parameter i_edcObjekt = EDC_Parameter.FUN_edcErtselleVersionsUpdateParameter(i_i64Wert);
					edcDatenbankAdapter.SUB_UpdateObjekt(i_edcObjekt);
				};
				edcDatenbankAdapter.SUB_FuehreDatenbankUpdatesDurch(num, 82, i_delParameterUpdateAction);
			}
			_edcDatenbankVersionsParameter = FUN_edcHoleDatenbankversionsParameter(edcDatenbankAdapter);
			PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Info, $"current database version: {_edcDatenbankVersionsParameter.PRO_i64Wert}");
		}

		private EDC_Parameter FUN_edcHoleDatenbankversionsParameter(INF_DatenbankAdapter i_edcAdapter)
		{
			EDC_Parameter i_edcSelectObjekt = new EDC_Parameter(EDC_Parameter.FUN_strDatenbankVersionWhereStatementErstellen());
			return i_edcAdapter.FUN_edcLeseObjekt(i_edcSelectObjekt);
		}

		private void SUB_RegistriereDatenzugriff(NameValueCollection i_fdcAppSettings)
		{
			INF_DatenbankAdapter edcDatenbankAdapter = FUN_edcErstelleDatenbankAdapter(i_fdcAppSettings);
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammBibliothekDataAccess>(() => new EDC_LoetprogrammBibliothekDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammProgrammDataAccess>(() => new EDC_LoetprogrammProgrammDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammValideDataAccess>(() => new EDC_LoetprogrammValideDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammVersionDataAccess>(() => new EDC_LoetprogrammVersionDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammParameterDataAccess>(() => new EDC_LoetprogrammParameterDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammSatzDatenDataAccess>(() => new EDC_LoetprogrammSatzDatenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammNutzenDatenDataAccess>(() => new EDC_LoetprogrammNutzenDatenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammEcp3DatenDataAccess>(() => new EDC_LoetprogrammEcp3DatenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammBildDataAccess>(() => new EDC_LoetprogrammBildDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CadBildDataAccess>(() => new EDC_CadBildDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CadDatenDataAccess>(() => new EDC_CadDatenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CadEinstellungenDataAccess>(() => new EDC_CadEinstellungenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_BenutzerVerwaltungDataAccess>(() => new EDC_BenutzerVerwaltungDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_BenutzerTrackingDataAccess>(() => new EDC_BenutzerTrackingDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_AktiveBenutzerDataAccess>(() => new EDC_AktiveBenutzerDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_SprachenDataAccess>(() => new EDC_SprachenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_MeldungenDataAccess>(() => new EDC_MeldungenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_MaschinenDataAccess>(() => new EDC_MaschinenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_DuesentabelleDataAccess>(() => new EDC_DuesentabelleDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_ProzessschreiberDataAccess>(() => new EDC_ProzessschreiberDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_MaschinenkonfigurationDataAccess>(() => new EDC_MaschinenkonfigurationDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammVersionDataAccess>(() => new EDC_LoetprogrammVersionDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprogrammBildDataAccess>(() => new EDC_LoetprogrammBildDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CodetabellenDataAccess>(() => new EDC_CodetabellenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_LoetprotokollDataAccess>(() => new EDC_LoetprotokollDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_ProduktionssteuerungDataAccess>(() => new EDC_ProduktionssteuerungDataAccess(edcDatenbankAdapter, m_edcIoDienst));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_BetriebsfallmportExportIDataAccess>(() => new EDC_BetriebsfallImportExportDataAccess(edcDatenbankAdapter, m_edcIoDienst));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CodeCacheDataAccess>(() => new EDC_CodeCacheDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_DatenbankdatenDataAccess>(() => new EDC_DatenbankdatenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_MaschinenEinstellungenDataAccess>(() => new EDC_MaschinenEinstellungenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_CodePipelineDataAccess>(() => new EDC_CodePipelineDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_DuesenbetriebDataAccess>(() => new EDC_DuesenbetriebDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_BauteilDataAccess>(() => new EDC_BauteilDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_NutzenDataAccess>(() => new EDC_NutzenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_BetriebsmittelDataAccess>(() => new EDC_BetriebsmittelDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_AoiDataAccess>(() => new EDC_AoiDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_DatenbankVerwaltungDataAccess>(() => new EDC_DatenbankVerwaltungDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_RuestkomponentenDataAccess>(() => new EDC_RuestkomponentenDataAccess(edcDatenbankAdapter));
			m_edcDataAccessProvider.SUB_DataAccessInterfaceRegistrieren<INF_RuestwerkzeugeDataAccess>(() => new EDC_RuestwerkzeugeDataAccess(edcDatenbankAdapter));
		}

		private INF_DatenbankAdapter FUN_edcErstelleDatenbankAdapter(NameValueCollection i_fdcAppSettings)
		{
			ENUM_DatenbankTyp i_enmDatenbankTyp = EDC_DatenbankEinstellungenHelfer.FUN_enmHoleDatenbankTyp(i_fdcAppSettings);
			string i_strDatenbankName = EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankName(i_fdcAppSettings);
			string i_strDatenverzeichnis = EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankVerzeichnis(i_fdcAppSettings);
			EDC_ErsasoftDatenbankModell i_edcDatenbankModell = new EDC_ErsasoftDatenbankModell(i_enmDatenbankTyp, i_strDatenbankName, i_strDatenverzeichnis);
			return EDC_AdapterFactory.FUN_edcErstelleDatenbankAdapter(i_fdcAppSettings, i_edcDatenbankModell);
		}

		private INF_OrganisationsAdapter FUN_edcErstelleDatenbankOrganisationsAdapter(NameValueCollection i_fdcAppSettings)
		{
			ENUM_DatenbankTyp i_enmDatenbankTyp = EDC_DatenbankEinstellungenHelfer.FUN_enmHoleDatenbankTyp(i_fdcAppSettings);
			string i_strDatenbankName = EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankName(i_fdcAppSettings);
			string i_strDatenverzeichnis = EDC_DatenbankEinstellungenHelfer.FUN_strHoleDatenbankVerzeichnis(i_fdcAppSettings);
			EDC_ErsasoftDatenbankModell i_edcDatenbankModell = new EDC_ErsasoftDatenbankModell(i_enmDatenbankTyp, i_strDatenbankName, i_strDatenverzeichnis);
			return EDC_AdapterFactory.FUN_edcErstelleOrganisationsAdapter(EDC_DatenbankEinstellungenHelfer.FUN_fdcErstelleServerEinstellungen(i_fdcAppSettings), i_edcDatenbankModell);
		}
	}
}
