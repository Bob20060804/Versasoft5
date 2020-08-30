using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class handlingUnitType
	{
		[XmlElement("packedMaterial")]
		public packedMaterialType[] packedMaterial
		{
			get;
			set;
		}

		[XmlElement("handlingUnit")]
		public handlingUnitType[] handlingUnit
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
