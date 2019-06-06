namespace Ersa.Global.Controls.BildEditor.Commands
{
	public abstract class EDC_BildEditorCommandBase
	{
		public abstract void SUB_Undo(EDC_BildEditorCanvas i_edcBildEditorCanvas);

		public abstract void SUB_Redo(EDC_BildEditorCanvas i_edcBildEditorCanvas);
	}
}
