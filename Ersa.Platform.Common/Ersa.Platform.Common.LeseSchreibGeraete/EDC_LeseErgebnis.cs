using System;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	public abstract class EDC_LeseErgebnis
	{
		public long PRO_i64ArrayIndex
		{
			get;
		}

		public DateTime PRO_fdcTimeStampSignalisiert
		{
			get;
			set;
		}

		public Exception PRO_fdcException
		{
			get;
			set;
		}

		public string PRO_strException => PRO_fdcException?.ToString() ?? "unknown error";

		public ENUM_MessageState PRO_enmPipelineState
		{
			get;
			set;
		}

		protected EDC_LeseErgebnis(long i_i64ArrayIndex)
		{
			PRO_i64ArrayIndex = i_i64ArrayIndex;
		}
	}
}
