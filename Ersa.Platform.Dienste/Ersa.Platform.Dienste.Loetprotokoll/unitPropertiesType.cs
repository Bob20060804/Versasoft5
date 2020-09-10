using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class unitPropertiesType
	{
		[XmlElement("unitProperty")]
		public propertyType[] unitProperty
		{
			get;
			set;
		}
	}
}
