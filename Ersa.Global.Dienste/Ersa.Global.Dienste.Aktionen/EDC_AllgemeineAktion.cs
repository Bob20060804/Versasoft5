using Ersa.Global.Dienste.Interfaces;
using System;

namespace Ersa.Global.Dienste.Aktionen
{
	public class EDC_AllgemeineAktion : INF_UndoableAction
	{
		private readonly Action m_delDo;

		private readonly Action m_delUndo;

		public EDC_AllgemeineAktion(Action i_delDo, Action i_delUndo)
		{
			m_delDo = i_delDo;
			m_delUndo = i_delUndo;
		}

		public void SUB_Do()
		{
			m_delDo?.Invoke();
		}

		public void SUB_Undo()
		{
			m_delUndo?.Invoke();
		}
	}
}
