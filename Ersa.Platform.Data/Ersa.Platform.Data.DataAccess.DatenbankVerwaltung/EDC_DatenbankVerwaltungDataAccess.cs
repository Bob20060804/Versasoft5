using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.DatenbankVerwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.DatenbankVerwaltung
{
	public class EDC_DatenbankVerwaltungDataAccess : EDC_DataAccess, INF_DatenbankVerwaltungDataAccess, INF_DataAccess
	{
		public EDC_DatenbankVerwaltungDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task FUN_fdcAktualisiereWerteInTabelleAsync(string i_strTabellenName, string i_strSpaltenName, IDictionary<string, string> i_dicMapping, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			IEnumerable<string> i_enuStatements = FUN_enuErstelleUpdateStatements(i_strTabellenName, i_strSpaltenName, i_dicMapping);
			try
			{
				await FUN_fdcFuehreStatementListeAusAsync(i_enuStatements, fdcTransaktion);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		private IEnumerable<string> FUN_enuErstelleUpdateStatements(string i_strTabellenName, string i_strSpaltenName, IDictionary<string, string> i_dicMapping)
		{
			return from i_fdcKvp in i_dicMapping
			select $"UPDATE {i_strTabellenName} SET {i_strSpaltenName} = '{i_fdcKvp.Value}' WHERE {i_strSpaltenName} = '{i_fdcKvp.Key}'";
		}
	}
}
