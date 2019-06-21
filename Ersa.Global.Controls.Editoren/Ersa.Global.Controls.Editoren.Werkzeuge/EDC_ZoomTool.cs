using Ersa.Global.Controls.Editoren.EditorElemente;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_ZoomTool : EDC_Tool
	{
		private Point? m_sttDragStartPoint;

		private EDC_RechteckElement m_edcAuswahlRahmen;

		public override bool PRO_blnErlaubeKontextmenue => false;

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			if (i_enmLeftButtonState != MouseButtonState.Pressed)
			{
				m_sttDragStartPoint = null;
				return false;
			}
			bool result = false;
			if (m_sttDragStartPoint.HasValue)
			{
				if (m_edcAuswahlRahmen == null)
				{
					m_edcAuswahlRahmen = new EDC_RechteckElement
					{
						PRO_fdcInnenFarbe = Brushes.Transparent,
						PRO_sttPosition = i_sttPosition,
						PRO_blnAuswaehlbar = false,
						PRO_i32ZIndex = 45
					};
					SUB_FuegeElementTemporaerHinzu(m_edcAuswahlRahmen);
				}
				double x = Math.Min(i_sttPosition.X, m_sttDragStartPoint.GetValueOrDefault().X);
				double y = Math.Min(i_sttPosition.Y, m_sttDragStartPoint.GetValueOrDefault().Y);
				Vector vector = i_sttPosition - m_sttDragStartPoint.GetValueOrDefault();
				m_edcAuswahlRahmen.PRO_sttPosition = new Point(x, y);
				m_edcAuswahlRahmen.SUB_AendereGroesse(new Size(Math.Abs(vector.X), Math.Abs(vector.Y)));
				result = true;
			}
			else
			{
				m_sttDragStartPoint = i_sttPosition;
			}
			return result;
		}

		public override bool FUN_blnMouseUp(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseUp(i_sttPosition, i_enmLeftButtonState);
			m_sttDragStartPoint = null;
			if (m_edcAuswahlRahmen != null)
			{
				SUB_ZoomAuswahlBeendet();
			}
			return false;
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
			}
			return false;
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_ZoomAuswahlBeendet();
		}

		private void SUB_ZoomAuswahlBeendet()
		{
			if (m_edcAuswahlRahmen != null)
			{
				Rect i_sttBereich = m_edcAuswahlRahmen.FUN_sttErmittleBereich();
				SUB_EntferneTemporaeresElement(m_edcAuswahlRahmen);
				m_edcAuswahlRahmen = null;
				SUB_AendereBildausschnitt(i_sttBereich);
			}
		}
	}
}
