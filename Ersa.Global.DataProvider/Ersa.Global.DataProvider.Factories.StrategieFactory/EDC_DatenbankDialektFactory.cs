using Ersa.Global.DataProvider.DatenbankDialekte.Postgres;
using Ersa.Global.DataProvider.DatenbankDialekte.SqlCe;
using Ersa.Global.DataProvider.DatenbankDialekte.SqlServer;
using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Interfaces;
using Npgsql;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace Ersa.Global.DataProvider.Factories.StrategieFactory
{
	public static class EDC_DatenbankDialektFactory
	{
		public static INF_DatenbankDialekt FUN_edcHoleDatenbankDialekt(ENUM_DatenbankTyp i_enmDatenbankTyp)
		{
			switch (i_enmDatenbankTyp)
			{
			case ENUM_DatenbankTyp.SqlServer:
				return new EDC_SqlServerDatenbankDialekt();
			case ENUM_DatenbankTyp.SqlExpress:
				return new EDC_SqlServerDatenbankDialekt();
			case ENUM_DatenbankTyp.SqlCe:
				return new EDC_SqlCeDatenbankDialekt();
			case ENUM_DatenbankTyp.Postgres:
				return new EDC_PostgresDatenbankDialekt();
			default:
				throw new ArgumentOutOfRangeException("i_enmDatenbankTyp", $"The provider '{i_enmDatenbankTyp}' is invalid or not supported.");
			}
		}

		public static INF_DatenbankDialekt FUN_edcHoleDatenbankDialekt(DbCommand i_fdcCommand)
		{
			if (i_fdcCommand is SqlCommand)
			{
				return new EDC_SqlServerDatenbankDialekt();
			}
			if (i_fdcCommand is SqlCeCommand)
			{
				return new EDC_SqlCeDatenbankDialekt();
			}
			if (i_fdcCommand is NpgsqlCommand)
			{
				return new EDC_PostgresDatenbankDialekt();
			}
			throw new ArgumentOutOfRangeException("i_fdcCommand", $"The provider for Command '{i_fdcCommand}' is invalid or not supported.");
		}
	}
}
