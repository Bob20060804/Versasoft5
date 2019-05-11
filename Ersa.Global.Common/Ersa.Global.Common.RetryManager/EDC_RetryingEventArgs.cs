using System;

namespace Ersa.Global.Common.RetryManager
{
	public class EDC_RetryingEventArgs : EventArgs
	{
		public int PRO_i32CurrentRetryCount
		{
			get;
			private set;
		}

		public TimeSpan PRO_sttDelay
		{
			get;
			private set;
		}

		public Exception PRO_fdcLastException
		{
			get;
			private set;
		}

		public EDC_RetryingEventArgs(int i_i32CurrentRetryCount, TimeSpan i_sttDelay, Exception i_fdcLastException)
		{
			PRO_i32CurrentRetryCount = i_i32CurrentRetryCount;
			PRO_sttDelay = i_sttDelay;
			PRO_fdcLastException = i_fdcLastException;
		}
	}
}
