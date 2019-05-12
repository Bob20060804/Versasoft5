using System;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	public interface INF_SimplifiedWcfServer
	{
		void SUB_StarteKommunikationsDienst<Tin, Tout>(Binding i_fdcBinding, Uri i_fdcBasisAdresse, Func<Tin, Task<Tout>> i_delServerHandler, ENUM_SerialisationTyp i_enmSerializationTyp = ENUM_SerialisationTyp.Xml);

		bool FUN_blnBeendeKommunikationsDienst();
	}
}
