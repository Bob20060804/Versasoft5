using Ersa.Global.Common;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Microsoft.IdentityModel.Claims;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	[Export(typeof(INF_AuthentifizierungsDienst))]
	[Export(typeof(INF_BenutzerInfoProvider))]
	public class EDC_AuthentifizierungsDienst : EDC_DisposableObject, INF_AuthentifizierungsDienst, IDisposable, INF_BenutzerInfoProvider
	{
		[Obsolete("Das Fallback-Verhalten auf die app.config kann in einer späteren Version herausgenommen werden, wenn die Daten in die DB übernommen wurden.")]
		private const string mC_strAuthentifizierungsProviderAppSettingKey = "AuthentifizierungsProvider";

		[Obsolete("Das Fallback-Verhalten auf die app.config kann in einer späteren Version herausgenommen werden, wenn die Daten in die DB übernommen wurden.")]
		private const string mC_strAbmeldungNachInMinutenAppSettingKey = "AbmeldungNachInMinuten";

		private const int mC_i32AutomatischeAbmeldungVerzoegerungIntervallMs = 30000;

		private const int mC_i32AutomatischeAbmeldungTestIntervallMs = 30000;

		private readonly IEventAggregator m_edcEventAggregator;

		private readonly INF_SchedulerDienst m_edcSchedulerDienst;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private readonly INF_AppSettingsDienst m_edcAppSettingsDienst;

		private readonly INF_MaschinenEinstellungenDienst m_edcMaschinenEinstellungenDienst;

		private IDisposable m_fdcAbmeldenNachTimer;

		private DateTime m_dtmLetzterLogin = DateTime.MinValue;

		private DateTime m_dtmLetzteBenutzerAktion = DateTime.MinValue;

		[ImportMany]
		public IEnumerable<Lazy<INF_AuthentifizierungsProvider, INF_AuthentifizierungsProviderMetadaten>> PRO_fdcAuthentifizierungsProvider
		{
			get;
			set;
		}

		public string PRO_strAuthentifizierungsProvider
		{
			get;
			private set;
		}

		public int PRO_i32AbmeldungNach
		{
			get;
			set;
		}

		public string PRO_strAktiverBenutzer
		{
			get
			{
				Claim claim = FUN_fdcClaimErmitteln("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
				if (claim == null)
				{
					return string.Empty;
				}
				return claim.Value;
			}
		}

		public string PRO_strAktiveRolle
		{
			get
			{
				Claim claim = FUN_fdcClaimErmitteln("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
				if (claim == null)
				{
					return string.Empty;
				}
				return claim.Value;
			}
		}

		public string PRO_strAktiverBenutzerBarcode
		{
			get
			{
				Claim claim = FUN_fdcClaimErmitteln("BenutzerBarcode");
				if (claim == null)
				{
					return string.Empty;
				}
				return claim.Value;
			}
		}

		public long PRO_i64BenutzerId
		{
			get
			{
				Claim claim = FUN_fdcClaimErmitteln("BenutzerId");
				if (claim == null)
				{
					return 0L;
				}
				return long.Parse(claim.Value);
			}
		}

		static EDC_AuthentifizierungsDienst()
		{
			ClaimsPrincipal threadPrincipal = new ClaimsPrincipal
			{
				Identities = 
				{
					(IClaimsIdentity)new ClaimsIdentity()
				}
			};
			AppDomain.CurrentDomain.SetThreadPrincipal(threadPrincipal);
		}

		[ImportingConstructor]
		public EDC_AuthentifizierungsDienst(IEventAggregator i_fdcEventAggregator, INF_SchedulerDienst i_edcSchedulerDienst, INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_AppSettingsDienst i_edcAppSettingsDienst, INF_MaschinenEinstellungenDienst i_edcMaschinenEinstellungenDienst)
		{
			m_edcEventAggregator = i_fdcEventAggregator;
			m_edcSchedulerDienst = i_edcSchedulerDienst;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			m_edcAppSettingsDienst = i_edcAppSettingsDienst;
			m_edcMaschinenEinstellungenDienst = i_edcMaschinenEinstellungenDienst;
		}

		public async Task FUN_fdcInitialisiereAsync()
		{
			await FUN_fdcLadeEinstellungenAsync().ConfigureAwait(continueOnCapturedContext: true);
			m_fdcAbmeldenNachTimer = FUN_fdcBenutzerAbmeldung();
			await FUN_fdcKonfigurierteIdentitaetLadenAsync(ENUM_AnmeldeStatus.enmAbgemeldet).ConfigureAwait(continueOnCapturedContext: true);
		}

		public void SUB_BenutzerWarAktivSignalisieren()
		{
			m_dtmLetzteBenutzerAktion = DateTime.Now;
		}

		public async Task<bool> FUN_fdcLoginAsync(string i_strBenutzerName, string i_strPasswort)
		{
			m_edcAutorisierungsDienst.SUB_RechteInformationZuruecksetzen();
			return SUB_BenutzerAnederungPublizieren(await FUN_fdcBenutzerIdentitaetLadenAsync(i_strBenutzerName, i_strPasswort).ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<bool> FUN_fdcLoginMitCodeAsync(string i_strCode)
		{
			m_edcAutorisierungsDienst.SUB_RechteInformationZuruecksetzen();
			return SUB_BenutzerAnederungPublizieren(await FUN_fdcBenutzerIdentitaetMitCodeLadenAsync(i_strCode).ConfigureAwait(continueOnCapturedContext: true));
		}

		public async Task<bool> FUN_fdcLoginMitBenutzerIdAsync(long i_i64BenutzerId)
		{
			m_edcAutorisierungsDienst.SUB_RechteInformationZuruecksetzen();
			return SUB_BenutzerAnederungPublizieren(await FUN_fdcBenutzerIdentitaetMitBenutzerIdLadenAsync(i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: true));
		}

		public Task FUN_fdcLogoutAsync()
		{
			return FUN_fdcLogoutAsync(i_blnDurchAutoAbmeldung: false);
		}

		public async Task FUN_fdcProviderNameSetzenAsync(string i_strProviderName)
		{
			await FUN_fdcSpeichereEinstellungenAsync(i_strProviderName, PRO_i32AbmeldungNach).ConfigureAwait(continueOnCapturedContext: true);
			if (!(PRO_strAuthentifizierungsProvider == i_strProviderName))
			{
				PRO_strAuthentifizierungsProvider = i_strProviderName;
				await FUN_fdcLogoutAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		public async Task FUN_fdcKonfigurierteIdentitaetLadenAsync(ENUM_AnmeldeStatus i_enmDefaultAnmeldeStatus)
		{
			if (!(await FUN_fdcAutoAbmeldungIdentitaetLadenAsync().ConfigureAwait(continueOnCapturedContext: true)))
			{
				await FUN_fdcDefaultIdentitaetLadenAsync(i_enmDefaultAnmeldeStatus).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		public async Task FUN_fdcDefaultIdentitaetLadenAsync(ENUM_AnmeldeStatus i_enmAnmeldeStatus)
		{
			FUN_blnClaimPruefenUndVeroeffentlichen(await(await FUN_fdcAktivenProviderLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FUN_fdcDefaultIdentitaetLadenAsync().ConfigureAwait(continueOnCapturedContext: true), i_enmAnmeldeStatus);
		}

		public async Task<bool> FUN_fdcAutoAbmeldungIdentitaetLadenAsync()
		{
			return FUN_blnClaimPruefenUndVeroeffentlichen(await(await FUN_fdcAktivenProviderLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FUN_fdcAutoAbmeldungIdentitaetLadenAsync().ConfigureAwait(continueOnCapturedContext: true), ENUM_AnmeldeStatus.enmAngemeldet);
		}

		protected override void SUB_InternalDispose()
		{
			SUB_FinalLogout();
			m_fdcAbmeldenNachTimer?.Dispose();
		}

		private async Task FUN_fdcLadeEinstellungenAsync()
		{
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = await m_edcMaschinenEinstellungenDienst.FUN_fdcHoleBenutzerverwaltungEinstellungenAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (eDC_MaschinenEinstellungenData == null)
			{
				string text = m_edcAppSettingsDienst.FUN_strAppSettingErmitteln("AuthentifizierungsProvider");
				PRO_strAuthentifizierungsProvider = (string.IsNullOrEmpty(text) ? "Default" : text);
				string text2 = m_edcAppSettingsDienst.FUN_strAppSettingErmitteln("AbmeldungNachInMinuten");
				if (!string.IsNullOrEmpty(text2) && int.TryParse(text2, out int result))
				{
					PRO_i32AbmeldungNach = result;
				}
				await FUN_fdcSpeichereEinstellungenAsync(PRO_strAuthentifizierungsProvider, PRO_i32AbmeldungNach).ConfigureAwait(continueOnCapturedContext: true);
			}
			else
			{
				PRO_strAuthentifizierungsProvider = (string.IsNullOrEmpty(eDC_MaschinenEinstellungenData.PRO_strTextWert) ? "Default" : eDC_MaschinenEinstellungenData.PRO_strTextWert);
				PRO_i32AbmeldungNach = (int)eDC_MaschinenEinstellungenData.PRO_i64LongWert;
			}
		}

		private Task FUN_fdcSpeichereEinstellungenAsync(string i_strProviderName, int i_i32AbmeldeZeit)
		{
			return m_edcMaschinenEinstellungenDienst.FUN_fdcSpeichereBenutzerverwaltungEinstellungenAsync(i_strProviderName, i_i32AbmeldeZeit);
		}

		private bool SUB_BenutzerAnederungPublizieren(ClaimsIdentity i_fdcIdentity)
		{
			if (i_fdcIdentity != null)
			{
				SUB_ClaimsIdentityFuerCurrentPrincipalSetzen(i_fdcIdentity);
				m_dtmLetzterLogin = DateTime.Now;
				m_edcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Publish(FUN_edcBenutzerGeaendertEventPayloadErstellen(ENUM_AnmeldeStatus.enmAngemeldet));
				return true;
			}
			return false;
		}

		private bool FUN_blnClaimPruefenUndVeroeffentlichen(ClaimsIdentity i_fdcNeuesClaim, ENUM_AnmeldeStatus i_enmAnmeldeStatus)
		{
			if (i_fdcNeuesClaim == null)
			{
				return false;
			}
			Claim claim = i_fdcNeuesClaim.Claims.SingleOrDefault((Claim i_fdcClaim) => i_fdcClaim.ClaimType == "BenutzerId");
			long num = (claim != null) ? long.Parse(claim.Value) : 0;
			if (PRO_i64BenutzerId == num)
			{
				return true;
			}
			SUB_ClaimsIdentityFuerCurrentPrincipalSetzen(i_fdcNeuesClaim);
			m_edcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Publish(FUN_edcBenutzerGeaendertEventPayloadErstellen(i_enmAnmeldeStatus));
			return true;
		}

		private async Task<ClaimsIdentity> FUN_fdcBenutzerIdentitaetLadenAsync(string i_strBenutzerName, string i_strPasswort)
		{
			return await(await FUN_fdcAktivenProviderLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FUN_fdcPruefeUndLadeClaimsFuerBenutzerAsync(i_strBenutzerName, i_strPasswort).ConfigureAwait(continueOnCapturedContext: true);
		}

		private async Task<ClaimsIdentity> FUN_fdcBenutzerIdentitaetMitCodeLadenAsync(string i_strCode)
		{
			return await(await FUN_fdcAktivenProviderLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FUN_fdcLadeClaimsFuerBenutzerCodeAsync(i_strCode).ConfigureAwait(continueOnCapturedContext: true);
		}

		private async Task<ClaimsIdentity> FUN_fdcBenutzerIdentitaetMitBenutzerIdLadenAsync(long i_i64BenutzerId)
		{
			return await(await FUN_fdcAktivenProviderLadenAsync().ConfigureAwait(continueOnCapturedContext: true)).FUN_fdcLadeClaimsFuerBenutzerIdAsync(i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: true);
		}

		private async Task<INF_AuthentifizierungsProvider> FUN_fdcAktivenProviderLadenAsync()
		{
			if (string.IsNullOrEmpty(PRO_strAuthentifizierungsProvider))
			{
				await FUN_fdcLadeEinstellungenAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
			return PRO_fdcAuthentifizierungsProvider.First((Lazy<INF_AuthentifizierungsProvider, INF_AuthentifizierungsProviderMetadaten> i_fdcProvider) => i_fdcProvider.Metadata.PRO_strAuthentifizierungsProvider == PRO_strAuthentifizierungsProvider).Value;
		}

		private EDC_BenutzerGeaendertEventPayload FUN_edcBenutzerGeaendertEventPayloadErstellen(ENUM_AnmeldeStatus i_enmStatus)
		{
			return new EDC_BenutzerGeaendertEventPayload
			{
				PRO_blnBenutzerverwaltungAktiv = (PRO_strAuthentifizierungsProvider == "LegacyProvider"),
				PRO_strAktiverBenutzer = PRO_strAktiverBenutzer,
				PRO_i64BenutzerId = PRO_i64BenutzerId,
				PRO_strAktiveRolle = PRO_strAktiveRolle,
				PRO_strAktiverBarcode = PRO_strAktiverBenutzerBarcode,
				PRO_enmAnmeldeStatus = i_enmStatus
			};
		}

		private Claim FUN_fdcClaimErmitteln(string i_strClaimTyp)
		{
			ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
			if (claimsPrincipal == null)
			{
				return null;
			}
			return ((IClaimsIdentity)claimsPrincipal.Identity).Claims.SingleOrDefault((Claim i_fdcClaim) => i_fdcClaim.ClaimType == i_strClaimTyp);
		}

		private void SUB_ClaimsIdentityFuerCurrentPrincipalSetzen(IClaimsIdentity i_fdcClaimsIdentity)
		{
			ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
			if (claimsPrincipal != null)
			{
				claimsPrincipal.Identities.Clear();
				claimsPrincipal.Identities.Add(i_fdcClaimsIdentity);
			}
		}

		private IDisposable FUN_fdcBenutzerAbmeldung()
		{
			Action i_delAktion = delegate
			{
				EDC_Dispatch.SUB_AktionStarten(async delegate
				{
					await FUN_fdcBehandleAbmeldungNachAsync().ConfigureAwait(continueOnCapturedContext: true);
					await FUN_fdcBehandleErsaAbmeldungAsync().ConfigureAwait(continueOnCapturedContext: true);
				});
			};
			return m_edcSchedulerDienst.FUN_fdcAufgabeTerminieren(i_delAktion, 30000, 30000);
		}

		private async Task FUN_fdcBehandleAbmeldungNachAsync()
		{
			bool flag = PRO_strAuthentifizierungsProvider != "Default";
			if (PRO_i32AbmeldungNach > 0 && flag && DateTime.Now.Subtract(m_dtmLetzteBenutzerAktion).TotalMinutes >= (double)PRO_i32AbmeldungNach)
			{
				await FUN_fdcLogoutAsync(i_blnDurchAutoAbmeldung: true).ConfigureAwait(continueOnCapturedContext: true);
				SUB_IntervalberechnungNeuStarten();
			}
		}

		private async Task FUN_fdcLogoutAsync(bool i_blnDurchAutoAbmeldung)
		{
			m_edcAutorisierungsDienst.SUB_RechteInformationZuruecksetzen();
			m_dtmLetzterLogin = DateTime.MinValue;
			if (i_blnDurchAutoAbmeldung)
			{
				await FUN_fdcKonfigurierteIdentitaetLadenAsync(ENUM_AnmeldeStatus.enmAbgemeldet).ConfigureAwait(continueOnCapturedContext: true);
			}
			else
			{
				await FUN_fdcDefaultIdentitaetLadenAsync(ENUM_AnmeldeStatus.enmAbgemeldet).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private void SUB_IntervalberechnungNeuStarten()
		{
			SUB_BenutzerWarAktivSignalisieren();
		}

		private async Task FUN_fdcBehandleErsaAbmeldungAsync()
		{
			if (PRO_strAktiveRolle == "ERSA" && DateTime.Now.Day != m_dtmLetzterLogin.Day)
			{
				await FUN_fdcLogoutAsync(i_blnDurchAutoAbmeldung: true).ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private void SUB_FinalLogout()
		{
			m_dtmLetzterLogin = DateTime.MinValue;
			SUB_ClaimsIdentityFuerCurrentPrincipalSetzen(new ClaimsIdentity());
			m_edcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Publish(FUN_edcBenutzerGeaendertEventPayloadErstellen(ENUM_AnmeldeStatus.enmAbgemeldet));
		}
	}
}
