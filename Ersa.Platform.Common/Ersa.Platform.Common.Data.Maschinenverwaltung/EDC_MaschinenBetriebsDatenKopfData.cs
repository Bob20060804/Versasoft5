using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("MachineOperatingDataHead", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenBetriebsDatenKopfData
	{
		public const string gC_strTabellenName = "MachineOperatingDataHead";

		public const string gC_strSpalteBetriebsDatenId = "OperatingDataId";

		public const string gC_strSpalteMaschinenId = "MachineId";

		public const string gC_strSpalteDatenTyp = "DataType";

		public const string gC_strSpalteErstelltAm = "CreationDate";

		private const string mC_strEndzeitpunktParameterName = "pEnde";

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

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true, PRO_blnIsNonUniqueIndex = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DataType", PRO_blnIsRequired = true)]
		public int PRO_i32DatenTyp
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_dtmErstellt
		{
			get;
			set;
		}

		public ENUM_BetriebsdatenTyp PRO_enmBetriebsdatenTyp
		{
			get
			{
				return (ENUM_BetriebsdatenTyp)PRO_i32DatenTyp;
			}
			set
			{
				PRO_i32DatenTyp = (int)value;
			}
		}

		public EDC_MaschinenBetriebsDatenKopfData()
		{
		}

		public EDC_MaschinenBetriebsDatenKopfData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strSelectMaxBetriebsdatenId(long i_i64MaschinenId, DateTime i_fdcBisDatum, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pEnde", i_fdcBisDatum);
			return string.Format("Select max({0}) From {1} Where {2} = {3} and {4} = {5} and {6} <= @{7}", "OperatingDataId", "MachineOperatingDataHead", "MachineId", i_i64MaschinenId, "DataType", (int)i_enmBetriebsdatenTyp, "CreationDate", "pEnde");
		}

		public static string FUN_strLoescheNichtEnthalteneIdsStatementErstellen(long i_i64MaschinenId, List<long> i_lstBetriebsdatenIds)
		{
			return string.Format("Delete From {0} Where {1} = {2} and {3} not in ({4})", "MachineOperatingDataHead", "MachineId", i_i64MaschinenId, "OperatingDataId", string.Join(",", i_lstBetriebsdatenIds));
		}

		public static string FUN_strSelectCountAufMaschinenIdStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select Count(*) from {0} Where {1} = {2}", "MachineOperatingDataHead", "MachineId", i_i64MaschinenId);
		}
	}
}
