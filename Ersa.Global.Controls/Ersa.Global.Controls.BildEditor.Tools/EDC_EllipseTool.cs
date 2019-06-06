using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_EllipseTool : EDC_RechteckBasisTool
	{
		private const string mC_strCursorUri = "pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Ellipse.cur";

		public EDC_EllipseTool()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Ellipse.cur"));
			base.PRO_fdcToolCursor = ((resourceStream != null) ? new Cursor(resourceStream.Stream) : Cursors.Arrow);
		}

		public override void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
			EDC_EllipsenGrafik i_edcGrafik = new EDC_EllipsenGrafik(position.X, position.Y, position.X + 1.0, position.Y + 1.0, i_edcBildEditorCanvas.PRO_dblStrichStaerke, i_edcBildEditorCanvas.PRO_fdcGrafikFarbe, i_edcBildEditorCanvas.PRO_dblSkalierung);
			EDC_BasisTool.SUB_FuegeNeuesGrafikObjektHinzu(i_edcBildEditorCanvas, i_edcGrafik);
		}
	}
}
