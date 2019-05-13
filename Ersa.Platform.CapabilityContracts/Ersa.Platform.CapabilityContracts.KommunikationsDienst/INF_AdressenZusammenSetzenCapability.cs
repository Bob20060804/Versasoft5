using Ersa.Platform.Common.Model;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.KommunikationsDienst
{
	public interface INF_AdressenZusammenSetzenCapability
	{
		string FUN_strErstellePhysischeAdresse(EDC_PrimitivParameter i_edcPrimitivParameter);

		IEnumerable<string> FUN_enuBehandleSollZeit(EDC_PrimitivParameter i_edcParameter);

		IEnumerable<string> FUN_enuBehandleIstZeit(EDC_PrimitivParameter i_edcParameter);
	}
}
