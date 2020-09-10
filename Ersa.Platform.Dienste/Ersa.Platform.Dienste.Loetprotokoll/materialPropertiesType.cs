using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class materialPropertiesType
	{
		[XmlElement("materialProperty")]
		public propertyType[] materialProperty
		{
			get;
			set;
		}
	}
}
