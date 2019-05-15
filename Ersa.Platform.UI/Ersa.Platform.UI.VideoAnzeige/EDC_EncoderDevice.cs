using Ersa.Global.Common;
using Microsoft.Expression.Encoder.Devices;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public class EDC_EncoderDevice : EDC_DisposableObject
	{
		public bool PRO_blnIstImFehlerZustand
		{
			get;
			set;
		}

		public EncoderDevice PRO_fdcEncoderDevice
		{
			get;
			set;
		}

		public virtual string PRO_strDeviceId
		{
			get
			{
				if (PRO_fdcEncoderDevice == null)
				{
					return null;
				}
				return PRO_fdcEncoderDevice.DevicePath;
			}
		}

		public int PRO_i32SekundaereDeviceId
		{
			get;
			set;
		}

		protected override void SUB_InternalDispose()
		{
			if (PRO_fdcEncoderDevice != null)
			{
				PRO_fdcEncoderDevice.Dispose();
			}
		}
	}
}
