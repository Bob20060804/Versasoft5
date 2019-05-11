using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_LaufwerkHelfer
	{
		public static bool FUN_blnIstVerzeichnisLokal(string i_strVerzeichnis)
		{
			if (string.IsNullOrEmpty(i_strVerzeichnis) || string.IsNullOrWhiteSpace(i_strVerzeichnis))
			{
				return false;
			}
			try
			{
				DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(new FileInfo(i_strVerzeichnis).FullName));
				return DriveType.Fixed.Equals(driveInfo.DriveType);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool FUN_blnIstVerzeichnisBeschreibbar(string i_strVerzeichnis)
		{
			try
			{
				AuthorizationRuleCollection accessRules = Directory.GetAccessControl(i_strVerzeichnis).GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier));
				bool flag = accessRules.Cast<FileSystemAccessRule>().Any((FileSystemAccessRule i_fdcRegel) => (i_fdcRegel.FileSystemRights & FileSystemRights.WriteData) > (FileSystemRights)0);
				bool flag2 = accessRules.Cast<FileSystemAccessRule>().Any((FileSystemAccessRule i_fdcRegel) => (i_fdcRegel.FileSystemRights & FileSystemRights.WriteData) > (FileSystemRights)0);
				bool flag3 = accessRules.Cast<FileSystemAccessRule>().Any((FileSystemAccessRule i_fdcRegel) => (i_fdcRegel.FileSystemRights & FileSystemRights.Read) > (FileSystemRights)0);
				return (flag && flag2) & flag3;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
