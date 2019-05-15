using Ersa.Global.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.VideoAnzeige
{
	[Export(typeof(INF_VideoDeviceVerwaltung))]
	public class EDC_VideoDeviceVerwaltung : EDC_DisposableObject, INF_VideoDeviceVerwaltung
	{
		private static readonly SemaphoreSlim m_fdcDeregistrierungsSemaphore = new SemaphoreSlim(1);

		private readonly EDC_LiveJob m_edcJob;

		private readonly IList<EDC_EncoderDevice> m_lstDevices;

		private readonly INF_EncoderApi m_edcEncoderApi;

		public event EventHandler<EDC_DeviceListeGeaendertArgs> m_evtDeviceListeGeaendert;

		[ImportingConstructor]
		public EDC_VideoDeviceVerwaltung(INF_EncoderApi i_edcEncoderApi)
		{
			m_edcEncoderApi = i_edcEncoderApi;
			m_edcJob = m_edcEncoderApi.FUN_edcLiveJobErstellen();
			m_lstDevices = new List<EDC_EncoderDevice>();
		}

		public bool FUN_blnVideoDeviceVorhanden(int i_i32DeviceIndex)
		{
			return FUN_blnVideoDeviceVorhanden(m_lstDevices, i_i32DeviceIndex);
		}

		public EDC_LiveDeviceSource FUN_fdcAktivesDeviceErmitteln(int i_i32DeviceIndex)
		{
			if (!FUN_blnVideoDeviceVorhanden(i_i32DeviceIndex))
			{
				return null;
			}
			EDC_EncoderDevice eDC_EncoderDevice = FUN_edcEncoderDeviceErmitteln(m_lstDevices, i_i32DeviceIndex);
			return m_edcEncoderApi.FUN_edcLiveVideoDeviceSourceErmitteln(m_edcJob, eDC_EncoderDevice.PRO_strDeviceId);
		}

		public EDC_LiveDeviceSource FUN_fdcDeviceAktivieren(int i_i32DeviceIndex)
		{
			if (!FUN_blnVideoDeviceVorhanden(i_i32DeviceIndex))
			{
				return null;
			}
			EDC_EncoderDevice eDC_EncoderDevice = FUN_edcEncoderDeviceErmitteln(m_lstDevices, i_i32DeviceIndex);
			if (!FUN_blnDeviceIdValide(eDC_EncoderDevice.PRO_strDeviceId))
			{
				return null;
			}
			return m_edcEncoderApi.FUN_edcVideoDeviceAktivieren(m_edcJob, eDC_EncoderDevice);
		}

		public void SUB_SetzeVideoOutputFenster(EDC_LiveDeviceSource i_fdcDevice, HandleRef i_fdcHandle)
		{
			m_edcEncoderApi.SUB_SetzeVideoOutputFenster(i_fdcDevice, i_fdcHandle);
		}

		public void SUB_VideoOutputFensterZuruecksetzen(EDC_LiveDeviceSource i_fdcDevice)
		{
			m_edcEncoderApi.SUB_VideoOutputFensterZuruecksetzen(i_fdcDevice);
		}

		public async Task FUN_fdcDeviceDeregistrierenAsync(int i_i32DeviceIndex)
		{
			EDC_EncoderDevice edcDevice = null;
			try
			{
				await m_fdcDeregistrierungsSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (FUN_blnVideoDeviceVorhanden(i_i32DeviceIndex))
				{
					edcDevice = FUN_edcEncoderDeviceErmitteln(m_lstDevices, i_i32DeviceIndex);
					EDC_LiveDeviceSource eDC_LiveDeviceSource = m_edcEncoderApi.FUN_edcLiveVideoDeviceSourceErmitteln(m_edcJob, edcDevice.PRO_strDeviceId);
					if (eDC_LiveDeviceSource != null)
					{
						m_edcEncoderApi.SUB_VideoDeviceEntfernen(m_edcJob, eDC_LiveDeviceSource);
					}
				}
			}
			finally
			{
				m_fdcDeregistrierungsSemaphore.Release();
				if (edcDevice != null)
				{
					m_lstDevices.Remove(edcDevice);
					SUB_DeviceListeGeaendertEventFeuern(ENUM_VideoDeviceAenderung.enmEntfernen, i_i32DeviceIndex);
					edcDevice.Dispose();
				}
			}
		}

		public async Task FUN_fdcDeviceRegistrierenAsync(int i_i32DeviceIndex)
		{
			try
			{
				await m_fdcDeregistrierungsSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<EDC_EncoderDevice> i_enuDevices = m_edcEncoderApi.FUN_enuEncoderDevicesLaden().ToList();
				if (!FUN_blnVideoDeviceVorhanden(i_enuDevices, i_i32DeviceIndex))
				{
					throw new InvalidOperationException($"No Video device found for the DeviceId {i_i32DeviceIndex}");
				}
				EDC_EncoderDevice edcDevice = FUN_edcEncoderDeviceErmitteln(i_enuDevices, i_i32DeviceIndex);
				if (!FUN_blnDeviceIdValide(edcDevice.PRO_strDeviceId))
				{
					throw new InvalidOperationException($"Invalid Device Address {edcDevice.PRO_strDeviceId} for the DeviceId {i_i32DeviceIndex}");
				}
				if (m_lstDevices.Any((EDC_EncoderDevice i_fdcDevice) => i_fdcDevice.PRO_strDeviceId == edcDevice.PRO_strDeviceId))
				{
					return;
				}
				m_lstDevices.Add(edcDevice);
			}
			catch (Exception)
			{
				EDC_EncoderDevice eDC_EncoderDevice = FUN_edcEncoderDeviceErmitteln(m_lstDevices, i_i32DeviceIndex);
				if (eDC_EncoderDevice == null)
				{
					m_lstDevices.Add(new EDC_EncoderDevice
					{
						PRO_blnIstImFehlerZustand = true,
						PRO_i32SekundaereDeviceId = i_i32DeviceIndex
					});
				}
				else
				{
					eDC_EncoderDevice.PRO_blnIstImFehlerZustand = true;
				}
			}
			finally
			{
				m_fdcDeregistrierungsSemaphore.Release();
			}
			SUB_DeviceListeGeaendertEventFeuern(ENUM_VideoDeviceAenderung.enmHinzufuegen, i_i32DeviceIndex);
		}

		public bool FUN_blnVideoDeviceImFehlerZustand(int i_i32DeviceIndex)
		{
			if (!FUN_blnVideoDeviceVorhanden(i_i32DeviceIndex))
			{
				return false;
			}
			return FUN_edcEncoderDeviceErmitteln(m_lstDevices, i_i32DeviceIndex).PRO_blnIstImFehlerZustand;
		}

		protected override void SUB_InternalDispose()
		{
			foreach (EDC_EncoderDevice lstDevice in m_lstDevices)
			{
				lstDevice.Dispose();
			}
			m_edcJob.Dispose();
		}

		private EDC_EncoderDevice FUN_edcEncoderDeviceErmitteln(IEnumerable<EDC_EncoderDevice> i_enuDevices, int i_i32DeviceIndex)
		{
			return i_enuDevices.SingleOrDefault((EDC_EncoderDevice i_edcDevice) => i_edcDevice.PRO_i32SekundaereDeviceId == i_i32DeviceIndex);
		}

		private bool FUN_blnVideoDeviceVorhanden(IEnumerable<EDC_EncoderDevice> i_enuDevices, int i_i32DeviceIndex)
		{
			return i_enuDevices.Any((EDC_EncoderDevice i_edcDevice) => i_edcDevice.PRO_i32SekundaereDeviceId == i_i32DeviceIndex);
		}

		private void SUB_DeviceListeGeaendertEventFeuern(ENUM_VideoDeviceAenderung i_enmAenderung, int i_i32DeviceIndex)
		{
			EventHandler<EDC_DeviceListeGeaendertArgs> evtDeviceListeGeaendert = this.m_evtDeviceListeGeaendert;
			EDC_DeviceListeGeaendertArgs e = new EDC_DeviceListeGeaendertArgs
			{
				PRO_enmDeviceAenderung = i_enmAenderung,
				PRO_i32DeviceIndex = i_i32DeviceIndex
			};
			evtDeviceListeGeaendert?.Invoke(this, e);
		}

		private bool FUN_blnDeviceIdValide(string i_strDeviceId)
		{
			return i_strDeviceId.StartsWith("\\");
		}
	}
}
