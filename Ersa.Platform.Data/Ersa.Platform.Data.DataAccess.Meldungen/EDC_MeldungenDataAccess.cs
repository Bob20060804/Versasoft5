using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Meldungen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Meldungen
{
	public class EDC_MeldungenDataAccess : EDC_DataAccess, INF_MeldungenDataAccess, INF_DataAccess
	{
		public EDC_MeldungenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcAlleMeldungenImIntervallAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strWhereStatement = EDC_MeldungenAbfrageData.FUN_strAlleAufgetretenWhereStatementErstellen(i_enmMeldungProduzent, i_enmMeldungsTyp, i_sttVon, i_sttBis, i_i64MaschinenId, dictionary);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MeldungenAbfrageData(i_strWhereStatement), i_fdcTransaktion, dictionary);
		}

		public Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcQuittierteMeldungenImIntervallAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strWhereStatement = EDC_MeldungenAbfrageData.FUN_strAlleQuittiertenWhereStatementErstellen(i_enmMeldungProduzent, i_enmMeldungsTyp, i_sttVon, i_sttBis, i_i64MaschinenId, dictionary);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MeldungenAbfrageData(i_strWhereStatement), i_fdcTransaktion, dictionary);
		}

		public Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcAlleNichtQuittiertenMeldungenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MeldungenAbfrageData.FUN_strNichtQittierteMeldungenWhereStatementErstellen(i_enmMeldungProduzent, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MeldungenAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcMeldungenHinzufuegenAsync(IEnumerable<EDC_MeldungData> i_enuNeueMeldungen, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(i_enuNeueMeldungen, i_fdcTransaktion);
		}

		public async Task FUN_fdcMeldungenAktualisierenAsync(IEnumerable<EDC_MeldungData> i_enuNeueMeldungen, IDbTransaction i_fdcTransaktion = null)
		{
			List<EDC_MeldungData> lstNeueMeldungen = i_enuNeueMeldungen.ToList();
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_MeldungData item in lstNeueMeldungen)
				{
					await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task FUN_fdcMeldungHinzufuegenAsync(EDC_MeldungData i_edcMeldung, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcMeldung.PRO_strMeldungsId = Guid.NewGuid().ToString();
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcMeldung, i_fdcTransaktion);
		}

		public Task<EDC_MeldungData> FUN_fdcMeldungenLadenAsync(string i_strMeldungId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MeldungData.FUN_strMeldungsIdWhereStatementErstellen(i_strMeldungId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MeldungData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task FUN_fdcMeldungenContextHinzufuegenAsync(IEnumerable<EDC_MeldungContextData> i_enuMeldungenContext, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(i_enuMeldungenContext, i_fdcTransaktion);
		}

		public Task FUN_fdcMeldungContextHinzufuegenAsync(EDC_MeldungContextData i_edcMeldungenContext, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcMeldungenContext, i_fdcTransaktion);
		}

		public Task<EDC_MeldungContextData> FUN_fdcMeldungenContextLadenAsync(string i_strMeldungId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MeldungContextData.FUN_strMeldungsIdWhereStatementErstellen(i_strMeldungId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MeldungContextData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task FUN_fdcZyklischeMeldungenHinzufuegenAsync(IEnumerable<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> i_enuNeueMeldungenTuple, IDbTransaction i_fdcTransaktion = null)
		{
			List<EDC_ZyklischeMeldungData> lstZyklisch = new List<EDC_ZyklischeMeldungData>();
			List<EDC_MeldungData> lstMeld = new List<EDC_MeldungData>();
			foreach (Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData> item in i_enuNeueMeldungenTuple.ToList())
			{
				item.Item1.PRO_strMeldungsId = item.Item2.PRO_strMeldungsId;
				lstZyklisch.Add(item.Item1);
				lstMeld.Add(item.Item2);
			}
			if (lstMeld.Any())
			{
				IDbTransaction dbTransaction = i_fdcTransaktion;
				if (dbTransaction == null)
				{
					dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				IDbTransaction fdcTransaktion = dbTransaction;
				try
				{
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(lstMeld, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(lstZyklisch, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (i_fdcTransaktion == null)
					{
						SUB_CommitTransaktion(fdcTransaktion);
					}
				}
				catch
				{
					if (i_fdcTransaktion == null)
					{
						SUB_RollbackTransaktion(fdcTransaktion);
					}
					throw;
				}
			}
		}

		public async Task FUN_fdcZyklischeMeldungenVorlagenSchreibenAsync(IEnumerable<EDC_ZyklischeMeldungVorlageGruppeData> i_enuGruppen, IEnumerable<EDC_ZyklischeMeldungVorlageData> i_enuMeldungen, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				await m_edcDatenbankAdapter.FUN_fdcLeereTabelleAsync("CyclicMessageLib").ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcLeereTabelleAsync("MessagesCyclicTemplates").ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(i_enuGruppen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(i_enuMeldungen, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<IEnumerable<EDC_ZyklischeMeldungVorlageData>> FUN_fdcZyklischeMeldungenEinerVorlageGruppeAsync(long i_i64GruppeId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_ZyklischeMeldungVorlageData.FUN_strVorlageGruppeIdWhereStatementErstellen(i_i64GruppeId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_ZyklischeMeldungVorlageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_ZyklischeMeldungVorlageGruppeData>> FUN_fdcAlleZyklischeMeldungVorlagenGruppenAsync(IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_ZyklischeMeldungVorlageGruppeData(), i_fdcTransaktion);
		}

		public async Task<long> FUN_fdcErmittleDieAnzahlMeldungenVorStartdatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strSql = EDC_MeldungData.FUN_strErstelleSelectCountVorDatumStatement(i_fdcStartzeitpunkt, i_i64MaschinenId, dictionary);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return 0L;
			}
			return Convert.ToInt64(obj);
		}

		public async Task<DateTime> FUN_fdcErmittleDatumLetzteQuittierteMeldungAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_MeldungData.FUN_strErstelleSelectMaxMeldungenStatement(i_i64MaschinenId);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return DateTime.Now.AddMinutes(-1.0);
			}
			return Convert.ToDateTime(obj);
		}

		public async Task FUN_fdcLoescheAlleQuittiertenMeldungenVorDatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				string i_strSql = EDC_MeldungData.FUN_strLoescheMeldungenQuittiertVorDatumStatement(i_fdcStartzeitpunkt, i_i64MaschinenId, dictionary);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
				string i_strSql2 = EDC_ZyklischeMeldungData.FUN_strLoescheUngueltigeZyklischeMeldungenStatement();
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql2, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task FUN_fdcMigriereDefaultWerteFuerExterneMeldungenAsync(IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_MeldungData.FUN_strExterneMeldungenDefaultUpdateStatementErstellen();
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}
	}
}
