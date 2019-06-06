using Ersa.Platform.Common.Data.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
	public interface INF_MeldungHinzufuegen
	{
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="i_i32Meldungsnummer"></param>
        /// <param name="i_strLokalisierungsKeyMesTyp"></param>
        /// <param name="i_i32MeldungOrt3"></param>
        /// <param name="possibleactions"></param>
        /// <param name="processactionsns"></param>
        /// <param name="details"></param>
        /// <param name="context"></param>
        /// <param name="i_blnDuplicateAllowed"></param>
        /// <param name="i_enuAktion"></param>
        /// <returns></returns>
		Task SUB_CreateMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3, IEnumerable<ENUM_MeldungAktionen> possibleactions, IEnumerable<ENUM_ProzessAktionen> processactionsns, string details, string context, bool i_blnDuplicateAllowed, ENUM_MeldungAktionen i_enuAktion);

        /// <summary>
        /// ��֪��Ϣ
        /// </summary>
        /// <param name="i_i32Meldungsnummer"></param>
        /// <param name="i_strLokalisierungsKeyMesTyp"></param>
        /// <param name="i_i32MeldungOrt3"></param>
        /// <returns></returns>
		Task SUB_QuittiereMessageAsync(int i_i32Meldungsnummer, string i_strLokalisierungsKeyMesTyp, int i_i32MeldungOrt3);
	}
}
