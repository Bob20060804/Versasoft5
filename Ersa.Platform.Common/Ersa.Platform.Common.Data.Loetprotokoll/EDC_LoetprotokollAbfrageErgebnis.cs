using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	public class EDC_LoetprotokollAbfrageErgebnis
	{
		public long PRO_i64ProtokollId
		{
			get;
			set;
		}

		public long PRO_i64LoetprogrammVersionsId
		{
			get;
			set;
		}

		public bool PRO_blnArbeitsversion
		{
			get;
			set;
		}

		public DateTime PRO_dtmEingangszeit
		{
			get;
			set;
		}

		public DateTime PRO_dtmAusgangszeit
		{
			get;
			set;
		}

		public string PRO_strBenutzerName
		{
			get;
			set;
		}

		public bool PRO_blnLoetgutSchlecht
		{
			get;
			set;
		}

		public long PRO_i64LaufendeNummer
		{
			get;
			set;
		}

		public ENUM_LoetprogrammModus PRO_enmModus
		{
			get;
			set;
		}

		public Dictionary<string, string> PRO_strProtokollParameter
		{
			get;
			set;
		}

		public Dictionary<string, List<string>> PRO_strProtokollCodes
		{
			get;
			set;
		}

		public IEnumerable<EDC_LoetprotokollSollIstVariable> PRO_enuVariablenListe
		{
			get;
			set;
		}

		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public string PRO_strLoetprogrammName
		{
			get;
			set;
		}
	}
}
