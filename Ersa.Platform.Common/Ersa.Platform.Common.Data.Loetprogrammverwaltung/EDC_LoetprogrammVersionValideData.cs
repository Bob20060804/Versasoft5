using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	[EDC_TabellenInformation("SolderingVersionValid", PRO_strTablespace = "ess5_programs")]
	public class EDC_LoetprogrammVersionValideData
	{
		public const string gC_strTabellenName = "SolderingVersionValid";

		public const string gC_strSpalteVersionsId = "HistoryId";

		public const string gC_strSpalteMaschineId = "MachineId";

		public const string gC_strSpalteIstValide = "IsValid";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("HistoryId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64VersionsId
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

		[EDC_SpaltenInformation("IsValid")]
		public bool PRO_blnValide
		{
			get;
			set;
		}

		public EDC_LoetprogrammVersionValideData()
		{
		}

		public EDC_LoetprogrammVersionValideData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strVersionsIdUndMaschinenIdWhereStatementErstellen(long i_i64VersionsId, long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} and {2} = {3}", "HistoryId", i_i64VersionsId, "MachineId", i_i64MaschinenId);
		}
	}
}
