using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Betriebsmittel
{
	[EDC_TabellenInformation("EquipmentTools", PRO_strTablespace = "ess5_production")]
	public class EDC_RuestwerkzeugeData
	{
		public const string gC_strTabellenName = "EquipmentTools";

		public const string gC_strSpalteRuestwerkzeugId = "EquipmentToolId";

		public const string gC_strSpalteRuestkomponentenId = "EquipmentId";

		public const string gC_strSpalteIdentifikation = "Identification";

		public const string gC_strSpalteGeloescht = "Deleted";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EquipmentToolId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64RuestwerkzeugId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EquipmentId", PRO_blnIsRequired = true)]
		public long PRO_i64RuestkomponentenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Identification", PRO_i32Length = 200, PRO_blnIsRequired = true)]
		public string PRO_strIdentifikation
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

		public EDC_RuestwerkzeugeData()
		{
		}

		public EDC_RuestwerkzeugeData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strRuestwerkzeugIdWhereStatementErstellen(long i_i64RuestwerkzeugId)
		{
			return string.Format("Where {0} = {1}", "EquipmentToolId", i_i64RuestwerkzeugId);
		}

		public static string FUN_strRuestkomponentenIdWhereStatementErstellen(long i_i64RuestkomponentenId)
		{
			return string.Format("Where {0} = {1} and {2} = '0'", "EquipmentId", i_i64RuestkomponentenId, "Deleted");
		}

		public static string FUN_strLoeschenUpdateStatementErstellen(long i_i64RuestwerkzeugId)
		{
			return string.Format("Update {0} Set {1} = '1' Where {2} = {3}", "EquipmentTools", "Deleted", "EquipmentToolId", i_i64RuestwerkzeugId);
		}

		public static string FUN_strLoeschenUpdateStatementFuerKomponentenIdErstellen(long i_i64RuestkomponentenId)
		{
			return string.Format("Update {0} Set {1} = '1' Where {2} = {3}", "EquipmentTools", "Deleted", "EquipmentId", i_i64RuestkomponentenId);
		}
	}
}
