using Microsoft.IdentityModel.Claims;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces
{
	public interface INF_AuthentifizierungsProvider
	{
		Task<ClaimsIdentity> FUN_fdcPruefeUndLadeClaimsFuerBenutzerAsync(string i_strBenutzerName, string i_strPasswort);

		Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerCodeAsync(string i_strCode);

		Task<ClaimsIdentity> FUN_fdcLadeClaimsFuerBenutzerIdAsync(long i_i64BenutzerId);

		Task<ClaimsIdentity> FUN_fdcDefaultIdentitaetLadenAsync();

		Task<ClaimsIdentity> FUN_fdcAutoAbmeldungIdentitaetLadenAsync();
	}
}
