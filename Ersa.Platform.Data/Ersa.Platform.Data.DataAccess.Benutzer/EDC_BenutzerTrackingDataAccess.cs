using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Benutzer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Benutzer
{
	public class EDC_BenutzerTrackingDataAccess : EDC_DataAccess, INF_BenutzerTrackingDataAccess, INF_DataAccess
	{
		public EDC_BenutzerTrackingDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task FUN_fdcTrackHinzufuegenAsync(EDC_BenutzerTrackData i_edcTrack, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcTrack.PRO_i64MaschinenId = i_i64MaschinenId;
			return FUN_fdcTrackSpeichernAsync(i_edcTrack, i_fdcTransaktion);
		}

		public async Task FUN_fdcTrackingsHinzufuegenAsync(IEnumerable<EDC_BenutzerTrackData> i_enuTrackListe, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (EDC_BenutzerTrackData item in i_enuTrackListe)
				{
					await FUN_fdcTrackSpeichernAsync(item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<IEnumerable<EDC_BenutzerTrackData>> FUN_fdcHoleTrackingsImIntervallAsync(long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strWhereStatement = EDC_BenutzerTrackData.FUN_strErstellIntervallWhereStatementErstellen(i_i64MaschinenId, i_sttVon, i_sttBis, dictionary);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_BenutzerTrackData(i_strWhereStatement), i_fdcTransaktion, dictionary);
		}

		private async Task FUN_fdcTrackSpeichernAsync(EDC_BenutzerTrackData i_edcTrack, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcTrack.PRO_i64TrackingId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcTrack, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
