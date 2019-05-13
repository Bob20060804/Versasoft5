using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung
{
	public interface INF_MaschinenDatenSichernCapability
	{
		long FUN_i64MaschinenKonfigurationsVersionErmitteln();

		IEnumerable<EDC_ParameterDaten> FUN_enuZuSicherndeMaschinenParameterErmitteln();

		IEnumerable<EDC_ParameterDaten> FUN_enuZuSicherndeKonfigParameterErmitteln();

		IEnumerable<Func<Task>> FUN_enuMaschinenDateienSichernOperationenErstellen(string i_strPfad);
	}
}
