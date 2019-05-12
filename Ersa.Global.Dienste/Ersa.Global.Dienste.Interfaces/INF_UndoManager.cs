using System;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_UndoManager
	{
		Guid PRO_sttZustand
		{
			get;
		}

		bool PRO_blnKannUndo
		{
			get;
		}

		bool PRO_blnKannRedo
		{
			get;
		}

		event Action<Guid> m_evtZustandGeaendert;

		void SUB_ErlaubeUndoRedoSetzen(bool i_blnErlauben);

		void SUB_Do(INF_UndoableAction i_edcAktion);

		void SUB_Undo();

		void SUB_Redo();

		void SUB_AddUndoAction(INF_UndoableAction i_edcAktion);

		void SUB_AddRedoAction(INF_UndoableAction i_edcAktion);

		void SUB_Reset();
	}
}
