using Ersa.Platform.Common.Model;

namespace Ersa.Platform.Plc.Interfaces
{
    /// <summary>
    /// 参数行为
    /// Treatment strategy parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface INF_ParameterBehandlungsStrategie<out T>
	{
        /// <summary>
        /// 处理当前时间
        /// Be handle current time
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// 处理目标时间
        /// Be handle target time
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter);

        /// <summary>
        /// 处理默认
        /// Be default
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <returns></returns>
		T FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter);
	}
}
