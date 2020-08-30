using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class materialLotType : materialAttributesType
	{
		[XmlElement("additionalId")]
		public additionalIdType[] additionalId
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialLot
		{
			get;
			set;
		}

		[XmlAttribute]
		public string material
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialVersion
		{
			get;
			set;
		}
	}
}
