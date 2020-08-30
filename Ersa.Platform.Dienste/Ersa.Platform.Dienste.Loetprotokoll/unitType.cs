using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class unitType
	{
		[XmlElement("additionalId")]
		public additionalIdType[] additionalId
		{
			get;
			set;
		}

		[XmlAttribute]
		public string unit
		{
			get;
			set;
		}

		[XmlAttribute("unitType")]
		public string unitType1
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
		public string materialVersion
		{
			get;
			set;
		}

		[XmlAttribute]
		public string materialVariant
		{
			get;
			set;
		}

		[XmlAttribute]
		public string state
		{
			get;
			set;
		}

		[XmlAttribute]
		public string processingState
		{
			get;
			set;
		}
	}
}
