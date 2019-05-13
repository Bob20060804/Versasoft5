using Ersa.Platform.Common.Data.CodeCache;
using Ersa.Platform.Common.Konstanten;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.DataDienste.CodeCache.Helfer
{
	public static class EDC_CodeCacheKonvertierungsHelfer
	{
		public static EDC_CodeCacheEintragData FUN_edcEintragDataErzeugen(string i_strHash, long i_i64ArrayIndex, long i_i64MaschinenId, string i_strVerwendung, string i_strBedeutung, string i_strCode)
		{
			return new EDC_CodeCacheEintragData
			{
				PRO_strHash = i_strHash,
				PRO_i64ArrayIndex = i_i64ArrayIndex,
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_strVerwendung = i_strVerwendung,
				PRO_strBedeutung = i_strBedeutung,
				PRO_strCode = i_strCode
			};
		}

		public static IEnumerable<EDC_CodeCacheEintragData> FUN_enuEintraegslisteErzeugen(string i_strHash, long i_i64ArrayIndex, long i_i64MaschinenId, IEnumerable<EDC_CodeMitVerwendungUndBedeutung> i_lstCodeMitVerwendungUndBedeutung)
		{
			return (from edcCodeMitVerwendungUndBedeutung in i_lstCodeMitVerwendungUndBedeutung
			select FUN_edcEintragDataErzeugen(i_strHash, i_i64ArrayIndex, i_i64MaschinenId, edcCodeMitVerwendungUndBedeutung.PRO_enmVerwendung.ToString(), edcCodeMitVerwendungUndBedeutung.PRO_enmBedeutung.ToString(), edcCodeMitVerwendungUndBedeutung.PRO_strCode)).ToList();
		}

		public static IEnumerable<EDC_CodeMitVerwendungUndBedeutung> FUN_enuListeCodeMitVerwendungUndBedeutungErzeugen(IEnumerable<EDC_CodeCacheEintragData> i_enuCodeZuVerwendung)
		{
			List<EDC_CodeMitVerwendungUndBedeutung> list = new List<EDC_CodeMitVerwendungUndBedeutung>();
			foreach (EDC_CodeCacheEintragData item in i_enuCodeZuVerwendung)
			{
				if (string.IsNullOrEmpty(item.PRO_strBedeutung))
				{
					ENUM_CodeBedeutung result2;
					if (Enum.TryParse(item.PRO_strVerwendung, ignoreCase: true, out ENUM_CodeVerwendung result))
					{
						list.Add(new EDC_CodeMitVerwendungUndBedeutung(item.PRO_strCode, result, ENUM_CodeBedeutung.NoUsageDefined));
					}
					else if (Enum.TryParse(item.PRO_strVerwendung, ignoreCase: true, out result2))
					{
						list.Add(new EDC_CodeMitVerwendungUndBedeutung(item.PRO_strCode, ENUM_CodeVerwendung.NoUsageDefined, result2));
					}
				}
				else
				{
					list.Add(new EDC_CodeMitVerwendungUndBedeutung(item.PRO_strCode, (ENUM_CodeVerwendung)Enum.Parse(typeof(ENUM_CodeVerwendung), item.PRO_strVerwendung), (ENUM_CodeBedeutung)Enum.Parse(typeof(ENUM_CodeBedeutung), item.PRO_strBedeutung)));
				}
			}
			return list;
		}

		public static EDC_LinkCacheData FUN_edcEintragLinkDataErzeugen(string i_strHash, long i_i64MaschinenId, string i_strLink, ENUM_LinkVerwendung i_enmLinkVerwendung)
		{
			return new EDC_LinkCacheData
			{
				PRO_strHash = i_strHash,
				PRO_enmLinkVerwendung = i_enmLinkVerwendung,
				PRO_strLink = i_strLink,
				PRO_i64MaschinenId = i_i64MaschinenId
			};
		}
	}
}
