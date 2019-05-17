using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_LpDateiNichtGefundenHinweis : EDC_BenutzerAbfrage<ENUM_LpDateiNichtGefundenEingabe>
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
	}
}
