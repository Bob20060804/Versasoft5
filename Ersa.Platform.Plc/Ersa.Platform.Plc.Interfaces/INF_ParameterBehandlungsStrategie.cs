using Ersa.Platform.Common.Model;

namespace Ersa.Platform.Plc.Interfaces
{
	public interface INF_ParameterBehandlungsStrategie<out T>
	{
		T FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter);

		T FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter);

		T FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter);
	}
}
