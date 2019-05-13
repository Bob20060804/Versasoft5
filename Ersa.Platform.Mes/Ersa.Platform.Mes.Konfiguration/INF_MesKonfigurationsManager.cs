using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.Konfiguration
{
    /// <summary>
    /// MES configuration manager
    /// </summary>
	public interface INF_MesKonfigurationsManager
	{
		IEnumerable<INF_MesKommunikationsDienst> PRO_enuKommunikationsDienste
		{
			get;
			set;
		}

		Task FUN_fdcInitialisierenAsync();

		Task<bool> FUN_fdcIstMesKonfiguriertAsync();

		Task<bool> FUN_fdcIstMesAktivAsync();

		Task FUN_fdcMesAktivSetzenAsync();

		Task FUN_fdcMesInaktivSetzenAsync();

		Task<ENUM_MesTyp> FUN_fdcHoleMesTypAsync();

		Task<EDC_MesKonfiguration> FUN_fdcHoleMesKonfigurationAsync();

		Task FUN_fdcLadeMesKonfigurationAsync();

		Task FUN_fdcSpeichereMesKonfigurationAsync(EDC_MesKonfiguration i_edcKonfiguration);

		Task<INF_MesKommunikationsDienst> FUN_fdcHoleKonfiguriertenKommunikationsDienstAsync();

		Task<INF_MesKommunikationsDienst> FUN_fdcHoleKommunikationsDienstAsync(ENUM_MesTyp i_enmMesTyp);
	}
}
