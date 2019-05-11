using System;

namespace Ersa.Global.Common.RetryManager.Strategies
{
	public abstract class EDC_RetryStrategy
	{
		public const int mC_i32DefaultClientRetryCount = 5;

		public const bool mC_blnDefaultFirstFastRetry = true;

		public static readonly TimeSpan ms_sttDefaultClientBackoff = TimeSpan.FromSeconds(10.0);

		public static readonly TimeSpan ms_sttDefaultMaxBackoff = TimeSpan.FromSeconds(30.0);

		public static readonly TimeSpan ms_sttDefaultMinBackoff = TimeSpan.FromSeconds(1.0);

		public static readonly TimeSpan ms_sttDefaultRetryInterval = TimeSpan.FromSeconds(1.0);

		public static readonly TimeSpan ms_sttDefaultRetryIncrement = TimeSpan.FromSeconds(1.0);

		private static readonly EDC_RetryStrategy ms_edcNoRetry = new EDC_FixedIntervalStrategy(0, ms_sttDefaultRetryInterval);

		private static readonly EDC_RetryStrategy ms_edcDefaultFixed = new EDC_FixedIntervalStrategy(5, ms_sttDefaultRetryInterval);

		private static readonly EDC_RetryStrategy ms_edcDefaultProgressive = new EDC_IncrementalStrategy(5, ms_sttDefaultRetryInterval, ms_sttDefaultRetryIncrement);

		private static readonly EDC_RetryStrategy ms_edcDefaultExponential = new EDC_ExponentialBackoffStrategy(5, ms_sttDefaultMinBackoff, ms_sttDefaultMaxBackoff, ms_sttDefaultClientBackoff);

		public static EDC_RetryStrategy PRO_edcNoRetry => ms_edcNoRetry;

		public static EDC_RetryStrategy PRO_edcDefaultFixed => ms_edcDefaultFixed;

		public static EDC_RetryStrategy PRO_edcDefaultProgressive => ms_edcDefaultProgressive;

		public static EDC_RetryStrategy PRO_edcDefaultExponential => ms_edcDefaultExponential;

		public bool PRO_blnSchnellerErsterRetry
		{
			get;
			protected set;
		}

		protected EDC_RetryStrategy(bool i_blnSchnellerErsterRetry)
		{
			PRO_blnSchnellerErsterRetry = i_blnSchnellerErsterRetry;
		}

		public abstract STRUCT_ShouldRetry FUN_sttShouldRetryErmitteln(int i_i32CurrentRetryCount, Exception i_fdcLastException);
	}
}
