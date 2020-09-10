using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class propertiesType
	{
		[XmlElement("unitProperties")]
		public unitPropertiesType[] unitProperties
		{
			get;
			set;
		}

		[XmlElement("equipmentProperties")]
		public equipmentPropertiesType[] equipmentProperties
		{
			get;
			set;
		}

		[XmlElement("orderProperties")]
		public orderPropertiesType[] orderProperties
		{
			get;
			set;
		}

		[XmlElement("materialProperties")]
		public materialPropertiesType[] materialProperties
		{
			get;
			set;
		}

		[XmlElement("operationProperties")]
		public operationPropertiesType[] operationProperties
		{
			get;
			set;
		}
	}
}
