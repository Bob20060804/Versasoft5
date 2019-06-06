using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Resources;

namespace Ersa.Global.Controls.Helpers
{
	public static class EDC_CursorHelfer
	{
		private struct EDC_IconInfo
		{
			public bool m_blnIsIcon;

			public int m_i32XHotspot;

			public int m_i32YHotspot;

			public IntPtr m_fdcHbmMask;

			public IntPtr m_fdcHbmColor;
		}

		public static Cursor FUN_fdcCursorErstellen(Bitmap i_fdcBmp, int i_i32XHotSpot, int i_i32YHotSpot)
		{
			EDC_IconInfo i_sttIconInfo = default(EDC_IconInfo);
			GetIconInfo(i_fdcBmp.GetHicon(), ref i_sttIconInfo);
			i_sttIconInfo.m_i32XHotspot = i_i32XHotSpot;
			i_sttIconInfo.m_i32YHotspot = i_i32YHotSpot;
			i_sttIconInfo.m_blnIsIcon = false;
			return CursorInteropHelper.Create(new EDC_SafeIconHandle(CreateIconIndirect(ref i_sttIconInfo)));
		}

		public static Cursor FUN_fdcCursorErstellen(Uri i_fdcBildUri, int i_i32XHotSpot, int i_i32YHotSpot)
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(i_fdcBildUri);
			if (resourceStream == null)
			{
				return null;
			}
			return FUN_fdcCursorErstellen(new Bitmap(resourceStream.Stream), i_i32XHotSpot, i_i32YHotSpot);
		}

		[DllImport("user32.dll")]
		private static extern IntPtr CreateIconIndirect(ref EDC_IconInfo i_sttIconInfo);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetIconInfo(IntPtr i_fdcIcon, ref EDC_IconInfo i_sttIconInfo);
	}
}
