using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class measuringDefinitionType
	{
		[XmlElement("channelDefinition")]
		public channelDefinitionType[] channelDefinition
		{
			get;
			set;
		}

		[XmlAttribute]
		public string equipment
		{
			get;
			set;
		}
	}
}
