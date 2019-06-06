using Ersa.Global.Controls.BildEditor.Commands;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public abstract class EDC_BasisTool : EDC_AbstractTool
	{
		protected Cursor PRO_fdcToolCursor
		{
			get;
			set;
		}

		public override void SUB_SetzeCursor(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_edcBildEditorCanvas.Cursor = PRO_fdcToolCursor;
		}

		public override void SUB_OnMouseUp(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			if (i_edcBildEditorCanvas.PRO_i32GrafikAnzahl > 0)
			{
				i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1].SUB_Normalisiere();
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(new EDC_BildEditorCommandAdd(i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1]));
			}
			i_edcBildEditorCanvas.ReleaseMouseCapture();
		}

		protected static void SUB_FuegeNeuesGrafikObjektHinzu(EDC_BildEditorCanvas i_edcBildEditorCanvas, EDC_GrafikBasisObjekt i_edcGrafik)
		{
			i_edcBildEditorCanvas.SUB_UnselektiereAlleGrafikObjekte();
			i_edcGrafik.PRO_blnIstSelektiert = true;
			i_edcGrafik.Clip = new RectangleGeometry(new Rect(0.0, 0.0, i_edcBildEditorCanvas.ActualWidth, i_edcBildEditorCanvas.ActualHeight));
			i_edcBildEditorCanvas.SUB_FuegeNeuesGrafikObjektHinzu(i_edcGrafik);
		}
	}
}
