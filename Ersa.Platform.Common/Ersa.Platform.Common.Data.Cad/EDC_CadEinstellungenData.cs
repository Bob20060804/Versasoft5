using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadSettings", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadEinstellungenData
	{
		public const string gC_strTabellenName = "CadSettings";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteDaten = "Data";

		private const string mC_strSpalteSetting = "Setting";

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

		[EDC_SpaltenInformation("Setting")]
		public string PRO_strEinstellungen
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Data")]
		public string PRO_strDaten
		{
			get;
			set;
		}

		public EDC_CadEinstellungenData()
		{
		}

		public EDC_CadEinstellungenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(long i_i64VersionId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionId);
		}

		public static string FUN_strLoescheEinstellungStatementErstellen(long i_i64VersionId)
		{
			return string.Format("Delete from {0} {1}", "CadSettings", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionId));
		}
	}
}
