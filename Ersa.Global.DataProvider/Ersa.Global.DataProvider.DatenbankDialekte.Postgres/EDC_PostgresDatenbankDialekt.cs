using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace Ersa.Global.DataProvider.DatenbankDialekte.Postgres
{
	public class EDC_PostgresDatenbankDialekt : INF_DatenbankDialekt
	{
		public const string mC_strPostgresOwner = "postgres";

		public const string mC_strPublicOwner = "public";

		private const string mC_strCharactervary = "character varying";

		private static readonly Dictionary<string, string> ms_dicMapping = new Dictionary<string, string>
		{
			{
				"String",
				"character varying"
			},
			{
				"Int64",
				"bigint"
			},
			{
				"DateTime",
				"timestamp without time zone"
			},
			{
				"Bitmap",
				"bytea"
			},
			{
				"Byte[]",
				"bytea"
			},
			{
				"Boolean",
				"boolean"
			},
			{
				"Int32",
				"integer"
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
				"text"
			}
		};

		public bool PRO_blnHatSequences => true;

		public bool PRO_blnHatTablespaces => true;

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
			return $"CONSTRAINT PK_TB_{i_strTabellenName} PRIMARY KEY ({i_strConstraint})";
		}

		public string FUN_strHoleNonUniqueHoleIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE INDEX {i_strIndexName} ON {i_strTabellenName} USING btree ({i_strSpaltenName}) {FUN_strHoleTablespaceFragment(i_strTablespace)}";
		}

		public string FUN_strHoleUniqueIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE UNIQUE INDEX {i_strIndexName} ON {i_strTabellenName} USING btree ({i_strSpaltenName}) {FUN_strHoleTablespaceFragment(i_strTablespace)}";
		}

		public string FUN_strHoleStandardErstellungsString(string i_strErstellungsstring, string i_strIndexErstellungsString, string i_strTabellenName, string i_strTablespace)
		{
			return string.Format("{0}\r\n                        WITH (OIDS=FALSE){1};\r\n                        ALTER TABLE {2} OWNER TO {3};{4}", i_strErstellungsstring, FUN_strHoleTablespaceFragment(i_strTablespace), i_strTabellenName, "postgres", i_strIndexErstellungsString);
		}

		public string FUN_strHoleDatenbankErstellungsString(string i_strDatenbankname)
		{
			return string.Format("  CREATE DATABASE {0}\r\n                        WITH OWNER = {1}\r\n                        ENCODING = 'UTF8'\r\n                        TABLESPACE = pg_default\r\n                        LC_COLLATE = 'German_Germany.1252'\r\n                        LC_CTYPE = 'German_Germany.1252'\r\n                        CONNECTION LIMIT = -1", i_strDatenbankname.ToLower(), "postgres");
		}

		public string FUN_strErstelleSequenceAnweisung(string i_strSequenceName)
		{
			return string.Format("\r\n                    CREATE SEQUENCE {0}\r\n                    INCREMENT 1\r\n                    MINVALUE 1\r\n                    MAXVALUE 9223372036854775807\r\n                    START 1\r\n                    CACHE 1;\r\n                    ALTER TABLE {0} OWNER TO {1};\r\n                    GRANT ALL ON SEQUENCE {0} TO {1};\r\n                    GRANT ALL ON SEQUENCE {0} TO {2}", i_strSequenceName, "postgres", "public");
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

		public string FUN_strHoleTablespaceErstellungsString(string i_strTablespaceName, string i_strDatenbankDatenVerzeichnis)
		{
			if (string.IsNullOrEmpty(i_strTablespaceName) || string.IsNullOrEmpty(i_strDatenbankDatenVerzeichnis))
			{
				return string.Empty;
			}
			return string.Format("CREATE TABLESPACE {0} OWNER {1} LOCATION '{2}'", i_strTablespaceName, "postgres", Path.Combine(i_strDatenbankDatenVerzeichnis, i_strTablespaceName));
		}

		public string FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge)
		{
			if (!"String".Equals(i_strDatenTyp))
			{
				return $"ADD COLUMN {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)} NULL";
			}
			if (i_i32DatenLaenge > 0)
			{
				return $"ADD COLUMN {i_strSpaltenName} {FUN_strHoleDatenbankDatenTypenMapping(i_strDatenTyp)}({i_i32DatenLaenge}) NULL";
			}
			return string.Format("ADD COLUMN {0} {1} NULL", i_strSpaltenName, FUN_strHoleDatenbankDatenTypenMapping("Text"));
		}

		public string FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge, bool i_blnIsErsteSpalte)
		{
			return FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement(i_strSpaltenName, i_strDatenTyp, i_i32DatenLaenge);
		}

		public string FUN_strHoleAlterTableDropColumnFuerEineSpalteStatement(string i_strSpaltenName)
		{
			return $"Drop Column {i_strSpaltenName}";
		}

		public string FUN_strHoleAlterTableDropColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, bool i_blnIsErsteSpalte)
		{
			return FUN_strHoleAlterTableDropColumnFuerEineSpalteStatement(i_strSpaltenName);
		}

		public string FUN_strHoleAlterTableChangeCharacterColumnLength(string i_strSpaltenName, int i_i32NeueLaenge)
		{
			return string.Format("alter column {0} type {1}({2})", i_strSpaltenName, "character varying", i_i32NeueLaenge);
		}

		public string FUN_strHoleTabellenListeStatement(string i_strDatenbankName)
		{
			return string.Format("SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = '{0}'", "public");
		}

		public string FUN_strHoleDatenbankExistiertStatement(string i_strDatenbankName)
		{
			return $"Select count(*) from pg_catalog.pg_database where datname = '{i_strDatenbankName.ToLower()}'";
		}

		public string FUN_strHoleTabelleExistiertStatement(string i_strTabellenName)
		{
			return string.Format("Select count(*) from information_schema.tables where table_schema = '{0}' and table_name = '{1}'", "public", i_strTabellenName.ToLower());
		}

		public string FUN_strHoleTablespaceFuerTabelleStatement(string i_strTabellenName)
		{
			return string.Format("SELECT tablespace FROM pg_tables WHERE tablename = '{0}' AND schemaname = '{1}'", i_strTabellenName.ToLower(), "public");
		}

		public string FUN_strHoleTablespaceExistenzAbfrage(string i_strTablespaceName)
		{
			return $"select count(*) from pg_tablespace where spcname='{i_strTablespaceName}'";
		}

		public string FUN_strHoleSequenceExistenzAbfrage(string i_strSequenceName)
		{
			return $"SELECT Count(*) from pg_class where relname='{i_strSequenceName}'";
		}

		public string FUN_strHoleSequenceAbfrage(string i_strSequenceName)
		{
			return $"SELECT nextval ('{i_strSequenceName}')";
		}

		public string FUN_strHoleSequenceWertSetzenStatement(string i_strSequenceName, uint i_i32Wert)
		{
			return $"ALTER SEQUENCE {i_strSequenceName} RESTART WITH {i_i32Wert}";
		}

		public string FUN_strHoleTabellenUmbenennenStatement(string i_strAlterName, string i_strNeuerName)
		{
			return $"ALTER TABLE {i_strAlterName} Rename To {i_strNeuerName}";
		}

		private string FUN_strHoleTablespaceFragment(string i_strTablespace)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(i_strTablespace))
			{
				result = $"TABLESPACE {i_strTablespace}";
			}
			return result;
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
				throw new ArgumentOutOfRangeException("strDataTypeName", $"The datatype '{text2}' is invalid or not supported.");
			}
			string text3 = Convert.ToBoolean(i_fdcRow["AllowDBNull"]) ? "NULL" : "NOT NULL";
			string text4 = string.Empty;
			if ("character varying".Equals(empty))
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
