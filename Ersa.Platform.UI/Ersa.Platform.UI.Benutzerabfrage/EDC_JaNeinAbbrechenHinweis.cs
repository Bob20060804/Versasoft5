using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_JaNeinAbbrechenHinweis : EDC_BenutzerAbfrage<ENUM_BenutzerEingabe>
	{
		public string PRO_strJaText
		{
			get;
			set;
		}

		public string PRO_strNeinText
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
