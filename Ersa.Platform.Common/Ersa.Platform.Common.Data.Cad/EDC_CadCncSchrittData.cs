using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadCncSteps", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadCncSchrittData
	{
		public const string gC_strTabellenName = "CadCncSteps";

		public const string gC_strSpalteVersionsId = "HistoryId";

		private const string mC_strSpalteSchrittId = "FlowStepId";

		private const string mC_strSpalteCncRang = "CncRang";

		private const string mC_strSpalteCncTyp = "CncType";

		private const string mC_strSpalteParameter1 = "Parameter1";

		private const string mC_strSpalteParameter2 = "Parameter2";

		private const string mC_strSpalteParameter3 = "Parameter3";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("FlowStepId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64SchrittId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("HistoryId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64VersionsId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CncRang", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32CncRang
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CncType")]
		public int PRO_i32CncTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter1")]
		public float PRO_sngParameter1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter2")]
		public float PRO_sngParameter2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter3")]
		public float PRO_sngParameter3
		{
			get;
			set;
		}

		public ENUM_CncSchrittTyp PRO_enmBahnTyp
		{
			get
			{
				return (ENUM_CncSchrittTyp)PRO_i32CncTyp;
			}
			set
			{
				PRO_i32CncTyp = (int)value;
			}
		}

		public EDC_CadCncSchrittData()
		{
		}

		public EDC_CadCncSchrittData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(IEnumerable<long> i_lstVersionsIds)
		{
			return string.Format("Where {0} in ({1})", "HistoryId", string.Join(",", (from i_i64VersionsId in i_lstVersionsIds
			select i_i64VersionsId.ToString()).ToArray()));
		}

		public static string FUN_strLoescheHistoryIdStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("DELETE FROM {0} {1}", "CadCncSteps", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
