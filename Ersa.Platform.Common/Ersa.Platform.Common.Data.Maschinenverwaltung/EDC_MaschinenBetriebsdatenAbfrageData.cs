using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_MaschinenBetriebsdatenAbfrageData : EDC_MaschinenBetriebsDatenWerteData
	{
		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteDatenTyp = "DataType";

		private const string mC_strSpalteErstelltAm = "CreationDate";

		private static readonly string ms_strFeldAuswahl = string.Format("SELECT {0}.{1} as {2}, ", "MachineOperatingDataHead", "MachineId", "MachineId") + string.Format("{0}.{1} as {2}, ", "MachineOperatingDataHead", "DataType", "DataType") + string.Format("{0}.{1} as {2}, ", "MachineOperatingDataHead", "CreationDate", "CreationDate") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "OperatingDataId") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "DataId") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "NameKey") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "TimeSpan") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "Percentage") + string.Format("{0}.{1}, ", "MachineOperatingDataValue", "Value") + string.Format("{0}.{1} ", "MachineOperatingDataValue", "RealValue");

		private static readonly string ms_strMaxTimestamp = string.Format("SELECT Max({0}.{1}) ", "MachineOperatingDataValue", "TimeSpan");

		private static readonly string ms_strTabellenJoin = string.Format("FROM {0} ", "MachineOperatingDataHead") + string.Format("INNER JOIN {0} ", "MachineOperatingDataValue") + string.Format("ON {0}.{1} = {2}.{3} ", "MachineOperatingDataHead", "OperatingDataId", "MachineOperatingDataValue", "OperatingDataId");

		public string PRO_strAbfrageStatement
		{
			get;
			private set;
		}

		[EDC_SpaltenInformation("MachineId")]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DataType")]
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

		public EDC_MaschinenBetriebsdatenAbfrageData()
		{
		}

		public EDC_MaschinenBetriebsdatenAbfrageData(string i_strWhereStatement)
			: base(i_strWhereStatement)
		{
		}

		public void SUB_ErstelleMaxBetriebszeitAbfrageFuerMaschine(long i_i64MaschinenId, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp)
		{
			PRO_strAbfrageStatement = $"{ms_strFeldAuswahl} {ms_strTabellenJoin} {FUN_strErstelleMaxZeitspanneSqlStatement(i_i64MaschinenId, i_enmBetriebsdatenTyp)}";
		}

		private static string FUN_strErstelleMaschinenIdUndDatentypWhereStatement(long i_i64MaschinenId, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp)
		{
			return string.Format("Where {0}.{1} = {2} ", "MachineOperatingDataHead", "MachineId", i_i64MaschinenId) + string.Format("and {0}.{1} = {2}", "MachineOperatingDataHead", "DataType", (int)i_enmBetriebsdatenTyp);
		}

		private static string FUN_strErstelleMaxZeitspanneSqlStatement(long i_i64MaschinenId, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp)
		{
			string arg = $"({ms_strMaxTimestamp} {ms_strTabellenJoin} {FUN_strErstelleMaschinenIdUndDatentypWhereStatement(i_i64MaschinenId, i_enmBetriebsdatenTyp)})";
			return string.Format("Where {0}.{1} = {2}", "MachineOperatingDataValue", "TimeSpan", arg);
		}
	}
}
