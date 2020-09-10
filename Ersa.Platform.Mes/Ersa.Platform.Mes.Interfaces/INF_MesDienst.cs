using Ersa.Platform.Common.Mes;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Interfaces
{
    /// <summary>
    /// MES Service �ӿ�
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
        /// MESϵͳ�Ƿ�����
        /// Mes System is connected
        /// </summary>
        /// <returns></returns>
		Task<bool> FUN_fdcIstMesSystemVerbundenAsync();

        /// <summary>
        /// �����Ƿ񼤻�
        /// Function is active
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <returns></returns>
		Task<bool> FUN_fdcIstFunktionAktivAsync(ENUM_MesFunktionen i_enuFunktion);

        /// <summary>
        /// ������MES�Ƿ񼤻�
        /// IstFunktionUndMesAktiv
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <returns></returns>
		Task<bool> FUN_fdcIstFunktionUndMesAktivAsync(ENUM_MesFunktionen i_enuFunktion);

        /// <summary>
        /// �������ͺ���Э��
        /// Soldering protocol Send individually
        /// </summary>
        /// <returns></returns>
		Task<bool> FUN_fdcLoetprotokollEinzelnSendenAsync();

        /// <summary>
        /// ���÷���
        /// Call function
        /// </summary>
        /// <param name="i_enuFunktion"></param>
        /// <param name="i_edcMaschinenDaten"></param>
        /// <returns></returns>
		Task<EDC_MesKommunikationsRueckgabe> FUN_fdcFunktionAufrufenAsync(ENUM_MesFunktionen i_enuFunktion, EDC_MesMaschinenDaten i_edcMaschinenDaten);
	}
}
