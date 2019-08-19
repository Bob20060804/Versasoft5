using System;

namespace Ersa.Platform.Common.Mes
{
    /// <summary>
    /// MES ∑Ω∑®  Ù–‘
    /// </summary>
	public class EDC_MesFunktionenAttribut : Attribute
	{
        /// <summary>
        /// identifiers
        /// </summary>
		public string PRO_strBezeichner
		{
			get;
			private set;
		}

		internal EDC_MesFunktionenAttribut(string i_strBezeichner)
		{
			PRO_strBezeichner = i_strBezeichner;
		}
	}
}
