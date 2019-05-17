using System;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_AuswahlAbfrage : EDC_BenutzerAbfrage<string>
	{
		public string PRO_strBestaetigenText
		{
			get;
			set;
		}

		public Func<string, string> PRO_delBestaetigenText
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public IList<string> PRO_lstAuswahlListe
		{
			get;
			set;
		}

		public Func<string, string> PRO_delValidierung
		{
			get;
			set;
		}

		public string PRO_strAuswahl
		{
			get;
			set;
		}

		public bool PRO_blnBestaetigenVerhindernOhneAenderung
		{
			get;
			set;
		}
	}
}
