using Ersa.Global.Mvvm;

namespace Ersa.Platform.UI.Positionsanzeige
{
	public class EDC_Zelle : BindableBase
	{
		private ENUM_ZellenStatus m_enmStatus;

		private bool m_blnIstAusgewaehlt;

		public ENUM_ZellenStatus PRO_enmStatus
		{
			get
			{
				return m_enmStatus;
			}
			set
			{
				SetProperty(ref m_enmStatus, value, "PRO_enmStatus");
			}
		}

		public bool PRO_blnIstAusgewaehlt
		{
			get
			{
				return m_blnIstAusgewaehlt;
			}
			set
			{
				SetProperty(ref m_blnIstAusgewaehlt, value, "PRO_blnIstAusgewaehlt");
			}
		}

		public object PRO_objContent
		{
			get;
			set;
		}
	}
}
