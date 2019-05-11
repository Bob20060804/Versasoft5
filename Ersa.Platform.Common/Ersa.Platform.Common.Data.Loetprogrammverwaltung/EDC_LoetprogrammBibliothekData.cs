using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using System;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("SolderingLibraries", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammBibliothekData
	{
		public const string gC_strTabellenName = "SolderingLibraries";

		public const string gC_strSpalteBibliothekId = "LibraryId";

		public const string gC_strSpalteGruppenId = "GroupId";

		public const string gC_strSpalteName = "Name";

		public const string gC_strSpalteGeloescht = "Deleted";

		public const string gC_strSpalteAngelegtAm = "CreationDate";

		public const string gC_strSpalteAngelegtVon = "CreationUser";

		public const string gC_strSpalteBearbeitetAm = "ChangeDate";

		public const string gC_strSpalteBearbeitetVon = "ChangeUser";

		public const string gC_strParameterBearbeitetAm = "pCreationDate";

		protected const string gC_strSpalteBeschreibung = "Description";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LibraryId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64BibliothekId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GroupId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64GruppenId
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

		[EDC_SpaltenInformation("Deleted")]
		public bool PRO_blnGeloescht
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

		public long PRO_i64ProgramAnzahl
		{
			get;
			set;
		}

		public EDC_LoetprogrammBibliothekData()
		{
		}

		public EDC_LoetprogrammBibliothekData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBibliotheksIdWhereStatementErstellen(long i_i64BibliotheksId)
		{
			return string.Format("Where {0} = {1}", "LibraryId", i_i64BibliotheksId);
		}

		public static string FUN_strBibliotheksNameWhereStatementErstellen(string i_strBibliotheksName, long i_i64MaschinenId)
		{
			return string.Format("{0} AND {1} = '{2}'", FUN_strGruppenIdWhereStatementErstellen(i_i64MaschinenId), "Name", i_strBibliotheksName);
		}

		public static string FUN_strDefaultBibWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("{0} and {1} = '{2}'", FUN_strGruppenIdWhereStatementErstellen(i_i64MaschinenId), "Name", "[defaultLibrary]");
		}

		public static string FUN_strUmbenennenUpdateAufIdStatementErstellen(long i_i64BibliothekId, string i_strBibliothekNeuerName)
		{
			return string.Format("Update {0} Set {1} = '{2}' Where {3} = {4} AND {5} = '0'", "SolderingLibraries", "Name", i_strBibliothekNeuerName, "LibraryId", i_i64BibliothekId, "Deleted");
		}

		public static string FUN_strLoeschenUpdateStatementErstellen(long i_i64BibliothekId, long i_i64BenutzerId)
		{
			return string.Format("Update {0} Set {1} = '1', {2} = {3}, {4} = @{5} Where {6} = {7}", "SolderingLibraries", "Deleted", "ChangeUser", i_i64BenutzerId, "ChangeDate", "pCreationDate", "LibraryId", i_i64BibliothekId);
		}

		public static string FUN_strAlleNichtGeloeschtenBibliothkenWhereErstellen(long i_i64MaschinenId)
		{
			return string.Format(" Where {0} = '0' and not {1} = '{2}' and {3} in ({4}) order by {5}", "Deleted", "Name", "[defaultLibrary]", "GroupId", EDC_MaschinenGruppenMitgliedData.FUN_strHoleGruppenIdVonMaschinenIdSunbselect(i_i64MaschinenId), "Name");
		}

		private static string FUN_strGruppenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} in ({1}) and {2} = '0'", "GroupId", EDC_MaschinenGruppenMitgliedData.FUN_strHoleGruppenIdVonMaschinenIdSunbselect(i_i64MaschinenId), "Deleted");
		}
	}
}
