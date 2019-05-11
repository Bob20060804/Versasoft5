using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("SolderingPrograms", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammData
	{
		public const string gC_strTabellenName = "SolderingPrograms";

		public const string gC_strSpalteProgrammId = "ProgramId";

		public const string gC_strSpalteBibliotheksId = "LibraryId";

		public const string gC_strSpalteName = "Name";

		public const string gC_strSpalteBeschreibung = "Description";

		public const string gC_strSpalteNotizen = "Notes";

		public const string gC_strSpalteGeloescht = "Deleted";

		public const string gC_strSpalteAngelegtAm = "CreationDate";

		public const string gC_strSpalteAngelegtVon = "CreationUser";

		public const string gC_strSpalteBearbeitetAm = "ChangeDate";

		public const string gC_strSpalteBearbeitetVon = "ChangeUser";

		public const string gC_strSpalteIstDefault = "IsDefault";

		public const string gC_strSpalteProgrammVersion = "Version";

		public const string gC_strSpalteEingangskontrolle = "InfeedInspection";

		public const string gC_strSpalteAusgangskontrolle = "OutfeedInspection";

		public const string gC_strParameterBearbeitetAm = "pCreationDate";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
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

		[EDC_SpaltenInformation("LibraryId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64BibliotheksId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Name", PRO_i32Length = 100, PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public string PRO_strName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Description", PRO_i32Length = 250)]
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

		[EDC_SpaltenInformation("Deleted")]
		public bool PRO_blnGeloescht
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Version")]
		public int PRO_i32Version
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

		[EDC_SpaltenInformation("IsDefault")]
		public bool PRO_blnIstDefault
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("InfeedInspection", PRO_i32Length = 200)]
		public string PRO_strEingangskontrolle
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OutfeedInspection", PRO_i32Length = 200)]
		public string PRO_strAusgangskontrolle
		{
			get;
			set;
		}

		public bool PRO_blnIstFuerMaschineValide
		{
			get;
			set;
		}

		public EDC_LoetprogrammData()
		{
		}

		public EDC_LoetprogrammData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strProgrammNameBibliotheksIdWhereStatementErstellen(string i_strProgrammName, long i_i64BibliotheksId)
		{
			return string.Format("Where {0} = '{1}' and {2} = {3} and {4} = '0'", "Name", i_strProgrammName, "LibraryId", i_i64BibliotheksId, "Deleted");
		}

		public static string FUN_strProgrammNameBibliotheksNameWhereStatementErstellen(string i_strProgrammName, string i_strBibliothekName)
		{
			return string.Format("Where {0} = '{1}' and {2} = '0' and {3} in (select {4} from {5} Where {6} = '{7}' AND {8} = '0')", "Name", i_strProgrammName, "Deleted", "LibraryId", "LibraryId", "SolderingLibraries", "Name", i_strBibliothekName, "Deleted");
		}

		public static string FUN_strAbfrageProgrammVonNameUndBibliotheksIdWhereStatementErstellen(string i_strProgrammName, long i_i64BibliotheksId)
		{
			return string.Format("Where {0} = '{1}' and {2} = {3} and {4} = '0'", "Name", i_strProgrammName, "LibraryId", i_i64BibliotheksId, "Deleted");
		}

		public static string FUN_strProgrammIdBibliotheksIdWhereStatementErstellen(long i_i64ProgrammId, long i_i64BibliotheksId)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "ProgramId", i_i64ProgrammId, "LibraryId", i_i64BibliotheksId);
		}

		public static string FUN_strBibliotheksIdNichtGeloeschtWhereStatementErstellen(long i_i64BibliotheksId)
		{
			return string.Format("Where {0} = {1} and {2} = '0' order by {3}", "LibraryId", i_i64BibliotheksId, "Deleted", "Name");
		}

		public static string FUN_strBibliotheksNameNichtGeloeschtWhereStatementErstellen(string i_strBibliotheksName)
		{
			return string.Format("Where {1} = '0' and {2} in (Select {3} From {4} Where {5} = '0' and {6} = '{7}')", "IsDefault", "Deleted", "LibraryId", "LibraryId", "SolderingLibraries", "Deleted", "Name", i_strBibliotheksName);
		}

		public static string FUN_strLoeschenUpdateStatementErstellen(long i_i64ProgrammId, long i_i64BenutzerId)
		{
			return string.Format("Update {0} Set {1} = '1', {2} = {3}, {4} = @{5} Where {6} = {7}", "SolderingPrograms", "Deleted", "ChangeUser", i_i64BenutzerId, "ChangeDate", "pCreationDate", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strProgrammeZuBibliothekLoeschenUpdateStatementErstellen(long i_i64BibiliotheksId, long i_i64BenutzerId)
		{
			return string.Format("Update {0} Set {1} = '1', {2} = {3}, {4} = @{5} Where {6} = {7}", "SolderingPrograms", "Deleted", "ChangeUser", i_i64BenutzerId, "ChangeDate", "pCreationDate", "LibraryId", i_i64BibiliotheksId);
		}

		public static string FUN_strProgrammeZuNeuerBibliothekZuordnenUpdateStatementErstellen(long i_i64ProgramId, long i_i64NeueBibiliotheksId)
		{
			return string.Format("Update {0} Set {1} = {2} Where {3} = {4}", "SolderingPrograms", "LibraryId", i_i64NeueBibiliotheksId, "ProgramId", i_i64ProgramId);
		}

		public static string FUN_strProgrammeNeuerNameZuordnenUpdateStatementErstellen(long i_i64ProgramId, string i_strNeuerName)
		{
			return string.Format("Update {0} Set {1} = '{2}' Where {3} = {4}", "SolderingPrograms", "Name", i_strNeuerName, "ProgramId", i_i64ProgramId);
		}

		public static string FUN_strNichtGeloeschteProgrammeAnzahlAbfrageStatementErstellen(long i_i64BibliotheksId)
		{
			return string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} and {3} = '0'", "SolderingPrograms", "LibraryId", i_i64BibliotheksId, "Deleted");
		}

		public static string FUN_strDefaultProgrammVersionNummerStatementErstellen()
		{
			return string.Format("Update {0} Set {1} = 0 Where {2} is null", "SolderingPrograms", "Version", "Version");
		}

		public static string FUN_strProgrammNeueVersionZuordnenUpdateStatementErstellen(IEnumerable<long> i_enuProgramId, long i_i64NeueVersion)
		{
			return string.Format("Update {0} Set {1} = {2} Where {3} in ({4})", "SolderingPrograms", "Version", i_i64NeueVersion, "ProgramId", string.Join(",", i_enuProgramId));
		}

		public static string FUN_strDefaultProgrammeWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = (Select Min({1}) ", "ProgramId", "ProgramId") + string.Format("From {0} Where {1} = '1' ", "SolderingPrograms", "IsDefault") + string.Format("and {0} = '0' and {1} ", "Deleted", "LibraryId") + string.Format("in (Select {0} ", "LibraryId") + string.Format("From {0} ", "SolderingLibraries") + string.Format("Where {0} = '0' ", "Deleted") + string.Format("and {0} ", "GroupId") + string.Format("in (Select {0} ", "GroupId") + string.Format("From {0} ", "MachineGroupMembers") + string.Format("Where {0} = {1})))", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strErstelleProgrammIdAbfrageVonBibliotheksNameundProgrammNamen(string i_strBibliotheksName, string i_strProgrammName)
		{
			return string.Format("SELECT {0}.{1} FROM {2} INNER JOIN {0} ON {2}.{3} = {0}.{4} WHERE ({2}.{5} = '{6}') AND ({0}.{7} = '{8}') AND ({0}.{9} = '0')", "SolderingPrograms", "ProgramId", "SolderingLibraries", "LibraryId", "LibraryId", "Name", i_strBibliotheksName, "Name", i_strProgrammName, "Deleted");
		}

		public static string FUN_strMaschinenGruppenWhereStatementErstellen(IEnumerable<long> i_enuMaschinenGruppenIds)
		{
			return string.Format("WHERE {0} IN (SELECT {1} \r\n                FROM {2} \r\n                WHERE {3} \r\n                IN ({4}))", "LibraryId", "LibraryId", "SolderingLibraries", "GroupId", string.Join(",", i_enuMaschinenGruppenIds));
		}
	}
}
