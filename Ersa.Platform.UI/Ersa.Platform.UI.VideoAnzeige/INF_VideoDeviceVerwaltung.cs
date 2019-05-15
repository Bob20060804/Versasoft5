using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public interface INF_VideoDeviceVerwaltung
	{
		event EventHandler<EDC_DeviceListeGeaendertArgs> m_evtDeviceListeGeaendert;

		bool FUN_blnVideoDeviceVorhanden(int i_i32DeviceIndex);

		EDC_LiveDeviceSource FUN_fdcAktivesDeviceErmitteln(int i_i32DeviceIndex);

		EDC_LiveDeviceSource FUN_fdcDeviceAktivieren(int i_i32DeviceIndex);

		void SUB_SetzeVideoOutputFenster(EDC_LiveDeviceSource i_edcDevice, HandleRef i_fdcHandle);

		void SUB_VideoOutputFensterZuruecksetzen(EDC_LiveDeviceSource i_fdcDevice);

		Task FUN_fdcDeviceDeregistrierenAsync(int i_i32DeviceIndex);

		Task FUN_fdcDeviceRegistrierenAsync(int i_i32DeviceIndex);

		bool FUN_blnVideoDeviceImFehlerZustand(int i_i32DeviceIndex);
	}
}
