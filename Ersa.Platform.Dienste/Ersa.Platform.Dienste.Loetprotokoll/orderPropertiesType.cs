using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class orderPropertiesType
	{
		[XmlElement("orderProperty")]
		public propertyType[] orderProperty
		{
			get;
			set;
		}
	}
}
