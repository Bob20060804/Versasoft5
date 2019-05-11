using System;

namespace BR.AN.PviServices
{
	public class NetworkAdapter : SNMPBase
	{
		private SNMPVariableCollection propVariables;

		internal SNMPConnectionStates propState;

		private string propMACAddress;

		public SNMPVariableCollection Variables => propVariables;

		public SNMPConnectionStates State => propState;

		public string MACAddress => propMACAddress;

		internal NetworkAdapter(string macAdr, SNMPBase parentObj)
			: base(macAdr, parentObj)
		{
			propState = SNMPConnectionStates.Connected;
			propMACAddress = macAdr;
			propVariables = new SNMPVariableCollection("SNMPLocal_" + base.Name, this);
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.Variables)
			{
				if (errorCode == 0)
				{
					OnLocalVariablesUpdate(pData, dataLen);
				}
				else
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", errorCode, base.Service.Language, Action.SNMPListLocalVariables));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private void OnLocalVariablesUpdate(IntPtr pData, uint dataLen)
		{
			int num = 0;
			if (propVariables != null)
			{
				propVariables.Cleanup();
				propVariables.InitConnect(this);
			}
			if (base.Service != null)
			{
				string text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				string text2 = "";
				for (num = 0; num < dataLen; num++)
				{
					if ('\t' != text[num] && text[num] != 0)
					{
						text2 += text[num];
						continue;
					}
					Variable value = new Variable(this, text2);
					propVariables.Add(text2, value);
					text2 = "";
				}
				OnSearchCompleted(new ErrorEventArgs(base.Name, "", 0, base.Service.Language, Action.SNMPListLocalVariables));
			}
			else
			{
				OnSearchCompleted(new ErrorEventArgs(base.Name, "", 4808, "en", Action.SNMPListLocalVariables));
			}
		}

		public int Search()
		{
			int num = -1;
			propVariables.InitConnect(this);
			if (propParent != null)
			{
				string objDesc = "CD=\"" + propParent.Name + "\"/\"/CN=" + MACAddress + "\"";
				if (!propIsConnected)
				{
					num = ConnectPviObject(withEvents: true, propMACAddress, objDesc, "", ObjectType.POBJ_STATION, 1403, out propLinkID);
				}
				if (num == 0 || 12002 == num)
				{
					num = GetSNMPVariables((int)propLinkID, 1402);
				}
			}
			else
			{
				num = -2;
			}
			return num;
		}

		public override void Cleanup()
		{
			if (propVariables != null)
			{
				propVariables.Cleanup();
			}
			if (base.Service != null)
			{
				PInvokePvicom.PviComUnlinkRequest(base.Service.hPvi, propLinkID, null, 0u, 0u);
			}
			base.Cleanup();
		}

		public override int Disconnect(bool synchronous)
		{
			if (!synchronous)
			{
				return base.Disconnect(synchronous);
			}
			propIsConnected = false;
			return UnlinkPviObject(propLinkID);
		}
	}
}
