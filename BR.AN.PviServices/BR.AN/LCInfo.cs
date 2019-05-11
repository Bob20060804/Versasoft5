using System;
using System.Collections;

namespace BR.AN
{
	public class LCInfo : IDisposable
	{
		internal bool propDisposed;

		private string propSerialNumber;

		private string propLCName;

		private string propLCInfo;

		private string propLCPort;

		private ArrayList propActivations;

		public string SerialNumber => propSerialNumber;

		public string Name => propLCName;

		public string Info => propLCInfo;

		public string Port => propLCPort;

		public ArrayList Activations => propActivations;

		public event DisposeEventHandler Disposing;

		public LCInfo()
		{
			Initialize("", "", "", "", new ArrayList(1));
		}

		internal LCInfo(string name, string port, string info, string serial, ArrayList activations)
		{
			Initialize(name, port, info, serial, activations);
		}

		private void Initialize(string name, string port, string info, string serial, ArrayList activations)
		{
			propDisposed = false;
			propLCName = name;
			propLCPort = port;
			propLCInfo = info;
			propSerialNumber = serial;
			propActivations = activations;
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
					propActivations.Clear();
					propActivations = null;
					propLCInfo = null;
					propLCName = null;
					propLCPort = null;
					propSerialNumber = null;
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
