using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.Adapter.Interface
{
	public interface INF_ReadonlyDatenbankAdapter
	{
		object FUN_fdcScalareAbfrage(string i_strSql);

		object FUN_fdcScalareAbfrage(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null);

		Task<object> FUN_fdcScalareAbfrageAsync(string i_strSql);

		Task<object> FUN_fdcScalareAbfrageAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null);

		IEnumerable<object> FUN_enuErstelleScalareObjektliste(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0);

		Task<IEnumerable<object>> FUN_fdcErstelleScalareObjektlisteAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0);

		IEnumerable<T> FUN_lstErstelleObjektliste<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0);

		Task<IEnumerable<T>> FUN_fdcErstelleObjektlisteAsync<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0);

		Task<IEnumerable<T>> FUN_fdcErstelleObjektlisteAsync<T>(string i_strSql, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null, int i_i16AbfrageLimit = 0);

		T FUN_edcLeseObjekt<T>(T i_edcSelectObjekt, Dictionary<string, object> i_dicParameter = null, IDbTransaction i_fdcDbTransaktion = null);

		Task<T> FUN_edcLeseObjektAsync<T>(T i_edcSelectObjekt, Dictionary<string, object> i_dicParameter = null, IDbTransaction i_fdcDbTransaktion = null);

		DataTable FUN_fdcLeseTabelleVollstaendig(string i_strTabellenName);

		Task<DataTable> FUN_fdcLeseTabelleVollstaendigAsync(string i_strTabellenName);

		DataTable FUN_fdcLeseInDataTable(string i_strSql, string i_strTabellenName = "");

		Task<DataTable> FUN_fdcLeseInDataTableAsync(string i_strSql, string i_strTabellenName = "");

		DataTable FUN_fdcLeseInDataTable<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);

		Task<DataTable> FUN_fdcLeseInDataTableAsync<T>(T i_edcSelectObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);
	}
}
