using System;

namespace Ersa.Platform.Common.Mes
{
	public class EDC_MesRueckgabeArgumenteAttribut : Attribute
	{
		public Type PRO_objTyp
		{
			get;
			private set;
		}

		public string PRO_strBezeichner
		{
			get;
			private set;
		}

		public ENUM_MesFunktionen PRO_enmMesFunktion
		{
			get;
			private set;
		}

		internal EDC_MesRueckgabeArgumenteAttribut(ENUM_MesFunktionen i_enmMesFunktion, Type i_objTyp, string i_strBezeichner)
		{
			PRO_enmMesFunktion = i_enmMesFunktion;
			PRO_objTyp = i_objTyp;
			PRO_strBezeichner = i_strBezeichner;
		}
	}
}
