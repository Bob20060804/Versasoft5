using System;

namespace BR.AN.PviServices
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
