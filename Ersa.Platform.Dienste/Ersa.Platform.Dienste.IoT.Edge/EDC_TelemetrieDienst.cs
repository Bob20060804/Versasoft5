using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.IoT;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.IoT.Services.Edge;
using Ersa.Platform.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace Ersa.Platform.Dienste.IoT.Edge
{
	[Export(typeof(INF_TelemetrieDienst))]
	public class EDC_TelemetrieDienst : EDC_IoTDienst, INF_TelemetrieDienst, INF_IoTDienst
	{
		private const int mC_i32MaxTransferFehlerAnzahl = 8;

		private readonly IEventAggregator m_fdcEventAggregator;

		private Timer m_fdcTimer;

		private int m_i32TransferFehlerCounter;

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_TelemetrieDienst(INF_CapabilityProvider i_edcCapabilityProvider, INF_VisuSettingsDienst i_edcVisuSettingsDienst, Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst i_edcAppSettingsDienst, INF_BenutzerInfoProvider i_edcBenutzerInfoProvider, IEdgeToDeviceCommunicationClient i_edcEdgeKommunikationsClient, IEventAggregator i_fdcEventAggregator)
			: base(i_edcCapabilityProvider, i_edcVisuSettingsDienst, i_edcAppSettingsDienst, i_edcBenutzerInfoProvider, i_edcEdgeKommunikationsClient)
		{
			m_fdcEventAggregator = i_fdcEventAggregator;
		}

		public async Task<bool> FUN_fdcTelemetrieTransferStartenAsync()
		{
			if (!FUN_blnIstCloudAnbindungFreigegeben())
			{
				return false;
			}
			if (m_fdcTimer != null)
			{
				return true;
			}
			int num = await FUN_fdcLeseTransferIntervallAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (num < 0)
			{
				return false;
			}
			m_fdcTimer = new Timer(num * 1000)
			{
				AutoReset = true
			};
			m_fdcTimer.Elapsed += async delegate(object s, ElapsedEventArgs e)
			{
				await FUN_fdcTelemetriedatenRunnerAsync(s, e).ConfigureAwait(continueOnCapturedContext: false);
			};
			m_fdcTimer.Start();
			m_i32TransferFehlerCounter = 0;
			m_fdcEventAggregator.GetEvent<EDC_TelemetrieGeaendertEvent>().Publish(ENUM_TelemetryZustand.Gestartet);
			return true;
		}

		public void SUB_TelemetrieTransferBeenden()
		{
			SUB_VerbindungBeenden();
			if (m_fdcTimer != null)
			{
				m_fdcTimer.Stop();
				m_fdcTimer.Dispose();
				m_fdcTimer = null;
				m_fdcEventAggregator.GetEvent<EDC_TelemetrieGeaendertEvent>().Publish(ENUM_TelemetryZustand.Beendet);
			}
		}

		public bool FUN_blnIstTransferGestartet()
		{
			return m_fdcTimer != null;
		}

		private async Task<int> FUN_fdcLeseTransferIntervallAsync()
		{
			EdgeRequestData edgeRequestData = new EdgeRequestData
			{
				IoTRequestTypeEnum = IoTRequestType.Settings,
				UserName = FUN_strHoleAktivenBenutzerNamen(),
				TelemetryDouble = new Dictionary<string, double>(),
				TelemetryString = new Dictionary<string, string>(),
				Messages = new Dictionary<int, List<string>>()
			};
			SUB_DeviceDatenHizufuegen(edgeRequestData);
			try
			{
				EdgeResponseData edgeResponseData = await FUN_fdcRequestSendenAsync(edgeRequestData);
				if (edgeResponseData.ResponseState != "ok")
				{
					PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, edgeResponseData.ErrorMessage);
					return -1;
				}
				if (edgeResponseData.Parameter.TryGetValue("TelemetryIntervall", out object value))
				{
					return Convert.ToInt32(value);
				}
			}
			catch (Exception ex)
			{
				m_i32TransferFehlerCounter++;
				Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, ex.Message, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, ex);
			}
			return -1;
		}

		private async Task FUN_fdcTelemetriedatenRunnerAsync(object i_objSource, ElapsedEventArgs i_fdcArgs)
		{
			try
			{
				EdgeRequestData edcRequest = new EdgeRequestData
				{
					IoTRequestTypeEnum = IoTRequestType.Telemetry,
					UserName = FUN_strHoleAktivenBenutzerNamen(),
					TelemetryDouble = new Dictionary<string, double>(),
					TelemetryString = new Dictionary<string, string>(),
					Messages = new Dictionary<int, List<string>>()
				};
				await FUN_fdcTelemetrieDatenHinzufuegenAsync(edcRequest).ConfigureAwait(continueOnCapturedContext: true);
				SUB_DeviceDatenHizufuegen(edcRequest);
				EdgeResponseData edgeResponseData = await FUN_fdcRequestSendenAsync(edcRequest);
				if (edgeResponseData.ResponseState != "ok")
				{
					PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, edgeResponseData.ErrorMessage);
				}
				m_i32TransferFehlerCounter = 0;
			}
			catch (Exception ex)
			{
				m_i32TransferFehlerCounter++;
				Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, ex.Message, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, ex);
			}
			if (m_i32TransferFehlerCounter >= 8)
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "The telemetry service was terminated after too many transmission errors");
				SUB_TelemetrieTransferBeenden();
				m_fdcEventAggregator.GetEvent<EDC_TelemetrieGeaendertEvent>().Publish(ENUM_TelemetryZustand.Fehler);
			}
		}
	}
}
