using System;

namespace Ersa.Global.Common.RetryManager
{
    /// <summary>
    /// ¶ÌÔİµÄºöÂÔ´íÎó
    /// </summary>
	public sealed class EDC_TransientErrorIgnoreStrategy : INF_TransientErrorDetectionStrategy
	{
        /// <summary>
        /// ÊÇ·ñÄÜ¶ÌÔİµÄºöÂÔ´íÎó
        /// </summary>
        /// <param name="i_fdcEx"></param>
        /// <returns></returns>
		public bool FUN_blnIsTransient(Exception i_fdcEx)
		{
			return false;
		}
	}
}
