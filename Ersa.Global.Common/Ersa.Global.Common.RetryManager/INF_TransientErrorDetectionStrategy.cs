using System;

namespace Ersa.Global.Common.RetryManager
{
	public interface INF_TransientErrorDetectionStrategy
	{
		bool FUN_blnIsTransient(Exception i_fdcEx);
	}
}
