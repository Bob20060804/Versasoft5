using System.Collections.Generic;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	public class EDC_RechteGruppe
	{
		public string PRO_strNameKey
		{
			get;
			set;
		}

		public IList<EDC_RechtBeschreibung> PRO_edcRechte
		{
			get;
			set;
		}

		public EDC_RechteGruppe()
		{
			PRO_edcRechte = new List<EDC_RechtBeschreibung>();
		}
	}
}
