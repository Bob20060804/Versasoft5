using Ersa.Platform.CapabilityContracts.BenutzerVerwaltung;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.DataContracts.Meldungen;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	[EDC_AuthentifizierungsProviderExportMetadaten(PRO_strAuthentifizierungsProvider = "Default")]
	public class EDC_DefaultAuthentifizierungsProvider : INF_AuthentifizierungsProvider
	{
		private readonly Lazy<INF_BenutzerVerwaltungCapability> m_edcBenutzerVerwaltungCapability;

		private readonly Lazy<INF_MeldungenDataAccess> m_edcMeldungenDataAccess;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<EDC_RechteGruppe> PRO_edcRechteGruppen
		{
			private get;
			set;
		}

		[ImportingConstructor]
		public EDC_DefaultAuthentifizierungsProvider(INF_CapabilityProvider i_edcCapabilityProvider, INF_DataAccessProvider i_edcDataAccessProvider)
		{
			m_edcBenutzerVerwaltungCapability = new Lazy<INF_BenutzerVerwaltungCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_BenutzerVerwaltungCapability>);
			m_edcMeldungenDataAccess = new Lazy<INF_MeldungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MeldungenDataAccess>);
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
		}

		public async Task<ClaimsIdentity> FUN_fdcPruefeUndLadeClaimsFuerBenutzerAsync(string i_strBenutzerName, string i_strPasswort)
		{
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (EDC_AuthentifizierungsProviderHelfer.FUN_blnIstErsaRolle(i_strBenutzerName, i_strPasswort, await m_edcMeldungenDataAccess.Value.FUN_fdcErmittleDatumLetzteQuittierteMeldungAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: true)))
			{
				long i_i64BenutzerId = await FUN_fdcErmittleDefaultBenutzerIdAsync("ersa").ConfigureAwait(continueOnCapturedContext: true);
				return EDC_AuthentifizierungsProviderHelfer.FUN_fdcErsaIdentitaetErstellen(FUN_lstRechteBeschreibungenLaden(), i_i64BenutzerId);
			}
			return null;
		}

		public Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerCodeAsync(string i_strCode)
		{
			return Task.FromResult<ClaimsIdentity>(null);
		}

		public Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerIdAsync(long i_i64BenutzerId)
		{
			return Task.FromResult<ClaimsIdentity>(null);
		}

		public async Task<ClaimsIdentity> FUN_fdcDefaultIdentitaetLadenAsync()
		{
			long i_i64BenutzerId = await FUN_fdcErmittleDefaultBenutzerIdAsync("admin").ConfigureAwait(continueOnCapturedContext: true);
			return EDC_AuthentifizierungsProviderHelfer.FUN_fdcAdministratorIdentitaetErstellen("admin", FUN_lstRechteBeschreibungenLaden(), i_i64BenutzerId);
		}

		public Task<ClaimsIdentity> FUN_fdcAutoAbmeldungIdentitaetLadenAsync()
		{
			return Task.FromResult<ClaimsIdentity>(null);
		}

		private IEnumerable<EDC_RechtBeschreibung> FUN_lstRechteBeschreibungenLaden()
		{
			return PRO_edcRechteGruppen.SelectMany((EDC_RechteGruppe i_edcRechteBeschreibungen) => i_edcRechteBeschreibungen.PRO_edcRechte);
		}

		private async Task<long> FUN_fdcErmittleDefaultBenutzerIdAsync(string i_strName)
		{
			if (m_edcBenutzerVerwaltungCapability.Value == null)
			{
				return 0L;
			}
			return (await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcDefaultBenutzerDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).ToList().FirstOrDefault((EDC_BenutzerAbfrageData i_edcBenutzer) => i_edcBenutzer.PRO_strBenutzername == i_strName)?.PRO_i64BenutzerId ?? 0;
		}
	}
}
