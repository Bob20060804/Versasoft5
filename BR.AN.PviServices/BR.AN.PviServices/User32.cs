using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	internal class User32
	{
		private const uint INFINITE = uint.MaxValue;

		private const uint WAIT_ABANDONED = 128u;

		private const uint WAIT_OBJECT_0 = 0u;

		private const uint WAIT_TIMEOUT = 258u;

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern uint WaitForSingleObject(SafeWaitHandle hHandle, uint dwMilliseconds);
	}
}
