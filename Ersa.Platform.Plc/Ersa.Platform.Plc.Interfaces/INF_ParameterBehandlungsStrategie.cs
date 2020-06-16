using Ersa.Platform.Common.Model;

namespace Ersa.Platform.Plc.Interfaces
{
    /// <summary>
    /// ������Ϊ
    /// Treatment strategy parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface INF_ParameterBehandlungsStrategie<out T>
	{
        /// <summary>
        /// ����ǰʱ��
        /// Be handle current time
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// ����Ŀ��ʱ��
        /// Be handle target time
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// ����Ĭ��
        /// Be default
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter);
	}
}
