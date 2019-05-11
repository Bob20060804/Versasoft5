using System;

namespace Ersa.Platform.Common.Produktionssteuerung
{
	public class EDC_Produktionssteuerungsdaten
	{
		public long PRO_i64ProduktionssteuerungId
		{
			get;
			set;
		}

		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		public string PRO_strBeschreibung
		{
			get;
			set;
		}

		public EDC_ProduktionsEinstellungen PRO_edcProduktionsEinstellungen
		{
			get;
			set;
		}

		public bool PRO_blnIstAktiv
		{
			get;
			set;
		}

		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		public long PRO_i64AngelegtVon
		{
			get;
			set;
		}

		public DateTime PRO_dtmBearbeitetAm
		{
			get;
			set;
		}

		public long PRO_i64BearbeitetVon
		{
			get;
			set;
		}
	}
}
