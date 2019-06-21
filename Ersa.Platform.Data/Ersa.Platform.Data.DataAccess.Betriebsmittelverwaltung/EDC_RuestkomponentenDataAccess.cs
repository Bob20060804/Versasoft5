using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Betriebsmittelverwaltung
{
	public class EDC_RuestkomponentenDataAccess : EDC_DataAccess, INF_RuestkomponentenDataAccess, INF_DataAccess
	{
		public EDC_RuestkomponentenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_RuestkomponentenData>> FUN_fdcLeseRuestkomponentenFuerTypeAusDatenbankAsync(long i_i64MachineGroupId, int i_i32RuestkomponentenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestkomponentenData.FUN_strHoleRuestkomponentenFuerTypWhereStatement(i_i64MachineGroupId, i_i32RuestkomponentenTyp);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_RuestkomponentenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_RuestkomponentenData> FUN_fdcLeseRuestkomponenteFuerIdAusDatenbankAsync(long i_i64RuestkomponentenlId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestkomponentenData.FUN_strRuestkomponentenIdWhereStatementErstellen(i_i64RuestkomponentenlId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_RuestkomponentenData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task<EDC_RuestkomponentenData> FUN_fdcRuestkomponentenDatenSatzHinzufuegenAsync(EDC_RuestkomponentenData i_edcRuestkomponentenData, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcRuestkomponentenData.PRO_i64RuestkomponentenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcRuestkomponentenData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i_edcRuestkomponentenData;
		}

		public Task FUN_fdcRuestkomponentenDatenSatzAendernAsync(EDC_RuestkomponentenData i_edcRuestkomponentenData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcRuestkomponentenData.PRO_strWhereStatement = EDC_RuestkomponentenData.FUN_strRuestkomponentenIdWhereStatementErstellen(i_edcRuestkomponentenData.PRO_i64RuestkomponentenId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcRuestkomponentenData, i_fdcTransaktion);
		}

		public Task FUN_fdcRuestkomponentenDatenSatzLoeschenAsync(long i_i64RuestkomponentenlId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_RuestkomponentenData.FUN_strLoeschenUpdateStatementErstellen(i_i64RuestkomponentenlId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerTypAsync(long i_i64MachineGroupId, ENUM_RuestkomponentenTyp i_enmTyp, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestkomponentenAbfrageData.FUN_strRuestkomponentenTypAbfrageWhereStaement(i_i64MachineGroupId, i_enmTyp);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_RuestkomponentenAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_RuestkomponentenAbfrageData>> FUN_fdcLeseRuestkomponentenUndWerkzeugeFuerKomponentenNameAsync(string i_strName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestkomponentenAbfrageData.FUN_strRuestkomponentenNameAbfrageWhereStaement(i_strName);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_RuestkomponentenAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_RuestkomponentenAbfrageData> FUN_fdcLeseRuestkomponenteUndWerkzeugFuerIdentifikationAsync(ENUM_RuestkomponentenTyp i_enmTyp, string i_strIdentifikation, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_RuestkomponentenAbfrageData.FUN_strRuestwerkzeugIdentifikationWhereStaement(i_enmTyp, i_strIdentifikation);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_RuestkomponentenAbfrageData(i_strWhereStatement), null, i_fdcTransaktion);
		}
	}
}
