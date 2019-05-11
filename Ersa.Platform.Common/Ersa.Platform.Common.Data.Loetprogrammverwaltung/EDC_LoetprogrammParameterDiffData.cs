using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramParameter", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammParameterDiffData
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

		[EDC_SpaltenInformation("Variable", PRO_i32Length = 100)]
		public string PRO_strVariable
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Value", PRO_i32Length = 50)]
		public string PRO_strWert
		{
			get;
			set;
		}

		public EDC_LoetprogrammParameterDiffData()
		{
		}

		public EDC_LoetprogrammParameterDiffData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strErstelleVariablenEinerVersionAbfrageString(long i_i64VersionsId, IEnumerable<string> i_enuVariablen)
		{
			return string.Format("{0} and {1} in ({2})", FUN_strErstelleSelectAllStringMitVersionsId(i_i64VersionsId), "Variable", string.Join<string>(",", (IEnumerable<string>)(from i_strVariable in i_enuVariablen
			select $"'{i_strVariable}'").ToList()));
		}

		public static string FUN_strErstelleVersionenVergleichAbfrageString(long i_i64VersionAltId, long i_i64VersionNeuId)
		{
			return $"{FUN_strErstelleSelectAllStringMitVersionsId(i_i64VersionAltId)} EXCEPT {FUN_strErstelleSelectAllStringMitVersionsId(i_i64VersionNeuId)}";
		}

		private static string FUN_strErstelleSelectAllStringMitVersionsId(long i_i64VersionsId)
		{
			return string.Format("SELECT {0}, {1} from  {2} Where {3} = {4}", "Variable", "Value", "ProgramParameter", "HistoryId", i_i64VersionsId);
		}
	}
}
