using Ersa.Global.Common.Data.Model;
using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Converter;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.DatabaseModel.Ersteller;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using Ersa.Global.DataAdapter.DatabaseModel.Updater;
using Ersa.Global.DataAdapter.Exeptions;
using Ersa.Global.DataAdapter.Extensions;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.Adapter.Impl
{
	public class EDC_DatenbankAdapter : INF_DatenbankAdapter, INF_ReadonlyDatenbankAdapter, INF_OrganisationsAdapter
	{
		private const int mC_i32ProviderWechselWarteZeit = 500;

		private const string mC_strProviderWechselMeldung = "data provider change in progress";

		private bool m_blnDatenProviderWechselWirdDurchgefuehrt;

		private string m_strLetzterVerbindungsFehler = string.Empty;

		public INF_DatenbankProvider PRO_edcDatenbankProvider
		{
			get;
			set;
		}

		public INF_DatenbankModel PRO_edcDatenbankModell
		{
			get;
			set;
		}

		public EDC_DatenbankAdapter(INF_DatenbankProvider i_edcDatenbankProvider, INF_DatenbankModel i_edcDatenbankModell)
		{
			PRO_edcDatenbankProvider = i_edcDatenbankProvider;
			PRO_edcDatenbankModell = i_edcDatenbankModell;
		}

		public void SUB_SetzeNeuenDatenbankProvider(INF_DatenbankProvider i_edcDatenbankProvider)
		{
			m_blnDatenProviderWechselWirdDurchgefuehrt = true;
			SUB_WarteAsynchron(500);
			PRO_edcDatenbankProvider = i_edcDatenbankProvider;
			SUB_WarteAsynchron(500);
			m_blnDatenProviderWechselWirdDurchgefuehrt = false;
		}

		public bool FUN_blnPruefeVerbindung(int i_i16Timeout)
		{
			return FUN_blnPruefeVerbindung(i_i16Timeout, 1);
		}

		public bool FUN_blnPruefeVerbindung(int i_i16Timeout, int i_i16AnzahlVeruche)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			DbConnection i_fdcConnection = PRO_edcDatenbankProvider.FUN_fdcGetConnection();
			m_strLetzterVerbindungsFehler = i_fdcConnection.FUN_blnPruefeVerbindung(i_i16Timeout, i_i16AnzahlVeruche);
			bool flag = FUN_blnCheckDatabaseService();
			if (!string.IsNullOrEmpty(m_strLetzterVerbindungsFehler) && !flag)
			{
				m_strLetzterVerbindungsFehler = $"Required database service is not running! {Environment.NewLine}{m_strLetzterVerbindungsFehler}";
			}
			return string.IsNullOrEmpty(m_strLetzterVerbindungsFehler);
		}

		public string FUN_strHoleLetzteVerbindungsFehlermeldung()
		{
			return m_strLetzterVerbindungsFehler;
		}

		public async Task<IDbTransaction> FUN_fdcStarteTransaktionAsync()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			DbConnection fdcConnection = PRO_edcDatenbankProvider.FUN_fdcGetConnection();
			await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
			return fdcConnection.BeginTransaction();
		}

		public IDbTransaction FUN_fdcStarteTransaktion()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			DbConnection dbConnection = PRO_edcDatenbankProvider.FUN_fdcGetConnection();
			dbConnection.Open();
			return dbConnection.BeginTransaction();
		}

		public void SUB_CommitTransaktion(IDbTransaction i_fdcDbTransaktion)
		{
			if (i_fdcDbTransaktion != null)
			{
				IDbConnection connection = i_fdcDbTransaktion.Connection;
				try
				{
					i_fdcDbTransaktion.Commit();
				}
				finally
				{
					connection?.Close();
				}
			}
		}

		public void SUB_RollbackTransaktion(IDbTransaction i_fdcDbTransaktion)
		{
			if (i_fdcDbTransaktion != null)
			{
				IDbConnection connection = i_fdcDbTransaktion.Connection;
				try
				{
					i_fdcDbTransaktion.Rollback();
				}
				finally
				{
					connection?.Close();
				}
			}
		}

		public object FUN_fdcScalareAbfrage(string i_strSql)
		{
			return FUN_fdcScalareAbfrage(i_strSql, null);
		}

		public object FUN_fdcScalareAbfrage(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return null;
			}
			DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				dbConnection?.Open();
				return FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteScalar();
			}
			finally
			{
				dbConnection?.Close();
			}
		}

		public Task<object> FUN_fdcScalareAbfrageAsync(string i_strSql)
		{
			return FUN_fdcScalareAbfrageAsync(i_strSql, null);
		}

		public async Task<object> FUN_fdcScalareAbfrageAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return null;
			}
			DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				if (fdcConnection != null)
				{
					await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				return await FUN_fdcErstelleCommand(i_strSql, fdcConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
			finally
			{
				fdcConnection?.Close();
			}
		}

		public void SUB_ExecuteStatement(string i_strSql)
		{
			SUB_ExecuteStatement(i_strSql, null, null);
		}

		public void SUB_ExecuteStatement(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					dbConnection?.Open();
					FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteNonQuery();
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public Task FUN_fdcExecuteStatementAsync(string i_strSql)
		{
			return FUN_fdcExecuteStatementAsync(i_strSql, null);
		}

		public async Task FUN_fdcExecuteStatementAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					if (fdcConnection != null)
					{
						await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
					}
					await FUN_fdcErstelleCommand(i_strSql, fdcConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					fdcConnection?.Close();
				}
			}
		}

		public void SUB_SpeichereObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					DbCommand dbCommand = FUN_fdcErstelleCommand(string.Empty, dbConnection, i_fdcDbTransaktion);
					EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Insert).SUB_ErstelleSqlStatement(i_edcObjekt, dbCommand);
					dbConnection?.Open();
					dbCommand.ExecuteNonQuery();
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public async Task FUN_fdcSpeichereObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					DbCommand fdcCommand = FUN_fdcErstelleCommand(string.Empty, fdcConnection, i_fdcDbTransaktion);
					EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Insert).SUB_ErstelleSqlStatement(i_edcObjekt, fdcCommand);
					if (fdcConnection != null)
					{
						await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
					}
					await fdcCommand.ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					fdcConnection?.Close();
				}
			}
		}

		public void SUB_SpeichereObjektListe<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null)
		{
			IList<T> list = (i_lstObjektListe as IList<T>) ?? i_lstObjektListe.ToList();
			if (list.Any() && !m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Insert);
				IDbTransaction i_fdcDbTransaktion2 = i_fdcDbTransaktion ?? FUN_fdcStarteTransaktion();
				try
				{
					DbCommand dbCommand = FUN_fdcErstelleCommand(string.Empty, null, i_fdcDbTransaktion2);
					foreach (T item in list)
					{
						dbCommand.Parameters.Clear();
						iNF_SqlStatementErsteller.SUB_ErstelleSqlStatement(item, dbCommand);
						dbCommand.ExecuteNonQuery();
					}
					if (i_fdcDbTransaktion == null)
					{
						SUB_CommitTransaktion(i_fdcDbTransaktion2);
					}
				}
				catch (Exception)
				{
					if (i_fdcDbTransaktion == null)
					{
						SUB_RollbackTransaktion(i_fdcDbTransaktion2);
					}
					throw;
				}
			}
		}

		public async Task FUN_fdcSpeichereObjektListeAsync<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null)
		{
			IList<T> lstObjektListe = (i_lstObjektListe as IList<T>) ?? i_lstObjektListe.ToList();
			if (lstObjektListe.Any() && !m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				INF_SqlStatementErsteller<T> edcInserter = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Insert);
				IDbTransaction dbTransaction = i_fdcDbTransaktion;
				if (dbTransaction == null)
				{
					dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				IDbTransaction fdcTransaktion = dbTransaction;
				try
				{
					DbCommand fdcCommand = FUN_fdcErstelleCommand(string.Empty, null, fdcTransaktion);
					foreach (T item in lstObjektListe)
					{
						fdcCommand.Parameters.Clear();
						edcInserter.SUB_ErstelleSqlStatement(item, fdcCommand);
						await fdcCommand.ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: true);
					}
					if (i_fdcDbTransaktion == null)
					{
						SUB_CommitTransaktion(fdcTransaktion);
					}
				}
				catch (Exception)
				{
					if (i_fdcDbTransaktion == null)
					{
						SUB_RollbackTransaktion(fdcTransaktion);
					}
					throw;
				}
			}
		}

		public void SUB_UpdateObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Update);
					DbCommand dbCommand = FUN_fdcErstelleCommand(string.Empty, dbConnection, i_fdcDbTransaktion, i_dicParameter);
					iNF_SqlStatementErsteller.SUB_ErstelleSqlStatement(i_edcObjekt, dbCommand);
					dbConnection?.Open();
					dbCommand.ExecuteNonQuery();
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public async Task FUN_fdcUpdateObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Update);
					DbCommand fdcCommand = FUN_fdcErstelleCommand(string.Empty, fdcConnection, i_fdcDbTransaktion, i_dicParameter);
					iNF_SqlStatementErsteller.SUB_ErstelleSqlStatement(i_edcObjekt, fdcCommand);
					if (fdcConnection != null)
					{
						await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
					}
					await fdcCommand.ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					fdcConnection?.Close();
				}
			}
		}

		public void SUB_LoescheObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Delete);
					DbCommand dbCommand = FUN_fdcErstelleCommand(string.Empty, dbConnection, i_fdcDbTransaktion, i_dicParameter);
					iNF_SqlStatementErsteller.SUB_ErstelleSqlStatement(i_edcObjekt, dbCommand);
					dbConnection?.Open();
					dbCommand.ExecuteNonQuery();
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public async Task FUN_fdcLoescheObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Delete);
					DbCommand fdcCommand = FUN_fdcErstelleCommand(string.Empty, fdcConnection, i_fdcDbTransaktion, i_dicParameter);
					iNF_SqlStatementErsteller.SUB_ErstelleSqlStatement(i_edcObjekt, fdcCommand);
					if (fdcConnection != null)
					{
						await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
					}
					await fdcCommand.ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					fdcConnection?.Close();
				}
			}
		}

		public void SUB_SchreibeDatenInTabelle(DataTable i_fdcDataTable, string i_strTabellenName, IDbTransaction i_fdcDbTransaktion = null)
		{
			if (i_fdcDataTable != null && i_fdcDataTable.Rows.Count != 0 && !m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				string i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit($"Select * From {i_strTabellenName}", 1);
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					dbConnection?.Open();
					DbDataAdapter dbDataAdapter = FUN_fdcErstelleDataAdapter(i_strSql, dbConnection, i_fdcDbTransaktion);
					PRO_edcDatenbankProvider.FUN_fdcGetCommandBuilder(dbDataAdapter);
					DataTable dataTable = new DataTable(i_strTabellenName);
					dbDataAdapter.Fill(dataTable);
					foreach (DataRow row in i_fdcDataTable.Rows)
					{
						DataRow dataRow2 = dataTable.NewRow();
						foreach (DataColumn column in dataTable.Columns)
						{
							if (i_fdcDataTable.Columns.Contains(column.ColumnName))
							{
								dataRow2[column.ColumnName] = row[column.ColumnName];
							}
						}
						dataTable.Rows.Add(dataRow2);
					}
					dbDataAdapter.Update(dataTable);
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public Task FUN_fdcSchreibeDatenInTabelleAsync(DataTable i_fdcDataTable, string i_strTabellenName, IDbTransaction i_fdcDbTransaktion = null)
		{
			return Task.Factory.StartNew(delegate
			{
				SUB_SchreibeDatenInTabelle(i_fdcDataTable, i_strTabellenName, i_fdcDbTransaktion);
			});
		}

		public IEnumerable<object> FUN_enuErstelleScalareObjektliste(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0)
		{
			List<object> list = new List<object>();
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return list;
			}
			if (i_i16AbfrageLimit > 0)
			{
				i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit(i_strSql, i_i16AbfrageLimit);
			}
			DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				dbConnection?.Open();
				using (DbDataReader dbDataReader = FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReader())
				{
					while (dbDataReader.Read())
					{
						if (dbDataReader.FieldCount > 0 && dbDataReader[0] != DBNull.Value)
						{
							list.Add(dbDataReader[0]);
						}
					}
				}
				return list;
			}
			finally
			{
				dbConnection?.Close();
			}
		}

		public async Task<IEnumerable<object>> FUN_fdcErstelleScalareObjektlisteAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0)
		{
			List<object> lstErgebnisse = new List<object>();
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return lstErgebnisse;
			}
			if (i_i16AbfrageLimit > 0)
			{
				i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit(i_strSql, i_i16AbfrageLimit);
			}
			DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				if (fdcConnection != null)
				{
					await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				using (DbDataReader fdcDataReader = await FUN_fdcErstelleCommand(i_strSql, fdcConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: true))
				{
					while (await fdcDataReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: true))
					{
						if (fdcDataReader.FieldCount > 0 && fdcDataReader[0] != DBNull.Value)
						{
							lstErgebnisse.Add(fdcDataReader[0]);
						}
					}
				}
				return lstErgebnisse;
			}
			finally
			{
				fdcConnection?.Close();
			}
		}

		public IEnumerable<T> FUN_lstErstelleObjektliste<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0)
		{
			List<T> list = new List<T>();
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return list;
			}
			INF_ObjektAusReader<T> iNF_ObjektAusReader = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataReaderConverter<T>();
			string i_strSql = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Select).FUN_strErstelleSqlStatement(i_edcSelectObjekt);
			if (i_i16AbfrageLimit > 0)
			{
				i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit(i_strSql, i_i16AbfrageLimit);
			}
			DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				dbConnection?.Open();
				using (DbDataReader dbDataReader = FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReader())
				{
					while (dbDataReader.Read())
					{
						list.Add(iNF_ObjektAusReader.FUN_edcLeseObjektAusReader(dbDataReader));
					}
				}
				return list;
			}
			finally
			{
				dbConnection?.Close();
			}
		}

		public Task<IEnumerable<T>> FUN_fdcErstelleObjektlisteAsync<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0)
		{
			string i_strSql = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Select).FUN_strErstelleSqlStatement(i_edcSelectObjekt);
			return FUN_fdcErstelleObjektlisteAsync<T>(i_strSql, i_fdcDbTransaktion, i_dicParameter, i_i16AbfrageLimit);
		}

		public async Task<IEnumerable<T>> FUN_fdcErstelleObjektlisteAsync<T>(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0)
		{
			List<T> lstErgebnisse = new List<T>();
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return lstErgebnisse;
			}
			INF_ObjektAusReader<T> edcConverter = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataReaderConverter<T>();
			if (i_i16AbfrageLimit > 0)
			{
				i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit(i_strSql, i_i16AbfrageLimit);
			}
			DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				if (fdcConnection != null)
				{
					await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				using (DbDataReader fdcDataReader = await FUN_fdcErstelleCommand(i_strSql, fdcConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: true))
				{
					while (await fdcDataReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: true))
					{
						lstErgebnisse.Add(edcConverter.FUN_edcLeseObjektAusReader(fdcDataReader));
					}
				}
				return lstErgebnisse;
			}
			finally
			{
				fdcConnection?.Close();
			}
		}

		public T FUN_edcLeseObjekt<T>(T i_edcSelectObjekt, Dictionary<string, object> i_dicParameter = null, IDbTransaction i_fdcDbTransaktion = null)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return default(T);
			}
			INF_ObjektAusReader<T> iNF_ObjektAusReader = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataReaderConverter<T>();
			string i_strSql = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Select).FUN_strErstelleSqlStatement(i_edcSelectObjekt);
			DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				dbConnection?.Open();
				using (DbDataReader dbDataReader = FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReader())
				{
					if (dbDataReader.Read())
					{
						return iNF_ObjektAusReader.FUN_edcLeseObjektAusReader(dbDataReader);
					}
				}
				return default(T);
			}
			finally
			{
				dbConnection?.Close();
			}
		}

		public async Task<T> FUN_edcLeseObjektAsync<T>(T i_edcSelectObjekt, Dictionary<string, object> i_dicParameter = null, IDbTransaction i_fdcDbTransaktion = null)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return default(T);
			}
			INF_ObjektAusReader<T> edcConverter = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataReaderConverter<T>();
			INF_SqlStatementErsteller<T> iNF_SqlStatementErsteller = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Select);
			string strSql = iNF_SqlStatementErsteller.FUN_strErstelleSqlStatement(i_edcSelectObjekt);
			DbConnection fdcConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				if (fdcConnection != null)
				{
					await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				using (DbDataReader fdcDataReader = await FUN_fdcErstelleCommand(strSql, fdcConnection, i_fdcDbTransaktion, i_dicParameter).ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: true))
				{
					if (await fdcDataReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: true))
					{
						return edcConverter.FUN_edcLeseObjektAusReader(fdcDataReader);
					}
				}
				return default(T);
			}
			finally
			{
				fdcConnection?.Close();
			}
		}

		public DataTable FUN_fdcLeseTabelleVollstaendig(string i_strTabellenName)
		{
			return FUN_fdcLeseInDataTable($"Select * from {i_strTabellenName}", i_strTabellenName);
		}

		public Task<DataTable> FUN_fdcLeseTabelleVollstaendigAsync(string i_strTabellenName)
		{
			return Task.Factory.StartNew(() => FUN_fdcLeseTabelleVollstaendig(i_strTabellenName));
		}

		public DataTable FUN_fdcLeseInDataTable(string i_strSql, string i_strTabellenName = "")
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return null;
			}
			using (DbConnection dbConnection = PRO_edcDatenbankProvider.FUN_fdcGetConnection())
			{
				DataTable dataTable = new DataTable(i_strTabellenName);
				dbConnection.Open();
				PRO_edcDatenbankProvider.FUN_fdcGetDataAdapter(i_strSql, dbConnection).Fill(dataTable);
				return dataTable;
			}
		}

		public Task<DataTable> FUN_fdcLeseInDataTableAsync(string i_strSql, string i_strTabellenName = "")
		{
			return Task.Factory.StartNew(() => FUN_fdcLeseInDataTable(i_strSql, i_strTabellenName));
		}

		public DataTable FUN_fdcLeseInDataTable<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				return null;
			}
			DataTable dataTable = new DataTable(i_edcSelectObjekt.FUN_strHoleTabellenName());
			string i_strSql = EDC_ConverterFactory.FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp.Select).FUN_strErstelleSqlStatement(i_edcSelectObjekt);
			DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
			try
			{
				dbConnection?.Open();
				DbCommand i_fdcCommand = FUN_fdcErstelleCommand(i_strSql, dbConnection, i_fdcDbTransaktion, i_dicParameter);
				PRO_edcDatenbankProvider.FUN_fdcGetDataAdapter(i_fdcCommand).Fill(dataTable);
				return dataTable;
			}
			finally
			{
				dbConnection?.Close();
			}
		}

		public Task<DataTable> FUN_fdcLeseInDataTableAsync<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null)
		{
			return Task.Factory.StartNew(() => FUN_fdcLeseInDataTable(i_edcSelectObjekt, i_fdcDbTransaktion, i_dicParameter));
		}

		public void SUB_SchreibeObjekteInTabelle<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null)
		{
			IList<T> list = (i_lstObjektListe as IList<T>) ?? i_lstObjektListe.ToList();
			if (list.Any() && !m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				INF_ObjektZuDataRow<T> iNF_ObjektZuDataRow = EDC_ConverterFactory.FUN_edcErstelleObjektZuDataRowConverter<T>();
				string i_strSql = $"Select * from {list.FirstOrDefault().FUN_strHoleTabellenName()}";
				i_strSql = PRO_edcDatenbankProvider.FUN_strSetzeAbfrageErgebnisLimit(i_strSql, 1);
				DbConnection dbConnection = (i_fdcDbTransaktion == null) ? PRO_edcDatenbankProvider.FUN_fdcGetConnection() : null;
				try
				{
					dbConnection?.Open();
					DbDataAdapter dbDataAdapter = FUN_fdcErstelleDataAdapter(i_strSql, dbConnection, i_fdcDbTransaktion);
					PRO_edcDatenbankProvider.FUN_fdcGetCommandBuilder(dbDataAdapter);
					DataTable dataTable = new DataTable();
					dbDataAdapter.Fill(dataTable);
					foreach (T item in list)
					{
						DataRow dataRow = dataTable.NewRow();
						DataRow dataRow2 = iNF_ObjektZuDataRow.FUN_edcErstelleDataRowAusObjekt(dataTable, item);
						foreach (DataColumn column in dataTable.Columns)
						{
							if (dataTable.Columns.Contains(column.ColumnName))
							{
								dataRow[column.ColumnName] = dataRow2[column.ColumnName];
							}
						}
						dataTable.Rows.Add(dataRow);
					}
					dbDataAdapter.Update(dataTable);
				}
				finally
				{
					dbConnection?.Close();
				}
			}
		}

		public Task FUN_fdcSchreibeObjekteInTabelleAsync<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null)
		{
			return Task.Factory.StartNew(delegate
			{
				SUB_SchreibeObjekteInTabelle(i_lstObjektListe, i_fdcDbTransaktion);
			});
		}

		public uint FUN_u32HoleNaechstenSequenceWert(string i_strSequenceName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_u32HoleNaechstenSequenceWert(FUN_fdcScalareAbfrage, SUB_ExecuteStatement, i_strSequenceName);
		}

		public Task<uint> FUN_fdcHoleNaechstenSequenceWertAsync(string i_strSequenceName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_u32HoleNaechstenSequenceWertAsync(FUN_fdcScalareAbfrageAsync, FUN_fdcExecuteStatementAsync, i_strSequenceName);
		}

		public void SUB_SetzeSequenceWert(string i_strSequenceName, uint i_i32Wert)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				PRO_edcDatenbankProvider.SUB_SetzeSequenceWert(SUB_ExecuteStatement, i_strSequenceName, i_i32Wert);
			}
		}

		public async Task FUN_fdcSetzeSequenceWertAsync(string i_strSequenceName, uint i_i32Wert)
		{
			if (!m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				await PRO_edcDatenbankProvider.FUN_fdcSetzeSequenceWertAsync(FUN_fdcExecuteStatementAsync, i_strSequenceName, i_i32Wert).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		public bool FUN_blnIstDatenbankDateibasiert()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.PRO_blnIstDateibasierendeDatenbank;
		}

		public bool FUN_blnExistiertDieTabelle(string i_strTabellenName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_blnExistiertTabelle(FUN_fdcScalareAbfrage, i_strTabellenName);
		}

		public bool FUN_blnExistiertDieDatenbank(string i_strDatenbankname)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_blnExistiertDieDatenbank(FUN_fdcScalareAbfrage, i_strDatenbankname);
		}

		public bool FUN_blnExistiertTablespace(string i_strTablespaceName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_blnExistiertTablespace(FUN_fdcScalareAbfrage, i_strTablespaceName);
		}

		public bool FUN_blnExistiertSequence(string i_strSequenceName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_blnExistiertTablespace(FUN_fdcScalareAbfrage, i_strSequenceName);
		}

		public void SUB_ErstelleDieDatenbank()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			EDC_DatenbankErsteller eDC_DatenbankErsteller = new EDC_DatenbankErsteller(PRO_edcDatenbankModell, this);
			if (PRO_edcDatenbankProvider.PRO_blnIstDateibasierendeDatenbank)
			{
				PRO_edcDatenbankProvider.SUB_ErstelleDateibasierendeDatenbank();
			}
			else
			{
				eDC_DatenbankErsteller.SUB_ErstelleServerbasierendeDatenbank();
			}
		}

		public void SUB_ErstelleDieDatenbankKomponenten()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			new EDC_DatenbankErsteller(PRO_edcDatenbankModell, this).SUB_ErstelleDatenbankKomponenten();
		}

		public void SUB_FuehreDatenbankUpdatesDurch(int i_i16AktuelleVersion, int i_i32EndVersion, Action<long> i_delParameterUpdateAction)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			new EDC_DatenbankUpdater(PRO_edcDatenbankModell, this).SUB_FuehreDatenbankUpdatesDurch(i_i16AktuelleVersion, i_i32EndVersion, i_delParameterUpdateAction);
		}

		public void SUB_ErstelleEineZusatztabelle(object i_objModell)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			new EDC_DatenbankErsteller(PRO_edcDatenbankModell, this).SUB_ErstelleEineZusatztabelle(i_objModell);
		}

		public void SUB_FuegeTabellenSpaltenHinzu(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strSql = PRO_edcDatenbankModell.FUN_strHoleAlterTableAddColumnStatement(i_strTabellenName, i_lstSpalten);
			SUB_ExecuteStatement(i_strSql);
		}

		public void SUB_LoescheTabellenSpalten(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			if (PRO_edcDatenbankProvider.PRO_blnKannMehrerTabellenSpaltenAufEinmalDroppen)
			{
				string i_strSql = PRO_edcDatenbankModell.FUN_strHoleAlterTableDropColumnStatement(i_strTabellenName, i_lstSpalten);
				SUB_ExecuteStatement(i_strSql);
			}
			else
			{
				List<EDC_DynamischeSpalte> list = new List<EDC_DynamischeSpalte>();
				foreach (EDC_DynamischeSpalte item in i_lstSpalten)
				{
					list.Clear();
					list.Add(item);
					string i_strSql2 = PRO_edcDatenbankModell.FUN_strHoleAlterTableDropColumnStatement(i_strTabellenName, list);
					SUB_ExecuteStatement(i_strSql2);
				}
			}
		}

		public Task FUN_fdcKomprimiereDatenbankAsync()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strDatenbankName = PRO_edcDatenbankProvider.FUN_strHoleDatenbankName();
			return PRO_edcDatenbankProvider.FUN_fdcKomprimiereDatenbankAsync(FUN_fdcExecuteStatementAsync, i_strDatenbankName);
		}

		public Task FUN_fdcRepariereDatenbankAsync()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strDatenbankName = PRO_edcDatenbankProvider.FUN_strHoleDatenbankName();
			return PRO_edcDatenbankProvider.FUN_fdcRepariereDatenbankAsync(FUN_fdcExecuteStatementAsync, i_strDatenbankName);
		}

		public Task FUN_fdcVerkleinereDatenbankAsync()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strDatenbankName = PRO_edcDatenbankProvider.FUN_strHoleDatenbankName();
			return PRO_edcDatenbankProvider.FUN_fdcVerkleinereDatenbankAsync(FUN_fdcExecuteStatementAsync, i_strDatenbankName);
		}

		public Task FUN_fdcDatensicherungAsync(string i_strSicherungsverzeichnis)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string text = PRO_edcDatenbankProvider.FUN_strHoleDatenbankName();
			if (PRO_edcDatenbankProvider.PRO_blnIstDateibasierendeDatenbank)
			{
				FileInfo fileInfo = new FileInfo(text);
				text = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
			}
			string i_strBenutzername = PRO_edcDatenbankProvider.FUN_strHoleDatenbankBenutzerName();
			string i_strPasswort = PRO_edcDatenbankProvider.FUN_strHoleDatenbankKennwort();
			string path = string.Format("{0}_{1}_{2}.{3}", text, DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), PRO_edcDatenbankProvider.PRO_strDatenbankIdentifier, PRO_edcDatenbankProvider.PRO_strDatensicherungErweiterung);
			string i_strSicherungspfad = Path.Combine(i_strSicherungsverzeichnis, path);
			return PRO_edcDatenbankProvider.FUN_fdcDatensicherungAsync(FUN_fdcExecuteStatementAsync, text, i_strBenutzername, i_strPasswort, i_strSicherungspfad);
		}

		public Task FUN_fdcReorganisiereDatenbankAsync()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strDatenbankName = PRO_edcDatenbankProvider.FUN_strHoleDatenbankName();
			return PRO_edcDatenbankProvider.FUN_fdcReorganisiereDatenbankAsync(FUN_fdcExecuteStatementAsync, i_strDatenbankName);
		}

		public IEnumerable<string> FUN_enuHoleListeAllerTabellen()
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strSql = PRO_edcDatenbankProvider.FUN_strHoleVorhandeneTabellenAbfrage();
			return FUN_enuErstelleScalareObjektliste(i_strSql).Cast<string>();
		}

		public async Task<DataTable> FUN_fdcLeseTabellenSchemaAsync(string i_strTabelle)
		{
			DbConnection fdcConnection = PRO_edcDatenbankProvider.FUN_fdcGetConnection();
			try
			{
				if (fdcConnection != null)
				{
					await fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				using (DbDataReader dbDataReader = await PRO_edcDatenbankProvider.FUN_fdcGetCommand($"SELECT * FROM {i_strTabelle}", fdcConnection).ExecuteReaderAsync(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo).ConfigureAwait(continueOnCapturedContext: true))
				{
					return dbDataReader.GetSchemaTable();
				}
			}
			finally
			{
				fdcConnection?.Close();
			}
		}

		public string FUN_strErstelleCreateTableScriptAusSchema(DataTable i_fdcSchema, string i_strTabelle)
		{
			string i_strTablespace = PRO_edcDatenbankProvider.FUN_strHoleTablespaceFuerTabelle(FUN_fdcScalareAbfrage, i_strTabelle);
			return PRO_edcDatenbankProvider.PRO_edcDialekt.FUN_strHoleTabellenErstellungsStatement(i_fdcSchema, i_strTabelle, i_strTablespace);
		}

		public void SUB_TrenneDatenbankverbindungen(string i_strDatenbankname)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			PRO_edcDatenbankProvider.SUB_TrenneDatenbankverbindungen(SUB_ExecuteStatement, i_strDatenbankname);
		}

		public void SUB_DropDatabase(string i_strDatenbankname)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			PRO_edcDatenbankProvider.SUB_DropDatabase(SUB_ExecuteStatement, i_strDatenbankname);
		}

		public void SUB_LeereTabelle(string i_strTabellenName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			PRO_edcDatenbankProvider.SUB_LeereTabelle(SUB_ExecuteStatement, i_strTabellenName);
		}

		public Task FUN_fdcLeereTabelleAsync(string i_strTabellenName)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return PRO_edcDatenbankProvider.FUN_fdcLeereTabelleAsync(FUN_fdcExecuteStatementAsync, i_strTabellenName);
		}

		public async Task<long> FUN_fdcHoleMaxNummerischenPrimaryKeyWertAsync<T>(T i_edcObjekt)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			string i_strSql = i_edcObjekt.FUN_strErstellePrimaryKeyMaxStatement();
			object obj = await FUN_fdcScalareAbfrageAsync(i_strSql).ConfigureAwait(continueOnCapturedContext: true);
			if (obj != null && obj != DBNull.Value)
			{
				return Convert.ToInt64(obj);
			}
			return 0L;
		}

		public async Task<long> FUN_fdcHoleNaechstenNummerischenPrimaryKeyWertAsync<T>(T i_edcObjekt)
		{
			if (m_blnDatenProviderWechselWirdDurchgefuehrt)
			{
				throw new EDC_DatenbankZugriffsException("data provider change in progress");
			}
			return await FUN_fdcHoleMaxNummerischenPrimaryKeyWertAsync(i_edcObjekt).ConfigureAwait(continueOnCapturedContext: true) + 1;
		}

		public string FUN_strHoleServerName()
		{
			return PRO_edcDatenbankProvider.FUN_strHoleServerName();
		}

		public NameValueCollection FUN_fdcHoleProviderEinstellungen()
		{
			return PRO_edcDatenbankProvider.FUN_fdcHoleProviderEinstellungen();
		}

		private static void SUB_WarteAsynchron(int i_i32Wartezeit)
		{
			Task.Run(async delegate
			{
				await Task.Delay(i_i32Wartezeit).ConfigureAwait(continueOnCapturedContext: true);
				return true;
			}).Wait(i_i32Wartezeit * 2);
		}

		private DbCommand FUN_fdcErstelleCommand(string i_strSql, DbConnection i_fdcConnection, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			if (i_fdcDbTransaktion != null)
			{
				return PRO_edcDatenbankProvider.FUN_fdcGetCommand(i_strSql, i_fdcDbTransaktion, i_dicParameter);
			}
			return PRO_edcDatenbankProvider.FUN_fdcGetCommand(i_strSql, i_fdcConnection, i_dicParameter);
		}

		private DbDataAdapter FUN_fdcErstelleDataAdapter(string i_strSql, DbConnection i_fdcConnection, IDbTransaction i_fdcDbTransaktion)
		{
			if (i_fdcConnection != null)
			{
				return PRO_edcDatenbankProvider.FUN_fdcGetDataAdapter(i_strSql, i_fdcConnection);
			}
			return PRO_edcDatenbankProvider.FUN_fdcGetDataAdapter(i_strSql, i_fdcDbTransaktion);
		}

		private bool FUN_blnCheckDatabaseService()
		{
			if (PRO_edcDatenbankProvider.PRO_blnIstDateibasierendeDatenbank)
			{
				return true;
			}
			if (!EDC_HostHelfer.FUN_blnIstHostaAdressLokal(FUN_strHoleServerName()))
			{
				return true;
			}
			List<string> list = (from i_strDienst in EDC_DienstHelfer.FUN_enuErstellerInstallierteDiensteListe().ToList()
			where i_strDienst.StartsWith(PRO_edcDatenbankProvider.PRO_strDatenbankDienstName)
			select i_strDienst).ToList();
			bool flag = true;
			foreach (string item in list)
			{
				flag &= EDC_DienstHelfer.FUN_blnIstDienstGestartet(item);
			}
			return flag;
		}
	}
}
