using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Cad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Cad
{
	public class EDC_CadEinstellungenDataAccess : EDC_DataAccess, INF_CadEinstellungenDataAccess, INF_DataAccess
	{
		public EDC_CadEinstellungenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_CadEinstellungenData> FUN_fdcHoleCadDatenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadEinstellungenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_CadEinstellungenData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task FUN_fdcSpeichereCadDatenAsync(long i_i64VersionId, string i_strDaten, string i_strEinstellungen, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcCadDatenLoeschenAsync(i_i64VersionId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrEmpty(i_strEinstellungen))
				{
					EDC_CadEinstellungenData i_edcObjekt = new EDC_CadEinstellungenData
					{
						PRO_i64VersionsId = i_i64VersionId,
						PRO_strEinstellungen = i_strEinstellungen,
						PRO_strDaten = i_strDaten
					};
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
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

		public Task FUN_fdcCadDatenLoeschenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64VersionId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcExportiereCadDatenAsync(long i_i64VersionId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadEinstellungenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadEinstellungenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcImportiereCadDatenAsync(DataSet i_fdcDataSet, long i_i64NeueVersionsId, IDbTransaction i_fdcTransaktion)
		{
			DataTable dataTable = i_fdcDataSet.Tables["CadSettings"];
			if (dataTable != null)
			{
				List<EDC_CadEinstellungenData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadEinstellungenData>().ToList();
				foreach (EDC_CadEinstellungenData item in list)
				{
					item.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				return m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion);
			}
			return Task.FromResult(0);
		}
	}
}
