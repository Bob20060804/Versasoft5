using Ersa.Platform.Common.Model;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public interface INF_ProzessSchreiberCapability
	{
		IList<EDC_PrimitivParameter> FUN_lstParameterFuerAufzeichnungErmitteln();

		ILookup<EDC_AdressRelevanterTeil, EDC_PrimitivParameter> FUN_fdcParameterGruppieren(IEnumerable<EDC_PrimitivParameter> i_enuParameter);
	}
}
