using Ersa.Global.Dienste.Interfaces;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
	public class EDC_SimplifiedWcfContract<Tin, Tout> : INF_SimplifiedWcfContract
	{
		private readonly Func<Tin, Task<Tout>> m_delServerHandler;

		private readonly INF_SerialisierungsDienst m_edcSerialisierungsDienst;

		public EDC_SimplifiedWcfContract(INF_SerialisierungsDienst i_edcSerialisierungsDienst, Func<Tin, Task<Tout>> i_delServerHandler)
		{
			m_edcSerialisierungsDienst = i_edcSerialisierungsDienst;
			m_delServerHandler = i_delServerHandler;
		}

		public Task<string> FUN_fdcPingAsync()
		{
			return Task.FromResult("pong");
		}

		public async Task<string> FUN_fdcSendeDatenRequestAsync(string i_strDaten)
		{
			Tin arg = m_edcSerialisierungsDienst.FUN_objDeserialisieren<Tin>(i_strDaten);
			Tout i_objObjekt = await m_delServerHandler(arg).ConfigureAwait(continueOnCapturedContext: false);
			return m_edcSerialisierungsDienst.FUN_strSerialisieren(i_objObjekt);
		}
	}
}
