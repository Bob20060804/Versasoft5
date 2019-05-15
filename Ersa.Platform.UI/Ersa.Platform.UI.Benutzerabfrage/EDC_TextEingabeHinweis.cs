using System;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_TextEingabeHinweis : EDC_BenutzerAbfrage<string>
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

		public bool PRO_blnReturnErlaubt
		{
			get;
			set;
		}

		public Func<string, string> PRO_delValidierung
		{
			get;
			set;
		}
	}
}
