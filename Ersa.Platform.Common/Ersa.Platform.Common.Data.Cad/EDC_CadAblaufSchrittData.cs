using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadFlowSteps", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadAblaufSchrittData
	{
		public const string gC_strTabellenName = "CadFlowSteps";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteModus = "Mode";

		private const string mC_strSpalteSchrittId = "FlowStepId";

		private const string mC_strSpalteStartpositionX = "StartPointX";

		private const string mC_strSpalteStartpositionY = "StartPointY";

		private const string mC_strSpalteEndpositionX = "EndPointX";

		private const string mC_strSpalteEndpositionY = "EndPointY";

		private const string mC_strSpalteSchrittTyp = "StepType";

		private const string mC_strSpalteGeometrieId = "GeomertyId";

		private const string mC_strSpalteRang = "Rank";

		private const string mC_strSpalteGruppenId = "MotionGroupId";

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

		[EDC_SpaltenInformation("StartPointX")]
		public float PRO_sngStartpunktX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("StartPointY")]
		public float PRO_sngStartpunktY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EndPointX")]
		public float PRO_sngEndpunktX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EndPointY")]
		public float PRO_sngEndpunktY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("StepType")]
		public int PRO_i32BahnTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GeomertyId")]
		public long PRO_i64GeometrieId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Rank")]
		public int PRO_i32Rang
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MotionGroupId")]
		public int PRO_i32GruppenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Mode")]
		public int PRO_i32SchrittModus
		{
			get;
			set;
		}

		public ENUM_BahnTyp PRO_enmBahnTyp
		{
			get
			{
				return (ENUM_BahnTyp)PRO_i32BahnTyp;
			}
			set
			{
				PRO_i32BahnTyp = (int)value;
			}
		}

		public ENUM_SchrittModus PRO_enmSchrittModus
		{
			get
			{
				return (ENUM_SchrittModus)PRO_i32SchrittModus;
			}
			set
			{
				PRO_i32SchrittModus = (int)value;
			}
		}

		public EDC_CadAblaufSchrittData()
		{
		}

		public EDC_CadAblaufSchrittData(string i_strWhereStatement)
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
			return string.Format("DELETE FROM {0} {1}", "CadFlowSteps", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
