using Ersa.Global.Mvvm;

namespace Ersa.Platform.UI.Codeleser
{
	public abstract class EDC_CodeleserBasis : BindableBase
	{
		private bool m_blnIstAktiv;

		private bool m_blnDarfBedientWerden = true;

		public long PRO_i64ArrayIndex
		{
			get;
		}

		public string PRO_strBezeichnung
		{
			get;
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

		public bool PRO_blnDarfBedientWerden
		{
			get
			{
				return m_blnDarfBedientWerden;
			}
			set
			{
				SetProperty(ref m_blnDarfBedientWerden, value, "PRO_blnDarfBedientWerden");
			}
		}

		protected EDC_CodeleserBasis(long i_i64ArrayIndex, string i_strBezeichnung)
		{
			PRO_i64ArrayIndex = i_i64ArrayIndex;
			PRO_strBezeichnung = i_strBezeichnung;
		}
	}
}
