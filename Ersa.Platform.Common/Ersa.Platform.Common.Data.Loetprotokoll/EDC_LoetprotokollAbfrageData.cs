using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Filter;
using Ersa.Global.Common.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_LoetprotokollAbfrageData
	{
		public const string gC_strSpalteBibliothekName = "BibliothekName";

		public const string gC_strSpalteProgrammName = "ProgrammName";

		public const string gC_strQuery = "Select Users.Username";

		private readonly long m_i64MaschinenId;

		private string m_strTabellennameDataFuerMaschine;

		private string m_strTabellennameKopfDataFuerMaschine;

		private string m_strTabellennameParameterDataFuerMaschine;

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "Users", PRO_strSpaltenName = "Username", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Username", PRO_i32Length = 20, PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProtocolId", PRO_blnIsRequired = true, PRO_blnIsPrimary = true)]
		public long PRO_i64ProtokollId
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "HistoryId", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|<|>", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("HistoryId", PRO_blnIsRequired = true)]
		public long PRO_i64LoetprogrammVersionsId
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "Draft", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Draft")]
		public bool PRO_blnArbeitsversion
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "EntryTime", PRO_strFilterKategorieNameKey = "Einlaufzeit", PRO_strFilterOperationen = "=|<>|<|>|between", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("EntryTime", PRO_blnIsRequired = true)]
		public DateTime PRO_dtmEingangszeit
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "OutletTime", PRO_strFilterKategorieNameKey = "Auslaufzeit", PRO_strFilterOperationen = "=|<>|<|>|between", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("OutletTime")]
		public DateTime PRO_dtmAusgangszeit
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "Faulty", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Faulty")]
		public bool PRO_blnLoetgutSchlecht
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "SerialBoardNumber", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|<|>", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("SerialBoardNumber")]
		public long PRO_i64LaufendeNummer
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolHeads", PRO_strSpaltenName = "Mode", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|<|>", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Mode")]
		public long PRO_i64Modus
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolParameter", PRO_strSpaltenName = "Parameter", PRO_strFilterKategorieNameKey = "Parameter", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Parameter", PRO_blnIsRequired = true, PRO_blnIsPrimary = true, PRO_i32Length = 100)]
		public string PRO_strParameter
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolParameter", PRO_strSpaltenName = "Content", PRO_strFilterKategorieNameKey = "Parameter", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Content", PRO_i32Length = 500)]
		public string PRO_strInhalt
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "ProtocolParameter", PRO_strSpaltenName = "Type", PRO_strFilterKategorieNameKey = "Parameter", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("Type", PRO_i32Length = 500)]
		public string PRO_strTyp
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "SolderingLibraries", PRO_strSpaltenName = "Name", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("BibliothekName", PRO_i32Length = 100, PRO_blnIsRequired = true)]
		public string PRO_strBibliothekName
		{
			get;
			set;
		}

		[EDC_FilterInformationen(PRO_strTabellenName = "SolderingPrograms", PRO_strSpaltenName = "Name", PRO_strFilterKategorieNameKey = "Erweitert", PRO_strFilterOperationen = "=|<>|in|not in|like", PRO_strFilterVerknuepfungen = "or|and")]
		[EDC_SpaltenInformation("ProgrammName", PRO_i32Length = 100, PRO_blnIsRequired = true)]
		public string PRO_strProgrammName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation(PRO_blnIsDynamischeSpalte = true)]
		public IEnumerable<EDC_DynamischeSpalte> PRO_lstDynamischeSpalten
		{
			get;
			set;
		} = new List<EDC_DynamischeSpalte>();


		public string PRO_strAbfrageStatement => string.Format("{0}, {1} {2}", "Select Users.Username", FUN_strBestimmeDynamischenSelektTeil(), FUN_strBestimmeDynamischesFromTeil());

		public EDC_LoetprotokollAbfrageData()
		{
		}

		public EDC_LoetprotokollAbfrageData(long i_blnMaschinenId, List<EDC_DynamischeSpalte> i_blnEdcDynamischeSpalten)
		{
			m_i64MaschinenId = i_blnMaschinenId;
			PRO_lstDynamischeSpalten = i_blnEdcDynamischeSpalten;
			SUB_BestimmeTabellennamenProMaschine();
		}

		public EDC_LoetprotokollAbfrageData(long i_blnMaschinenId, List<EDC_DynamischeSpalte> i_blnEdcDynamischeSpalten, string i_strWhereStatement)
		{
			m_i64MaschinenId = i_blnMaschinenId;
			PRO_lstDynamischeSpalten = i_blnEdcDynamischeSpalten;
			PRO_strWhereStatement = i_strWhereStatement;
			SUB_BestimmeTabellennamenProMaschine();
		}

		public static string FUN_strWhereUndOrderByStatementMitListeFilterKonkretErstellen(IEnumerable<STRUCT_FilterKonkret> i_lstFilterKonkret, Dictionary<string, object> i_fdcDictionary)
		{
			List<STRUCT_FilterKonkret> i_lstFilterKonkret2 = i_lstFilterKonkret.ToList();
			return $"{EDC_FilterErstellungsHelfer.FUN_strErstelleFilterWhereStatement(i_lstFilterKonkret2, i_fdcDictionary)} {EDC_FilterErstellungsHelfer.FUN_strErstelleFilterOrderByStatement(i_lstFilterKonkret2)}";
		}

		public static string FUN_strProtokollIdWhereStatementErstellen(long i_i64ProtokollId, long i_i64MaschinenId)
		{
			return string.Format("Where {0}_MA{1}.{2} = {3}", "ProtocolData", i_i64MaschinenId, "ProtocolId", i_i64ProtokollId);
		}

		private string FUN_strBestimmeDynamischesFromTeil()
		{
			string text = $"From {m_strTabellennameKopfDataFuerMaschine}";
			text = string.Format("{0} Left Outer Join {1} On {2}.{3} = {4}.{5}", text, m_strTabellennameDataFuerMaschine, m_strTabellennameKopfDataFuerMaschine, "ProtocolId", m_strTabellennameDataFuerMaschine, "ProtocolId");
			text = string.Format("{0} Left Outer Join {1} On {2}.{3} = {4}.{5}", text, m_strTabellennameParameterDataFuerMaschine, m_strTabellennameKopfDataFuerMaschine, "ProtocolId", m_strTabellennameParameterDataFuerMaschine, "ProtocolId");
			text = string.Format("{0} Inner Join {1} On {2}.{3} = {4}.{5}", text, "Users", m_strTabellennameKopfDataFuerMaschine, "UserId", "Users", "UserId");
			text = string.Format("{0} Inner Join {1} On {2}.{3} = {4}.{5}", text, "ProgramHistory", m_strTabellennameKopfDataFuerMaschine, "HistoryId", "ProgramHistory", "HistoryId");
			text = string.Format("{0} Inner Join {1} On {2}.{3} = {4}.{5}", text, "SolderingPrograms", "ProgramHistory", "ProgramId", "SolderingPrograms", "ProgramId");
			return string.Format("{0} Inner Join {1} On {2}.{3} = {4}.{5}", text, "SolderingLibraries", "SolderingPrograms", "LibraryId", "SolderingLibraries", "LibraryId");
		}

		private string FUN_strBestimmeDynamischenSelektTeil()
		{
			string arg = string.Format("{0}.{1}", m_strTabellennameKopfDataFuerMaschine, "ProtocolId");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "HistoryId");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "Draft");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "EntryTime");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "OutletTime");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "Faulty");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "SerialBoardNumber");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameKopfDataFuerMaschine, "Mode");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameParameterDataFuerMaschine, "Parameter");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameParameterDataFuerMaschine, "Content");
			arg = string.Format("{0}, {1}.{2}", arg, m_strTabellennameParameterDataFuerMaschine, "Type");
			arg = string.Format("{0}, {1}.{2} as {3}", arg, "SolderingLibraries", "Name", "BibliothekName");
			arg = string.Format("{0}, {1}.{2} as {3}", arg, "SolderingPrograms", "Name", "ProgrammName");
			return PRO_lstDynamischeSpalten.Aggregate(arg, (string i_blnCurrent, EDC_DynamischeSpalte i_edcDynamischeSpalte) => $"{i_blnCurrent}, {m_strTabellennameDataFuerMaschine}.{i_edcDynamischeSpalte.PRO_strSpaltenName}");
		}

		private void SUB_BestimmeTabellennamenProMaschine()
		{
			EDC_LoetprotokollData eDC_LoetprotokollData = new EDC_LoetprotokollData(m_i64MaschinenId);
			m_strTabellennameDataFuerMaschine = eDC_LoetprotokollData.PRO_strTabellenname;
			EDC_LoetprotokollKopfData eDC_LoetprotokollKopfData = new EDC_LoetprotokollKopfData(m_i64MaschinenId);
			m_strTabellennameKopfDataFuerMaschine = eDC_LoetprotokollKopfData.PRO_strTabellenname;
			EDC_LoetprotokollParameterData eDC_LoetprotokollParameterData = new EDC_LoetprotokollParameterData(m_i64MaschinenId);
			m_strTabellennameParameterDataFuerMaschine = eDC_LoetprotokollParameterData.PRO_strTabellenname;
		}
	}
}
