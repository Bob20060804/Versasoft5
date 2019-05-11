using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public class TcpIp : DeviceBase
	{
		private TCPModes propTcpMode;

		private string propTarget;

		private bool propUniqueDeviceForSAandLOPO;

		private short sourcePort;

		private uint propLOPO;

		private byte sourceStation;

		private short destinationPort;

		private uint propREPO;

		private byte destinationStation;

		private byte checkDestinationStation;

		private int propQuickDownload;

		private string destinationIpAddress;

		public TCPModes TcpMode
		{
			get
			{
				return propTcpMode;
			}
			set
			{
				propTcpMode = value;
				InitMode(propTcpMode);
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/CKDA")]
		public byte CheckDestinationStation
		{
			get
			{
				return checkDestinationStation;
			}
			set
			{
				checkDestinationStation = value;
			}
		}

		[CLSCompliant(false)]
		[PviKeyWord("/LOPO")]
		public uint LocalPort
		{
			get
			{
				return propLOPO;
			}
			set
			{
				propLOPO = value;
			}
		}

		public short SourcePort
		{
			get
			{
				return sourcePort;
			}
			set
			{
				sourcePort = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/TA")]
		public string Target
		{
			get
			{
				return propTarget;
			}
			set
			{
				propTarget = value;
			}
		}

		[PviKeyWord("/SA")]
		public byte SourceStation
		{
			get
			{
				return sourceStation;
			}
			set
			{
				sourceStation = value;
			}
		}

		[CLSCompliant(false)]
		[PviKeyWord("/REPO")]
		[PviCpuParameter]
		public uint RemotePort
		{
			get
			{
				return propREPO;
			}
			set
			{
				propREPO = value;
			}
		}

		public short DestinationPort
		{
			get
			{
				return destinationPort;
			}
			set
			{
				destinationPort = value;
			}
		}

		[PviKeyWord("/ANSL")]
		[PviCpuParameter]
		public int QuickDownload
		{
			get
			{
				return propQuickDownload;
			}
			set
			{
				propQuickDownload = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/DA")]
		public byte DestinationStation
		{
			get
			{
				return destinationStation;
			}
			set
			{
				destinationStation = value;
			}
		}

		[PviKeyWord("/UDEV")]
		public bool UniqueDeviceForSAandLOPO
		{
			get
			{
				return propUniqueDeviceForSAandLOPO;
			}
			set
			{
				propUniqueDeviceForSAandLOPO = value;
			}
		}

		[PviKeyWord("/DAIP")]
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

		public override string DeviceParameterString
		{
			get
			{
				string text = "";
				return base.DeviceParameterString + "/LOPO=" + ((0 < propLOPO) ? propLOPO.ToString() : sourcePort.ToString()) + " /SA=" + sourceStation.ToString() + (propUniqueDeviceForSAandLOPO ? " /UDEV=1 " : " ");
			}
		}

		public override string CpuParameterString
		{
			get
			{
				string text = "";
				if (DestinationIpAddress != null && 0 < DestinationIpAddress.Length)
				{
					if (DestinationStation != 0)
					{
						return base.CpuParameterString + " /DA=" + DestinationStation.ToString() + " /CKDA=" + checkDestinationStation.ToString() + " /REPO=" + ((0 < propREPO) ? propREPO.ToString() : destinationPort.ToString()) + ((Target != null && 0 < Target.Length) ? (" /TA=" + Target) : "") + " /ANSL=" + propQuickDownload.ToString() + " ";
					}
					return base.CpuParameterString + "/DAIP=" + DestinationIpAddress + " /CKDA=" + checkDestinationStation.ToString() + " /REPO=" + ((0 < propREPO) ? propREPO.ToString() : destinationPort.ToString()) + ((Target != null && 0 < Target.Length) ? (" /TA=" + Target) : "") + " /ANSL=" + propQuickDownload.ToString() + " ";
				}
				if (DestinationStation != 0)
				{
					return base.CpuParameterString + "/DA=" + DestinationStation.ToString() + " /CKDA=" + checkDestinationStation.ToString() + " /REPO=" + ((0 < propREPO) ? propREPO.ToString() : destinationPort.ToString()) + ((Target != null && 0 < Target.Length) ? (" /TA=" + Target) : "") + " /ANSL=" + propQuickDownload.ToString() + " ";
				}
				return base.CpuParameterString + "/CKDA=" + checkDestinationStation.ToString() + " /REPO=" + ((0 < propREPO) ? propREPO.ToString() : destinationPort.ToString()) + ((Target != null && 0 < Target.Length) ? (" /TA=" + Target) : "") + " /ANSL=" + propQuickDownload.ToString() + " ";
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = base.ToXMLTextWriter(ref writer, flags);
			if (checkDestinationStation > 0)
			{
				writer.WriteAttributeString("CheckDestinationStation", checkDestinationStation.ToString());
			}
			if (sourcePort > 0 || 0 < propLOPO)
			{
				writer.WriteAttributeString("SourcePort", (0 < propLOPO) ? propLOPO.ToString() : sourcePort.ToString());
			}
			if (sourceStation > 0)
			{
				writer.WriteAttributeString("SourceStation", sourceStation.ToString());
			}
			if (destinationPort > 0 || 0 < propREPO)
			{
				writer.WriteAttributeString("DestinationPort", (0 < propREPO) ? propREPO.ToString() : destinationPort.ToString());
			}
			if (destinationStation > 0)
			{
				writer.WriteAttributeString("DestinationStation", destinationStation.ToString());
			}
			if (destinationIpAddress != null && destinationIpAddress.Length > 0)
			{
				writer.WriteAttributeString("DestinationIPAddress", destinationIpAddress.ToString());
			}
			if (QuickDownload != 0)
			{
				writer.WriteAttributeString("QuickDownload", QuickDownload.ToString());
			}
			if (propTarget != "")
			{
				writer.WriteAttributeString("Target", propTarget);
			}
			if (propUniqueDeviceForSAandLOPO)
			{
				writer.WriteAttributeString("UniqueDeviceForSAandLOPO", propUniqueDeviceForSAandLOPO.ToString());
			}
			return result;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			base.FromXmlTextReader(ref reader, flags, baseObj);
			TcpIp tcpIp = (TcpIp)baseObj;
			if (tcpIp == null)
			{
				return -1;
			}
			int result = 0;
			byte result2 = 0;
			uint result3 = 0u;
			string text = "";
			text = reader.GetAttribute("CheckDestinationStation");
			if (text != null && text.Length > 0 && PviParse.TryParseByte(text, out result2))
			{
				tcpIp.checkDestinationStation = result2;
			}
			text = "";
			text = reader.GetAttribute("SourcePort");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result3))
			{
				tcpIp.propLOPO = result3;
				if (32767L >= (long)tcpIp.propLOPO)
				{
					tcpIp.sourcePort = Convert.ToInt16(tcpIp.propLOPO);
				}
			}
			text = "";
			text = reader.GetAttribute("SourceStation");
			if (text != null && text.Length > 0 && PviParse.TryParseByte(text, out result2))
			{
				tcpIp.sourceStation = result2;
			}
			text = "";
			text = reader.GetAttribute("DestinationPort");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result3))
			{
				tcpIp.propREPO = result3;
				if (32767L >= (long)tcpIp.propREPO)
				{
					tcpIp.destinationPort = Convert.ToInt16(tcpIp.propREPO);
				}
			}
			text = "";
			text = reader.GetAttribute("DestinationStation");
			if (text != null && text.Length > 0 && PviParse.TryParseByte(text, out result2))
			{
				tcpIp.destinationStation = result2;
			}
			text = "";
			text = reader.GetAttribute("DestinationIPAddress");
			if (text != null && text.Length > 0)
			{
				tcpIp.destinationIpAddress = text;
			}
			text = "";
			text = reader.GetAttribute("QuickDownload");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result))
			{
				tcpIp.propQuickDownload = result;
			}
			text = "";
			text = reader.GetAttribute("Target");
			if (text != null && text.Length > 0)
			{
				propTarget = text;
			}
			text = "";
			text = reader.GetAttribute("UniqueDeviceForSAandLOPO");
			propUniqueDeviceForSAandLOPO = false;
			if (text != null && text.ToLower().CompareTo("true") == 0)
			{
				propUniqueDeviceForSAandLOPO = true;
			}
			return 0;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		private void UpdateParameters(string parameters, bool bCpu)
		{
			if (parameters == null)
			{
				return;
			}
			string[] array = parameters.Split(" ".ToCharArray());
			string paraValue = "";
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (text.ToUpper().StartsWith("/IF=TCP"))
				{
					propInterfaceName = text.Substring(4);
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DAIP=", text, ref destinationIpAddress))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/LOPO=", text, ref propLOPO))
				{
					if (32767L >= (long)propLOPO)
					{
						sourcePort = Convert.ToInt16(propLOPO);
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/SA=", text, ref sourceStation))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/UDEV=", text, ref paraValue))
				{
					propUniqueDeviceForSAandLOPO = false;
					if (paraValue.CompareTo("true") == 0)
					{
						propUniqueDeviceForSAandLOPO = true;
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/DA=", text, ref destinationStation))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/REPO=", text, ref propREPO))
				{
					destinationPort = 11159;
					if (32767L >= (long)propREPO)
					{
						destinationPort = Convert.ToInt16(propREPO);
					}
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/CKDA=", text, ref checkDestinationStation))
				{
					flag = true;
				}
				else if (DeviceBase.UpdateParameterFromString("/ANSL=", text, ref propQuickDownload))
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

		internal override void Init()
		{
			base.Init();
			propDeviceType = DeviceType.TcpIp;
			InitTCpParams(0, 0, 0u, 0u, "");
			base.InterfaceName = "tcpip";
		}

		private void InitTCpParams(byte sa, byte da, uint sp, uint dp, string daip)
		{
			propTarget = null;
			sourceStation = sa;
			destinationStation = da;
			propLOPO = sp;
			if (32767L >= (long)propLOPO)
			{
				sourcePort = Convert.ToInt16(propLOPO);
			}
			propREPO = dp;
			if (32767L >= (long)propREPO)
			{
				destinationPort = Convert.ToInt16(propREPO);
			}
			destinationIpAddress = daip;
			propQuickDownload = 1;
			propUniqueDeviceForSAandLOPO = false;
		}

		public TcpIp()
			: base(DeviceType.TcpIp)
		{
			Init();
			propInterfaceName = "tcpip";
		}

		public TcpIp(TCPModes mode)
			: base(DeviceType.TcpIp)
		{
			Init();
			propTcpMode = mode;
			InitMode(mode);
			propInterfaceName = "tcpip";
		}

		private void InitMode(TCPModes mode)
		{
			switch (mode)
			{
			case TCPModes.AR000:
				InitTCpParams(1, 2, 11159u, 11160u, "127.0.0.1");
				break;
			case TCPModes.AR010:
				InitTCpParams(1, 2, 0u, 0u, "192.168.0.2");
				break;
			case TCPModes.STANDARD:
				InitTCpParams(1, 2, 11159u, 11159u, "");
				break;
			default:
				InitTCpParams(0, 0, 0u, 0u, "");
				break;
			}
		}

		public override string ToString()
		{
			return DeviceParameterString + CpuParameterString.Trim();
		}
	}
}
