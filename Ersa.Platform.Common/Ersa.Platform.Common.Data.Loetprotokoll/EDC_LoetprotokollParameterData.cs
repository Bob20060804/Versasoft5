using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	[EDC_TabellenInformation("PRO_strTabellenname", PRO_blnNameIstProperty = true, PRO_strTablespace = "ess5_protocol")]
	public class EDC_LoetprotokollParameterData
	{
		public const string gC_strTabellenName = "ProtocolParameter";

		public const string gC_strSpalteProtokollId = "ProtocolId";

		public const string gC_strSpalteParameter = "Parameter";

		public const string gC_strSpalteInhalt = "Content";

		public const string gC_strSpalteTyp = "Type";

		public string PRO_strTabellenname => string.Format("{0}_MA{1}", "ProtocolParameter", PRO_i64MaschinenId);

		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProtocolId", PRO_blnIsRequired = true, PRO_blnIsPrimary = true)]
		public long PRO_i64ProtokollId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Parameter", PRO_blnIsRequired = true, PRO_blnIsPrimary = true, PRO_i32Length = 100, PRO_blnIsNonUniqueIndex = true)]
		public string PRO_strParameter
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Content", PRO_i32Length = 500)]
		public string PRO_strInhalt
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Type", PRO_i32Length = 1)]
		public string PRO_strTyp
		{
			get;
			set;
		}

		public EDC_LoetprotokollParameterData()
		{
		}

		public EDC_LoetprotokollParameterData(long i_i64MaschinenId)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
		}

		public EDC_LoetprotokollParameterData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProtokollIdWhereStatementErstellen(long i_i64ProtokollId)
		{
			return string.Format("Where {0} = {1}", "ProtocolId", i_i64ProtokollId);
		}

		public static string FUN_strProtokollIdUndTypWhereStatementErstellen(long i_i64ProtokollId, string i_strTyp)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "ProtocolId", i_i64ProtokollId, "Type", i_strTyp);
		}
	}
}
