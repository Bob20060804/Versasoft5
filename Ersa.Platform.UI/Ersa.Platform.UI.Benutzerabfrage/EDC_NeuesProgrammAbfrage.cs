using Ersa.Platform.UI.Benutzereingabe;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_NeuesProgrammAbfrage : EDC_BenutzerAbfrage<EDC_NeuesProgrammEingabe>
	{
		public IList<string> PRO_lstBibliotheken
		{
			get;
			set;
		}

		public string PRO_strBibliothekName
		{
			get;
			set;
		}

		public string PRO_strProgrammName
		{
			get;
			set;
		}

		public Func<string, string, string> PRO_delValidierung
		{
			get;
			set;
		}

		public EDC_NeuesProgrammAbfrage()
		{
			base.PRO_edcErgebnis = new EDC_NeuesProgrammEingabe();
		}
	}
}
