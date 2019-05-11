using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Benutzer
{
	[EDC_TabellenInformation("ActiveUsers")]
	public class EDC_AktiverBenutzerData
	{
		public const string gC_strTabellenName = "ActiveUsers";

		public const string gC_strSpalteBenutzerId = "UserId";

		public const string gC_strSpalteMachineId = "MachineId";

		private const string mC_strSpalteLoginZeitpunkt = "LoginTime";

		private const string mC_strSpalteIp = "IP";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UserId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LoginTime", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public DateTime PRO_dtmLoginZeitpunkt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IP", PRO_i32Length = 40, PRO_blnIsRequired = true)]
		public string PRO_strIp
		{
			get;
			set;
		}

		public EDC_AktiverBenutzerData()
		{
		}

		public EDC_AktiverBenutzerData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBenutzerIdWhereStatementErstellen(long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} AND {2} = {3}", "UserId", i_i64BenutzerId, "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strSelectCountFuerAnwenderUndMaschineStatementErstellen(long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return string.Format("Select Count(*) From {0} {1}", "ActiveUsers", FUN_strBenutzerIdWhereStatementErstellen(i_i64BenutzerId, i_i64MaschinenId));
		}
	}
}
