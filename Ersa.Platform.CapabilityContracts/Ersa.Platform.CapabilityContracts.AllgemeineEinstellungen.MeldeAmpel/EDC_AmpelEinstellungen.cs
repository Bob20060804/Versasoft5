using Ersa.Platform.Common.Model;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.MeldeAmpel
{
	public class EDC_AmpelEinstellungen
	{
		public EDC_BooleanParameter PRO_edcAktiv
		{
			get;
			set;
		}

		public EDC_IntegerParameter PRO_edcMaxZeit
		{
			get;
			set;
		}

		public bool PRO_blnHatAenderung
		{
			get
			{
				if (!PRO_edcAktiv.PRO_blnHatAenderung)
				{
					return PRO_edcMaxZeit.PRO_blnHatAenderung;
				}
				return true;
			}
		}
	}
}
