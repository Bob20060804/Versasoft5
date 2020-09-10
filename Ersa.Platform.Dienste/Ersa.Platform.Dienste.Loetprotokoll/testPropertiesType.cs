using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class testPropertiesType
	{
		[XmlElement("testProperty")]
		public propertyType[] testProperty
		{
			get;
			set;
		}
	}
}
