using System;

namespace Ersa.Global.Common.RetryManager
{
    /// <summary>
    /// ���ݵĺ��Դ���
    /// </summary>
	public sealed class EDC_TransientErrorIgnoreStrategy : INF_TransientErrorDetectionStrategy
	{
        /// <summary>
        /// �Ƿ��ܶ��ݵĺ��Դ���
        /// </summary>
        /// <param name="i_fdcEx"></param>
        /// <returns></returns>
		public bool FUN_blnIsTransient(Exception i_fdcEx)
		{
			return false;
		}
	}
}
