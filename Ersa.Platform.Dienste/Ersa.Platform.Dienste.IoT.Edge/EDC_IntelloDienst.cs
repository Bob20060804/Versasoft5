using Ersa.Global.Common.Extensions;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.IoT;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.IoT.Services.Edge;
using Ersa.Platform.Logging;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.IoT.Edge
{
	[Export(typeof(INF_IntelloDienst))]
	public class EDC_IntelloDienst : EDC_IoTDienst, INF_IntelloDienst, INF_IoTDienst
	{
		private const string mC_strZipErweiterung = "zip";

		private const int mC_i32MaxArchivierungsGroesse = 10;

		private readonly INF_BetriebsfallImportExportDienst m_edcBetriebsfallImportExport;

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_IntelloDienst(INF_CapabilityProvider i_edcCapabilityProvider, INF_VisuSettingsDienst i_edcVisuSettingsDienst, Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst i_edcAppSettingsDienst, INF_BenutzerInfoProvider i_edcBenutzerInfoProvider, INF_BetriebsfallImportExportDienst i_edcBetriebsfallImportExport, IEdgeToDeviceCommunicationClient i_edcEdgeKommunikationsClient)
			: base(i_edcCapabilityProvider, i_edcVisuSettingsDienst, i_edcAppSettingsDienst, i_edcBenutzerInfoProvider, i_edcEdgeKommunikationsClient)
		{
			m_edcBetriebsfallImportExport = i_edcBetriebsfallImportExport;
		}

		public Task<bool> FUN_fdcServicefallErstellenAsync(EdgeRequestData i_edcServiceRequestData)
		{
			return FUN_fdcIntelloRequestVersendenAsync(i_edcServiceRequestData);
		}

		public Task<bool> FUN_fdcServicefallFuerExceptionErstellenAsync(Exception i_fdcEx)
		{
			EdgeRequestData i_edcServiceRquestData = new EdgeRequestData
			{
				ClassificationEnum = IntelloClassification.VisuSoftware,
				Note = i_fdcEx.Message,
				UserName = FUN_strHoleAktivenBenutzerNamen()
			};
			return FUN_fdcIntelloRequestVersendenAsync(i_edcServiceRquestData);
		}

		private async Task<bool> FUN_fdcIntelloRequestVersendenAsync(EdgeRequestData i_edcServiceRquestData)
		{
			i_edcServiceRquestData.IoTRequestTypeEnum = IoTRequestType.Intello;
			SUB_MaschinenDatenHinzufügen(i_edcServiceRquestData);
			SUB_DeviceDatenHizufuegen(i_edcServiceRquestData);
			if (!(await FUN_fdcBetriebsfallExportierenUndDatenAnfuegen(i_edcServiceRquestData)))
			{
				return false;
			}
			EdgeResponseData edgeResponseData = await FUN_fdcRequestSendenAsync(i_edcServiceRquestData);
			if (edgeResponseData.ResponseState != "ok")
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, edgeResponseData.ErrorMessage);
				return false;
			}
			return true;
		}

		private async Task<bool> FUN_fdcBetriebsfallExportierenUndDatenAnfuegen(EdgeRequestData i_edcServiceRquestData)
		{
			string text = FUN_strDefaultBetriebsfallExportPfadErmitteln();
			string strDefaultDateiName = string.Format("{0}{1}{2}_{3}.{4}", text, Path.DirectorySeparatorChar, FUN_strMaschinenNummerErmitteln(), DateTime.Now.FUN_strToDatumStringFuerDefaultDateiNamen(), "zip");
			try
			{
				await m_edcBetriebsfallImportExport.FUN_fdcExportierenAsync(strDefaultDateiName, 10240).ConfigureAwait(continueOnCapturedContext: true);
				i_edcServiceRquestData.SetAttachmentFile(strDefaultDateiName);
				return true;
			}
			catch (Exception i_excException)
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Error exporting machine state", null, null, null, i_excException);
				return false;
			}
		}

		private string FUN_strDefaultBetriebsfallExportPfadErmitteln()
		{
			string text = Path.Combine(FUN_strExportPfadPfadErmitteln(), "MachineState");
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(text);
			}
			return text;
		}
	}
}
