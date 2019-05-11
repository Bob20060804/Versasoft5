using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramHistory", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammVersionData
	{
		public const string gC_strTabellenName = "ProgramHistory";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteProgrammId = "ProgramId";

		public const string gC_strSpalteAngelegtAm = "CreationDate";

		public const string gC_strSpalteAngelegtVon = "CreationUser";

		public const string gC_strSpalteStatus = "Status";

		public const string gC_strSpalteSetNummer = "SetNumber";

		public const string gC_strSpalteBearbeitetAm = "ChangeDate";

		public const string gC_strSpalteBearbeitetVon = "ChangeUser";

		public const string gC_strFreigabeStatus = "ReleaseState";

		public const string gC_strFreigabeNotizen = "ReleaseNotes";

		private const string mC_strSpalteNotizen = "Notes";

		public const string gC_strSpalteVersion = "Version";

		public const string gC_strSpalteUnsichtbar = "IsHidden";

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

		[EDC_SpaltenInformation("ProgramId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Notes")]
		public string PRO_strKommentar
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationUser")]
		public long PRO_i64AngelegtVon
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Status")]
		public int PRO_i32ProgrammStatus
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SetNumber")]
		public int PRO_i32SetNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeDate")]
		public DateTime PRO_dtmBearbeitetAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeUser")]
		public long PRO_i64BearbeitetVon
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ReleaseState")]
		public int PRO_i32FreigabeStatus
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ReleaseNotes")]
		public string PRO_strFreigabeNotizen
		{
			get;
			set;
		}

		public ENUM_LoetprogrammStatus PRO_enmProgrammStatus
		{
			get
			{
				return (ENUM_LoetprogrammStatus)PRO_i32ProgrammStatus;
			}
			set
			{
				PRO_i32ProgrammStatus = (int)value;
			}
		}

		public ENUM_LoetprogrammFreigabeStatus PRO_enmFreigabeStatus
		{
			get
			{
				return (ENUM_LoetprogrammFreigabeStatus)PRO_i32FreigabeStatus;
			}
			set
			{
				PRO_i32FreigabeStatus = (int)value;
			}
		}

		public EDC_LoetprogrammVersionData()
		{
		}

		public EDC_LoetprogrammVersionData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0}.{1} = {2}", "ProgramHistory", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0}.{1} = {2}", "ProgramHistory", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strFreigegebeneVersionWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0}.{1} = {2} AND {3}.{4} = {5}", "ProgramHistory", "ProgramId", i_i64ProgrammId, "ProgramHistory", "Status", 3);
		}

		public static string FUN_strArbeitsversionWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0}.{1} = {2} AND {3}.{4} = {5} order by {6} desc", "ProgramHistory", "ProgramId", i_i64ProgrammId, "ProgramHistory", "Status", 4, "HistoryId");
		}

		public static string FUN_strSichtbareVersionenWhereStatementErstellen(long i_i64ProgrammId, bool i_blnMitSortierung = true)
		{
			string text = string.Format("Where {0}.{1} = {2} AND {3}.{4} in ({5},{6},{7})", "ProgramHistory", "ProgramId", i_i64ProgrammId, "ProgramHistory", "Status", 4, 3, 2);
			if (i_blnMitSortierung)
			{
				text += string.Format(" ORDER BY {0}", "SetNumber");
			}
			return text;
		}

		public static string FUN_strSelectMaxSetNummerFuerProgrammIdStatement(long i_i64ProgrammId)
		{
			return string.Format("Select max({0}) From {1} Where {2} = {3}", "SetNumber", "ProgramHistory", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strSelectDistinctStatusFuerProgrammIdStatement(long i_i64ProgrammId)
		{
			return string.Format("Select distinct({0}) From {1} {2}", "Status", "ProgramHistory", FUN_strSichtbareVersionenWhereStatementErstellen(i_i64ProgrammId, i_blnMitSortierung: false));
		}

		public static string FUN_strSelectDistinctProgrammIdOhneHistoryStatementErstellen()
		{
			return string.Format("Select distinct {0} From {1} Where {2} = '1'", "ProgramId", "ProgramHistory", "IsHidden");
		}

		public static string FUN_strSelectDistinctProgrammIdMitHistoryStatementErstellen()
		{
			return string.Format("Select distinct {0} From {1} Where {2} = '0'", "ProgramId", "ProgramHistory", "IsHidden");
		}

		public static string FUN_strMigrationsEinstiegsUpdateStatementErstellen()
		{
			return string.Format("Update {0} Set {1} = {2}, {3} = 0, {4} = {5}, {6} = {7}", "ProgramHistory", "Status", 1, "SetNumber", "ChangeDate", "CreationDate", "ChangeUser", "CreationUser");
		}

		public static string FUN_strVorlagenZuArbeitsversionenUpdateStatementErstellen()
		{
			return string.Format("Update {0} Set {1} = {2} Where {3} in (Select {4} FROM {5} Where {6} = '1')", "ProgramHistory", "Status", 4, "ProgramId", "ProgramId", "SolderingPrograms", "IsDefault");
		}

		public static string FUN_strAlleVersionenAufVersioniertUpdateStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Update {0} Set {1} = {2}, {3} = {4} Where {5} = {6} and {7} > 0", "ProgramHistory", "Status", 2, "SetNumber", "Version", "ProgramId", i_i64ProgrammId, "Version");
		}

		public static string FUN_strGroessteVersioneAlsFreigegebenUpdateStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Update {0} Set {1} = {2}, {3} = 1 {4}", "ProgramHistory", "Status", 3, "SetNumber", FUN_strGroessterVersionWhereStatementErstellen(i_i64ProgrammId));
		}

		public static string FUN_strGroessteVersioneAlsFreigegebenUndVersionBeibehaltenUpdateStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Update {0} Set {1} = {2}, {3} = {4} {5}", "ProgramHistory", "Status", 3, "SetNumber", "Version", FUN_strGroessterVersionWhereStatementErstellen(i_i64ProgrammId));
		}

		public static string FUN_strReleaseStateUpdateStatementErstellen()
		{
			return string.Format("Update {0} Set {1} = 0", "ProgramHistory", "ReleaseState");
		}

		private static string FUN_strGroessterVersionWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1} AND {2} = ({3})", "ProgramId", i_i64ProgrammId, "Version", FUN_strSelectMaxVersionFuerProgrammId(i_i64ProgrammId));
		}

		private static string FUN_strSelectMaxVersionFuerProgrammId(long i_i64ProgrammId)
		{
			return string.Format("Select max({0}) From {1} Where {2} = {3}", "Version", "ProgramHistory", "ProgramId", i_i64ProgrammId);
		}
	}
}
