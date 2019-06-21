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
	public class EDC_LoetprogrammParameterDataAccess : EDC_DataAccess, INF_LoetprogrammParameterDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammParameterDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_LoetprogrammParameterData>> FUN_enuAlleParameterZuVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammParameterData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammParameterData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleParameterInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammParameterData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammParameterData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcParameterListeSpeichernAsync(IEnumerable<EDC_LoetprogrammParameterData> i_enuParameterListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_LoetprogrammParameterData> lstDaten = i_enuParameterListe.ToList();
			foreach (EDC_LoetprogrammParameterData item in lstDaten)
			{
				item.PRO_i64VersionsId = i_i64VersionsId;
			}
			await FUN_fdcParameterVersionLoeschenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstDaten, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcParameterListeHinzufuegenAsync(IEnumerable<EDC_LoetprogrammParameterData> i_enuParameterListe, long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_LoetprogrammParameterData> list = i_enuParameterListe.ToList();
			foreach (EDC_LoetprogrammParameterData item in list)
			{
				item.PRO_i64VersionsId = i_i64VersionsId;
			}
			return m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(list, i_fdcTransaktion);
		}

		public Task FUN_fdcParameterVersionLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion)
		{
			string i_strSql = EDC_LoetprogrammParameterData.FUN_strHistoryEintraegeLoeschenStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task FUN_fdcParameterDatenImportierenAsync(DataSet i_fdcDataSet, long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramParameter"];
			if (dataTable != null)
			{
				List<EDC_LoetprogrammParameterData> i_enuParameterListe = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammParameterData>().ToList();
				await FUN_fdcParameterListeSpeichernAsync(i_enuParameterListe, i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task<IEnumerable<EDC_LoetprogrammParameterDiffData>> FUN_fdcErmittleParameterAenderungenZwischenZweiVersionenAsync(long i_i64VersionAltId, long i_i64VersionNeuId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammParameterDiffData.FUN_strErstelleVersionenVergleichAbfrageString(i_i64VersionAltId, i_i64VersionNeuId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync<EDC_LoetprogrammParameterDiffData>(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_LoetprogrammParameterDiffData>> FUN_fdcErmittleParameterWerteEinerVersionenAsync(long i_i64VersionId, IEnumerable<string> i_enuVariablen, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammParameterDiffData.FUN_strErstelleVariablenEinerVersionAbfrageString(i_i64VersionId, i_enuVariablen);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync<EDC_LoetprogrammParameterDiffData>(i_strSql, i_fdcTransaktion);
		}
	}
}
