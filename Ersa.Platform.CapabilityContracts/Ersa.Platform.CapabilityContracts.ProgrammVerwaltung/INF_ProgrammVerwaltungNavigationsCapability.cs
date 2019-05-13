using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public interface INF_ProgrammVerwaltungNavigationsCapability
	{
		Task FUN_fdcZuProgrammVerwaltungNavigierenAsync(string i_strBibliotheksName = null);
	}
}
