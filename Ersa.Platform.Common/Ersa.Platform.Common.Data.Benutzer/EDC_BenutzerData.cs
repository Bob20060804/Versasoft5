using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Benutzer
{
	[EDC_TabellenInformation("Users")]
	public class EDC_BenutzerData
	{
		public const string gC_strTabellenName = "Users";

		public const string gC_strSpalteIstDefaultBenutzer = "IsDefault";

		public const string gC_strSpalteBenutzerId = "UserId";

		public const string gC_strSpalteBenutzername = "Username";

		public const string gC_strSpaltePasswortSalt = "PasswordSalt";

		public const string gC_strSpaltePasswortHash = "PasswordHash";

		public const string gC_strSpalteAngelegtAm = "CreatedAt";

		public const string gC_strSpalteExterneAuthentifizierung = "IsExtern";

		public const string gC_strSpalteBarcode = "code";

		public const string gC_strSpalteIstAktiv = "IsActive";

		public const string gC_strSpalteMachineId = "MachineId";

		public const string gC_strSpalteIstAktivNachAutoAbmeldung = "IsActiveAfterAutoLogout";

		private const string mC_strSpalteRechte = "Permissions";

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

		[EDC_SpaltenInformation("Username", PRO_i32Length = 50, PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PasswordSalt", PRO_i32Length = 20, PRO_blnIsRequired = true)]
		public string PRO_strPasswortSalt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PasswordHash", PRO_i32Length = 28, PRO_blnIsRequired = true)]
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

		[EDC_SpaltenInformation("code", PRO_i32Length = 200)]
		public string PRO_strBarcode
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

		[EDC_SpaltenInformation("IsActiveAfterAutoLogout")]
		public bool PRO_blnIstAktivNachAutoAbmeldung
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

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		public EDC_BenutzerData()
		{
		}

		public EDC_BenutzerData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBenutzerIdSunbselectErstellen(string i_strSubselect)
		{
			return string.Format("Where {0} in ({1})", "UserId", i_strSubselect);
		}

		public static string FUN_strBenutzerNamenWhereStatement(string i_strBenutzername)
		{
			return string.Format("Where {0} = '{1}'", "Username", i_strBenutzername);
		}

		public static string FUN_strBenutzerIdWhereStatementErstellen(long i_i64BenutzerId)
		{
			return string.Format("Where {0} = {1}", "UserId", i_i64BenutzerId);
		}
	}
}
