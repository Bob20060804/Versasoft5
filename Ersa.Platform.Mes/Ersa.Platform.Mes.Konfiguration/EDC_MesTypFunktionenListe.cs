using Ersa.Platform.Common.Mes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Konfiguration
{
	[Serializable]
	public class EDC_MesTypFunktionenListe : IEquatable<EDC_MesTypFunktionenListe>
	{
		private List<EDC_MesFunktionenKonfiguration> m_lstMesFunktionen = new List<EDC_MesFunktionenKonfiguration>();

		public ENUM_MesTyp PRO_enuMesTyp
		{
			get;
			set;
		}

		public List<EDC_MesFunktionenKonfiguration> PRO_lstMesFunktionen
		{
			get
			{
				return m_lstMesFunktionen;
			}
			set
			{
				m_lstMesFunktionen = value;
			}
		}

		public bool Equals(EDC_MesTypFunktionenListe i_edcOther)
		{
			if (i_edcOther == null)
			{
				return false;
			}
			if (PRO_enuMesTyp.Equals(i_edcOther.PRO_enuMesTyp))
			{
				return FUN_blnSindListenGleich(PRO_lstMesFunktionen, i_edcOther.PRO_lstMesFunktionen);
			}
			return false;
		}

		public override string ToString()
		{
			return base.ToString() + " (" + PRO_enuMesTyp + ")";
		}

		private bool FUN_blnSindListenGleich(List<EDC_MesFunktionenKonfiguration> i_lstListeA, List<EDC_MesFunktionenKonfiguration> i_lstListeB)
		{
			if (i_lstListeA == null && i_lstListeB == null)
			{
				return true;
			}
			if (i_lstListeA == null && i_lstListeB != null)
			{
				return false;
			}
			if (i_lstListeA != null && i_lstListeB == null)
			{
				return false;
			}
			int count = i_lstListeA.Count;
			if (count != i_lstListeB.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				EDC_MesFunktionenKonfiguration eDC_MesFunktionenKonfiguration = i_lstListeB[i];
				EDC_MesFunktionenKonfiguration eDC_MesFunktionenKonfiguration2 = i_lstListeA[i];
				if (eDC_MesFunktionenKonfiguration2 == null && eDC_MesFunktionenKonfiguration != null)
				{
					return false;
				}
				if (eDC_MesFunktionenKonfiguration2 != null && eDC_MesFunktionenKonfiguration == null)
				{
					return false;
				}
				if ((eDC_MesFunktionenKonfiguration2 != null || eDC_MesFunktionenKonfiguration != null) && !eDC_MesFunktionenKonfiguration2.Equals(eDC_MesFunktionenKonfiguration))
				{
					return false;
				}
			}
			return true;
		}
	}
}
