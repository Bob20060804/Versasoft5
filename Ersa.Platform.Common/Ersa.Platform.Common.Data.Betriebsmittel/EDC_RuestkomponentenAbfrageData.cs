using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Betriebsmittel
{
	[EDC_TabellenInformation("PRO_strAbfrageStatement", true, PRO_blnQueryIstProperty = true)]
	public class EDC_RuestkomponentenAbfrageData : EDC_RuestkomponentenData
	{
		private const string mC_strSpalteRuestwerkzeugId = "EquipmentToolId";

		private const string mC_strSpalteIdentifikation = "Identification";

		public string PRO_strAbfrageStatement => string.Format("SELECT {0}.*, ", "Equipment") + string.Format("{0}.{1} AS {2},", "EquipmentTools", "EquipmentToolId", "EquipmentToolId") + string.Format("{0}.{1} AS {2}", "EquipmentTools", "Identification", "Identification") + string.Format(" FROM {0} ", "Equipment") + string.Format(" LEFT OUTER JOIN {0} ON {1}.{2} = {3}.{4} ", "EquipmentTools", "EquipmentTools", "EquipmentId", "Equipment", "EquipmentId");

		[EDC_SpaltenInformation("EquipmentToolId")]
		public long PRO_i64RuestwerkzeugId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Identification")]
		public string PRO_strIdentifikation
		{
			get;
			set;
		}

		public EDC_RuestkomponentenAbfrageData()
		{
		}

		public EDC_RuestkomponentenAbfrageData(string i_strWhereStatement)
			: base(i_strWhereStatement)
		{
		}

		public static string FUN_strRuestkomponentenTypAbfrageWhereStaement(long i_i64MachineGroupId, ENUM_RuestkomponentenTyp i_enmTyp)
		{
			return string.Format(" Where {0}.{1} = {2} and {3} = {4} and {5}.{6} = '0' and {7}.{8} = '0'", "Equipment", "Type", (int)i_enmTyp, "MachineGroupId", i_i64MachineGroupId, "Equipment", "Deleted", "EquipmentTools", "Deleted");
		}

		public static string FUN_strRuestkomponentenNameAbfrageWhereStaement(string i_strName)
		{
			return string.Format(" Where {0}.{1} = '{2}' and {3}.{4} = '0' and {5}.{6} = '0'", "Equipment", "Name", i_strName, "Equipment", "Deleted", "EquipmentTools", "Deleted");
		}

		public static string FUN_strRuestwerkzeugIdentifikationWhereStaement(ENUM_RuestkomponentenTyp i_enmTyp, string i_strIdentifikation)
		{
			return string.Format(" Where {0}.{1} = {2} and {3}.{4} = '{5}' and {6}.{7} = '0' and {8}.{9} = '0'", "Equipment", "Type", (int)i_enmTyp, "EquipmentTools", "Identification", i_strIdentifikation, "Equipment", "Deleted", "EquipmentTools", "Deleted");
		}
	}
}
