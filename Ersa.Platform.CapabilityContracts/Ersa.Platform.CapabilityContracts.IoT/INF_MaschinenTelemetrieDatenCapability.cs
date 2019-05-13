using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.IoT
{
	public interface INF_MaschinenTelemetrieDatenCapability
	{
		Task<Dictionary<string, double>> FUN_fdcTelemtrieDoubleWerteLadenAsync();

		Task<Dictionary<string, string>> FUN_fdcTelemtrieStringWerteLadenAsync();

		Task<Dictionary<int, List<string>>> FUN_fdcTelemtrieMeldungenLadenAsync();
	}
}
