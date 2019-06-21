using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Nutzen;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Nutzen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Nutzen
{
	public class EDC_NutzenDataAccess : EDC_DataAccess, INF_NutzenDataAccess, INF_DataAccess
	{
		public EDC_NutzenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcSchreibeNutzenDataInDatenbankAsync(string i_strHash, string i_strNutzenCode, string i_strNestCode, string i_strNestDaten, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				uint num = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
				EDC_NutzenData i_edcObjekt = new EDC_NutzenData
				{
					PRO_i64NutzenDataId = num,
					PRO_strHash = i_strHash,
					PRO_strNutzenCode = i_strNutzenCode,
					PRO_strNestCode = i_strNestCode,
					PRO_strNestDaten = i_strNestDaten
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<IEnumerable<EDC_NutzenData>> FUN_fdcLeseNestDatenZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_NutzenData.FUN_strHashWhereStatementErstellen(i_strHash);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_NutzenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_NutzenData>> FUN_fdcLeseNestDatenFuerNutzenCodeAusDatenbankAsync(string i_strHash, string i_strNutzenCode, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_NutzenData.FUN_strNutzenCodeWhereStatementErstellen(i_strHash, i_strNutzenCode);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_NutzenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_NutzenData> FUN_fdcLeseNestDatenFuerNestCodeAusDatenbankAsync(string i_strHash, string i_strPcbCode, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_NutzenData.FUN_strNestCodeWhereStatementErstellen(i_strHash, i_strPcbCode);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_NutzenData(i_strWhereStatement), null, i_fdcTransaktion);
		}
	}
}
