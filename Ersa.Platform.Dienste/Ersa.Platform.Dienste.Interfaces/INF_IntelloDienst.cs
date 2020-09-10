using Ersa.Platform.Common.IoT;
using System;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_IntelloDienst : INF_IoTDienst
	{
		Task<bool> FUN_fdcServicefallErstellenAsync(EdgeRequestData i_edcServiceRequestData);

		Task<bool> FUN_fdcServicefallFuerExceptionErstellenAsync(Exception i_fdcEx);
	}
}
