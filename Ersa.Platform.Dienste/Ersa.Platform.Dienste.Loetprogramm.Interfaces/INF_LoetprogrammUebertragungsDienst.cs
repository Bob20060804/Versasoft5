using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_LoetprogrammUebertragungsDienst<in TLoetprogramm>
	{
		Task<bool> FUN_blnLoetprogrammVollstaendigUebertragenAsync(TLoetprogramm i_edcLoetprogramm, CancellationToken i_fdcCancellationToken, params Func<Task>[] ia_delAktionenVorUebertragung);

		Task FUN_fdcLoetprogrammKopfdatenUebertragen(TLoetprogramm i_edcLoetprogramm, CancellationToken i_fdcCancellationToken);

		Task FUN_fdcLoetprogrammAdressenDeregistrierenAsync(TLoetprogramm i_edcLoetprogramm);
	}
}
