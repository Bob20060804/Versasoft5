using Ersa.Global.Common.Helper;
using Ersa.Platform.Common.Data.CodeCache;
using Ersa.Platform.Common.Konstanten;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataContracts.CodeCache;
using Ersa.Platform.DataDienste.CodeCache.Helfer;
using Ersa.Platform.DataDienste.CodeCache.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.CodeCache
{
	[Export(typeof(INF_CodesCache))]
	public class EDC_CodesCache : EDC_DataDienst, INF_CodesCache
	{
		private readonly Lazy<INF_CodeCacheDataAccess> m_fdcCodeCacheDataAccess;

		[ImportingConstructor]
		public EDC_CodesCache(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_fdcCodeCacheDataAccess = new Lazy<INF_CodeCacheDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CodeCacheDataAccess>);
		}

		public string FUN_strEintragErzeugen(string i_strKey, string i_strSalt)
		{
			return EDC_HashHelfer.FUN_strHashErzeugen(i_strKey, i_strSalt);
		}

		public async Task FUN_fdcCodesZumEintragHinzufuegenAsync(string i_strHash, long i_i64ArrayIndex, string i_strCodeVerwendung, string i_strCodeBedeutung, List<string> i_lstCodes)
		{
			IDbTransaction fdcTransaktion = await m_fdcCodeCacheDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				foreach (string i_lstCode in i_lstCodes)
				{
					EDC_CodeCacheEintragData eDC_CodeCacheEintragData = EDC_CodeCacheKonvertierungsHelfer.FUN_edcEintragDataErzeugen(i_strHash, i_i64ArrayIndex, i64MaschinenId, i_strCodeVerwendung, i_strCodeBedeutung, i_lstCode);
					await m_fdcCodeCacheDataAccess.Value.FUN_fdcSchreibeCodesZuHashInDatenbankAsync(new EDC_CodeCacheEintragData[1]
					{
						eDC_CodeCacheEintragData
					}, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_fdcCodeCacheDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_fdcCodeCacheDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcCodesHinzufuegenAsync(string i_strHash, long i_i64ArrayIndex, IEnumerable<EDC_CodeMitVerwendungUndBedeutung> i_lstCodeMitVerwendungUndBedeutung)
		{
			IEnumerable<EDC_CodeCacheEintragData> i_enuCodeCacheEintraege = EDC_CodeCacheKonvertierungsHelfer.FUN_enuEintraegslisteErzeugen(i_strHash, i_i64ArrayIndex, await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false), i_lstCodeMitVerwendungUndBedeutung);
			await m_fdcCodeCacheDataAccess.Value.FUN_fdcSchreibeCodesZuHashInDatenbankAsync(i_enuCodeCacheEintraege);
		}

		public async Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerVerwendungAsync(string i_strHash, string i_strCodeverwendung)
		{
			return EDC_CodeCacheKonvertierungsHelfer.FUN_enuListeCodeMitVerwendungUndBedeutungErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseCodesZuHashUndVerwendungAusDatenbankAsync(i_strHash, i_strCodeverwendung).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerBedeutungAsync(string i_strHash, string i_strCodebedeutung)
		{
			return EDC_CodeCacheKonvertierungsHelfer.FUN_enuListeCodeMitVerwendungUndBedeutungErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseCodesZuHashUndBedeutungAusDatenbankAsync(i_strHash, i_strCodebedeutung).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerVerwendungUndBedeutungAsync(string i_strHash, string i_strCodeverwendung, string i_strCodebedeutung)
		{
			return EDC_CodeCacheKonvertierungsHelfer.FUN_enuListeCodeMitVerwendungUndBedeutungErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseCodesZuHashUndVerwendungUndBedeutungAusDatenbankAsync(i_strHash, i_strCodeverwendung, i_strCodebedeutung).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcAlleCodesErmittelnAsync(string i_strHash)
		{
			return EDC_CodeCacheKonvertierungsHelfer.FUN_enuListeCodeMitVerwendungUndBedeutungErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseCodesZuHashAusDatenbankAsync(i_strHash).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IDictionary<string, List<string>>> FUN_fdcCodesFuerProtokollErmittelnAsync(string i_strHash)
		{
			Dictionary<string, List<string>> dicVerwendungUndCodes = new Dictionary<string, List<string>>();
			List<EDC_CodeCacheEintragData> source = (await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseCodesZuHashAusDatenbankAsync(i_strHash).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			foreach (EDC_CodeCacheEintragData item in from i_edcEintrag in source
			where i_edcEintrag.PRO_strVerwendung == ENUM_CodeVerwendung.Protocol.ToString()
			select i_edcEintrag)
			{
				SUB_FuegeBedeutungInDictionaryEin(dicVerwendungUndCodes, item);
			}
			foreach (EDC_CodeCacheEintragData item2 in from i_edcEintrag in source
			where i_edcEintrag.PRO_strVerwendung == ENUM_CodeVerwendung.Program.ToString()
			select i_edcEintrag)
			{
				SUB_FuegeVerwendungInDictionaryEin(dicVerwendungUndCodes, item2);
				SUB_FuegeBedeutungInDictionaryEin(dicVerwendungUndCodes, item2);
			}
			foreach (EDC_CodeCacheEintragData item3 in from i_edcEintrag in source
			where i_edcEintrag.PRO_strVerwendung == ENUM_CodeVerwendung.InfeedStopper.ToString()
			select i_edcEintrag)
			{
				SUB_FuegeVerwendungInDictionaryEin(dicVerwendungUndCodes, item3);
				SUB_FuegeBedeutungInDictionaryEin(dicVerwendungUndCodes, item3);
			}
			foreach (EDC_CodeCacheEintragData item4 in from i_edcEintrag in source
			where i_edcEintrag.PRO_strVerwendung == ENUM_CodeVerwendung.MesInfeedReleaseAndRecipe.ToString()
			select i_edcEintrag)
			{
				SUB_FuegeVerwendungInDictionaryEin(dicVerwendungUndCodes, item4);
				SUB_FuegeBedeutungInDictionaryEin(dicVerwendungUndCodes, item4);
			}
			return dicVerwendungUndCodes;
		}

		public async Task FUN_fdcLinksHinzufuegenAsync(string i_strHash, IEnumerable<string> i_enuLinks, ENUM_LinkVerwendung i_enmLinkVerwendung)
		{
			IDbTransaction fdcTransaktion = await m_fdcCodeCacheDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				foreach (string i_enuLink in i_enuLinks)
				{
					EDC_LinkCacheData eDC_LinkCacheData = EDC_CodeCacheKonvertierungsHelfer.FUN_edcEintragLinkDataErzeugen(i_strHash, i64MaschinenId, i_enuLink, i_enmLinkVerwendung);
					await m_fdcCodeCacheDataAccess.Value.FUN_fdcSchreibeLinksZuHashInDatenbankAsync(new EDC_LinkCacheData[1]
					{
						eDC_LinkCacheData
					}, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_fdcCodeCacheDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_fdcCodeCacheDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<IEnumerable<string>> FUN_fdcLinksFuerHashUndVerwendungErmittelnAsync(string i_strHash, ENUM_LinkVerwendung i_enmLinkVerwendung)
		{
			return FUN_enuLinkListeErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseLinkZuHashUndVerwendungAusDatenbankAsync(i_strHash, i_enmLinkVerwendung).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<IEnumerable<string>> FUN_fdcLinksFuerHashErmittelnAsync(string i_strHash)
		{
			return FUN_enuLinkListeErzeugen(await m_fdcCodeCacheDataAccess.Value.FUN_fdcLeseLinksZuHashAusDatenbankAsync(i_strHash).ConfigureAwait(continueOnCapturedContext: false));
		}

		private static IEnumerable<string> FUN_enuLinkListeErzeugen(IEnumerable<EDC_LinkCacheData> enuLinkCacheData)
		{
			List<string> list = new List<string>();
			foreach (EDC_LinkCacheData enuLinkCacheDatum in enuLinkCacheData)
			{
				list.Add(enuLinkCacheDatum.PRO_strLink);
			}
			return list;
		}

		private static void SUB_FuegeBedeutungInDictionaryEin(Dictionary<string, List<string>> i_dicVerwendungUndCodes, EDC_CodeCacheEintragData i_edcCodeEintrag)
		{
			if (i_edcCodeEintrag.PRO_strBedeutung != ENUM_CodeBedeutung.NoUsageDefined.ToString())
			{
				i_dicVerwendungUndCodes.TryGetValue(i_edcCodeEintrag.PRO_strBedeutung, out List<string> value);
				if (value == null)
				{
					value = new List<string>();
					i_dicVerwendungUndCodes.Add(i_edcCodeEintrag.PRO_strBedeutung, value);
				}
				value.Add(i_edcCodeEintrag.PRO_strCode);
			}
		}

		private static void SUB_FuegeVerwendungInDictionaryEin(Dictionary<string, List<string>> i_dicVerwendungUndCodes, EDC_CodeCacheEintragData i_edcEintragZuHash)
		{
			i_dicVerwendungUndCodes.TryGetValue(i_edcEintragZuHash.PRO_strVerwendung, out List<string> value);
			if (value == null)
			{
				value = new List<string>();
				i_dicVerwendungUndCodes.Add(i_edcEintragZuHash.PRO_strVerwendung, value);
			}
			value.Add(i_edcEintragZuHash.PRO_strCode);
		}
	}
}
