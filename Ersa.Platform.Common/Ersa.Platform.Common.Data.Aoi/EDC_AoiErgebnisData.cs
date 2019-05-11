using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Aoi
{
	[EDC_TabellenInformation("AoiResults", PRO_strTablespace = "ess5_cad")]
	public class EDC_AoiErgebnisData
	{
		public const string gC_strTabellenName = "AoiResults";

		private const string mC_strSpalteResultId = "ResultId";

		private const string mC_strSpalteAoiTyp = "AoiType";

		private const string mC_strSpaltePanelCode = "PanelCode";

		private const string mC_strSpalteProgrammId = "ProgramId";

		private const string mC_strSpalteStepGuid = "StepGuid";

		private const string mC_strSpaltePanelNr = "Panel";

		private const string mC_strSpalteHash = "Hash";

		private const string mC_strSpalteErgebnis = "Result";

		private const string mC_strSpalteManuell = "Manual";

		private const string mC_strSpalteDaten = "Data";

		private const string mC_strSpalteBinaries = "Binaries";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ResultId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ResultId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("AoiType", PRO_blnIsRequired = true)]
		public int PRO_i32AoiTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PanelCode", PRO_blnIsRequired = true)]
		public string PRO_strPanelCode
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("StepGuid", PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strStepGuid
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProgramId", PRO_blnIsRequired = true)]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Panel", PRO_blnIsRequired = true)]
		public int PRO_i32Panel
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Hash", PRO_i32Length = 32)]
		public string PRO_strHash
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Result")]
		public int PRO_i32Ergebnis
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Manual")]
		public bool PRO_blnManuell
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Data")]
		public string PRO_strDaten
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate", PRO_blnIsRequired = true)]
		public DateTime PRO_dtmAngelegtAm
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

		public EDC_AoiErgebnisData()
		{
		}

		public EDC_AoiErgebnisData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleResultIdWhereStatement(long i_i64ResultId)
		{
			return string.Format("Where {0} = {1}", "ResultId", i_i64ResultId);
		}

		public static string FUN_strHoleProgramIdWhereStatement(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strHoleProgramIdUndStepGuidWhereStatement(long i_i64ProgrammId, string i_strStepGuid)
		{
			return string.Format("Where {0} = {1} And {2} = '{3}'", "ProgramId", i_i64ProgrammId, "StepGuid", i_strStepGuid);
		}

		public static string FUN_strHoleProgramidStepGuidUndPanelWhereStatement(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel)
		{
			return string.Format("Where {0} = {1} And {2} = '{3}' And {4} = {5}", "ProgramId", i_i64ProgrammId, "StepGuid", i_strStepGuid, "Panel", i_i32Panel);
		}

		public static string FUN_strHoleHashWhereStatement(string i_strHash)
		{
			return string.Format("Where {0} = '{1}'", "Hash", i_strHash);
		}

		public static string FUN_strHoleHashWhereStatement(string i_strHash, int i_i32AoiType)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}'", "Hash", i_strHash, "AoiType", i_i32AoiType);
		}

		public static string FUN_strLoescheResultIdStatementErstellen(long i_i64ResultId)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiResults", FUN_strHoleResultIdWhereStatement(i_i64ResultId));
		}

		public static string FUN_strLoescheProgramIdStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiResults", FUN_strHoleProgramIdWhereStatement(i_i64ProgrammId));
		}

		public static string FUN_strLoescheHashStatementErstellen(string i_strHash)
		{
			return string.Format("DELETE FROM {0} {1}", "AoiResults", FUN_strHoleHashWhereStatement(i_strHash));
		}
	}
}
