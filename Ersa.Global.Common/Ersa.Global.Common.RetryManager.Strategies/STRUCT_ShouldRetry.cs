using System;

namespace Ersa.Global.Common.RetryManager.Strategies
{
	public struct STRUCT_ShouldRetry
	{
		public static readonly STRUCT_ShouldRetry ms_sttNoRetry = new STRUCT_ShouldRetry
		{
			PRO_blnShouldRetry = false,
			PRO_sttDelay = TimeSpan.Zero
		};

		public bool PRO_blnShouldRetry
		{
			get;
			set;
		}

		public TimeSpan PRO_sttDelay
		{
			get;
			set;
		}
	}
}
