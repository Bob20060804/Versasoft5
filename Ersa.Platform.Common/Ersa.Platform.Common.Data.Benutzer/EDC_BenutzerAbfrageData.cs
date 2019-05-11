using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Benutzer
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_BenutzerAbfrageData
	{
		private const string mC_strSpalteBenutzerId = "UserId";

		private const string mC_strSpalteBenutzername = "Username";

		private const string mC_strSpaltePasswortSalt = "PasswordSalt";

		private const string mC_strSpaltePasswortHash = "PasswordHash";

		private const string mC_strSpalteAngelegtAm = "CreatedAt";

		private const string mC_strSpalteIstDefaultBenutzer = "IsDefault";

		private const string mC_strSpalteExternAuthentifizierung = "IsExtern";

		private const string mC_strSpalteBarcode = "code";

		private const string mC_strSpalteMachineId = "MachineId";

		private const string mC_strSpalteRechte = "Permissions";

		private const string mC_strSpalteIstAktivNachAutoAbmeldung = "IsActiveAfterAutoLogout";

		private const string mC_strSpalteIstAktiv = "IsActive";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		public string PRO_strAbfrageStatement => string.Format("SELECT {0}.{1} AS {2}, ", "Users", "UserId", "UserId") + string.Format(" {0}.{1} AS {2}, ", "Users", "Username", "Username") + string.Format(" {0}.{1} AS {2}, ", "Users", "PasswordSalt", "PasswordSalt") + string.Format(" {0}.{1} AS {2}, ", "Users", "PasswordHash", "PasswordHash") + string.Format(" {0}.{1} AS {2}, ", "Users", "CreatedAt", "CreatedAt") + string.Format(" {0}.{1} AS {2}, ", "Users", "IsDefault", "IsDefault") + string.Format(" {0}.{1} AS {2}, ", "Users", "IsExtern", "IsExtern") + string.Format(" {0}.{1} AS {2}, ", "Users", "code", "code") + string.Format(" {0}.{1} AS {2}, ", "UserMachineMapping", "MachineId", "MachineId") + string.Format(" {0}.{1} AS {2}, ", "UserMachineMapping", "Permissions", "Permissions") + string.Format(" {0}.{1} AS {2}, ", "UserMachineMapping", "IsActiveAfterAutoLogout", "IsActiveAfterAutoLogout") + string.Format(" {0}.{1} AS {2} ", "UserMachineMapping", "IsActive", "IsActive") + string.Format(" FROM {0} ", "Users") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "UserMachineMapping", "Users", "UserId", "UserMachineMapping", "UserId");

		[EDC_SpaltenInformation("UserId")]
		public long PRO_i64BenutzerId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId")]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Username")]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PasswordSalt")]
		public string PRO_strPasswortSalt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PasswordHash")]
		public string PRO_strPasswortHash
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreatedAt")]
		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsDefault")]
		public bool PRO_blnIstDefaultBenutzer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsExtern")]
		public bool PRO_blnIstExternerBenutzer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("code")]
		public string PRO_strBarcode
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

		public EDC_BenutzerAbfrageData()
		{
			PRO_blnIstAktiv = true;
		}

		public EDC_BenutzerAbfrageData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strDefaultMaschinenBenutzerWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = '1' AND {2}.{3} = {4}", "Users", "IsDefault", "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strNichtDefaultMaschinenBenutzerWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = '0' AND {2}.{3} = {4}", "Users", "IsDefault", "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenBenutzerAktivWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = '1' AND {2}.{3} = '0' AND {4}.{5} = {6}", "UserMachineMapping", "IsActive", "Users", "IsDefault", "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenBenutzerMitNamenWhereStatementErstellen(string i_strBenutzerName, long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = '{2}' And {3}.{4} = {5}", "Users", "Username", i_strBenutzerName, "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenBenutzerMitCodeWhereStatementErstellen(string i_strCode, long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = '{2}' And {3}.{4} = {5}", "Users", "code", i_strCode, "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenBenutzerMitIdWhereStatementErstellen(long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return string.Format("Where {0}.{1} = {2} And {3}.{4} = {5}", "Users", "UserId", i_i64BenutzerId, "UserMachineMapping", "MachineId", i_i64MaschinenId);
		}
	}
}
