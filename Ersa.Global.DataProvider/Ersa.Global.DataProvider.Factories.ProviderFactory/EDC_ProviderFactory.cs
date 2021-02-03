using Ersa.Global.DataProvider.DatenbankProvider.Postgres;
using Ersa.Global.DataProvider.DatenbankProvider.SqlCe;
using Ersa.Global.DataProvider.DatenbankProvider.SqlServer;
using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Helfer;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Specialized;

namespace Ersa.Global.DataProvider.Factories.ProviderFactory
{
	public static class EDC_ProviderFactory
	{
		/// <summary>
		/// 创建数据提供者
		/// Create database provider
		/// </summary>
		/// <param name="i_fdcDatenbankEinstellungen"></param>
		/// <returns></returns>
		public static INF_DatenbankProvider FUN_fdcErstelleDatenbankProvider(NameValueCollection i_fdcDatenbankEinstellungen)
		{
			// 获得数据库相关列表
			NameValueCollection nameValueCollection = FUN_fdcErstelleDbRelevanteListe(i_fdcDatenbankEinstellungen);
			// 获得数据库类型
			ENUM_DatenbankTyp eNUM_DatenbankTyp = EDC_ProviderConverterHelfer.FUN_enmDatenbankTypErmitteln(nameValueCollection["ProviderName"]);
			// 根据数据库类型创建提供者
			switch (eNUM_DatenbankTyp)
			{
			case ENUM_DatenbankTyp.SqlServer:
				return new EDC_SqlServerProvider(nameValueCollection);
			case ENUM_DatenbankTyp.SqlExpress:
				return new EDC_SqlServerProvider(nameValueCollection);
			case ENUM_DatenbankTyp.SqlCe:
				return new EDC_SqlCeProvider(nameValueCollection);
			case ENUM_DatenbankTyp.Postgres:
				return new EDC_PostgresProvider(nameValueCollection);
			default:
				throw new ArgumentOutOfRangeException("enmDatenbankTyp", $"The provider '{eNUM_DatenbankTyp}' is invalid or not supported.");
			}
		}

		/// <summary>
		/// 创造数据库相关列表
		/// </summary>
		/// <param name="i_fdcDatenbankEinstellungen"></param>
		/// <returns></returns>
		private static NameValueCollection FUN_fdcErstelleDbRelevanteListe(NameValueCollection i_fdcDatenbankEinstellungen)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (i_fdcDatenbankEinstellungen["ProviderName"] != null)
			{
				nameValueCollection.Add("ProviderName", i_fdcDatenbankEinstellungen["ProviderName"]);
			}
			if (i_fdcDatenbankEinstellungen["DataDirectory"] != null)
			{
				nameValueCollection.Add("DataDirectory", i_fdcDatenbankEinstellungen["DataDirectory"]);
			}
			if (i_fdcDatenbankEinstellungen["BinaryDirectory"] != null)
			{
				nameValueCollection.Add("BinaryDirectory", i_fdcDatenbankEinstellungen["BinaryDirectory"]);
			}
			if (i_fdcDatenbankEinstellungen["Server"] != null)
			{
				nameValueCollection.Add("Server", i_fdcDatenbankEinstellungen["Server"]);
			}
			if (i_fdcDatenbankEinstellungen["Port"] != null)
			{
				nameValueCollection.Add("Port", i_fdcDatenbankEinstellungen["Port"]);
			}
			if (i_fdcDatenbankEinstellungen["UserId"] != null)
			{
				nameValueCollection.Add("UserId", i_fdcDatenbankEinstellungen["UserId"]);
			}
			if (i_fdcDatenbankEinstellungen["Password"] != null)
			{
				nameValueCollection.Add("Password", i_fdcDatenbankEinstellungen["Password"]);
			}
			if (i_fdcDatenbankEinstellungen["Pooling"] != null)
			{
				nameValueCollection.Add("Pooling", i_fdcDatenbankEinstellungen["Pooling"]);
			}
			if (i_fdcDatenbankEinstellungen["MinPoolSize"] != null)
			{
				nameValueCollection.Add("MinPoolSize", i_fdcDatenbankEinstellungen["MinPoolSize"]);
			}
			if (i_fdcDatenbankEinstellungen["MaxPoolSize"] != null)
			{
				nameValueCollection.Add("MaxPoolSize", i_fdcDatenbankEinstellungen["MaxPoolSize"]);
			}
			if (i_fdcDatenbankEinstellungen["ConnectionLifeTime"] != null)
			{
				nameValueCollection.Add("ConnectionLifeTime", i_fdcDatenbankEinstellungen["ConnectionLifeTime"]);
			}
			if (i_fdcDatenbankEinstellungen["CommandTimeout"] != null)
			{
				nameValueCollection.Add("CommandTimeout", i_fdcDatenbankEinstellungen["CommandTimeout"]);
			}
			if (i_fdcDatenbankEinstellungen["Database"] != null)
			{
				nameValueCollection.Add("Database", i_fdcDatenbankEinstellungen["Database"]);
			}
			if (i_fdcDatenbankEinstellungen["AsynchronousProcessing"] != null)
			{
				nameValueCollection.Add("AsynchronousProcessing", i_fdcDatenbankEinstellungen["AsynchronousProcessing"]);
			}
			if (i_fdcDatenbankEinstellungen["DataSource"] != null)
			{
				nameValueCollection.Add("DataSource", i_fdcDatenbankEinstellungen["DataSource"]);
			}
			if (i_fdcDatenbankEinstellungen["MaxDatabaseSize"] != null)
			{
				nameValueCollection.Add("MaxDatabaseSize", i_fdcDatenbankEinstellungen["MaxDatabaseSize"]);
			}
			return nameValueCollection;
		}
	}
}
