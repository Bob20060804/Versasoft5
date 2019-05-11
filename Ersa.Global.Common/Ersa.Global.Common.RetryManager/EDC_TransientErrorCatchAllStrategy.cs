using System;

namespace Ersa.Global.Common.RetryManager
{
	public sealed class EDC_TransientErrorCatchAllStrategy : INF_TransientErrorDetectionStrategy
	{
		public bool FUN_blnIsTransient(Exception i_fdcEx)
		{
			return true;
		}
	}
}
