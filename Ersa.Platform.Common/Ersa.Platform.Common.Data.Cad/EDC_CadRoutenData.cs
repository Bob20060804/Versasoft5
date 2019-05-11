using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadRoutingData", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadRoutenData
	{
		public const string gC_strTabellenName = "CadRoutingData";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteSyncpositionY = "SyncPos";

		private const string mC_strSpalteRoutenId = "RoutingId";

		private const string mC_strSpalteSchrittTyp = "StepType";

		private const string mC_strSpalteModus = "Mode";

		private const string mC_strSpalteModul = "MachineModule";

		private const string mC_strSpalteModulNummer = "ModuleNumber";

		private const string mC_strSpalteWerkzeugNummer = "ToolNumber";

		private const string mC_strSpalteSyncModus = "SyncMode";

		private const string mC_strSpalteSyncId = "SyncId";

		private const string mC_strSpalteStartpositionX = "StartPointX";

		private const string mC_strSpalteStartpositionY = "StartPointY";

		private const string mC_strSpalteEndpositionX = "EndPointX";

		private const string mC_strSpalteEndpositionY = "EndPointY";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RoutingId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64RoutingId
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

		[EDC_SpaltenInformation("StepType")]
		public int PRO_i32BahnTyp
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

		[EDC_SpaltenInformation("MachineModule")]
		public int PRO_i32MaschinenModul
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ModuleNumber")]
		public int PRO_i32ModulNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ToolNumber")]
		public int PRO_i32WerkzeugNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SyncMode")]
		public int PRO_i32SynchronisationsModus
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SyncId")]
		public int PRO_i32SynchronisationsId
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

		[EDC_SpaltenInformation("SyncPos")]
		public int PRO_i32SynchronisationsPosition
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

		public ENUM_CadMaschinenModule PRO_enmMaschinenModul
		{
			get
			{
				return (ENUM_CadMaschinenModule)PRO_i32MaschinenModul;
			}
			set
			{
				PRO_i32MaschinenModul = (int)value;
			}
		}

		public ENUM_SynchronisationsPosition PRO_enmSynchronisationsPosition
		{
			get
			{
				return (ENUM_SynchronisationsPosition)PRO_i32SynchronisationsPosition;
			}
			set
			{
				PRO_i32SynchronisationsPosition = (int)value;
			}
		}

		public EDC_CadRoutenData()
		{
		}

		public EDC_CadRoutenData(string i_strWhereStatement)
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
			return string.Format("DELETE FROM {0} {1}", "CadRoutingData", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
