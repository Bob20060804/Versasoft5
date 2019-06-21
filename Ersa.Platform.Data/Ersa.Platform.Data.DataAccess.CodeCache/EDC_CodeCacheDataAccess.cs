using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.CodeCache;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.CodeCache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.CodeCache
{
	public class EDC_CodeCacheDataAccess : EDC_DataAccess, INF_CodeCacheDataAccess, INF_DataAccess
	{
		public EDC_CodeCacheDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcSchreibeCodesZuHashInDatenbankAsync(IEnumerable<EDC_CodeCacheEintragData> i_enuCodeCacheEintraege, IDbTransaction i_fdcTransaktion = null)
		{
			List<EDC_CodeCacheEintragData> lstCodeCachEintraege = i_enuCodeCacheEintraege.ToList();
			foreach (EDC_CodeCacheEintragData edcEintragData in lstCodeCachEintraege)
			{
				EDC_CodeCacheEintragData eDC_CodeCacheEintragData = edcEintragData;
				long num2 = eDC_CodeCacheEintragData.PRO_i64CacheId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
				edcEintragData.PRO_dtmAngelegtAm = DateTime.Now;
			}
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstCodeCachEintraege, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndVerwendungAusDatenbankAsync(string i_strHash, string i_strVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CodeCacheEintragData.FUN_strHashUndVerwendungWhereStatementErstellen(i_strHash, i_strVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeCacheEintragData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndBedeutungAusDatenbankAsync(string i_strHash, string i_strBedeutung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CodeCacheEintragData.FUN_strHashUndBedeutungWhereStatementErstellen(i_strHash, i_strBedeutung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeCacheEintragData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashUndVerwendungUndBedeutungAusDatenbankAsync(string i_strHash, string i_strVerwendung, string i_strBedeutung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CodeCacheEintragData.FUN_strHashUndVerwendungUndBedeutungWhereStatementErstellen(i_strHash, i_strVerwendung, i_strBedeutung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeCacheEintragData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CodeCacheEintragData>> FUN_fdcLeseCodesZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CodeCacheEintragData.FUN_strHashWhereStatementErstellen(i_strHash);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodeCacheEintragData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcSchreibeLinksZuHashInDatenbankAsync(IEnumerable<EDC_LinkCacheData> i_enuLinkCacheEintraege, IDbTransaction i_fdcTransaktion = null)
		{
			List<EDC_LinkCacheData> lstLinkCachEintraege = i_enuLinkCacheEintraege.ToList();
			foreach (EDC_LinkCacheData edcEintragData in lstLinkCachEintraege)
			{
				EDC_LinkCacheData eDC_LinkCacheData = edcEintragData;
				long num2 = eDC_LinkCacheData.PRO_i64LinkId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
				edcEintragData.PRO_dtmAngelegtAm = DateTime.Now;
			}
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstLinkCachEintraege, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<IEnumerable<EDC_LinkCacheData>> FUN_fdcLeseLinksZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LinkCacheData.FUN_strHashWhereStatementErstellen(i_strHash);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LinkCacheData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_LinkCacheData>> FUN_fdcLeseLinkZuHashUndVerwendungAusDatenbankAsync(string i_strHash, ENUM_LinkVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LinkCacheData.FUN_strHashUndVerwendungWhereStatementErstellen(i_strHash, i_enmVerwendung);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LinkCacheData(i_strWhereStatement), i_fdcTransaktion);
		}
	}
}
