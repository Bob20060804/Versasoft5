using Ersa.Platform.Common.Model;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts
{
	public interface INF_DiagnosticsCapability
	{
		IEnumerable<EDC_PrimitivParameter> FUN_enuMaschinenParameterErmitteln();

		IEnumerable<EDC_PrimitivParameter> FUN_enuKonfigParameterErmitteln();

		IEnumerable<EDC_PrimitivParameter> FUN_enuLoetprogrammParameterErmitteln();
	}
}
