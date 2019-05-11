using System;
using System.IO;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_Base64Helfer
	{
		public static string FUN_strGetFileContentAsBase64(string i_strFile)
		{
			using (FileStream fileStream = File.Open(i_strFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				return Convert.ToBase64String(new BinaryReader(fileStream).ReadBytes((int)fileStream.Length));
			}
		}

		public static byte[] FUN_bytGetBytesFromBase64String(string i_strContent)
		{
			return Convert.FromBase64String(i_strContent);
		}

		public static string FUN_strWriteFileContentToFile(string i_strFileName, string i_strPath, string i_strContent)
		{
			string text = Path.Combine(i_strPath, i_strFileName);
			File.WriteAllBytes(text, FUN_bytGetBytesFromBase64String(i_strContent));
			return text;
		}
	}
}
