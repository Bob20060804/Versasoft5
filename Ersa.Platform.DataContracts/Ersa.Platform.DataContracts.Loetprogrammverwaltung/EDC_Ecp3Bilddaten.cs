namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public class EDC_Ecp3Bilddaten
	{
		public string PRO_strEcp3BildDateiName
		{
			get;
			set;
		}

		public byte[] PRO_bytEcp3BildDaten
		{
			get;
			set;
		}

		public bool PRO_blnEcp3BildWurdeGeaendert
		{
			get;
			set;
		}

		public EDC_Ecp3Bilddaten()
		{
			PRO_bytEcp3BildDaten = null;
			PRO_strEcp3BildDateiName = string.Empty;
			PRO_blnEcp3BildWurdeGeaendert = false;
		}
	}
}
