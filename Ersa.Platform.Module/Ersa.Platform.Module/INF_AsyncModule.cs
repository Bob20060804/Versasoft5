using Prism.Modularity;
using System.Threading.Tasks;

namespace Ersa.Platform.Module
{
	public interface INF_AsyncModule : IModule
	{
		Task PRO_fdcInitialisierung
		{
			get;
		}
	}
}
