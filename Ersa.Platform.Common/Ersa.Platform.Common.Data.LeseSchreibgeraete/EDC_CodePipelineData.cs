using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.LeseSchreibGeraete;

namespace Ersa.Platform.Common.Data.LeseSchreibgeraete
{
	[EDC_TabellenInformation("CodePipelines", PRO_strTablespace = "ess5_production")]
	public class EDC_CodePipelineData
	{
		public const string gC_strTabellenName = "CodePipelines";

		private const string mC_strSpalteArrayIndex = "ArrayIndex";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteZweig = "BranchNr";

		private const string mC_strSpalteElement = "PipeElement";

		private const string mC_strSpalteInhalt = "Content";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ArrayIndex", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ArrayIndex
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BranchNr", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64Zweig
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PipeElement", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64PipelineElement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Content")]
		public string PRO_strInhalt
		{
			get;
			set;
		}

		public ENUM_PipelineElement PRO_enmPipelineElement
		{
			get
			{
				return (ENUM_PipelineElement)PRO_i64PipelineElement;
			}
			set
			{
				PRO_i64PipelineElement = (long)value;
			}
		}

		public EDC_CodePipelineData()
		{
		}

		public EDC_CodePipelineData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex);
		}

		public static string FUN_strWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = {5}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex, "BranchNr", i_i64Zweig);
		}

		public static string FUN_strWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Element, long i_i64Zweig)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = {5} and {6} = {7}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex, "PipeElement", i_i64Element, "BranchNr", i_i64Zweig);
		}

		public static string FUN_strPipelineSelectCountStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodePipelines", FUN_strWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex, i_i64Zweig));
		}
	}
}
