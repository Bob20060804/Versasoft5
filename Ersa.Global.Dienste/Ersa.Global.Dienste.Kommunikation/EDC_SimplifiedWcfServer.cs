using Ersa.Global.Dienste.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste.Kommunikation
{
	[Export(typeof(INF_SimplifiedWcfServer))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_SimplifiedWcfServer : INF_SimplifiedWcfServer
	{
		private ServiceHost m_fdcServiceHost;

		private INF_SimplifiedWcfContract m_edcContract;

		[Import("Ersa.XmlSerialisierer", typeof(INF_SerialisierungsDienst))]
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
		public EDC_SimplifiedWcfServer()
		{
		}

		public void SUB_StarteKommunikationsDienst<Tin, Tout>(Binding i_fdcBinding, Uri i_fdcBasisAdresse, Func<Tin, Task<Tout>> i_delServerHandler, ENUM_SerialisationTyp i_enmSerializationTyp = ENUM_SerialisationTyp.Xml)
		{
			if (m_fdcServiceHost != null)
			{
				FUN_blnBeendeKommunikationsDienst();
			}
			if (ENUM_SerialisationTyp.Xml.Equals(i_enmSerializationTyp) && PRO_edcXmlSerialisierungsDienst != null)
			{
				PRO_edcSerialisierungsDienst = PRO_edcXmlSerialisierungsDienst;
			}
			if (ENUM_SerialisationTyp.Json.Equals(i_enmSerializationTyp) && PRO_edcJsonSerialisierungsDienst != null)
			{
				PRO_edcSerialisierungsDienst = PRO_edcJsonSerialisierungsDienst;
			}
			m_edcContract = new EDC_SimplifiedWcfContract<Tin, Tout>(PRO_edcSerialisierungsDienst, i_delServerHandler);
			m_fdcServiceHost = new ServiceHost(m_edcContract, i_fdcBasisAdresse);
			m_fdcServiceHost.AddServiceEndpoint(typeof(INF_SimplifiedWcfContract), i_fdcBinding, i_fdcBasisAdresse);
			if (m_fdcServiceHost.State == CommunicationState.Created && m_fdcServiceHost.State != CommunicationState.Opened)
			{
				m_fdcServiceHost.Open();
			}
		}

		public bool FUN_blnBeendeKommunikationsDienst()
		{
			if (m_fdcServiceHost == null || m_edcContract == null)
			{
				return true;
			}
			try
			{
				m_fdcServiceHost.Close();
				return true;
			}
			finally
			{
				m_fdcServiceHost = null;
				m_edcContract = null;
			}
		}
	}
}
