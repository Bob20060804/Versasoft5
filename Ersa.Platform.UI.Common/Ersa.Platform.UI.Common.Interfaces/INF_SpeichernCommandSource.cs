using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_SpeichernCommandSource : INF_BasisCommandSource
	{
		Task FUN_fdcAenderungenSpeichernAsync();
	}
}
