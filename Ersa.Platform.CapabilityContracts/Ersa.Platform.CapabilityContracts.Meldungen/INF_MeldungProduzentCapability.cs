using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Meldungen
{
	public interface INF_MeldungProduzentCapability
	{
		ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
		}

		bool FUN_blnIstKonfiguriert();

		Task FUN_fdcMeldungBehandelnAnfordernAsync(INF_Meldung i_edcMeldung, ENUM_MeldungAktionen i_enmAktion);

		Task<IEnumerable<INF_Meldung>> FUN_fdcErmittleZuQuittierendeMeldungenAsync(IEnumerable<INF_Meldung> i_enuNichQuittierteMeldungen);
	}
}
