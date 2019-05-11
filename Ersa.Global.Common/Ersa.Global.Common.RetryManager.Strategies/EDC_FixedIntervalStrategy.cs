using System;

namespace Ersa.Global.Common.RetryManager.Strategies
{
	public class EDC_FixedIntervalStrategy : EDC_RetryStrategy
	{
		private readonly int m_i32RetryCount;

		private readonly TimeSpan m_sttRetryInterval;

		public EDC_FixedIntervalStrategy()
			: this(5)
		{
		}

		public EDC_FixedIntervalStrategy(int i_i32RetryCount)
			: this(i_i32RetryCount, EDC_RetryStrategy.ms_sttDefaultRetryInterval)
		{
		}

		public EDC_FixedIntervalStrategy(int i_i32RetryCount, TimeSpan i_sttRetryInterval, bool i_blnFirstFastRetry = true)
			: base(i_blnFirstFastRetry)
		{
			m_i32RetryCount = i_i32RetryCount;
			m_sttRetryInterval = i_sttRetryInterval;
		}

		public override STRUCT_ShouldRetry FUN_sttShouldRetryErmitteln(int i_i32CurrentRetryCount, Exception i_fdcLastException)
		{
			if (m_i32RetryCount == 0 || i_i32CurrentRetryCount >= m_i32RetryCount)
			{
				return STRUCT_ShouldRetry.ms_sttNoRetry;
			}
			STRUCT_ShouldRetry result = default(STRUCT_ShouldRetry);
			result.PRO_blnShouldRetry = true;
			result.PRO_sttDelay = m_sttRetryInterval;
			return result;
		}
	}
}
