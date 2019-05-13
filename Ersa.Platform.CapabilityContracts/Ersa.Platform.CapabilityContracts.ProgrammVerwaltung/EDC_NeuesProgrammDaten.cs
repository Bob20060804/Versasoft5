namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public class EDC_NeuesProgrammDaten
	{
		public string PRO_strBibliothekName
		{
			get;
		}

		public string PRO_strProgrammName
		{
			get;
		}

		public EDC_NeuesProgrammDaten(string i_strBibliothekName, string i_strProgrammName)
		{
			PRO_strBibliothekName = i_strBibliothekName;
			PRO_strProgrammName = i_strProgrammName;
		}
	}
}
