using Ersa.Platform.Common.Model;

namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public class EDC_LoetgutZaehlerElement
	{
		public EDC_IntegerParameter PRO_edcZaehlerParameter
		{
			get;
		}

		public bool PRO_blnIstEditierbar
		{
			get;
		}

		public EDC_LoetgutZaehlerElement(EDC_IntegerParameter i_blnZaehlerParameter, bool i_blnIstEditierbar)
		{
			PRO_edcZaehlerParameter = i_blnZaehlerParameter;
			PRO_blnIstEditierbar = i_blnIstEditierbar;
		}
	}
}
