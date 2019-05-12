using Ersa.Platform.Common.Data.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MeldungHinzufuegen
	{
		Task SUB_CreateMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3, IEnumerable<ENUM_MeldungAktionen> possibleactions, IEnumerable<ENUM_ProzessAktionen> processactionsns, string details, string context, bool i_blnDuplicateAllowed, ENUM_MeldungAktionen i_enuAktion);

		Task SUB_QuittiereMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3);
	}
}
