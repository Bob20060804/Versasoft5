using System;

namespace BR.AN.PviServices
{
	public class FunctionInfo
	{
		internal string moduleName;

		internal uint codeOffset;

		public string ModuleName => moduleName;

		[CLSCompliant(false)]
		public uint CodeOffset
		{
			get
			{
				return codeOffset;
			}
		}

		internal FunctionInfo()
		{
		}

		internal FunctionInfo(ExceptionBRModuleFunction functionInfo)
		{
			moduleName = functionInfo.brmName;
			codeOffset = functionInfo.codeOffset;
		}

		public override string ToString()
		{
			return "CodeOffset=\"0x" + $"{codeOffset:X8}" + "\" ModuleName=\"" + moduleName.ToString() + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string arg = $"<tr>\r\n<td>ModuleName</td>\r\n<td>{ModuleName}</td>\r\n</tr>";
			string arg2 = $"<tr>\r\n<td>CodeOffset</td>\r\n<td>{CodeOffset}</td>\r\n</tr>";
			string str = $"<tr>\r\n<td align=\"left\" valign=\"top\">FunctionInfo</td>\r\n<td >\r\n<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\" style=\"border-collapse: collapse\">{arg}{arg2}\r\n";
			return str + "</table>\r\n</td>\r\n</tr>\r\n";
		}

		internal virtual string ToStringCSV()
		{
			return string.Format("\"{0}\";\"0x{1:X8}\";", (moduleName != null) ? moduleName : "", CodeOffset);
		}
	}
}
