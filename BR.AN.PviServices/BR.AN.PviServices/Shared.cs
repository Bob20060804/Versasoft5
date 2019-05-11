using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class Shared : DeviceBase
	{
		private byte propChannel;

		[PviKeyWord("/IF")]
		public byte Channel
		{
			get
			{
				return propChannel;
			}
			set
			{
				propChannel = value;
				propInterfaceName = "LS251_" + value.ToString();
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = SaveConnectionAttributes(ref writer);
			if (propChannel != 1)
			{
				writer.WriteAttributeString("Channel", propChannel.ToString());
			}
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			Shared shared = (Shared)baseObj;
			if (shared == null)
			{
				return -1;
			}
			string text = "";
			text = reader.GetAttribute("Channel");
			if (text != null && text.Length > 0)
			{
				byte result = 0;
				if (PviParse.TryParseByte(text, out result))
				{
					shared.propChannel = result;
				}
			}
			return 0;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override void UpdateDeviceParameters(string parameters)
		{
			string[] array = parameters.Split(" ".ToCharArray());
			bool flag = false;
			propKnownDeviceParameters = "";
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (text.ToUpper().StartsWith("/IF=LS251_"))
				{
					propInterfaceName = text.Substring(4);
					propChannel = Convert.ToByte(text.Substring(10));
					flag = true;
				}
				else
				{
					base.UpdateDeviceParameters(text);
				}
				if (flag)
				{
					string text2 = text;
					if (text.IndexOf("/") != 0)
					{
						text2 = "/" + text;
					}
					if (propKnownDeviceParameters.Length == 0)
					{
						propKnownDeviceParameters = text2.Trim();
					}
					else
					{
						propKnownDeviceParameters = propKnownDeviceParameters + " " + text2.Trim();
					}
				}
			}
		}

		public Shared()
			: base(DeviceType.Shared)
		{
			propChannel = 1;
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
