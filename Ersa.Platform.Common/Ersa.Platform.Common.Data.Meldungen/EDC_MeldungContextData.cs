using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("MessageContext", PRO_strTablespace = "ess5_messages")]
	public class EDC_MeldungContextData
	{
		public const string mC_strTabellenName = "MessageContext";

		public const string mC_strSpalteMeldungsId = "MessageId";

		public const string mC_strSpalteDetails = "Details";

		public const string mC_strSpalteContext = "Context";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MessageId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 36)]
		public string PRO_strMeldungGuid
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Details")]
		public string PRO_strDetails
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Context")]
		public string PRO_strContext
		{
			get;
			set;
		}

		public EDC_MeldungContextData()
		{
		}

		public EDC_MeldungContextData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMeldungsIdWhereStatementErstellen(string i_strMeldungsId)
		{
			return string.Format("Where {0} = '{1}'", "MessageId", i_strMeldungsId);
		}
	}
}
