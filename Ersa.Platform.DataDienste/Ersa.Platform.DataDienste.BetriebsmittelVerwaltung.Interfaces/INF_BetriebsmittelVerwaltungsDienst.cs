using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces
{
	public interface INF_BetriebsmittelVerwaltungsDienst
	{
		Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcLeseBetriebsmittelDatenFuerTypAsync(ENUM_BetriebsmittelTyp i_enuBetriebsmittelTyp);

		Task<EDC_BetriebsmittelData> FUN_fdcLeseBetriebsmittelDatenFuerIdAsync(long i_i64BetriebsmittelId);

		Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcBetriebsmittelDatenSaetzeHinzufuegenAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData);

		Task FUN_fdcBetriebsmittelDatenSaetzeAendernAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData);

		Task FUN_fdcBetriebsmittelDatenSaetzeLoeschenAsync(IEnumerable<EDC_BetriebsmittelData> i_enuBetriebsmittelData);
	}
}
