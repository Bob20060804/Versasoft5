using Ersa.Global.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Dienste.Aktionen
{
	public class EDC_MultiAktion : INF_UndoableAction
	{
		private readonly IList<INF_UndoableAction> m_lstAktionen = new List<INF_UndoableAction>();

		private readonly bool m_blnUndoRueckwaertsAusfuehren;

		public EDC_MultiAktion(bool i_blnUndoRueckwaertsAusfuehren = false)
		{
			m_blnUndoRueckwaertsAusfuehren = i_blnUndoRueckwaertsAusfuehren;
		}

		public EDC_MultiAktion(IEnumerable<INF_UndoableAction> i_enuAktionen, bool i_blnUndoRueckwaertsAusfuehren = false)
			: this(i_blnUndoRueckwaertsAusfuehren)
		{
			SUB_AktionenAufnehmen(i_enuAktionen);
		}

		[Obsolete("SUB_AktionAufnehmen verwenden")]
		public void SUB_AddAction(INF_UndoableAction i_edcAktion)
		{
			m_lstAktionen.Add(i_edcAktion);
		}

		public void SUB_AktionAufnehmen(INF_UndoableAction i_edcAktion)
		{
			m_lstAktionen.Add(i_edcAktion);
		}

		public void SUB_AktionenAufnehmen(IEnumerable<INF_UndoableAction> i_enuAktionen)
		{
			foreach (INF_UndoableAction item in i_enuAktionen)
			{
				SUB_AktionAufnehmen(item);
			}
		}

		public void SUB_Do()
		{
			foreach (INF_UndoableAction item in m_lstAktionen)
			{
				item.SUB_Do();
			}
		}

		public void SUB_Undo()
		{
			object enumerable;
			if (!m_blnUndoRueckwaertsAusfuehren)
			{
				IEnumerable<INF_UndoableAction> lstAktionen = m_lstAktionen;
				enumerable = lstAktionen;
			}
			else
			{
				enumerable = m_lstAktionen.Reverse();
			}
			foreach (INF_UndoableAction item in (IEnumerable<INF_UndoableAction>)enumerable)
			{
				item.SUB_Undo();
			}
		}
	}
}
