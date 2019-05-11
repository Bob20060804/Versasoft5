using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class NetworkAdapterCollection : SNMPCollectionBase
	{
		private Variable propMACAddresses;

		private int propRefreshTime;

		public int RefreshTime
		{
			get
			{
				return propRefreshTime;
			}
			set
			{
				propRefreshTime = value;
				if (propParent == null || base.Service == null)
				{
					return;
				}
				if (value != 0 && 2 * ((SimpleNetworkManagementProtocol)propParent).ResponseTimeout > value)
				{
					propRefreshTime = 2 * ((SimpleNetworkManagementProtocol)propParent).ResponseTimeout;
				}
				if (ConnectionStates.Connected == propMACAddresses.propConnectionState)
				{
					propMACAddresses.RefreshTime = propRefreshTime;
					return;
				}
				propMACAddresses.propRefreshTime = propRefreshTime;
				if (0 < propRefreshTime)
				{
					if (!base.Service.SNMP.propIsConnected)
					{
						base.Service.SNMP.Connect();
					}
					propMACAddresses.Connect();
				}
			}
		}

		public NetworkAdapter this[string indexer]
		{
			get
			{
				return (NetworkAdapter)propItems[indexer];
			}
		}

		internal NetworkAdapterCollection(string name, SimpleNetworkManagementProtocol parentObj)
			: base(name, parentObj)
		{
			int retVal = 0;
			propMACAddresses = new Variable(parentObj, "MacAddresses");
			propMACAddresses.propVariableAccess = Access.Read;
			propMACAddresses.propRefreshTime = 0;
			propMACAddresses.UpdateDataFormat("VT=string VL=1024 VN=1", Action.NONE, 0, initOnly: true, ref retVal);
			propMACAddresses.Value.TypePreset = true;
			propMACAddresses.propActive = true;
			propMACAddresses.ValueChanged += MACAddresses_ValueChanged;
			propRefreshTime = 0;
		}

		private void MACAddresses_ValueChanged(object sender, VariableEventArgs e)
		{
			int retVal = 0;
			if (base.Service == null)
			{
				return;
			}
			CollectionErrorEventArgs collectionErrorEventArgs = new CollectionErrorEventArgs(base.Name, e.ErrorCode, base.Service.Language, Action.SNMPListStations);
			if (e.ErrorCode == 0)
			{
				string text = ((Variable)sender).Value.ToString();
				if (text.Length >= ((Variable)sender).Value.DataSize)
				{
					propMACAddresses.Value.TypePreset = true;
					propMACAddresses.propPviValue.propTypeLength = 1024 + ((Variable)sender).Value.DataSize;
					propMACAddresses.UpdateDataFormat("VT=string VL=" + propMACAddresses.propPviValue.propTypeLength.ToString() + " VN=1", Action.VariableInternFormat, 0, initOnly: false, createNew: true, ref retVal);
					return;
				}
				text = text.Trim();
				Hashtable hashtable = new Hashtable();
				if (0 < text.Length)
				{
					string[] array = text.Split('\t');
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = array.GetValue(i).ToString();
						if (!ContainsKey(text2))
						{
							hashtable.Add(text2, text2);
							NetworkAdapter value = new NetworkAdapter(text2, base.Parent);
							collectionErrorEventArgs.NewItems.Add(text2);
							Add(text2, value);
						}
						else
						{
							this[text2].propState = SNMPConnectionStates.Connected;
						}
					}
				}
				if (0 < base.Count)
				{
					foreach (NetworkAdapter value2 in base.Values)
					{
						if (!hashtable.ContainsKey(value2.Name))
						{
							value2.propState = SNMPConnectionStates.Unpluged;
							collectionErrorEventArgs.ChangedItems.Add(value2.Name);
						}
					}
				}
			}
			OnChanged(collectionErrorEventArgs);
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.SNMPListLocalVariables)
			{
				if (errorCode != 0)
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", errorCode, base.Service.Language, Action.SNMPListLocalVariables));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		public int Search()
		{
			int num = 0;
			if (propRequesting)
			{
				return -1;
			}
			propRequestCount = 0;
			if (0 < base.Count)
			{
				foreach (NetworkAdapter value in base.Values)
				{
					propRequestCount++;
					value.SearchCompleted += Adapter_SearchCompleted;
					num = value.Search();
					if (num != 0)
					{
						propRequestCount--;
						value.SearchCompleted -= Adapter_SearchCompleted;
						OnError(new ErrorEventArgs(base.Name, "", num, base.Service.Language, Action.SNMPListLocalVariables, value.MACAddress));
						num = 0;
					}
				}
			}
			if (propRequestCount == 0)
			{
				OnSearchCompleted(new ErrorEventArgs(base.Name, "", 0, base.Service.Language, Action.SNMPListLocalVariables));
			}
			return num;
		}

		private void Adapter_SearchCompleted(object sender, ErrorEventArgs e)
		{
			((NetworkAdapter)sender).SearchCompleted -= Adapter_SearchCompleted;
			propRequestCount--;
			if (e.ErrorCode != 0)
			{
				OnError(new ErrorEventArgs(base.Name, "", e.ErrorCode, base.Service.Language, e.Action, ((NetworkAdapter)sender).MACAddress));
			}
			if (propRequestCount == 0)
			{
				propRequesting = false;
				OnSearchCompleted(new ErrorEventArgs(base.Name, "", e.ErrorCode, base.Service.Language, e.Action));
			}
		}

		public override void Cleanup()
		{
			if (0 < base.Count)
			{
				foreach (NetworkAdapter value in base.Values)
				{
					value.Cleanup();
				}
			}
			base.Cleanup();
		}
	}
}
