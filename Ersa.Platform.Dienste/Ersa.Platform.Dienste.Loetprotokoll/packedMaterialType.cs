using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class packedMaterialType
	{
		public propertiesType properties
		{
			get;
			set;
		}

		[XmlElement("additionalId")]
		public additionalIdType[] additionalId
		{
			get;
			set;
		}

		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string type
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
		public double quantity
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool quantitySpecified
		{
			get;
			set;
		}

		[XmlAttribute]
		public string UnitOfMeasure
		{
			get;
			set;
		}

		[XmlAttribute]
		public string description
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
