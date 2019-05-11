using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Data.Cad
{
	[EDC_TabellenInformation("CadForbiddenAreas", PRO_strTablespace = "ess5_cad")]
	public class EDC_CadVerbotenerBereichData
	{
		public const string gC_strTabellenName = "CadForbiddenAreas";

		public const string gC_strSpalteVersionsId = "HistoryId";

		private const string mC_strSpalteBereichId = "AreaId";

		private const string mC_strSpalteStartpositionX = "StartPointX";

		private const string mC_strSpalteStartpositionY = "StartPointY";

		private const string mC_strSpalteEndpositionX = "EndPointX";

		private const string mC_strSpalteEndpositionY = "EndPointY";

		private const string mC_strSpalteBereichHoehe = "Height";

		private const string mC_strSpalteUeberquerenErlaubt = "CrossingAllowed";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("AreaId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64BereichId
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

		[EDC_SpaltenInformation("StartPointX")]
		public float PRO_sngStartpunktX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("StartPointY")]
		public float PRO_sngStartpunktY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EndPointX")]
		public float PRO_sngEndpunktX
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("EndPointY")]
		public float PRO_sngEndpunktY
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Height")]
		public float PRO_sngHoehe
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CrossingAllowed")]
		public bool PRO_blnUeberquerenErlaubt
		{
			get;
			set;
		}

		public EDC_CadVerbotenerBereichData()
		{
		}

		public EDC_CadVerbotenerBereichData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("Where {0} = {1}", "HistoryId", i_i64VersionsId);
		}

		public static string FUN_strHistoryIdWhereStatementErstellen(IEnumerable<long> i_lstVersionsIds)
		{
			return string.Format("Where {0} in ({1})", "HistoryId", string.Join(",", (from i_i64VersionsId in i_lstVersionsIds
			select i_i64VersionsId.ToString()).ToArray()));
		}

		public static string FUN_strLoescheHistoryIdStatementErstellen(long i_i64VersionsId)
		{
			return string.Format("DELETE FROM {0} {1}", "CadForbiddenAreas", FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId));
		}
	}
}
