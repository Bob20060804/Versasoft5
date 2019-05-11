using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("NozzleSetValues", PRO_strTablespace = "ess5_programs")]
	public class EDC_DuesenSollwerteData
	{
		public const string gC_strTabellenName = "NozzleSetValues";

		public const string gC_strSpalteGeometrieId = "GeomertyId";

		public const string gC_strSpalteMaxBetriebszeit = "MaxOperatingTime";

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

		[EDC_SpaltenInformation("MaxOperatingTime", PRO_blnIsRequired = true)]
		public long PRO_i64MaxBetriebszeit
		{
			get;
			set;
		}

		public EDC_DuesenSollwerteData()
		{
		}

		public EDC_DuesenSollwerteData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleGeometrieIdWhereStatement(long i_i64GeometrieId)
		{
			return string.Format("Where {0} = {1}", "GeomertyId", i_i64GeometrieId);
		}
	}
}
