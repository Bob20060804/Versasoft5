using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class equipmentPropertiesType
	{
		[XmlElement("equipmentProperty")]
		public propertyType[] equipmentProperty
		{
			get;
			set;
		}
	}
}
