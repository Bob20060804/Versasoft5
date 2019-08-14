using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.DatenbankVerwaltung
{
	public interface INF_DatenbankVerwaltungDataAccess : INF_DataAccess
	{
		Task FUN_fdcAktualisiereWerteInTabelleAsync(string i_strTabellenName, string i_strSpaltenName, IDictionary<string, string> i_dicMapping, IDbTransaction i_fdcTransaktion = null);
	}
}
