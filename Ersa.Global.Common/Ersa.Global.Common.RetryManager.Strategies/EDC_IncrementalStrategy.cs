using System;

namespace Ersa.Global.Common.RetryManager.Strategies
{
	public class EDC_IncrementalStrategy : EDC_RetryStrategy
	{
		private readonly int m_i32RetryCount;

		private readonly TimeSpan m_sttInitialInterval;

		private readonly TimeSpan m_sttIncrement;

		public EDC_IncrementalStrategy()
			: this(5, EDC_RetryStrategy.ms_sttDefaultRetryInterval, EDC_RetryStrategy.ms_sttDefaultRetryIncrement)
		{
		}

		public EDC_IncrementalStrategy(int i_i32RetryCount, TimeSpan i_sttInitialInterval, TimeSpan i_sttIncrement)
			: this(i_i32RetryCount, i_sttInitialInterval, i_sttIncrement, i_blnFirstFastRetry: true)
		{
		}

		public EDC_IncrementalStrategy(int i_i32RetryCount, TimeSpan i_sttInitialInterval, TimeSpan i_sttIncrement, bool i_blnFirstFastRetry)
			: base(i_blnFirstFastRetry)
		{
			m_i32RetryCount = i_i32RetryCount;
			m_sttInitialInterval = i_sttInitialInterval;
			m_sttIncrement = i_sttIncrement;
		}

		public override STRUCT_ShouldRetry FUN_sttShouldRetryErmitteln(int i_i32CurrentRetryCount, Exception i_fdcLastException)
		{
			if (i_i32CurrentRetryCount < m_i32RetryCount)
			{
				STRUCT_ShouldRetry result = default(STRUCT_ShouldRetry);
				result.PRO_blnShouldRetry = true;
				result.PRO_sttDelay = TimeSpan.FromMilliseconds(m_sttInitialInterval.TotalMilliseconds + m_sttIncrement.TotalMilliseconds * (double)i_i32CurrentRetryCount);
				return result;
			}
			return STRUCT_ShouldRetry.ms_sttNoRetry;
		}
	}
}
