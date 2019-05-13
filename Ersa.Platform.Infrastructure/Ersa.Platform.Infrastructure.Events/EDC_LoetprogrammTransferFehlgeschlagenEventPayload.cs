using System;

namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_LoetprogrammTransferFehlgeschlagenEventPayload
	{
		public Exception PRO_fdcException
		{
			get;
			set;
		}
	}
}
