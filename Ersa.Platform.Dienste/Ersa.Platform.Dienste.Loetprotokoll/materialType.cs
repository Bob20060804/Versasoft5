using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class materialType : materialAttributesType
	{
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
