using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	public interface INF_SimplifiedWcfClient
	{
		void SUB_VerbindungHerstellen(Binding i_fdcBinding, EndpointAddress i_fdcEndpunkt, ENUM_SerialisationTyp i_enmSerializationTyp = ENUM_SerialisationTyp.Xml);

		bool FUN_blnIstClientInitialisiert();

		bool FUN_blnVerbindungBeenden();

		Task<bool> FUN_fdcPingAsync();

		Task<Tout> FUN_fdcSendeDatenRequestAsync<Tin, Tout>(Tin i_edcDatenobjekt);
	}
}
