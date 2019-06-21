using System.Windows.Input;

namespace Ersa.AllgemeineEinstellungen
{
	public static class EDC_RoutedCommands
	{
		public static readonly RoutedCommand ms_cmdMaschinenGruppeUmbenennen = FUN_cmdCommandErstellen("ms_cmdMaschinenGruppeUmbenennen");

		public static readonly RoutedCommand ms_cmdMaschinenGruppeAktivSetzen = FUN_cmdCommandErstellen("ms_cmdMaschinenGruppeAktivSetzen");

		public static readonly RoutedCommand ms_cmdFlussmittelHinzufuegen = FUN_cmdCommandErstellen("ms_cmdFlussmittelHinzufuegen");

		public static readonly RoutedCommand ms_cmdFlussmittelLoeschen = FUN_cmdCommandErstellen("ms_cmdFlussmittelLoeschen");

		public static readonly RoutedCommand ms_cmdFlussmittelAendern = FUN_cmdCommandErstellen("ms_cmdFlussmittelAendern");

		public static readonly RoutedCommand ms_cmdNiederhaltergruppeHinzufuegen = FUN_cmdCommandErstellen("ms_cmdNiederhaltergruppeHinzufuegen");

		public static readonly RoutedCommand ms_cmdNiederhaltergruppeLoeschen = FUN_cmdCommandErstellen("ms_cmdNiederhaltergruppeLoeschen");

		public static readonly RoutedCommand ms_cmdNiederhaltergruppeUmbenennen = FUN_cmdCommandErstellen("ms_cmdNiederhaltergruppeUmbenennen");

		public static readonly RoutedCommand ms_cmdNiederhalterHinzufuegen = FUN_cmdCommandErstellen("ms_cmdNiederhalterHinzufuegen");

		public static readonly RoutedCommand ms_cmdNiederhalterLoeschen = FUN_cmdCommandErstellen("ms_cmdNiederhalterLoeschen");

		public static readonly RoutedCommand ms_cmdNiederhalterBearbeiten = FUN_cmdCommandErstellen("ms_cmdNiederhalterBearbeiten");

		private static RoutedCommand FUN_cmdCommandErstellen(string i_strName)
		{
			return new RoutedCommand(i_strName, typeof(EDC_RoutedCommands));
		}
	}
}
