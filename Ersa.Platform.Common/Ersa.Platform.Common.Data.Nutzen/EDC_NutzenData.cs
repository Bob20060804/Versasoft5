using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Nutzen
{
	[EDC_TabellenInformation("PanelData", PRO_strTablespace = "ess5_programs")]
	public class EDC_NutzenData
	{
		public const string gC_strTabellenName = "PanelData";

		private const string mC_strSpalteNutzenDataId = "PanelDataId";

		private const string mC_strSpalteHash = "Hash";

		private const string mC_strSpalteNutzenCode = "PanelCode";

		private const string mC_strSpalteNestCode = "PcbCode";

		private const string mC_strSpalteNestData = "PcbData";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PanelDataId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64NutzenDataId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Hash", PRO_blnIsRequired = true, PRO_i32Length = 32)]
		public string PRO_strHash
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PanelCode", PRO_blnIsRequired = true, PRO_i32Length = 400)]
		public string PRO_strNutzenCode
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PcbCode", PRO_blnIsRequired = true, PRO_i32Length = 400)]
		public string PRO_strNestCode
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("PcbData", PRO_blnIsRequired = true)]
		public string PRO_strNestDaten
		{
			get;
			set;
		}

		public EDC_NutzenData()
		{
		}

		public EDC_NutzenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHashWhereStatementErstellen(string i_strHash)
		{
			return string.Format("Where {0} = '{1}'", "Hash", i_strHash);
		}

		public static string FUN_strNutzenCodeWhereStatementErstellen(string i_strHash, string i_strNutzenCode)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}'", "Hash", i_strHash, "PanelCode", i_strNutzenCode);
		}

		public static string FUN_strNestCodeWhereStatementErstellen(string i_strHash, string i_strPcbCode)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}'", "Hash", i_strHash, "PcbCode", i_strPcbCode);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}", "PanelDataId", PRO_i64NutzenDataId, "Hash", PRO_strHash, "PanelCode", PRO_strNutzenCode, "PcbCode", PRO_strNestCode, "PcbData", PRO_strNestDaten);
		}
	}
}
