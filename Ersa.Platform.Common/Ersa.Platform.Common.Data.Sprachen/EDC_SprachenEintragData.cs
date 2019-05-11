using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Common.Data.Sprachen
{
	[EDC_TabellenInformation("LanguageEntries")]
	public class EDC_SprachenEintragData
	{
		public const string mC_strTabellenName = "LanguageEntries";

		private const string mC_strSpalteSprache = "Language";

		private const string mC_strSpalteKey = "TextKey";

		private const string mC_strSpalteText = "Text";

		[EDC_SpaltenInformation("Language", PRO_blnIsPrimary = true, PRO_i32Length = 10, PRO_blnIsRequired = true)]
		public string PRO_strSprache
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("TextKey", PRO_blnIsPrimary = true, PRO_i32Length = 10, PRO_blnIsRequired = true)]
		public string PRO_strKey
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Text", PRO_i32Length = 400, PRO_blnIsRequired = true)]
		public string PRO_strText
		{
			get;
			set;
		}
	}
}
