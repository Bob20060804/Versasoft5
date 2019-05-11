using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Aoi
{
	[EDC_TabellenInformation("AoiPrograms", PRO_strTablespace = "ess5_cad")]
	public class EDC_AoiProgramData
	{
		public const string gC_strTabellenName = "AoiPrograms";

		private const string mC_strSpalteProgramId = "ProgramId";

		private const string mC_strSpalteDaten = "Data";

		private const string mC_strSpalteSetting = "Setting";

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

		public EDC_AoiProgramData()
		{
		}

		public EDC_AoiProgramData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProgrammIdWhereStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Where {0} = {1}", "ProgramId", i_i64ProgrammId);
		}

		public static string FUN_strLoescheProgrammStatementErstellen(long i_i64ProgrammId)
		{
			return string.Format("Delete from {0} {1}", "AoiPrograms", FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId));
		}
	}
}
