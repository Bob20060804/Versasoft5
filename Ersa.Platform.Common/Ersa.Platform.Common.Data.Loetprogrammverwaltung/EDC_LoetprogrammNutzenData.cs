using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramPanelParameter", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammNutzenData
	{
		public const string gC_strTabellenName = "ProgramPanelParameter";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteNutzenId = "PanelId";

		public const string gC_strSpalteNutzenCode = "PanelCode";

		public const string gC_strSpalteSm1Active = "Sm1Active";

		public const string gC_strSpalteFm1Active = "Fm1Active";

		public const string gC_strSpalteFm2Active = "Fm2Active";

		public const string gC_strSpalteLm1Active = "Lm1Active";

		public const string gC_strSpalteLm2Active = "Lm2Active";

		public const string gC_strSpalteLm3Active = "Lm3Active";

		public const string gC_strSpalteOffsetX = "OffsetX";

		public const string gC_strSpalteOffsetY = "OffsetY";

		public const string gC_strSpalteOffsetZ = "OffsetZ";

		public const string gC_strSpalteRotationDeg = "RotationDeg";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
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

		[EDC_SpaltenInformation("PanelId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64NutzenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PanelCode", PRO_i32Length = 512)]
		public string PRO_i32NutzenCode
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Sm1Active")]
		public bool PRO_blnSm1Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Fm1Active")]
		public bool PRO_blnFm1Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Fm2Active")]
		public bool PRO_blnFm2Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Lm1Active")]
		public bool PRO_blnLm1Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Lm2Active")]
		public bool PRO_blnLm2Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Lm3Active")]
		public bool PRO_blnLm3Active
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OffsetX")]
		public float PRO_sngOffsetX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OffsetY")]
		public float PRO_sngOffsetY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OffsetZ")]
		public float PRO_sngOffsetZ
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RotationDeg")]
		public float PRO_sngRotationDeg
		{
			get;
			set;
		}

		public EDC_LoetprogrammNutzenData()
		{
		}

		public EDC_LoetprogrammNutzenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strVersionsLoeschenStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Delete from {0} {1}", "ProgramPanelParameter", FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
