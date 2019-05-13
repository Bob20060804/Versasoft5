using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Ruestkontrolle
{
	public interface INF_RuestkontrolleCapability
	{
		Task FUN_fdcRuestkontrollePipelineStartenAsync(long i_i64ArrayIndex);

		Task FUN_fdcRuestkontrollePipelineBeendenAsync(long i_i64ArrayIndex);

		Task<bool> FUN_fdcIstRuestkontrollePipelineAktivAsync(long i_i64ArrayIndex);
	}
}
