using Ersa.Platform.UI.Common.Interfaces;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel.Composition;
using System.IO;

namespace Ersa.Platform.UI.Common
{
	[Export(typeof(INF_IoDialogHelfer))]
	public class EDC_IoDialogHelfer : INF_IoDialogHelfer
	{
		public string FUN_strOpenFileDialog(string i_strInitialesVerzeichnis, string i_strFilter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = i_strFilter
			};
			if (!string.IsNullOrEmpty(i_strInitialesVerzeichnis))
			{
				openFileDialog.InitialDirectory = Path.GetFullPath(i_strInitialesVerzeichnis);
			}
			if (openFileDialog.ShowDialog() == true)
			{
				return openFileDialog.FileName;
			}
			return null;
		}

		public string FUN_strSaveFiledialog(string i_strFilter, string i_strDefaultVerzeichnis, string i_strDefaultFileExtension, string i_strDefaultFileName = null)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = i_strFilter,
				DefaultExt = i_strDefaultFileExtension,
				InitialDirectory = Path.GetFullPath(i_strDefaultVerzeichnis)
			};
			if (!string.IsNullOrEmpty(i_strDefaultFileName))
			{
				saveFileDialog.FileName = $"{i_strDefaultFileName}.{i_strDefaultFileExtension}";
			}
			if (saveFileDialog.ShowDialog() == true)
			{
				return saveFileDialog.FileName;
			}
			return null;
		}

		public string FUN_strBrowseFolderDialog(string i_strInitialesVerzeichnis = null)
		{
			CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
			{
				IsFolderPicker = true,
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				EnsureFileExists = true,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = false,
				ShowPlacesList = true
			};
			if (!string.IsNullOrEmpty(i_strInitialesVerzeichnis))
			{
				commonOpenFileDialog.InitialDirectory = i_strInitialesVerzeichnis;
				commonOpenFileDialog.DefaultDirectory = i_strInitialesVerzeichnis;
			}
			if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				return commonOpenFileDialog.FileName;
			}
			return null;
		}
	}
}
