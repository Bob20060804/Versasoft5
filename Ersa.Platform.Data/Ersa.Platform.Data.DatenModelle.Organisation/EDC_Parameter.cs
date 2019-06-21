using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Data.DatenModelle.Organisation
{
	[EDC_TabellenInformation("Parameter")]
	public class EDC_Parameter
	{
		public const string mC_strTabellenName = "Parameter";

		public const string mC_strSpalteParameter = "Parameter";

		public const string mC_strSpalteInhalt = "Content";

		public const string mC_strSpalteWert = "Value";

		public const string mC_strDatenbankversionParameter = "database version";

		public const string mC_strSprachenXmlVersion = "sprachen.xml version";

		public const string mC_strLetztesBackup = "last database backup";

		public const string mC_strLoetprogrammVariablenVersion = "soldering program variables version";

		public const string mC_strProzessschreiberVariablenVersion = "recorder variables version";

		public const string mC_strLoetprotokollVariablenVersion = "soldering protocol variables version";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 40)]
		public string PRO_strParameter
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Content", PRO_i32Length = 250)]
		public string PRO_strInhalt
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

		public EDC_Parameter()
		{
		}

		public EDC_Parameter(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static EDC_Parameter FUN_edcErtselleVersionsUpdateParameter(long i_i64Version)
		{
			return new EDC_Parameter(FUN_strDatenbankVersionWhereStatementErstellen())
			{
				PRO_strParameter = "database version",
				PRO_i64Wert = i_i64Version
			};
		}

		public static string FUN_strDatenbankVersionWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "database version");
		}

		public static string FUN_strSprachenXmlVersionWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "sprachen.xml version");
		}

		public static string FUN_strLetztesBackupWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "last database backup");
		}

		public static string FUN_strLoetprogrammVariablenVersionWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "soldering program variables version");
		}

		public static string FUN_strProzessschreiberVariablenVersionWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "recorder variables version");
		}

		public static string FUN_strLoetprotokollVariablenVersionWhereStatementErstellen()
		{
			return string.Format("Where {0} = '{1}'", "Parameter", "soldering protocol variables version");
		}
	}
}
