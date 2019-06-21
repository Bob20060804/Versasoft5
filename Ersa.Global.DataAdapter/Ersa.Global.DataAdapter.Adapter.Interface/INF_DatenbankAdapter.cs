using Ersa.Global.DataAdapter.DatabaseModel.Model;
using Ersa.Global.DataProvider.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.Adapter.Interface
{
	public interface INF_DatenbankAdapter : INF_ReadonlyDatenbankAdapter, INF_OrganisationsAdapter
	{
		INF_DatenbankProvider PRO_edcDatenbankProvider
		{
			get;
		}

		INF_DatenbankModel PRO_edcDatenbankModell
		{
			get;
		}

		Task<IDbTransaction> FUN_fdcStarteTransaktionAsync();

		IDbTransaction FUN_fdcStarteTransaktion();

		void SUB_CommitTransaktion(IDbTransaction i_fdcDbTransaktion);

		void SUB_RollbackTransaktion(IDbTransaction i_fdcDbTransaktion);

		void SUB_ExecuteStatement(string i_strSql);

		void SUB_ExecuteStatement(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter);

		Task FUN_fdcExecuteStatementAsync(string i_strSql);

		Task FUN_fdcExecuteStatementAsync(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null);

		void SUB_SpeichereObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null);

		Task FUN_fdcSpeichereObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null);

		void SUB_SpeichereObjektListe<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null);

		Task FUN_fdcSpeichereObjektListeAsync<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null);

		void SUB_UpdateObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);

		Task FUN_fdcUpdateObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);

		void SUB_LoescheObjekt<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);

		Task FUN_fdcLoescheObjektAsync<T>(T i_edcObjekt, IDbTransaction i_fdcDbTransaktion = null, Dictionary<string, object> i_dicParameter = null);

		void SUB_SchreibeDatenInTabelle(DataTable i_fdcDataTable, string i_strTabellenName, IDbTransaction i_fdcDbTransaktion = null);

		Task FUN_fdcSchreibeDatenInTabelleAsync(DataTable i_fdcDataTable, string i_strTabellenName, IDbTransaction i_fdcDbTransaktion = null);

		void SUB_SchreibeObjekteInTabelle<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null);

		Task FUN_fdcSchreibeObjekteInTabelleAsync<T>(IEnumerable<T> i_lstObjektListe, IDbTransaction i_fdcDbTransaktion = null);

		uint FUN_u32HoleNaechstenSequenceWert(string i_strSequenceName);

		Task<uint> FUN_fdcHoleNaechstenSequenceWertAsync(string i_strSequenceName);

		void SUB_SetzeSequenceWert(string i_strSequenceName, uint i_i32Wert);

		Task FUN_fdcSetzeSequenceWertAsync(string i_strSequenceName, uint i_i32Wert);

		Task<long> FUN_fdcHoleMaxNummerischenPrimaryKeyWertAsync<T>(T i_edcObjekt);

		Task<long> FUN_fdcHoleNaechstenNummerischenPrimaryKeyWertAsync<T>(T i_edcObjekt);
	}
}
