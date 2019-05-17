using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;

namespace Ersa.Platform.UI.VideoAnzeige
{
	[Export(typeof(INF_EncoderApi))]
	public class EDC_EncoderApi : INF_EncoderApi
	{
		public EDC_LiveJob FUN_edcLiveJobErstellen()
		{
			return new EDC_LiveJob
			{
				PRO_fdcLiveJob = new LiveJob()
			};
		}

		public IEnumerable<EDC_EncoderDevice> FUN_enuEncoderDevicesLaden()
		{
			return EncoderDevices.FindDevices(EncoderDeviceType.Video).Select((EncoderDevice i_fdcDevice, int i_i32DeviceIndex) => new EDC_EncoderDevice
			{
				PRO_fdcEncoderDevice = i_fdcDevice,
				PRO_i32SekundaereDeviceId = i_i32DeviceIndex
			});
		}

		public void SUB_SetzeVideoOutputFenster(EDC_LiveDeviceSource i_fdcDevice, HandleRef i_fdcHandle)
		{
			SUB_VideoOutputFensterZuruecksetzen(i_fdcDevice);
			i_fdcDevice.PRO_fdcLiveDeviceSource.PreviewWindow = new PreviewWindow(i_fdcHandle);
		}

		public void SUB_VideoOutputFensterZuruecksetzen(EDC_LiveDeviceSource i_fdcDevice)
		{
			if (i_fdcDevice.PRO_fdcLiveDeviceSource.PreviewWindow != null)
			{
				i_fdcDevice.PRO_fdcLiveDeviceSource.PreviewWindow.Dispose();
			}
		}

		public EDC_LiveDeviceSource FUN_edcVideoDeviceAktivieren(EDC_LiveJob i_edcJob, EDC_EncoderDevice i_edcEncoderDevice)
		{
			LiveDeviceSource pRO_fdcLiveDeviceSource = i_edcJob.PRO_fdcLiveJob.AddDeviceSource(i_edcEncoderDevice.PRO_fdcEncoderDevice, null);
			return new EDC_LiveDeviceSource
			{
				PRO_fdcLiveDeviceSource = pRO_fdcLiveDeviceSource
			};
		}

		public void SUB_VideoDeviceEntfernen(EDC_LiveJob i_edcJob, EDC_LiveDeviceSource i_edcLiveDeviceSource)
		{
			if (i_edcLiveDeviceSource.PRO_fdcLiveDeviceSource != null)
			{
				i_edcJob.PRO_fdcLiveJob.RemoveDeviceSource(i_edcLiveDeviceSource.PRO_fdcLiveDeviceSource);
			}
		}

		public EDC_LiveDeviceSource FUN_edcLiveVideoDeviceSourceErmitteln(EDC_LiveJob i_edcJob, string i_strDevicePath)
		{
			LiveDeviceSource liveDeviceSource = i_edcJob.PRO_fdcLiveJob.DeviceSources.SingleOrDefault((LiveDeviceSource i_fdcSource) => i_fdcSource.VideoDevice.DevicePath == i_strDevicePath);
			if (liveDeviceSource != null)
			{
				return new EDC_LiveDeviceSource
				{
					PRO_fdcLiveDeviceSource = liveDeviceSource
				};
			}
			return null;
		}
	}
}
