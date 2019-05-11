using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.Common.Data.Model;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	[EDC_TabellenInformation("PRO_strTabellenname", PRO_blnNameIstProperty = true, PRO_strTablespace = "ess5_protocol")]
	public class EDC_LoetprotokollData
	{
		public const string gC_strTabellenName = "ProtocolData";

		public const string gC_strSpalteProtokollId = "ProtocolId";

		private IEnumerable<EDC_DynamischeSpalte> m_lstDynamischeSpalten = new List<EDC_DynamischeSpalte>();

		public string PRO_strTabellenname => string.Format("{0}_MA{1}", "ProtocolData", PRO_i64MaschinenId);

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

		[EDC_SpaltenInformation(PRO_blnIsDynamischeSpalte = true)]
		public IEnumerable<EDC_DynamischeSpalte> PRO_lstDynamischeSpalten
		{
			get
			{
				return m_lstDynamischeSpalten;
			}
			set
			{
				m_lstDynamischeSpalten = value;
			}
		}

		public EDC_LoetprotokollData()
		{
		}

		public EDC_LoetprotokollData(long i_i64MaschinenId)
		{
			PRO_i64MaschinenId = i_i64MaschinenId;
		}

		public EDC_LoetprotokollData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strProtokollIdWhereStatement(long i_i64ProtokollId)
		{
			return string.Format("Where {0} = {1}", "ProtocolId", i_i64ProtokollId);
		}
	}
}
