using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class ModemNET2000 : ModemBase
	{
		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			return base.ToXMLTextWriter(ref writer, flags, "LINE", "LNNET2000");
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			return base.FromXmlTextReader(ref reader, flags, baseObj);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateDeviceParameters(string parameters)
		{
			base.UpdateDeviceParameters(parameters);
		}
	}
}
