using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Serializable]
	public abstract class DeviceBase
	{
		protected string propKnownDeviceParameters;

		private string propUnknownDevParameters;

		protected string propKnownCpuParameters;

		private string propUnknownCpuParameters;

		private string propSavePath;

		private string propRoutingPath;

		protected int propIntervalTimeout;

		protected string propInterfaceName;

		private int propResponseTimeout;

		internal DeviceType propDeviceType;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public string KnownDeviceParameters
		{
			get
			{
				return propKnownDeviceParameters;
			}
		}

		public virtual string DeviceParameterString
		{
			get
			{
				string str = "/IF=";
				str = str + InterfaceName + " ";
				if (propUnknownDevParameters != null && 0 < propUnknownDevParameters.Length)
				{
					str = str + propUnknownDevParameters + " ";
				}
				return str;
			}
		}

		public virtual string CpuParameterString
		{
			get
			{
				string text = "";
				string str = "";
				string str2 = "";
				string str3 = "";
				if (RoutingPath != null && 0 < RoutingPath.Length)
				{
					str = "/CN=" + RoutingPath + " ";
				}
				if (ResponseTimeout != 0)
				{
					str2 = "/RT=" + ResponseTimeout.ToString() + " ";
				}
				if (SavePath != null && 0 < SavePath.Length)
				{
					str3 = "/SP='" + SavePath + "' ";
				}
				text = str + str2 + str3;
				if (propUnknownCpuParameters != null && 0 < propUnknownCpuParameters.Length)
				{
					text = text + propUnknownCpuParameters + " ";
				}
				return text;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string UnknownDeviceParameters
		{
			get
			{
				return propUnknownDevParameters;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string KnownCpuParameters
		{
			get
			{
				return propKnownCpuParameters;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string UnknownCpuParameters
		{
			get
			{
				return propUnknownCpuParameters;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/SP")]
		internal string SavePath
		{
			get
			{
				return propSavePath;
			}
			set
			{
				propSavePath = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/CN")]
		public string RoutingPath
		{
			get
			{
				return propRoutingPath;
			}
			set
			{
				propRoutingPath = value;
			}
		}

		[PviKeyWord("/IT")]
		public int IntervalTimeout
		{
			get
			{
				return propIntervalTimeout;
			}
			set
			{
				propIntervalTimeout = value;
			}
		}

		[PviKeyWord("/IF")]
		public string InterfaceName
		{
			get
			{
				return propInterfaceName;
			}
			set
			{
				propInterfaceName = value;
			}
		}

		[PviCpuParameter]
		[PviKeyWord("/RT")]
		public int ResponseTimeout
		{
			get
			{
				return propResponseTimeout;
			}
			set
			{
				propResponseTimeout = value;
			}
		}

		public DeviceType DeviceType => propDeviceType;

		public DeviceBase(DeviceType type)
		{
			Init();
			propDeviceType = type;
			UpdateInterfaceName(type);
		}

		private void UpdateInterfaceName(DeviceType type)
		{
			switch (type)
			{
			case DeviceType.ANSLTcp:
				propInterfaceName = "tcpip";
				break;
			case DeviceType.AR000:
				propInterfaceName = "SPWIN";
				break;
			case DeviceType.Shared:
				propInterfaceName = "LS251_1";
				break;
			case DeviceType.Can:
				propInterfaceName = "inacan1";
				break;
			case DeviceType.Modem:
				propInterfaceName = "modem1";
				break;
			case DeviceType.TcpIp:
				propInterfaceName = "tcpip";
				break;
			case DeviceType.TcpIpMODBUS:
				propInterfaceName = "MBUSTCP";
				break;
			default:
				propInterfaceName = "COM1";
				break;
			}
		}

		internal DeviceBase(DeviceType type, ref XmlTextReader reader, ConfigurationFlags flags)
		{
			Init();
			propDeviceType = type;
			UpdateInterfaceName(type);
		}

		internal static bool UpdateParameterFromString(string strParam, string strConnection, ref byte paraValue)
		{
			string text = strConnection.Replace('"', '\0');
			if (text.ToUpper().StartsWith(strParam))
			{
				if (0 < text.Substring(strParam.Length).Length)
				{
					if (-1 != text.Substring(strParam.Length).ToUpper().IndexOf("0X"))
					{
						paraValue = Convert.ToByte(text.Substring(strParam.Length + 2), 16);
					}
					else
					{
						paraValue = Convert.ToByte(text.Substring(strParam.Length));
					}
				}
				return true;
			}
			return false;
		}

		internal static bool UpdateParameterFromString(string strParam, string strConnection, ref uint paraValue)
		{
			string text = strConnection.Replace('"', '\0');
			if (text.ToUpper().StartsWith(strParam))
			{
				if (0 < text.Substring(strParam.Length).Length)
				{
					if (-1 != text.Substring(strParam.Length).ToUpper().IndexOf("0X"))
					{
						paraValue = Convert.ToUInt32(text.Substring(strParam.Length + 2), 16);
					}
					else
					{
						paraValue = Convert.ToUInt32(text.Substring(strParam.Length));
					}
				}
				return true;
			}
			return false;
		}

		internal static bool UpdateParameterFromString(string strParam, string strConnection, ref int paraValue)
		{
			string text = strConnection.Replace('"', '\0');
			if (text.ToUpper().StartsWith(strParam))
			{
				if (0 < text.Substring(strParam.Length).Length)
				{
					if (-1 != text.Substring(strParam.Length).ToUpper().IndexOf("0X"))
					{
						paraValue = Convert.ToInt32(text.Substring(strParam.Length + 2), 16);
					}
					else
					{
						paraValue = Convert.ToInt32(text.Substring(strParam.Length));
					}
				}
				return true;
			}
			return false;
		}

		internal static bool UpdateParameterFromString(string strParam, string strConnection, ref string paraValue)
		{
			if (strConnection.ToUpper().StartsWith(strParam))
			{
				paraValue = "";
				if (0 < strConnection.Substring(strParam.Length).Length)
				{
					paraValue = strConnection.Substring(strParam.Length);
					paraValue.Trim();
				}
				return true;
			}
			return false;
		}

		internal virtual void Init()
		{
			propSavePath = "";
			propDeviceType = DeviceType.Serial;
			propResponseTimeout = 0;
			propRoutingPath = "";
			propInterfaceName = "COM1";
			propIntervalTimeout = 0;
			propUnknownCpuParameters = "";
			propKnownCpuParameters = "";
			propUnknownDevParameters = "";
			propKnownDeviceParameters = "";
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			writer.WriteAttributeString("DeviceType", propDeviceType.ToString());
			num = SaveConnectionAttributes(ref writer);
			if (propSavePath != null && propSavePath.Length > 0)
			{
				writer.WriteAttributeString("SavePath", propSavePath);
			}
			if (InterfaceName != null && InterfaceName.Length > 0)
			{
				writer.WriteAttributeString("InterfaceName", propInterfaceName);
			}
			if (propKnownCpuParameters != null && propKnownCpuParameters.Length > 0)
			{
				writer.WriteAttributeString("KnownCpuParameters", propKnownCpuParameters);
			}
			if (propKnownDeviceParameters != null && propKnownDeviceParameters.Length > 0)
			{
				writer.WriteAttributeString("KnownDeviceParameters", propKnownDeviceParameters);
			}
			if (propUnknownCpuParameters != null && propUnknownCpuParameters.Length > 0)
			{
				writer.WriteAttributeString("UnknownCpuParameters", propUnknownCpuParameters);
			}
			if (propUnknownDevParameters != null && propUnknownDevParameters.Length > 0)
			{
				writer.WriteAttributeString("UnknownDevParameters", propUnknownDevParameters);
			}
			return num;
		}

		public int SaveConnectionAttributes(ref XmlTextWriter writer)
		{
			if (propRoutingPath != null && propRoutingPath.Length > 0)
			{
				writer.WriteAttributeString("RoutingPath", propRoutingPath.ToString());
			}
			if (propResponseTimeout != 0)
			{
				writer.WriteAttributeString("ResponseTimeout", propResponseTimeout.ToString());
			}
			if (propIntervalTimeout != 0)
			{
				writer.WriteAttributeString("IntervalTimeout", propIntervalTimeout.ToString());
			}
			return 0;
		}

		public virtual int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, DeviceBase baseObj)
		{
			string text = "";
			text = reader.GetAttribute("DeviceType");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "ansl":
					propDeviceType = DeviceType.ANSLTcp;
					break;
				case "ar010":
				case "arwin":
				case "tcpip":
					propDeviceType = DeviceType.TcpIp;
					break;
				case "tcpipmodbus":
					propDeviceType = DeviceType.TcpIpMODBUS;
					break;
				case "shared":
					propDeviceType = DeviceType.Shared;
					break;
				case "serial":
					propDeviceType = DeviceType.Serial;
					break;
				case "modem":
					propDeviceType = DeviceType.Modem;
					break;
				case "can":
					propDeviceType = DeviceType.Can;
					break;
				case "ar000":
				case "arsim":
					propDeviceType = DeviceType.AR000;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("SavePath");
			if (text != null && text.Length > 0)
			{
				baseObj.propSavePath = text;
			}
			text = "";
			text = reader.GetAttribute("InterfaceName");
			if (text != null && text.Length > 0)
			{
				baseObj.propInterfaceName = text;
			}
			text = "";
			text = reader.GetAttribute("KnownCpuParameters");
			if (text != null && text.Length > 0)
			{
				baseObj.propKnownCpuParameters = text;
			}
			text = "";
			text = reader.GetAttribute("KnownDeviceParameters");
			if (text != null && text.Length > 0)
			{
				baseObj.propKnownDeviceParameters = text;
			}
			text = "";
			text = reader.GetAttribute("UnknownCpuParameters");
			if (text != null && text.Length > 0)
			{
				baseObj.propUnknownCpuParameters = text;
			}
			text = "";
			text = reader.GetAttribute("UnknownDevParameters");
			if (text != null && text.Length > 0)
			{
				baseObj.propUnknownDevParameters = text;
			}
			text = "";
			text = reader.GetAttribute("RoutingPath");
			if (text != null && text.Length > 0)
			{
				baseObj.propRoutingPath = text;
			}
			text = "";
			text = reader.GetAttribute("ResponseTimeout");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					baseObj.propResponseTimeout = result;
				}
			}
			text = "";
			text = reader.GetAttribute("IntervalTimeout");
			if (text != null && text.Length > 0)
			{
				int result2 = 0;
				if (PviParse.TryParseInt32(text, out result2))
				{
					baseObj.propIntervalTimeout = result2;
				}
			}
			return 0;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void UpdateDeviceParameters(string parmItem)
		{
			string[] array = parmItem.Split(" ".ToCharArray());
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (UpdateParameterFromString("/IF=", text, ref propInterfaceName))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("/RT=", text, ref propResponseTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("RT=", text, ref propResponseTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("/IT=", text, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("IT=", text, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (-1 == propUnknownDevParameters.IndexOf(text) && -1 == propKnownDeviceParameters.IndexOf(text))
				{
					string text2 = text;
					if (text.IndexOf("/") != 0)
					{
						text2 = "/" + text;
					}
					if (propUnknownDevParameters.Length == 0)
					{
						propUnknownDevParameters = text2.Trim();
					}
					else
					{
						propUnknownDevParameters = propUnknownDevParameters + " " + text2.Trim();
					}
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void UpdateCpuParameters(string parmItem)
		{
			string[] array = parmItem.Split(" ".ToCharArray());
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				string text = (string)array.GetValue(i);
				if (UpdateParameterFromString("/IF=", text, ref propInterfaceName))
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
				else if (UpdateParameterFromString("/RT=", text, ref propResponseTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("RT=", text, ref propResponseTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("/IT=", text, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("IT=", text, ref propIntervalTimeout))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("/SP=", text, ref propSavePath))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("SP=", text, ref propSavePath))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("/CN=", text, ref propRoutingPath))
				{
					flag = true;
				}
				else if (UpdateParameterFromString("CN=", text, ref propRoutingPath))
				{
					flag = true;
				}
				else if (-1 == propUnknownCpuParameters.IndexOf(text) && -1 == propKnownCpuParameters.IndexOf(text))
				{
					string text2 = text;
					if (text.IndexOf("/") != 0)
					{
						text2 = "/" + text;
					}
					if (propUnknownCpuParameters.Length == 0)
					{
						propUnknownCpuParameters = text2.Trim();
					}
					else
					{
						propUnknownCpuParameters = propUnknownCpuParameters + " " + text2.Trim();
					}
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
	}
}
