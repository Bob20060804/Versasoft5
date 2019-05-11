using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.CodeCache
{
	[EDC_TabellenInformation("CodeCache", PRO_strTablespace = "ess5_programs")]
	public class EDC_CodeCacheEintragData
	{
		public const string gC_strTabellenName = "CodeCache";

		public const string gC_strSpalteBedeutung = "Meaning";

		public const string gC_strSpalteMaschinenId = "MachineId";

		public const string gC_strSpalteArrayIndex = "ArrayIndex";

		private const string mC_strSpalteCacheId = "CacheId";

		private const string mC_strSpalteHash = "Hash";

		private const string mC_strSpalteVerwendung = "Usage";

		private const string mC_strSpalteCode = "Code";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CacheId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64CacheId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId")]
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

		[EDC_SpaltenInformation("Usage", PRO_blnIsRequired = true, PRO_i32Length = 100)]
		public string PRO_strVerwendung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Meaning", PRO_i32Length = 100)]
		public string PRO_strBedeutung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Code", PRO_blnIsRequired = true, PRO_i32Length = 400)]
		public string PRO_strCode
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

		[EDC_SpaltenInformation("ArrayIndex")]
		public long PRO_i64ArrayIndex
		{
			get;
			set;
		}

		public EDC_CodeCacheEintragData()
		{
		}

		public EDC_CodeCacheEintragData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strHashWhereStatementErstellen(string i_strHash)
		{
			return string.Format("Where {0} = '{1}' order by {2}", "Hash", i_strHash, "CacheId");
		}

		public static string FUN_strHashUndBedeutungWhereStatementErstellen(string i_strHash, string i_strBedeutung)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}' order by {4}", "Hash", i_strHash, "Meaning", i_strBedeutung, "CacheId");
		}

		public static string FUN_strHashUndVerwendungWhereStatementErstellen(string i_strHash, string i_strVerwendung)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}' order by {4}", "Hash", i_strHash, "Usage", i_strVerwendung, "CacheId");
		}

		public static string FUN_strHashUndVerwendungUndBedeutungWhereStatementErstellen(string i_strHash, string i_strVerwendung, string i_strBedeutung)
		{
			return string.Format("Where {0} = '{1}' And {2} = '{3}' And {4} = '{5}' order by {6}", "Hash", i_strHash, "Usage", i_strVerwendung, "Meaning", i_strBedeutung, "CacheId");
		}
	}
}
