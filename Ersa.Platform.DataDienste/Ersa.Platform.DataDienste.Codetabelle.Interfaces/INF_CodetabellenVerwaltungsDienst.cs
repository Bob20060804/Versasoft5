using Ersa.Platform.Common.Codetabellen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle.Interfaces
{
	public interface INF_CodetabellenVerwaltungsDienst
	{
		Task<EDC_Codetabelle> FUN_edcAktiveCodetabelleErmittelnAsync();

		Task FUN_fdcAktiveCodetabelleSetzenAsync(long i_i64IdAktiveCodetabelle);

		Task<IEnumerable<EDC_Codetabelle>> FUN_enuCodetabellenErmittelnAsync();

		Task FUN_fdcNeueCodetabelleSpeichernAsync(EDC_Codetabelle i_edcCodetabelle);

		Task FUN_fdcGeaenderteCodetabelleSpeichernAsync(EDC_Codetabelle i_edcCodetabelle);

		Task FUN_fdcCodetabelleLoeschenAsync(EDC_Codetabelle i_edcCodetabelle);

		Task FUN_fdcCodetabelleExportierenAsync(EDC_Codetabelle i_edcCodetabelle, string i_strDateiPfad);

		Task<EDC_Codetabelle> FUN_edcCodetabelleImportierenAsync(string i_strDateiPfad);
	}
}
