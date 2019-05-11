using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Duesentabelle
{
	[EDC_TabellenInformation("Nozzles", PRO_strTablespace = "ess5_programs")]
	public class EDC_DuesenData
	{
		public const string gC_strTabellenName = "Nozzles";

		public const string gC_strSpalteDuesenId = "NozzleId";

		public const string gC_strSpalteGeometrieId = "GeomertyId";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteAggregatName = "UnitName";

		private const string mC_strSpalteDuesenInhalt = "NozzleContent";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NozzleId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64DuesenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GeomertyId", PRO_blnIsRequired = true)]
		public long PRO_i64GeometrieId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UnitName", PRO_blnIsRequired = true, PRO_i32Length = 6)]
		public string PRO_strAggregatName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NozzleContent")]
		public string PRO_strDuesenInhalt
		{
			get;
			set;
		}

		public EDC_DuesenData()
		{
		}

		public EDC_DuesenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleMaschinenIdWhereStatement(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strHoleDuesenIdWhereStatement(long i_i64DuesenId)
		{
			return string.Format("Where {0} = {1}", "NozzleId", i_i64DuesenId);
		}

		public static string FUN_strAggregatNameWhereStatementErstellen(string i_strAggregatName, long i_i64MaschinenId)
		{
			return string.Format("{0} and {1} = '{2}'", FUN_strHoleMaschinenIdWhereStatement(i_i64MaschinenId), "UnitName", i_strAggregatName);
		}

		public static string FUN_strAggregatTypWhereStatementErstellen(string i_strAggregattyp, long i_i64MaschinenId)
		{
			return string.Format("{0} and {1} like '{2}%'", FUN_strHoleMaschinenIdWhereStatement(i_i64MaschinenId), "UnitName", i_strAggregattyp);
		}

		public static string FUN_strErstelleMaschinenIdLoeschenStatement(long i_i64MaschinenId)
		{
			return string.Format("Delete from {0} {1}", "Nozzles", FUN_strHoleMaschinenIdWhereStatement(i_i64MaschinenId));
		}

		public static string FUN_strErstelleDefaultGeometrieIdUpdateStatement(long i_i64NeuerWert)
		{
			return string.Format("Update {0} Set {1} = {2}", "Nozzles", "GeomertyId", i_i64NeuerWert);
		}

		public static string FUN_strGeometrieIdSubselectStatement(long i_i64MaschinenId)
		{
			return string.Format("Select distinct {0} from {1} Where {2} = {3}", "GeomertyId", "Nozzles", "MachineId", i_i64MaschinenId);
		}
	}
}
