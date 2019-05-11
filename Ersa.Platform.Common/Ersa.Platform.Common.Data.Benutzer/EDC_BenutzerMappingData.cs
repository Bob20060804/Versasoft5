using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Benutzer
{
	[EDC_TabellenInformation("UserMachineMapping")]
	public class EDC_BenutzerMappingData
	{
		public const string gC_strTabellenName = "UserMachineMapping";

		public const string gC_strSpalteBenutzerId = "UserId";

		public const string gC_strSpalteMachineId = "MachineId";

		public const string gC_strSpalteRechte = "Permissions";

		public const string gC_strSpalteIstAktivNachAutoAbmeldung = "IsActiveAfterAutoLogout";

		public const string gC_strSpalteIstAktiv = "IsActive";

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

		[EDC_SpaltenInformation("MachineId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Permissions")]
		public int PRO_i32Rechte
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsActiveAfterAutoLogout")]
		public bool PRO_blnIstAktivNachAutoAbmeldung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsActive")]
		public bool PRO_blnIstAktiv
		{
			get;
			set;
		}

		public EDC_BenutzerMappingData()
		{
		}

		public EDC_BenutzerMappingData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strBenutzerUndMaschinenIdWhereStatementErstellen(long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "UserId", i_i64BenutzerId, "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strBenutzerIdSunbselectErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select {0} From {1} {2}", "UserId", "UserMachineMapping", FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId));
		}
	}
}
