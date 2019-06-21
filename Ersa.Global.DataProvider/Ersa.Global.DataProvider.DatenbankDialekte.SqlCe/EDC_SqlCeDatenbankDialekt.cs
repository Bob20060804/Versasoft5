using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;

namespace Ersa.Global.DataProvider.DatenbankDialekte.SqlCe
{
	public class EDC_SqlCeDatenbankDialekt : INF_DatenbankDialekt
	{
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
				"image"
			},
			{
				"Byte[]",
				"image"
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

		public bool PRO_blnHatSequences => false;

		public bool PRO_blnHatTablespaces => false;

		public Dictionary<string, string> FUN_dicDatenTypenMapping()
		{
			return ms_dicMapping;
		}

		public DbParameter FUN_edcErstelleDbParameterMitDatentyp(DbCommand i_fdcCommand, string i_strDatentyp)
		{
			string text = i_strDatentyp.Replace("System.", string.Empty);
			SqlCeParameter sqlCeParameter = (SqlCeParameter)i_fdcCommand.CreateParameter();
			switch (text)
			{
			case "String":
				sqlCeParameter.SqlDbType = SqlDbType.NText;
				break;
			case "Int64":
				sqlCeParameter.DbType = DbType.Int64;
				break;
			case "Single":
				sqlCeParameter.DbType = DbType.Single;
				break;
			case "DateTime":
				sqlCeParameter.DbType = DbType.DateTime;
				break;
			case "Boolean":
				sqlCeParameter.DbType = DbType.Boolean;
				break;
			case "Bitmap":
				sqlCeParameter.SqlDbType = SqlDbType.Image;
				break;
			case "Byte[]":
				sqlCeParameter.SqlDbType = SqlDbType.Image;
				break;
			case "Int32":
				sqlCeParameter.DbType = DbType.Int32;
				break;
			case "Int16":
				sqlCeParameter.DbType = DbType.Int16;
				break;
			case "Byte":
				sqlCeParameter.DbType = DbType.Byte;
				break;
			default:
				throw new ArgumentOutOfRangeException("i_strDatentyp", $"The datatype '{i_strDatentyp}' is invalid or not supported.");
			}
			return sqlCeParameter;
		}

		public DbType FUN_edcHoleDenProviderDatenTyp(string i_strDatentyp)
		{
			switch (i_strDatentyp)
			{
			case "String":
				return DbType.String;
			case "Int64":
				return DbType.Int64;
			case "Single":
				return DbType.Single;
			case "DateTime":
				return DbType.DateTime;
			case "Boolean":
				return DbType.Boolean;
			case "Bitmap":
				return DbType.Decimal;
			case "Byte[]":
				return DbType.Binary;
			case "Int32":
				return DbType.Int32;
			case "Int16":
				return DbType.Int16;
			case "Byte":
				return DbType.Byte;
			default:
				throw new ArgumentOutOfRangeException("i_strDatentyp", $"The datatype '{i_strDatentyp}' is invalid or not supported.");
			}
		}

		public string FUN_strHoleDatenbankDatenTypenMapping(string i_strDatentyp)
		{
			string key = i_strDatentyp.Replace("System.", string.Empty);
			ms_dicMapping.TryGetValue(key, out string value);
			return value;
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

		public string FUN_strHoleConstraintErstellungsString(string i_strTabellenName, string i_strConstraint)
		{
			return $"CONSTRAINT PK_TB_{i_strTabellenName} PRIMARY KEY ({i_strConstraint})";
		}

		public string FUN_strHoleNonUniqueHoleIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE INDEX {i_strIndexName} ON {i_strTabellenName} ({i_strSpaltenName})";
		}

		public string FUN_strHoleUniqueIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace)
		{
			return $"CREATE UNIQUE INDEX {i_strIndexName} ON {i_strTabellenName} ({i_strSpaltenName})";
		}

		public string FUN_strHoleStandardErstellungsString(string i_strErstellungsstring, string i_strIndexErstellungsString, string i_strTabellenName, string i_strTablespace)
		{
			return $"{i_strErstellungsstring};{i_strIndexErstellungsString}";
		}

		public string FUN_strHoleDatenbankErstellungsString(string i_strDatenbankname)
		{
			return string.Empty;
		}

		public string FUN_strErstelleSequenceAnweisung(string i_strSequenceName)
		{
			return string.Empty;
		}

		public string FUN_strHoleTablespaceErstellungsString(string i_strTablespaceName, string i_strDatenbankDatenVerzeichnis)
		{
			return string.Empty;
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
			throw new NotImplementedException();
		}

		public string FUN_strHoleAlterTableChangeCharacterColumnLength(string i_strSpaltenName, int i_i32NeueLaenge)
		{
			return string.Format("ALTER COLUMN {0} {1}({2})", i_strSpaltenName, "nvarchar", i_i32NeueLaenge);
		}

		public string FUN_strHoleTabellenListeStatement(string i_strDatenbankName)
		{
			return "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
		}

		public string FUN_strHoleDatenbankExistiertStatement(string i_strDatenbankName)
		{
			throw new NotImplementedException();
		}

		public string FUN_strHoleTabelleExistiertStatement(string i_strTabellenName)
		{
			return $"Select count(*) from INFORMATION_SCHEMA.TABLES where table_name = '{i_strTabellenName}'";
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
			throw new NotImplementedException();
		}

		public string FUN_strHoleSequenceAbfrage(string i_strSequenceName)
		{
			return $"Select Value From Sequences Where Name='{i_strSequenceName}'";
		}

		public string FUN_strHoleSequenceWertSetzenStatement(string i_strSequenceName, uint i_i32Wert)
		{
			return $"UPDATE Sequences SET Value = {i_i32Wert} Where Name='{i_strSequenceName}'";
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
