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
	public class EDC_LoetprogrammNutzenDatenDataAccess : EDC_DataAccess, INF_LoetprogrammNutzenDatenDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammNutzenDatenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_LoetprogrammNutzenData>> FUN_fdcLadeNutzenDatenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammNutzenData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammNutzenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleNutzendatenInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammNutzenData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammNutzenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcSpeichereNutzenDatenAsync(long i_i64VersionsId, IEnumerable<EDC_LoetprogrammNutzenData> i_enuNutzenDaten, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_LoetprogrammNutzenData> lstNutzenDaten = i_enuNutzenDaten.ToList();
			foreach (EDC_LoetprogrammNutzenData item in lstNutzenDaten)
			{
				item.PRO_i64VersionsId = i_i64VersionsId;
			}
			await FUN_fdcNutzenDatenVersionLoeschenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstNutzenDaten, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcNutzenDatenVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			string i_strSql = EDC_LoetprogrammNutzenData.FUN_strVersionsLoeschenStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task FUN_fdcNutzenDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramPanelParameter"];
			if (dataTable != null)
			{
				List<EDC_LoetprogrammNutzenData> i_enuNutzenDaten = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammNutzenData>().ToList();
				await FUN_fdcSpeichereNutzenDatenAsync(i_i64VersionsId, i_enuNutzenDaten, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
