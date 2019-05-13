using System.Collections.Generic;

namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_DuesenSollwerteGeaendertEventPayload
	{
		public Dictionary<long, long> PRO_dicGeaenderteDuesenSollwerte
		{
			get;
			set;
		}
	}
}
