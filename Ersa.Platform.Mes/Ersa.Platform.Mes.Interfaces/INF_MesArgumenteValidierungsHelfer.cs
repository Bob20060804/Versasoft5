using Ersa.Platform.Common.Mes;

namespace Ersa.Platform.Mes.Interfaces
{
	/// <summary>
	/// MES参数验证帮助
	/// </summary>
	public interface INF_MesArgumenteValidierungsHelfer
	{
		void SUB_PruefeArgumente(ENUM_MesFunktionen i_enmFunktion, EDC_MesMaschinenDaten i_edcMesMaschinenDaten);
	}
}
