namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public class EDC_FiducialBilddaten
	{
		public byte[] PROa_bytBilddaten
		{
			get;
			set;
		}

		public bool PRO_blnBildGeandert
		{
			get;
			set;
		}

		public double PRO_dblPositionX
		{
			get;
			set;
		}

		public double PRO_dblPositionY
		{
			get;
			set;
		}

		public int PRO_i32BelichtungszeitRelativ
		{
			get;
			set;
		}

		public double PRO_dblManuelleKorrekturX
		{
			get;
			set;
		}

		public double PRO_dblManuelleKorrekturY
		{
			get;
			set;
		}

		public double PRO_dblOffsetZZuTransport
		{
			get;
			set;
		}

		public EDC_FiducialBilddaten()
		{
			PROa_bytBilddaten = null;
			PRO_blnBildGeandert = false;
		}
	}
}
