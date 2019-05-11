using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("MessagesCyclic", PRO_strTablespace = "ess5_messages")]
	public class EDC_ZyklischeMeldungData
	{
		public const string mC_strTabellenName = "MessagesCyclic";

		public const string mC_strSpalteMeldungsId = "MessageId";

		public const string mC_strSpalteMeldung = "Message";

		public const string mC_strSpalteOrt1 = "Facility1";

		public const string mC_strSpalteOrt2 = "Facility2";

		public const string mC_strSpalteOrt3 = "Facility3";

		public const string mC_strSpalteEinlaufBlockiert = "InfeedBlocked";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strMeldungsId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Message", PRO_i32Length = 200, PRO_blnIsRequired = true)]
		public string PRO_strMeldung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility1", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility2", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility3", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt3
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("InfeedBlocked")]
		public bool PRO_blnEinlaufSperreAktiv
		{
			get;
			set;
		}

		public EDC_ZyklischeMeldungData()
		{
		}

		public EDC_ZyklischeMeldungData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMeldungsIdWhereStatementErstellen(string i_strMeldungsId)
		{
			return string.Format("Where {0} = '{1}'", "MessageId", i_strMeldungsId);
		}

		public static string FUN_strLoescheUngueltigeZyklischeMeldungenStatement()
		{
			return string.Format("DELETE FROM {0} WHERE({1} NOT IN(SELECT {1} FROM {2}))", "MessagesCyclic", "MessageId", "Messages");
		}

		public static string FUN_strErstelleSelectCountStatement(string i_strWhere)
		{
			return string.Format("Select Count(*) from {0} {1}", "MessagesCyclic", i_strWhere);
		}
	}
}
