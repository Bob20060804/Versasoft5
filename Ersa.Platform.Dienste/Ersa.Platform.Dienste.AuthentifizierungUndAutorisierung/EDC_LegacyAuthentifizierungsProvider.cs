using Ersa.Platform.CapabilityContracts.BenutzerVerwaltung;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.DataContracts.Meldungen;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	[EDC_AuthentifizierungsProviderExportMetadaten(PRO_strAuthentifizierungsProvider = "LegacyProvider")]
	public class EDC_LegacyAuthentifizierungsProvider : INF_AuthentifizierungsProvider
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
		public EDC_LegacyAuthentifizierungsProvider(INF_CapabilityProvider i_edcCapabilityProvider, INF_DataAccessProvider i_edcDataAccessProvider)
		{
			m_edcBenutzerVerwaltungCapability = new Lazy<INF_BenutzerVerwaltungCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_BenutzerVerwaltungCapability>);
			m_edcMeldungenDataAccess = new Lazy<INF_MeldungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MeldungenDataAccess>);
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
		}

		public async Task<ClaimsIdentity> FUN_fdcPruefeUndLadeClaimsFuerBenutzerAsync(string i_strBenutzerName, string i_strPasswort)
		{
			long i_i64MaschinenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (EDC_AuthentifizierungsProviderHelfer.FUN_blnIstErsaRolle(i_strBenutzerName, i_strPasswort, await m_edcMeldungenDataAccess.Value.FUN_fdcErmittleDatumLetzteQuittierteMeldungAsync(i_i64MaschinenId)))
			{
				long i_i64BenutzerId = await FUN_fdcErmittleDefaultBenutzerIdAsync("ersa").ConfigureAwait(continueOnCapturedContext: true);
				return EDC_AuthentifizierungsProviderHelfer.FUN_fdcErsaIdentitaetErstellen(FUN_lstRechteBeschreibungenLaden(), i_i64BenutzerId);
			}
			if (m_edcBenutzerVerwaltungCapability.Value == null)
			{
				return null;
			}
			return FUN_fdcErmittelBenutzerClaims(await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcBenutzerLadenAsync(i_strBenutzerName, i_strPasswort).ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerCodeAsync(string i_strCode)
		{
			if (m_edcBenutzerVerwaltungCapability.Value == null)
			{
				return null;
			}
			return FUN_fdcErmittelBenutzerClaims(await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcBenutzerAnhandCodeLadenAsync(i_strCode).ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerIdAsync(long i_i64BenutzerId)
		{
			if (m_edcBenutzerVerwaltungCapability.Value == null)
			{
				return null;
			}
			return FUN_fdcErmittelBenutzerClaims(await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcBenutzerLadenAsync(i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<ClaimsIdentity> FUN_fdcAutoAbmeldungIdentitaetLadenAsync()
		{
			return FUN_fdcErmittelBenutzerClaims(await FUN_fdcErmittleAutoAbmeldungBenutzerAsync().ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<ClaimsIdentity> FUN_fdcDefaultIdentitaetLadenAsync()
		{
			return FUN_fdcAnonymeIdentitaetErstellen("---", await FUN_fdcErmittleDefaultBenutzerIdAsync("---").ConfigureAwait(continueOnCapturedContext: true));
		}

		private static ClaimsIdentity FUN_fdcAnonymeIdentitaetErstellen(string i_strName, long i_i64BenutzerId)
		{
			return new ClaimsIdentity
			{
				Claims = 
				{
					new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Anonym"),
					new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", i_strName),
					new Claim("BenutzerId", i_i64BenutzerId.ToString(CultureInfo.InvariantCulture)),
					new Claim("BenutzerBarcode", string.Empty)
				}
			};
		}

		private static ClaimsIdentity FUN_fdcUserIdentitaetErstellen(string i_strName, IEnumerable<string> i_lstRechte, long i_i64BenutzerId, string i_strBarcode)
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", i_strName));
			claimsIdentity.Claims.Add(new Claim("BenutzerId", i_i64BenutzerId.ToString(CultureInfo.InvariantCulture)));
			claimsIdentity.Claims.Add(new Claim("BenutzerBarcode", string.IsNullOrEmpty(i_strBarcode) ? string.Empty : i_strBarcode));
			foreach (string item in i_lstRechte)
			{
				claimsIdentity.Claims.Add(new Claim("Recht", item));
			}
			return claimsIdentity;
		}

		private ClaimsIdentity FUN_fdcErmittelBenutzerClaims(EDC_BenutzerAbfrageData i_edcBenutzer)
		{
			if (i_edcBenutzer == null)
			{
				return null;
			}
			if (!i_edcBenutzer.FUN_blnIstAdministrator())
			{
				return FUN_fdcUserIdentitaetErstellen(i_edcBenutzer.PRO_strBenutzername, i_edcBenutzer.FUN_enuRechteErmitteln(), i_edcBenutzer.PRO_i64BenutzerId, i_edcBenutzer.PRO_strBarcode);
			}
			return EDC_AuthentifizierungsProviderHelfer.FUN_fdcAdministratorIdentitaetErstellen(i_edcBenutzer.PRO_strBenutzername, FUN_lstRechteBeschreibungenLaden(), i_edcBenutzer.PRO_i64BenutzerId);
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
			return (await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcDefaultBenutzerDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FirstOrDefault((EDC_BenutzerAbfrageData i_edcBenutzer) => i_edcBenutzer.PRO_strBenutzername == i_strName)?.PRO_i64BenutzerId ?? 0;
		}

		private async Task<EDC_BenutzerAbfrageData> FUN_fdcErmittleAutoAbmeldungBenutzerAsync()
		{
			if (m_edcBenutzerVerwaltungCapability.Value == null)
			{
				return null;
			}
			return (await m_edcBenutzerVerwaltungCapability.Value.FUN_fdcBenutzerDatenLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FirstOrDefault(delegate(EDC_BenutzerAbfrageData i_edcBenutzer)
			{
				if (i_edcBenutzer.PRO_blnIstAktiv && i_edcBenutzer.PRO_blnIstAktivNachAutoAbmeldung)
				{
					return !i_edcBenutzer.PRO_blnIstDefaultBenutzer;
				}
				return false;
			});
		}
	}
}
