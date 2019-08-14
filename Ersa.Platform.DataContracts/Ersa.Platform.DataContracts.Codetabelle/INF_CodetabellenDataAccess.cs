using Ersa.Platform.Common.Data.Codetabelle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Codetabelle
{
	public interface INF_CodetabellenDataAccess : INF_DataAccess
	{
		Task<EDC_CodetabelleData> FUN_edcCodetabelleLadenAsync(long i_i64CodetabellenId);

		Task<IEnumerable<EDC_CodetabelleData>> FUN_fdcCodetabellenLadenAsync(long i_i64MaschinenId);

		Task<IEnumerable<EDC_CodetabelleneintragData>> FUN_fdcCodetabelleneintraegeLadenAsync(long i_i64CodetabellenId);

		Task<long> FUN_fdcCodetabelleMitEintraegenHinzufuegenAsync(string i_strCodetabellenName, IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege, long i_i64BenutzerId, long i_i64MaschinenGruppenId);

		Task FUN_fdcCodetabelleMitEintraegenAendernAsync(long i_i64CodeTabellenId, string i_strNeuerName, IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege, long i_i64BenutzerId);

		Task FUN_fdcCodetabelleMitEintraegenLoeschenAsync(long i_i64CodetabellenId);
	}
}
