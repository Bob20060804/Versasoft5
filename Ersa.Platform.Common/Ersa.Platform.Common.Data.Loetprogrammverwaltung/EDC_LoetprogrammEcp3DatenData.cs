using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("ProgramEcp3Data", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammEcp3DatenData
	{
		public const string gC_strTabellenName = "ProgramEcp3Data";

		public const string gC_strSpalteHistoryId = "HistoryId";

		private const string mC_strSpalteVersion = "Version";

		private const string mC_strSpalteBeschreibung = "Description";

		private const string mC_strSpalteModifikationsOperator = "ModObject";

		private const string mC_strSpalteVerboteneBereiche = "BoundingCubes";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("HistoryId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64HistoryId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Version", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 20)]
		public string PRO_strVersion
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

		[EDC_SpaltenInformation("ModObject")]
		public string PRO_strModifikationsOperator
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("BoundingCubes")]
		public string PRO_strVerboteneBereiche
		{
			get;
			set;
		}

		public EDC_LoetprogrammEcp3DatenData()
		{
		}

		public EDC_LoetprogrammEcp3DatenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(long i_i64HistoryId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64HistoryId);
		}

		public static string FUN_strHistoryEintraegeLoeschenStatementErstellen(IEnumerable<long> i_enuHistoryIds)
		{
			return string.Format("Delete from  {0} Where {1} in ({2})", "ProgramEcp3Data", "HistoryId", string.Join(",", (from n in i_enuHistoryIds
			select n.ToString()).ToArray()));
		}
	}
}
