using System.Xml;

namespace BR.AN.PviServices
{
	public class HardwareInformationNode
	{
		public string Name
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public HardwareInformationNode(XmlReader xmlTReader)
		{
			bool flag = true;
			while (xmlTReader.AttributeCount == 0 && flag)
			{
				flag = xmlTReader.Read();
			}
			string attribute = xmlTReader.GetAttribute("Name");
			if (!string.IsNullOrEmpty(attribute))
			{
				Name = attribute;
			}
			attribute = xmlTReader.GetAttribute("Path");
			if (!string.IsNullOrEmpty(attribute))
			{
				Path = attribute;
			}
			attribute = xmlTReader.GetAttribute("State");
			if (!string.IsNullOrEmpty(attribute))
			{
				State = attribute;
			}
		}
	}
}
