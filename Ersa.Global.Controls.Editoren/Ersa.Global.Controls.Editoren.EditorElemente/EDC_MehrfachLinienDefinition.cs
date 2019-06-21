using Ersa.Global.Mvvm;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_MehrfachLinienDefinition : BindableBase
	{
		private EDC_LinienElement m_edcLinienElement;

		private Vector m_edcVerschiebung;

		public EDC_LinienElement PRO_edcLinienElement
		{
			get
			{
				return m_edcLinienElement;
			}
			set
			{
				SetProperty(ref m_edcLinienElement, value, "PRO_edcLinienElement");
			}
		}

		public Vector PRO_sttVerschiebung
		{
			get
			{
				return m_edcVerschiebung;
			}
			set
			{
				SetProperty(ref m_edcVerschiebung, value, "PRO_sttVerschiebung");
			}
		}

		public EDC_MehrfachLinienDefinition(EDC_LinienElement i_edcLinienElement, Vector i_sttVerschiebung)
		{
			PRO_sttVerschiebung = i_sttVerschiebung;
			PRO_edcLinienElement = i_edcLinienElement;
		}
	}
}
