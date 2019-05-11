using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	[EDC_TabellenInformation("MachineSettings", PRO_strTablespace = "ess5_production")]
	public class EDC_MaschinenEinstellungenData
	{
		public const string gC_strTabellenName = "MachineSettings";

		public const string gC_strSpalteMemoWert = "MemoValue";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteSettingTyp = "SettingType";

		private const string mC_strSpalteSettingIndex = "SettingIndex";

		private const string mC_strSpalteLongWert = "LongValue";

		private const string mC_strSpalteTextWert = "TextValue";

		private const string mC_strSpalteRealWert = "RealValue";

		private const string mC_strSpalteArrayWert = "ArrayValue";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
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

		[EDC_SpaltenInformation("SettingType", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32SettingType
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("SettingIndex", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public int PRO_i32SettingIndex
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LongValue")]
		public long PRO_i64LongWert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TextValue", PRO_i32Length = 500)]
		public string PRO_strTextWert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("RealValue")]
		public float PRO_sngRealWert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ArrayValue")]
		public byte[] PRO_bytArrayWert
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MemoValue")]
		public string PRO_strMemoWert
		{
			get;
			set;
		}

		public ENUM_EinstellungsTyp PRO_enmSettingsTyp
		{
			get
			{
				return (ENUM_EinstellungsTyp)PRO_i32SettingType;
			}
			set
			{
				PRO_i32SettingType = (int)value;
			}
		}

		public EDC_MaschinenEinstellungenData()
		{
		}

		public EDC_MaschinenEinstellungenData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenIdMitTypWhereStatementErstellen(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmSettingsTyp)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "MachineId", i_i64MaschinenId, "SettingType", (int)i_enmSettingsTyp);
		}

		public static string FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmSettingsTyp, int i_i32SettingsIndex)
		{
			return string.Format("Where {0} = {1} and {2} = {3} and {4} = {5}", "MachineId", i_i64MaschinenId, "SettingType", (int)i_enmSettingsTyp, "SettingIndex", i_i32SettingsIndex);
		}

		public static string FUN_strLoescheEinstellungStatementErstellen(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmSettingsTyp)
		{
			return string.Format("Delete from {0} {1}", "MachineSettings", FUN_strMaschinenIdMitTypWhereStatementErstellen(i_i64MaschinenId, i_enmSettingsTyp));
		}

		public static string FUN_strLoescheEinstellungStatementErstellen(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmSettingsTyp, int i_i32SettingsIndex)
		{
			return string.Format("Delete from {0} {1}", "MachineSettings", FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(i_i64MaschinenId, i_enmSettingsTyp, i_i32SettingsIndex));
		}
	}
}
