using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class TcpIpMODBUS : DeviceBase
	{
		private string propFBDConfiguration;

		private string propDestinationIPAddress;

		private int propPortNumber;

		private int propUnitID;

		private int propConnectionRetries;

		[PviKeyWord("/CFG")]
		[PviCpuParameter]
		public string FBDConfiguration
		{
			get
			{
				return propFBDConfiguration;
			}
			set
			{
				propFBDConfiguration = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/DAIP")]
		public string DestinationIPAddress
		{
			get
			{
				return propDestinationIPAddress;
			}
			set
			{
				propDestinationIPAddress = value;
			}
		}

		[PviKeyWord("/PN")]
		[PviCpuParameter]
		public int PortNumber
		{
			get
			{
				return propPortNumber;
			}
			set
			{
				propPortNumber = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/DA")]
		public int UnitID
		{
			get
			{
				return propUnitID;
			}
			set
			{
				propUnitID = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/CR")]
		public int ConnectionRetries
		{
			get
			{
				return propConnectionRetries;
			}
			set
			{
				propConnectionRetries = value;
			}
		}

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				return base.DeviceParameterString;
			}
		}

		public override string CpuParameterString
		{
			get
			{
				string text = "";
				if (FBDConfiguration != null && 0 < FBDConfiguration.Length)
				{
					if (UnitID != 0)
					{
						return base.CpuParameterString + "/CFG=" + FBDConfiguration.ToString() + " /DA=" + UnitID.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
					}
					return base.CpuParameterString + "/CFG=" + FBDConfiguration.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
				}
				if (DestinationIPAddress != null && 0 < DestinationIPAddress.Length)
				{
					if (UnitID != 0)
					{
						return base.CpuParameterString + "/DAIP=" + DestinationIPAddress + " /PN=" + PortNumber.ToString() + " /DA=" + UnitID.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
					}
					return base.CpuParameterString + "/DAIP=" + DestinationIPAddress + " /PN=" + PortNumber.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
				}
				if (UnitID != 0)
				{
					return base.CpuParameterString + "/PN=" + PortNumber.ToString() + " /DA=" + UnitID.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
				}
				return base.CpuParameterString + "/PN=" + PortNumber.ToString() + " /CR=" + ConnectionRetries.ToString() + " ";
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			base.ToXMLTextWriter(ref writer, flags);
			writer.WriteAttributeString("LINE", "LNMODBUS");
			if (FBDConfiguration != null && FBDConfiguration.Length > 0)
			{
				writer.WriteAttributeString("FBDConfiguration", FBDConfiguration);
			}
			if (DestinationIPAddress != null && DestinationIPAddress.Length > 0)
			{
				writer.WriteAttributeString("DestinationIpAddress", DestinationIPAddress);
			}
			if (PortNumber > 0)
			{
				writer.WriteAttributeString("PortNumber", PortNumber.ToString());
			}
			if (UnitID > 0)
			{
				writer.WriteAttributeString("UnitID", UnitID.ToString());
			}
			if (ConnectionRetries > 0)
			{
				writer.WriteAttributeString("ConnectionRetries", ConnectionRetries.ToString());
			}
			return 0;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			TcpIpMODBUS tcpIpMODBUS = (TcpIpMODBUS)baseObj;
			if (tcpIpMODBUS == null)
			{
				return -1;
			}
			string text = "";
			text = reader.GetAttribute("DestinationIpAddress");
			if (text != null && text.Length > 0)
			{
				tcpIpMODBUS.propDestinationIPAddress = text;
			}
			text = "";
			text = reader.GetAttribute("FBDConfiguration");
			if (text != null && text.Length > 0)
			{
				tcpIpMODBUS.propFBDConfiguration = text;
			}
			text = "";
			text = reader.GetAttribute("PortNumber");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					tcpIpMODBUS.propPortNumber = result;
				}
			}
			text = "";
			text = reader.GetAttribute("UnitID");
			if (text != null && text.Length > 0)
			{
				int result2 = 0;
				if (PviParse.TryParseInt32(text, out result2))
				{
					tcpIpMODBUS.propUnitID = result2;
				}
			}
			text = "";
			text = reader.GetAttribute("ConnectionRetries");
			if (text != null && text.Length > 0)
			{
				int result3 = 0;
				if (PviParse.TryParseInt32(text, out result3))
				{
					tcpIpMODBUS.propConnectionRetries = result3;
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
				if (DeviceBase.UpdateParameterFromString("/IF=", text, ref propInterfaceName))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DAIP=", text, ref propDestinationIPAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CFG=", text, ref propFBDConfiguration))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/PN=", text, ref propPortNumber))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DA=", text, ref propUnitID))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CR=", text, ref propConnectionRetries))
				{
					flag = true;
				}
				else
				{
					base.UpdateDeviceParameters(text);
				}
				if (flag)
				{
					if (propKnownDeviceParameters.Length == 0)
					{
						propKnownDeviceParameters = text.Trim();
					}
					else
					{
						propKnownDeviceParameters = propKnownDeviceParameters + " " + text.Trim();
					}
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateCpuParameters(string parameters)
		{
			string[] array = parameters.Split(" ".ToCharArray());
			bool flag = true;
			propKnownCpuParameters = " ";
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (DeviceBase.UpdateParameterFromString("/DAIP=", text, ref propDestinationIPAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CFG=", text, ref propFBDConfiguration))
				{
					flag = true;
				}
				else
				{
					base.UpdateCpuParameters(text);
				}
				if (flag)
				{
					string text2 = text;
					if (text.IndexOf("/") != 0)
					{
						text2 = "/" + text;
					}
					if (propKnownCpuParameters.Length == 0)
					{
						propKnownCpuParameters = text2.Trim();
					}
					else
					{
						propKnownCpuParameters = propKnownCpuParameters + " " + text2.Trim();
					}
				}
			}
		}

		public TcpIpMODBUS()
			: base(DeviceType.TcpIpMODBUS)
		{
			propFBDConfiguration = "";
			propDestinationIPAddress = "";
			propPortNumber = 502;
			propUnitID = 255;
			propConnectionRetries = 0;
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
