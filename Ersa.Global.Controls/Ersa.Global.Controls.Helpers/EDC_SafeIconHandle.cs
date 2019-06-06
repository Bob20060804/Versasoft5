using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public EDC_SafeIconHandle(IntPtr hIcon)
			: base(ownsHandle: true)
		{
			SetHandle(hIcon);
		}

		private EDC_SafeIconHandle()
			: base(ownsHandle: true)
		{
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DestroyIcon([In] IntPtr hIcon);

		protected override bool ReleaseHandle()
		{
			return DestroyIcon(handle);
		}
	}
}
