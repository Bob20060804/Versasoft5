using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Factories.StrategieFactory;
using Ersa.Global.DataProvider.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Global.DataProvider.DatenbankProvider.Postgres
{
	public class EDC_PostgresProvider : EDC_BasisProvider, INF_DatenbankProvider
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
					m_edcDialekt = EDC_DatenbankDialektFactory.FUN_edcHoleDatenbankDialekt(ENUM_DatenbankTyp.Postgres);
				}
				return m_edcDialekt;
			}
		}

		public string PRO_strDatensicherungErweiterung => "backup";

		public string PRO_strDatenbankIdentifier => "postgr";

		public string PRO_strDatenbankDienstName => "postgresql-x64";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_fdcAppSettings"></param>
		public EDC_PostgresProvider(NameValueCollection i_fdcAppSettings)
            : base(i_fdcAppSettings)
		{
		}

		public string FUN_strHoleVorhandeneTabellenAbfrage()
		{
			return PRO_edcDialekt.FUN_strHoleTabellenListeStatement(FUN_strHoleDatenbankName());
		}

		public DbConnection FUN_fdcGetConnection()
		{
			return new NpgsqlConnection(PRO_strConnectionString);
		}

		public DbCommand FUN_fdcGetCommand()
		{
			return new NpgsqlCommand();
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, Dictionary<string, object> i_dicParameter = null)
		{
			NpgsqlCommand npgsqlCommand = new NpgsqlCommand(i_strSql);
			SUB_SetzeCommandParameter(npgsqlCommand, i_dicParameter);
			return npgsqlCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, DbConnection i_fdcConnection, Dictionary<string, object> i_dicParameter = null)
		{
			NpgsqlCommand npgsqlCommand = new NpgsqlCommand(i_strSql, (NpgsqlConnection)i_fdcConnection);
			SUB_SetzeCommandParameter(npgsqlCommand, i_dicParameter);
			return npgsqlCommand;
		}

		public DbCommand FUN_fdcGetCommand(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null)
		{
			NpgsqlCommand npgsqlCommand = new NpgsqlCommand(i_strSql, (NpgsqlConnection)i_fdcDbTransaktion.Connection, (NpgsqlTransaction)i_fdcDbTransaktion);
			SUB_SetzeCommandParameter(npgsqlCommand, i_dicParameter);
			return npgsqlCommand;
		}

		public void SUB_SetzeCommandParameter(DbCommand i_fdcCommand, Dictionary<string, object> i_dicParameter)
		{
			if (i_dicParameter != null)
			{
				NpgsqlCommand npgsqlCommand = i_fdcCommand as NpgsqlCommand;
				if (npgsqlCommand != null)
				{
					foreach (KeyValuePair<string, object> item in i_dicParameter)
					{
						npgsqlCommand.Parameters.AddWithValue(item.Key, item.Value);
					}
				}
			}
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(DbCommand i_fdcCommand)
		{
			return new NpgsqlDataAdapter((NpgsqlCommand)i_fdcCommand);
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, DbConnection i_fdcConnection)
		{
			return new NpgsqlDataAdapter((NpgsqlCommand)FUN_fdcGetCommand(i_strSql, i_fdcConnection));
		}

		public DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, IDbTransaction i_fdcDdTransaktion)
		{
			return new NpgsqlDataAdapter((NpgsqlCommand)FUN_fdcGetCommand(i_strSql, i_fdcDdTransaktion));
		}

		public DbCommandBuilder FUN_fdcGetCommandBuilder(DbDataAdapter i_fdcAdapter)
		{
			return new NpgsqlCommandBuilder((NpgsqlDataAdapter)i_fdcAdapter);
		}

		public string FUN_strSetzeAbfrageErgebnisLimit(string i_strSql, int i_i32Limit)
		{
			i_strSql += $" limit {Convert.ToString(i_i32Limit)}";
			return i_strSql;
		}

		public bool FUN_blnExistiertTablespace(Func<string, object> i_delAbfrageFunc, string i_strTablespaceName)
		{
			string arg = PRO_edcDialekt.FUN_strHoleTablespaceExistenzAbfrage(i_strTablespaceName);
			object obj = i_delAbfrageFunc(arg);
			return obj != DBNull.Value && Convert.ToUInt32(obj) != 0;
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
			try
			{
				string arg = PRO_edcDialekt.FUN_strHoleTablespaceFuerTabelleStatement(i_strTabellenName);
				return Convert.ToString(i_delAbfrageFunc(arg));
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public void SUB_ErstelleDateibasierendeDatenbank()
		{
		}

		public Task FUN_fdcKomprimiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = "VACUUM";
			return i_delExecuteFuncAsync(arg);
		}

		public Task FUN_fdcRepariereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return FUN_fdcVerkleinereDatenbankAsync(i_delExecuteFuncAsync, i_strDatenbankName);
		}

		public Task FUN_fdcVerkleinereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			return i_delExecuteFuncAsync("VACUUM FULL");
		}

		public Task FUN_fdcDatensicherungAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName, string i_strBenutzername, string i_strPasswort, string i_strSicherungspfad)
		{
			return Task.Run(delegate
			{
				string text = Path.Combine(FUN_strHoleDatenbankBinaerVerzeichnis(), "pg_dump.exe");
				if (File.Exists(text))
				{
					string arg = "\"" + i_strSicherungspfad + "\"";
					Process process = Process.Start(new ProcessStartInfo
					{
						EnvironmentVariables = 
						{
							{
								"PGPASSWORD",
								i_strPasswort
							}
						},
						FileName = text,
						Arguments = $"-U {i_strBenutzername} -F c -f {arg} {i_strDatenbankName.ToLower()}",
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true
					});
					if (process != null)
					{
						while (!process.HasExited)
						{
							Task.Delay(1000).Wait(2000);
						}
					}
				}
			});
		}

		public Task FUN_fdcReorganisiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName)
		{
			string arg = $"Reindex database {i_strDatenbankName}";
			return i_delExecuteFuncAsync(arg);
		}

		public void SUB_TrenneDatenbankverbindungen(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
			string obj = $"select pg_terminate_backend(pg_stat_activity.pid) from pg_stat_activity where datname='{i_strDatenbankName.ToLower()}' and pid <> pg_backend_pid()";
			i_delExecuteFunc(obj);
		}

		public void SUB_DropDatabase(Action<string> i_delExecuteFunc, string i_strDatenbankName)
		{
			string obj = $"Drop Database If Exists {i_strDatenbankName.ToLower()}";
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
					"Port",
					base.PRO_fdcEinstellungen["Port"]
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
					"MinPoolSize",
					base.PRO_fdcEinstellungen["MinPoolSize"]
				},
				{
					"MaxPoolSize",
					base.PRO_fdcEinstellungen["MaxPoolSize"]
				},
				{
					"CommandTimeout",
					base.PRO_fdcEinstellungen["CommandTimeout"]
				},
				{
					"Timeout",
					base.PRO_fdcEinstellungen["ConnectionLifeTime"]
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
