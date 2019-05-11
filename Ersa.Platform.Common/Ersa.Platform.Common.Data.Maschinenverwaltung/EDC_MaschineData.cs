using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("Machines", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschineData
	{
		public const string mC_strTabellenName = "Machines";

		public const string mC_strSpalteCodetabellenId = "CodeTableId";

		public const string mC_strSpalteDefaultProgramm = "DefaultProgramId";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteMaschinenTyp = "MachineType";

		private const string mC_strSpalteMaschinenNummer = "MachineNumber";

		private const string mC_strSpalteKommentar = "Comment";

		private const string mC_strSpalteOrt = "Location";

		private const string mC_strSpalteProduktionslinie = "ProductionLine";

		private const string mC_strSpalteAngelegt = "CreationDate";

		private const string mC_strSelektivMaschinenPrefix = "S";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineType", PRO_i32Length = 40, PRO_blnIsRequired = true)]
		public string PRO_strMaschinenTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineNumber", PRO_i32Length = 30, PRO_blnIsRequired = true)]
		public string PRO_strMaschinenNummer
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Comment")]
		public string PRO_strKommentar
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Location", PRO_i32Length = 100)]
		public string PRO_strOrt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProductionLine", PRO_i32Length = 100)]
		public string PRO_strProduktionslinie
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_dtmAngelegt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeTableId")]
		public long PRO_i64CodetabellenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DefaultProgramId")]
		public long PRO_i64DefaultProgramId
		{
			get;
			set;
		}

		public EDC_MaschineData()
		{
		}

		public EDC_MaschineData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strSelektivMaschinenWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}' order by {2}", "MachineType", "S", "MachineId");
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenNummerWhereStatementErstellen(string i_strMaschinenNummer)
		{
			return string.Format("Where {0} = '{1}'", "MachineNumber", i_strMaschinenNummer);
		}

		public static string FUN_strUpdateCodetabellenIdStatementErstellen(long i_i64MaschinenId, long i_i64CodeTabellenId)
		{
			return string.Format("Update {0} Set {1} = {2} where {3} = {4}", "Machines", "CodeTableId", i_i64CodeTabellenId, "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strUpdateDefaultProgrammIdStatementErstellen(long i_i64MaschinenId, long i_i64ProgrammId)
		{
			return string.Format("Update {0} Set {1} = {2} where {3} = {4}", "Machines", "DefaultProgramId", i_i64ProgrammId, "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strSelectCountMaschinenTypErstellen(string i_strMaschinentyp)
		{
			return string.Format("Select Count(*) from {0} Where {1} = '{2}'", "Machines", "MachineType", i_strMaschinentyp);
		}
	}
}
