using System.Collections.Generic;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente
{
	public class EDC_AuswahlVorlagenParameter : EDC_VorlageParameter
	{
		private object m_objWert;

		public object PRO_objWert
		{
			get
			{
				return m_objWert;
			}
			set
			{
				SetProperty(ref m_objWert, value, "PRO_objWert");
			}
		}

		public IList<EDC_AuswahlElement> PRO_lstAuswahlliste
		{
			get;
			set;
		}
	}
}
