using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Betriebsmittel
{
	[EDC_TabellenInformation("Equipment", PRO_strTablespace = "ess5_production")]
	public class EDC_RuestkomponentenData
	{
		public const string gC_strTabellenName = "Equipment";

		public const string gC_strSpalteRuestkomponentenId = "EquipmentId";

		public const string gC_strSpalteMachineGruppenId = "MachineGroupId";

		public const string gC_strSpalteName = "Name";

		public const string gC_strSpalteTyp = "Type";

		public const string gC_strSpalteGeloescht = "Deleted";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EquipmentId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64RuestkomponentenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineGroupId", PRO_blnIsRequired = true)]
		public long PRO_i64MachinenGruppenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Name", PRO_i32Length = 100, PRO_blnIsRequired = true)]
		public string PRO_strName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Type", PRO_blnIsRequired = true)]
		public int PRO_i32Typ
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Deleted")]
		public bool PRO_blnGeloescht
		{
			get;
			set;
		}

		public ENUM_RuestkomponentenTyp PRO_enmTyp
		{
			get
			{
				return (ENUM_RuestkomponentenTyp)PRO_i32Typ;
			}
			set
			{
				PRO_i32Typ = (int)value;
			}
		}

		public EDC_RuestkomponentenData()
		{
		}

		public EDC_RuestkomponentenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleRuestkomponentenFuerTypWhereStatement(long i_i64MachineGroupId, int i_i32RuestkomponentenTyp)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = '0'", "Type", i_i32RuestkomponentenTyp, "MachineGroupId", i_i64MachineGroupId, "Deleted");
		}

		public static string FUN_strHoleAuchGeloeschteRuestkomponentenFuerTypWhereStatement(long i_i64MachineGroupId, int i_i32RuestkomponentenTyp)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "Type", i_i32RuestkomponentenTyp, "MachineGroupId", i_i64MachineGroupId);
		}

		public static string FUN_strRuestkomponentenIdWhereStatementErstellen(long i_i64RuestkomponentenId)
		{
			return string.Format("Where {0} = {1}", "EquipmentId", i_i64RuestkomponentenId);
		}

		public static string FUN_strRuestkomponentenNameWhereStatementErstellen(long i_i64MachineGroupId, string i_strRuestkomponentenName)
		{
			return string.Format("Where {0} = '{1}' and {2} = {3} and {4} = '0'", "Name", i_strRuestkomponentenName, "MachineGroupId", i_i64MachineGroupId, "Deleted");
		}

		public static string FUN_strLoeschenUpdateStatementErstellen(long i_i64RuestkomponentenId)
		{
			return string.Format("Update {0} Set {1} = '1' Where {2} = {3}", "Equipment", "Deleted", "EquipmentId", i_i64RuestkomponentenId);
		}
	}
}
