using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.CapabilityContracts.IoT;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Extensions;
using Ersa.Platform.Common.IoT;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.IoT.Services.Edge;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.IoT.Edge
{
	public abstract class EDC_IoTDienst : INF_IoTDienst
	{
		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		private readonly Lazy<INF_MaschinenTelemetrieDatenCapability> m_edcMaschinenTelemetrieDatenCapability;

		private readonly INF_VisuSettingsDienst m_edcVisuSettingsDienst;

		private readonly INF_BenutzerInfoProvider m_edcBenutzerInfoProvider;

		private readonly Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst m_edcAppSettingsDienst;

		private readonly IEdgeToDeviceCommunicationClient m_edcEdgeKommunikationsClient;

		protected EDC_IoTDienst(INF_CapabilityProvider i_edcCapabilityProvider, INF_VisuSettingsDienst i_edcVisuSettingsDienst, Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst i_edcAppSettingsDienst, INF_BenutzerInfoProvider i_edcBenutzerInfoProvider, IEdgeToDeviceCommunicationClient i_edcEdgeKommunikationsClient)
		{
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			m_edcMaschinenTelemetrieDatenCapability = new Lazy<INF_MaschinenTelemetrieDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenTelemetrieDatenCapability>);
			m_edcVisuSettingsDienst = i_edcVisuSettingsDienst;
			m_edcAppSettingsDienst = i_edcAppSettingsDienst;
			m_edcBenutzerInfoProvider = i_edcBenutzerInfoProvider;
			m_edcEdgeKommunikationsClient = i_edcEdgeKommunikationsClient;
		}

		public bool FUN_blnIstCloudAnbindungFreigegeben()
		{
			if (!bool.TryParse(m_edcAppSettingsDienst.FUN_strAppSettingErmitteln("CloudServiceAvailable"), out bool result))
			{
				return false;
			}
			return result;
		}

		protected void SUB_MaschinenDatenHinzufügen(EdgeRequestData i_edcServiceRquestData)
		{
			i_edcServiceRquestData.VisuVersion = FUN_strHoleVisuVersion();
			i_edcServiceRquestData.VisuName = FUN_strHoleVisuName();
			i_edcServiceRquestData.FirmwareVersion = FUN_strHoleFirmwareVersion();
		}

		protected void SUB_DeviceDatenHizufuegen(EdgeRequestData i_edcServiceRquestData)
		{
			i_edcServiceRquestData.DeviceId = FUN_strMaschinenNummerErmitteln();
			i_edcServiceRquestData.IoTDeviceTypEnum = FUN_edcIotDeviceTypErmitteln();
		}

		protected async Task<bool> FUN_fdcTelemetrieDatenHinzufuegenAsync(EdgeRequestData i_edcServiceRquestData)
		{
			if (m_edcMaschinenTelemetrieDatenCapability.Value == null)
			{
				return false;
			}
			i_edcServiceRquestData.TelemetryDouble = await m_edcMaschinenTelemetrieDatenCapability.Value.FUN_fdcTelemtrieDoubleWerteLadenAsync().ConfigureAwait(continueOnCapturedContext: true);
			i_edcServiceRquestData.TelemetryString = await m_edcMaschinenTelemetrieDatenCapability.Value.FUN_fdcTelemtrieStringWerteLadenAsync().ConfigureAwait(continueOnCapturedContext: true);
			i_edcServiceRquestData.Messages = await m_edcMaschinenTelemetrieDatenCapability.Value.FUN_fdcTelemtrieMeldungenLadenAsync().ConfigureAwait(continueOnCapturedContext: true);
			i_edcServiceRquestData.OperatingModeEnum = (m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion() ? IoTDeviceOperatingMode.Automatic : IoTDeviceOperatingMode.SetupMode);
			return true;
		}

		protected async Task<EdgeResponseData> FUN_fdcRequestSendenAsync(EdgeRequestData i_edcServiceRequestData, bool i_blnVerbindungWiederBeenden = true)
		{
			bool blnVerbindungHergestellt = m_edcEdgeKommunikationsClient.IsConnected();
			try
			{
				if (!blnVerbindungHergestellt)
				{
					m_edcEdgeKommunikationsClient.Connect();
				}
				i_edcServiceRequestData.Timestamp = DateTime.UtcNow;
				return await m_edcEdgeKommunikationsClient.SendRequestAsync(i_edcServiceRequestData);
			}
			finally
			{
				if (!blnVerbindungHergestellt && i_blnVerbindungWiederBeenden)
				{
					m_edcEdgeKommunikationsClient.Disconnect();
				}
			}
		}

		protected void SUB_VerbindungBeenden()
		{
			if (m_edcEdgeKommunikationsClient.IsConnected())
			{
				m_edcEdgeKommunikationsClient.Disconnect();
			}
		}

		protected string FUN_strHoleFirmwareVersion()
		{
			if (m_edcMaschinenBasisDatenCapability.Value.FUN_enuMaschinenVersionenErmitteln().Any())
			{
				return m_edcMaschinenBasisDatenCapability.Value.FUN_enuMaschinenVersionenErmitteln().First().PRO_strVersion;
			}
			return string.Empty;
		}

		protected string FUN_strHoleVisuName()
		{
			string result = string.Empty;
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				result = entryAssembly.FUN_fdcAssemblyProduktErmitteln();
			}
			return result;
		}

		protected string FUN_strHoleVisuVersion()
		{
			string result = string.Empty;
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				result = entryAssembly.FUN_fdcAssemblyVersionStringErmitteln();
			}
			return result;
		}

		protected string FUN_strHoleAktivenBenutzerNamen()
		{
			return m_edcBenutzerInfoProvider.PRO_strAktiverBenutzer;
		}

		protected string FUN_strMaschinenNummerErmitteln()
		{
			return m_edcMaschinenBasisDatenCapability.Value.FUN_strMaschinenNummerErmitteln();
		}

		protected string FUN_strExportPfadPfadErmitteln()
		{
			return m_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadExport");
		}

		protected IoTDeviceTyp FUN_edcIotDeviceTypErmitteln()
		{
			return m_edcMaschinenBasisDatenCapability.Value.FUN_edcHoleIotDeviceTyp();
		}
	}
}
