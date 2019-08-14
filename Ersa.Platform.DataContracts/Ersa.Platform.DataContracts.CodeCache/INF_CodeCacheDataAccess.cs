using Ersa.Platform.Common.Data.CodeCache;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.CodeCache
{
	public interface INF_CodeCacheDataAccess : INF_DataAccess
	{
		Task FUN_fdcSchreibeCodesZuHashInDatenbankAsync(IEnumerable<EDC_CodeCacheEintragData> i_enuCodeCacheEintraege, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndVerwendungAusDatenbankAsync(string i_strHash, string i_strVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndBedeutungAusDatenbankAsync(string i_strHash, string i_strBedeutung, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndVerwendungUndBedeutungAusDatenbankAsync(string i_strHash, string i_strVerwendung, string i_strBedeutung, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSchreibeLinksZuHashInDatenbankAsync(IEnumerable<EDC_LinkCacheData> i_enuLinkCacheEintraege, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LinkCacheData>> FUN_fdcLeseLinksZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LinkCacheData>> FUN_fdcLeseLinkZuHashUndVerwendungAusDatenbankAsync(string i_strHash, ENUM_LinkVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);
	}
}
