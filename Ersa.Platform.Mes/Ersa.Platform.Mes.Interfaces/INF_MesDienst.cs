using Ersa.Platform.Common.Mes;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
    /// <summary>
    /// MES Service 接口
    /// </summary>
	public interface INF_MesDienst : INF_MeldungProzessor
	{
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
		Task<EDC_MesInitialisierungsRueckgabe> FUN_fdcInitialisiereAsync();

        /// <summary>
        /// Deinitialize
        /// </summary>
        /// <returns></returns>
		Task FUN_fdcDeinitialisiereAsync();

        /// <summary>
        /// MES系统是否连接
        /// Mes System is connected
        /// </summary>
        /// <returns></returns>
		Task<bool> FUN_fdcIstMesSystemVerbundenAsync();

        /// <summary>
        /// 方法是否激活
        /// Function is active
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <returns></returns>
		Task<bool> FUN_fdcIstFunktionAktivAsync(ENUM_MesFunktionen i_enuFunktion);

        /// <summary>
        /// 方法和MES是否激活
        /// IstFunktionUndMesAktiv
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <returns></returns>
		Task<bool> FUN_fdcIstFunktionUndMesAktivAsync(ENUM_MesFunktionen i_enuFunktion);

        /// <summary>
        /// 单独发送焊接协议
        /// Soldering protocol Send individually
        /// </summary>
        /// <returns></returns>
		Task<bool> FUN_fdcLoetprotokollEinzelnSendenAsync();

        /// <summary>
        /// 调用方法
        /// Call function
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <param name="i_edcMaschinenDaten"></param>
        /// <returns></returns>
		Task<EDC_MesKommunikationsRueckgabe> FUN_fdcFunktionAufrufenAsync(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten);
	}
}
