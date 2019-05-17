using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public interface INF_EncoderApi
	{
		EDC_LiveJob FUN_edcLiveJobErstellen();

		IEnumerable<EDC_EncoderDevice> FUN_enuEncoderDevicesLaden();

		void SUB_SetzeVideoOutputFenster(EDC_LiveDeviceSource i_fdcDevice, HandleRef i_fdcHandle);

		void SUB_VideoOutputFensterZuruecksetzen(EDC_LiveDeviceSource i_fdcDevice);

		EDC_LiveDeviceSource FUN_edcVideoDeviceAktivieren(EDC_LiveJob i_edcJob, EDC_EncoderDevice i_edcEncoderDevice);

		void SUB_VideoDeviceEntfernen(EDC_LiveJob i_edcJob, EDC_LiveDeviceSource i_edcLiveDeviceSource);

		EDC_LiveDeviceSource FUN_edcLiveVideoDeviceSourceErmitteln(EDC_LiveJob i_edcJob, string i_strDevicePath);
	}
}
