using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ersa.Global.DataProvider.Helfer
{
	public static class EDC_UncPfadHelfer
	{
		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int WNetGetConnection([MarshalAs(UnmanagedType.LPTStr)] string i_strLocalName, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder i_fdcRemoteName, ref int i_i32Length);

		public static string FUN_strHoleUncPfad(string i_strLaufwerkpfadoriginalPath)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int i_i32Length = stringBuilder.Capacity;
			if (i_strLaufwerkpfadoriginalPath.Length > 2 && i_strLaufwerkpfadoriginalPath[1] == ':')
			{
				char c = i_strLaufwerkpfadoriginalPath[0];
				if (((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) && WNetGetConnection(i_strLaufwerkpfadoriginalPath.Substring(0, 2), stringBuilder, ref i_i32Length) == 0)
				{
					new DirectoryInfo(i_strLaufwerkpfadoriginalPath);
					string path = Path.GetFullPath(i_strLaufwerkpfadoriginalPath).Substring(Path.GetPathRoot(i_strLaufwerkpfadoriginalPath).Length);
					return Path.Combine(stringBuilder.ToString().TrimEnd(), path);
				}
			}
			return i_strLaufwerkpfadoriginalPath;
		}
	}
}
