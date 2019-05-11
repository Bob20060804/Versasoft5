using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BR.AN.PviServices
{
	public class LicenceInfo : IDisposable
	{
		private Service propService;

		private byte[] porpLCName;

		internal bool propDisposed;

		private string propLicenseName;

		private string propInfo;

		private RuntimeStates propRuntimeState;

		public string LicenceName => propLicenseName;

		public string Info => propInfo;

		public RuntimeStates RuntimeState => propRuntimeState;

		public event DisposeEventHandler Disposing;

		public LicenceInfo(Service service)
		{
			propDisposed = false;
			porpLCName = new byte[73];
			propService = service;
			propLicenseName = "NO LICENCE";
			propInfo = "";
			propRuntimeState = RuntimeStates.Undefined;
		}

		public void Dispose()
		{
			if (!propDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
					porpLCName = null;
					propInfo = null;
					propLicenseName = null;
					propService = null;
				}
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public int Update(int responseInfo)
		{
			return propService.UpdateLicenceInfo();
		}

		internal void UpdateRuntimeState(IntPtr pRuntimeInfo)
		{
			int num = 8;
			propLicenseName = "";
			byte b = 0;
			if (IntPtr.Zero == pRuntimeInfo)
			{
				propLicenseName = "NO LICENCE";
				propRuntimeState = RuntimeStates.Undefined;
				return;
			}
			for (int i = 0; i < porpLCName.Length; i++)
			{
				porpLCName[i] = 0;
			}
			b = Marshal.ReadByte(pRuntimeInfo);
			Marshal.Copy(pRuntimeInfo, porpLCName, 0, 73);
			for (int i = 8; i < porpLCName.Length; i++)
			{
				if (porpLCName[i] == 0)
				{
					num = i;
					break;
				}
			}
			propLicenseName = Encoding.ASCII.GetString(porpLCName, 8, num - 8);
			switch (b)
			{
			case 1:
				propRuntimeState = RuntimeStates.Trial;
				break;
			case 2:
				propRuntimeState = RuntimeStates.Runtime;
				break;
			case 4:
				propRuntimeState = RuntimeStates.Locked;
				break;
			default:
				propRuntimeState = RuntimeStates.Undefined;
				break;
			}
		}

		public override string ToString()
		{
			return propRuntimeState.ToString() + ";" + LicenceName;
		}
	}
}
