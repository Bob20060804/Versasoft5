namespace BR.AN.PviServices
{
	public class TcpDestinationSettings
	{
		private string propStringData;

		private string propDestinationIPAddress;

		private int propRemotePortNumber;

		private int propDestinationStationNumber;

		private string propRoutingInformation;

		public string DestinationIPAddress => propDestinationIPAddress;

		public int RemotePortNumber => propRemotePortNumber;

		public int DestinationStationNumber => propDestinationStationNumber;

		public string RoutingInformation => propRoutingInformation;

		public TcpDestinationSettings()
		{
			Init();
		}

		private void Init()
		{
			propStringData = "";
			propDestinationIPAddress = "";
			propRemotePortNumber = 0;
			propDestinationStationNumber = 0;
			propRoutingInformation = "";
		}

		internal void Parse(string strData)
		{
			int num = 0;
			Init();
			if (0 >= strData.Length)
			{
				return;
			}
			propStringData = strData;
			string[] array = strData.Split(' ');
			for (num = 0; num < array.Length; num++)
			{
				string strConnection = array.GetValue(num).ToString();
				if (!DeviceBase.UpdateParameterFromString("/DAIP=", strConnection, ref propDestinationIPAddress) && !DeviceBase.UpdateParameterFromString("/IP=", strConnection, ref propDestinationIPAddress) && !DeviceBase.UpdateParameterFromString("/REPO=", strConnection, ref propRemotePortNumber) && !DeviceBase.UpdateParameterFromString("/PT=", strConnection, ref propRemotePortNumber) && !DeviceBase.UpdateParameterFromString("/DA=", strConnection, ref propDestinationStationNumber))
				{
					DeviceBase.UpdateParameterFromString("/CN=", strConnection, ref propRoutingInformation);
				}
			}
		}

		public override string ToString()
		{
			return propStringData;
		}
	}
}
