using Ersa.Global.Common.FortsetzungsPolicy;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.DataContracts.Betriebsfall;
using Ersa.Platform.DataDienste.MaschinenVerwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Betriebsfall
{
	[Export(typeof(INF_BetriebsfallImportExportDienst))]
	public class EDC_BetriebsfallImportExportDienst : INF_BetriebsfallImportExportDienst
	{
		private const int mC_i32MaxVersucheWiederholbareOperation = 5;

		private const string mC_strMachinenDatenDateiName = "machineData.json";

		private const string mC_strDatenbankExportDateiName = "DatabaseExport.xml";

		private readonly INF_MaschinenVerwaltungsDienst m_edcMaschinenVerwaltungsDienst;

		private readonly INF_IODienst m_edcIoDienst;

		private readonly INF_ZipArchivDienst m_edcZipArchivDienst;

		private readonly INF_BenutzerInfoProvider m_edcBenutzerInfoProvider;

		private readonly INF_VisuSettingsDienst m_edcVisuSettingsDienst;

		private readonly Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst m_edcAppSettingsDienst;

		private readonly Lazy<INF_BetriebsfallmportExportIDataAccess> m_edcBetriebsfallExportImportDataAccess;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		[ImportingConstructor]
		public EDC_BetriebsfallImportExportDienst(INF_MaschinenVerwaltungsDienst i_edcMaschinenVerwaltungsDienst, INF_IODienst i_edcIoDienst, INF_ZipArchivDienst i_edcZipArchivDienst, INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, INF_BenutzerInfoProvider i_edcBenutzerInfoProvider, INF_VisuSettingsDienst i_edcVisuSettingsDienst, Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst i_edcAppSettingsDienst)
		{
			m_edcMaschinenVerwaltungsDienst = i_edcMaschinenVerwaltungsDienst;
			m_edcIoDienst = i_edcIoDienst;
			m_edcZipArchivDienst = i_edcZipArchivDienst;
			m_edcBenutzerInfoProvider = i_edcBenutzerInfoProvider;
			m_edcVisuSettingsDienst = i_edcVisuSettingsDienst;
			m_edcAppSettingsDienst = i_edcAppSettingsDienst;
			m_edcBetriebsfallExportImportDataAccess = new Lazy<INF_BetriebsfallmportExportIDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_BetriebsfallmportExportIDataAccess>);
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
		}

		public async Task FUN_fdcImportierenAsync(string i_strImportDateiPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcFortschrittsEmpfaenger)
		{
			string strTempVerzeichnisPfad = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			try
			{
				if (m_edcIoDienst.FUN_blnVerzeichnisExistiert(strTempVerzeichnisPfad))
				{
					m_edcIoDienst.SUB_VerzeichnisRekursivLoeschen(strTempVerzeichnisPfad);
				}
				m_edcIoDienst.SUB_VerzeichnisErstellen(strTempVerzeichnisPfad);
				m_edcZipArchivDienst.SUB_ZipArchivEntpacken(i_strImportDateiPfad, strTempVerzeichnisPfad);
				string i_strImportDatei = Path.Combine(strTempVerzeichnisPfad, "DatabaseExport.xml");
				await m_edcBetriebsfallExportImportDataAccess.Value.FUN_fdcImportBetriebsdatenDataAsync(i_strImportDatei);
				string i_strPfad = Path.Combine(strTempVerzeichnisPfad, "machineData.json");
				await FUN_fdcOperationenMitWiederholungAusfuehrenAsync(m_edcMaschinenVerwaltungsDienst.FUN_enuMaschinenDatenLadenOperationenErstellen(i_strPfad, i_fdcFortschrittsEmpfaenger));
				string i_strPfad2 = Path.Combine(strTempVerzeichnisPfad, "Ersasoft");
				string fullPath = Path.GetFullPath("./");
				string text = m_edcAppSettingsDienst.FUN_strAppSettingErmitteln("SettingsFilePath");
				if (!string.IsNullOrEmpty(text))
				{
					string fileName = Path.GetFileName(text);
					m_edcIoDienst.SUB_VerzeichnisKopieren(i_strPfad2, fullPath, i_blnUeberschreiben: true, fileName);
				}
				else
				{
					m_edcIoDienst.SUB_VerzeichnisKopieren(i_strPfad2, fullPath, i_blnUeberschreiben: true);
				}
			}
			finally
			{
				if (m_edcIoDienst.FUN_blnVerzeichnisExistiert(strTempVerzeichnisPfad))
				{
					m_edcIoDienst.SUB_VerzeichnisRekursivLoeschen(strTempVerzeichnisPfad);
				}
			}
		}

		public async Task FUN_fdcExportierenAsync(string i_strExportDateiPfad, int i_i32MaxDateiGroesseInKb)
		{
			if (string.IsNullOrEmpty(i_strExportDateiPfad))
			{
				throw new ArgumentNullException("i_strExportDateiPfad");
			}
			string directoryName = Path.GetDirectoryName(i_strExportDateiPfad);
			if (string.IsNullOrEmpty(directoryName))
			{
				throw new ArgumentException("Invalid export path. Parent directory could not be determined.", "i_strExportDateiPfad");
			}
			string strTempVerzeichnisPfad = Path.Combine(directoryName, "temp");
			try
			{
				if (m_edcIoDienst.FUN_blnVerzeichnisExistiert(strTempVerzeichnisPfad))
				{
					m_edcIoDienst.SUB_VerzeichnisRekursivLoeschen(strTempVerzeichnisPfad);
				}
				m_edcIoDienst.SUB_VerzeichnisErstellen(strTempVerzeichnisPfad);
				string i_strPfad = Path.Combine(strTempVerzeichnisPfad, "machineData.json");
				await FUN_fdcOperationenMitWiederholungAusfuehrenAsync(m_edcMaschinenVerwaltungsDienst.FUN_enuMaschinenDatenSichernOperationenErstellen(i_strPfad, m_edcBenutzerInfoProvider.PRO_i64BenutzerId));
				SUB_AnwendungsdateienKopieren(strTempVerzeichnisPfad);
				long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync();
				await m_edcBetriebsfallExportImportDataAccess.Value.FUN_fdcExportBetriebsdatenDataAsync(strTempVerzeichnisPfad, "DatabaseExport.xml", i_i64MaschinenId);
				m_edcZipArchivDienst.SUB_ZipArchivErstellen(strTempVerzeichnisPfad, new EDC_ZipErstellungsOptionen
				{
					PRO_strZielDateiPfad = i_strExportDateiPfad,
					PRO_i32MaxGroesseInKb = i_i32MaxDateiGroesseInKb
				});
			}
			finally
			{
				if (m_edcIoDienst.FUN_blnVerzeichnisExistiert(strTempVerzeichnisPfad))
				{
					m_edcIoDienst.SUB_VerzeichnisRekursivLoeschen(strTempVerzeichnisPfad);
				}
			}
		}

		private static async Task FUN_fdcOperationenMitWiederholungAusfuehrenAsync(IEnumerable<Func<Task>> i_enuOperationen)
		{
			EDC_FortsetzungsPolicy edcFortsetzungsPolicy = new EDC_FortsetzungsPolicy();
			if ((await edcFortsetzungsPolicy.FUN_fdcOperationenAusfuehrenAsync(i_enuOperationen)).PRO_blnErfolgreich)
			{
				return;
			}
			int i32Versuche = 0;
			STRUCT_OperationsErgebnis sTRUCT_OperationsErgebnis;
			do
			{
				sTRUCT_OperationsErgebnis = await edcFortsetzungsPolicy.FUN_fdcOperationWiederholenUndFortsetzenAsync();
				if (sTRUCT_OperationsErgebnis.PRO_blnErfolgreich)
				{
					return;
				}
			}
			while (i32Versuche++ <= 5);
			throw sTRUCT_OperationsErgebnis.PRO_fdcException;
		}

		private void SUB_AnwendungsdateienKopieren(string i_strTempVerzeichnisPfad)
		{
			string i_strPfad = m_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadLogging");
			string i_strSearchPattern = string.Format("{0}*.{1}", "ERSASOFT", "log");
			IEnumerable<string> source = m_edcIoDienst.FUN_enuHoleDateiListeAusVerzeichnis(i_strPfad, i_strSearchPattern);
			INF_IODienst edcIoDienst = m_edcIoDienst;
			IEnumerable<string> first = source.OrderByDescending(edcIoDienst.FUN_dtmDateiDatumErmitteln).Take(2);
			string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			foreach (string item in first.Union(new string[1]
			{
				configurationFile
			}).ToList())
			{
				if (!string.IsNullOrEmpty(item))
				{
					string i_strDestinationPfad = Path.Combine(i_strTempVerzeichnisPfad, Path.GetFileName(item));
					m_edcIoDienst.SUB_DateiKopieren(item, i_strDestinationPfad, i_blnUeberschreiben: false);
				}
			}
		}
	}
}
