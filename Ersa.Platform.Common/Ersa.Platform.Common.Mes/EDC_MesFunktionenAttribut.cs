using System;

namespace Ersa.Platform.Common.Mes
{
	public class EDC_MesFunktionenAttribut : Attribute
	{
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
