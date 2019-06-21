using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Interfaces;
using Ersa.Global.Controls.Editoren.Interfaces.Intern;
using Ersa.Global.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public abstract class EDC_Tool : BindableBase
	{
		private readonly IDictionary<EDC_EditorElement, IDisposable> m_dicTemporaereElementeDisposables = new Dictionary<EDC_EditorElement, IDisposable>();

		private readonly object m_objLock = new object();

		private INF_EditorKontext m_edcEditorKontext;

		private INF_WerkzeugKontext m_edcWerkzeugKontext;

		public abstract bool PRO_blnErlaubeKontextmenue
		{
			get;
		}

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_WerkzeugKontextInitialisieren(INF_WerkzeugKontext i_edcWerkzeugKontext)
		{
			m_edcWerkzeugKontext = i_edcWerkzeugKontext;
		}

		public virtual void SUB_AlleVerfuegbarenKontexteInitialisieren(params object[] ia_objKontexte)
		{
			INF_WerkzeugKontext iNF_WerkzeugKontext = ia_objKontexte.OfType<INF_WerkzeugKontext>().FirstOrDefault();
			if (iNF_WerkzeugKontext != null)
			{
				m_edcWerkzeugKontext = iNF_WerkzeugKontext;
			}
		}

		public virtual bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			return false;
		}

		public virtual bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			return false;
		}

		public virtual bool FUN_blnMouseUp(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			return false;
		}

		public virtual void SUB_MouseLeave()
		{
		}

		public virtual void SUB_MouseEnter()
		{
		}

		public virtual void SUB_PreviewKeyDown(Key i_enmKey)
		{
		}

		public virtual void SUB_SetzeInitialeMausposition(Point i_sttPosition, bool i_blnMausInBildEditor)
		{
		}

		public virtual void SUB_WerkzeugDeaktiviert()
		{
		}

		public virtual Cursor FUN_fdcHoleWerkzeugCursor()
		{
			return Cursors.Arrow;
		}

		internal virtual void SUB_EditorKontextInitialisieren(INF_EditorKontext i_edcEditorKontext)
		{
			m_edcEditorKontext = i_edcEditorKontext;
		}

		protected IEnumerable<EDC_EditorElement> FUN_enuHoleAlleElemente()
		{
			return m_edcEditorKontext?.FUN_enuHoleAlleElemente() ?? Enumerable.Empty<EDC_EditorElement>();
		}

		protected IEnumerable<EDC_EditorElement> FUN_enuHoleAlleAusgewaehltenElemente()
		{
			return from i_edcElement in FUN_enuHoleAlleElemente()
			where i_edcElement.PRO_blnAusgewaehlt
			select i_edcElement;
		}

		protected EDC_EditorElement FUN_edcHoleElementAnPosition(Point i_sttPosition)
		{
			return m_edcEditorKontext?.FUN_edcHoleElementAnPosition(i_sttPosition);
		}

		protected IEnumerable<EDC_EditorElement> FUN_enuHoleElementeInBereich(Rect i_sttBereich)
		{
			return from i_edcElement in FUN_enuHoleAlleElemente()
			where i_edcElement.FUN_blnIstInBereich(i_sttBereich)
			select i_edcElement;
		}

		protected void SUB_FuegeElementTemporaerHinzu(EDC_EditorElement i_edcElement)
		{
			if (m_edcEditorKontext != null)
			{
				IDisposable value = m_edcEditorKontext.FUN_fdcFuegeElementTemporaerHinzu(i_edcElement);
				lock (m_objLock)
				{
					m_dicTemporaereElementeDisposables.Add(i_edcElement, value);
				}
			}
		}

		protected void SUB_EntferneTemporaeresElement(EDC_EditorElement i_edcElement)
		{
			if (i_edcElement != null)
			{
				lock (m_objLock)
				{
					m_dicTemporaereElementeDisposables[i_edcElement]?.Dispose();
					m_dicTemporaereElementeDisposables.Remove(i_edcElement);
				}
			}
		}

		protected void SUB_AendereBildausschnitt(Rect i_sttBereich)
		{
			m_edcEditorKontext?.SUB_AendereBildausschnitt(i_sttBereich);
		}

		protected void SUB_WerkzeugDeaktivierenAnfordern()
		{
			m_edcWerkzeugKontext?.SUB_WerkzeugDeaktivierenAnfordern();
		}
	}
}
