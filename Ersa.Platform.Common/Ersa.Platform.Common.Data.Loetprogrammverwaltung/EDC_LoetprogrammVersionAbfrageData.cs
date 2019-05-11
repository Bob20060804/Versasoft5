using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_LoetprogrammVersionAbfrageData : EDC_LoetprogrammVersionData
	{
		private const string mC_strSpalteBenutzername = "Username";

		private const string mC_strSpalteIstValide = "IsValid";

		private const string mC_strSpalteMaschineId = "MachineId";

		public string PRO_strAbfrageStatement => string.Format("SELECT {0}.*, ", "ProgramHistory") + string.Format("{0}.{1} AS {2}, ", "Users", "Username", "Username") + string.Format("{0}.{1} AS {2}, ", "SolderingVersionValid", "MachineId", "MachineId") + string.Format("case when {0}.{1} is null Then '1' else  {2}.{3} end AS {4}", "SolderingVersionValid", "IsValid", "SolderingVersionValid", "IsValid", "IsValid") + string.Format(" FROM {0} ", "ProgramHistory") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "Users", "Users", "UserId", "ProgramHistory", "ChangeUser") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "SolderingVersionValid", "SolderingVersionValid", "HistoryId", "ProgramHistory", "HistoryId");

		[EDC_SpaltenInformation("Username")]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsValid")]
		public bool PRO_blnValide
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId")]
		public long? PRO_i64MaschinenId
		{
			get;
			set;
		}

		public EDC_LoetprogrammVersionAbfrageData()
		{
		}

		public EDC_LoetprogrammVersionAbfrageData(string i_strWhereStatement)
			: base(i_strWhereStatement)
		{
		}
	}
}
