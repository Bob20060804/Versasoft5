using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.DataContracts.Benutzer;
using Ersa.Platform.DataDienste.Benutzer.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Benutzer
{
	[Export(typeof(INF_BenutzerTrackingDienst))]
	public class EDC_BenutzerTrackingDienst : EDC_DataDienst, INF_BenutzerTrackingDienst
	{
		private const string mC_strAnmeldenKey = "13_32";

		private const string mC_strAbmeldenKey = "13_33";

		private static readonly SemaphoreSlim ms_fdcSemaphore = new SemaphoreSlim(1);

		private readonly Lazy<INF_BenutzerVerwaltungDataAccess> m_edcBenutzerDataAccess;

		private readonly Lazy<INF_BenutzerTrackingDataAccess> m_edcTrackingDataAccess;

		private readonly Lazy<INF_AktiveBenutzerDataAccess> m_edcAktiveBenutzerDataAccess;

		private readonly IEventAggregator m_fdcEventAggregator;

		private long? m_i64IdLetzterAngemeldeterBenutzer;

		[ImportingConstructor]
		public EDC_BenutzerTrackingDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, IEventAggregator i_fdcEventAggregator)
			: base(i_edcCapabilityProvider)
		{
			m_edcBenutzerDataAccess = new Lazy<INF_BenutzerVerwaltungDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_BenutzerVerwaltungDataAccess>);
			m_edcTrackingDataAccess = new Lazy<INF_BenutzerTrackingDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_BenutzerTrackingDataAccess>);
			m_edcAktiveBenutzerDataAccess = new Lazy<INF_AktiveBenutzerDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_AktiveBenutzerDataAccess>);
			m_fdcEventAggregator = i_fdcEventAggregator;
		}

		public void SUB_Initialisieren()
		{
			m_fdcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Subscribe(async delegate(EDC_BenutzerGeaendertEventPayload i_edcPayload)
			{
				await FUN_fdcBenutzerGeaendertBehandelnAsync(i_edcPayload.PRO_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
			});
		}

		public Task FUN_fdcTrackHinzufuegenAsync(string i_strActivityKey, IDbTransaction i_fdcTransaktion = null)
		{
			if (m_i64IdLetzterAngemeldeterBenutzer.HasValue)
			{
				return FUN_fdcTrackHinzufuegenAsync(m_i64IdLetzterAngemeldeterBenutzer.Value, i_strActivityKey, i_fdcTransaktion);
			}
			return Task.FromResult(0);
		}

		public async Task FUN_fdcParameterTrackHinzufuegenAsync(IEnumerable<EDC_ParameterTrack> i_enuParameter)
		{
			long pRO_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long pRO_i64BenutzerId = m_i64IdLetzterAngemeldeterBenutzer ?? 0;
			List<EDC_BenutzerTrackData> list = new List<EDC_BenutzerTrackData>();
			foreach (EDC_ParameterTrack item in i_enuParameter)
			{
				list.Add(new EDC_BenutzerTrackData
				{
					PRO_i64BenutzerId = pRO_i64BenutzerId,
					PRO_dtmZeitpunkt = DateTime.Now,
					PRO_i64MaschinenId = pRO_i64MaschinenId,
					PRO_strAktivitaet = item.PRO_strActivityKey,
					PRO_strParameter = item.PRO_strParameter,
					PRO_strAlterWert = item.PRO_strAlterWert,
					PRO_strNeuerWert = item.PRO_strNeuerWert
				});
			}
			await m_edcTrackingDataAccess.Value.FUN_fdcTrackingsHinzufuegenAsync(list).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcTrackHinzufuegenAsync(long i_i64BenutzerId, string i_strActivityKey, IDbTransaction i_fdcTransaktion = null)
		{
			long num = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_BenutzerTrackData i_edcTrack = new EDC_BenutzerTrackData
			{
				PRO_i64BenutzerId = i_i64BenutzerId,
				PRO_dtmZeitpunkt = DateTime.Now,
				PRO_i64MaschinenId = num,
				PRO_strAktivitaet = i_strActivityKey
			};
			await m_edcTrackingDataAccess.Value.FUN_fdcTrackHinzufuegenAsync(i_edcTrack, num, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcBenutzerGeaendertBehandelnAsync(long i_i64BenutzerId)
		{
			try
			{
				await ms_fdcSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcBenutzerAbgemeldetBehandelnAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (i_i64BenutzerId == 0L)
				{
					m_i64IdLetzterAngemeldeterBenutzer = null;
				}
				else
				{
					m_i64IdLetzterAngemeldeterBenutzer = i_i64BenutzerId;
				}
				if (m_i64IdLetzterAngemeldeterBenutzer.HasValue)
				{
					await FUN_fdcTrackHinzufuegenAsync(m_i64IdLetzterAngemeldeterBenutzer.Value, "13_32").ConfigureAwait(continueOnCapturedContext: false);
					await FUN_fdcAktiverBenutzerHinzufuegenAsync(m_i64IdLetzterAngemeldeterBenutzer.Value).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			finally
			{
				ms_fdcSemaphore.Release(1);
			}
		}

		private async Task FUN_fdcBenutzerAbgemeldetBehandelnAsync()
		{
			if (m_i64IdLetzterAngemeldeterBenutzer.HasValue)
			{
				await FUN_fdcAktiverBenutzerEntfernenAsync(m_i64IdLetzterAngemeldeterBenutzer.Value).ConfigureAwait(continueOnCapturedContext: false);
				await FUN_fdcTrackHinzufuegenAsync(m_i64IdLetzterAngemeldeterBenutzer.Value, "13_33").ConfigureAwait(continueOnCapturedContext: false);
			}
			m_i64IdLetzterAngemeldeterBenutzer = null;
		}

		private async Task<long> FUN_fdcBenutzerIdErmittelnAsync(string i_strBenutzername)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (await m_edcBenutzerDataAccess.Value.FUN_fdcMaschinenBenutzerMitNamenLadenAsync(i_strBenutzername, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault((EDC_BenutzerAbfrageData i_edcBenutzer) => i_edcBenutzer.PRO_blnIstAktiv)?.PRO_i64BenutzerId ?? 0;
		}

		private async Task FUN_fdcAktiverBenutzerHinzufuegenAsync(long i_i64BenutzerId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_AktiverBenutzerData i_edcAktiverBenutzer = new EDC_AktiverBenutzerData
			{
				PRO_i64BenutzerId = i_i64BenutzerId,
				PRO_dtmLoginZeitpunkt = DateTime.Now,
				PRO_strIp = FUN_strIpErmitteln()
			};
			await m_edcAktiveBenutzerDataAccess.Value.FUN_fdcAktiverBenutzerHinzufuegenAsync(i_edcAktiverBenutzer, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcAktiverBenutzerEntfernenAsync(long i_i64BenutzerId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcAktiveBenutzerDataAccess.Value.FUN_fdcAktiverBenutzerEntfernenAsync(i_i64BenutzerId, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		private string FUN_strIpErmitteln()
		{
			return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault((IPAddress i_fdcIpAdresse) => i_fdcIpAdresse.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? string.Empty;
		}
	}
}
