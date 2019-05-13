using System;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public class EDC_BibOderPrgImportAbfrageItem
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

		public Func<bool, EDC_BibliothekId, string, string> PRO_delImportOptionenValidierer
		{
			get;
			set;
		}

		public Func<string, EDC_BibliothekId, string, Task<string>> PRO_delPrgImport
		{
			get;
			set;
		}

		public Func<string, string, Task<string[]>> PRO_delBibImport
		{
			get;
			set;
		}
	}
}
