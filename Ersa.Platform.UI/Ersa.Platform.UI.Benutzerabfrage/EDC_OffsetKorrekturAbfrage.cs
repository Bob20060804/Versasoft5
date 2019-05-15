using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_OffsetKorrekturAbfrage : EDC_BenutzerAbfrage<EDC_OffsetKorrekturEingabe>
	{
		public string PRO_strBestaetigenText
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public string PRO_strOffsetText
		{
			get;
			set;
		}

		public string PRO_strOffsetAbsolutVerwendenText
		{
			get;
			set;
		}

		public EDC_OffsetKorrekturAbfrage()
		{
			base.PRO_edcErgebnis = new EDC_OffsetKorrekturEingabe();
		}
	}
}
