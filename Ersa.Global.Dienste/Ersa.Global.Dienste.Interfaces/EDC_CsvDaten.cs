using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Dienste.Interfaces
{
	public class EDC_CsvDaten
	{
		public string PRO_strTrennzeichen
		{
			get;
			set;
		}

		public IEnumerable<string> PRO_enuDateikopf
		{
			get;
			set;
		}

		public IEnumerable<string> PRO_enuHeaderSpalten
		{
			get;
			set;
		}

		public IEnumerable<EDC_CsvDatensatz> PRO_enuDaten
		{
			get;
			set;
		}

		public EDC_CsvDaten()
		{
			PRO_strTrennzeichen = ";";
			PRO_enuDateikopf = Enumerable.Empty<string>();
			PRO_enuHeaderSpalten = Enumerable.Empty<string>();
			PRO_enuDaten = Enumerable.Empty<EDC_CsvDatensatz>();
		}
	}
}
