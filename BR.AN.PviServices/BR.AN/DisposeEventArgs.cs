using System;

namespace BR.AN
{
	public class DisposeEventArgs : EventArgs
	{
		private bool propDisposing;

		public bool Disposing => propDisposing;

		public DisposeEventArgs(bool disposing)
		{
			propDisposing = disposing;
		}
	}
}
