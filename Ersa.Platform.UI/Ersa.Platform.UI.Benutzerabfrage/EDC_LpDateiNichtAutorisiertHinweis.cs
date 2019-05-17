using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_LpDateiNichtAutorisiertHinweis : EDC_BenutzerAbfrage<ENUM_LpDateiNichtAutorisiertEingabe>
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

		public string PRO_strErneutVersuchenText
		{
			get;
			set;
		}
	}
}
