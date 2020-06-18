using Ersa.Global.Common.Helper;
using Ersa.Global.Common.RetryManager.Strategies;
using System;
using System.Threading.Tasks;

namespace Ersa.Global.Common.RetryManager
{
	public class EDC_RetryPolicy
	{
		private static readonly EDC_RetryPolicy ms_edcNoRetry = new EDC_RetryPolicy(new EDC_TransientErrorIgnoreStrategy(), EDC_RetryStrategy.PRO_edcNoRetry);

		private static readonly EDC_RetryPolicy ms_edcDefaultFixed = new EDC_RetryPolicy(new EDC_TransientErrorCatchAllStrategy(), EDC_RetryStrategy.PRO_edcDefaultFixed);

		private static readonly EDC_RetryPolicy ms_edcDefaultProgressive = new EDC_RetryPolicy(new EDC_TransientErrorCatchAllStrategy(), EDC_RetryStrategy.PRO_edcDefaultProgressive);

		private static readonly EDC_RetryPolicy ms_edcDefaultExponential = new EDC_RetryPolicy(new EDC_TransientErrorCatchAllStrategy(), EDC_RetryStrategy.PRO_edcDefaultExponential);

		public static EDC_RetryPolicy PRO_edcNoRetryPolicy => ms_edcNoRetry;

		public static EDC_RetryPolicy PRO_edcDefaultFixedPolicy => ms_edcDefaultFixed;

		public static EDC_RetryPolicy PRO_edcDefaultProgressivePolicy => ms_edcDefaultProgressive;

		public static EDC_RetryPolicy PRO_edcDefaultExponentialPolicy => ms_edcDefaultExponential;

		public EDC_RetryStrategy PRO_edcRetryStrategy
		{
			get;
			private set;
		}

		public INF_TransientErrorDetectionStrategy PRO_edcErrorDetectionStrategy
		{
			get;
			private set;
		}

		public event EventHandler<EDC_RetryingEventArgs> m_evtRetrying;

		public EDC_RetryPolicy(INF_TransientErrorDetectionStrategy i_edcErrorDetectionStrategy, EDC_RetryStrategy i_edcRetryStrategy)
		{
			PRO_edcErrorDetectionStrategy = i_edcErrorDetectionStrategy;
			PRO_edcRetryStrategy = i_edcRetryStrategy;
		}

		public EDC_RetryPolicy(INF_TransientErrorDetectionStrategy i_edcErrorDetectionStrategy, int i_i32RetryCount)
			: this(i_edcErrorDetectionStrategy, new EDC_FixedIntervalStrategy(i_i32RetryCount))
		{
		}

		public EDC_RetryPolicy(INF_TransientErrorDetectionStrategy i_edcErrorDetectionStrategy, int i_i32RetryCount, TimeSpan i_sttRetryInterval)
			: this(i_edcErrorDetectionStrategy, new EDC_FixedIntervalStrategy(i_i32RetryCount, i_sttRetryInterval))
		{
		}

		public EDC_RetryPolicy(INF_TransientErrorDetectionStrategy i_edcErrorDetectionStrategy, int i_i32RetryCount, TimeSpan i_sttMinBackoff, TimeSpan i_sttMaxBackoff, TimeSpan i_sttDeltaBackoff)
			: this(i_edcErrorDetectionStrategy, new EDC_ExponentialBackoffStrategy(i_i32RetryCount, i_sttMinBackoff, i_sttMaxBackoff, i_sttDeltaBackoff))
		{
		}

		public EDC_RetryPolicy(INF_TransientErrorDetectionStrategy i_edcErrorDetectionStrategy, int i_i32RetryCount, TimeSpan i_sttInitialInterval, TimeSpan i_sttIncrement)
			: this(i_edcErrorDetectionStrategy, new EDC_IncrementalStrategy(i_i32RetryCount, i_sttInitialInterval, i_sttIncrement))
		{
		}

		public void SUB_ExecuteAction(Action i_delAction)
		{
			FUN_objExecuteAction((Func<object>)delegate
			{
				i_delAction();
				return null;
			});
		}

		public TResult FUN_objExecuteAction<TResult>(Func<TResult> i_delFunc)
		{
			int num = 0;
			while (true)
			{
				TimeSpan sttDelay;
				try
				{
					return i_delFunc();
				}
				catch (Exception ex)
				{
					STRUCT_ShouldRetry sTRUCT_ShouldRetry = PRO_edcRetryStrategy.FUN_sttShouldRetryErmitteln(num++, ex);
					if (!PRO_edcErrorDetectionStrategy.FUN_blnIsTransient(ex) || !sTRUCT_ShouldRetry.PRO_blnShouldRetry)
					{
						throw;
					}
					sttDelay = sTRUCT_ShouldRetry.PRO_sttDelay;
				}
				if (sttDelay.TotalMilliseconds < 0.0)
				{
					sttDelay = TimeSpan.Zero;
				}
				// SUB_OnRetrying(num, ex, sttDelay);
				if (num > 1 || !PRO_edcRetryStrategy.PRO_blnSchnellerErsterRetry)
				{
					EDC_TaskHelfer.SUB_RunSync(() => Task.Delay(sttDelay));
				}
			}
		}

		private void SUB_OnRetrying(int i_i32RetryCount, Exception i_fdcLastError, TimeSpan i_sttDelay)
		{
			if (this.m_evtRetrying != null)
			{
				this.m_evtRetrying(this, new EDC_RetryingEventArgs(i_i32RetryCount, i_sttDelay, i_fdcLastError));
			}
		}
	}
}
