using Ersa.Platform.DataContracts.Datenbankdaten;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataContracts.Meldungen;
using Ersa.Platform.DataContracts.Prozessschreiber;
using Ersa.Platform.DataDienste.Benutzer.Interfaces;
using Ersa.Platform.DataDienste.Wartung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Wartung.Implementierung
{
	[Export(typeof(INF_DatenbankWartungsDienst))]
	public class EDC_DatenbankWartungsDienst : EDC_DataDienst, INF_DatenbankWartungsDienst
	{
		private const string mC_strDatenSicherungGestartetTrackKey = "13_787";

		private const string mC_strAuslieferungszustandTrackKey = "14_63";

		private const string mC_strAlteMeldungenLoeschen = "13_789";

		private const string mC_strAlteSchreiberdatenLoeschen = "13_800";

		private const string mC_strDatenbankReorganisieren = "13_804";

		private readonly Lazy<INF_DatenbankdatenDataAccess> m_edcDatenbankdatenDataAccess;

		private readonly Lazy<INF_MeldungenDataAccess> m_edcMeldungenDataAccess;

		private readonly Lazy<INF_ProzessschreiberDataAccess> m_edcProzessschreiberDataAccess;

		private readonly Lazy<INF_MaschinenDataAccess> m_edcMaschinenDataAccess;

		private readonly INF_BenutzerTrackingDienst m_edcBenutzerTrackingDienst;

		[ImportingConstructor]
		public EDC_DatenbankWartungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, INF_BenutzerTrackingDienst i_edcBenutzerTrackingDienst)
			: base(i_edcCapabilityProvider)
		{
			m_edcDatenbankdatenDataAccess = new Lazy<INF_DatenbankdatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DatenbankdatenDataAccess>);
			m_edcMeldungenDataAccess = new Lazy<INF_MeldungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MeldungenDataAccess>);
			m_edcProzessschreiberDataAccess = new Lazy<INF_ProzessschreiberDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_ProzessschreiberDataAccess>);
			m_edcMaschinenDataAccess = new Lazy<INF_MaschinenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenDataAccess>);
			m_edcBenutzerTrackingDienst = i_edcBenutzerTrackingDienst;
		}

		public Task FUN_fdcSpeicherAktuellesDatumAlsLetztesBackupDatumAsync()
		{
			return m_edcDatenbankdatenDataAccess.Value.FUN_fdcSpeicherAktuellesDatumAlsLetztesBackupDatumAsync();
		}

		public Task<string> FUN_fdcLeseLetztesBackupDatumAusDatenbankAsync()
		{
			return m_edcDatenbankdatenDataAccess.Value.FUN_fdcLeseLetztesBackupDatumAusDatenbankAsync();
		}

		public async Task FUN_fdcSichereDieDatenbankAsync(string i_strSicherungspfad)
		{
			await m_edcDatenbankdatenDataAccess.Value.FUN_fdcSichereDieDatenbankAsync(i_strSicherungspfad).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcBenutzerTrackingDienst.FUN_fdcTrackHinzufuegenAsync("13_787").ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcAuslieferungszustandFuerMaschineSetzenAsync()
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			DateTime fdcLoeschDatum = DateTime.Now;
			IDbTransaction fdcTransaktion = await m_edcMeldungenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcMeldungenDataAccess.Value.FUN_fdcLoescheAlleQuittiertenMeldungenVorDatumAsync(i64MaschinenId, fdcLoeschDatum, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcProzessschreiberDataAccess.Value.FUN_fdcLoescheAlleProzessschreiberDatenVorDatumAsync(i64MaschinenId, fdcLoeschDatum, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcMaschinenDataAccess.Value.FUN_fdcStelleBetriebsdatenAufAuslieferungszustandAsync(i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcBenutzerTrackingDienst.FUN_fdcTrackHinzufuegenAsync("14_63", fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcMeldungenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcMeldungenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcLoescheAlteMeldungenVorStartdatumAsync(DateTime i_fdcStartdatum)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMeldungenDataAccess.Value.FUN_fdcLoescheAlleQuittiertenMeldungenVorDatumAsync(i_i64MaschinenId, i_fdcStartdatum).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcBenutzerTrackingDienst.FUN_fdcTrackHinzufuegenAsync("13_789").ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_fdcErmittleDieAnzahlMeldungenVorStartdatumAsync(DateTime i_fdcStartdatum)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMeldungenDataAccess.Value.FUN_fdcErmittleDieAnzahlMeldungenVorStartdatumAsync(i_i64MaschinenId, i_fdcStartdatum).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcLoescheAlteProzessschreiberdatenVorStartdatumAsync(DateTime i_fdcStartdatum)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcProzessschreiberDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await m_edcProzessschreiberDataAccess.Value.FUN_fdcLoescheAlleProzessschreiberDatenVorDatumAsync(i64MaschinenId, i_fdcStartdatum, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcBenutzerTrackingDienst.FUN_fdcTrackHinzufuegenAsync("13_800", fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				m_edcProzessschreiberDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcProzessschreiberDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(DateTime i_fdcStartdatum)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcProzessschreiberDataAccess.Value.FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(i_i64MaschinenId, i_fdcStartdatum, null).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcFuehreDatenbankReorganisierungDurchAsync()
		{
			await m_edcDatenbankdatenDataAccess.Value.FUN_fdcFuehreDatenbankWartungDurchAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcBenutzerTrackingDienst.FUN_fdcTrackHinzufuegenAsync("13_804").ConfigureAwait(continueOnCapturedContext: false);
		}

		public bool FUN_blnIstDatenbankLokalInstalliert()
		{
			return m_edcDatenbankdatenDataAccess.Value.FUN_blnIstDieDatenbankLokalInstalliert();
		}
	}
}
