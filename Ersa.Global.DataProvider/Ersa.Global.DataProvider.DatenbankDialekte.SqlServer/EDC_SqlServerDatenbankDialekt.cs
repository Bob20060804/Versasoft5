using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Ersa.Global.DataProvider.DatenbankDialekte.SqlServer
{
	public class EDC_SqlServerDatenbankDialekt : INF_DatenbankDialekt
	{
		public const string mC_strPublicOwner = "dbo";

		private const string mC_strCharactervary = "nvarchar";

		private static readonly Dictionary<string, string> ms_dicMapping = new Dictionary<string, string>
		{
			{
				"String",
				"nvarchar"
			},
			{
				"Int64",
				"bigint"
			},
			{
				"DateTime",
				"datetime"
			},
			{
				"Bitmap",
				"varbinary(MAX)"
			},
			{
				"Byte[]",
				"varbinary(MAX)"
			},
			{
				"Boolean",
				"bit"
			},
			{
				"Int32",
				"int"
			},
			{
				"Int16",
				"smallint"
			},
			{
				"Single",
				"real"
			},
			{
				"Text",
				"ntext"
			}
		};

		public bool PRO_blnHatSequences => true;

		public bool PRO_blnHatTablespaces => false;

		public Dictionary<string, string> FUN_dicDatenTypenMapping()
		{
			return ms_dicMapping;
		}

		public DbParameter FUN_edcErstelleDbParameterMitDatentyp(DbCommand i_fdcCommand, string i_strDatentyp)
		{
			string text = i_strDatentyp.Replace("System.", string.Empty);
			DbParameter dbParameter = i_fdcCommand.CreateParameter();
			switch (text)
			{
			case "String":
				dbParameter.DbType = DbType.String;
				break;
			case "Int64":
				dbParameter.DbType = DbType.Int64;
				break;
			case "Single":
				dbParameter.DbType = DbType.Single;
				break;
			case "DateTime":
				dbParameter.DbType = DbType.DateTime;
				break;
			case "Boolean":
				dbParameter.DbType = DbType.Boolean;
				break;
			case "Bitmap":
				dbParameter.DbType = DbType.Binary;
				break;
			case "Byte[]":
				dbParameter.DbType = DbType.Binary;
				break;
			case "Int32":
				dbParameter.DbType = DbType.Int32;
				break;
			case "Int16":
				dbParameter.DbType = DbType.Int16;
				break;
			case "Byte":
				dbParameter.DbType = DbType.Byte;
				break;
			default:
				throw new ArgumentOutOfRangeException("i_strDatentyp", $"The datatype '{i_strDatentyp}' is invalid or not supported.");
			}
			return dbParameter;
		}

		public string FUN_strHoleDatenbankDatenTypenMapping(string i_strDatentyp)
		{
			string key = i_strDatentyp.Replace("System.", string.Empty);
			ms_dicMapping.TryGetValue(key, out string value);
			return value;
		}

		public string FUN_strHoleConstraintErstellungsString(string i_strTabellenName, string i_strConstraint)
		{
			return $"CONSTRAINT PK_TB_{i_strTabellenName} PRIMARY KEY CLUSTERED ({i_strConstraint}) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
		}

		public string FUN_strHoleNonUniqueHoleIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE NONCLUSTERED INDEX {i_strIndexName} ON {i_strTabellenName} ({i_strSpaltenName}) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
		}

		public string FUN_strHoleUniqueIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE UNIQUE NONCLUSTERED INDEX {i_strIndexName} ON {i_strTabellenName} ({i_strSpaltenName}) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
		}

		public string FUN_strHoleStandardErstellungsString(string i_strErstellungsstring, string i_strIndexErstellungsString, string i_strTabellenName, string i_strTablespace)
		{
			return $"{i_strErstellungsstring};{i_strIndexErstellungsString}";
		}

		public string FUN_strHoleDatenbankErstellungsString(string i_strDatenbankname)
		{
			return string.Format("\r\n                CREATE DATABASE [{0}] CONTAINMENT = NONE;\r\n                ALTER DATABASE {0} MODIFY FILE ( NAME = N'{0}' , SIZE = 100MB , MAXSIZE = UNLIMITED, FILEGROWTH = 2048KB );\r\n                ALTER DATABASE {0} MODIFY FILE ( NAME = N'{0}_log' , SIZE = 10MB , MAXSIZE = 10GB, FILEGROWTH = 10% );\r\n                ALTER DATABASE [{0}] SET COMPATIBILITY_LEVEL = 110;\r\n                ALTER DATABASE [{0}] SET ANSI_NULL_DEFAULT OFF;\r\n                ALTER DATABASE [{0}] SET ANSI_NULLS OFF;\r\n                ALTER DATABASE [{0}] SET ANSI_PADDING OFF;\r\n                ALTER DATABASE [{0}] SET ANSI_WARNINGS OFF;\r\n                ALTER DATABASE [{0}] SET ARITHABORT OFF;\r\n                ALTER DATABASE [{0}] SET AUTO_CLOSE OFF;\r\n                ALTER DATABASE [{0}] SET AUTO_CREATE_STATISTICS ON;\r\n                ALTER DATABASE [{0}] SET AUTO_SHRINK OFF;\r\n                ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS ON;\r\n                ALTER DATABASE [{0}] SET CURSOR_CLOSE_ON_COMMIT OFF;\r\n                ALTER DATABASE [{0}] SET CURSOR_DEFAULT  GLOBAL;\r\n                ALTER DATABASE [{0}] SET CONCAT_NULL_YIELDS_NULL OFF;\r\n                ALTER DATABASE [{0}] SET NUMERIC_ROUNDABORT OFF;\r\n                ALTER DATABASE [{0}] SET QUOTED_IDENTIFIER OFF;\r\n                ALTER DATABASE [{0}] SET RECURSIVE_TRIGGERS OFF;\r\n                ALTER DATABASE [{0}] SET  DISABLE_BROKER;\r\n                ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;\r\n                ALTER DATABASE [{0}] SET DATE_CORRELATION_OPTIMIZATION OFF;\r\n                ALTER DATABASE [{0}] SET TRUSTWORTHY OFF;\r\n                ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION OFF;\r\n                ALTER DATABASE [{0}] SET PARAMETERIZATION SIMPLE;\r\n                ALTER DATABASE [{0}] SET READ_COMMITTED_SNAPSHOT OFF;\r\n                ALTER DATABASE [{0}] SET HONOR_BROKER_PRIORITY OFF;\r\n                ALTER DATABASE [{0}] SET RECOVERY SIMPLE;\r\n                ALTER DATABASE [{0}] SET  MULTI_USER;\r\n                ALTER DATABASE [{0}] SET PAGE_VERIFY CHECKSUM;\r\n                ALTER DATABASE [{0}] SET DB_CHAINING OFF;\r\n                ALTER DATABASE [{0}] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF );\r\n                ALTER DATABASE [{0}] SET TARGET_RECOVERY_TIME = 0 SECONDS;\r\n                ALTER DATABASE [{0}] SET  READ_WRITE\r\n                ", i_strDatenbankname);
		}

		public string FUN_strErstelleSequenceAnweisung(string i_strSequenceName)
		{
			return $"\r\n                             CREATE SEQUENCE {i_strSequenceName}\r\n                             AS [bigint]\r\n                             START WITH 1\r\n                             INCREMENT BY 1\r\n                             MINVALUE 1\r\n                             MAXVALUE 9223372036854775807\r\n                             CACHE";
		}

		public string FUN_strHoleTablespaceErstellungsString(string i_strTablespaceName, string i_strDatenbankDatenVerzeichnis)
		{
			return string.Empty;
		}

		public string FUN_strHoleTabellenErstellungsStatement(DataTable i_fdcSchema, string i_strTabellenName, string i_strTablespace)
		{
			List<string> list = new List<string>();
			foreach (DataRow row in i_fdcSchema.Rows)
			{
				list.Add(FUN_strHoleSpaltenStatement(row));
			}
			string text = FUN_strHoleConstraintFuerSchema(i_fdcSchema, i_strTabellenName);
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
			string i_strErstellungsstring = string.Format("CREATE TABLE {0} ({1})", i_strTabellenName, string.Join(",", list.ToArray()));
			return FUN_strHoleStandardErstellungsString(i_strErstellungsstring, string.Empty, i_strTabellenName, i_strTablespace);
		}

		public string FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge)
		{
			if (!"String".Equals(i_strDatenTyp))
			{
				return $"ADD {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)} NULL";
			}
			if (i_i32DatenLaenge > 0)
			{
				return $"ADD {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)}({i_i32DatenLaenge}) NULL";
			}
			return string.Format("ADD {0} {1} NULL", i_strSpaltenName, FUN_strHoleDatenbankDatenTypenMapping("Text"));
		}

		public string FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge, bool i_blnIsErsteSpalte)
		{
			string text = string.Empty;
			if (i_blnIsErsteSpalte)
			{
				text = "Add";
			}
			if (!"String".Equals(i_strDatenTyp))
			{
				return $"{text} {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)} NULL";
			}
			if (i_i32DatenLaenge > 0)
			{
				return $"{text} {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)}({i_i32DatenLaenge}) NULL";
			}
			return string.Format("{0} {1} {2} NULL", text, i_strSpaltenName, FUN_strHoleDatenbankDatenTypenMapping("Text"));
		}

		public string FUN_strHoleAlterTableDropColumnFuerEineSpalteStatement(string i_strSpaltenName)
		{
			return $"Drop Column {i_strSpaltenName}";
		}

		public string FUN_strHoleAlterTableDropColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, bool i_blnIsErsteSpalte)
		{
			string arg = string.Empty;
			if (i_blnIsErsteSpalte)
			{
				arg = "Drop Column";
			}
			return $"{arg} {i_strSpaltenName}";
		}

		public string FUN_strHoleAlterTableChangeCharacterColumnLength(string i_strSpaltenName, int i_i32NeueLaenge)
		{
			return string.Format("ALTER COLUMN {0} {1}({2})", i_strSpaltenName, "nvarchar", i_i32NeueLaenge);
		}

		public string FUN_strHoleTabellenListeStatement(string i_strDatenbankName)
		{
			return $"SELECT\u00a0TABLE_NAME\u00a0FROM\u00a0INFORMATION_SCHEMA.TABLES\u00a0WHERE\u00a0TABLE_CATALOG='{i_strDatenbankName}'";
		}

		public string FUN_strHoleDatenbankExistiertStatement(string i_strDatenbankName)
		{
			return $"Select count(*) from sys.databases where name = '{i_strDatenbankName}'";
		}

		public string FUN_strHoleTabelleExistiertStatement(string i_strTabellenName)
		{
			return string.Format("Select count(*) from INFORMATION_SCHEMA.TABLES where table_schema = '{0}' and table_name = '{1}'", "dbo", i_strTabellenName);
		}

		public string FUN_strHoleTablespaceExistenzAbfrage(string i_strTablespaceName)
		{
			throw new NotImplementedException();
		}

		public string FUN_strHoleTablespaceFuerTabelleStatement(string i_strTabellenName)
		{
			throw new NotImplementedException();
		}

		public string FUN_strHoleSequenceExistenzAbfrage(string i_strSequenceName)
		{
			return $"SELECT Count(*) from sys.objects where name='{i_strSequenceName}' AND type_desc='SEQUENCE_OBJECT'";
		}

		public string FUN_strHoleSequenceAbfrage(string i_strSequenceName)
		{
			return $"SELECT NEXT VALUE FOR {i_strSequenceName}";
		}

		public string FUN_strHoleSequenceWertSetzenStatement(string i_strSequenceName, uint i_i32Wert)
		{
			return $"ALTER SEQUENCE {i_strSequenceName} RESTART WITH {i_i32Wert}";
		}

		public string FUN_strHoleTabellenUmbenennenStatement(string i_strAlterName, string i_strNeuerName)
		{
			return $"sp_rename '{i_strAlterName}', '{i_strNeuerName}'";
		}

		private string FUN_strHoleSpaltenStatement(DataRow i_fdcRow)
		{
			string text = i_fdcRow["ColumnName"].ToString();
			string text2 = i_fdcRow["DataType"].ToString();
			long num = Convert.ToInt64(i_fdcRow["ColumnSize"]);
			string empty = string.Empty;
			empty = ((!text2.ToLower().Contains("String".ToLower()) || (num > 0 && num <= 1000)) ? FUN_strHoleDatenbankDatenTypenMapping(text2) : FUN_strHoleDatenbankDatenTypenMapping("Text"));
			if (string.IsNullOrEmpty(empty))
			{
				throw new ArgumentOutOfRangeException("strMapping", $"The datatype '{text2}' is invalid or not supported.");
			}
			string text3 = Convert.ToBoolean(i_fdcRow["AllowDBNull"]) ? "NULL" : "NOT NULL";
			string text4 = string.Empty;
			if ("nvarchar".Equals(empty))
			{
				text4 = string.Format("({0})", i_fdcRow["ColumnSize"]);
			}
			return $"{text} {empty}{text4} {text3}";
		}

		private string FUN_strHoleConstraintFuerSchema(DataTable i_fdcSchema, string i_strTabellenName)
		{
			List<string> list = new List<string>();
			foreach (DataRow row in i_fdcSchema.Rows)
			{
				bool flag = Convert.ToBoolean(row["IsKey"]);
				string item = row["ColumnName"].ToString();
				if (flag)
				{
					list.Add(item);
				}
			}
			if (list.Any())
			{
				return FUN_strHoleConstraintErstellungsString(i_strTabellenName, string.Join(",", list.ToArray()));
			}
			return string.Empty;
		}
	}
}
