namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_IoDialogHelfer
	{
		string FUN_strOpenFileDialog(string i_strInitialesVerzeichnis, string i_strFilter);

		string FUN_strSaveFiledialog(string i_strFilter, string i_strDefaultVerzeichnis, string i_strDefaultFileExtension, string i_strDefaultFileName = null);

		string FUN_strBrowseFolderDialog(string i_strInitialesVerzeichnis = null);
	}
}
