using System;
using System.Windows.Input;

namespace Ersa.Global.Controls.Helpers
{
	public static class EDC_RoutedCommandHelfer
	{
		public static RoutedCommand FUN_cmdCommandErstellen(string i_strName, Type i_fdcType)
		{
			return new RoutedCommand(i_strName, i_fdcType);
		}
	}
}
