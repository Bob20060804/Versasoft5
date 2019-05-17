using System;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_TypeExtension
	{
		public static bool FUN_blnIstTypAbgeleitet(this Type i_typPruefTyp, Type i_typBasisTyp)
		{
			bool flag = false;
			Type type = i_typPruefTyp;
			while (!flag && type != null)
			{
				flag = (type == i_typBasisTyp);
				type = type.BaseType;
			}
			return flag;
		}
	}
}
