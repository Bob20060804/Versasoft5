using System;

namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_UnbehandelteExceptionAufgetretenEventPayload
	{
		public Exception PRO_fdcException
		{
			get;
			set;
		}
	}
}
