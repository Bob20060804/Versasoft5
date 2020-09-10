using Ersa.Global.Common.Helper;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	public static class EDC_AuthentifizierungsProviderHelfer
	{
		public static ClaimsIdentity FUN_fdcAdministratorIdentitaetErstellen(string i_strName, IEnumerable<EDC_RechtBeschreibung> i_lstAlleRechtBeschreibungen, long i_i64BenutzerId)
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Administrator"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", i_strName));
			claimsIdentity.Claims.Add(new Claim("BenutzerId", i_i64BenutzerId.ToString(CultureInfo.InvariantCulture)));
			claimsIdentity.Claims.Add(new Claim("BenutzerBarcode", string.Empty));
			SUB_RechteClaimsHinzufuegen(i_lstAlleRechtBeschreibungen, claimsIdentity, (EDC_RechtBeschreibung i_edcRechteBeschreibung) => !i_edcRechteBeschreibung.PRO_blnNurFuerErsa);
			return claimsIdentity;
		}

		public static ClaimsIdentity FUN_fdcErsaIdentitaetErstellen(IEnumerable<EDC_RechtBeschreibung> i_lstAlleRechtBeschreibungen, long i_i64BenutzerId)
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			claimsIdentity.Claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "ersa"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "ERSA"));
			claimsIdentity.Claims.Add(new Claim("BenutzerId", i_i64BenutzerId.ToString(CultureInfo.InvariantCulture)));
			claimsIdentity.Claims.Add(new Claim("BenutzerBarcode", string.Empty));
			SUB_RechteClaimsHinzufuegen(i_lstAlleRechtBeschreibungen, claimsIdentity, null);
			return claimsIdentity;
		}

		public static bool FUN_blnIstErsaRolle(string i_strBenutzerName, string i_strPasswort, DateTime i_dtmVergleichsDatum)
		{
			if (i_strBenutzerName == "ersa" && EDC_HashHelfer.FUN_blnIstHashGueltig(i_strPasswort, "1757627144", "fuZHgnORVBWDpI/SLV4x2lNNqKY="))
			{
				return true;
			}
			DateTime now = DateTime.Now;
			if (now.AddHours(1.0) < i_dtmVergleichsDatum)
			{
				if (i_strBenutzerName == "ersa")
				{
					return EDC_TagesPasswortHelfer.FUN_blnIstLangesTagesPasswortGueltig(i_strPasswort, now, EDC_AuthentifizierungsKonstanten.msa_bytUniversalSchluessel);
				}
				return false;
			}
			if (i_strBenutzerName == "ersa")
			{
				return EDC_TagesPasswortHelfer.FUN_blnIstTagesPasswortGueltig(i_strPasswort, now, EDC_AuthentifizierungsKonstanten.msa_bytUniversalSchluessel);
			}
			return false;
		}

		public static bool FUN_blnIstErsaRolle(string i_strBenutzerName, string i_strPasswort, DateTime i_dtmVergleichsDatum, DateTime i_dtmVergleichsDatumHeute)
		{
			if (i_strBenutzerName == "ersa" && EDC_HashHelfer.FUN_blnIstHashGueltig(i_strPasswort, "1757627144", "fuZHgnORVBWDpI/SLV4x2lNNqKY="))
			{
				return true;
			}
			if (i_dtmVergleichsDatumHeute < i_dtmVergleichsDatum)
			{
				if (i_strBenutzerName == "ersa")
				{
					return EDC_TagesPasswortHelfer.FUN_blnIstLangesTagesPasswortGueltig(i_strPasswort, i_dtmVergleichsDatumHeute, EDC_AuthentifizierungsKonstanten.msa_bytUniversalSchluessel);
				}
				return false;
			}
			if (i_strBenutzerName == "ersa")
			{
				return EDC_TagesPasswortHelfer.FUN_blnIstTagesPasswortGueltig(i_strPasswort, i_dtmVergleichsDatumHeute, EDC_AuthentifizierungsKonstanten.msa_bytUniversalSchluessel);
			}
			return false;
		}

		private static void SUB_RechteClaimsHinzufuegen(IEnumerable<EDC_RechtBeschreibung> i_lstRechtBeschreibungen, IClaimsIdentity i_fdcClaimsIdentity, Func<EDC_RechtBeschreibung, bool> i_delFilterAusdruck)
		{
			foreach (EDC_RechtBeschreibung item in (i_delFilterAusdruck != null) ? i_lstRechtBeschreibungen.Where(i_delFilterAusdruck) : i_lstRechtBeschreibungen)
			{
				i_fdcClaimsIdentity.Claims.Add(new Claim("Recht", item.PRO_strRecht));
			}
		}
	}
}
