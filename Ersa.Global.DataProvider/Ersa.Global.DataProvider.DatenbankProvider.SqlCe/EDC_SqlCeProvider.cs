using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Factories.StrategieFactory;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Global.DataProvider.DatenbankProvider.SqlCe
{
	public class EDC_SqlCeProvider : EDC_BasisProvider, INF_DatenbankProvider
	{
		private static readonly SemaphoreSlim ms_fdcAsyncLock = new SemaphoreSlim(1);

		private readonly object m_fdcLockObject = new object();

		private DbConnectionStringBuilder m_fdcConnectionStringBuilder;

		private INF_DatenbankDialekt m_edcDialekt;

		public bool PRO_blnIstDateibasierendeDatenbank => true;

		public bool PRO_blnKannMehrerTabellenSpaltenAufEinmalDroppen => false;

		public string PRO_strConnectionString
		{
			get
			{
				if (m_fdcConnectionStringBuilder == null)
				{
					m_fdcConnectionStringBuilder = FUN_fdcConnectionStringBuilderErstellen();
				}
				return m_fdcConnectionStringBuilder.ConnectionString;
			}
		}

		public INF_DatenbankDialekt PRO_edcDialekt
		{
			get
			{
				if (m_edcDialekt == null)
				{
					m_edcDialekt = EDC_DatenbankDialektFactory.FUN_edcHoleDatenbankDialekt(ENUM_DatenbankTyp.SqlCe);
				}
				return m_edcDialekt;
			}
		}

		public string PRO_strDatensicherungErweiterung => "sdfcpy";

		public string PRO_strDatenbankIdentifier => "sqlce";

		public string PRO_strDatenbankDienstName => string.Empty;

		public EDC_SqlCeProvider(NameValueCollection i_fdcAppSettings)
			: base(i_fdcAppSettings)
		{
		}

		public string FUN_strHoleVorhandeneTabellenAbfrage()
		{
			return PRO_edcDialekt.FUN_strHoleTabellenListeStatement(FUN_strHoleDatenbankName());
		}

		public DbConnection FUN_fdcGetConnection()
		{
			return new SqlCeConnection(PRO_strConnectionString);
		}

		public DbCommand FUN_fdcGetCommand()
		{
			return new SqlCeCommand();
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCeCommand sqlCeCommand = new SqlCeCommand(i_strSql);
			SUB_SetzeCommandParameter(sqlCeCommand, i_dicParameter);
			return sqlCeCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, DbConnection i_fdcConnection, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCeCommand sqlCeCommand = new SqlCeCommand(i_strSql, (SqlCeConnection)i_fdcConnection);
			SUB_SetzeCommandParameter(sqlCeCommand, i_dicParameter);
			return sqlCeCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCeCommand sqlCeCommand = new SqlCeCommand(i_strSql, (SqlCeConnection)i_fdcDbTransaktion.Connection, (SqlCeTransaction)i_fdcDbTransaktion);
			SUB_SetzeCommandParameter(sqlCeCommand, i_dicParameter);
			return sqlCeCommand;
		}

		public void SUB_SetzeCommandParameter(DbCommand i_fdcCommand, Dictionary<string, object> i_dicParameter)
		{
			if (i_dicParameter != null)
			{
				SqlCeCommand sqlCeCommand = i_fdcCommand as SqlCeCommand;
				if (sqlCeCommand != null)
				{
					foreach (KeyValuePair<string, object> item in i_dicParameter)
					{
						sqlCeCommand.Parameters.AddWithValue(item.Key, item.Value);
					}
				}
			}
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(DbCommand i_fdcCommand)
		{
			return new SqlCeDataAdapter((SqlCeCommand)i_fdcCommand);
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, IDbTransaction i_fdcDdTransaktion)
		{
			return new SqlCeDataAdapter((SqlCeCommand)FUN_fdcGetCommand(i_strSql, i_fdcDdTransaktion));
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, DbConnection i_fdcConnection)
		{
			return new SqlCeDataAdapter((SqlCeCommand)FUN_fdcGetCommand(i_strSql, i_fdcConnection));
		}

		public DbCommandBuilder FUN_fdcGetCommandBuilder(DbDataAdapter i_fdcAdapter)
		{
			return new SqlCeCommandBuilder((SqlCeDataAdapter)i_fdcAdapter);
		}

		public string FUN_strSetzeAbfrageErgebnisLimit(string i_strSql, int i_i32Limit)
		{
			int num = i_strSql.ToLower().IndexOf("select", StringComparison.Ordinal);
			if (num != -1)
			{
				i_strSql = i_strSql.Insert(num + "select".Length, $" top {Convert.ToString(i_i32Limit)} ");
			}
			return i_strSql;
		}

		public bool FUN_blnExistiertTablespace(Func<string, object> i_delAbfrageFunc, string i_strTablespaceName)
		{
			return true;
		}

		public bool FUN_blnExistiertSequence(Func<string, object> i_delAbfrageFunc, string i_strSequenceName)
		{
			string arg = PRO_edcDialekt.FUN_strHoleSequenceAbfrage(i_strSequenceName);
			object obj = i_delAbfrageFunc(arg);
			return obj != DBNull.Value && Convert.ToUInt32(obj) != 0;
		}

		public uint FUN_u32HoleNaechstenSequenceWert(Func<string, object> i_delAbfrageFunc, Action<string> i_delUpdateFunc, string i_strSequenceName)
		{
			object obj2;
			lock (m_fdcLockObject)
			{
				string arg = PRO_edcDialekt.FUN_strHoleSequenceAbfrage(i_strSequenceName);
				string obj = FUN_strHoleSequenceSetzenAnweisung(i_strSequenceName);
				obj2 = i_delAbfrageFunc(arg);
				if (obj2 != DBNull.Value)
				{
					i_delUpdateFunc(obj);
				}
			}
			if (obj2 == DBNull.Value)
			{
				return 0u;
			}
			return Convert.ToUInt32(obj2);
		}

		public async Task<uint> FUN_u32HoleNaechstenSequenceWertAsync(Func<string, Task<object>> i_delAbfrageFuncAsync, Func<string, Task> i_delUpdateFuncAsync, string i_strSequenceName)
		{
			try
			{
				await ms_fdcAsyncLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
				string arg = PRO_edcDialekt.FUN_strHoleSequenceAbfrage(i_strSequenceName);
				string strUpdateSql = FUN_strHoleSequenceSetzenAnweisung(i_strSequenceName);
				object fdcWert = await i_delAbfrageFuncAsync(arg).ConfigureAwait(continueOnCapturedContext: true);
				if (fdcWert != DBNull.Value)
				{
					await i_delUpdateFuncAsync(strUpdateSql).ConfigureAwait(continueOnCapturedContext: true);
				}
				return (fdcWert != DBNull.Value) ? Convert.ToUInt32(fdcWert) : 0u;
			}
			finally
			{
				ms_fdcAsyncLock.Release();
			}
		}

		public void SUB_SetzeSequenceWert(Action<string> i_delUpdateFunc, string i_strSequenceName, uint i_i32Wert)
		{
			string obj = PRO_edcDialekt.FUN_strHoleSequenceWertSetzenStatement(i_strSequenceName, i_i32Wert);
			i_delUpdateFunc(obj);
		}

		public Task FUN_fdcSetzeSequenceWertAsync(Func<string, Task> i_delUpdateFuncAsync, string i_strSequenceName, uint i_i32Wert)
		{
			string arg = PRO_edcDialekt.FUN_strHoleSequenceWertSetzenStatement(i_strSequenceName, i_i32Wert);
			return i_delUpdateFuncAsync(arg);
		}

		public bool FUN_blnExistiertDieDatenbank(Func<string, object> i_delAbfrageFunc, string i_strDatenbankname)
		{
			try
			{
				return File.Exists(new FileInfo(i_strDatenbankname).FullName);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool FUN_blnExistiertTabelle(Func<string, object> i_delAbfrageFunc, string i_strTabellenName)
		{
			try
			{
				string arg = PRO_edcDialekt.FUN_strHoleTabelleExistiertStatement(i_strTabellenName);
				object obj = i_delAbfrageFunc(arg);
				return obj != DBNull.Value && Convert.ToUInt32(obj) != 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public string FUN_strHoleTablespaceFuerTabelle(Func<string, object> i_delAbfrageFunc, string i_strTabellenName)
		{
			return string.Empty;
		}

		public void SUB_ErstelleDateibasierendeDatenbank()
		{
			FileInfo fileInfo = new FileInfo(FUN_strHoleDatenbankName());
			if (!Directory.Exists(fileInfo.DirectoryName))
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
			SqlCeEngine sqlCeEngine = new SqlCeEngine(PRO_strConnectionString);
			sqlCeEngine.CreateDatabase();
			sqlCeEngine.Dispose();
		}

		public Task FUN_fdcKomprimiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return FUN_fdcKomprimiereDatenbankAsync();
		}

		public Task FUN_fdcRepariereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return Task.Run(delegate
			{
				SqlCeEngine sqlCeEngine = new SqlCeEngine(PRO_strConnectionString);
				sqlCeEngine.Repair(null, RepairOption.DeleteCorruptedRows);
				sqlCeEngine.Dispose();
			});
		}

		public Task FUN_fdcVerkleinereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return Task.Run(delegate
			{
				SqlCeEngine sqlCeEngine = new SqlCeEngine(PRO_strConnectionString);
				sqlCeEngine.Shrink();
				sqlCeEngine.Dispose();
			});
		}

		public Task FUN_fdcDatensicherungAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName, string i_strBenutzername, string i_strPasswort, string i_strSicherungspfad)
		{
			return Task.Run(delegate
			{
				File.Copy(FUN_strHoleDatenbankName(), i_strSicherungspfad);
			});
		}

		public Task FUN_fdcReorganisiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return FUN_fdcKomprimiereDatenbankAsync();
		}

		public void SUB_TrenneDatenbankverbindungen(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
		}

		public void SUB_DropDatabase(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
			File.Delete(FUN_strHoleDatenbankName());
		}

		public string FUN_strHoleSequenceSetzenAnweisung(string i_strSequenceName)
		{
			return $"UPDATE Sequences SET Value = Value + 1 Where Name='{i_strSequenceName}'";
		}

		public void SUB_LeereTabelle(Action<string> i_delUpdateFunc, string i_strTabellenName)
		{
			string obj = $"Delete From {i_strTabellenName};";
			i_delUpdateFunc(obj);
		}

		public Task FUN_fdcLeereTabelleAsync(Func<string, Task> i_delUpdateFunc, string i_strTabellenName)
		{
			string arg = $"Delete From {i_strTabellenName};";
			return i_delUpdateFunc(arg);
		}

		protected override DbConnectionStringBuilder FUN_fdcConnectionStringBuilderErstellen()
		{
			return new DbConnectionStringBuilder
			{
				{
					"Data Source",
					FUN_strHoleDatenbankName()
				},
				{
					"Max Database Size",
					base.PRO_fdcEinstellungen["MaxDatabaseSize"]
				}
			};
		}

		private Task FUN_fdcKomprimiereDatenbankAsync()
		{
			return Task.Run(delegate
			{
				SqlCeEngine sqlCeEngine = new SqlCeEngine(PRO_strConnectionString);
				sqlCeEngine.Compact(null);
				sqlCeEngine.Dispose();
			});
		}
	}
}
