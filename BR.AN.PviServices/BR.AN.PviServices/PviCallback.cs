using System;

namespace BR.AN.PviServices
{
	internal delegate bool PviCallback(int wParam, int lParam, IntPtr data, uint dataLen, ref ResponseInfo info);
}
