using Ersa.Platform.Common.LeseSchreibGeraete.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	public class EDC_CodeLeseErgebnis : EDC_LeseErgebnis
	{
		public Dictionary<string, List<EDC_CodeMitVerwendungUndBedeutung>> PRO_dicCodes
		{
			get;
			set;
		}

		public Dictionary<string, ENUM_LeseFehler> PRO_dicCodeLeseFehler
		{
			get;
			private set;
		}

		public IEnumerable<long> PRO_enuProgrammIds
		{
			get;
			set;
		}

		public bool PRO_blnEinlaufsperreAnfordern
		{
			get;
			set;
		}

		public bool PRO_blnNestCodeErkennungAktiv
		{
			get;
			set;
		}

		public bool PRO_blnErgebnisWurdeVerarbeitet
		{
			get;
			set;
		}

		public DateTime? PRO_fdcTimeStampVerarbeitet
		{
			get;
			set;
		}

		public IEnumerable<EDC_NestDaten> PRO_enuNestDaten
		{
			get;
			set;
		}

		public Dictionary<string, Dictionary<string, List<string>>> PRO_dicCodesAlsListe => PRO_dicCodes.ToDictionary((KeyValuePair<string, List<EDC_CodeMitVerwendungUndBedeutung>> i_fdcPaar) => i_fdcPaar.Key, (KeyValuePair<string, List<EDC_CodeMitVerwendungUndBedeutung>> i_fdcPaar) => i_fdcPaar.Value.FUN_dicCodesAlsDictionary());

		public EDC_CodeLeseErgebnis(long i_i64ArrayIndex)
			: base(i_i64ArrayIndex)
		{
			PRO_dicCodes = new Dictionary<string, List<EDC_CodeMitVerwendungUndBedeutung>>();
			PRO_dicCodeLeseFehler = new Dictionary<string, ENUM_LeseFehler>();
			PRO_enuProgrammIds = new List<long>();
		}
	}
}
