using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Betriebsmittel
{
	[EDC_TabellenInformation("OperatingMaterial", PRO_strTablespace = "ess5_production")]
	public class EDC_BetriebsmittelData
	{
		public const string gC_strSpalteGeloescht = "Deleted";

		public const string gC_strTabellenName = "OperatingMaterial";

		private const string mC_strSpalteBetriebsmittelId = "MaterialId";

		private const string mC_strSpalteTyp = "Type";

		private const string mC_strSpalteName = "Name";

		private const string mC_strSpalteSpezifikation = "Specification";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MaterialId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64BetriebsmittelId
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

		[EDC_SpaltenInformation("Name", PRO_i32Length = 100, PRO_blnIsRequired = true)]
		public string PRO_strName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Specification", PRO_i32Length = 200)]
		public string PRO_strSpezifikation
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

		public ENUM_BetriebsmittelTyp PRO_enmTyp
		{
			get
			{
				return (ENUM_BetriebsmittelTyp)PRO_i32Typ;
			}
			set
			{
				PRO_i32Typ = (int)value;
			}
		}

		public EDC_BetriebsmittelData()
		{
		}

		public EDC_BetriebsmittelData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHoleBetriebsmittelTypWhereStatement(int i_i32BetriebsmaterialTyp)
		{
			return string.Format("Where {0} = {1} and {2} = '0'", "Type", i_i32BetriebsmaterialTyp, "Deleted");
		}

		public static string FUN_strBetriebsmittelIdWhereStatementErstellen(long i_i64BetriebsmaterialId)
		{
			return string.Format("Where {0} = {1}", "MaterialId", i_i64BetriebsmaterialId);
		}

		public static string FUN_strBetriebsmittelNameWhereStatementErstellen(string i_strBetriebsmaterialName)
		{
			return string.Format("Where {0} = '{1}'", "Name", i_strBetriebsmaterialName);
		}

		public static string FUN_strLoeschenUpdateStatementErstellen(long i_i64BetriebsmaterialId)
		{
			return string.Format("Update {0} Set {1} = '1' Where {2} = {3}", "OperatingMaterial", "Deleted", "MaterialId", i_i64BetriebsmaterialId);
		}
	}
}
