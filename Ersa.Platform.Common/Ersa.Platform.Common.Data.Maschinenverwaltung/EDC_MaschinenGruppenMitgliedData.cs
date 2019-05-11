using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("MachineGroupMembers", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenGruppenMitgliedData
	{
		public const string gC_strTabellenName = "MachineGroupMembers";

		public const string gC_strSpalteGruppenId = "GroupId";

		public const string gC_strSpalteMaschinenId = "MachineId";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("GroupId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64GruppenId
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

		public EDC_MaschinenGruppenMitgliedData()
		{
		}

		public EDC_MaschinenGruppenMitgliedData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strBestimmtesMitgliedWhereStatementErstellen(long i_i64GruppenId, long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "GroupId", i_i64GruppenId, "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strGruppeZuMaschinenWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strHoleGruppenIdVonMaschinenIdSunbselect(long i_i64MaschinenId)
		{
			return string.Format("Select {0}.{1} from {2} Where {3}.{4} = {5}", "MachineGroupMembers", "GroupId", "MachineGroupMembers", "MachineGroupMembers", "MachineId", i_i64MaschinenId);
		}
	}
}
