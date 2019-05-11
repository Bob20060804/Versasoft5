using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class Can : DeviceBase
	{
		private int propCANIdentifiers;

		private int propMAXStationNumber;

		private int propControllerNumber;

		private byte channel;

		private int interruptNumber;

		private int ioPort;

		private int messageCount;

		private int cycleTime;

		private int baseId;

		private int baudRate;

		private int sourceAddress;

		private int destinationAddress;

		[DefaultValue(29)]
		[Browsable(true)]
		[Category("Communication")]
		[Description("Gets or sets the number of CAN identifiers. CAN communication with 29-bit identifiers (extended frames) or 11-bit identifiers (standard frames). If 29-bit CAN identifiers (extended frames) are used, then 11-bit identifiers cannot be sent or received. Every station in the INA2000 network must have the same setting.")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[PviKeyWord("/CMODE")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int CANIdentifiers
		{
			get
			{
				return propCANIdentifiers;
			}
			set
			{
				propCANIdentifiers = value;
			}
		}

		[Description("Gets or sets the highest station number. Number of maximum possible INA2000 stations (= highest station number). Every station in the INA2000 network must have the same setting.")]
		[Category("Communication")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[PviKeyWord("/MDA")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(32)]
		public int MAXStationNumber
		{
			get
			{
				return propMAXStationNumber;
			}
			set
			{
				propMAXStationNumber = value;
			}
		}

		[PviKeyWord("/CNO")]
		[Description("Gets or sets the number of the controller. 2 CAN controllers are available on the LS172 card. The desired controller is selected with the CNO parameter. No value other than 0 (zero) may be specified for the default CAN controller.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(0)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[Category("Communication")]
		public int ControllerNumber
		{
			get
			{
				return propControllerNumber;
			}
			set
			{
				propControllerNumber = value;
			}
		}

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
				propInterfaceName = "inacan" + value.ToString();
			}
		}

		[PviKeyWord("/BD")]
		public int BaudRate
		{
			get
			{
				return baudRate;
			}
			set
			{
				baudRate = value;
			}
		}

		[PviKeyWord("/BI")]
		public int BaseId
		{
			get
			{
				return baseId;
			}
			set
			{
				baseId = value;
			}
		}

		[PviKeyWord("/CT")]
		public int CycleTime
		{
			get
			{
				return cycleTime;
			}
			set
			{
				cycleTime = value;
			}
		}

		[PviKeyWord("/MC")]
		public int MessageCount
		{
			get
			{
				return messageCount;
			}
			set
			{
				messageCount = value;
			}
		}

		[PviKeyWord("/IO")]
		public int IoPort
		{
			get
			{
				return ioPort;
			}
			set
			{
				ioPort = value;
			}
		}

		[PviKeyWord("/IR")]
		public int InterruptNumber
		{
			get
			{
				return interruptNumber;
			}
			set
			{
				interruptNumber = value;
			}
		}

		[PviKeyWord("/SA")]
		public int SourceAddress
		{
			get
			{
				return sourceAddress;
			}
			set
			{
				sourceAddress = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/DA")]
		public int DestinationAddress
		{
			get
			{
				return destinationAddress;
			}
			set
			{
				destinationAddress = value;
			}
		}

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				return base.DeviceParameterString + "/CNO=" + ControllerNumber.ToString() + " /BI=" + BaseId.ToString() + " /IT=" + base.IntervalTimeout.ToString() + " /MDA=" + MAXStationNumber.ToString() + " /BD=" + BaudRate.ToString() + " /CMODE=" + CANIdentifiers.ToString() + " /CT=" + CycleTime.ToString() + " /MC=" + MessageCount.ToString() + " /SA=" + SourceAddress.ToString() + " /IR=" + InterruptNumber.ToString() + " /IO=" + IoPort.ToString() + " ";
			}
		}

		public override string CpuParameterString
		{
			get
			{
				string text = "";
				return base.CpuParameterString + "/DA=" + destinationAddress.ToString() + " ";
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			num = base.ToXMLTextWriter(ref writer, flags);
			if (channel != 1)
			{
				writer.WriteAttributeString("Channel", channel.ToString());
			}
			if (baseId != 1598)
			{
				writer.WriteAttributeString("BaseID", baseId.ToString());
			}
			if (baudRate != 500000)
			{
				writer.WriteAttributeString("Baudrate", baudRate.ToString());
			}
			if (cycleTime != 10)
			{
				writer.WriteAttributeString("CycleTime", cycleTime.ToString());
			}
			if (MessageCount != 10)
			{
				writer.WriteAttributeString("Messages", messageCount.ToString());
			}
			if (sourceAddress != 1)
			{
				writer.WriteAttributeString("Source", sourceAddress.ToString());
			}
			if (destinationAddress != 2)
			{
				writer.WriteAttributeString("Destination", destinationAddress.ToString());
			}
			if (interruptNumber != 10)
			{
				writer.WriteAttributeString("IRQ", interruptNumber.ToString());
			}
			if (ioPort != 900)
			{
				writer.WriteAttributeString("Port", ioPort.ToString());
			}
			writer.WriteAttributeString("Stations", propMAXStationNumber.ToString());
			writer.WriteAttributeString("Controller", propControllerNumber.ToString());
			writer.WriteAttributeString("Identifiers", propCANIdentifiers.ToString());
			return num;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			Can can = (Can)baseObj;
			if (can == null)
			{
				return -1;
			}
			int result = 0;
			string text = "";
			text = reader.GetAttribute("Channel");
			if (text != null && text.Length > 0)
			{
				byte result2 = 0;
				if (PviParse.TryParseByte(text, out result2))
				{
					can.channel = result2;
				}
			}
			text = "";
			text = reader.GetAttribute("BaseID");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.baseId = result;
			}
			text = "";
			text = reader.GetAttribute("Baudrate");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.baudRate = result;
			}
			text = "";
			text = reader.GetAttribute("CycleTime");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.cycleTime = result;
			}
			text = "";
			text = reader.GetAttribute("Messages");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.messageCount = result;
			}
			text = "";
			text = reader.GetAttribute("Source");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.sourceAddress = result;
			}
			text = "";
			text = reader.GetAttribute("Destination");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.destinationAddress = result;
			}
			text = "";
			text = reader.GetAttribute("IRQ");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.interruptNumber = result;
			}
			text = "";
			text = reader.GetAttribute("Port");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.ioPort = result;
			}
			text = "";
			text = reader.GetAttribute("Stations");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.propMAXStationNumber = result;
			}
			text = "";
			text = reader.GetAttribute("Controller");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.propControllerNumber = result;
			}
			text = "";
			text = reader.GetAttribute("Identifiers");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				can.propCANIdentifiers = result;
			}
			return 0;
		}

		private void UpdateParameters(string parameters, bool bCpu)
		{
			string[] array = parameters.Split(" ".ToCharArray());
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (text.ToUpper().StartsWith("/IF=INACAN"))
				{
					propInterfaceName = text.Substring(4);
					channel = Convert.ToByte(text.Substring(10));
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CNO=", text, ref propControllerNumber))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/BI=", text, ref baseId))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/IT=", text, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/MDA=", text, ref propMAXStationNumber))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/BD=", text, ref baudRate))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CMODE=", text, ref propCANIdentifiers))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CT=", text, ref cycleTime))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/MC=", text, ref messageCount))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/SA=", text, ref sourceAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DA=", text, ref destinationAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/IR=", text, ref interruptNumber))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/IO=", text, ref ioPort))
				{
					flag = true;
				}
				else if (bCpu)
				{
					base.UpdateCpuParameters(text);
				}
				else
				{
					base.UpdateDeviceParameters(text);
				}
				if (!flag)
				{
					continue;
				}
				string text2 = text;
				if (text.IndexOf("/") != 0)
				{
					text2 = "/" + text;
				}
				if (bCpu)
				{
					if (propKnownCpuParameters.Length == 0)
					{
						propKnownCpuParameters = text2.Trim();
					}
					else
					{
						propKnownCpuParameters = propKnownCpuParameters + " " + text2.Trim();
					}
				}
				else if (propKnownDeviceParameters.Length == 0)
				{
					propKnownDeviceParameters = text2.Trim();
				}
				else
				{
					propKnownDeviceParameters = propKnownDeviceParameters + " " + text2.Trim();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override void UpdateDeviceParameters(string parameters)
		{
			propKnownDeviceParameters = "";
			UpdateParameters(parameters, bCpu: false);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateCpuParameters(string parameters)
		{
			propKnownCpuParameters = "";
			UpdateParameters(parameters, bCpu: true);
		}

		public Can()
			: base(DeviceType.Can)
		{
			propCANIdentifiers = 29;
			propMAXStationNumber = 32;
			propIntervalTimeout = 0;
			propControllerNumber = 0;
			channel = 1;
			baudRate = 500000;
			baseId = 1598;
			cycleTime = 10;
			messageCount = 10;
			ioPort = 900;
			interruptNumber = 10;
			sourceAddress = 1;
			destinationAddress = 2;
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
