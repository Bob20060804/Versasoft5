using Ersa.Global.Mvvm;
using Ersa.Platform.Common.Mes;
using System;

namespace Ersa.Platform.Mes.Konfiguration
{
	[Serializable]
	public class EDC_MesFunktionenKonfiguration : BindableBase, IEquatable<EDC_MesFunktionenKonfiguration>
	{
		private bool m_blnIstAktiv;

		public ENUM_MesFunktionen PRO_enmFunktion
		{
			get;
			set;
		}

		public bool PRO_blnIstAktiv
		{
			get
			{
				return m_blnIstAktiv;
			}
			set
			{
				SetProperty(ref m_blnIstAktiv, value, "PRO_blnIstAktiv");
			}
		}

		public bool Equals(EDC_MesFunktionenKonfiguration i_edcOther)
		{
			if (i_edcOther == null)
			{
				return false;
			}
			if (PRO_enmFunktion.Equals(i_edcOther.PRO_enmFunktion))
			{
				return PRO_blnIstAktiv.Equals(i_edcOther.PRO_blnIstAktiv);
			}
			return false;
		}

		public override string ToString()
		{
			return $"Function={PRO_enmFunktion};Enabled={PRO_blnIstAktiv}";
		}
	}
}
