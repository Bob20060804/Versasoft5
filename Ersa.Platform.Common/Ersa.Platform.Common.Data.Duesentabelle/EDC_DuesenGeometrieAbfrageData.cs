using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("Select NozzleGeometries.* from NozzleGeometries inner join Nozzles on NozzleGeometries.GeomertyId = Nozzles.GeomertyId", true)]
	public class EDC_DuesenGeometrieAbfrageData : EDC_DuesenGeometrienData
	{
		public const string gC_strQuery = "Select NozzleGeometries.* from NozzleGeometries inner join Nozzles on NozzleGeometries.GeomertyId = Nozzles.GeomertyId";

		public EDC_DuesenGeometrieAbfrageData()
		{
		}

		public EDC_DuesenGeometrieAbfrageData(string i_strWhereStatement)
			: this()
		{
			base.PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strDuesenGeometrienAusDuesenIdAbfrageWhereStaement(long i_i64DuesenId)
		{
			return string.Format(" Where {0}.{1} = {2}", "Nozzles", "NozzleId", i_i64DuesenId);
		}
	}
}
