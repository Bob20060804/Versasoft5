using Ersa.Platform.CapabilityContracts.KommunikationsDienst;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
    /// <summary>
    /// ×¢²áÊÂ¼þ ²ßÂÔ
    /// </summary>
	[Export]
	public class EDC_EventHandlerRegistrierungsStrategie : INF_ParameterBehandlungsStrategie<string>
	{
		private readonly Lazy<INF_AdressenZusammenSetzenCapability> m_edcAdressenZusammensetzerCapability;

		[ImportingConstructor]
		public EDC_EventHandlerRegistrierungsStrategie(INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcAdressenZusammensetzerCapability = new Lazy<INF_AdressenZusammenSetzenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_AdressenZusammenSetzenCapability>);
		}

		public string FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter)
		{
			return m_edcAdressenZusammensetzerCapability.Value.FUN_enuBehandleSollZeit(i_edcParameter).ToArray()[5];
		}

		public string FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter)
		{
			throw new InvalidOperationException("Die Sollzeit wirft keine Events!");
		}

		public string FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter)
		{
			return m_edcAdressenZusammensetzerCapability.Value.FUN_strErstellePhysischeAdresse(i_edcParameter);
		}
	}
}
