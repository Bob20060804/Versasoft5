using System;

namespace Ersa.Global.Common.RetryManager
{
    /// <summary>
    /// ���ݵ��ƻ��쳣��
    /// </summary>
	public sealed class EDC_TransientErrorCatchAllStrategy : INF_TransientErrorDetectionStrategy
	{
        /// <summary>
        /// ���ݵ�
        /// </summary>
        /// <param name="i_fdcEx"></param>
        /// <returns></returns>
		public bool FUN_blnIsTransient(Exception i_fdcEx)
		{
			return true;
		}
	}
}
