using Ersa.Platform.Common.Mes;
using System;

namespace Ersa.Platform.Mes.Modell
{
	[Serializable]
	public class EDC_MesMeldungContext
	{
		public ENUM_MesFunktionen PRO_enmMesFunktion
		{
			get;
			set;
		}

		public EDC_MesMaschinenDaten PRO_edcMesMaschinenDaten
		{
			get;
			set;
		}

		public EDC_MesMeldungContext(ENUM_MesFunktionen i_enmMesFunktion, EDC_MesMaschinenDaten i_edcMesMaschinenDaten)
		{
			PRO_enmMesFunktion = i_enmMesFunktion;
			PRO_edcMesMaschinenDaten = i_edcMesMaschinenDaten;
		}
	}
}
