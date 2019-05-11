using System;

namespace BR.AN.PviServices
{
	public class Callstack
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

		internal Callstack()
		{
		}

		internal Callstack(ExceptionBRModuleCallstack callstack)
		{
			propModuleName = callstack.brmName;
			propCodeOffset = callstack.codeOffset;
		}

		public override string ToString()
		{
			return "CodeOffset=\"" + propCodeOffset + "\" ModuleName=\"" + propModuleName.ToString() + "\"";
		}

		internal virtual string ToStringHTM()
		{
			string arg = $"<tr>\r\n<td>ModuleName</td>\r\n<td>{ModuleName}</td>\r\n</tr>";
			string arg2 = $"<tr>\r\n<td>CodeOffset</td>\r\n<td>{CodeOffset}</td>\r\n</tr>";
			string str = $"<tr>\r\n<td align=\"left\" valign=\"top\">CallStack</td>\r\n<td >\r\n<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">{arg}{arg2}\r\n";
			return str + "</table>\r\n</td>\r\n</tr>\r\n";
		}

		internal virtual string ToStringCSV()
		{
			return string.Format("\"{0}\";\"{1:X8}\";", (propModuleName != null) ? propModuleName : "", CodeOffset);
		}
	}
}
