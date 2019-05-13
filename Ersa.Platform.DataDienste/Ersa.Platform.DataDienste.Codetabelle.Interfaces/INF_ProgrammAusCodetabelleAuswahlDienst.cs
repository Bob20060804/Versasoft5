using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle.Interfaces
{
	public interface INF_ProgrammAusCodetabelleAuswahlDienst
	{
		Task<IEnumerable<long>> FUN_fdcErmittleProgrammIdsFuerCodeAsync(string i_strSuchCode);
	}
}
