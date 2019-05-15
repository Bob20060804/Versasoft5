using Ersa.Platform.UI.Benutzereingabe;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_LoginAbfrage : EDC_BenutzerAbfrage<EDC_LoginEingabe>
	{
		public string PRO_strFehler
		{
			get;
			set;
		}

		public string PRO_strFehlendesRechtText
		{
			get;
			set;
		}

		public string PRO_strAnmeldenText
		{
			get;
			set;
		}

		public string PRO_strBenutzerNameText
		{
			get;
			set;
		}

		public string PRO_strPasswortText
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public bool PRO_blnBenutzerNameFixiert
		{
			get;
			set;
		}

		public EDC_LoginAbfrage()
		{
			base.PRO_edcErgebnis = new EDC_LoginEingabe();
		}
	}
}
