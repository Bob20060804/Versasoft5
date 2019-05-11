using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("MessagesCyclicTemplates", PRO_strTablespace = "ess5_messages")]
	public class EDC_ZyklischeMeldungVorlageData
	{
		public const string mC_strTabellenName = "MessagesCyclicTemplates";

		private const string mC_strSpalteVorlageId = "TemplateId";

		private const string mC_strSpalteAktiv = "Activ";

		private const string mC_strSpalteMeldung = "Message";

		private const string mC_strSpalteOrt1 = "Facility1";

		private const string mC_strSpalteOrt2 = "Facility2";

		private const string mC_strSpalteOrt3 = "Facility3";

		private const string mC_strSpalteIntervall = "Interval";

		private const string mC_strSpalteZeit1 = "Time1";

		private const string mC_strSpalteZeit2 = "Time2";

		private const string mC_strSpalteEinlaufSperreAktiv = "BlockInfeedActive";

		private const string mC_strSpalteEinlaufSperreNach = "BlockInfeedAfter";

		private const string mC_strSpalteAnzahlRuecksetzungen = "NumberOfResets";

		private const string mC_strSpalteIntervallRuecksetzungen = "PeriodOfResets";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TemplateId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64VorlageId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CyclicLibId", PRO_blnIsRequired = true)]
		public long PRO_i64VorlageGruppeId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Activ", PRO_blnIsRequired = true)]
		public bool PRO_blnAktiv
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Message", PRO_i32Length = 200, PRO_blnIsRequired = true)]
		public string PRO_strMeldung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility1", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility2", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility3", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt3
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Interval")]
		public int PRO_i32Intervall
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Time1")]
		public int PRO_i32Zeit1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Time2")]
		public int PRO_i32Zeit2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BlockInfeedActive")]
		public bool PRO_blnEinlaufSperreAktiv
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BlockInfeedAfter")]
		public int PRO_i32EinlaufSperreNach
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NumberOfResets")]
		public int PRO_i32AnzahlRueckstellungen
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PeriodOfResets")]
		public int PRO_i32IntervallRueckstellungen
		{
			get;
			set;
		}

		public EDC_ZyklischeMeldungVorlageData()
		{
		}

		public EDC_ZyklischeMeldungVorlageData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVorlageIdWhereStatementErstellen(long i_i64MeldungsVorlageId)
		{
			return string.Format("Where {0} = {1}", "TemplateId", i_i64MeldungsVorlageId);
		}

		public static string FUN_strVorlageGruppeIdWhereStatementErstellen(long i_i64VorlageGruppeId)
		{
			return string.Format("Where {0} = {1}", "CyclicLibId", i_i64VorlageGruppeId);
		}

		public static string FUN_strVorlageGruppeNameWhereStatementErstellen(string i_strName)
		{
			return string.Format("Where {0} = '{1}'", "Name", i_strName);
		}
	}
}
