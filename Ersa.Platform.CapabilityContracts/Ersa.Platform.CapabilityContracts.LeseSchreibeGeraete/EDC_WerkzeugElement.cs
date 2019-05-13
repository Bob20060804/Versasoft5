using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;

namespace Ersa.Platform.CapabilityContracts.LeseSchreibeGeraete
{
	public class EDC_WerkzeugElement
	{
		public ENUM_RuestWerkzeug PRO_enmWerkzeug
		{
			get;
		}

		public string PRO_strNameKey
		{
			get;
		}

		public string PRO_strDefaultAntenne
		{
			get;
		}

		public EDC_WerkzeugElement(ENUM_RuestWerkzeug i_enmWerkzeug, string i_strNameKey, string i_strDefaultAntenne = null)
		{
			PRO_enmWerkzeug = i_enmWerkzeug;
			PRO_strNameKey = i_strNameKey;
			PRO_strDefaultAntenne = i_strDefaultAntenne;
		}
	}
}
