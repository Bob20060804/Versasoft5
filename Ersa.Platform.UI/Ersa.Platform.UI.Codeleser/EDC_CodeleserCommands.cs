using System.Windows.Input;

namespace Ersa.Platform.UI.Codeleser
{
	public static class EDC_CodeleserCommands
	{
		public static readonly RoutedCommand ms_cmdCodeLesen = FUN_cmdCommandErstellen("ms_cmdCodeLesen");

		public static readonly RoutedCommand ms_cmdRuestkontrolleLesen = FUN_cmdCommandErstellen("ms_cmdRuestkontrolleLesen");

		private static RoutedCommand FUN_cmdCommandErstellen(string i_strName)
		{
			return new RoutedCommand(i_strName, typeof(EDC_CodeleserCommands));
		}
	}
}
