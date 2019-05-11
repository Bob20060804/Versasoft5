using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class AR000 : DeviceBase
	{
		private int propSourceAddress;

		private int propDestinationAddress;

		[PviKeyWord("/SA")]
		public int SourceAddress
		{
			get
			{
				return propSourceAddress;
			}
			set
			{
				propSourceAddress = value;
			}
		}

		[PviKeyWord("/DA")]
		public int DestinationAddress
		{
			get
			{
				return propDestinationAddress;
			}
			set
			{
				propDestinationAddress = value;
			}
		}

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				return base.DeviceParameterString + "/SA=" + propSourceAddress.ToString() + " /DA=" + propDestinationAddress.ToString() + " ";
			}
		}

		public AR000()
			: base(DeviceType.AR000)
		{
			propSourceAddress = 1;
			propDestinationAddress = 2;
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			num = base.ToXMLTextWriter(ref writer, flags);
			writer.WriteAttributeString("Channel", "SPWIN");
			if (propSourceAddress != 1)
			{
				writer.WriteAttributeString("Source", propSourceAddress.ToString());
			}
			if (propDestinationAddress != 2)
			{
				writer.WriteAttributeString("Destination", propDestinationAddress.ToString());
			}
			return num;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			AR000 aR = (AR000)baseObj;
			if (aR == null)
			{
				return -1;
			}
			string text = "";
			text = reader.GetAttribute("Source");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					aR.propSourceAddress = result;
				}
			}
			text = "";
			text = reader.GetAttribute("Destination");
			if (text != null && text.Length > 0)
			{
				int result2 = 0;
				if (PviParse.TryParseInt32(text, out result2))
				{
					aR.propDestinationAddress = result2;
				}
			}
			return 0;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateDeviceParameters(string parameters)
		{
			string[] array = parameters.Split(" ".ToCharArray());
			for (int i = 0; i < array.Length; i++)
			{
				string text = (string)array.GetValue(i);
				if (text.ToUpper().StartsWith("/IF=AR"))
				{
					propInterfaceName = text.Substring(4);
				}
				else if (!DeviceBase.UpdateParameterFromString("/SA=", text, ref propSourceAddress) && !DeviceBase.UpdateParameterFromString("/DA=", text, ref propDestinationAddress))
				{
					base.UpdateDeviceParameters(text);
				}
			}
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
