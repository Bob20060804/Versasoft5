using Ersa.Platform.Common.Loetprotokoll;
using Ersa.Platform.Common.Model;
using Ersa.Platform.DataContracts.Loetprotokoll;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public interface INF_LoetprotokollCapability
	{
		void SUB_AufzeichnungStarten();

		void SUB_AufzeichnungStoppen();

		IList<EDC_PrimitivParameter> FUN_lstParameterErmitteln();

		IList<EDC_PrimitivParameter> FUN_lstParameterVomTypErmitteln(ENUM_ProtokollParameterTyp i_enmParameterTyp);

		ILookup<EDC_AdressRelevanterTeil, EDC_PrimitivParameter> FUN_fdcParameterGruppieren(IEnumerable<EDC_PrimitivParameter> i_enuParameter);

		INF_LoetParameterIstSollConverter FUN_edcHoleLoetParameterIstSollwertConverter();
	}
}
