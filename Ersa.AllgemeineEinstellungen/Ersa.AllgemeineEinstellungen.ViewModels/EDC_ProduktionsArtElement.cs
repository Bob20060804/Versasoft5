using Ersa.Global.Mvvm;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public abstract class EDC_ProduktionsArtElement : BindableBase
	{
		private bool m_blnIstBedienbar = true;

		public bool PRO_blnBedienbar
		{
			get
			{
				return m_blnIstBedienbar;
			}
			set
			{
				SetProperty(ref m_blnIstBedienbar, value, "PRO_blnBedienbar");
			}
		}

		public string PRO_strLocKey
		{
			get;
		}

		protected EDC_ProduktionsArtElement(string i_strLocKey)
		{
			PRO_strLocKey = i_strLocKey;
		}
	}
}
