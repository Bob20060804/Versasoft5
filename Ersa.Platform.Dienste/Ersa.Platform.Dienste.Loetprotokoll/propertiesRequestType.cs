using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class propertiesRequestType
	{
		[XmlElement("unitProperties")]
		public unitPropertiesRequestType[] unitProperties
		{
			get;
			set;
		}

		[XmlElement("equipmentProperties")]
		public equipmentPropertiesRequestType[] equipmentProperties
		{
			get;
			set;
		}

		[XmlElement("orderProperties")]
		public orderPropertiesRequestType[] orderProperties
		{
			get;
			set;
		}

		[XmlElement("materialProperties")]
		public materialPropertiesRequestType[] materialProperties
		{
			get;
			set;
		}

		[XmlElement("operationProperties")]
		public operationPropertiesRequestType[] operationProperties
		{
			get;
			set;
		}
	}
}
