using Ersa.Global.Mvvm;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente
{
	public class EDC_VorlageParameter : BindableBase
	{
		private string m_strBeschriftung;

		public string PRO_strBeschriftung
		{
			get
			{
				return m_strBeschriftung;
			}
			set
			{
				SetProperty(ref m_strBeschriftung, value, "PRO_strBeschriftung");
			}
		}
	}
}
