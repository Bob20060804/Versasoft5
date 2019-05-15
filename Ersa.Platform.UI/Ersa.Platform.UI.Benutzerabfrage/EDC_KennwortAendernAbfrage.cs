using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_KennwortAendernAbfrage : EDC_BenutzerAbfrage<EDC_KennwortAendernEingabe>
	{
		public bool PRO_blnAltesKennwortNichtEingeben
		{
			get;
			set;
		}

		public string PRO_strAendernText
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public string PRO_strFehlerText
		{
			get;
			set;
		}

		public string PRO_strAltesKennwortText
		{
			get;
			set;
		}

		public string PRO_strNeuesKennwortText
		{
			get;
			set;
		}

		public string PRO_strNeuesKennwortWiederholungText
		{
			get;
			set;
		}
	}
}
