using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_PunktTool : EDC_BasisTool
	{
		private const string mC_strCursorUri = "pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Punkt.cur";

		public EDC_PunktTool()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Punkt.cur"));
			base.PRO_fdcToolCursor = ((resourceStream != null) ? new Cursor(resourceStream.Stream) : Cursors.Arrow);
		}

		public override void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			EDC_PunktGrafik i_edcGrafik = new EDC_PunktGrafik(i_fdcArgs.GetPosition(i_edcBildEditorCanvas), i_edcBildEditorCanvas.PRO_dblStrichStaerke, i_edcBildEditorCanvas.PRO_fdcGrafikFarbe, i_edcBildEditorCanvas.PRO_dblSkalierung);
			EDC_BasisTool.SUB_FuegeNeuesGrafikObjektHinzu(i_edcBildEditorCanvas, i_edcGrafik);
		}

		public override void SUB_OnMouseMove(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseEventArgs i_fdcArgs)
		{
			i_edcBildEditorCanvas.Cursor = base.PRO_fdcToolCursor;
			if (i_fdcArgs.LeftButton == MouseButtonState.Pressed)
			{
				i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1].SUB_BewegeHandleZu(i_fdcArgs.GetPosition(i_edcBildEditorCanvas), 2);
			}
		}
	}
}
