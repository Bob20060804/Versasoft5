using Ersa.Platform.Common.Codetabellen;

namespace Ersa.Platform.DataDienste.Codetabelle.Interfaces
{
	public interface INF_CodetabellenImportExportDienst
	{
		EDC_Codetabelle FUN_edcImport(string i_strDateiPfad);

		void SUB_Export(EDC_Codetabelle i_edcCodetabelle, string i_strDateiPfad);
	}
}
