using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Codetabelle;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Codetabelle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Codetabelle
{
	public class EDC_CodetabellenDataAccess : EDC_DataAccess, INF_CodetabellenDataAccess, INF_DataAccess
	{
		public EDC_CodetabellenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_CodetabelleData> FUN_edcCodetabelleLadenAsync(long i_i64CodetabellenId)
		{
			string i_strWhereStatement = EDC_CodetabelleData.FUN_strCodetabellenIdWhereStatementErstellen(i_i64CodetabellenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_CodetabelleData(i_strWhereStatement));
		}

		public Task<IEnumerable<EDC_CodetabelleData>> FUN_fdcCodetabellenLadenAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_CodetabelleData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodetabelleData(i_strWhereStatement));
		}

		public Task<IEnumerable<EDC_CodetabelleneintragData>> FUN_fdcCodetabelleneintraegeLadenAsync(long i_i64CodetabellenId)
		{
			string i_strWhereStatement = EDC_CodetabelleneintragData.FUN_strCodetabellenIdWhereStatementErstellen(i_i64CodetabellenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_CodetabelleneintragData(i_strWhereStatement));
		}

		public async Task<long> FUN_fdcCodetabelleMitEintraegenHinzufuegenAsync(string i_strCodetabellenName, IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege, long i_i64BenutzerId, long i_i64MaschinenGruppenId)
		{
			List<EDC_CodetabelleneintragData> lstCodetabellenEintraege = i_enuCodetabelleneintraege.ToList();
			uint i64NeueCodeTabellenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			DateTime now = DateTime.Now;
			EDC_CodetabelleData edcCodetabelle = new EDC_CodetabelleData
			{
				PRO_strName = i_strCodetabellenName,
				PRO_i64CodetabellenId = i64NeueCodeTabellenId,
				PRO_dtmAngelegtAm = now,
				PRO_i64AngelegtVon = i_i64BenutzerId,
				PRO_i64GruppenId = i_i64MaschinenGruppenId,
				PRO_dtmBearbeitetAm = now,
				PRO_i64BearbeitetVon = i_i64BenutzerId
			};
			foreach (EDC_CodetabelleneintragData item in lstCodetabellenEintraege)
			{
				item.PRO_i64CodetabellenId = i64NeueCodeTabellenId;
				EDC_CodetabelleneintragData eDC_CodetabelleneintragData = item;
				eDC_CodetabelleneintragData.PRO_i64EintragsId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcCodetabelle, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstCodetabellenEintraege, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
			return i64NeueCodeTabellenId;
		}

		public async Task FUN_fdcCodetabelleMitEintraegenAendernAsync(long i_i64CodeTabellenId, string i_strNeuerName, IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege, long i_i64BenutzerId)
		{
			List<EDC_CodetabelleneintragData> lstCodetabellenEintraege = i_enuCodetabelleneintraege.ToList();
			Dictionary<string, object> dicParameterDictionary = new Dictionary<string, object>();
			string strCodeTabellenUpdateStatement = EDC_CodetabelleData.FUN_strUpdateStatementErstellen(i_i64CodeTabellenId, i_strNeuerName, i_i64BenutzerId, DateTime.Now, dicParameterDictionary);
			string strCodeTabellenEintraegeLoeschenWhereStatement = EDC_CodetabelleneintragData.FUN_strCodetabellenIdWhereStatementErstellen(i_i64CodeTabellenId);
			foreach (EDC_CodetabelleneintragData item in lstCodetabellenEintraege)
			{
				item.PRO_i64CodetabellenId = i_i64CodeTabellenId;
				EDC_CodetabelleneintragData eDC_CodetabelleneintragData = item;
				long num2 = eDC_CodetabelleneintragData.PRO_i64EintragsId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(strCodeTabellenUpdateStatement, fdcTransaktion, dicParameterDictionary).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodetabelleneintragData(strCodeTabellenEintraegeLoeschenWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSchreibeObjekteInTabelleAsync(lstCodetabellenEintraege, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcCodetabelleMitEintraegenLoeschenAsync(long i_i64CodetabellenId)
		{
			string strCodeTabelleLoeschenWhereStatement = EDC_CodetabelleData.FUN_strCodetabellenIdWhereStatementErstellen(i_i64CodetabellenId);
			string strCodeTabellenEintraegeLoeschenWhereStatement = EDC_CodetabelleneintragData.FUN_strCodetabellenIdWhereStatementErstellen(i_i64CodetabellenId);
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodetabelleData(strCodeTabelleLoeschenWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_CodetabelleneintragData(strCodeTabellenEintraegeLoeschenWhereStatement), fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}
	}
}
