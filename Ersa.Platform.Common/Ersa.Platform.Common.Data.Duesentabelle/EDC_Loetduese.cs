namespace Ersa.Platform.Common.Data.Duesentabelle
{
	public class EDC_Loetduese
	{
		public string PRO_strAggregatName
		{
			get;
			set;
		}

		public string PRO_strMaschinenNummer
		{
			get;
			set;
		}

		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		public string PRO_strName
		{
			get;
			set;
		}

		public string PRO_strBeschreibung
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

		public double PRO_dblAbmessungY
		{
			get;
			set;
		}

		public double PRO_dblAbmessungZ
		{
			get;
			set;
		}

		public double PRO_dblBezugOffsetX
		{
			get;
			set;
		}

		public double PRO_dblBezugOffsetY
		{
			get;
			set;
		}

		public EDC_Loetduese(long i_i64MaschinenId, string i_strAggregatName, string i_strMaschinenNummer)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
			PRO_strAggregatName = i_strAggregatName;
			PRO_strMaschinenNummer = i_strMaschinenNummer;
		}
	}
}
