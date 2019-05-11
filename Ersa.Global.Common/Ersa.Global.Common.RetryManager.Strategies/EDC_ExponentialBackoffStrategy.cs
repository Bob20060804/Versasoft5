using System;

namespace Ersa.Global.Common.RetryManager.Strategies
{
	public class EDC_ExponentialBackoffStrategy : EDC_RetryStrategy
	{
		private readonly int m_i32RetryCount;

		private readonly TimeSpan m_sttMinBackoff;

		private readonly TimeSpan m_sttMaxBackoff;

		private readonly TimeSpan m_sttDeltaBackoff;

		public EDC_ExponentialBackoffStrategy()
			: this(5, EDC_RetryStrategy.ms_sttDefaultMinBackoff, EDC_RetryStrategy.ms_sttDefaultMaxBackoff, EDC_RetryStrategy.ms_sttDefaultClientBackoff)
		{
		}

		public EDC_ExponentialBackoffStrategy(int i_i32RetryCount, TimeSpan i_sttMinBackoff, TimeSpan i_sttMaxBackoff, TimeSpan i_sttDeltaBackoff)
			: this(i_i32RetryCount, i_sttMinBackoff, i_sttMaxBackoff, i_sttDeltaBackoff, i_blnFirstFastRetry: true)
		{
		}

		public EDC_ExponentialBackoffStrategy(int i_i32RetryCount, TimeSpan i_sttMinBackoff, TimeSpan i_sttMaxBackoff, TimeSpan i_sttDeltaBackoff, bool i_blnFirstFastRetry)
			: base(i_blnFirstFastRetry)
		{
			m_i32RetryCount = i_i32RetryCount;
			m_sttMinBackoff = i_sttMinBackoff;
			m_sttMaxBackoff = i_sttMaxBackoff;
			m_sttDeltaBackoff = i_sttDeltaBackoff;
		}

		public override STRUCT_ShouldRetry FUN_sttShouldRetryErmitteln(int i_i32CurrentRetryCount, Exception i_fdcLastException)
		{
			if (i_i32CurrentRetryCount < m_i32RetryCount)
			{
				int num = (int)((Math.Pow(2.0, i_i32CurrentRetryCount) - 1.0) * m_sttDeltaBackoff.TotalMilliseconds);
				int num2 = (int)Math.Min(m_sttMinBackoff.TotalMilliseconds + (double)num, m_sttMaxBackoff.TotalMilliseconds);
				STRUCT_ShouldRetry result = default(STRUCT_ShouldRetry);
				result.PRO_blnShouldRetry = true;
				result.PRO_sttDelay = TimeSpan.FromMilliseconds(num2);
				return result;
			}
			return STRUCT_ShouldRetry.ms_sttNoRetry;
		}
	}
}
