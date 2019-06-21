using System;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	[Obsolete("EDC_RechteckElement verwenden und anstatt PRO_strText neues Property PRO_objContent verwenden")]
	public class EDC_RechteckElementMitText : EDC_RechteckElement
	{
		private string m_strText;

		private Brush m_fdcTextFarbe;

		private Brush m_fdcTextFehlerFarbe;

		public string PRO_strText
		{
			get
			{
				return m_strText;
			}
			set
			{
				SetProperty(ref m_strText, value, "PRO_strText");
			}
		}

		public Brush PRO_fdcTextFarbe
		{
			get
			{
				return m_fdcTextFarbe;
			}
			set
			{
				SetProperty(ref m_fdcTextFarbe, value, "PRO_fdcTextFarbe");
			}
		}

		public Brush PRO_fdcTextFehlerFarbe
		{
			get
			{
				return m_fdcTextFehlerFarbe;
			}
			set
			{
				SetProperty(ref m_fdcTextFehlerFarbe, value, "PRO_fdcTextFehlerFarbe");
			}
		}

		public EDC_RechteckElementMitText()
		{
			m_fdcTextFarbe = EDC_EditorKonstanten.PRO_fdcFarbeDunkel;
			m_fdcTextFehlerFarbe = EDC_EditorKonstanten.PRO_fdcFehlerFarbeDunkel;
		}
	}
}
