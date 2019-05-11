using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class RedundancyInformation
	{
		public RedundancyPriority RifModeSwitchPosition
		{
			get;
			set;
		}

		public RedundancyPriority CpuRedundancyPriotity
		{
			get;
			set;
		}

		public RedundancyState CpuRedundancyState
		{
			get;
			set;
		}

		public DateTime LongPushTimeStamp
		{
			get;
			set;
		}

		public DateTime ShortPushTimeStamp
		{
			get;
			set;
		}

		public RedundancySwitchPossibility CpuRedundancySwitchState
		{
			get;
			set;
		}

		public RedundancyLinkState CpuRedundancyLink
		{
			get;
			set;
		}

		public string ConfigurationId
		{
			get;
			set;
		}

		public RedundancyRRadMappingStates RRadMappingStates
		{
			get;
			set;
		}

		public RedundancyRRadStates RRadStates
		{
			get;
			set;
		}

		public RedundantCpuIpConfigurations IpConfigurations
		{
			get;
			set;
		}

		public bool SynchronisationDone
		{
			get;
			set;
		}

		public RedundancyInformation(string redundancyInformationXmlString)
		{
			XmlReader xmlReader = null;
			try
			{
				IpConfigurations = new RedundantCpuIpConfigurations();
				xmlReader = XmlReader.Create(new StringReader(redundancyInformationXmlString));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						switch (xmlReader.Name)
						{
						case "CpuRedInfo":
							ParseRootNode(xmlReader);
							break;
						case "PriIpConf":
							IpConfigurations.Primary = new List<IpAdressConfiguration>();
							AddAllConfiguration(xmlReader.ReadSubtree(), IpConfigurations.Primary);
							break;
						case "SecIpConf":
							IpConfigurations.Secundary = new List<IpAdressConfiguration>();
							AddAllConfiguration(xmlReader.ReadSubtree(), IpConfigurations.Secundary);
							break;
						case "CluIpConf":
							IpConfigurations.Cluster = new IpAdressConfiguration(xmlReader.ReadSubtree());
							break;
						case "ActIpConf":
							IpConfigurations.Active = new IpAdressConfiguration(xmlReader.ReadSubtree());
							break;
						case "InactIpConf":
							IpConfigurations.Inactive = new IpAdressConfiguration(xmlReader.ReadSubtree());
							break;
						case "LocalIp":
							IpConfigurations.Local = new IpAdressConfiguration(xmlReader.ReadSubtree());
							break;
						case "PartnerIp":
							IpConfigurations.Partner = new IpAdressConfiguration(xmlReader.ReadSubtree());
							break;
						}
					}
				}
				while (xmlReader.Read());
			}
			catch
			{
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		private static void AddAllConfiguration(XmlReader xmlTReader, List<IpAdressConfiguration> list)
		{
			xmlTReader.MoveToContent();
			while (xmlTReader.Read())
			{
				if (xmlTReader.HasAttributes)
				{
					list.Add(new IpAdressConfiguration(xmlTReader.ReadSubtree()));
				}
			}
		}

		private void ParseRootNode(XmlReader xmlTReader)
		{
			string attribute = xmlTReader.GetAttribute("RIFModeSwitch");
			if (!string.IsNullOrEmpty(attribute))
			{
				RifModeSwitchPosition = (RedundancyPriority)Enum.Parse(typeof(RedundancyPriority), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("LongPushTimeStamp");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					LongPushTimeStamp = DateTime.Parse(attribute);
				}
				catch
				{
				}
			}
			attribute = xmlTReader.GetAttribute("ShortPushTimeStamp");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					ShortPushTimeStamp = DateTime.Parse(attribute);
				}
				catch
				{
				}
			}
			attribute = xmlTReader.GetAttribute("CpuModeSwitch");
			if (!string.IsNullOrEmpty(attribute))
			{
				CpuRedundancyPriotity = (RedundancyPriority)Enum.Parse(typeof(RedundancyPriority), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("CpuProcessCtrlState");
			if (!string.IsNullOrEmpty(attribute))
			{
				CpuRedundancyState = (RedundancyState)Enum.Parse(typeof(RedundancyState), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("SwitchoverLevel");
			if (!string.IsNullOrEmpty(attribute))
			{
				CpuRedundancySwitchState = (RedundancySwitchPossibility)Enum.Parse(typeof(RedundancySwitchPossibility), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("RIFLinkActive");
			if (!string.IsNullOrEmpty(attribute))
			{
				CpuRedundancyLink = (RedundancyLinkState)Enum.Parse(typeof(RedundancyLinkState), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("ProjectName");
			if (!string.IsNullOrEmpty(attribute))
			{
				ConfigurationId = attribute;
			}
			attribute = xmlTReader.GetAttribute("RRADMapping");
			if (!string.IsNullOrEmpty(attribute))
			{
				RRadMappingStates = (RedundancyRRadMappingStates)Enum.Parse(typeof(RedundancyRRadMappingStates), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("RRADState");
			if (!string.IsNullOrEmpty(attribute))
			{
				RRadStates = (RedundancyRRadStates)Enum.Parse(typeof(RedundancyRRadStates), attribute, ignoreCase: true);
			}
			attribute = xmlTReader.GetAttribute("CpuInSync");
			if (!string.IsNullOrEmpty(attribute))
			{
				SynchronisationDone = (int.Parse(attribute) == 1);
			}
			else
			{
				SynchronisationDone = true;
			}
		}
	}
}
