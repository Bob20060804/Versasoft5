using System;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class BondInformation
	{
		public string Name
		{
			get;
			set;
		}

		public int EventID
		{
			get;
			set;
		}

		public string ActiveDevice
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint SwitchCount
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint ErrorTimeStampSeconds
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint ErrorTimeStampNanoSeconds
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint PrimaryLinkStatus
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint PrimaryLinkErrorCount
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint SecondaryLinkStatus
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint SecondaryLinkErrorCount
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint ArpMonitorPeriod
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint NumberArpTargets
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint NumberArpReachedTargets
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint PrimaryArpErrorCount
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		public uint SecondaryArpErrorCount
		{
			get;
			set;
		}

		public BondInformation()
		{
		}

		internal BondInformation(string bondInfoXmlString)
		{
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(bondInfoXmlString));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						switch (xmlReader.Name)
						{
						case "EthBondEvent":
							AddEthBondInfo(xmlReader.ReadSubtree());
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

		private void AddEthBondInfo(XmlReader xmlTReader)
		{
			xmlTReader.MoveToContent();
			if (xmlTReader.Read() && xmlTReader.HasAttributes)
			{
				string attribute = xmlTReader.GetAttribute("BondName");
				if (!string.IsNullOrEmpty(attribute))
				{
					Name = attribute;
				}
				attribute = xmlTReader.GetAttribute("EventID");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						EventID = int.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("ActiveDevice");
				if (!string.IsNullOrEmpty(attribute))
				{
					ActiveDevice = attribute;
				}
				attribute = xmlTReader.GetAttribute("PrimaryLinkStatus");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						PrimaryLinkStatus = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("SecondaryLinkStatus");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						SecondaryLinkStatus = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("NumberArpReachedTargets");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						NumberArpReachedTargets = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("SwitchCount");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						SwitchCount = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("PrimaryLinkErrorCount");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						PrimaryLinkErrorCount = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("SecondaryLinkErrorCount");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						SecondaryLinkErrorCount = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("PrimaryArpErrorCount");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						PrimaryArpErrorCount = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("SecondaryArpErrorCount");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						SecondaryArpErrorCount = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("ErrorTimeStampSeconds");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						ErrorTimeStampSeconds = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("ErrorTimeStampNanoSeconds");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						ErrorTimeStampNanoSeconds = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("ArpMonitorPeriod");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						ArpMonitorPeriod = uint.Parse(attribute);
					}
					catch
					{
					}
				}
				attribute = xmlTReader.GetAttribute("NumberArpTargets");
				if (!string.IsNullOrEmpty(attribute))
				{
					try
					{
						NumberArpTargets = uint.Parse(attribute);
					}
					catch
					{
					}
				}
			}
		}
	}
}
