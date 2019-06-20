using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Modell;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
    /// <summary>
    /// Mes function
    /// </summary>
	public interface INF_MesFunktion
	{
        /// <summary>
        /// 所有的mes方法
        /// </summary>
		ENUM_MesFunktionen PRO_enuMesFunktion
		{
			get;
		}

        /// <summary>
        /// Return Mes communication 
        /// </summary>
        /// <param name="i_edcEinstellung"></param>
        /// <param name="i_edcMaschinenDaten"></param>
        /// <returns></returns>
		Task<EDC_MesKommunikationsRueckgabe> FUN_fdcAusfuehrenAsync(EDC_MesTypEinstellung i_edcEinstellung, EDC_MesMaschinenDaten i_edcMaschinenDaten);
	}
}
