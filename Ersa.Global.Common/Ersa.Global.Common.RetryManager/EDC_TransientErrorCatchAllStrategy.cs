using System;

namespace Ersa.Global.Common.RetryManager
{
    /// <summary>
    /// 短暂的破获异常类
    /// </summary>
	public sealed class EDC_TransientErrorCatchAllStrategy : INF_TransientErrorDetectionStrategy
	{
        /// <summary>
        /// 短暂的
        /// </summary>
        /// <param name="i_fdcEx"></param>
        /// <returns></returns>
		public bool FUN_blnIsTransient(Exception i_fdcEx)
		{
			return true;
		}
	}
}
