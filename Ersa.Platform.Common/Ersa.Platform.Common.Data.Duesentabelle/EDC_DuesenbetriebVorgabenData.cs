using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("NozzleOperatingTargetValues", PRO_strTablespace = "ess5_production")]
	public class EDC_DuesenbetriebVorgabenData
	{
		public const string gC_strTabellenName = "NozzleOperatingTargetValues";

		private const string mC_strSpalteGeometrieId = "GeomertyId";

		private const string mC_strEinschaltDauer = "WaveOnTargetTime";

		private const string mC_strAussschaltDauer = "WaveOffTargetTime";

		private const string mC_strGesamtDauer = "TotalTargetTime";

		private const string mC_strAktivierungsanzahl = "DispenceTargetNumber";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GeomertyId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64GeometrieId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("WaveOnTargetTime")]
		public long PRO_i64WelleEinSollzeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("WaveOffTargetTime")]
		public long PRO_i64WelleAusSollzeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TotalTargetTime")]
		public long PRO_i64GesamtSollzeit
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DispenceTargetNumber")]
		public long PRO_i64AnzahlSollaktivierungen
		{
			get;
			set;
		}

		public EDC_DuesenbetriebVorgabenData()
		{
		}

		public EDC_DuesenbetriebVorgabenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleDuesengeometrieIdWhereStatement(long i_i64GeometrieId)
		{
			return string.Format("Where {0} = {1}", "GeomertyId", i_i64GeometrieId);
		}
	}
}
