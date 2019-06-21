using Ersa.Global.DataProvider.Datenbanktypen;
using System;
using System.Collections.Generic;

namespace Ersa.Global.DataProvider.Helfer
{
	public static class EDC_ProviderConverterHelfer
	{
		public const string mC_strPostgresProviderName = "Npgsql";

		public const string mC_strSqlServerProviderName = "System.Data.SqlClient";

		public const string mC_strSqlCeProviderName = "System.Data.SqlServerCe.4.0";

		private static readonly IDictionary<string, ENUM_DatenbankTyp> ms_dicDatenbankTypen = new Dictionary<string, ENUM_DatenbankTyp>
		{
			{
				"Npgsql",
				ENUM_DatenbankTyp.Postgres
			},
			{
				"System.Data.SqlClient",
				ENUM_DatenbankTyp.SqlServer
			},
			{
				"System.Data.SqlServerCe.4.0",
				ENUM_DatenbankTyp.SqlCe
			}
		};

		public static ENUM_DatenbankTyp FUN_enmDatenbankTypErmitteln(string i_strProviderName)
		{
			if (string.IsNullOrWhiteSpace(i_strProviderName))
			{
				throw new ArgumentNullException("i_strProviderName", "The provider name cannot be empty");
			}
			if (!ms_dicDatenbankTypen.ContainsKey(i_strProviderName))
			{
				throw new ArgumentOutOfRangeException("i_strProviderName", $"The provider '{i_strProviderName}' is invalid or not supported.");
			}
			return ms_dicDatenbankTypen[i_strProviderName];
		}

		public static string FUN_strDatenbankTypZuProviderName(ENUM_DatenbankTyp i_enuDatenbankTyp)
		{
			switch (i_enuDatenbankTyp)
			{
			case ENUM_DatenbankTyp.Postgres:
				return "Npgsql";
			case ENUM_DatenbankTyp.SqlServer:
				return "System.Data.SqlClient";
			case ENUM_DatenbankTyp.SqlExpress:
				return "System.Data.SqlClient";
			case ENUM_DatenbankTyp.SqlCe:
				return "System.Data.SqlServerCe.4.0";
			default:
				throw new ArgumentOutOfRangeException("i_enuDatenbankTyp", $"The database-type '{i_enuDatenbankTyp}' is invalid or not supported.");
			}
		}
	}
}
