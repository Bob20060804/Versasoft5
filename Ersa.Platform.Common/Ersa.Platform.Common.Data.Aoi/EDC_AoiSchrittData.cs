using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Aoi
{
	[EDC_TabellenInformation("AoiStepData", PRO_strTablespace = "ess5_cad")]
	public class EDC_AoiSchrittData
	{
		public const string gC_strTabellenName = "AoiStepData";

		private const string mC_strSpalteProgrammId = "ProgramId";

		private const string mC_strSpalteStepGuid = "StepGuid";

		private const string mC_strSpaltePanelNr = "Panel";

		private const string mC_strSpalteBinariesBild = "Binaries";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProgramId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("StepGuid", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strStepGuid
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Panel", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32Panel
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Binaries")]
		public byte[] PRO_bytBinaries
		{
			get;
			set;
		}

		public EDC_AoiSchrittData()
		{
		}

		public EDC_AoiSchrittData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleProgrammIdUndStepGuidWhereStatement(long i_i64ProgrammId, string i_strStepGuid)
		{
			return string.Format("Where {0} = {1} And {2} = '{3}'", "ProgramId", i_i64ProgrammId, "StepGuid", i_strStepGuid);
		}

		public static string FUN_strHoleProgrammIdUndStepGuidUndPanelWhereStatement(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel)
		{
			return string.Format("Where {0} = {1} And {2} = '{3}' And {4} = {5}", "ProgramId", i_i64ProgrammId, "StepGuid", i_strStepGuid, "Panel", i_i32Panel);
		}

		public static string FUN_strHoleProgramIdWhereStatement(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strLoescheStepStatementErstellen(long i_i64ProgrammId, string i_strStepGuid)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiStepData", FUN_strHoleProgrammIdUndStepGuidWhereStatement(i_i64ProgrammId, i_strStepGuid));
		}

		public static string FUN_strLoescheStepStatementErstellen(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiStepData", FUN_strHoleProgrammIdUndStepGuidUndPanelWhereStatement(i_i64ProgrammId, i_strStepGuid, i_i32Panel));
		}

		public static string FUN_strLoescheProgramIdStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiStepData", FUN_strHoleProgramIdWhereStatement(i_i64ProgrammId));
		}
	}
}
