using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public abstract class ModemBase : DeviceBase
	{
		private string propModem;

		private int propCommPort;

		private string propPhoneNumber;

		private int propRedial;

		private int propRedialTimeout;

		[PviKeyWord("/MO")]
		public string Modem
		{
			get
			{
				return propModem;
			}
			set
			{
				propModem = value;
			}
		}

		[PviKeyWord("/IF")]
		public int CommunicationPort
		{
			get
			{
				return propCommPort;
			}
			set
			{
				propCommPort = value;
				propInterfaceName = "modem" + value.ToString();
			}
		}

		[PviKeyWord("/TN")]
		public string PhoneNumber
		{
			get
			{
				return propPhoneNumber;
			}
			set
			{
				propPhoneNumber = value;
			}
		}

		[PviKeyWord("/MR")]
		public int Redial
		{
			get
			{
				return propRedial;
			}
			set
			{
				propRedial = value;
			}
		}

		[PviKeyWord("/RI")]
		public int RedialTimeout
		{
			get
			{
				return propRedialTimeout;
			}
			set
			{
				propRedialTimeout = value;
			}
		}

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				return base.DeviceParameterString + "/MO=" + Modem + " /TN=" + PhoneNumber + " /MR=" + Redial.ToString() + " /RI=" + RedialTimeout.ToString() + " /IT=" + base.IntervalTimeout.ToString() + " ";
			}
		}

		public ModemBase()
			: base(DeviceType.Modem)
		{
			propModem = "MicroLink 56k";
			propCommPort = 1;
			propPhoneNumber = "";
			propRedialTimeout = 60;
			propIntervalTimeout = 40;
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags, string attributeName, string attributeValue)
		{
			int result = base.ToXMLTextWriter(ref writer, flags);
			if (propCommPort != 1)
			{
				writer.WriteAttributeString("Channel", propCommPort.ToString());
			}
			if (propModem != "MicroLink 56k")
			{
				writer.WriteAttributeString("Modem", propModem);
			}
			if (propPhoneNumber != null && propPhoneNumber.Length > 0)
			{
				writer.WriteAttributeString("PhoneNumber", propPhoneNumber);
			}
			if (propRedialTimeout != 60)
			{
				writer.WriteAttributeString("RedialTimeout", propRedialTimeout.ToString());
			}
			if (propRedial != 0)
			{
				writer.WriteAttributeString("Redial", propRedial.ToString());
			}
			if (attributeName != null && attributeName.Length > 0 && attributeValue != null && attributeValue.Length > 0)
			{
				writer.WriteAttributeString(attributeName, attributeValue);
			}
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			ModemBase modemBase = (ModemBase)baseObj;
			if (modemBase == null)
			{
				return -1;
			}
			int result = 0;
			string text = "";
			text = reader.GetAttribute("Channel");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				modemBase.propCommPort = result;
			}
			text = reader.GetAttribute("Modem");
			if (text != null && text.Length > 0)
			{
				modemBase.propModem = text;
			}
			text = reader.GetAttribute("PhoneNumber");
			if (text != null && text.Length > 0)
			{
				modemBase.propPhoneNumber = text;
			}
			text = reader.GetAttribute("RedialTimeout");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				modemBase.propRedialTimeout = result;
			}
			text = reader.GetAttribute("Redial");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				modemBase.propRedial = result;
			}
			return 0;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateDeviceParameters(string parameters)
		{
			bool flag = false;
			propKnownDeviceParameters = "";
			string text = parameters.Replace(" /", "\t");
			string[] array = text.Split("\t".ToCharArray());
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text2 = (string)array.GetValue(i);
				if (DeviceBase.UpdateParameterFromString("/IF=", text2, ref propInterfaceName))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("IT=", text2, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("MO=", text2, ref propModem))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("TN=", text2, ref propPhoneNumber))
				{
					flag = true;
				}
				else if (text2.ToUpper().StartsWith("MR="))
				{
					if (text2.Substring(3).IndexOf("INFINITE") == 0)
					{
						propRedial = -1;
					}
					else
					{
						propRedial = Convert.ToInt32(text2.Substring(3));
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("RI=", text2, ref propRedialTimeout))
				{
					flag = true;
				}
				else
				{
					base.UpdateDeviceParameters(text2);
				}
				if (flag)
				{
					string text3 = text2;
					if (text2.IndexOf("/") != 0)
					{
						text3 = "/" + text2;
					}
					if (propKnownDeviceParameters.Length == 0)
					{
						propKnownDeviceParameters = text3.Trim();
					}
					else
					{
						propKnownDeviceParameters = propKnownDeviceParameters + " " + text3.Trim();
					}
				}
			}
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
