using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.LeseSchreibeGeraete
{
	public interface INF_LsgRuestenCapability
	{
		IObservable<EDC_AuftragRuestkontrolle> FUN_fdcLesenTriggerErstellen(long i_i64ArrayIndex);

		void SUB_TestLesenTriggern(ENUM_RuestWerkzeug i_enmWerkzeug);

		Task FUN_fdcRuestdatenGelesenSignalisierenAsync(EDC_RuestErgebnis i_edcRuestErgebnis);

		IEnumerable<IGrouping<string, EDC_WerkzeugElement>> FUN_enuErmittleWerkzeuge();

		Task FUN_fdcAktiveWerkzeugeSpeichernAsync(params ENUM_RuestWerkzeug[] ia_enmAktiveWerkzeuge);
	}
}
