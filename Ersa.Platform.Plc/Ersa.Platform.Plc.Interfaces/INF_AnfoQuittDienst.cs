using Ersa.Platform.Common.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.Interfaces
{
	public interface INF_AnfoQuittDienst
	{
		Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync(EDC_ToggleWert i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken));

		Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync(EDC_ToggleZustandWert i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken));
	}
}
