using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_LpInterpreterNichtGefundenHinweis : EDC_BenutzerAbfrage<ENUM_LpInterpreterNichtGefundenEingabe>
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
	}
}
