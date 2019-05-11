using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("MachineOperatingDataValue", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenBetriebsDatenWerteData
	{
		public const string gC_strTabellenName = "MachineOperatingDataValue";

		public const string gC_strSpalteBetriebsDatenId = "OperatingDataId";

		public const string gC_strSpalteDatenId = "DataId";

		public const string gC_strSpalteNamenKey = "NameKey";

		public const string gC_strSpalteZeitspanne = "TimeSpan";

		public const string gC_strSpalteAnteil = "Percentage";

		public const string gC_strSpalteWert = "Value";

		public const string gC_strSpalteSingleWert = "RealValue";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("OperatingDataId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64BetriebsDatenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DataId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 100)]
		public string PRO_strDatenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NameKey", PRO_i32Length = 10)]
		public string PRO_strNameKey
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TimeSpan")]
		public long PRO_i64Zeitspanne
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Percentage")]
		public long PRO_i64Anteil
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Value")]
		public long PRO_i64Wert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RealValue")]
		public float PRO_sngRealWert
		{
			get;
			set;
		}

		public EDC_MaschinenBetriebsDatenWerteData()
		{
		}

		public EDC_MaschinenBetriebsDatenWerteData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBetriebsdatenIdWhereStatementErstellen(long i_i64BetriebsDatenId)
		{
			return string.Format("Where {0} = {1}", "OperatingDataId", i_i64BetriebsDatenId);
		}

		public static string FUN_strLoescheUngueltigeBetriebsdatenStatementErstellen()
		{
			return string.Format("DELETE FROM {0} WHERE({1} NOT IN (SELECT {1} FROM {2}))", "MachineOperatingDataValue", "OperatingDataId", "MachineOperatingDataHead");
		}

		public static string FUN_strBetriebsdatenNullenStatementErstellen(List<long> i_lstBetriebsdatenIds)
		{
			return string.Format("Update {0} Set {1} = 0, {2} = 0, {3} = 0, {4} = 0 Where {5} in ({6})", "MachineOperatingDataValue", "TimeSpan", "Percentage", "Value", "RealValue", "OperatingDataId", string.Join(",", i_lstBetriebsdatenIds));
		}
	}
}
