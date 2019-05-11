using Ersa.Global.Common.Data.Attributes;
using Ersa.Platform.Common.LeseSchreibGeraete;

namespace Ersa.Platform.Common.Data.LeseSchreibgeraete
{
	[EDC_TabellenInformation("CodeConfigurations", PRO_strTablespace = "ess5_production")]
	public class EDC_CodeKonfigData
	{
		public const string gC_strTabellenName = "CodeConfigurations";

		public const string gC_strSpalteAlbAusElb = "AlbFromElb";

		private const string mC_strSpalteArrayIndex = "ArrayIndex";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteIstKonfiguriert = "IsConfigured";

		private const string mC_strSpalteWirdVerwendet = "IsActive";

		private const string mC_strSpalteOrt = "Location";

		private const string mC_strSpalteSpur = "Track";

		private const string mC_strSpalteFunktion = "CodeFunction";

		private const string mC_strSpalteAlb = "UseAlb";

		private const string mC_strSpalteElb = "UseElb";

		private const string mC_strSpalteTimeout = "Timeout";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ArrayIndex", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ArrayIndex
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

		[EDC_SpaltenInformation("IsConfigured")]
		public bool PRO_blnIstKonfiguriert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsActive")]
		public bool PRO_blnWirdVerwendet
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Location")]
		public long PRO_i64Ort
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Track")]
		public long PRO_i64Spur
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CodeFunction")]
		public long PRO_i64Verwendung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("AlbFromElb")]
		public bool PRO_blnAlbAusElb
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UseAlb")]
		public bool PRO_blnAlbVerwenden
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UseElb")]
		public bool PRO_blnElbVerwenden
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Timeout")]
		public long PRO_i64Timeout
		{
			get;
			set;
		}

		public ENUM_LsgOrt PRO_enmOrt
		{
			get
			{
				return (ENUM_LsgOrt)PRO_i64Ort;
			}
			set
			{
				PRO_i64Ort = (long)value;
			}
		}

		public ENUM_LsgSpur PRO_enmSpur
		{
			get
			{
				return (ENUM_LsgSpur)PRO_i64Spur;
			}
			set
			{
				PRO_i64Spur = (long)value;
			}
		}

		public ENUM_LsgVerwendung PRO_enmCodeVerwendung
		{
			get
			{
				return (ENUM_LsgVerwendung)PRO_i64Verwendung;
			}
			set
			{
				PRO_i64Verwendung = (long)value;
			}
		}

		public EDC_CodeKonfigData()
		{
		}

		public EDC_CodeKonfigData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenIdUndAktivWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} and {2} = '1' and {3} = '1'", "MachineId", i_i64MaschinenId, "IsConfigured", "IsActive");
		}

		public static string FUN_strMaschinenIdUndArrayIndexWhereStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MachineId", i_i64MaschinenId, "ArrayIndex", i_i64ArrayIndex);
		}

		public static string FUN_strMaschinenIdUndVerwendungWhereStatementErstellen(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MachineId", i_i64MaschinenId, "CodeFunction", (int)i_enmVerwendung);
		}

		public static string FUN_strNurAktiveMaschinenIdUndVerwendungWhereStatementErstellen(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = '1' and {5} = '1'", "MachineId", i_i64MaschinenId, "CodeFunction", (int)i_enmVerwendung, "IsConfigured", "IsActive");
		}

		public static string FUN_strMaschinenIdSelectCountStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodeConfigurations", FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId));
		}

		public static string FUN_strMaschinenIdUndArrayIndexSelectCountStatementErstellen(long i_i64MaschinenId, long i_i64ArrayIndex)
		{
			return string.Format("Select Count(*) from {0} {1}", "CodeConfigurations", FUN_strMaschinenIdUndArrayIndexWhereStatementErstellen(i_i64MaschinenId, i_i64ArrayIndex));
		}
	}
}
