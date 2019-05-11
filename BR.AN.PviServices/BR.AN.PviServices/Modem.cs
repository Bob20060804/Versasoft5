using System;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class Modem : ModemBase
	{
		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			return base.ToXMLTextWriter(ref writer, flags, "", "");
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			return base.FromXmlTextReader(ref reader, flags, baseObj);
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
