using System.Collections.Generic;

namespace Ersa.Platform.Common.LeseSchreibGeraete.Extensions
{
	public static class EDC_CodeMitVerwendungUndBedeutungExtensions
	{
		public static Dictionary<string, List<string>> FUN_dicCodesAlsDictionary(this IList<EDC_CodeMitVerwendungUndBedeutung> i_lstGeleseneCodes)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			foreach (EDC_CodeMitVerwendungUndBedeutung i_lstGeleseneCode in i_lstGeleseneCodes)
			{
				string pRO_strCode = i_lstGeleseneCode.PRO_strCode;
				string key = i_lstGeleseneCode.PRO_enmVerwendung.ToString();
				if (dictionary.TryGetValue(key, out List<string> value))
				{
					if (!value.Contains(pRO_strCode))
					{
						value.Add(pRO_strCode);
					}
				}
				else
				{
					dictionary.Add(key, new List<string>
					{
						pRO_strCode
					});
				}
				string key2 = i_lstGeleseneCode.PRO_enmBedeutung.ToString();
				if (dictionary.TryGetValue(key2, out List<string> value2))
				{
					if (!value2.Contains(pRO_strCode))
					{
						value2.Add(pRO_strCode);
					}
				}
				else
				{
					dictionary.Add(key2, new List<string>
					{
						pRO_strCode
					});
				}
			}
			return dictionary;
		}
	}
}
