using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_BibOderPrgImportAbfrageFormatItem
	{
		public string PRO_strPrgDateiFilter
		{
			get;
			set;
		}

		public Func<bool, string, string[]> PRO_delAuswahlValidierer
		{
			get;
			set;
		}

		public bool PRO_blnPrgAuswahlAlsVerzeichnis
		{
			get;
			set;
		}

		public Func<string, string> PRO_delBibNameAusPfad
		{
			get;
			set;
		}

		public Func<string, string> PRO_delPrgNameAusPfad
		{
			get;
			set;
		}

		public Func<bool, EDC_BibliothekIdentifier, string, string> PRO_delImportOptionenValidierer
		{
			get;
			set;
		}

		public Func<string, EDC_BibliothekIdentifier, string, Task<string>> PRO_delPrgImport
		{
			get;
			set;
		}

		public Func<string, string, Task<string[]>> PRO_delBibImport
		{
			get;
			set;
		}

		public static EDC_BibOderPrgImportAbfrageItem FUN_edcKonvertieren(EDC_BibOderPrgImportAbfrageFormatItem i_edcItem)
		{
			return new EDC_BibOderPrgImportAbfrageItem
			{
				PRO_blnPrgAuswahlAlsVerzeichnis = i_edcItem.PRO_blnPrgAuswahlAlsVerzeichnis,
				PRO_strPrgDateiFilter = i_edcItem.PRO_strPrgDateiFilter,
				PRO_delAuswahlValidierer = i_edcItem.PRO_delAuswahlValidierer,
				PRO_delBibNameAusPfad = i_edcItem.PRO_delBibNameAusPfad,
				PRO_delPrgNameAusPfad = i_edcItem.PRO_delPrgNameAusPfad,
				PRO_delImportOptionenValidierer = ((bool i_blnBib, EDC_BibliothekId i_edcBibAuswahl, string i_strPrgName) => i_edcItem.PRO_delImportOptionenValidierer(i_blnBib, new EDC_BibliothekIdentifier
				{
					PRO_i64BibliothekId = (i_edcBibAuswahl?.PRO_i64BibliothekId ?? 0),
					PRO_strBibliotheksName = (i_edcBibAuswahl?.PRO_strBibliotheksName ?? string.Empty)
				}, i_strPrgName)),
				PRO_delPrgImport = ((string i_strPfad, EDC_BibliothekId i_edcBib, string i_strPrgName) => i_edcItem.PRO_delPrgImport(i_strPfad, new EDC_BibliothekIdentifier
				{
					PRO_i64BibliothekId = i_edcBib.PRO_i64BibliothekId,
					PRO_strBibliotheksName = i_edcBib.PRO_strBibliotheksName
				}, i_strPrgName)),
				PRO_delBibImport = i_edcItem.PRO_delBibImport
			};
		}
	}
}
