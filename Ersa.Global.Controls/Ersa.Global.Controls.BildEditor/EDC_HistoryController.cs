using Ersa.Global.Controls.BildEditor.Commands;
using System;
using System.Collections.Generic;

namespace Ersa.Global.Controls.BildEditor
{
	public class EDC_HistoryController
	{
		private readonly EDC_BildEditorCanvas m_edcBildEditorCanvas;

		private List<EDC_BildEditorCommandBase> m_lstHistory;

		private int m_i32NaechsterUndoSchritt;

		public bool CanUndo
		{
			get
			{
				if (m_i32NaechsterUndoSchritt >= 0)
				{
					return m_i32NaechsterUndoSchritt <= m_lstHistory.Count - 1;
				}
				return false;
			}
		}

		public bool CanRedo => m_i32NaechsterUndoSchritt != m_lstHistory.Count - 1;

		public bool IsDirty => CanUndo;

		public event EventHandler StateChanged;

		public EDC_HistoryController(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			m_edcBildEditorCanvas = i_edcBildEditorCanvas;
			ClearHistory();
		}

		public void ClearHistory()
		{
			m_lstHistory = new List<EDC_BildEditorCommandBase>();
			m_i32NaechsterUndoSchritt = -1;
			RaiseStateChangedEvent();
		}

		public void AddCommandToHistory(EDC_BildEditorCommandBase i_blnBildEditorCommand)
		{
			TrimHistoryList();
			m_lstHistory.Add(i_blnBildEditorCommand);
			m_i32NaechsterUndoSchritt++;
			RaiseStateChangedEvent();
		}

		public void Undo()
		{
			if (CanUndo)
			{
				m_lstHistory[m_i32NaechsterUndoSchritt].SUB_Undo(m_edcBildEditorCanvas);
				m_i32NaechsterUndoSchritt--;
				RaiseStateChangedEvent();
			}
		}

		public void Redo()
		{
			if (CanRedo)
			{
				int index = m_i32NaechsterUndoSchritt + 1;
				m_lstHistory[index].SUB_Redo(m_edcBildEditorCanvas);
				m_i32NaechsterUndoSchritt++;
				RaiseStateChangedEvent();
			}
		}

		private void TrimHistoryList()
		{
			if (m_lstHistory.Count != 0 && m_i32NaechsterUndoSchritt != m_lstHistory.Count - 1)
			{
				for (int num = m_lstHistory.Count - 1; num > m_i32NaechsterUndoSchritt; num--)
				{
					m_lstHistory.RemoveAt(num);
				}
			}
		}

		private void RaiseStateChangedEvent()
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, EventArgs.Empty);
			}
		}
	}
}
