using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class MemoryInformation
	{
		public List<Drive> Drives
		{
			get;
			set;
		}

		public List<MemoryInfo> Memories
		{
			get;
			set;
		}

		public MemoryInformation(string memoryInformationXmlString)
		{
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(memoryInformationXmlString));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						switch (xmlReader.Name)
						{
						case "Drive":
							ParseDrivesList(xmlReader.ReadSubtree());
							break;
						case "Memory":
							ParseMemoriesList(xmlReader.ReadSubtree());
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

		private void ParseDrivesList(XmlReader xmlTReader)
		{
			Drives = new List<Drive>();
			xmlTReader.MoveToContent();
			while (xmlTReader.Read())
			{
				if (xmlTReader.HasAttributes)
				{
					Drives.Add(new Drive(xmlTReader.ReadSubtree()));
				}
			}
		}

		private void ParseMemoriesList(XmlReader xmlTReader)
		{
			Memories = new List<MemoryInfo>();
			xmlTReader.MoveToContent();
			while (xmlTReader.Read())
			{
				if (xmlTReader.HasAttributes)
				{
					Memories.Add(new MemoryInfo(xmlTReader.ReadSubtree()));
				}
			}
		}
	}
}
