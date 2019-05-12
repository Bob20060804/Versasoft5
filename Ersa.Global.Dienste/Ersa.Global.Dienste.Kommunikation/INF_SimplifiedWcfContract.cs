using System.ServiceModel;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	[ServiceContract]
	public interface INF_SimplifiedWcfContract
	{
		[OperationContract]
		Task<string> FUN_fdcPingAsync();

		[OperationContract]
		Task<string> FUN_fdcSendeDatenRequestAsync(string i_strDaten);
	}
}
