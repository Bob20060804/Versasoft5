using Ersa.Global.Common.Helper;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Helfer;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Extensions
{
	public static class EDC_BenutzerDataExtensions
	{
		public static void SUB_PasswortSetzen(this EDC_BenutzerAbfrageData i_edcBenutzer, string i_strPasswort)
		{
			i_edcBenutzer.PRO_strPasswortSalt = EDC_HashHelfer.FUN_strZufaelligenSaltErzeugen();
			i_edcBenutzer.PRO_strPasswortHash = EDC_HashHelfer.FUN_strHashErzeugen(i_strPasswort, i_edcBenutzer.PRO_strPasswortSalt);
		}

		public static bool FUN_blnLoginDatenKorrekt(this EDC_BenutzerAbfrageData i_edcBenutzer, string i_strPasswort)
		{
			return EDC_HashHelfer.FUN_blnIstHashGueltig(i_strPasswort, i_edcBenutzer.PRO_strPasswortSalt, i_edcBenutzer.PRO_strPasswortHash);
		}

		public static bool FUN_blnLoginDatenKorrekt(this EDC_BenutzerData i_edcBenutzer, string i_strPasswort)
		{
			return EDC_HashHelfer.FUN_blnIstHashGueltig(i_strPasswort, i_edcBenutzer.PRO_strPasswortSalt, i_edcBenutzer.PRO_strPasswortHash);
		}

		public static bool FUN_blnIstAdministrator(this EDC_BenutzerAbfrageData i_edcBenutzer)
		{
			return (i_edcBenutzer.PRO_i32Rechte & 1) == 1;
		}

		public static IEnumerable<string> FUN_enuRechteErmitteln(this EDC_BenutzerAbfrageData i_edcBenutzer)
		{
			return EDC_RechteHelfer.FUN_enuLegacyRechteKonvertieren(i_edcBenutzer.PRO_i32Rechte);
		}

		public static void SUB_RechteSetzen(this EDC_BenutzerAbfrageData i_edcBenutzer, bool i_blnIstAdministrator, IEnumerable<string> i_enuRechte)
		{
			i_edcBenutzer.PRO_i32Rechte = EDC_RechteHelfer.FUN_i32NeueRechteKonvertieren(i_blnIstAdministrator, i_enuRechte);
		}

		public static EDC_BenutzerData FUN_edcConvertToBenutzerData(this EDC_BenutzerAbfrageData i_edcBenutzer)
		{
			return new EDC_BenutzerData
			{
				PRO_i64BenutzerId = i_edcBenutzer.PRO_i64BenutzerId,
				PRO_blnIstDefaultBenutzer = i_edcBenutzer.PRO_blnIstDefaultBenutzer,
				PRO_dtmAngelegtAm = i_edcBenutzer.PRO_dtmAngelegtAm,
				PRO_strBenutzername = i_edcBenutzer.PRO_strBenutzername,
				PRO_strPasswortHash = i_edcBenutzer.PRO_strPasswortHash,
				PRO_strPasswortSalt = i_edcBenutzer.PRO_strPasswortSalt,
				PRO_blnIstExternerBenutzer = i_edcBenutzer.PRO_blnIstExternerBenutzer,
				PRO_strBarcode = i_edcBenutzer.PRO_strBarcode
			};
		}

		public static EDC_BenutzerMappingData FUN_edcConvertToMappingData(this EDC_BenutzerAbfrageData i_edcBenutzer)
		{
			return new EDC_BenutzerMappingData
			{
				PRO_i64BenutzerId = i_edcBenutzer.PRO_i64BenutzerId,
				PRO_i32Rechte = i_edcBenutzer.PRO_i32Rechte,
				PRO_i64MaschinenId = i_edcBenutzer.PRO_i64MaschinenId,
				PRO_blnIstAktivNachAutoAbmeldung = i_edcBenutzer.PRO_blnIstAktivNachAutoAbmeldung,
				PRO_blnIstAktiv = i_edcBenutzer.PRO_blnIstAktiv
			};
		}
	}
}
