using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class Serial : DeviceBase
	{
		private FlowControls propFlowControl;

		private byte channel;

		private Parity parity;

		private int baudrate;

		[PviKeyWord("/IF")]
		public byte Channel
		{
			get
			{
				return channel;
			}
			set
			{
				channel = value;
				propInterfaceName = "COM" + value.ToString();
			}
		}

		[PviKeyWord("/BD")]
		public int BaudRate
		{
			get
			{
				return baudrate;
			}
			set
			{
				baudrate = value;
			}
		}

		[PviKeyWord("/PA")]
		public Parity Parity
		{
			get
			{
				return parity;
			}
			set
			{
				parity = value;
			}
		}

		[PviKeyWord("/RS")]
		public FlowControls FlowControl
		{
			get
			{
				return propFlowControl;
			}
			set
			{
				propFlowControl = value;
			}
		}

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				if (FlowControls.NOT_SET != FlowControl)
				{
					string[] array = new string[10]
					{
						base.DeviceParameterString,
						"/BD=",
						baudrate.ToString(),
						" /PA=",
						null,
						null,
						null,
						null,
						null,
						null
					};
					string[] array2 = array;
					int num = (int)parity;
					array2[4] = num.ToString();
					array[5] = " /IT=";
					array[6] = base.IntervalTimeout.ToString();
					array[7] = " /RS=";
					array[8] = ((int)FlowControl).ToString();
					array[9] = " ";
					return string.Concat(array);
				}
				string[] array3 = new string[8]
				{
					base.DeviceParameterString,
					"/BD=",
					baudrate.ToString(),
					" /PA=",
					null,
					null,
					null,
					null
				};
				string[] array4 = array3;
				int num2 = (int)parity;
				array4[4] = num2.ToString();
				array3[5] = " /IT=";
				array3[6] = base.IntervalTimeout.ToString();
				array3[7] = " ";
				return string.Concat(array3);
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = base.ToXMLTextWriter(ref writer, flags);
			if (channel != 1)
			{
				writer.WriteAttributeString("Channel", channel.ToString());
			}
			if (baudrate != 57600)
			{
				writer.WriteAttributeString("BaudRate", baudrate.ToString());
			}
			if (parity != Parity.Even)
			{
				writer.WriteAttributeString("Parity", parity.ToString());
			}
			if (propFlowControl != FlowControls.NOT_SET)
			{
				writer.WriteAttributeString("FlowControl", propFlowControl.ToString());
			}
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			Serial serial = (Serial)baseObj;
			if (serial == null)
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
					serial.channel = result;
				}
			}
			text = "";
			text = reader.GetAttribute("BaudRate");
			if (text != null && text.Length > 0)
			{
				int result2 = 0;
				if (PviParse.TryParseInt32(text, out result2))
				{
					serial.baudrate = result2;
				}
			}
			text = "";
			text = reader.GetAttribute("Parity");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "even":
					parity = Parity.Even;
					break;
				case "mark":
					parity = Parity.Mark;
					break;
				case "none":
					parity = Parity.None;
					break;
				case "odd":
					parity = Parity.Odd;
					break;
				case "space":
					parity = Parity.Space;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("FlowControl");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "not_set":
					propFlowControl = FlowControls.NOT_SET;
					break;
				case "rs232":
					propFlowControl = FlowControls.RS232;
					break;
				case "rs422":
					propFlowControl = FlowControls.RS422;
					break;
				case "rts_off":
					propFlowControl = FlowControls.RTS_OFF;
					break;
				case "system":
					propFlowControl = FlowControls.SYSTEM;
					break;
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
			string paraValue = "";
			propKnownDeviceParameters = "";
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (text.ToUpper().StartsWith("/IF=COM"))
				{
					propInterfaceName = text.Substring(4);
					channel = Convert.ToByte(text.Substring(7));
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/BD=", text, ref baudrate))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/RS=", text, ref paraValue))
				{
					propFlowControl = FlowControls.NOT_SET;
					if (paraValue.CompareTo("SYSTEM") == 0 || paraValue.CompareTo("-1") == 0)
					{
						propFlowControl = FlowControls.SYSTEM;
					}
					else if (paraValue.CompareTo("RTS_OFF") == 0 || paraValue.CompareTo("0") == 0)
					{
						propFlowControl = FlowControls.RTS_OFF;
					}
					else if (paraValue.CompareTo("RS232") == 0 || paraValue.CompareTo("232") == 0)
					{
						propFlowControl = FlowControls.RS232;
					}
					else if (paraValue.CompareTo("RS422") == 0 || paraValue.CompareTo("422") == 0)
					{
						propFlowControl = FlowControls.RS422;
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/PA=", text, ref paraValue))
				{
					parity = Parity.None;
					if (paraValue.CompareTo("even") == 0 || paraValue.CompareTo("2") == 0)
					{
						parity = Parity.Even;
					}
					else if (paraValue.CompareTo("mark") == 0 || paraValue.CompareTo("3") == 0)
					{
						parity = Parity.Mark;
					}
					else if (paraValue.CompareTo("odd") == 0 || paraValue.CompareTo("1") == 0)
					{
						parity = Parity.Odd;
					}
					else if (paraValue.CompareTo("space") == 0 || paraValue.CompareTo("4") == 0)
					{
						parity = Parity.Space;
					}
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

		public Serial()
			: base(DeviceType.Serial)
		{
			channel = 1;
			baudrate = 57600;
			parity = Parity.Even;
			propFlowControl = FlowControls.NOT_SET;
			propIntervalTimeout = 20;
			propInterfaceName = "COM" + channel.ToString();
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
