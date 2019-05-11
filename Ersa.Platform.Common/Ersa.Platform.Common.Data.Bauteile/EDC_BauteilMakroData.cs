using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Bauteile
{
	[EDC_TabellenInformation("PackageMacros", PRO_strTablespace = "ess5_components")]
	public class EDC_BauteilMakroData
	{
		public const string gC_strTabellenName = "PackageMacros";

		public const string gC_strSpalteBauteilId = "PackageId";

		public const string gC_strSpalteGeometrieId = "GeomertyId";

		public const string gC_strSpalteKategory = "Category";

		public const string gC_strSpalteMakro = "Macro";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PackageId", PRO_i32Length = 66, PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public string PRO_strBauteilId
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

		[EDC_SpaltenInformation("Category", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64Kategorie
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Macro")]
		public string PRO_strMakro
		{
			get;
			set;
		}

		public EDC_BauteilMakroData()
		{
		}

		public EDC_BauteilMakroData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBauteilIdWhereStatementErstellen(string i_strBauteilId)
		{
			return string.Format("Where {0} = '{1}'", "PackageId", i_strBauteilId);
		}

		public static string FUN_strBauteilIdUndGeometrieIdWhereStatementErstellen(string i_strBauteilId, long i_i64GeometrieId)
		{
			return string.Format("Where {0} = '{1}' and {2} = {3}", "PackageId", i_strBauteilId, "GeomertyId", i_i64GeometrieId);
		}

		public static string FUN_strBauteilIdsWhereStatementErstellen(IEnumerable<string> i_strBauteilIds)
		{
			return string.Format("Where {0} in ({1})", "PackageId", string.Join<string>(",", (IEnumerable<string>)(from i_strVariable in i_strBauteilIds
			select $"'{i_strVariable}'").ToList()));
		}

		public static string FUN_strBauteilWhereStatementErstellen(EDC_BauteilMakroData i_edcMakro)
		{
			return string.Format("Where {0} = '{1}' and {2} = {3} and {4} = {5}", "PackageId", i_edcMakro.PRO_strBauteilId, "GeomertyId", i_edcMakro.PRO_i64GeometrieId, "Category", i_edcMakro.PRO_i64Kategorie);
		}
	}
}
