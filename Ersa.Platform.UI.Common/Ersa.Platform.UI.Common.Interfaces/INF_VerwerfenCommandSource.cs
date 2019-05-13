using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_VerwerfenCommandSource : INF_BasisCommandSource
	{
		Task FUN_fdcAenderungenVerwerfenAsync();
	}
}
