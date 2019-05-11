using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("MaschineGroups", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenGruppeData
	{
		public const string gC_strTabellenName = "MaschineGroups";

		public const string gC_strSpalteGruppenId = "GroupId";

		public const string gC_strSpalteMaschinenTyp = "MachineType";

		private const string mC_strSpalteGruppenName = "GroupName";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GroupId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64GruppenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GroupName", PRO_i32Length = 50)]
		public string PRO_strGruppenName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineType", PRO_i32Length = 40)]
		public string PRO_strMaschinenTyp
		{
			get;
			set;
		}

		public EDC_MaschinenGruppeData()
		{
		}

		public EDC_MaschinenGruppeData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strGruppenIdWhereStatementErstellen(long i_i64GruppenId)
		{
			return string.Format("Where {0} = {1}", "GroupId", i_i64GruppenId);
		}

		public static string FUN_strDefaultGruppenWhereStatementErstellen(string i_strMaschinenTyp)
		{
			return string.Format("Where {0} = '{1}'", "GroupName", FUN_strHoleDefaultGruppenName(i_strMaschinenTyp));
		}

		public static string FUN_strMaschinenTypeWhereStatmentErstellen(string i_strMaschinenTyp)
		{
			return string.Format("Where {0} = '{1}'", "MachineType", i_strMaschinenTyp);
		}

		public static string FUN_strGruppenIdUndMaschinenTypeWhereStatmentErstellen(long i_i64GruppenId, string i_strMaschinenTyp)
		{
			return string.Format("Where {0} = {1} And {2} = '{3}'", "GroupId", i_i64GruppenId, "MachineType", i_strMaschinenTyp);
		}

		public static string FUN_strHoleDefaultGruppenName(string i_strMaschinenTyp)
		{
			return $"Default-{i_strMaschinenTyp}";
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} in (select {1} from {2} where {3} = {4})", "GroupId", "GroupId", "MachineGroupMembers", "MachineId", i_i64MaschinenId);
		}
	}
}
