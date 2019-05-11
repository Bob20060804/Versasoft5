using System.Collections.Generic;

namespace Ersa.Platform.Common.Mes
{
	public class EDC_MesKommunikationsRueckgabe : EDC_MesBasisRueckgabe
	{
		public ENUM_MesKommunikationsStatus PRO_enuKommunikationsStatus
		{
			get;
			set;
		}

		public Dictionary<ENUM_MesRueckgabeArgumente, object> PRO_dicArgumente
		{
			get;
			set;
		}
	}
}
