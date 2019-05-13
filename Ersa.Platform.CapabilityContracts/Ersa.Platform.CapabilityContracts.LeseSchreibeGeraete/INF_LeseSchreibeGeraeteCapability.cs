using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.LeseSchreibeGeraete
{
	public interface INF_LeseSchreibeGeraeteCapability
	{
		Task<IEnumerable<long>> FUN_fdcHoleLeseSchreibgeraeteKonfigurationIndizesAsync();

		Task FUN_fdcLeseSchreibgeraeteKonfigurationenSetzenAsync(IEnumerable<EDC_CodeKonfigData> i_lstCodeKonfigurationen);

		Task<bool> FUN_fdcIstAlbElbVerfuegbarAsync(long i_i64ArrayIndex);

		IObservable<bool> FUN_fdcAktiviereLesenTriggerErstellen(long i_i64ArrayIndex);

		Task FUN_fdcCodeGelesenSignalisierenAsync(EDC_CodeLeseErgebnis i_blnCodeLeseErgebnis);

		void SUB_LeseSchreibeGeraeteKonfigurationAnsichtVerlassen();
	}
}
