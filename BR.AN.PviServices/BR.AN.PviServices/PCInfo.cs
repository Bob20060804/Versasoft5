using System;

namespace BR.AN.PviServices
{
	public class PCInfo
	{
		internal string propModuleName;

		internal uint propCodeOffset;

		public string ModuleName => propModuleName;

		[CLSCompliant(false)]
		public uint CodeOffset
		{
			get
			{
				return propCodeOffset;
			}
		}

		internal PCInfo()
		{
		}

		internal PCInfo(ExceptionBRModulePC pcInfo)
		{
			propModuleName = pcInfo.brmName;
			propCodeOffset = pcInfo.codeOffset;
		}

		public override string ToString()
		{
			return "CodeOffset=\"0x" + $"{propCodeOffset:X8}" + ((propModuleName != null) ? ("\" ModuleName=\"" + propModuleName) : "") + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string arg = $"<tr>\r\n<td>ModuleName</td>\r\n<td>{ModuleName}</td>\r\n</tr>";
			string arg2 = $"<tr>\r\n<td>ModuleName</td>\r\n<td>{CodeOffset}</td>\r\n</tr>";
			string str = $"<tr>\r\n<td align=\"left\" valign=\"top\">PCInfo</td>\r\n<td >\r\n<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">{arg}{arg2}\r\n";
			return str + "</table>\r\n</td>\r\n</tr>\r\n";
		}

		internal virtual string ToStringCSV()
		{
			return string.Format("\"{0}\";\"0x{1:X8}\";", (propModuleName != null) ? propModuleName : "", CodeOffset);
		}

		public bool ReplaceModuleName(string pModuleName)
		{
			if (propModuleName != null && 0 < propModuleName.Length && pModuleName != null && 0 < pModuleName.Length)
			{
				propModuleName = pModuleName;
				return true;
			}
			return false;
		}
	}
}
