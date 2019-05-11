using System;

namespace BR.AN
{
	public class LicComponets : IDisposable
	{
		internal bool propDisposed;

		private string propOrderID;

		private string propLicenseText;

		private bool propRequiresBRIPC;

		public string OrderID => propOrderID;

		public string LicenseText => propLicenseText;

		public bool RequiresBRIPC => propRequiresBRIPC;

		public event DisposeEventHandler Disposing;

		public LicComponets()
		{
			Initialize("", "", requiresBRIPC: false);
		}

		internal LicComponets(string orderID, string licText, bool requiresBRIPC)
		{
			Initialize(orderID, licText, requiresBRIPC);
		}

		private void Initialize(string orderID, string licText, bool requiresBRIPC)
		{
			propDisposed = false;
			propOrderID = orderID;
			propLicenseText = licText;
			propRequiresBRIPC = requiresBRIPC;
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
					propLicenseText = null;
					propOrderID = null;
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
	}
}
