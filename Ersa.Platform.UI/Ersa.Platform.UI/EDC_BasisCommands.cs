using System;
using System.Windows.Input;

namespace Ersa.Platform.UI
{
	[Obsolete("Ersa.Platform.UI.Common.EDC_BasisCommands verwenden")]
	public static class EDC_BasisCommands
	{
		public static readonly RoutedCommand ms_cmdWertGeaendert = FUN_cmdCommandErstellen("ms_cmdWertGeaendert");

		public static readonly RoutedCommand ms_cmdSpeichern = FUN_cmdCommandErstellen("ms_cmdSpeichern");

		public static readonly RoutedCommand ms_cmdVerwerfen = FUN_cmdCommandErstellen("ms_cmdVerwerfen");

		public static readonly RoutedCommand ms_cmdVerlassen = FUN_cmdCommandErstellen("ms_cmdVerlassen");

		public static readonly RoutedCommand ms_cmdPositionsAusgewaehlt = FUN_cmdCommandErstellen("ms_cmdPositionsAusgewaehlt");

		private static RoutedCommand FUN_cmdCommandErstellen(string i_strName)
		{
			return new RoutedCommand(i_strName, typeof(EDC_BasisCommands));
		}
	}
}
