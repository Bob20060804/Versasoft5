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
	public class EDC_LoetprogrammSatzDatenDataAccess : EDC_DataAccess, INF_LoetprogrammSatzDatenDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammSatzDatenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_LoetprogrammSatzDatenData>> FUN_enuAlleSatzDatenZuVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammSatzDatenData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammSatzDatenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleSatzdatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammSatzDatenData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammSatzDatenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcSatzDatenListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammSatzDatenData> i_enuSatzDatenListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_LoetprogrammSatzDatenData> lstDaten = i_enuSatzDatenListe.ToList();
			foreach (EDC_LoetprogrammSatzDatenData item in lstDaten)
			{
				item.PRO_i64VersionsId = i_i64VersionsId;
			}
			await FUN_fdcSatzDatenVersionLoeschenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstDaten, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcSatzDatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			string i_strSql = EDC_LoetprogrammSatzDatenData.FUN_strHistoryEintraegeLoeschenStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task FUN_fdcSatzDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramSetData"];
			if (dataTable != null)
			{
				List<EDC_LoetprogrammSatzDatenData> i_enuSatzDatenListe = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammSatzDatenData>().ToList();
				await FUN_fdcSatzDatenListeHinzufuegenAsync(i_enuSatzDatenListe, i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
