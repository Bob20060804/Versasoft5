using Ersa.Platform.Common.Loetprogramm;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public interface INF_BibOderPrgImportHandler
	{
		ENUM_ImportFormat PRO_enmImportFormat
		{
			get;
		}

		EDC_ProgrammImportOptionen PRO_edcProgrammImportOptionen
		{
			get;
		}

		void SUB_BibliothekAuswahlValidieren(string i_strPfad);

		void SUB_ProgrammAuswahlValidieren(string i_strPfad);

		string FUN_strBibliothekNameAusPfadErmitteln(string i_strPfad);

		string FUN_strProgrammNameAusPfadErmitteln(string i_strPfad);

		Task<long> FUN_i64BibliothekImportierenAsnyc(string i_strImportPfad, string i_strNeuerBibliothekName);

		Task<long> FUN_i64ProgrammImportierenAsync(string i_strImportPfad, long i_i64BibliothekId, string i_strNeuerProgrammName);
	}
}
