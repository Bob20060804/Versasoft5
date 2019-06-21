using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Betriebsmittelverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Betriebsmittelverwaltung
{
	public class EDC_BetriebsmittelDataAccess : EDC_DataAccess, INF_BetriebsmittelDataAccess, INF_DataAccess
	{
		public EDC_BetriebsmittelDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcLeseBetriebsmittelFuerTypeAusDatenbankAsync(int i_i32BetriebsmittelTyp, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BetriebsmittelData.FUN_strHoleBetriebsmittelTypWhereStatement(i_i32BetriebsmittelTyp);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BetriebsmittelData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_BetriebsmittelData> FUN_fdcLeseBetriebsmittelDatenFuerIdAusDatenbankAsync(long i_i64BetriebsmittelId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_BetriebsmittelData.FUN_strBetriebsmittelIdWhereStatementErstellen(i_i64BetriebsmittelId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_BetriebsmittelData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task<EDC_BetriebsmittelData> FUN_fdcBetriebsmittelDatenSatzHinzufuegenAsync(EDC_BetriebsmittelData i_edcBetriebsmittelData, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcBetriebsmittelData.PRO_i64BetriebsmittelId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcBetriebsmittelData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i_edcBetriebsmittelData;
		}

		public Task FUN_fdcBetriebsmittelDatenSatzAendernAsync(EDC_BetriebsmittelData i_edcBetriebsmittelData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcBetriebsmittelData.PRO_strWhereStatement = EDC_BetriebsmittelData.FUN_strBetriebsmittelIdWhereStatementErstellen(i_edcBetriebsmittelData.PRO_i64BetriebsmittelId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcBetriebsmittelData, i_fdcTransaktion);
		}

		public Task FUN_fdcBetriebsmittelDatenSatzLoeschenAsync(long i_i64BetriebsmittelId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_BetriebsmittelData.FUN_strLoeschenUpdateStatementErstellen(i_i64BetriebsmittelId);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}
	}
}
