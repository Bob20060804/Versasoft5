namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces
{
	public interface INF_AutorisierungsDienst
	{
		bool FUN_blnIstBenutzerAutorisiert(string i_strRecht);

		bool FUN_blnBenutzerUeberFehlendeRechteInformieren();

		void SUB_RechteInformationZuruecksetzen();
	}
}
