using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public static class EDC_RoutedCommands
	{
		public static readonly RoutedCommand ms_cmdDataGridRowAusgeklapptGeaendert = FUN_cmdCommandErstellen("ms_cmdDataGridRowAusgeklapptGeaendert");

		public static readonly RoutedCommand ms_cmdExpanderGroupBoxEingeklappt = FUN_cmdCommandErstellen("ms_cmdExpanderGroupBoxEingeklappt");

		public static readonly RoutedCommand ms_cmdExpanderGroupBoxAusgeklappt = FUN_cmdCommandErstellen("ms_cmdExpanderGroupBoxAusgeklappt");

		public static readonly RoutedCommand ms_cmdPasswortGeaendert = FUN_cmdCommandErstellen("ms_cmdPasswortGeaendert");

		private static RoutedCommand FUN_cmdCommandErstellen(string i_strName)
		{
			return new RoutedCommand(i_strName, typeof(EDC_RoutedCommands));
		}
	}
}
