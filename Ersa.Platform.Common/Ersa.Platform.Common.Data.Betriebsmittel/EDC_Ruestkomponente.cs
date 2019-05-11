using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Betriebsmittel
{
	public class EDC_Ruestkomponente
	{
		public EDC_RuestkomponentenData PRO_edcRuestkomponenteData
		{
			get;
			set;
		}

		public bool PRO_blnGeloescht
		{
			get;
			set;
		}

		public bool PRO_blnGeaendert
		{
			get;
			set;
		}

		public IEnumerable<EDC_Ruestwerkzeug> PRO_enuRuestwerkzeuge
		{
			get;
			set;
		}
	}
}
