using Ersa.Platform.CapabilityContracts.KommunikationsDienst;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
	[Export]
	public class EDC_AdressRegistrierungsStrategie : INF_ParameterBehandlungsStrategie<IEnumerable<string>>
	{
		private readonly Lazy<INF_AdressenZusammenSetzenCapability> m_edcAdressenZusammensetzerCapability;

		[ImportingConstructor]
		public EDC_AdressRegistrierungsStrategie(INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcAdressenZusammensetzerCapability = new Lazy<INF_AdressenZusammenSetzenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_AdressenZusammenSetzenCapability>);
		}

		public IEnumerable<string> FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter)
		{
			return m_edcAdressenZusammensetzerCapability.Value.FUN_enuBehandleIstZeit(i_edcParameter);
		}

		public IEnumerable<string> FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter)
		{
			return m_edcAdressenZusammensetzerCapability.Value.FUN_enuBehandleSollZeit(i_edcParameter);
		}

		public IEnumerable<string> FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter)
		{
			return new List<string>
			{
				m_edcAdressenZusammensetzerCapability.Value.FUN_strErstellePhysischeAdresse(i_edcParameter)
			};
		}
	}
}
