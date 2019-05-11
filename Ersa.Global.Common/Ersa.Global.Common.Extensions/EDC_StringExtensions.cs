using System;

namespace Ersa.Global.Common.Extensions
{
	public static class EDC_StringExtensions
	{
		public static bool FUN_blnContainsCaseInsensitive(this string i_strText, string i_strSuchbegriff)
		{
			return i_strText.IndexOf(i_strSuchbegriff, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}
