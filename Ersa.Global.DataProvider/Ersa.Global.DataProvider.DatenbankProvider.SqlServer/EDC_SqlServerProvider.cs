using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Factories.StrategieFactory;
using Ersa.Global.DataProvider.Helfer;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Ersa.Global.DataProvider.DatenbankProvider.SqlServer
{
	public class EDC_SqlServerProvider : EDC_BasisProvider, INF_DatenbankProvider
	{
		private DbConnectionStringBuilder m_fdcConnectionStringBuilder;

		private INF_DatenbankDialekt m_edcDialekt;

		public bool PRO_blnIstDateibasierendeDatenbank => false;

		public bool PRO_blnKannMehrerTabellenSpaltenAufEinmalDroppen => true;

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
					m_edcDialekt = EDC_DatenbankDialektFactory.FUN_edcHoleDatenbankDialekt(ENUM_DatenbankTyp.SqlServer);
				}
				return m_edcDialekt;
			}
		}

		public string PRO_strDatensicherungErweiterung => "backup";

		public string PRO_strDatenbankIdentifier => "sqlsrv";

		public string PRO_strDatenbankDienstName => "MSSQL$";

		public EDC_SqlServerProvider(NameValueCollection i_fdcAppSettings)
			: base(i_fdcAppSettings)
		{
		}

		public string FUN_strHoleVorhandeneTabellenAbfrage()
		{
			return PRO_edcDialekt.FUN_strHoleTabellenListeStatement(FUN_strHoleDatenbankName());
		}

		public DbConnection FUN_fdcGetConnection()
		{
			return new SqlConnection(PRO_strConnectionString);
		}

		public DbCommand FUN_fdcGetCommand()
		{
			return new SqlCommand();
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCommand sqlCommand = new SqlCommand(i_strSql);
			SUB_SetzeCommandParameter(sqlCommand, i_dicParameter);
			return sqlCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, DbConnection i_fdcConnection, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCommand sqlCommand = new SqlCommand(i_strSql, (SqlConnection)i_fdcConnection);
			SUB_SetzeCommandParameter(sqlCommand, i_dicParameter);
			return sqlCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			SqlCommand sqlCommand = new SqlCommand(i_strSql, (SqlConnection)i_fdcDbTransaktion.Connection, (SqlTransaction)i_fdcDbTransaktion);
			SUB_SetzeCommandParameter(sqlCommand, i_dicParameter);
			return sqlCommand;
		}

		public void SUB_SetzeCommandParameter(DbCommand i_fdcCommand, Dictionary<string, object> i_dicParameter)
		{
			if (i_dicParameter != null)
			{
				SqlCommand sqlCommand = i_fdcCommand as SqlCommand;
				if (sqlCommand != null)
				{
					foreach (KeyValuePair<string, object> item in i_dicParameter)
					{
						sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
					}
				}
			}
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(DbCommand i_fdcCommand)
		{
			return new SqlDataAdapter((SqlCommand)i_fdcCommand);
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, IDbTransaction i_fdcDdTransaktion)
		{
			return new SqlDataAdapter((SqlCommand)FUN_fdcGetCommand(i_strSql, i_fdcDdTransaktion));
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, DbConnection i_fdcConnection)
		{
			return new SqlDataAdapter((SqlCommand)FUN_fdcGetCommand(i_strSql, i_fdcConnection));
		}

		public DbCommandBuilder FUN_fdcGetCommandBuilder(DbDataAdapter i_fdcAdapter)
		{
			return new SqlCommandBuilder((SqlDataAdapter)i_fdcAdapter);
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
			string arg = PRO_edcDialekt.FUN_strHoleSequenceExistenzAbfrage(i_strSequenceName);
			object obj = i_delAbfrageFunc(arg);
			return obj != DBNull.Value && Convert.ToUInt32(obj) != 0;
		}

		public uint FUN_u32HoleNaechstenSequenceWert(Func<string, object> i_delAbfrageFunc, Action<string> i_delUpdateFunc, string i_strSequenceName)
		{
			string arg = PRO_edcDialekt.FUN_strHoleSequenceAbfrage(i_strSequenceName);
			object obj = i_delAbfrageFunc(arg);
			if (obj == DBNull.Value)
			{
				return 0u;
			}
			return Convert.ToUInt32(obj);
		}

		public async Task<uint> FUN_u32HoleNaechstenSequenceWertAsync(Func<string, Task<object>> i_delAbfrageFuncAsync, Func<string, Task> i_delUpdateFuncAsync, string i_strSequenceName)
		{
			string arg = PRO_edcDialekt.FUN_strHoleSequenceAbfrage(i_strSequenceName);
			object obj = await i_delAbfrageFuncAsync(arg).ConfigureAwait(continueOnCapturedContext: true);
			return (obj != DBNull.Value) ? Convert.ToUInt32(obj) : 0u;
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
				string arg = PRO_edcDialekt.FUN_strHoleDatenbankExistiertStatement(i_strDatenbankname);
				object obj = i_delAbfrageFunc(arg);
				return obj != DBNull.Value && Convert.ToUInt32(obj) != 0;
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
		}

		public Task FUN_fdcKomprimiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = $"DBCC SHRINKDATABASE ('{i_strDatenbankName}', NOTRUNCATE)";
			return i_delExecuteFuncAsync(arg);
		}

		public Task FUN_fdcRepariereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DBCC CHECKDB ('{0}'); ALTER DATABASE [{0}] SET MULTI_USER WITH ROLLBACK IMMEDIATE", i_strDatenbankName);
			return i_delExecuteFuncAsync(arg);
		}

		public Task FUN_fdcVerkleinereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = $"DBCC SHRINKDATABASE ('{i_strDatenbankName}', TRUNCATEONLY)";
			return i_delExecuteFuncAsync(arg);
		}

		public Task FUN_fdcDatensicherungAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName, string i_strBenutzername, string i_strPasswort, string i_strSicherungspfad)
		{
			string arg = EDC_UncPfadHelfer.FUN_strHoleUncPfad(i_strSicherungspfad);
			string arg2 = $"BACKUP DATABASE {i_strDatenbankName} TO DISK = '{arg}'";
			return i_delExecuteFuncAsync(arg2);
		}

		public Task FUN_fdcReorganisiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = "EXEC sp_MSforeachtable @command1=\"ALTER INDEX ALL ON ? REORGANIZE\"";
			return i_delExecuteFuncAsync(arg);
		}

		public void SUB_TrenneDatenbankverbindungen(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
			string obj = $"ALTER DATABASE [{i_strDatenbankName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
			i_delExecuteFunc(obj);
		}

		public void SUB_DropDatabase(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
			string obj = $"Drop Database {i_strDatenbankName}";
			i_delExecuteFunc(obj);
		}

		public void SUB_LeereTabelle(Action<string> i_delUpdateFunc, string i_strTabellenName)
		{
			string obj = $"Truncate Table {i_strTabellenName};";
			i_delUpdateFunc(obj);
		}

		public Task FUN_fdcLeereTabelleAsync(Func<string, Task> i_delUpdateFunc, string i_strTabellenName)
		{
			string arg = $"Truncate Table {i_strTabellenName};";
			return i_delUpdateFunc(arg);
		}

		protected override DbConnectionStringBuilder FUN_fdcConnectionStringBuilderErstellen()
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder
			{
				{
					"Server",
					base.PRO_fdcEinstellungen["Server"]
				},
				{
					"User Id",
					base.PRO_fdcEinstellungen["UserId"]
				},
				{
					"Password",
					base.PRO_fdcEinstellungen["Password"]
				},
				{
					"Pooling",
					base.PRO_fdcEinstellungen["Pooling"]
				},
				{
					"Min Pool Size",
					base.PRO_fdcEinstellungen["MinPoolSize"]
				},
				{
					"Max Pool Size",
					base.PRO_fdcEinstellungen["MaxPoolSize"]
				},
				{
					"Connection LifeTime",
					base.PRO_fdcEinstellungen["ConnectionLifeTime"]
				},
				{
					"Asynchronous Processing",
					"True"
				}
			};
			string text = base.PRO_fdcEinstellungen["Database"];
			if (!string.IsNullOrEmpty(text))
			{
				dbConnectionStringBuilder.Add("Database", text.ToLower());
			}
			return dbConnectionStringBuilder;
		}
	}
}
