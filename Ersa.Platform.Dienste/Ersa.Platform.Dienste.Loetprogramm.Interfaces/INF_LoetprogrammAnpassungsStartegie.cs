using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_LoetprogrammAnpassungsStartegie<TLoetprogramm> where TLoetprogramm : class
	{
		Task FUN_fdcAnpassenAsync(TLoetprogramm i_edcLoetprogramm);
	}
}
