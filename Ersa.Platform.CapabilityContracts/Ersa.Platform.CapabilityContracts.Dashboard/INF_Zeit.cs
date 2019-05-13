using System;

namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public interface INF_Zeit : INF_BetriebsDatenWert
	{
		TimeSpan PRO_fdcZeitSpanne
		{
			get;
		}

		int PRO_i32Anteil
		{
			get;
		}
	}
}
