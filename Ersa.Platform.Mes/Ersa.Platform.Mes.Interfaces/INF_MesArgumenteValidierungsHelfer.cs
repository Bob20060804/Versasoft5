using Ersa.Platform.Common.Mes;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesArgumenteValidierungsHelfer
	{
		void SUB_PruefeArgumente(ENUM_MesFunktionen i_enmFunktion, EDC_MesMaschinenDaten i_edcMesMaschinenDaten);
	}
}
