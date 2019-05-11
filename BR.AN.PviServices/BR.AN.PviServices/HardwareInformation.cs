using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class HardwareInformation : List<HardwareInformationNode>
	{
		public HardwareInformation(string hardwareInformationXmlString)
		{
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(hardwareInformationXmlString));
				xmlReader.MoveToContent();
				do
				{
					string name;
					if (xmlReader.NodeType == XmlNodeType.Element && (name = xmlReader.Name) != null && name == "Node")
					{
						Add(new HardwareInformationNode(xmlReader.ReadSubtree()));
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
	}
}
