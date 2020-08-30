using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_TelemetrieDienst : INF_IoTDienst
	{
		Task<bool> FUN_fdcTelemetrieTransferStartenAsync();

		void SUB_TelemetrieTransferBeenden();

		bool FUN_blnIstTransferGestartet();
	}
}
