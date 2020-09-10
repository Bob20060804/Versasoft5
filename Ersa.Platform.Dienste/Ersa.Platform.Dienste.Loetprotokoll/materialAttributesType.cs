using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	[XmlInclude(typeof(materialLotType))]
	[XmlInclude(typeof(materialType))]
	public class materialAttributesType
	{
		[XmlAttribute]
		public string type
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

		[XmlAttribute]
		public string position
		{
			get;
			set;
		}

		[XmlAttribute]
		public string assemblyPosition
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
		public double scrapQuantity
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool scrapQuantitySpecified
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
		public string state
		{
			get;
			set;
		}
	}
}
