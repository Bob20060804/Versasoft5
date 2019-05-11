using System.Net;
using System.Xml;

namespace BR.AN.PviServices
{
	public class IpAdressConfiguration
	{
		public string DeviceName
		{
			get;
			set;
		}

		public IPAddress IpAdress
		{
			get;
			set;
		}

		public IPAddress SubnetMask
		{
			get;
			set;
		}

		public string HostName
		{
			get;
			set;
		}

		public int Baudrate
		{
			get;
			set;
		}

		public int AnslPort
		{
			get;
			set;
		}

		public IPAddress ClusterIpAdress
		{
			get;
			set;
		}

		public IpAdressConfiguration(XmlReader xmlTReader)
		{
			bool flag = true;
			while (xmlTReader.AttributeCount == 0 && flag)
			{
				flag = xmlTReader.Read();
			}
			string attribute = xmlTReader.GetAttribute("Name");
			if (!string.IsNullOrEmpty(attribute))
			{
				DeviceName = attribute;
			}
			attribute = xmlTReader.GetAttribute("Addr");
			if (!string.IsNullOrEmpty(attribute))
			{
				IpAdress = IPAddress.Parse(attribute);
			}
			attribute = xmlTReader.GetAttribute("SubNetMask");
			if (!string.IsNullOrEmpty(attribute))
			{
				SubnetMask = IPAddress.Parse(attribute);
			}
			attribute = xmlTReader.GetAttribute("HostName");
			if (!string.IsNullOrEmpty(attribute))
			{
				HostName = attribute;
			}
			attribute = xmlTReader.GetAttribute("BaudRate");
			if (!string.IsNullOrEmpty(attribute))
			{
				Baudrate = int.Parse(attribute);
			}
			attribute = xmlTReader.GetAttribute("AnslPort");
			if (!string.IsNullOrEmpty(attribute))
			{
				AnslPort = int.Parse(attribute);
			}
			attribute = xmlTReader.GetAttribute("CluAddr");
			if (!string.IsNullOrEmpty(attribute))
			{
				ClusterIpAdress = IPAddress.Parse(attribute);
			}
		}
	}
}
