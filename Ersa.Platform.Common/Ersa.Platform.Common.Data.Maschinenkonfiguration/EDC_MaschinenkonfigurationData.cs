using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Maschinenkonfiguration
{
	[EDC_TabellenInformation("MachineConfigurations", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenkonfigurationData
	{
		public const string mC_strTabellenName = "MachineConfigurations";

		public const string mC_strSpalteKonfigurationsId = "ConfigurationId";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteBeschreibung = "Description";

		private const string mC_strSpalteDateiname = "FileName";

		private const string mC_strSpalteKonfiguration = "Configuration";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		private const string mC_strSpalteAngelegtVon = "CreationUser";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ConfigurationId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64KonfigurationsId
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

		[EDC_SpaltenInformation("Description", PRO_i32Length = 250)]
		public string PRO_strBeschreibung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("FileName", PRO_blnIsRequired = true)]
		public string PRO_strDateiname
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Configuration", PRO_blnIsRequired = true)]
		public string PRO_strKonfigurationsDatei
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate", PRO_blnIsRequired = true)]
		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationUser", PRO_blnIsRequired = true)]
		public long PRO_dtmAngelegtVon
		{
			get;
			set;
		}

		public EDC_MaschinenkonfigurationData()
		{
		}

		public EDC_MaschinenkonfigurationData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strKonfigurationsIdWhereStatementErstellen(long i_i64KonfigurationsId)
		{
			return string.Format("Where {0} = {1}", "ConfigurationId", i_i64KonfigurationsId);
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} order by {2} desc", "MachineId", i_i64MaschinenId, "CreationDate");
		}

		public static string FUN_strSelectMaxKonfigurationsId(long i_i64MaschinenId)
		{
			return string.Format("Select max({0}) From {1} Where {2} = {3} ", "ConfigurationId", "MachineConfigurations", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strLoescheKonfigurationsId(long i_i64KonfigurationsId)
		{
			return string.Format("Delete from {0} {1}", "MachineConfigurations", FUN_strKonfigurationsIdWhereStatementErstellen(i_i64KonfigurationsId));
		}
	}
}
