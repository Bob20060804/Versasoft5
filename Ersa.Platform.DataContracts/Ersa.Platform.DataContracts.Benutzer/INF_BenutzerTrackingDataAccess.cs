using Ersa.Platform.Common.Data.Benutzer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Benutzer
{
	public interface INF_BenutzerTrackingDataAccess : INF_DataAccess
	{
		Task FUN_fdcTrackHinzufuegenAsync(EDC_BenutzerTrackData i_edcTrack, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcTrackingsHinzufuegenAsync(IEnumerable<EDC_BenutzerTrackData> i_enuTrackListe, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BenutzerTrackData>> FUN_fdcHoleTrackingsImIntervallAsync(long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null);
	}
}
