using System.Windows.Input;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public abstract class EDC_RechteckBasisTool : EDC_BasisTool
	{
		public override void SUB_OnMouseMove(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseEventArgs i_fdcArgs)
		{
			i_edcBildEditorCanvas.Cursor = base.PRO_fdcToolCursor;
			if (i_fdcArgs.LeftButton == MouseButtonState.Pressed && i_edcBildEditorCanvas.IsMouseCaptured && i_edcBildEditorCanvas.PRO_i32GrafikAnzahl > 0)
			{
				i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1].SUB_BewegeHandleZu(i_fdcArgs.GetPosition(i_edcBildEditorCanvas), 5);
			}
		}
	}
}
