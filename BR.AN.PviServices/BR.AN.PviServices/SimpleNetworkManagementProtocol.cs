using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	public class SimpleNetworkManagementProtocol : SNMPBase
	{
		private uint propLineLinkID;

		private string propLineName;

		private string propDeviceName;

		private int propResponseTimeout;

		private NetworkAdapterCollection propNetworkAdapters;

		private SNMPVariableCollection propVariables;

		[PviKeyWord("/RT")]
		public int ResponseTimeout
		{
			get
			{
				return propResponseTimeout;
			}
			set
			{
				int propResponseTimeout2 = propResponseTimeout;
				propResponseTimeout = value;
			}
		}

		public NetworkAdapterCollection NetworkAdapters => propNetworkAdapters;

		public SNMPVariableCollection Variables => propVariables;

		internal SimpleNetworkManagementProtocol(Service serviceObj)
			: base("snmp", serviceObj)
		{
			propLineName = "LnSNMP";
			propDeviceName = "snmp";
			propResponseTimeout = 1000;
			propNetworkAdapters = new NetworkAdapterCollection("NetworkAdapters", this);
			propVariables = new SNMPVariableCollection("SNMPGlobal_" + base.Name, this);
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteStartElement("SNMP");
			writer.WriteAttributeString("LINE", propLineName);
			writer.WriteAttributeString("DEVICE", propDeviceName);
			writer.WriteAttributeString("ResponseTimeout", propResponseTimeout.ToString());
			writer.WriteEndElement();
			return 0;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags)
		{
			string text = "";
			text = reader.GetAttribute("LINE");
			if (text != null && text.Length > 0)
			{
				propLineName = text;
			}
			text = "";
			text = reader.GetAttribute("DEVICE");
			if (text != null && text.Length > 0)
			{
				propDeviceName = text;
			}
			text = "";
			text = reader.GetAttribute("ResponseTimeout");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					propResponseTimeout = result;
				}
			}
			return 0;
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			switch (accessType)
			{
			case PVIReadAccessTypes.ChildTypes:
				if (errorCode != 0)
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", errorCode, base.Service.Language, Action.SNMPListStations));
				}
				else
				{
					OnStationsListUpdate(pData, dataLen);
				}
				break;
			case PVIReadAccessTypes.Variables:
				if (errorCode != 0)
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", errorCode, base.Service.Language, Action.SNMPListGlobalVariables));
				}
				else
				{
					OnGlobalVariablesUpdate(pData, dataLen);
				}
				break;
			default:
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
				break;
			}
		}

		private void OnStationsListUpdate(IntPtr pData, uint dataLen)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (propNetworkAdapters != null)
			{
				propNetworkAdapters.InitConnect(this);
			}
			Hashtable hashtable = new Hashtable();
			string text = PviMarshal.PtrToStringAnsi(pData, dataLen);
			string text2 = "";
			for (num2 = 0; num2 < dataLen; num2++)
			{
				if ('\t' != text[num2] && text[num2] != 0)
				{
					text2 += text[num2];
					continue;
				}
				num = text2.IndexOf("OT=Station");
				if (-1 != num)
				{
					string text3 = text2.Substring(0, num - 1);
					if (!hashtable.ContainsKey(text3))
					{
						hashtable.Add(text3, text3);
					}
					if (!propNetworkAdapters.ContainsKey(text3))
					{
						NetworkAdapter value = new NetworkAdapter(text3, this);
						propNetworkAdapters.Add(text3, value);
					}
					else
					{
						propNetworkAdapters[text3].propState = SNMPConnectionStates.Connected;
					}
				}
				text2 = "";
			}
			if (propNetworkAdapters != null && 0 < propNetworkAdapters.Count)
			{
				foreach (NetworkAdapter value2 in propNetworkAdapters.Values)
				{
					if (!hashtable.ContainsKey(value2.Name))
					{
						value2.propState = SNMPConnectionStates.Unpluged;
					}
				}
			}
			if (Actions.ListSNMPVariables == (propRequestQueue & Actions.ListSNMPVariables))
			{
				propRequestQueue ^= Actions.ListSNMPVariables;
				num3 = GetSNMPVariables((int)propLinkID, 1401);
				if (num3 != 0)
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", num3, base.Service.Language, Action.SNMPListGlobalVariables, "global SNMP variables"));
				}
			}
			else
			{
				OnSearchCompleted(new ErrorEventArgs(base.Name, "", 0, base.Service.Language, Action.SNMPListGlobalVariables));
			}
		}

		private void OnGlobalVariablesUpdate(IntPtr pData, uint dataLen)
		{
			int num = 0;
			if (propVariables != null)
			{
				propVariables.Cleanup();
				propVariables.InitConnect(this);
			}
			string text = PviMarshal.PtrToStringAnsi(pData, dataLen);
			string text2 = "";
			for (num = 0; num < dataLen; num++)
			{
				if ('\t' != text[num] && text[num] != 0)
				{
					text2 += text[num];
					continue;
				}
				Variable variable = new Variable(this, text2);
				if (text2.CompareTo("MacAddresses") == 0)
				{
					variable.RefreshTime = 20 * propResponseTimeout;
				}
				propVariables.Add(text2, variable);
				text2 = "";
			}
			OnSearchCompleted(new ErrorEventArgs(base.Name, "", 0, base.Service.Language, Action.SNMPListGlobalVariables));
		}

		public int Search()
		{
			int num = 0;
			if (propIsConnected)
			{
				num = GetMACStations();
				if (num == 0)
				{
					propRequestQueue |= Actions.ListSNMPVariables;
				}
			}
			else
			{
				num = ConnectPviObjects();
				if (num == 0 || 12002 == num)
				{
					propIsConnected = true;
					num = GetMACStations();
					if (num == 0)
					{
						propRequestQueue |= Actions.ListSNMPVariables;
					}
				}
			}
			return num;
		}

		internal override int Connect()
		{
			int num = 0;
			propNetworkAdapters.InitConnect(this);
			propVariables.InitConnect(this);
			num = ConnectPviObjects();
			if (num == 0 || 12002 == num)
			{
				propIsConnected = true;
			}
			return num;
		}

		private int ConnectPviObjects()
		{
			string objDesc = "CD=\"Pvi\"/\"" + propLineName + "\"";
			int num = 0;
			num = ConnectPviObject(withEvents: true, propLineName, objDesc, "", ObjectType.POBJ_LINE, 1403, out propLineLinkID);
			if (num == 0 || 12002 == num)
			{
				string objDesc2 = $"CD=\"{propLineName}\"/\"/IF={propDeviceName} /RT={propResponseTimeout}\"";
				num = ConnectPviObject(withEvents: true, propDeviceName, objDesc2, "", ObjectType.POBJ_DEVICE, 1404, out propLinkID);
			}
			return num;
		}

		private int GetMACStations()
		{
			int num = 0;
			if (base.Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComReadRequest(base.Service.hPvi, propLinkID, AccessTypes.ListExtern, base.Service.cbRead, 4294967294u, propServiceArrayIndex);
			}
			return PInvokePvicom.PviComMsgReadRequest(base.Service.hPvi, propLinkID, AccessTypes.ListExtern, base.Service.WindowHandle, 1400u, propServiceArrayIndex);
		}

		public override void Cleanup()
		{
			if (propVariables != null)
			{
				propVariables.Cleanup();
			}
			if (propNetworkAdapters != null)
			{
				propNetworkAdapters.Cleanup();
			}
			CancelRequest();
			UnlinkPviObject(propLineLinkID);
			propIsConnected = false;
			base.Cleanup();
		}

		public override string ToString()
		{
			return "LINE=\"" + propLineName + "\" DEVICE=\"" + propDeviceName + "\" ResponseTimeout=\"" + propResponseTimeout.ToString() + "\"";
		}
	}
}
