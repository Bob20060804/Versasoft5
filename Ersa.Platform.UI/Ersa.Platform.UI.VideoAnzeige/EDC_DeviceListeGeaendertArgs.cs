using System;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public class EDC_DeviceListeGeaendertArgs : EventArgs
	{
		public ENUM_VideoDeviceAenderung PRO_enmDeviceAenderung
		{
			get;
			set;
		}

		public int PRO_i32DeviceIndex
		{
			get;
			set;
		}
	}
}
