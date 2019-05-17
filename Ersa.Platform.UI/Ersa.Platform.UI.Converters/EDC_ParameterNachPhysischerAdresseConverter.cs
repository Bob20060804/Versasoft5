using Ersa.Platform.CapabilityContracts.KommunikationsDienst;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Infrastructure.Prism;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Platform.UI.Converters
{
	public class EDC_ParameterNachPhysischerAdresseConverter : IValueConverter
	{
		private static readonly object ms_objSyncObject = new object();

		private static INF_AdressenZusammenSetzenCapability ms_edcAdressZusammensetzerCapability;

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			EDC_PrimitivParameter eDC_PrimitivParameter = i_objValue as EDC_PrimitivParameter;
			if (eDC_PrimitivParameter == null)
			{
				return string.Empty;
			}
			lock (ms_objSyncObject)
			{
				if (ms_edcAdressZusammensetzerCapability == null)
				{
					ms_edcAdressZusammensetzerCapability = EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<INF_CapabilityProvider>().FUN_edcCapabilityHolen<INF_AdressenZusammenSetzenCapability>();
				}
			}
			return $"{i_objParameter}{Environment.NewLine}{ms_edcAdressZusammensetzerCapability.FUN_strErstellePhysischeAdresse(eDC_PrimitivParameter)}";
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
