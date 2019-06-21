using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_BauteilAuswahlTool : EDC_Tool
	{
		private readonly Cursor m_fdcCursor;

		private Action<Point> m_delPositionAusgewaehlt;

		public override bool PRO_blnErlaubeKontextmenue => false;

		public EDC_BauteilAuswahlTool()
		{
			m_fdcCursor = FUN_fdcCursorErstellen();
		}

		public void SUB_WerkzeugInitialisieren(Action<Point> i_delPositionAusgewaehlt)
		{
			m_delPositionAusgewaehlt = i_delPositionAusgewaehlt;
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				m_delPositionAusgewaehlt?.Invoke(i_sttPosition);
			}
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
			}
			return false;
		}

		public override Cursor FUN_fdcHoleWerkzeugCursor()
		{
			return m_fdcCursor ?? base.FUN_fdcHoleWerkzeugCursor();
		}

		private static Cursor FUN_fdcCursorErstellen()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/Bilder/Cursors/Cursor_Bauteilauswahl_32x32.cur", UriKind.Absolute));
			if (resourceStream == null)
			{
				return null;
			}
			return new Cursor(resourceStream.Stream);
		}
	}
}
