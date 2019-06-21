using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess
{
	public abstract class EDC_DataAccess : INF_DataAccess
	{
		protected readonly INF_DatenbankAdapter m_edcDatenbankAdapter;

		protected EDC_DataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			m_edcDatenbankAdapter = i_edcDatenbankAdapter;
		}

		public IDbTransaction FUN_fdcStarteTransaktion()
		{
			return m_edcDatenbankAdapter.FUN_fdcStarteTransaktion();
		}

		public Task<IDbTransaction> FUN_fdcStarteTransaktionAsync()
		{
			return m_edcDatenbankAdapter.FUN_fdcStarteTransaktionAsync();
		}

		public void SUB_CommitTransaktion(IDbTransaction i_fdcDbTransaktion)
		{
			m_edcDatenbankAdapter.SUB_CommitTransaktion(i_fdcDbTransaktion);
		}

		public void SUB_RollbackTransaktion(IDbTransaction i_fdcDbTransaktion)
		{
			m_edcDatenbankAdapter.SUB_RollbackTransaktion(i_fdcDbTransaktion);
		}

		public async Task<long> FUN_fdcHoleNaechstenSequenzWertAsync(string i_strSequenzName)
		{
			return await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync(i_strSequenzName).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcFuehreStatementListeAusAsync(IEnumerable<string> i_enuStatements, IDbTransaction i_fdcTransaktion = null)
		{
			List<string> lstStatements = i_enuStatements.ToList();
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (string item in lstStatements)
				{
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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
	}
}
