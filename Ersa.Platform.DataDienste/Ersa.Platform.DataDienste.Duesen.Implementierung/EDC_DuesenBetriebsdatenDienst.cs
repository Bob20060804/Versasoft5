using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.Common.Selektiv;
using Ersa.Platform.DataContracts.Duesentabelle;
using Ersa.Platform.DataDienste.Duesen.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Duesen.Implementierung
{
	[Export(typeof(INF_DuesenBetriebsdatenDienst))]
	public class EDC_DuesenBetriebsdatenDienst : EDC_DataDienst, INF_DuesenBetriebsdatenDienst
	{
		private readonly Lazy<INF_DuesenbetriebDataAccess> m_edcDuesenbetriebDataAccess;

		private readonly Lazy<INF_DuesentabelleDataAccess> m_edcDuesenTabellenDataAccess;

		[ImportingConstructor]
		public EDC_DuesenBetriebsdatenDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcDuesenbetriebDataAccess = new Lazy<INF_DuesenbetriebDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DuesenbetriebDataAccess>);
			m_edcDuesenTabellenDataAccess = new Lazy<INF_DuesentabelleDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DuesentabelleDataAccess>);
		}

		public async Task<IEnumerable<EDC_DuesenbetriebWerteData>> FUN_fdcHoleAlleAktuellenDuesenBetriebswerteFuerMaschineAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcDuesenbetriebDataAccess.Value.FUN_fdcHoleAlleAktuellenDuesenBetriebswerteFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_DuesenBetriebWerte> FUN_fdcHoleAktuelleDuesenBetriebswerteFuerDueseAsync(long i_i64DuesenId, ENUM_SelektivTiegel i_enmTiegel)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64GeomertieId = await FUN_fdcHoleGeomertieIdFuerDueseAsync(i_i64DuesenId).ConfigureAwait(continueOnCapturedContext: false);
			EDC_DuesenbetriebWerteData edcNeueDuesenWerteData = (await FUN_fdcHoleAktuellesDuesenBetriebswertDataFuerDueseAsync(i64GeomertieId, i64MaschinenId, i_enmTiegel).ConfigureAwait(continueOnCapturedContext: false)) ?? (await FUN_fdcErstelleNeuenDuesenBetriebswertAsync(i64GeomertieId, i64MaschinenId, i_enmTiegel).ConfigureAwait(continueOnCapturedContext: false));
			if (edcNeueDuesenWerteData != null)
			{
				long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDuesenbetriebDataAccess.Value.FUN_fdcTrackeDuesenWechselAsync(edcNeueDuesenWerteData, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
				return edcNeueDuesenWerteData.FUN_edcKonvertiereZuBetriebsWerte(i_i64DuesenId);
			}
			return null;
		}

		public async Task FUN_fdcSpeichereAktuelleDuesenBetriebswerteFuerDueseAsync(EDC_DuesenBetriebWerte i_edcWerteAktuelleDuese, bool i_blnDueseIstJetztVerbraucht)
		{
			await FUN_fdcSpeichereDuesenBetriebswerteAsync(i_i64MaschinenId: await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false), i_i64GeometrieId: await FUN_fdcHoleGeomertieIdFuerDueseAsync(i_edcWerteAktuelleDuese.PRO_i64DuesenId).ConfigureAwait(continueOnCapturedContext: false), i_edcWerteAktuelleDuese: i_edcWerteAktuelleDuese, i_blnDueseIstJetztVerbraucht: i_blnDueseIstJetztVerbraucht).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereAktuelleDuesenBetriebswerteAsync(IEnumerable<EDC_DuesenBetriebWerte> i_enuWerteAktuelleDuese)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_DuesenBetriebWerte> list = i_enuWerteAktuelleDuese.ToList();
			foreach (EDC_DuesenBetriebWerte edcWert in list)
			{
				await FUN_fdcSpeichereDuesenBetriebswerteAsync(await FUN_fdcHoleGeomertieIdFuerDueseAsync(edcWert.PRO_i64DuesenId).ConfigureAwait(continueOnCapturedContext: false), i64MaschinenId, edcWert, i_blnDueseIstJetztVerbraucht: false).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync()
		{
			return m_edcDuesenTabellenDataAccess.Value.FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync();
		}

		public async Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleVonMaschineVerwendetenStandardGeometrienAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcDuesenTabellenDataAccess.Value.FUN_fdcHoleAlleVonMaschineVerwendetenStandardGeometrienAsync(i_i64MaschinenId);
		}

		public Task<bool> FUN_fdcAktualisiereDuesenGeometrienAusDateiAsync(string i_strImportDatei)
		{
			return m_edcDuesenTabellenDataAccess.Value.FUN_fdcAktualisiereDuesenGeometrienAusDateiAsync(i_strImportDatei);
		}

		public async Task<IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData>> FUN_fdcHoleAlleVonMaschineVerwendetenLoetduesenGeometrienUndSollwerteAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcDuesenTabellenDataAccess.Value.FUN_fdcHoleAlleDuesenGeometrienUndSollwerteAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<EDC_DuesenGeometrieUndSollwerteAbfrageData> FUN_fdcHoleAlleLoetduesenGeometrienUndSollwerteZuEinerGeometrieAsync(long i_i64GeometrieId)
		{
			return m_edcDuesenTabellenDataAccess.Value.FUN_fdcHoleDuesenGeometrieUndSollwerteZuEinerGeometrieAsync(i_i64GeometrieId);
		}

		public async Task FUN_fdcSpeichereAktuelleDuesenSollwerteAsync(IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData> i_enuWerteAktuelleSollwerteDuese)
		{
			IDbTransaction fdcTransaktion = await m_edcDuesenTabellenDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (EDC_DuesenGeometrieUndSollwerteAbfrageData item in i_enuWerteAktuelleSollwerteDuese.ToList())
				{
					EDC_DuesenSollwerteData edcDuesenSollwerte = new EDC_DuesenSollwerteData
					{
						PRO_i64GeometrieId = item.PRO_i64GeometrieId,
						PRO_i64MaxBetriebszeit = item.PRO_i64MaxBetriebszeit
					};
					if (item.PRO_i64GeomertyIdInSollwerte == 0L)
					{
						await m_edcDuesenTabellenDataAccess.Value.FUN_fdcDuesenSollwerteDatenSatzHinzufuegenAsync(edcDuesenSollwerte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						await m_edcDuesenTabellenDataAccess.Value.FUN_fdcDuesenSollwerteDatenSatzAendernAsync(edcDuesenSollwerte, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				m_edcDuesenTabellenDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcDuesenTabellenDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcSpeichereDuesenBetriebswerteAsync(long i_i64GeometrieId, long i_i64MaschinenId, EDC_DuesenBetriebWerte i_edcWerteAktuelleDuese, bool i_blnDueseIstJetztVerbraucht)
		{
			EDC_DuesenbetriebWerteData eDC_DuesenbetriebWerteData = await FUN_fdcHoleAktuellesDuesenBetriebswertDataFuerDueseAsync(i_i64GeometrieId, i_i64MaschinenId, i_edcWerteAktuelleDuese.PRO_enmTiegel).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_DuesenbetriebWerteData == null)
			{
				eDC_DuesenbetriebWerteData = await FUN_fdcErstelleNeuenDuesenBetriebswertAsync(i_i64GeometrieId, i_i64MaschinenId, i_edcWerteAktuelleDuese.PRO_enmTiegel).ConfigureAwait(continueOnCapturedContext: false);
			}
			EDC_DuesenbetriebWerteData eDC_DuesenbetriebWerteData2 = eDC_DuesenbetriebWerteData;
			if (eDC_DuesenbetriebWerteData2 != null)
			{
				SUB_UebernehmeWerte(eDC_DuesenbetriebWerteData2, i_edcWerteAktuelleDuese);
				if (i_blnDueseIstJetztVerbraucht)
				{
					eDC_DuesenbetriebWerteData2.PRO_dtmDuesenEndeDatum = DateTime.Now;
				}
				await m_edcDuesenbetriebDataAccess.Value.FUN_fdcDuesenBetriebswertSpeichernAsync(eDC_DuesenbetriebWerteData2).ConfigureAwait(continueOnCapturedContext: false);
				if (i_blnDueseIstJetztVerbraucht)
				{
					await FUN_fdcErstelleNeuenDuesenBetriebswertAsync(i_i64GeometrieId, i_i64MaschinenId, i_edcWerteAktuelleDuese.PRO_enmTiegel).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		private async Task<EDC_DuesenbetriebWerteData> FUN_fdcHoleAktuellesDuesenBetriebswertDataFuerDueseAsync(long i_i64GeometrieId, long i_i64MaschinenId, ENUM_SelektivTiegel i_enmTiegel)
		{
			if (i_i64GeometrieId == 0L)
			{
				return null;
			}
			return await m_edcDuesenbetriebDataAccess.Value.FUN_fdcHoleAktuelleDuesenBetriebswerteFuerDueseAsync(i_i64MaschinenId, i_i64GeometrieId, i_enmTiegel).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<EDC_DuesenbetriebWerteData> FUN_fdcErstelleNeuenDuesenBetriebswertAsync(long i_i64GeometrieId, long i_i64MaschinenId, ENUM_SelektivTiegel i_enmTiegel)
		{
			if (i_i64GeometrieId == 0L)
			{
				return null;
			}
			return await m_edcDuesenbetriebDataAccess.Value.FUN_fdcErstelleNeuenDuesenBetriebswertAsync(i_i64MaschinenId, i_i64GeometrieId, i_enmTiegel).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<long> FUN_fdcHoleGeomertieIdFuerDueseAsync(long i_i64DuesenId)
		{
			return (await m_edcDuesenTabellenDataAccess.Value.FUN_fdcHoleDuesenDatenMitDuesenIdAsync(i_i64DuesenId).ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64GeometrieId ?? 0;
		}

		private void SUB_UebernehmeWerte(EDC_DuesenbetriebWerteData i_edcWerteData, EDC_DuesenBetriebWerte i_edcWerte)
		{
			i_edcWerteData.PRO_i64AnzahlAktivierungen = i_edcWerte.PRO_i64AnzahlAktivierungen;
			i_edcWerteData.PRO_i64WelleEinZeit = i_edcWerte.PRO_i64WelleEinZeit;
			i_edcWerteData.PRO_i64WelleAusZeit = i_edcWerte.PRO_i64WelleAusZeit;
			i_edcWerteData.PRO_i64GesamtZeit = i_edcWerte.PRO_i64GesamtZeit;
		}
	}
}
