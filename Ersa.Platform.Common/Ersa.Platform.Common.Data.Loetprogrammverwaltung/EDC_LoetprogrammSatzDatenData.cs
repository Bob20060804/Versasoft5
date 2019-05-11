using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramSetData", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammSatzDatenData
	{
		public const string gC_strTabellenName = "ProgramSetData";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteZeileninhalt = "RowContent";

		private const string mC_strSpalteZeilennummer = "RowNumber";

		private const string mC_strSpalteReihenfolge = "SortOrder";

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

		[EDC_SpaltenInformation("RowNumber", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32Zeilennummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SortOrder")]
		public int PRO_i32Reihenfolge
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RowContent", PRO_i32Length = 1000, PRO_blnIsRequired = true)]
		public string PRO_strZeileninhalt
		{
			get;
			set;
		}

		public EDC_LoetprogrammSatzDatenData()
		{
		}

		public EDC_LoetprogrammSatzDatenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strHistoryEintraegeLoeschenStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Delete from  {0} Where {1} = {2}", "ProgramSetData", "HistoryId", i_i64VersionsId);
		}
	}
}
