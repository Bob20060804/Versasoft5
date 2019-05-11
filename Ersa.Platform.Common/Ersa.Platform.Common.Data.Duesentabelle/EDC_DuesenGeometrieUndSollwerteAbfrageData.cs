using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_DuesenGeometrieUndSollwerteAbfrageData : EDC_DuesenGeometrienData
	{
		private const string mC_strSpalteGeomertyIdInSollwerte = "GeomertyIdInSollwerte";

		private const string mC_strSpalteMaxBetriebszeit = "MaxOperatingTime";

		public string PRO_strAbfrageStatement => string.Format("SELECT {0}.*, ", "NozzleGeometries") + string.Format("{0}.{1} AS  {2},", "NozzleSetValues", "GeomertyId", "GeomertyIdInSollwerte") + string.Format("case when {0}.{1} is null Then '0' else  {2}.{3} end AS {4}", "NozzleSetValues", "MaxOperatingTime", "NozzleSetValues", "MaxOperatingTime", "MaxOperatingTime") + string.Format(" FROM {0} ", "NozzleGeometries") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "NozzleSetValues", "NozzleSetValues", "GeomertyId", "NozzleGeometries", "GeomertyId");

		[EDC_SpaltenInformation("GeomertyIdInSollwerte")]
		public long PRO_i64GeomertyIdInSollwerte
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MaxOperatingTime")]
		public long PRO_i64MaxBetriebszeit
		{
			get;
			set;
		}

		public EDC_DuesenGeometrieUndSollwerteAbfrageData()
		{
		}

		public EDC_DuesenGeometrieUndSollwerteAbfrageData(string i_strWhereStatement)
			: base(i_strWhereStatement)
		{
		}

		public static string FUN_strDuesenGeometrienIdAbfrageWhereStaement(long i_i64GeometrieId)
		{
			return string.Format(" Where {0}.{1} = {2}", "NozzleGeometries", "GeomertyId", i_i64GeometrieId);
		}

		public static string FUN_strDuesenGeometrienIdSubselectWhereStaement(string i_strSubselect)
		{
			return string.Format(" Where {0}.{1} in ({2})", "NozzleGeometries", "GeomertyId", i_strSubselect);
		}
	}
}
