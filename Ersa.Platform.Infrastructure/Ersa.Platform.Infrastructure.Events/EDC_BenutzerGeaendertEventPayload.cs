namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_BenutzerGeaendertEventPayload
	{
		public bool PRO_blnBenutzerverwaltungAktiv
		{
			get;
			set;
		}

		public string PRO_strAktiverBenutzer
		{
			get;
			set;
		}

		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		public string PRO_strAktiveRolle
		{
			get;
			set;
		}

		public string PRO_strAktiverBarcode
		{
			get;
			set;
		}

		public ENUM_AnmeldeStatus PRO_enmAnmeldeStatus
		{
			get;
			set;
		}
	}
}
