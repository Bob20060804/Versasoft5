namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces
{
	public interface INF_BenutzerInfoProvider
	{
		string PRO_strAktiverBenutzer
		{
			get;
		}

		string PRO_strAktiveRolle
		{
			get;
		}

		long PRO_i64BenutzerId
		{
			get;
		}
	}
}
