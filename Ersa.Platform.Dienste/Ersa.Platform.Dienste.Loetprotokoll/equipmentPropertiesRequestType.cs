using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class equipmentPropertiesRequestType
	{
		[XmlElement("equipmentProperty")]
		public propertyRequestType[] equipmentProperty
		{
			get;
			set;
		}
	}
}
