using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Aoi;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Aoi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Aoi
{
	public class EDC_AoiDataAccess : EDC_DataAccess, INF_AoiDataAccess, INF_DataAccess
	{
		public EDC_AoiDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_AoiProgramData> FUN_fdcHoleAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiProgramData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_AoiProgramData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task FUN_fdcSpeichereAoiProgrammAsync(long i_i64ProgrammId, string i_strDaten, string i_strEinstellungen, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcLoescheAoiProgrammAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				EDC_AoiProgramData i_edcObjekt = new EDC_AoiProgramData
				{
					PRO_i64ProgrammId = i_i64ProgrammId,
					PRO_strEinstellungen = i_strEinstellungen,
					PRO_strDaten = i_strDaten
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public Task FUN_fdcLoescheAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_AoiProgramData.FUN_strLoescheProgrammStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<EDC_AoiSchrittData> FUN_fdcHoleAoiSchrittDatenAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiSchrittData.FUN_strHoleProgrammIdUndStepGuidUndPanelWhereStatement(i_i64ProgrammId, i_strStepGuid, i_i32Panel);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_AoiSchrittData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_AoiSchrittData>> FUN_fdcHoleAoiSchrittDatenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiSchrittData.FUN_strHoleProgramIdWhereStatement(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_AoiSchrittData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcSpeichereAoiSchrittDatenAsync(long i_i64ProgrammId, IEnumerable<EDC_AoiSchrittData> i_enuAoiSchrittData, IDbTransaction i_fdcTransaktion)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcLoescheAoiSchrittDataAsync(i_i64ProgrammId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuAoiSchrittData != null)
				{
					List<EDC_AoiSchrittData> list = i_enuAoiSchrittData.ToList();
					foreach (EDC_AoiSchrittData item in list)
					{
						item.PRO_i64ProgrammId = i_i64ProgrammId;
					}
					await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcSpeichereAoiSchrittDatenAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, byte[] i_bytBinaries, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcLoescheAoiSchrittDataAsync(i_i64ProgrammId, i_strStepGuid, i_i32Panel, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				EDC_AoiSchrittData i_edcObjekt = new EDC_AoiSchrittData
				{
					PRO_strStepGuid = i_strStepGuid,
					PRO_i32Panel = i_i32Panel,
					PRO_i64ProgrammId = i_i64ProgrammId,
					PRO_bytBinaries = i_bytBinaries
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public Task FUN_fdcLoescheAoiSchrittDataAsync(long i_i64ProgrammId, string i_strStepGuid, int i_i32Panel, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_AoiSchrittData.FUN_strLoescheStepStatementErstellen(i_i64ProgrammId, i_strStepGuid, i_i32Panel);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task FUN_fdcLoescheAoiSchrittDataAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_AoiSchrittData.FUN_strLoescheProgramIdStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_AoiErgebnisData>> FUN_fdcHoleAoiErgebnisMitHashAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiErgebnisData.FUN_strHoleHashWhereStatement(i_strHash);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_AoiErgebnisData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_AoiErgebnisData>> FUN_fdcHoleAoiErgebnisMitHashUndAoiTypAsync(string i_strHash, int i_i32AoiType, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiErgebnisData.FUN_strHoleHashWhereStatement(i_strHash, i_i32AoiType);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_AoiErgebnisData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcSpeichereAoiErgebnisDatenListAsync(IEnumerable<EDC_AoiErgebnisData> i_enuErgebnisData, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_AoiErgebnisData edcErgebnisData in i_enuErgebnisData)
				{
					EDC_AoiErgebnisData eDC_AoiErgebnisData = edcErgebnisData;
					eDC_AoiErgebnisData.PRO_i64ResultId = await FUN_fdcHoleNaechstenSequenzWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
					edcErgebnisData.PRO_dtmAngelegtAm = DateTime.Now;
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcErgebnisData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
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

		public Task<DataTable> FUN_fdcExportiereAoiProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiProgramData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_AoiProgramData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcImportiereAoiProgrammAsync(DataSet i_fdcDataSet, long i_i64NeueProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["AoiPrograms"];
			if (dataTable != null)
			{
				List<EDC_AoiProgramData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_AoiProgramData>().ToList();
				foreach (EDC_AoiProgramData item in list)
				{
					item.PRO_i64ProgrammId = i_i64NeueProgrammId;
				}
				return m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion);
			}
			return Task.FromResult(0);
		}

		public Task<DataTable> FUN_fdcExportiereAoiSchrittDatenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_AoiSchrittData.FUN_strHoleProgramIdWhereStatement(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_AoiSchrittData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcImportiereAoiSchrittDatenAsync(DataSet i_fdcDataSet, long i_i64NeueProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["AoiStepData"];
			if (dataTable != null)
			{
				List<EDC_AoiSchrittData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_AoiSchrittData>().ToList();
				foreach (EDC_AoiSchrittData item in list)
				{
					item.PRO_i64ProgrammId = i_i64NeueProgrammId;
				}
				return m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion);
			}
			return Task.FromResult(0);
		}
	}
}
