using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadMotionGroups", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadBewegungsGruppenData
	{
		public const string gC_strTabellenName = "CadMotionGroups";

		public const string gC_strSpalteVersionsId = "HistoryId";

		private const string mC_strSpalteGruppenId = "MotionGroupId";

		private const string mC_strSpalteParentId = "ParentGroupId";

		private const string mC_strSpalteRang = "Rank";

		private const string mC_strSpalteSonderwerkzeug = "Equipment";

		private const string mC_strSpalteModul = "MachineModule";

		private const string mC_strSpalteModulNumber = "ModuleNumber";

		private const string mC_strSpalteWerkzeug = "ToolNumber";

		private const string mC_strSpalteAutorouting = "AllowAutorouting";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MotionGroupId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32GruppenId
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

		[EDC_SpaltenInformation("ParentGroupId")]
		public int PRO_i32ParentGruppenId
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

		[EDC_SpaltenInformation("Equipment")]
		public int PRO_i32Sonderwerkzeug
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

		[EDC_SpaltenInformation("AllowAutorouting")]
		public bool PRO_blnAutoroutingErlaubt
		{
			get;
			set;
		}

		public ENUM_SonderWerkzeug PRO_enmSonderwerkzeug
		{
			get
			{
				return (ENUM_SonderWerkzeug)PRO_i32Sonderwerkzeug;
			}
			set
			{
				PRO_i32Sonderwerkzeug = (int)value;
			}
		}

		public EDC_CadBewegungsGruppenData()
		{
		}

		public EDC_CadBewegungsGruppenData(string i_strWhereStatement)
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
			return string.Format("DELETE FROM {0} {1}", "CadMotionGroups", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
