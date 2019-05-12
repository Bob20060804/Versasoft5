using Ersa.Global.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_UndoManager))]
	public class EDC_UndoManager : INF_UndoManager
	{
		private readonly IDictionary<Guid, INF_UndoableAction> m_dicZustaende = new Dictionary<Guid, INF_UndoableAction>();

		private bool m_blnErlaubeUndoUndRedo;

		public Guid PRO_sttZustand
		{
			get;
			private set;
		}

		public bool PRO_blnKannUndo
		{
			get
			{
				if (m_blnErlaubeUndoUndRedo)
				{
					return PRO_sttZustand != Guid.Empty;
				}
				return false;
			}
		}

		public bool PRO_blnKannRedo
		{
			get
			{
				if (m_blnErlaubeUndoUndRedo)
				{
					return PRO_sttZustand != m_dicZustaende.LastOrDefault().Key;
				}
				return false;
			}
		}

		public event Action<Guid> m_evtZustandGeaendert;

		public EDC_UndoManager()
		{
			m_blnErlaubeUndoUndRedo = true;
		}

		public void SUB_ErlaubeUndoRedoSetzen(bool i_blnErlauben)
		{
			m_blnErlaubeUndoUndRedo = i_blnErlauben;
			SUB_StatusUpdate();
		}

		public void SUB_Do(INF_UndoableAction i_edcAktion)
		{
			SUB_AktionAufnehmen(i_edcAktion, i_blnAktionAusfuehren: true);
		}

		public void SUB_Undo()
		{
			if (PRO_blnKannUndo)
			{
				INF_UndoableAction iNF_UndoableAction = m_dicZustaende[PRO_sttZustand];
				if (iNF_UndoableAction != null)
				{
					Guid value = (from i_fdcKvp in FUN_enuErmittleVorherigeAktionen()
					select i_fdcKvp.Key).LastOrDefault();
					iNF_UndoableAction.SUB_Undo();
					SUB_StatusUpdate(value);
				}
			}
		}

		public void SUB_Redo()
		{
			if (PRO_blnKannRedo)
			{
				KeyValuePair<Guid, INF_UndoableAction> keyValuePair = FUN_enuErmittleNachfolgendeAktionen().FirstOrDefault();
				if (!(keyValuePair.Key == Guid.Empty) && keyValuePair.Value != null)
				{
					keyValuePair.Value.SUB_Do();
					SUB_StatusUpdate(keyValuePair.Key);
				}
			}
		}

		public void SUB_AddUndoAction(INF_UndoableAction i_edcAktion)
		{
			SUB_AktionAufnehmen(i_edcAktion, i_blnAktionAusfuehren: false);
		}

		public void SUB_AddRedoAction(INF_UndoableAction i_edcAktion)
		{
			m_dicZustaende.Add(Guid.NewGuid(), i_edcAktion);
			SUB_StatusUpdate();
		}

		public void SUB_Reset()
		{
			m_dicZustaende.Clear();
			SUB_StatusUpdate(Guid.Empty);
		}

		private void SUB_AktionAufnehmen(INF_UndoableAction i_edcAktion, bool i_blnAktionAusfuehren)
		{
			Guid[] array = (from i_fdcKvp in FUN_enuErmittleNachfolgendeAktionen()
			select i_fdcKvp.Key).ToArray();
			foreach (Guid key in array)
			{
				m_dicZustaende.Remove(key);
			}
			Guid guid = Guid.NewGuid();
			m_dicZustaende.Add(guid, i_edcAktion);
			if (i_blnAktionAusfuehren)
			{
				i_edcAktion.SUB_Do();
			}
			SUB_StatusUpdate(guid);
		}

		private IEnumerable<KeyValuePair<Guid, INF_UndoableAction>> FUN_enuErmittleVorherigeAktionen()
		{
			if (!(PRO_sttZustand == Guid.Empty))
			{
				return m_dicZustaende.TakeWhile((KeyValuePair<Guid, INF_UndoableAction> i_fdcKvp) => i_fdcKvp.Key != PRO_sttZustand);
			}
			return Enumerable.Empty<KeyValuePair<Guid, INF_UndoableAction>>();
		}

		private IEnumerable<KeyValuePair<Guid, INF_UndoableAction>> FUN_enuErmittleNachfolgendeAktionen()
		{
			if (!(PRO_sttZustand == Guid.Empty))
			{
				return m_dicZustaende.SkipWhile((KeyValuePair<Guid, INF_UndoableAction> i_fdcKvp) => i_fdcKvp.Key != PRO_sttZustand).Skip(1);
			}
			return m_dicZustaende;
		}

		private void SUB_StatusUpdate(Guid? i_sttNeueId = default(Guid?))
		{
			if (i_sttNeueId.HasValue)
			{
				PRO_sttZustand = i_sttNeueId.Value;
			}
			this.m_evtZustandGeaendert?.Invoke(PRO_sttZustand);
		}
	}
}
