using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Globalization;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramParameter", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammParameterData
	{
		public const string gC_strTabellenName = "ProgramParameter";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteVariable = "Variable";

		private const string mC_strSpalteDatenTyp = "DataType";

		private const string mC_strSpalteWert = "Value";

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

		[EDC_SpaltenInformation("Variable", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 100)]
		public string PRO_strVariable
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DataType", PRO_i32Length = 10)]
		public string PRO_strDatenTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Value", PRO_i32Length = 50, PRO_blnIsRequired = true)]
		public string PRO_strWert
		{
			get;
			set;
		}

		public EDC_LoetprogrammParameterData()
		{
		}

		public EDC_LoetprogrammParameterData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strVersionsIdWhereStatementErstellen(IEnumerable<long> i_lst64VersionsId)
		{
			string text = string.Empty;
			foreach (long item in i_lst64VersionsId)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += item.ToString(CultureInfo.InvariantCulture);
			}
			return string.Format("Where {0} in ({1}) order by {0}", "HistoryId", text);
		}

		public static string FUN_strHistoryEintraegeLoeschenStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Delete from  {0} Where {1} = {2}", "ProgramParameter", "HistoryId", i_i64VersionsId);
		}
	}
}
