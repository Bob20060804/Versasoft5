using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class ANSLTcp : DeviceBase
	{
		private uint remotePort;

		private uint communicationTimeout;

		private uint sendDelay;

		private bool propRedundancyCommMode;

		private uint communicationBufferSize;

		private string destinationIpAddress;

		private string _sslConfiguration;

		[PviCpuParameter]
		[PviKeyWord("/PT")]
		[CLSCompliant(false)]
		public uint RemotePort
		{
			get
			{
				return remotePort;
			}
			set
			{
				remotePort = value;
			}
		}

		[CLSCompliant(false)]
		[PviCpuParameter]
		[PviKeyWord("/COMT")]
		public uint CommunicationTimeout
		{
			get
			{
				return communicationTimeout;
			}
			set
			{
				communicationTimeout = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/SDT")]
		[CLSCompliant(false)]
		public uint SendDelay
		{
			get
			{
				return sendDelay;
			}
			set
			{
				sendDelay = value;
			}
		}

		public bool RedundancyCommMode
		{
			get
			{
				return propRedundancyCommMode;
			}
			set
			{
				propRedundancyCommMode = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/BSIZE")]
		[CLSCompliant(false)]
		public uint CommunicationBufferSize
		{
			get
			{
				return communicationBufferSize;
			}
			set
			{
				communicationBufferSize = value;
			}
		}

		[PviKeyWord("/IP")]
		[PviCpuParameter]
		public string DestinationIpAddress
		{
			get
			{
				return destinationIpAddress;
			}
			set
			{
				destinationIpAddress = value;
			}
		}

		[PviKeyWord("/SSL")]
		[PviCpuParameter]
		public string SSLConfiguration
		{
			get
			{
				return _sslConfiguration;
			}
			set
			{
				_sslConfiguration = value;
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
				text = base.CpuParameterString + "/IP=" + DestinationIpAddress + " /PT=" + remotePort.ToString() + " /COMT=" + CommunicationTimeout.ToString();
				if (0 < SendDelay)
				{
					text = text + " /SDT=" + SendDelay.ToString();
				}
				if (CommunicationBufferSize != 0 && 65536 != CommunicationBufferSize)
				{
					text = text + " /BSIZE=" + CommunicationBufferSize.ToString();
				}
				if (SSLConfiguration != null && 0 < SSLConfiguration.Length)
				{
					text = text + " /SSL='" + SSLConfiguration.ToString() + "'";
				}
				return text;
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = base.ToXMLTextWriter(ref writer, flags);
			if (DestinationIpAddress != null && DestinationIpAddress.Length > 0)
			{
				writer.WriteAttributeString("DestinationIPAddress", DestinationIpAddress.ToString());
			}
			if (0 < CommunicationTimeout)
			{
				writer.WriteAttributeString("CommunicationTimeout", CommunicationTimeout.ToString());
			}
			if (0 < CommunicationBufferSize)
			{
				writer.WriteAttributeString("CommunicationBufferSize", CommunicationBufferSize.ToString());
			}
			if (0 < SendDelay)
			{
				writer.WriteAttributeString("SendDelay", SendDelay.ToString());
			}
			if (0 < RemotePort)
			{
				writer.WriteAttributeString("RemotePort", RemotePort.ToString());
			}
			if (SSLConfiguration != null && SSLConfiguration.Length > 0)
			{
				writer.WriteAttributeString("SSLConfiguration", SSLConfiguration.ToString());
			}
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			ANSLTcp aNSLTcp = (ANSLTcp)baseObj;
			if (aNSLTcp == null)
			{
				return -1;
			}
			uint result = 0u;
			string text = "";
			text = reader.GetAttribute("remotePort");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result))
			{
				aNSLTcp.remotePort = result;
			}
			text = "";
			text = reader.GetAttribute("CommunicationTimeout");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result))
			{
				aNSLTcp.communicationTimeout = result;
			}
			text = "";
			text = reader.GetAttribute("CommunicationBufferSize");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result))
			{
				aNSLTcp.communicationBufferSize = result;
			}
			text = "";
			text = reader.GetAttribute("SendDelay");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result))
			{
				aNSLTcp.sendDelay = result;
			}
			text = "";
			destinationIpAddress = "";
			destinationIpAddress = reader.GetAttribute("DestinationIPAddress");
			_sslConfiguration = "";
			_sslConfiguration = reader.GetAttribute("SSLConfiguration");
			return 0;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		private void UpdateParameters(string parameters, bool bCpu)
		{
			uint paraValue = 0u;
			if (parameters == null)
			{
				return;
			}
			string[] array = parameters.Split(" ".ToCharArray());
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (DeviceBase.UpdateParameterFromString("/IF=", text, ref propInterfaceName))
				{
					if (propInterfaceName.ToLower().CompareTo("tcpip") != 0)
					{
						propInterfaceName = "tcpip";
						text = null;
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/IP=", text, ref destinationIpAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DAIP=", text, ref destinationIpAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/PT=", text, ref remotePort))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/COMT=", text, ref communicationTimeout))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/SDT=", text, ref sendDelay))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/BSIZE=", text, ref communicationBufferSize))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/RED=", text, ref paraValue))
				{
					propRedundancyCommMode = false;
					if (paraValue != 0)
					{
						propRedundancyCommMode = true;
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/SSL=", text, ref _sslConfiguration))
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
				if (!flag || text == null)
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

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UpdateDeviceParameters(string parameters)
		{
			propKnownDeviceParameters = "";
			UpdateParameters(parameters, bCpu: false);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override void UpdateCpuParameters(string parameters)
		{
			propKnownCpuParameters = "";
			UpdateParameters(parameters, bCpu: true);
		}

		public ANSLTcp()
			: base(DeviceType.ANSLTcp)
		{
			Init();
		}

		internal override void Init()
		{
			base.Init();
			propDeviceType = DeviceType.ANSLTcp;
			communicationBufferSize = 0u;
			communicationTimeout = 1500u;
			sendDelay = 0u;
			destinationIpAddress = "127.0.0.1";
			_sslConfiguration = "";
			remotePort = 11169u;
			propInterfaceName = "tcpip";
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
