using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Codetabelle
{
	[EDC_TabellenInformation("CodeTableMembers", PRO_strTablespace = "ess5_programs")]
	public class EDC_CodetabelleneintragData
	{
		public const string gC_strTabellenName = "CodeTableMembers";

		private const string mC_strSpalteEintragId = "CodeMemberId";

		private const string mC_strSpalteCodetabellenId = "CodeTableId";

		private const string mC_strSpalteBibliotheksId = "LibraryId";

		private const string mC_strSpalteProgrammId = "ProgramId";

		private const string mC_strSpalteCode = "Code";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeMemberId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64EintragsId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeTableId")]
		public long PRO_i64CodetabellenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LibraryId")]
		public long PRO_i64BibliotheksId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProgramId")]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Code", PRO_i32Length = 500)]
		public string PRO_strCode
		{
			get;
			set;
		}

		public EDC_CodetabelleneintragData()
		{
		}

		public EDC_CodetabelleneintragData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strCodetabellenIdWhereStatementErstellen(long i_i64CodetabellenId)
		{
			return string.Format("Where {0} = {1}", "CodeTableId", i_i64CodetabellenId);
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} in (select {1} from {2} where {3} in (select {4} from {5} where {6} = {7}))", "CodeTableId", "CodeTableId", "CodeTables", "GroupId", "GroupId", "MachineGroupMembers", "MachineId", i_i64MaschinenId);
		}
	}
}
