using Ersa.Global.Dienste.Interfaces;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	[Export(typeof(INF_SimplifiedWcfClient))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_SimplifiedWcfClient : INF_SimplifiedWcfClient
	{
		private const int mC_i32MaxVerbindungsversuche = 5;

		private const int mC_i32ZeitZwischenVerbindungsversuche = 1000;

		private ChannelFactory<INF_SimplifiedWcfContract> m_fdcFactory;

		private INF_SimplifiedWcfContract m_edcClientContract;

		public INF_SerialisierungsDienst PRO_edcSerialisierungsDienst
		{
			get;
			set;
		}

		[Import("Ersa.XmlSerialisierer", typeof(INF_SerialisierungsDienst))]
		public INF_SerialisierungsDienst PRO_edcXmlSerialisierungsDienst
		{
			get;
			set;
		}

		[Import("Ersa.JsonSerialisierer", typeof(INF_SerialisierungsDienst))]
		public INF_SerialisierungsDienst PRO_edcJsonSerialisierungsDienst
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_SimplifiedWcfClient()
		{
		}

		public void SUB_VerbindungHerstellen(Binding i_fdcBinding, EndpointAddress i_fdcEndpunkt, ENUM_SerialisationTyp i_enmSerializationTyp = ENUM_SerialisationTyp.Xml)
		{
			if (ENUM_SerialisationTyp.Xml.Equals(i_enmSerializationTyp) && PRO_edcXmlSerialisierungsDienst != null)
			{
				PRO_edcSerialisierungsDienst = PRO_edcXmlSerialisierungsDienst;
			}
			if (ENUM_SerialisationTyp.Json.Equals(i_enmSerializationTyp) && PRO_edcJsonSerialisierungsDienst != null)
			{
				PRO_edcSerialisierungsDienst = PRO_edcJsonSerialisierungsDienst;
			}
			FUN_blnVerbindungBeenden();
			m_fdcFactory = new ChannelFactory<INF_SimplifiedWcfContract>(i_fdcBinding, i_fdcEndpunkt);
			m_edcClientContract = m_fdcFactory.CreateChannel();
		}

		public bool FUN_blnVerbindungBeenden()
		{
			if (m_edcClientContract == null)
			{
				return true;
			}
			try
			{
				IChannel channel = (IChannel)m_edcClientContract;
				if (channel.State == CommunicationState.Opened)
				{
					channel.Close();
				}
				else
				{
					channel.Abort();
				}
				return true;
			}
			finally
			{
				m_edcClientContract = null;
				m_fdcFactory = null;
			}
		}

		public async Task<bool> FUN_fdcPingAsync()
		{
			SUB_CheckClient();
			return "pong".Equals(await m_edcClientContract.FUN_fdcPingAsync().ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<Tout> FUN_fdcSendeDatenRequestAsync<Tin, Tout>(Tin i_edcDatenobjekt)
		{
			SUB_CheckClient();
			string i_strDaten = PRO_edcSerialisierungsDienst.FUN_strSerialisieren(i_edcDatenobjekt);
			string i_strFormatierterString = await m_edcClientContract.FUN_fdcSendeDatenRequestAsync(i_strDaten).ConfigureAwait(continueOnCapturedContext: false);
			return PRO_edcSerialisierungsDienst.FUN_objDeserialisieren<Tout>(i_strFormatierterString);
		}

		public bool FUN_blnIstClientInitialisiert()
		{
			if (m_fdcFactory == null || m_edcClientContract == null)
			{
				return false;
			}
			IChannel channel = (IChannel)m_edcClientContract;
			if (!CommunicationState.Opened.Equals(channel.State) && !CommunicationState.Opening.Equals(channel.State) && !CommunicationState.Created.Equals(channel.State))
			{
				return false;
			}
			return true;
		}

		private void SUB_CheckClient()
		{
			if (m_fdcFactory == null)
			{
				throw new FaultException("Client not initialized");
			}
			if (m_edcClientContract == null)
			{
				m_edcClientContract = m_fdcFactory.CreateChannel();
			}
			IChannel channel = (IChannel)m_edcClientContract;
			for (int i = 0; i < 5; i++)
			{
				if (channel.State != CommunicationState.Faulted)
				{
					break;
				}
				channel.Abort();
				m_edcClientContract = m_fdcFactory.CreateChannel();
				if (channel.State == CommunicationState.Faulted)
				{
					Task.Delay(1000).Wait(1000);
				}
			}
			if (channel.State == CommunicationState.Faulted)
			{
				throw new FaultException("Client cannot connect");
			}
		}
	}
}
