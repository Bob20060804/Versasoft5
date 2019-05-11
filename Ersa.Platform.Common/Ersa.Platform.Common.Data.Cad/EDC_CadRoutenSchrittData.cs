using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadRoutingSteps", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadRoutenSchrittData : EDC_CadCncSchrittData
	{
		public new const string gC_strTabellenName = "CadRoutingSteps";

		public EDC_CadRoutenSchrittData()
		{
		}

		public EDC_CadRoutenSchrittData(string i_strWhereStatement)
			: base(i_strWhereStatement)
		{
		}

		public new static string FUN_strLoescheHistoryIdStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("DELETE FROM {0} {1}", "CadRoutingSteps", EDC_CadCncSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
