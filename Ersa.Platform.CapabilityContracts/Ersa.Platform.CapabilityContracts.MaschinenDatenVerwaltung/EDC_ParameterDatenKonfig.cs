using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung
{
	public class EDC_ParameterDatenKonfig
	{
		public string PRO_strVersion
		{
			get;
			set;
		}

		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		public string PRO_strDatum
		{
			get;
			set;
		}

		public IEnumerable<EDC_ParameterDaten> PRO_enuParameter
		{
			get;
			set;
		}
	}
}
