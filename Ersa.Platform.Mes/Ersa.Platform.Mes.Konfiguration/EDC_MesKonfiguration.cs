using Ersa.Global.Common.Extensions;
using Ersa.Platform.Common.Mes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Konfiguration
{
	[Serializable]
	public class EDC_MesKonfiguration
	{
		private const int mC_i32AktuelleVersion = 1;

		private List<EDC_MesTypFunktionenListe> m_lstMesFunktionen;

		public int PRO_i32Version
		{
			get;
			set;
		}

		public ENUM_MesTyp PRO_enuMesTyp
		{
			get;
			set;
		}

		public bool PRO_blnIstFunctionExitAktiv
		{
			get;
			set;
		}

		public List<EDC_MesTypFunktionenListe> PRO_lstMesFunktionen
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

		public EDC_MesFunktionenKonfiguration this[ENUM_MesFunktionen i_enmFunktion]
		{
			get
			{
				if (ENUM_MesTyp.KeinMes.Equals(PRO_enuMesTyp))
				{
					return null;
				}
				return PRO_lstMesFunktionen.Find((EDC_MesTypFunktionenListe i_edcTypFunktionen) => i_edcTypFunktionen.PRO_enuMesTyp.Equals(PRO_enuMesTyp))?.PRO_lstMesFunktionen.Find((EDC_MesFunktionenKonfiguration i_edcFunktion) => i_edcFunktion.PRO_enmFunktion == i_enmFunktion);
			}
		}

		public List<EDC_MesFunktionenKonfiguration> FUN_lstHoleMesFunktionenListeFuerMesTyp(ENUM_MesTyp i_enmMesTyp)
		{
			if (ENUM_MesTyp.KeinMes.Equals(i_enmMesTyp))
			{
				return new List<EDC_MesFunktionenKonfiguration>();
			}
			EDC_MesTypFunktionenListe eDC_MesTypFunktionenListe = PRO_lstMesFunktionen.Find((EDC_MesTypFunktionenListe i_edcTypFunktionen) => i_edcTypFunktionen.PRO_enuMesTyp.Equals(i_enmMesTyp));
			if (eDC_MesTypFunktionenListe == null)
			{
				return new List<EDC_MesFunktionenKonfiguration>();
			}
			return eDC_MesTypFunktionenListe.PRO_lstMesFunktionen;
		}

		public EDC_MesKonfiguration FUN_edcClone()
		{
			EDC_MesKonfiguration edcKopie = new EDC_MesKonfiguration
			{
				PRO_enuMesTyp = PRO_enuMesTyp,
				PRO_blnIstFunctionExitAktiv = PRO_blnIstFunctionExitAktiv,
				PRO_lstMesFunktionen = new List<EDC_MesTypFunktionenListe>()
			};
			edcKopie.PRO_lstMesFunktionen.Clear();
			PRO_lstMesFunktionen.ForEach(delegate(EDC_MesTypFunktionenListe i_edcTypFunktion)
			{
				edcKopie.PRO_lstMesFunktionen.Add(i_edcTypFunktion.FUN_edcDeepClone());
			});
			return edcKopie;
		}

		public bool FUN_blnGleich(EDC_MesKonfiguration i_edcMesKonfiguration)
		{
			if (i_edcMesKonfiguration?.PRO_blnIstFunctionExitAktiv != PRO_blnIstFunctionExitAktiv)
			{
				return false;
			}
			if (i_edcMesKonfiguration.PRO_enuMesTyp != PRO_enuMesTyp)
			{
				return false;
			}
			foreach (EDC_MesTypFunktionenListe edcOrigTypFunktionen in PRO_lstMesFunktionen)
			{
				EDC_MesTypFunktionenListe eDC_MesTypFunktionenListe = i_edcMesKonfiguration.PRO_lstMesFunktionen.Find((EDC_MesTypFunktionenListe i_edcTypFunktionen) => i_edcTypFunktionen.PRO_enuMesTyp.Equals(edcOrigTypFunktionen.PRO_enuMesTyp));
				if (eDC_MesTypFunktionenListe == null)
				{
					return false;
				}
				if (!edcOrigTypFunktionen.Equals(eDC_MesTypFunktionenListe))
				{
					return false;
				}
			}
			return true;
		}

		public void SUB_VerionskompatiblitaetHerstellen()
		{
			if (PRO_i32Version < 1)
			{
				PRO_i32Version = 1;
			}
		}

		public override string ToString()
		{
			return $"Version={PRO_i32Version}|MesType={PRO_enuMesTyp}|FunctionExitEnabled={PRO_blnIstFunctionExitAktiv}";
		}
	}
}
