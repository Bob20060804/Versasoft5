using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_OkAbbrechenHinweis : EDC_BenutzerAbfrage<ENUM_BenutzerEingabe>
	{
		public string PRO_strOkText
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}
	}
}
