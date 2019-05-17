using System.Windows.Input;

namespace Ersa.Platform.UI.Programm
{
	public static class EDC_ProgrammCommands
	{
		public static readonly RoutedCommand ms_cmdBibliothekUmbenennen = FUN_cmdCommandErstellen("ms_cmdBibliothekUmbenennen");

		public static readonly RoutedCommand ms_cmdBibliothekDuplizieren = FUN_cmdCommandErstellen("ms_cmdBibliothekDuplizieren");

		public static readonly RoutedCommand ms_cmdBibliothekLoeschen = FUN_cmdCommandErstellen("ms_cmdBibliothekLoeschen");

		public static readonly RoutedCommand ms_cmdBibliothekExportieren = FUN_cmdCommandErstellen("ms_cmdBibliothekExportieren");

		public static readonly RoutedCommand ms_cmdProgrammErstellen = FUN_cmdCommandErstellen("ms_cmdProgrammErstellen");

		public static readonly RoutedCommand ms_cmdAusgewaehltesProgrammLaden = FUN_cmdCommandErstellen("ms_cmdAusgewaehltesProgrammLaden");

		public static readonly RoutedCommand ms_cmdProgrammLaden = FUN_cmdCommandErstellen("ms_cmdProgrammLaden");

		public static readonly RoutedCommand ms_cmdProgrammAuswahlGeaendert = FUN_cmdCommandErstellen("ms_cmdProgrammAuswahlGeaendert");

		public static readonly RoutedCommand ms_cmdProgrammUmbenennen = FUN_cmdCommandErstellen("ms_cmdProgrammUmbenennen");

		public static readonly RoutedCommand ms_cmdProgrammVerschieben = FUN_cmdCommandErstellen("ms_cmdProgrammVerschieben");

		public static readonly RoutedCommand ms_cmdProgrammDuplizieren = FUN_cmdCommandErstellen("ms_cmdProgrammDuplizieren");

		public static readonly RoutedCommand ms_cmdProgrammLoeschen = FUN_cmdCommandErstellen("ms_cmdProgrammLoeschen");

		public static readonly RoutedCommand ms_cmdProgrammInfo = FUN_cmdCommandErstellen("ms_cmdProgrammInfo");

		public static readonly RoutedCommand ms_cmdProgrammExportieren = FUN_cmdCommandErstellen("ms_cmdProgrammExportieren");

		public static readonly RoutedCommand ms_cmdProgrammVersionenVergleichen = FUN_cmdCommandErstellen("ms_cmdProgrammVersionenVergleichen");

		public static readonly RoutedCommand ms_cmdProgrammOderBibImportieren = FUN_cmdCommandErstellen("ms_cmdProgrammOderBibImportieren");

		public static readonly RoutedCommand ms_cmdVersionLaden = FUN_cmdCommandErstellen("ms_cmdVersionLaden");

		public static readonly RoutedCommand ms_cmdVersionAnzeigen = FUN_cmdCommandErstellen("ms_cmdVersionAnzeigen");

		public static readonly RoutedCommand ms_cmdVersionBearbeiten = FUN_cmdCommandErstellen("ms_cmdVersionBearbeiten");

		public static readonly RoutedCommand ms_cmdVersionAuswahlGeaendert = FUN_cmdCommandErstellen("ms_cmdVersionAuswahlGeaendert");

		public static readonly RoutedCommand ms_cmdVersionLoeschen = FUN_cmdCommandErstellen("ms_cmdVersionLoeschen");

		private static RoutedCommand FUN_cmdCommandErstellen(string i_strName)
		{
			return new RoutedCommand(i_strName, typeof(EDC_ProgrammCommands));
		}
	}
}
