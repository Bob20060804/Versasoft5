using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.CodeCache
{
	[EDC_TabellenInformation("LinkCache", PRO_strTablespace = "ess5_production")]
	public class EDC_LinkCacheData
	{
		public const string gC_strTabellenName = "LinkCache";

		private const string mC_strSpalteLinkId = "LinkId";

		public const string gC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteHash = "Hash";

		private const string mC_strSpalteVerwendung = "Usage";

		private const string mC_strSpalteLink = "Link";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("LinkId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64LinkId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Hash", PRO_blnIsRequired = true, PRO_i32Length = 32)]
		public string PRO_strHash
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Usage", PRO_blnIsRequired = true)]
		public int PRO_i32LinkVerwendung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Link", PRO_blnIsRequired = true, PRO_i32Length = 1000)]
		public string PRO_strLink
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

		public ENUM_LinkVerwendung PRO_enmLinkVerwendung
		{
			get
			{
				return (ENUM_LinkVerwendung)PRO_i32LinkVerwendung;
			}
			set
			{
				PRO_i32LinkVerwendung = (int)value;
			}
		}

		public EDC_LinkCacheData()
		{
		}

		public EDC_LinkCacheData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHashWhereStatementErstellen(string i_strHash)
		{
			return string.Format("Where {0} = '{1}' order by {2}", "Hash", i_strHash, "LinkId");
		}

		public static string FUN_strHashUndVerwendungWhereStatementErstellen(string i_strHash, ENUM_LinkVerwendung i_enmVerwendung)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}' order by {4}", "Hash", i_strHash, "Usage", (int)i_enmVerwendung, "LinkId");
		}
	}
}
