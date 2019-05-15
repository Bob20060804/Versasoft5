using Ersa.Global.Mvvm;

namespace Ersa.Platform.UI.ParallelBedienung
{
	public class EDC_BildschirmEinstellungen : BindableBase
	{
		private bool m_blnIstAktiv;

		public int PRO_i32Nummer
		{
			get;
			private set;
		}

		public bool PRO_blnIstPrimaer
		{
			get;
			private set;
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

		public EDC_BildschirmEinstellungen(int i_i32Nummer, bool i_blnIstPrimaer, bool i_blnIstAktiv)
		{
			PRO_i32Nummer = i_i32Nummer;
			PRO_blnIstPrimaer = i_blnIstPrimaer;
			PRO_blnIstAktiv = i_blnIstAktiv;
		}
	}
}
