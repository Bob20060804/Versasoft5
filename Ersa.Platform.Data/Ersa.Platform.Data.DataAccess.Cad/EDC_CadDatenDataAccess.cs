using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Cad;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Cad
{
	public class EDC_CadDatenDataAccess : EDC_DataAccess, INF_CadDatenDataAccess, INF_DataAccess
	{
		public EDC_CadDatenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_CadVerbotenerBereichData>> FUN_fdcAlleVerbotenenBereicheLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadVerbotenerBereichData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadVerbotenerBereichData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcVerboteneBereicheSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadVerbotenerBereichData> i_enuVerboteneBereiche, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleVerboteneBereicheLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuVerboteneBereiche != null)
				{
					List<EDC_CadVerbotenerBereichData> list = i_enuVerboteneBereiche.ToList();
					foreach (EDC_CadVerbotenerBereichData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleVerboteneBereicheLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadVerbotenerBereichData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadAblaufSchrittData>> FUN_fdcAlleAblaufSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadAblaufSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadAblaufSchrittData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcAblaufSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadAblaufSchrittData> i_enuAblaufschritte, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleAblaufSchritteLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuAblaufschritte != null)
				{
					List<EDC_CadAblaufSchrittData> list = i_enuAblaufschritte.ToList();
					foreach (EDC_CadAblaufSchrittData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleAblaufSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadAblaufSchrittData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadCncSchrittData>> FUN_fdcAlleCncSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadCncSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadCncSchrittData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcCncSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadCncSchrittData> i_enuCncSchritte, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleCncSchritteLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuCncSchritte != null)
				{
					List<EDC_CadCncSchrittData> list = i_enuCncSchritte.ToList();
					foreach (EDC_CadCncSchrittData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleCncSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadCncSchrittData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadRoutenSchrittData>> FUN_fdcAlleRoutenSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadCncSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadRoutenSchrittData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcRoutenSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadRoutenSchrittData> i_enuRoutenSchritte, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleRoutenSchritteLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuRoutenSchritte != null)
				{
					List<EDC_CadRoutenSchrittData> list = i_enuRoutenSchritte.ToList();
					foreach (EDC_CadRoutenSchrittData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleRoutenSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadRoutenSchrittData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadRoutenData>> FUN_fdcAlleRoutenDatenLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadRoutenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadRoutenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcRoutenDatenSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadRoutenData> i_enuRoutenDaten, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleRoutenDatenLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuRoutenDaten != null)
				{
					List<EDC_CadRoutenData> list = i_enuRoutenDaten.ToList();
					foreach (EDC_CadRoutenData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleRoutenDatenLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadRoutenData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_CadBewegungsGruppenData>> FUN_fdcAlleBewegungsGruppenLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_CadBewegungsGruppenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CadBewegungsGruppenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcBewegungsGruppenSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadBewegungsGruppenData> i_enuGruppen, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await FUN_fdcAlleBewegungsGruppenLoeschenAsync(i_i64VersionsId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_enuGruppen != null)
				{
					List<EDC_CadBewegungsGruppenData> list = i_enuGruppen.ToList();
					foreach (EDC_CadBewegungsGruppenData item in list)
					{
						item.PRO_i64VersionsId = i_i64VersionsId;
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

		public Task FUN_fdcAlleBewegungsGruppenLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_CadBewegungsGruppenData.FUN_strLoescheHistoryIdStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task<DataSet> FUN_fdcExportiereProjektDatenVersionAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			DataSet fdcDataSet = new DataSet();
			string i_strWhereStatement = EDC_CadVerbotenerBereichData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadVerbotenerBereichData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable != null)
			{
				fdcDataSet.Tables.Add(dataTable);
			}
			i_strWhereStatement = EDC_CadAblaufSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable2 = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadAblaufSchrittData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable2 != null)
			{
				fdcDataSet.Tables.Add(dataTable2);
			}
			i_strWhereStatement = EDC_CadBewegungsGruppenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable3 = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadBewegungsGruppenData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable3 != null)
			{
				fdcDataSet.Tables.Add(dataTable3);
			}
			i_strWhereStatement = EDC_CadCncSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable4 = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadCncSchrittData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable4 != null)
			{
				fdcDataSet.Tables.Add(dataTable4);
			}
			i_strWhereStatement = EDC_CadRoutenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable5 = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadRoutenData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable5 != null)
			{
				fdcDataSet.Tables.Add(dataTable5);
			}
			i_strWhereStatement = EDC_CadCncSchrittData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			DataTable dataTable6 = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_CadRoutenSchrittData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (dataTable6 != null)
			{
				fdcDataSet.Tables.Add(dataTable6);
			}
			return fdcDataSet;
		}

		public async Task FUN_fdcImportiereAusLoetprogrammAsync(DataSet i_fdcDataSet, long i_i64NeueVersionsId, IDbTransaction i_fdcTransaktion)
		{
			DataTable dataTable = i_fdcDataSet.Tables["CadForbiddenAreas"];
			if (dataTable != null)
			{
				List<EDC_CadVerbotenerBereichData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadVerbotenerBereichData>().ToList();
				foreach (EDC_CadVerbotenerBereichData item in list)
				{
					item.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			dataTable = i_fdcDataSet.Tables["CadFlowSteps"];
			if (dataTable != null)
			{
				List<EDC_CadAblaufSchrittData> list2 = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadAblaufSchrittData>().ToList();
				foreach (EDC_CadAblaufSchrittData item2 in list2)
				{
					item2.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list2, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			dataTable = i_fdcDataSet.Tables["CadMotionGroups"];
			if (dataTable != null)
			{
				List<EDC_CadBewegungsGruppenData> list3 = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadBewegungsGruppenData>().ToList();
				foreach (EDC_CadBewegungsGruppenData item3 in list3)
				{
					item3.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list3, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			dataTable = i_fdcDataSet.Tables["CadCncSteps"];
			if (dataTable != null)
			{
				List<EDC_CadCncSchrittData> list4 = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadCncSchrittData>().ToList();
				foreach (EDC_CadCncSchrittData item4 in list4)
				{
					item4.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list4, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			dataTable = i_fdcDataSet.Tables["CadRoutingData"];
			if (dataTable != null)
			{
				List<EDC_CadRoutenData> list5 = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadRoutenData>().ToList();
				foreach (EDC_CadRoutenData item5 in list5)
				{
					item5.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list5, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			dataTable = i_fdcDataSet.Tables["CadRoutingSteps"];
			if (dataTable != null)
			{
				List<EDC_CadRoutenSchrittData> list6 = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_CadRoutenSchrittData>().ToList();
				foreach (EDC_CadRoutenSchrittData item6 in list6)
				{
					item6.PRO_i64VersionsId = i_i64NeueVersionsId;
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list6, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
