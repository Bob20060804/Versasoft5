using Ersa.Platform.UI.Benutzereingabe;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_OptionAbfrage : EDC_BenutzerAbfrage<object>
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

		public IList<EDC_OptionEingabe> PRO_lstOptionen
		{
			get;
			set;
		}

		public bool PRO_blnAbbrechenMoeglich
		{
			get;
			set;
		}
	}
}
