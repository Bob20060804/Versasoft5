using System.Collections.Generic;

namespace Ersa.Platform.Common.Mes
{
    /// <summary>
    /// mes communication return
    /// </summary>
	public class EDC_MesKommunikationsRueckgabe : EDC_MesBasisRueckgabe
	{
        /// <summary>
        /// mes communication status
        /// </summary>
		public ENUM_MesKommunikationsStatus PRO_enuKommunikationsStatus
		{
			get;
			set;
		}

        /// <summary>
        /// 
        /// </summary>
		public Dictionary<ENUM_MesRueckgabeArgumente, object> PRO_dicArgumente
		{
			get;
			set;
		}
	}
}
