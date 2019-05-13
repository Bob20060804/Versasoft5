namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public class EDC_ProgrammImportOptionen
	{
		public bool PRO_blnPrgAuswahlAlsVerzeichnis
		{
			get;
			private set;
		}

		public string PRO_strPrgDateiFilter
		{
			get;
			private set;
		}

		private EDC_ProgrammImportOptionen(bool i_blnPrgAuswahlAlsVerzeichnis, string i_strPrgDateiFilter)
		{
			PRO_blnPrgAuswahlAlsVerzeichnis = i_blnPrgAuswahlAlsVerzeichnis;
			PRO_strPrgDateiFilter = i_strPrgDateiFilter;
		}

		public static EDC_ProgrammImportOptionen FUNs_edcAlsVerzeichnisAuswahl()
		{
			return new EDC_ProgrammImportOptionen(i_blnPrgAuswahlAlsVerzeichnis: true, null);
		}

		public static EDC_ProgrammImportOptionen FUNs_edcAlsDateiAuswahl(string i_strDateiFilter)
		{
			return new EDC_ProgrammImportOptionen(i_blnPrgAuswahlAlsVerzeichnis: false, i_strDateiFilter);
		}
	}
}
