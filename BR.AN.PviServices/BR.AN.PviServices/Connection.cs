using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Connection : Base
	{
		public enum CommSyncDirections
		{
			ConnectionToCpu,
			FromCpuToConnection
		}

		private bool propLineNameHasBeenSet;

		private bool propDeviceNameHasBeenSet;

		private bool stationNameHasBeenSet;

		internal bool propLineIsDirty;

		internal bool propDeviceIsDirty;

		internal string propLineDesc;

		internal string propDeviceDesc;

		internal string propConnectionParameter;

		private Cpu propCpu;

		internal PviLINE pviLineObj;

		internal PviDEVICE pviDeviceObj;

		internal PviSTATION pviStationObj;

		private string propDeviceName;

		private string propModuleInfoPath;

		private DeviceType propDeviceType;

		private DeviceBase propDevice;

		internal Serial serial;

		internal Can can;

		internal TcpIp tcpip;

		internal ANSLTcp tcpANSL;

		internal TcpIpMODBUS tcpipModBus;

		internal SimpleNetworkManagementProtocol snmpLine;

		internal Shared shared;

		internal Modem propINAModem;

		internal AR000 arSim;

		private string propOldDeviceName;

		private string propOldStationName;

		private string propOldLineName;

		public Cpu Cpu => propCpu;

		public override Service Service
		{
			get
			{
				if (propCpu == null)
				{
					return base.Service;
				}
				return propCpu.Service;
			}
		}

		protected internal virtual string DeviceParameter
		{
			get
			{
				return pviDeviceObj.ObjectParam;
			}
			set
			{
				ParseDeviceParameters(value);
				UpdateDeviceParameter();
			}
		}

		protected internal virtual string LineName
		{
			get
			{
				return pviLineObj.Name;
			}
			set
			{
				propLineNameHasBeenSet = true;
				pviLineObj.propName = value;
			}
		}

		protected internal virtual string DeviceName
		{
			get
			{
				return pviDeviceObj.Name;
			}
			set
			{
				propDeviceNameHasBeenSet = true;
				pviDeviceObj.propName = value;
				propDeviceName = value;
			}
		}

		protected internal virtual string StationName
		{
			get
			{
				return pviStationObj.Name;
			}
			set
			{
				stationNameHasBeenSet = true;
				pviStationObj.SetName(value);
			}
		}

		protected internal virtual string ConnectionParameter
		{
			get
			{
				return propConnectionParameter;
			}
			set
			{
				ParseConnectionParameters(value);
				UpdateDeviceParameter();
			}
		}

		public DeviceBase Device => propDevice;

		public DeviceType DeviceType
		{
			get
			{
				return propDeviceType;
			}
			set
			{
				string name = pviLineObj.Name;
				switch (value)
				{
				case DeviceType.TcpIpMODBUS:
					pviLineObj.Initialize(1);
					break;
				case DeviceType.ANSLTcp:
					pviLineObj.Initialize(2);
					break;
				default:
					pviLineObj.Initialize(0);
					break;
				}
				propConnectionParameter = "";
				propDeviceType = value;
				switch (DeviceType)
				{
				case DeviceType.ANSLTcp:
					propDevice = tcpANSL;
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "ANSL";
					}
					break;
				case DeviceType.TcpIp:
					propDevice = tcpip;
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "TcpIp";
					}
					break;
				case DeviceType.TcpIpMODBUS:
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "TcpIpMODBUS";
					}
					pviLineObj.Initialize(1);
					propDevice = tcpipModBus;
					break;
				case DeviceType.Modem:
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "Modem";
					}
					propDevice = propINAModem;
					break;
				case DeviceType.Can:
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "Can";
					}
					propDevice = can;
					break;
				case DeviceType.Shared:
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "Shared";
					}
					propDevice = shared;
					break;
				default:
					if (!propDeviceNameHasBeenSet)
					{
						propDeviceName = "Com1";
					}
					propDevice = serial;
					break;
				}
				if (stationNameHasBeenSet)
				{
					pviLineObj.propName = name;
				}
				propDevice.Init();
			}
		}

		public Serial Serial
		{
			get
			{
				return serial;
			}
			set
			{
				ResetDevices(DeviceType.Serial);
				propDevice = (serial = value);
			}
		}

		public Modem Modem
		{
			get
			{
				return propINAModem;
			}
			set
			{
				ResetDevices(DeviceType.Modem);
				propDevice = (propINAModem = value);
			}
		}

		public Can Can
		{
			get
			{
				return can;
			}
			set
			{
				ResetDevices(DeviceType.Can);
				propDevice = (can = value);
			}
		}

		public AR000 AR000
		{
			get
			{
				return arSim;
			}
			set
			{
				ResetDevices(DeviceType.AR000);
				propDevice = (arSim = value);
			}
		}

		public TcpIp TcpIp
		{
			get
			{
				return tcpip;
			}
			set
			{
				ResetDevices(DeviceType.TcpIp);
				propDevice = (tcpip = value);
			}
		}

		public ANSLTcp ANSLTcp
		{
			get
			{
				return tcpANSL;
			}
			set
			{
				ResetDevices(DeviceType.ANSLTcp);
				propDevice = (tcpANSL = value);
			}
		}

		public Shared Shared
		{
			get
			{
				return shared;
			}
			set
			{
				ResetDevices(DeviceType.Shared);
				propDevice = (shared = value);
			}
		}

		public TcpIpMODBUS TcpIpMODBUS
		{
			get
			{
				return tcpipModBus;
			}
			set
			{
				ResetDevices(DeviceType.TcpIpMODBUS);
				propDevice = (tcpipModBus = value);
			}
		}

		public string ModuleInfoPath
		{
			get
			{
				return propModuleInfoPath;
			}
			set
			{
				propModuleInfoPath = value;
			}
		}

		public int ResponseTimeout
		{
			get
			{
				return propDevice.ResponseTimeout;
			}
			set
			{
				propDevice.ResponseTimeout = value;
			}
		}

		public string Routing
		{
			get
			{
				return propDevice.RoutingPath;
			}
			set
			{
				propDevice.RoutingPath = value;
			}
		}

		public override string FullName
		{
			get
			{
				if (base.Name.Length > 0)
				{
					if (Parent != null)
					{
						return Parent.FullName + "." + base.Name;
					}
					return base.Name;
				}
				if (Parent != null)
				{
					return Parent.FullName;
				}
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				string text = "";
				if (base.Name != null && 0 < base.Name.Length)
				{
					return pviStationObj.PviPathName + base.Name;
				}
				if (pviStationObj != null)
				{
					if (0 < propOldLineName.Length || 0 < propOldStationName.Length || 0 < propOldDeviceName.Length)
					{
						text = pviLineObj.PviPathName;
						if (0 < propOldLineName.Length)
						{
							text = pviLineObj.Parent.PviPathName + "/" + propOldLineName;
						}
						text = ((0 >= propOldDeviceName.Length) ? (text + "/" + pviDeviceObj.Name) : (text + "/" + propOldDeviceName));
						if (0 < propOldStationName.Length)
						{
							return text + "/" + propOldStationName;
						}
						return text + "/" + pviStationObj.Name;
					}
					return pviStationObj.PviPathName;
				}
				return "";
			}
		}

		internal static bool ConnectionChangeInProgress(ConnectionStates connState)
		{
			switch (connState)
			{
			case ConnectionStates.ConnectionChanging:
				return true;
			case ConnectionStates.ConnectionChanged:
				return true;
			default:
				return false;
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			writer.WriteStartElement("Connection");
			num = propDevice.ToXMLTextWriter(ref writer, flags);
			if (Routing != null && Routing.Length > 0)
			{
				writer.WriteAttributeString("Routing", Routing);
			}
			if (propDeviceName != null && propDeviceName.Length > 0)
			{
				writer.WriteAttributeString("DeviceName", propDeviceName);
			}
			if (propConnectionParameter != null && propConnectionParameter.Length > 0)
			{
				writer.WriteAttributeString("ConnectionParameter", propConnectionParameter);
			}
			writer.WriteEndElement();
			return num;
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				Base propParent = propParent;
				if (disposing)
				{
					pviLineObj.Connected -= pviLineObj_Connected;
					pviLineObj.Disconnected -= pviLineObj_Disconnected;
					pviDeviceObj.Connected -= pviDeviceObj_Connected;
					pviDeviceObj.Disconnected -= pviDeviceObj_Disconnected;
					pviStationObj.Connected -= pviStationObj_Connected;
					pviStationObj.Disconnected -= pviStationObj_Disconnected;
					pviStationObj.Dispose(disposing, removeFromCollection);
					pviDeviceObj.Dispose(disposing, removeFromCollection);
					pviLineObj.Dispose(disposing, removeFromCollection);
					pviStationObj = null;
					pviDeviceObj = null;
					pviLineObj = null;
					serial = null;
					can = null;
					tcpip = null;
					tcpipModBus = null;
					tcpANSL = null;
					shared = null;
					arSim = null;
					propINAModem = null;
					propDeviceType = DeviceType.Serial;
				}
				base.Dispose(disposing, removeFromCollection);
			}
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			string text = "";
			text = reader.GetAttribute("DeviceType");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "ansltcp":
					propDevice = tcpANSL;
					DeviceType = DeviceType.ANSLTcp;
					break;
				case "ar010":
				case "arwin":
				case "tcpip":
					propDevice = tcpip;
					DeviceType = DeviceType.TcpIp;
					break;
				case "tcpipmodbus":
					propDevice = tcpipModBus;
					DeviceType = DeviceType.TcpIpMODBUS;
					break;
				case "shared":
					propDevice = shared;
					DeviceType = DeviceType.Shared;
					break;
				case "serial":
					propDevice = serial;
					DeviceType = DeviceType.Serial;
					break;
				case "modem":
					propDevice = propINAModem;
					DeviceType = DeviceType.Modem;
					break;
				case "can":
					propDevice = can;
					DeviceType = DeviceType.Can;
					break;
				case "arsim":
				case "ar000":
					propDevice = arSim;
					DeviceType = DeviceType.AR000;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("Routing");
			if (text != null && text.Length > 0)
			{
				Routing = text;
			}
			text = "";
			text = reader.GetAttribute("DeviceName");
			if (text != null && text.Length > 0)
			{
				propDeviceName = text;
			}
			text = "";
			text = reader.GetAttribute("ConnectionParameter");
			if (text != null && text.Length > 0)
			{
				propConnectionParameter = text;
			}
			propDevice.FromXmlTextReader(ref reader, flags, propDevice);
			reader.Read();
			return 0;
		}

		public Connection(Service serviceObj)
			: base(serviceObj)
		{
			propLineNameHasBeenSet = false;
			propDeviceNameHasBeenSet = false;
			stationNameHasBeenSet = false;
			propOldDeviceName = "";
			propOldStationName = "";
			propOldLineName = "";
			propLineIsDirty = false;
			propDeviceIsDirty = false;
			propLineDesc = "";
			propDeviceDesc = "";
		}

		public Connection(Cpu cpu)
			: base(cpu)
		{
			propLineNameHasBeenSet = false;
			propDeviceNameHasBeenSet = false;
			stationNameHasBeenSet = false;
			propOldDeviceName = "";
			propOldStationName = "";
			propOldLineName = "";
			propDeviceIsDirty = false;
			propDeviceIsDirty = false;
			propLineDesc = "";
			propDeviceDesc = "";
			propCpu = cpu;
			serial = new Serial();
			can = new Can();
			tcpip = new TcpIp();
			tcpipModBus = new TcpIpMODBUS();
			shared = new Shared();
			arSim = new AR000();
			tcpANSL = new ANSLTcp();
			propINAModem = new Modem();
			propDeviceType = DeviceType.Serial;
			propDevice = serial;
			propDeviceName = "COM1";
			propConnectionParameter = "";
			propModuleInfoPath = "";
			pviLineObj = new PviLINE(cpu, "LNINA2");
			pviDeviceObj = new PviDEVICE(pviLineObj, null);
			pviStationObj = new PviSTATION(pviDeviceObj, "");
			pviLineObj.Connected += pviLineObj_Connected;
			pviLineObj.ConnectionChanged += Line_ConnectionChanged;
			pviLineObj.Disconnected += pviLineObj_Disconnected;
			pviDeviceObj.Connected += pviDeviceObj_Connected;
			pviDeviceObj.ConnectionChanged += Device_ConnectionChanged;
			pviDeviceObj.Disconnected += pviDeviceObj_Disconnected;
			pviStationObj.Connected += pviStationObj_Connected;
			pviStationObj.Disconnected += pviStationObj_Disconnected;
		}

		internal void ResetLinkIds()
		{
			pviLineObj.propLinkId = 0u;
			pviLineObj.propConnectionState = ConnectionStates.Unininitialized;
			pviDeviceObj.propLinkId = 0u;
			pviDeviceObj.propConnectionState = ConnectionStates.Unininitialized;
			pviStationObj.propLinkId = 0u;
			pviStationObj.propConnectionState = ConnectionStates.Unininitialized;
		}

		private void pviStationObj_Disconnected(object sender, PviEventArgs e)
		{
			pviDeviceObj.PviDisconnect();
		}

		private void pviStationObj_Connected(object sender, PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				OnError(new PviEventArgs(pviStationObj.Name, pviStationObj.Address, e.ErrorCode, Service.Language, Action.CpuConnect, Service));
			}
			OnConnected(new PviEventArgs(propName, propAddress, e.ErrorCode, Service.Language, Action.CpuConnect, Service));
		}

		private void pviDeviceObj_Disconnected(object sender, PviEventArgs e)
		{
			pviLineObj.PviDisconnect();
		}

		private void pviDeviceObj_Connected(object sender, PviEventArgs e)
		{
			int num = 0;
			if (e.ErrorCode != 0)
			{
				OnError(new PviEventArgs(pviDeviceObj.Name, pviDeviceObj.Address, e.ErrorCode, Service.Language, Action.DeviceConnect, Service));
				OnConnected(new PviEventArgs(propName, propAddress, e.ErrorCode, Service.Language, Action.CpuConnect, Service));
			}
			else
			{
				if (!Service.WaitForParentConnection)
				{
					return;
				}
				if (DeviceType < DeviceType.TcpIpMODBUS)
				{
					if (!stationNameHasBeenSet)
					{
						pviStationObj.SetName("PLC_" + pviDeviceObj.Name);
					}
					num = pviStationObj.PviConnect();
					if (num != 0)
					{
						OnConnected(new PviEventArgs(propName, propAddress, num, Service.Language, Action.StationConnect, Service));
					}
				}
				else
				{
					OnConnected(new PviEventArgs(propName, propAddress, e.ErrorCode, Service.Language, Action.CpuConnect, Service));
				}
			}
		}

		private void pviLineObj_Disconnected(object sender, PviEventArgs e)
		{
			OnDisconnected(new PviEventArgs(propName, propAddress, e.ErrorCode, Service.Language, Action.CpuDisconnect, Service));
		}

		private void pviLineObj_Connected(object sender, PviEventArgs e)
		{
			int num = 0;
			if (e.ErrorCode != 0)
			{
				OnError(new PviEventArgs(pviLineObj.Name, pviLineObj.Address, e.ErrorCode, Service.Language, Action.LineConnect, Service));
				OnConnected(new PviEventArgs(propName, propAddress, e.ErrorCode, Service.Language, Action.CpuConnect, Service));
			}
			else if (Service.WaitForParentConnection)
			{
				num = pviDeviceObj.PviConnect();
				if (num != 0)
				{
					OnConnected(new PviEventArgs(propName, propAddress, num, Service.Language, Action.DeviceConnect, Service));
				}
			}
		}

		private void CheckForParamterChanges()
		{
			if (pviLineObj.ObjectParam.CompareTo(propLineDesc) != 0)
			{
				propLineIsDirty = true;
			}
			if (pviDeviceObj.ObjectParam.CompareTo(propDeviceDesc) != 0)
			{
				propDeviceIsDirty = true;
			}
			propLineDesc = pviLineObj.ObjectParam;
			propDeviceDesc = pviDeviceObj.ObjectParam;
		}

		public override void Connect()
		{
			int errorCode = 0;
			propReturnValue = 0;
			UpdateDeviceParameter();
			propOldDeviceName = "";
			propOldStationName = "";
			propOldLineName = "";
			if (!propLineNameHasBeenSet)
			{
				propOldLineName = pviLineObj.Name;
			}
			if (!propDeviceNameHasBeenSet)
			{
				propOldDeviceName = pviDeviceObj.Name;
			}
			if (!stationNameHasBeenSet)
			{
				propOldStationName = pviStationObj.Name;
			}
			if (DeviceType.TcpIpMODBUS == DeviceType)
			{
				pviStationObj.propName = TcpIpMODBUS.DestinationIPAddress;
				pviStationObj.propName = pviStationObj.propName.Replace('.', '_');
			}
			propReturnValue = pviLineObj.PviConnect();
			if (propReturnValue != 0)
			{
				OnConnected(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.LineConnect, Service));
			}
			if (!Service.WaitForParentConnection)
			{
				propReturnValue = pviDeviceObj.PviConnect();
				if (propReturnValue != 0)
				{
					OnConnected(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.DeviceConnect, Service));
				}
				if (DeviceType < DeviceType.TcpIpMODBUS)
				{
					if (!stationNameHasBeenSet)
					{
						pviStationObj.SetName("PLC_" + pviDeviceObj.Name);
					}
					propReturnValue = pviStationObj.PviConnect();
					if (propReturnValue != 0)
					{
						OnConnected(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.StationConnect, Service));
					}
				}
			}
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.CpuConnect, Service));
			}
		}

		internal int TurnOffEvents()
		{
			int num = 0;
			num = pviLineObj.TurnOffEvents();
			num = pviDeviceObj.TurnOffEvents();
			return pviStationObj.TurnOffEvents();
		}

		internal int TurnOnEvents()
		{
			int num = 0;
			num = pviLineObj.TurnOnEvents();
			num = pviDeviceObj.TurnOnEvents();
			return pviStationObj.TurnOnEvents();
		}

		public override int ChangeConnection()
		{
			int num = 0;
			if (ConnectionChangeInProgress(propConnectionState))
			{
				return 15002;
			}
			UpdateDeviceParameter();
			num = ChangeToInvalidLineConnection();
			if (num != 0)
			{
				return num;
			}
			num = ChangeDeviceConnection();
			if (num != 0)
			{
				return num;
			}
			num = ChangeLineConnection();
			if (num != 0)
			{
				return num;
			}
			propDeviceIsDirty = false;
			OnConnectionChanged(0, Action.DeviceConnect);
			return num;
		}

		private int ChangeToInvalidLineConnection()
		{
			int result = 0;
			string text = "\"/LN=\"";
			if (propLineIsDirty)
			{
				if (pviLineObj.LinkId == 0)
				{
					return 12004;
				}
				IntPtr zero = IntPtr.Zero;
				zero = PviMarshal.StringToHGlobal(text);
				result = Write(Service.hPvi, pviLineObj.LinkId, AccessTypes.Connect, zero, text.Length);
				PviMarshal.FreeHGlobal(ref zero);
			}
			return result;
		}

		private int ChangeLineConnection()
		{
			int result = 0;
			string text = "";
			IntPtr zero = IntPtr.Zero;
			if (propLineIsDirty)
			{
				if (Cpu.propConnectionState == ConnectionStates.Unininitialized)
				{
					propConnectionState = ConnectionStates.Unininitialized;
					Cpu.propConnectionState = ConnectionStates.Unininitialized;
					Cpu.propLinkId = 0u;
					Cpu.Requests = Actions.NONE;
					Cpu.Connect();
				}
				else
				{
					if (ConnectionStates.ConnectedError == Cpu.propConnectionState)
					{
						Cpu.Requests = Actions.NONE;
						Cpu.Requests |= Actions.Connect;
						Cpu.Requests |= Actions.GetCpuInfo;
						Cpu.Requests |= Actions.GetLBType;
					}
					if (pviLineObj.LinkId == 0)
					{
						return 12004;
					}
					text = pviLineObj.ObjectParam;
					result = pviLineObj.ObjectParam.IndexOf("\"/\"");
					if (-1 != result)
					{
						text = text.Substring(result + 2);
					}
					else
					{
						result = pviLineObj.ObjectParam.IndexOf("CD=\"");
						if (-1 != result)
						{
							text = text.Substring(result + 3);
						}
					}
					zero = PviMarshal.StringToHGlobal(text);
					result = Write(Service.hPvi, pviLineObj.LinkId, AccessTypes.Connect, zero, text.Length);
					PviMarshal.FreeHGlobal(ref zero);
				}
			}
			propLineIsDirty = false;
			return result;
		}

		private int ChangeToInvalidDeviceConnection()
		{
			int num = 0;
			string text = "\"/IF=invalid \"";
			IntPtr zero = IntPtr.Zero;
			if (pviDeviceObj.LinkId == 0)
			{
				return 12004;
			}
			zero = PviMarshal.StringToHGlobal(text);
			num = Write(Service.hPvi, pviDeviceObj.LinkId, AccessTypes.Connect, zero, text.Length);
			PviMarshal.FreeHGlobal(ref zero);
			return num;
		}

		private int ChangeDeviceConnection()
		{
			int result = 0;
			string text = "";
			IntPtr zero = IntPtr.Zero;
			if (propDeviceIsDirty)
			{
				if (Cpu.propConnectionState == ConnectionStates.Unininitialized)
				{
					propConnectionState = ConnectionStates.Unininitialized;
					Cpu.propConnectionState = ConnectionStates.Unininitialized;
					Cpu.propLinkId = 0u;
					Cpu.Requests = Actions.NONE;
					Cpu.Connect();
				}
				else
				{
					if (ConnectionStates.ConnectedError == Cpu.propConnectionState)
					{
						Cpu.Requests = Actions.NONE;
						Cpu.Requests |= Actions.Connect;
						Cpu.Requests |= Actions.GetCpuInfo;
						Cpu.Requests |= Actions.GetLBType;
					}
					if (pviDeviceObj.LinkId == 0)
					{
						return 12004;
					}
					text = pviDeviceObj.ObjectParam;
					result = pviDeviceObj.ObjectParam.IndexOf("\"/\"");
					if (-1 != result)
					{
						text = text.Substring(result + 2);
					}
					else
					{
						result = pviDeviceObj.ObjectParam.IndexOf("CD=\"");
						if (-1 != result)
						{
							text = text.Substring(result + 3);
						}
					}
					zero = PviMarshal.StringToHGlobal(text);
					result = Write(Service.hPvi, pviDeviceObj.LinkId, AccessTypes.Connect, zero, text.Length);
					PviMarshal.FreeHGlobal(ref zero);
				}
			}
			propDeviceIsDirty = false;
			return result;
		}

		private void Line_ConnectionChanged(object sender, PviEventArgs e)
		{
			int num = 0;
			if (e.ErrorCode != 0)
			{
				OnError(e);
			}
			if (ConnectionStates.ConnectionChanging == propConnectionState)
			{
				propConnectionState = ConnectionStates.ConnectionChanged;
				OnConnectionChanged(e.ErrorCode, e.Action);
				return;
			}
			propConnectionState = ConnectionStates.ConnectionChanging;
			num = ChangeDeviceConnection();
			if (num != 0)
			{
				e.SetErrorCode(num);
				OnError(sender, e);
			}
		}

		private void Device_ConnectionChanged(object sender, PviEventArgs e)
		{
			OnConnectionChanged(e.ErrorCode, e.Action);
		}

		internal void DisconnectNoResponses()
		{
			propOldDeviceName = "";
			propOldStationName = "";
			propOldLineName = "";
			pviStationObj.Disconnect(noResponse: true);
			pviDeviceObj.Disconnect(noResponse: true);
			pviLineObj.Disconnect(noResponse: true);
			propConnectionState = ConnectionStates.Disconnected;
		}

		public override void Disconnect()
		{
			propOldDeviceName = "";
			propOldStationName = "";
			propOldLineName = "";
			if (ConnectionStates.Connected >= propConnectionState)
			{
				propConnectionState = ConnectionStates.Disconnecting;
				propReturnValue = pviStationObj.PviDisconnect();
			}
		}

		private void UpdateDeviceObjectName(string newDevObjName)
		{
			pviDeviceObj.propName = newDevObjName;
		}

		public void SynchronizeCommunicationParameters(CommSyncDirections syncDirection)
		{
			if (Cpu != null)
			{
				if (syncDirection == CommSyncDirections.ConnectionToCpu)
				{
					Cpu.SavePath = Device.SavePath;
					return;
				}
				Device.ResponseTimeout = Cpu.ResponseTimeout;
				Device.SavePath = Cpu.SavePath;
			}
		}

		private string getLineName()
		{
			if (!propLineNameHasBeenSet && 0 < propOldLineName.Length)
			{
				return propOldLineName;
			}
			return pviLineObj.Name;
		}

		private void UpdateDeviceParameterAnslTcp()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = $"TCPANSL";
			}
			if (ResponseTimeout > 0)
			{
				text = $" /RT={ResponseTimeout}";
			}
			if (0 < tcpANSL.CommunicationBufferSize)
			{
				text5 = $"/BSIZE={tcpANSL.CommunicationBufferSize}";
				text6 = $" /COMT={tcpANSL.CommunicationTimeout}";
			}
			else
			{
				text5 = "";
				text6 = $"/COMT={tcpANSL.CommunicationTimeout}";
			}
			text7 = "";
			if (0 < tcpANSL.SendDelay)
			{
				text7 = $" /SDT={tcpANSL.SendDelay}";
			}
			text8 = $" /IP={tcpANSL.DestinationIpAddress}";
			text4 = $" /PT={tcpANSL.RemotePort}";
			if (tcpANSL.SSLConfiguration != null && 0 < tcpANSL.SSLConfiguration.Length)
			{
				text9 = $" /SSL='{tcpANSL.SSLConfiguration}'";
			}
			if (0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=tcpip {Device.UnknownDeviceParameters}\"");
			}
			if (Routing != null && Routing.Length > 0)
			{
				text2 = $" /CN={Routing}";
			}
			if (ModuleInfoPath != null && ModuleInfoPath.Length > 0)
			{
				text3 = $" /SP='{ModuleInfoPath}'";
			}
			propConnectionParameter = "";
			if ((Routing != null && Routing.Length > 0) || (ModuleInfoPath != null && ModuleInfoPath.Length > 0) || ResponseTimeout > 0)
			{
				propConnectionParameter = $"{text5}{text6}{text7}{text8}{text4}{text2}{text3}{text}{text9}";
			}
			else
			{
				propConnectionParameter = $"{text5}{text6}{text7}{text8}{text4}{text9}";
			}
		}

		private void UpdateDeviceParameterSerial()
		{
			string arg = "";
			string arg2 = "";
			string arg3 = "";
			string text = "";
			string text2 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = $"COM{Serial.Channel.ToString()}";
			}
			Parity parity = Serial.Parity;
			if (ResponseTimeout > 0)
			{
				arg = $" /RT={ResponseTimeout}";
			}
			if (serial.IntervalTimeout > 0 && 20 != serial.IntervalTimeout)
			{
				text = $" /IT={serial.IntervalTimeout}";
			}
			if (FlowControls.NOT_SET != Serial.FlowControl)
			{
				text2 = $" /RS={(int)Serial.FlowControl}";
			}
			if (0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} /BD={Serial.BaudRate.ToString()} /PA={(int)parity}{text}{text2} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=COM{Serial.Channel.ToString()} /BD={Serial.BaudRate.ToString()} /PA={(int)parity}{text}{text2} {Device.UnknownDeviceParameters}\"");
			}
			if (Routing != null && Routing.Length > 0)
			{
				arg2 = $" /CN={Routing}";
			}
			if (ModuleInfoPath != null && ModuleInfoPath.Length > 0)
			{
				arg3 = $" /SP='{ModuleInfoPath}'";
			}
			propConnectionParameter = "";
			if ((Routing != null && Routing.Length > 0) || (ModuleInfoPath != null && ModuleInfoPath.Length > 0) || ResponseTimeout > 0)
			{
				propConnectionParameter = $"{arg2}{arg3}{arg}";
			}
		}

		private void UpdateDeviceParameterTcpIp()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			string text10 = "";
			string text11 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = "TCPIP";
			}
			if (tcpip.SourcePort > 0)
			{
				text4 = $" /LOPO={TcpIp.SourcePort}";
			}
			if (tcpip.LocalPort != 0)
			{
				text4 = $" /LOPO={TcpIp.LocalPort}";
			}
			if (tcpip.SourceStation > 0)
			{
				text5 = $" /SA={tcpip.SourceStation}";
			}
			if (tcpip.UniqueDeviceForSAandLOPO)
			{
				text6 = " /UDEV=1";
			}
			if (Device.InterfaceName != null && 0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName}{text5}{text4}{text6} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=tcpip{text5}{text4}{text6} {Device.UnknownDeviceParameters}\"");
			}
			if (Routing != null && Routing.Length > 0)
			{
				text2 = $" /CN={Routing}";
			}
			if (ModuleInfoPath != null && ModuleInfoPath.Length > 0)
			{
				text3 = $" /SP='{ModuleInfoPath}'";
			}
			text7 = ((tcpip.DestinationIpAddress == null || tcpip.DestinationIpAddress.Length <= 0) ? $" /CKDA={tcpip.CheckDestinationStation}" : $" /DAIP={tcpip.DestinationIpAddress}");
			if (tcpip.Target != null && tcpip.Target.Length > 0)
			{
				text8 = $" /TA={tcpip.Target}";
			}
			if (tcpip.ResponseTimeout > 0)
			{
				text = $" /RT={tcpip.ResponseTimeout}";
			}
			if (tcpip.DestinationPort > 0)
			{
				text9 = $" /REPO={tcpip.DestinationPort}";
			}
			if (tcpip.RemotePort != 0)
			{
				text9 = $" /REPO={tcpip.RemotePort}";
			}
			if (tcpip.DestinationStation > 0)
			{
				text10 = $"/DA={tcpip.DestinationStation}";
			}
			if (tcpip.QuickDownload != 1)
			{
				text11 = $" /ANSL={tcpip.QuickDownload}";
			}
			propConnectionParameter = $"{text10}{text2}{text3}{text7}{text8}{text}{text9}{text11}";
			propConnectionParameter = propConnectionParameter.Trim();
		}

		private void UpdateDeviceParameterModbus()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = "MBUSTCP";
			}
			pviDeviceObj.Inititialize(getLineName(), $"\"/IF=MBUSTCP {Device.UnknownDeviceParameters}\"");
			if (tcpipModBus.DestinationIPAddress != null && tcpipModBus.DestinationIPAddress.Length > 0)
			{
				text = $"/DAIP={tcpipModBus.DestinationIPAddress}";
			}
			else
			{
				propConnectionParameter = "";
			}
			if (tcpipModBus.PortNumber != 502)
			{
				text2 = $" /PN={tcpipModBus.PortNumber}";
			}
			if (tcpipModBus.UnitID != 255)
			{
				text3 = $" /DA={tcpipModBus.UnitID}";
			}
			if (tcpipModBus.ConnectionRetries > 0)
			{
				text4 = $" /CR={tcpipModBus.ConnectionRetries}";
			}
			propConnectionParameter = string.Format("{0}{1}{2}", text, text2, text3, text4);
		}

		private void UpdateDeviceParameterCan()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = "INACAN";
			}
			if (can.IntervalTimeout > 0)
			{
				text4 = $" /IT={can.IntervalTimeout}";
			}
			if (0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} /CNO={can.ControllerNumber} /BD={can.BaudRate} /BI={can.BaseId} /CT={can.CycleTime} /MC={can.MessageCount} /IO={can.IoPort} /IR={can.InterruptNumber} /SA={can.SourceAddress}{text4} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=INACAN{can.Channel} /CNO={can.ControllerNumber} /BD={can.BaudRate} /BI={can.BaseId} /CT={can.CycleTime} /MC={can.MessageCount} /IO={can.IoPort} /IR={can.InterruptNumber} /SA={can.SourceAddress}{text4} {Device.UnknownDeviceParameters}\"");
			}
			if (Routing != null && Routing.Length > 0)
			{
				text2 = $" /CN={Routing}";
			}
			if (ModuleInfoPath != null && ModuleInfoPath.Length > 0)
			{
				text3 = $" /SP='{ModuleInfoPath}'";
			}
			if (can.ResponseTimeout > 0)
			{
				text = $" /RT={can.ResponseTimeout}";
			}
			propConnectionParameter = $"/DA={can.DestinationAddress}{text2}{text3}{text}";
		}

		private void UpdateDeviceParameterLS251()
		{
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = $"LS251_{shared.Channel.ToString()}";
			}
			if (0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=LS251_{shared.Channel.ToString()} {Device.UnknownDeviceParameters}\"");
			}
		}

		private void UpdateDeviceParameterModem()
		{
			string arg = "";
			string arg2 = "";
			string arg3 = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = "modem" + propINAModem.CommunicationPort.ToString();
			}
			if (ResponseTimeout > 0)
			{
				arg = $" /RT={ResponseTimeout}";
			}
			if (0 < Device.InterfaceName.Length)
			{
				if (-1 == Modem.Redial)
				{
					pviDeviceObj.Inititialize(getLineName(), string.Format("\"/IF={0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", Device.InterfaceName, Modem.Modem, Modem.PhoneNumber, "INFINITE", Modem.RedialTimeout.ToString(), Modem.IntervalTimeout.ToString(), Device.UnknownDeviceParameters));
				}
				else
				{
					pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} /MO={Modem.Modem} /TN={Modem.PhoneNumber} /MR={Modem.Redial.ToString()} /RI={Modem.RedialTimeout.ToString()} /IT={Modem.IntervalTimeout.ToString()} {Device.UnknownDeviceParameters}\"");
				}
			}
			else if (-1 == Modem.Redial)
			{
				pviDeviceObj.Inititialize(getLineName(), string.Format("\"/IF=modem{0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", Modem.CommunicationPort, Modem.Modem, Modem.PhoneNumber, "INFINITE", Modem.RedialTimeout.ToString(), Modem.IntervalTimeout.ToString(), Device.UnknownDeviceParameters));
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=modem{Modem.CommunicationPort} /MO={Modem.Modem} /TN={Modem.PhoneNumber} /MR={Modem.Redial.ToString()} /RI={Modem.RedialTimeout.ToString()} /IT={Modem.IntervalTimeout.ToString()} {Device.UnknownDeviceParameters}\"");
			}
			if (Routing != null && Routing.Length > 0)
			{
				arg2 = $" /CN={Routing}";
			}
			if (ModuleInfoPath != null && ModuleInfoPath.Length > 0)
			{
				arg3 = $" /SP='{ModuleInfoPath}'";
			}
			if ((Routing != null && Routing.Length > 0) || (ModuleInfoPath != null && ModuleInfoPath.Length > 0) || ResponseTimeout > 0)
			{
				propConnectionParameter = $"{arg2}{arg3}{arg}";
			}
		}

		private void UpdateDeviceParameterArSim()
		{
			string arg = "";
			if (!propDeviceNameHasBeenSet)
			{
				propDeviceName = $"SPWIN";
			}
			if (arSim.ResponseTimeout > 0)
			{
				arg = $" /RT={arSim.ResponseTimeout}";
			}
			if (0 < Device.InterfaceName.Length)
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF={Device.InterfaceName} /SA={arSim.SourceAddress} {Device.UnknownDeviceParameters}\"");
			}
			else
			{
				pviDeviceObj.Inititialize(getLineName(), $"\"/IF=SPWIN /SA={arSim.SourceAddress} {Device.UnknownDeviceParameters}\"");
			}
			propConnectionParameter = $"/DA={arSim.DestinationAddress}{arg}";
		}

		internal void UpdateDeviceParameter()
		{
			switch (DeviceType)
			{
			case DeviceType.ANSLTcp:
				UpdateDeviceParameterAnslTcp();
				break;
			case DeviceType.Serial:
				UpdateDeviceParameterSerial();
				break;
			case DeviceType.TcpIp:
				UpdateDeviceParameterTcpIp();
				break;
			case DeviceType.TcpIpMODBUS:
				UpdateDeviceParameterModbus();
				break;
			case DeviceType.Can:
				UpdateDeviceParameterCan();
				break;
			case DeviceType.Shared:
				UpdateDeviceParameterLS251();
				break;
			case DeviceType.Modem:
				UpdateDeviceParameterModem();
				break;
			case DeviceType.AR000:
				UpdateDeviceParameterArSim();
				break;
			}
			if (0 < Device.SavePath.Length)
			{
				string text = propConnectionParameter;
				propConnectionParameter = text + " /SP='" + Device.SavePath + "' " + Device.UnknownCpuParameters;
			}
			else if (0 < Device.UnknownCpuParameters.Length)
			{
				propConnectionParameter = propConnectionParameter + " " + Device.UnknownCpuParameters;
			}
			UpdateDeviceObjectName(propDeviceName);
			if (DeviceType < DeviceType.TcpIpMODBUS)
			{
				pviStationObj.Initialize(pviDeviceObj.Name, "");
			}
			else
			{
				pviStationObj.Initialize(pviDeviceObj.Name, $"\"{ConnectionParameter}\"");
			}
			CheckForParamterChanges();
		}

		private void ParseDeviceParameters(string parameters)
		{
			int num = parameters.IndexOf("/IF=");
			if (-1 == num)
			{
				return;
			}
			string text = parameters.Substring(num + 4, 3);
			if (text.ToUpper().StartsWith("COM"))
			{
				DeviceType = DeviceType.Serial;
			}
			else if (text.ToUpper().Equals("TCP"))
			{
				if (DeviceType != DeviceType.TcpIp && DeviceType != DeviceType.ANSLTcp)
				{
					DeviceType = DeviceType.TcpIp;
				}
			}
			else if (text.ToUpper().Equals("ANS"))
			{
				DeviceType = DeviceType.ANSLTcp;
			}
			else if (text.ToUpper().Equals("CAN"))
			{
				DeviceType = DeviceType.Can;
			}
			else if (text.ToUpper().Equals("INA"))
			{
				DeviceType = DeviceType.Can;
			}
			else if (text.ToUpper().Equals("MOD"))
			{
				DeviceType = DeviceType.Modem;
			}
			else if (text.ToUpper().Equals("LS2"))
			{
				DeviceType = DeviceType.Shared;
			}
			else
			{
				DeviceType = DeviceType.Serial;
			}
			propDevice.UpdateDeviceParameters(parameters);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetLINEName(string name)
		{
			LineName = name;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public void SetDEVICEName(string name)
		{
			DeviceName = name;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetSTATIONName(string name)
		{
			StationName = name;
		}

		private void ParseConnectionParameters(string parameters)
		{
			int num = parameters.IndexOf("/IF=");
			if (-1 != num)
			{
				string text = parameters.Substring(num + 4, 3);
				if (text.ToLower().CompareTo("tcp") == 0)
				{
					DeviceType = DeviceType.TcpIp;
				}
				else if (text.ToLower().CompareTo("ansl") == 0)
				{
					DeviceType = DeviceType.ANSLTcp;
				}
				else if (text.ToLower().CompareTo("can") == 0)
				{
					DeviceType = DeviceType.Can;
				}
				else if (text.ToLower().CompareTo("mod") == 0)
				{
					DeviceType = DeviceType.Modem;
				}
				else
				{
					DeviceType = DeviceType.Serial;
				}
			}
			propDevice.UpdateCpuParameters(parameters);
		}

		private void ResetDevices(DeviceType type)
		{
			propDeviceType = type;
			propINAModem = null;
			can = null;
			serial = null;
			tcpip = null;
			tcpipModBus = null;
			snmpLine = null;
			arSim = null;
		}

		public override string ToString()
		{
			string text = "";
			switch (DeviceType)
			{
			case DeviceType.Can:
				return can.ToString();
			case DeviceType.Modem:
				return Modem.ToString();
			case DeviceType.TcpIp:
				return tcpip.ToString();
			case DeviceType.TcpIpMODBUS:
				return tcpipModBus.ToString();
			case DeviceType.ANSLTcp:
				return tcpANSL.ToString();
			default:
				return serial.ToString();
			}
		}
	}
}
