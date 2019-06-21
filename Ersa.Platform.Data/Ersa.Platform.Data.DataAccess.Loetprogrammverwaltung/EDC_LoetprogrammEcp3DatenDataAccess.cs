using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammEcp3DatenDataAccess : EDC_DataAccess, INF_LoetprogrammEcp3DatenDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammEcp3DatenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_LoetprogrammEcp3DatenData>> FUN_enuAlleEcp3DatenZuVersionLadenAsync(long i_i64HistoryId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammEcp3DatenData.FUN_strHistoryIdWhereStatementErstellen(i_i64HistoryId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammEcp3DatenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleEcp3DatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammEcp3DatenData.FUN_strHistoryIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammEcp3DatenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcEcp3DatenListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammEcp3DatenData> i_enuEcp3Daten, long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_LoetprogrammEcp3DatenData> lstDaten = i_enuEcp3Daten.ToList();
			foreach (EDC_LoetprogrammEcp3DatenData item in lstDaten)
			{
				item.PRO_i64HistoryId = i_i64VersionsId;
			}
			await FUN_fdcEcp3DatenVersionLoeschenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstDaten, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcEcp3DatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			string i_strSql = EDC_LoetprogrammEcp3DatenData.FUN_strHistoryEintraegeLoeschenStatementErstellen(new List<long>
			{
				i_i64VersionsId
			});
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task FUN_fdcEcp3DatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramEcp3Data"];
			if (dataTable != null)
			{
				List<EDC_LoetprogrammEcp3DatenData> i_enuEcp3Daten = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammEcp3DatenData>().ToList();
				await FUN_fdcEcp3DatenListeHinzufuegenAsync(i_enuEcp3Daten, i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
