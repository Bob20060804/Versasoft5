using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Sprachen;
using Ersa.Platform.Data.DatenModelle.Organisation;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Sprache;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Sprachen
{
	public class EDC_SprachenDataAccess : EDC_DataAccess, INF_SprachenDataAccess, INF_DataAccess
	{
		public EDC_SprachenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcLadeSprachenXmlInDatenbankAsync(Dictionary<CultureInfo, Dictionary<string, string>> i_dicSprachenPool)
		{
			long i64AktuelleDatenbankVersion = await FUN_i64LeseSprachversionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64AktuelleXmlVersion = FUN_i64LeseSprachversionAusDictionary(i_dicSprachenPool);
			if (i64AktuelleDatenbankVersion < i64AktuelleXmlVersion)
			{
				List<EDC_SprachenEintragData> lstSprachenEintraege = new List<EDC_SprachenEintragData>();
				foreach (KeyValuePair<CultureInfo, Dictionary<string, string>> item in i_dicSprachenPool)
				{
					foreach (KeyValuePair<string, string> item2 in item.Value)
					{
						lstSprachenEintraege.Add(new EDC_SprachenEintragData
						{
							PRO_strSprache = item.Key.Name,
							PRO_strKey = item2.Key,
							PRO_strText = item2.Value
						});
					}
				}
				IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					await m_edcDatenbankAdapter.FUN_fdcLeereTabelleAsync("LanguageEntries").ConfigureAwait(continueOnCapturedContext: false);
					await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstSprachenEintraege, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await FUN_fdcSpeicherVersionsnummerAsync(fdcTransaktion, i64AktuelleDatenbankVersion, i64AktuelleXmlVersion).ConfigureAwait(continueOnCapturedContext: false);
					SUB_CommitTransaktion(fdcTransaktion);
				}
				catch
				{
					SUB_RollbackTransaktion(fdcTransaktion);
					throw;
				}
			}
		}

		private async Task<long> FUN_i64LeseSprachversionAusDatenbankAsync()
		{
			EDC_Parameter i_edcSelectObjekt = new EDC_Parameter(EDC_Parameter.FUN_strSprachenXmlVersionWhereStatementErstellen());
			return (await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(i_edcSelectObjekt).ConfigureAwait(continueOnCapturedContext: true))?.PRO_i64Wert ?? (-1);
		}

		private long FUN_i64LeseSprachversionAusDictionary(Dictionary<CultureInfo, Dictionary<string, string>> i_dicSprachenPool)
		{
			if (!i_dicSprachenPool[new CultureInfo("de")].TryGetValue("0_0", out string value))
			{
				return -1L;
			}
			if (!long.TryParse(value, out long result))
			{
				return -1L;
			}
			return result;
		}

		private async Task FUN_fdcSpeicherVersionsnummerAsync(IDbTransaction i_fdcTransaction, long i_i64AlteVersionsNummer, long i_i64NeueVersionsNummer)
		{
			string pRO_strWhereStatement = EDC_Parameter.FUN_strSprachenXmlVersionWhereStatementErstellen();
			EDC_Parameter edcNeuerVersionsParameter = new EDC_Parameter
			{
				PRO_i64Wert = i_i64NeueVersionsNummer,
				PRO_strParameter = "sprachen.xml version",
				PRO_strWhereStatement = pRO_strWhereStatement
			};
			if (i_i64AlteVersionsNummer == -1)
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcNeuerVersionsParameter, i_fdcTransaction).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcNeuerVersionsParameter, i_fdcTransaction).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
