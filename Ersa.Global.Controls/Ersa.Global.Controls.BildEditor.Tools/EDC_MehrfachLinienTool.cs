using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_MehrfachLinienTool : EDC_BasisTool
	{
		private const double mC_dblMinimalerAbstand = 15.0;

		private const string mC_strCursorUri = "pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/MehrfachLinie.cur";

		private double m_dblLetzetePositionX;

		private double m_dblLetztePositionY;

		private EDC_MehrfachLinienGrafik m_edcMehrfachLinienGrafik;

		private Point m_fdcLetztePosition;

		public EDC_MehrfachLinienTool()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/MehrfachLinie.cur"));
			base.PRO_fdcToolCursor = ((resourceStream != null) ? new Cursor(resourceStream.Stream) : Cursors.Arrow);
		}

		public override void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
			m_edcMehrfachLinienGrafik = new EDC_MehrfachLinienGrafik(new Point[2]
			{
				position,
				new Point(position.X + 1.0, position.Y + 1.0)
			}, i_edcBildEditorCanvas.PRO_dblStrichStaerke, i_edcBildEditorCanvas.PRO_fdcGrafikFarbe, i_edcBildEditorCanvas.PRO_dblSkalierung);
			EDC_BasisTool.SUB_FuegeNeuesGrafikObjektHinzu(i_edcBildEditorCanvas, m_edcMehrfachLinienGrafik);
			m_dblLetzetePositionX = position.X;
			m_dblLetztePositionY = position.Y;
		}

		public override void SUB_OnMouseMove(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseEventArgs i_fdcArgs)
		{
			i_edcBildEditorCanvas.Cursor = base.PRO_fdcToolCursor;
			if (i_fdcArgs.LeftButton == MouseButtonState.Pressed && i_edcBildEditorCanvas.IsMouseCaptured && m_edcMehrfachLinienGrafik != null)
			{
				m_fdcLetztePosition = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
				double num = (m_fdcLetztePosition.X - m_dblLetzetePositionX) * (m_fdcLetztePosition.X - m_dblLetzetePositionX) + (m_fdcLetztePosition.Y - m_dblLetztePositionY) * (m_fdcLetztePosition.Y - m_dblLetztePositionY);
				double num2 = (i_edcBildEditorCanvas.PRO_dblSkalierung <= 0.0) ? 225.0 : (225.0 / i_edcBildEditorCanvas.PRO_dblSkalierung);
				if (num < num2)
				{
					m_edcMehrfachLinienGrafik.SUB_BewegeHandleZu(m_fdcLetztePosition, m_edcMehrfachLinienGrafik.PRO_i32HandleAnzahl);
					return;
				}
				m_dblLetzetePositionX = m_fdcLetztePosition.X;
				m_dblLetztePositionY = m_fdcLetztePosition.Y;
			}
		}

		public override void SUB_OnMouseUp(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			if (m_edcMehrfachLinienGrafik != null)
			{
				m_edcMehrfachLinienGrafik.SUB_FuegePunktHinzu(m_fdcLetztePosition);
				m_edcMehrfachLinienGrafik = null;
				base.SUB_OnMouseUp(i_edcBildEditorCanvas, i_fdcArgs);
			}
		}
	}
}
