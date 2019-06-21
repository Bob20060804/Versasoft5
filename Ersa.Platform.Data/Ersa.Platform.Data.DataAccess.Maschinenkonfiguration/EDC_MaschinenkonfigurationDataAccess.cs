using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Platform.Common.Data.Maschinenkonfiguration;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Maschinenkonfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Maschinenkonfiguration
{
	public class EDC_MaschinenkonfigurationDataAccess : EDC_DataAccess, INF_MaschinenkonfigurationDataAccess, INF_DataAccess
	{
		public EDC_MaschinenkonfigurationDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<long> FUN_fdcSpeichereKonfigurationAsync(EDC_MaschinenkonfigurationData i_edcMaschinenkonfiguration, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcMaschinenkonfiguration.PRO_i64KonfigurationsId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			i_edcMaschinenkonfiguration.PRO_dtmAngelegtAm = DateTime.Now;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcMaschinenkonfiguration, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i_edcMaschinenkonfiguration.PRO_i64KonfigurationsId;
		}

		public Task<IEnumerable<EDC_MaschinenkonfigurationData>> FUN_fdcLadeAlleMaschinenkonfigurationenZuMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_MaschinenkonfigurationData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenkonfigurationData(i_strWhereStatement));
		}

		public Task<DataTable> FUN_fdcLadeAlleMaschinenkonfigurationenZuMaschineInDataTableAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_MaschinenkonfigurationData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_MaschinenkonfigurationData(i_strWhereStatement));
		}

		public async Task<EDC_MaschinenkonfigurationData> FUN_fdcLadeNeusteMaschinenkonfigurationenZuMaschineAsync(long i_i64MaschinenId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long num = await FUN_fdcHoleMaxKonfigurationeIdFuerMaschineAsync(i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				EDC_MaschinenkonfigurationData result = null;
				if (num > 0)
				{
					result = await FUN_fdcLadeMaschinenkonfigurationAsync(num, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				return null;
			}
		}

		public async Task<DataTable> FUN_fdcLadeNeusteMaschinenkonfigurationenZuMaschineInDataTableAsync(long i_i64MaschinenId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				long num = await FUN_fdcHoleMaxKonfigurationeIdFuerMaschineAsync(i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				DataTable result = null;
				if (num > 0)
				{
					string i_strWhereStatement = EDC_MaschinenkonfigurationData.FUN_strKonfigurationsIdWhereStatementErstellen(num);
					result = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_MaschinenkonfigurationData(i_strWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcTransaktion);
				return result;
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				return null;
			}
		}

		public Task<long> FUN_fdcImportiereKonfigurationAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion, long i_i64NeueMaschinenId = 0L)
		{
			INF_ObjektAusDataRow<EDC_MaschinenkonfigurationData> iNF_ObjektAusDataRow = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataRowConverter<EDC_MaschinenkonfigurationData>();
			DataRow i_fdcDataRow = i_fdcDataTable.Rows[0];
			EDC_MaschinenkonfigurationData eDC_MaschinenkonfigurationData = iNF_ObjektAusDataRow.FUN_edcLeseObjektAusDataRow(i_fdcDataRow);
			if (i_i64NeueMaschinenId > 0)
			{
				eDC_MaschinenkonfigurationData.PRO_i64MaschinenId = i_i64NeueMaschinenId;
			}
			return FUN_fdcSpeichereKonfigurationAsync(eDC_MaschinenkonfigurationData, i_fdcTransaktion);
		}

		public Task<EDC_MaschinenkonfigurationData> FUN_fdcLadeMaschinenkonfigurationAsync(long i_i64KonfigurationsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenkonfigurationData.FUN_strKonfigurationsIdWhereStatementErstellen(i_i64KonfigurationsId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenkonfigurationData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		private async Task<long> FUN_fdcHoleMaxKonfigurationeIdFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion)
		{
			string i_strSql = EDC_MaschinenkonfigurationData.FUN_strSelectMaxKonfigurationsId(i_i64MaschinenId);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (obj != null && obj != DBNull.Value)
			{
				return Convert.ToInt64(obj);
			}
			return 0L;
		}
	}
}
