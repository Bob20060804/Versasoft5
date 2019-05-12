using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Dienste.Interfaces
{
	public class EDC_CsvDatensatz
	{
		public IEnumerable<string> PRO_enuDaten
		{
			get;
			set;
		}

		public EDC_CsvDatensatz()
		{
			PRO_enuDaten = Enumerable.Empty<string>();
		}
	}
}
