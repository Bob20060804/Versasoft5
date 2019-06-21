using Ersa.Global.Controls.Helpers;
using System.Windows.Input;

namespace Ersa.Global.Controls.Editoren
{
	public static class EDC_RoutedCommands
	{
		public static readonly RoutedCommand ms_cmdBildEditorMausPositionGeaendert = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdBildEditorMausPositionGeaendert", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittAusgewaehlt = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittAusgewaehlt", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschritteModifizieren = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschritteModifizieren", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittElementeEntfernen = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittElementeEntfernen", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittAktivierungGeaendert = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittAktivierungGeaendert", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittElementGeaendert = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittElementGeaendert", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittElementWertGeaendert = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittElementWertGeaendert", typeof(EDC_RoutedCommands));

		public static readonly RoutedCommand ms_cmdAblaufschrittElementAuswahlWertGeaendert = EDC_RoutedCommandHelfer.FUN_cmdCommandErstellen("ms_cmdAblaufschrittElementAuswahlWertGeaendert", typeof(EDC_RoutedCommands));
	}
}
