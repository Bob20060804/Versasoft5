using Ersa.Platform.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces
{
	public interface INF_AuthentifizierungsDienst : IDisposable
	{
		string PRO_strAuthentifizierungsProvider
		{
			get;
		}

		int PRO_i32AbmeldungNach
		{
			get;
			set;
		}

		Task FUN_fdcProviderNameSetzenAsync(string i_strProviderName);

		Task<bool> FUN_fdcLoginAsync(string i_strBenutzerName, string i_strPasswort);

		Task<bool> FUN_fdcLoginMitCodeAsync(string i_strCode);

		Task<bool> FUN_fdcLoginMitBenutzerIdAsync(long i_i64BenutzerId);

		Task FUN_fdcLogoutAsync();

		Task FUN_fdcDefaultIdentitaetLadenAsync(ENUM_AnmeldeStatus i_enmAnmeldeStatus);

		Task<bool> FUN_fdcAutoAbmeldungIdentitaetLadenAsync();

		Task FUN_fdcInitialisiereAsync();

		void SUB_BenutzerWarAktivSignalisieren();
	}
}
