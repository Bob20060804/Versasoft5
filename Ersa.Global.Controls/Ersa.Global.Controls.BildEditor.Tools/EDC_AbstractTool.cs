using System.Windows.Input;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public abstract class EDC_AbstractTool
	{
		public abstract void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs);

		public abstract void SUB_OnMouseMove(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseEventArgs i_fdcArgs);

		public abstract void SUB_OnMouseUp(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs);

		public abstract void SUB_SetzeCursor(EDC_BildEditorCanvas i_edcBildEditorCanvas);
	}
}
