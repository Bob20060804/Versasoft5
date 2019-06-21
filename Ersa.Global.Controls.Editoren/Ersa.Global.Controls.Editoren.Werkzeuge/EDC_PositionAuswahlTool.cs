using Ersa.Global.Controls.Editoren.EditorElemente;
using System;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_PositionAuswahlTool : EDC_Tool
	{
		private Action<Point> m_delPositionAusgewaehlt;

		private EDC_EditorElement m_edcAuswahlElement;

		public override bool PRO_blnErlaubeKontextmenue => false;

		public void SUB_WerkzeugInitialisieren(Action<Point> i_delPositionAusgewaehlt)
		{
			m_delPositionAusgewaehlt = i_delPositionAusgewaehlt;
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				if (m_edcAuswahlElement != null)
				{
					SUB_EntferneTemporaeresElement(m_edcAuswahlElement);
					m_edcAuswahlElement = null;
				}
				m_delPositionAusgewaehlt?.Invoke(i_sttPosition);
			}
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
			}
			return false;
		}

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			if (m_edcAuswahlElement == null)
			{
				m_edcAuswahlElement = EDC_GeometrieElement.FUN_edcErzeugeFadenkreuz();
				m_edcAuswahlElement.PRO_blnUebergehtSkalierung = true;
				m_edcAuswahlElement.PRO_i32ZIndex = 45;
				SUB_FuegeElementTemporaerHinzu(m_edcAuswahlElement);
			}
			m_edcAuswahlElement.PRO_sttPosition = i_sttPosition;
			return false;
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_EntferneTemporaeresElement(m_edcAuswahlElement);
			m_edcAuswahlElement = null;
		}
	}
}
