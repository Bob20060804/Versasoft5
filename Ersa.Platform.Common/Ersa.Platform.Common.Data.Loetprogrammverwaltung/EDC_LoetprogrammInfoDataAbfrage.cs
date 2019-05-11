using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_LoetprogrammInfoDataAbfrage
	{
		public const string gC_strSpalteMaschineId = "MachineId";

		private const string gC_strSpalteBibliothekId = "BibId";

		private const string gC_strSpalteBibName = "BibName";

		private const string gC_strSpalteProgrammId = "ProgId";

		private const string gC_strSpalteLpName = "LpName";

		private const string gC_strSpalteIstDefault = "IsDefault";

		private const string gC_strSpalteNotizen = "Notes";

		private const string gC_strSpalteStatus = "Status";

		private const string gC_strSpalteSetNummer = "SetNumber";

		private const string gC_strSpalteBenutzername = "Username";

		private const string gC_strSpalteBearbeitetAm = "ChangeDate";

		private const string gC_strSpalteVersionsId = "HistoryId";

		private const string gC_strSpalteBeschreibung = "Description";

		private const string gC_strSpalteIstValide = "IsValid";

		private const string gC_strSpalteEingangskontrolle = "InfeedInspection";

		private const string gC_strSpalteAusgangskontrolle = "OutfeedInspection";

		private const string gC_strSpalteProgrammVersion = "Version";

		private const string gC_strSpalteFreigabeStatus = "ReleaseState";

		private const string gC_strSpalteFreigabeNotizen = "ReleaseNotes";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		public string PRO_strAbfrageStatement => string.Format("SELECT {0}.{1} AS {2}, ", "SolderingLibraries", "LibraryId", "BibId") + string.Format("{0}.{1} AS {2}, ", "SolderingLibraries", "Name", "BibName") + string.Format("{0}.{1} AS {2}, ", "SolderingPrograms", "ProgramId", "ProgId") + string.Format("{0}.{1} AS {2}, ", "SolderingPrograms", "Name", "LpName") + string.Format("{0}.{1} AS {2}, ", "SolderingPrograms", "IsDefault", "IsDefault") + string.Format("{0}.{1} AS {2}, ", "SolderingPrograms", "ChangeDate", "ChangeDate") + string.Format("{0}.{1} AS {2}, ", "SolderingPrograms", "Notes", "Notes") + string.Format("{0}.{1} AS {2},", "SolderingPrograms", "Description", "Description") + string.Format("{0}.{1} AS {2},", "SolderingPrograms", "InfeedInspection", "InfeedInspection") + string.Format("{0}.{1} AS {2},", "SolderingPrograms", "OutfeedInspection", "OutfeedInspection") + string.Format("{0}.{1} AS {2},", "SolderingPrograms", "Version", "Version") + string.Format("{0}.{1} AS {2}, ", "ProgramHistory", "HistoryId", "HistoryId") + string.Format("{0}.{1} AS {2}, ", "ProgramHistory", "Status", "Status") + string.Format("{0}.{1} AS {2}, ", "ProgramHistory", "SetNumber", "SetNumber") + string.Format("{0}.{1} AS {2}, ", "ProgramHistory", "ReleaseState", "ReleaseState") + string.Format("{0}.{1} AS {2}, ", "ProgramHistory", "ReleaseNotes", "ReleaseNotes") + string.Format("{0}.{1} AS {2}, ", "Users", "Username", "Username") + string.Format("{0}.{1} AS {2}, ", "SolderingVersionValid", "MachineId", "MachineId") + string.Format("case when {0}.{1} is null Then '1' else  {2}.{3} end AS {4}", "SolderingVersionValid", "IsValid", "SolderingVersionValid", "IsValid", "IsValid") + string.Format(" FROM {0} ", "SolderingLibraries") + string.Format(" INNER JOIN {0} ON {1}.{2} = {3}.{4}", "SolderingPrograms", "SolderingLibraries", "LibraryId", "SolderingPrograms", "LibraryId") + string.Format(" INNER JOIN {0} ON {1}.{2} = {3}.{4}", "ProgramHistory", "SolderingPrograms", "ProgramId", "ProgramHistory", "ProgramId") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "Users", "Users", "UserId", "SolderingPrograms", "ChangeUser") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "SolderingVersionValid", "SolderingVersionValid", "HistoryId", "ProgramHistory", "HistoryId");

		[EDC_SpaltenInformation("BibId")]
		public long PRO_i64BibliotheksId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BibName")]
		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProgId")]
		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LpName")]
		public string PRO_strProgrammName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsDefault")]
		public bool PRO_blnIstDefault
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Version")]
		public int PRO_i64Version
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

		[EDC_SpaltenInformation("Description")]
		public string PRO_strBeschreibung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Notes")]
		public string PRO_strNotizen
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("InfeedInspection")]
		public string PRO_strEingangskontrolle
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OutfeedInspection")]
		public string PRO_strAusgangskontrolle
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("HistoryId")]
		public long PRO_i64VersionsId
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

		[EDC_SpaltenInformation("Username")]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsValid")]
		public bool PRO_blnValide
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId")]
		public long? PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ReleaseState")]
		public int PRO_i32FreigebeStatus
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ReleaseNotes")]
		public string PRO_strFreigebeNotizen
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
				return (ENUM_LoetprogrammFreigabeStatus)PRO_i32FreigebeStatus;
			}
			set
			{
				PRO_i32FreigebeStatus = (int)value;
			}
		}

		public EDC_LoetprogrammInfoDataAbfrage()
		{
		}

		public EDC_LoetprogrammInfoDataAbfrage(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0}.{1} = {2} ", "SolderingPrograms", "ProgramId", i_i64ProgrammId) + $"{FUN_strHoleSichtbareVersionenAbfrageErweiterung()}";
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where  {0}.{1} = {2} ", "ProgramHistory", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strAktuelleFreigegebeneVersionWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0}.{1} = {2} ", "ProgramHistory", "Status", 3) + string.Format("and {0}.{1} in ", "ProgramHistory", "ProgramId") + string.Format("(select {0}.{1} ", "ProgramHistory", "ProgramId") + string.Format("from {0} ", "ProgramHistory") + string.Format("where {0}.{1} = {2})", "ProgramHistory", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strBibliotheksIdUndNichtGeloeschtWhereStatementErstellen(long i_i64BibliotheksId)
		{
			return FUN_strBibliotheksIdUndNichtGeloeschtBasisWhereStatementErstellen(i_i64BibliotheksId) + $"{FUN_strHoleSichtbareVersionenAbfrageErweiterung()}";
		}

		public static string FUN_strBibliotheksIdUndNichtGeloeschtMitSuchbegriffWhereStatementErstellen(long i_i64BibliotheksId, string i_strSuchbegriff)
		{
			return FUN_strBibliotheksIdUndNichtGeloeschtBasisWhereStatementErstellen(i_i64BibliotheksId) + string.Format("and Lower({0}.{1}) Like Lower('%{2}%') ", "SolderingPrograms", "Name", i_strSuchbegriff) + $"{FUN_strHoleSichtbareVersionenAbfrageErweiterung()}";
		}

		private static string FUN_strBibliotheksIdUndNichtGeloeschtBasisWhereStatementErstellen(long i_i64BibliotheksId)
		{
			return string.Format("Where {0}.{1} = {2} ", "SolderingLibraries", "LibraryId", i_i64BibliotheksId) + string.Format("and {0}.{1} = '0' ", "SolderingPrograms", "Deleted");
		}

		private static string FUN_strHoleSichtbareVersionenAbfrageErweiterung()
		{
			return string.Format("AND {0}.{1} in ({2},{3},{4}) order by {0}.{1} desc", "ProgramHistory", "Status", 4, 3, 2);
		}
	}
}
