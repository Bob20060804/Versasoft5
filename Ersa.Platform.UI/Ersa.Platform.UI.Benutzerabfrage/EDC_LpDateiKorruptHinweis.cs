using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_LpDateiKorruptHinweis : EDC_BenutzerAbfrage<ENUM_LpDateiKorruptEingabe>
	{
		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public string PRO_strProgrammLadenText
		{
			get;
			set;
		}

		public string PRO_strAlsInvalidMarkierenText
		{
			get;
			set;
		}

		public string PRO_strLoeschenText
		{
			get;
			set;
		}
	}
}
